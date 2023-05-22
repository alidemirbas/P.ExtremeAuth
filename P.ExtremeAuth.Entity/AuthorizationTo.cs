using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.ExtremeAuth.Entity
{
    public class AuthorizationTo 
    {
        public Guid Id { get; set; }
        public Guid RefId { get; set; }

        public Guid AuthorizationOfId { get; set; }
        public AuthorizationOf AuthorizationOf { get; set; }

        public string StateValue { get; set; }//kalan hak bu proc'larla guncellenir mesela 4 kaldi
                                              //tp icin ornek ise
                                              //defined 10 olsun condition < olsun
                                              //right nullable olsun  ve gelen her tp value ile tranbus'ta (tum onaylardan gecerse) guncellensin bu sayede hem en son ne deger islem gormus onu da biliriz                                              
    }
}
