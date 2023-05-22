using P.ExtremeAuth.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.ExtremeAuth.Processors
{
    public class EqualityProcessor : IProcessor
    {
        private ProcedureDefinition _definition;
        public ProcedureDefinition Definition
        {
            get
            {
                if (_definition != null)
                    return _definition;
                _definition = new ProcedureDefinition
                {
                    Type = this.GetType().Name,
                    Name = this.GetType().Name,
                    Description = ""
                };
                return _definition;
            }
        }

        public void Execute(RefBox refBox)
        {
            if (refBox.StateValue == null || refBox.ProcedureValue == null)
                throw new System.Exception();//todo

            refBox.StateValue = refBox.ProcedureValue;
        }
    }

}
