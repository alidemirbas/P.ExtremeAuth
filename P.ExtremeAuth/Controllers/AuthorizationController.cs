using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P.ExtremeAuth.Processors;
using P.ExtremeAuth.Entity;
using P.ExtremeAuth.Models;
using System.Diagnostics;
using P.ExtremeAuth.Entity.Enums;
using P.ExtremeAuth.Processors.Enums;

namespace P.ExtremeAuth.Controllers
{
    //note parametrelerin boyle olma sebebi
    //https://stackoverflow.com/questions/24874490/pass-multiple-complex-objects-to-a-post-put-web-api-method/43547269


    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly Data.DbContext _db;
        private readonly ProcessorCache _authProcCache;

        public AuthorizationController(Data.DbContext db, ProcessorCache authProcCache)
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
                .OrderBy(x => x.Priority)
                .ToArray();

            foreach (var authOf in authOfs)
            {
                foreach (var authTo in authOf.AuthorizationTos)
                {
                    if (!Process(authOf, authTo))
                        throw new Exception("No Auth");
                }
            }

            _db.SaveChanges();

            //todo ya buraya kadar gelip de tran karsi tarafa ulastirilamazsa hak yenmis olur
            return Ok();
        }


        private bool Process(Entity.AuthorizationOf authorizationOf, Entity.AuthorizationTo authorizationTo)
        {
            //senaryo
            //su-su tarih arasi 5 hak
            //runtime before olur
            //start date proc'u tanimlidir esitle islemi Startdate=Datetime.now olur guncellenir
            //end date 'e bir proc tanimi gerekmez. bir proc'dan gecmeden devam edilir
            //count proc'u ise cikar islemine tabi tutulur 5-procVal(1) guncellenir
            //yetkiler saglaniyor mu kontrol edilir

            //senaryo
            //drone before 10 mt yukselike ciksin ciktimi ok
            //after tanimlanmis olan one dogru gitmeye hakki 0 olan dronun after proc ile 100 mt
            //yol hakki taninmis olsun

            //senaryo
            //kampanya
            //100 tl'lik alisverise 10 tl indirim ceki
            //before 100 tl alisveris auth oric vs.. ok mi?
            //after auth'da sifir olarak tanimlanmis indirim ceki proc ile 10 arttirlsin
            //ve cekin baslangic tarihi hemen 100tl lik av nin ardi olsun
            //artik sonraki alisveris te bu hak kullanilsin
            //opsiyonel olmasi icin bir sonraki av icin arti bool bir auth daha olsun ki
            //bu 10 tl indirim cekini istedigi av de kullanisin



            //AuthorizationAssignment enitity'sindeki notlari oku
            if (authorizationTo.StateValue == null)
            {
                if (authorizationOf.Authorization.ConditionOperator != (int)ConditionOperator.None || authorizationOf.Authorization.Procedures.Count() > 0)
                    throw new Exception("FaultyRecord");//buraya hic dusmemeli a ve aa'daki notlari oku
                return true;
            }

            //bir proc yoksa zaten direk es gececek
            var procs = authorizationOf.Authorization.Procedures.Where(x => x.Runtime == (int)Runtime.BeforeAuthority);//BEFORE
            
            foreach (var proc in procs)//mesela 2 ile carp 3'e bol
            {
                var authProc = _authProcCache.Processors.Single(x => x.Definition.Id == proc.ProcedureDefinitionId);

                authProc.Execute(new RefBox(authorizationOf.Authorization.TypeCode, authorizationTo.StateValue, proc.Value));
            }

            var ok = Condition.Compile(authorizationTo.StateValue, authorizationOf.Authorization.ConditionValue, authorizationOf.Authorization.TypeCode, authorizationOf.Authorization.ConditionOperator);

            procs = authorizationOf.Authorization.Procedures.Where(x => x.Runtime == (int)Runtime.AfterAuthority);//AFTER
            
            foreach (var proc in procs)//mesela 2 ile carp 3'e bol
            {
                var authProc = _authProcCache.Processors.Single(x => x.Definition.Id == proc.ProcedureDefinitionId);

                authProc.Execute(new RefBox(authorizationOf.Authorization.TypeCode, authorizationTo.StateValue, proc.Value));
            }

            //bir daha ok bakilmaz. transaction'dan sonra deger degistirlsin istenmistir

            //authorization'un guncellenmesi ancak transactionbus'ta tum sartlar gerceklesirse yapilacak cunku gerceklesmezse bosa hak yemis olursun. mesela 5kezyap auth'u -1 olacak bosuna.. condition'u saglayan prmler le tekrar gelir isini halleder bosa hak yenmez ooohhhh cumleyi ne uzattim misss

            return ok;
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
                existingEntity.Mock = entity.Mock;
                existingEntity.ConditionOperator = entity.ConditionOperator;
                existingEntity.ConditionValue = entity.ConditionValue;
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
                .Include(x => x.Authorization)
                .ThenInclude(x => x.Procedures)
                .AsNoTracking()
                .Where(x => x.RefId == authOfId)
                .Select(x => x.Authorization)
                .ToArray();

            return Ok(entities);
        }
    }
}

