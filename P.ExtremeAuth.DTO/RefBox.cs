using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.ExtremeAuth.DTO
{
    public class RefBox
    {
        public RefBox(TypeCode typeCode, object stateValue, object procedureValue)
        {
            if (stateValue == null || procedureValue == null)
                throw new Exception();//todo

            TypeCode = typeCode;
            AuthorizationStateValue = Convert.ChangeType(stateValue, TypeCode);
            ProcedureValue = Convert.ChangeType(procedureValue, TypeCode);
        }

        public TypeCode TypeCode { get; set; }

        public object AuthorizationStateValue { get; set; }//buna proc val uygulanir ona gore kosula bakilir
        //note primitive tiplerin degerleri method scope'unda degistirilse bile disarda ayni kalir cunku adresleri ile beraber degerleri de stack'tedir
        public object ProcedureValue { get; set; }
        //bu yuzden Proc'la beraber degisiklik yapmam gerektiginden bunu uydurdum
    }
}
