using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P.ExtremeAuth.DTO;
using P.ExtremeAuth.Entity;
using P.ExtremeAuth.Models;

namespace P.ExtremeAuth.Controllers
{
    //note parametrelerin boyle olma sebebi
    //https://stackoverflow.com/questions/24874490/pass-multiple-complex-objects-to-a-post-put-web-api-method/43547269


    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly Data.DbContext _db;
        private readonly AuthProcCache _authProcCache;

        public AuthorizationController(Data.DbContext db, AuthProcCache authProcCache)
        {
            _db = db;
            _authProcCache = authProcCache;
        }

        [HttpPost]
        public IActionResult CheckAuthorization(AuthorizationCheck authCheck)
        {
            var authorizationToIds = authCheck.AuthorizationTos.Select(x => x.Id);
            var authOfs = _db.AuthorizationOf
                .Include(x => x.AuthorizationTos)
                .Include(x => x.Authorization)
                .ThenInclude(x => x.Procedures)
                .AsNoTracking()
                .Where(x => x.RefId == authCheck.AuthOfId && x.AuthorizationTos.Any(y => authorizationToIds.Contains(y.RefId)))
                .ToArray();

            foreach (var authOf in authOfs)
            {
                foreach (var proc in authOf.Authorization.Procedures.OrderBy(x => x.Priority))
                {
                    var authProc = _authProcCache.Processors.Single(x => x.Definition.Id == proc.ProcedureDefinitionId);

                    foreach (var authTo in authOf.AuthorizationTos)
                    {
                        var refBox = new RefBox(authOf.Authorization.TypeCode, authTo.StateValue, proc.Value);

                        authProc.Execute(refBox);

                        authTo.StateValue = refBox.AuthorizationStateValue.ToString();//interger'in datetime'in hep override tostring'leri senin isini gorececk merak etme

                        _db.AuthorizationTo.Update(authTo);
                    }
                }
            }

            _db.SaveChanges();

            //todo ya buraya kadar gelip de tran karsi tarafa ulastirilamazsa hak yenmis olur
            return Ok();
        }

        [HttpPost]
        public IActionResult Save(Authorization entity)
        {
            if (entity.Id != default)
            {
                _db.Authorization.Add(entity);
            }
            else
            {
                var existingEntity = _db.Authorization
                    .Single(x => entity.Id == entity.Id);
                
                existingEntity.ConditionValue = entity.ConditionValue;
                existingEntity.Mock= entity.Mock;
                existingEntity.ConditionOperator = entity.ConditionOperator;
                existingEntity.ConditionValue= entity.ConditionValue;
                existingEntity.Description = entity.Description;
                existingEntity.Name = entity.Name;
                existingEntity.TypeCode = entity.TypeCode;
            }

            _db.SaveChanges();

            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var existingEntity = _db.Authorization
                .Include(x => x.Procedures)
                .Single(x => x.Id == id);

            _db.Procedure.RemoveRange(existingEntity.Procedures);
            _db.Authorization.Remove(existingEntity);

            _db.SaveChanges();

            return Ok();
        }

        [HttpGet("{authOfId}")]
        public IActionResult GetAll(Guid authOfId)
        {
            var entities = _db.AuthorizationOf
                .Include(x=>x.Authorization)
                .ThenInclude(x => x.Procedures)
                .AsNoTracking()
                .Where(x => x.RefId == authOfId)
                .Select(x=>x.Authorization)
                .ToArray();

            return Ok(entities);
        }
    }
}

