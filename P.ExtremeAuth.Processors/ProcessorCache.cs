using P.ExtremeAuth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.ExtremeAuth.Processors
{
    public class ProcessorCache
    {
        public ProcessorCache(DbContext db)
        {
            _db = db;
        }

        private static bool _seed;

        private readonly DbContext _db;
        
        public IEnumerable<IProcessor> Processors { get; internal set; }

        public void Seed()
        {
            if (_seed)
                return;

            //buradaki islemler startup'ta yapilma sebebi 
            //cok fazla proc olabileceginden program daha transaction'lari almaya baslamadan once yapilmali

            var procInterface = typeof(IProcessor);
            var procAssembly = procInterface.Assembly;
            var procCache = new List<IProcessor>();

            procAssembly.DefinedTypes
                .Where(x => procInterface.IsAssignableFrom(x) && !x.IsInterface/*iproc'un kendisini alma*/)
                .ToList()
                .ForEach(pdeType =>
                {
                    procCache.Add((IProcessor)Activator.CreateInstance(pdeType));
                });

            var procDefEntityList = _db.ProcedureDefinition.ToList();
            var delProcDefEntityList = procDefEntityList.Where(x => !procCache.Any(y => y.Definition.Type == x.Type)).ToList();//artik assembly'de olmayan proc def'lari ve dolayisiyla girilmis olan proc'lari silmek gerek islem yapilamayacagi icin

            var delIds = delProcDefEntityList.Select(x => x.Id);
            var delProcEntityList = _db.Procedure.Where(x => delIds.Contains(x.ProcedureDefinitionId)).ToList();//ama ilk iliskili proc'lar silinmeli

            foreach (var ent in delProcEntityList)
                _db.Procedure.Remove(ent);

            foreach (var ent in delProcDefEntityList)
            {
                procDefEntityList.Remove(ent);
                _db.ProcedureDefinition.Remove(ent);
            }

            //attn authorization tablosu etkilenmez cunku eger auth'un listesinde hicbir proc yoksa bu auth'a bir islem uygulanmayacagi kurali/anlami tasir

            //temizlik bitti simdi yenileri ekle

            var newProcDefs = procCache.Where(x => !procDefEntityList.Any(y => y.Type == x.Definition.Type)).Select(x => x.Definition);
            
            foreach (var newProcDef in newProcDefs)
                _db.ProcedureDefinition.Add(newProcDef);

            _db.SaveChanges();

            //portaldan artik yeni islemler olusturulabilir

            //Id'leri set ettin
            procDefEntityList = _db.ProcedureDefinition.ToList();
            procCache.ForEach(x => x.Definition.Id = procDefEntityList.Single(y => y.Type == x.Definition.Type).Id);

            Processors = procCache;

            _seed = true;
        }
    }
}
