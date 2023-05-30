using System;

namespace P.ExtremeAuth.Entity
{
    public class Procedure 
    {
        public Guid Id { get; set; }//todo bu propu yok et fluent mapping'te cokacok yap

        //Authorization.StateValue'ye uygulanacak deger //client'tan gelen degere de Authorization.Value x Condition uygulanir
        public string Value { get; set; }
        public int Runtime { get; set; }//bu burda cunku Authorization.Procedures lerden bazilari once bazilari sonra olabilsin diye
        public int Priority { get; set; }
        //typecode'a gerek yok auth'daki ref

        //senaryo
        //su-su tarih arasi 5 hak
        //runtime before olur
        //start date proc'u tanimlidir esitle islemi Startdate=Datetime.now olur guncellenir
        //end date 'e bir proc tanimi gerekmez. bir proc'dan gecmeden devam edilir
        //count proc'u ise cikar islemine tabi tutulur 5-procVal(1) guncellenir
        //yetkiler saglaniyor mu kontrol edilir

        public Guid ProcedureDefinitionId { get; set; }
        public virtual ProcedureDefinition ProcedureDefinition { get; set; }

        public Guid AuthorizationId { get; set; }
        public virtual Authorization Authorization { get; set; }
    }
}
