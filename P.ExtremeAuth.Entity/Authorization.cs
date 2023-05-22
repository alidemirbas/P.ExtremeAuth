using System;
using System.Collections.Generic;

namespace P.ExtremeAuth.Entity
{
    //bu entity bir sayfada admine bir authorization grubunun tum hepsinin birden neyi yapmak istedigini bildirir.
    //Kampanya gibi dusunursek mesela bayram kampanyasi altinda auth'lar var diyelim, bilmem ne kadar alisveris ve x kadar takside su olsun gibi



    //rule
    //AuthorizationOf'u var fakat AuthorizationTos 0'sa yetki var ama daha kimseye atama yapilmamais demek. kimse hicbirsey yapamaz yetki yok hata firlat demek amaaa
    //UniqueIdTo null ve de StataValue null ise bu herseye aciktir demek ornegin su otomati gibi fakat
    //UniqueIdTo null degil StateValue null ise x entity'si y entity'si uzerinde bir yetkiye sahiptir fakat bir kosul yoktur demek. yetkinin var olmus olmasi yeterli. (orn  Open yetkisi. bu transaction bu client'a acik mi? evet,auth var,cond none... ve bir proc uygulanmasina gerek yok demek) (yukarda auth'da cond null olmasi sart)
    //UniqueIdTo null ve StateValue degilse o zaman bu yine public bir yetkidir ama bir kosulu saglamasi gerektirir. orn 100'tl uzeri kargo bedava


    public class Authorization
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public TypeCode TypeCode { get; set; }//exp: int

        //rule eger condition none ise bu demektir ki yetkinin var olmus olmasi yeterli. (orn  Open yetkisi. bu transaction bu client'a acik mi? evet,auth var,cond none... ve bir proc uygulanmasina gerek yok demek)
        //oku AuthorizationAssignment
        public int ConditionOperator { get; set; }//kosul sorgulanmasi proc'a bagli olarak hem once hem sonra olabilir
        public string ConditionValue { get; set; }//bu degismez mesela 5 giris hakki

        public bool Mock { get; set; }//EZ kampanya icin dusun category ve ayrica product kampanyasi var. ama category kamp uygulandiktan sonra bir de product kamp uygulansin istemiyorsun. o zaman cat kamp mock true olacak

        public virtual ICollection<Procedure> Procedures { get; set; }//birden fazla isleme tabi olabilir mesela 2 ile carp 3'e bol

        //rule AuthOf un içinde göreceksin bir tane Auth'u olabilir ve o To'lara uygulanır
        //bu prop ise bu auth'un neler/kimler tarafından tüketildiğini görmek için saxece. to'luk bir durum yok
        public virtual ICollection<AuthorizationOf> AuthorizationOfs { get; set; }
    }
}
