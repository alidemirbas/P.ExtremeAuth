using System;

namespace P.ExtremeAuth.Entity
{
    public class ProcedureDefinition //todo constraint ayni isimde birden fazla islem olmamali (gerci zaten reflection ile kontrol edildiginden olmasi mumkun degil) (bu tablonun cms'te bir karsiligi olamayacak)
    {
        public Guid Id { get; set; }
        public string Type { get; set; }//kodda instance alinirken hangi islem sinifi oldugnu interface'e atamak icin. maksat genericlestirmek
                                             //proje her ayaga kaldirildiginda  Reflection ile kontrol edilip silinen class'lar bu tablodan ve Islem tablosundan silinsin , yeni islemtanimi class'lari tespit edilsin ve insert edilsin

        public string Name { get; set; }
        public string Description { get; set; }

        //public List<Procedure> Islemler { get; set; }//sadece nav prop dusunme. (hic dusunme:neden bir IslemTaniminin birden cok islemi olabilir diye) //belki lazim olur diye konuldu
        //vazgectim
    }
}
