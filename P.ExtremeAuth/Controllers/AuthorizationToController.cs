using Microsoft.AspNetCore.Mvc;
using P.ExtremeAuth.Data;
using P.ExtremeAuth.Entity;

namespace P.ExtremeAuth.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthorizationToController : ControllerBase
    {
        private readonly Data.DbContext _db;

        public AuthorizationToController(DbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult Save(AuthorizationTo entity)
        {
            if (entity.Id == default)
            {
                _db.AuthorizationTo.Add(entity);
            }
            else
            {
                var existingEntity = _db.AuthorizationTo
                    .Single(x => x.Id == entity.Id);

                existingEntity.RefId= entity.RefId;
                existingEntity.AuthorizationOfId = entity.AuthorizationOfId;
                existingEntity.StateValue = entity.StateValue;
            }

            _db.SaveChanges();

            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var existingEntity = _db.AuthorizationTo
                    .Single(x => x.Id == id);
            
            _db.Remove(existingEntity);

            _db.SaveChanges();

            return Ok();
        }

    }
}