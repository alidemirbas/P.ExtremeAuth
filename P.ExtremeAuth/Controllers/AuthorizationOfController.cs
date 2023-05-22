using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P.ExtremeAuth.DTO;
using P.ExtremeAuth.Entity;
using P.ExtremeAuth.Models;

namespace P.ExtremeAuth.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthorizationOfController : ControllerBase
    {
        private readonly Data.DbContext _db;

        public AuthorizationOfController(Data.DbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult Save(AuthorizationOf entity)
        {
            if (entity.Id == default)
            {
                _db.AuthorizationOf.Add(entity);
            }
            else
            {
                var existingEntity = _db.AuthorizationOf
                    .Single(x => x.Id == entity.Id);

                existingEntity.RefId = entity.RefId;
                existingEntity.AuthorizationId = entity.AuthorizationId;
            }

            _db.SaveChanges();

            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var existingEntity = _db.AuthorizationOf
                .Include(x=>x.AuthorizationTos)
                .Single(x => x.Id == id);

            _db.AuthorizationTo.RemoveRange(existingEntity.AuthorizationTos);
            _db.AuthorizationOf.Remove(existingEntity);

            return Ok(); 
        }

        [HttpGet("{refId}")]
        public ActionResult GetAuthOfs(Guid refId)
        {
            var entities = _db.AuthorizationOf
                .Include(x=>x.Authorization)
                .ThenInclude(x=>x.Procedures)
                .Include(x => x.AuthorizationTos)
                .AsNoTracking()
                .Where(x => x.RefId == refId)
                .ToArray();

            return Ok(entities);
        }
    }
}