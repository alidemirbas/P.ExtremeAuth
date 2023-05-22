using P.ExtremeAuth.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.ExtremeAuth.Processors
{
    public class AdditionProcessor : IProcessor
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

            switch (refBox.TypeCode)
            {
                case TypeCode.Empty:
                    break;
                case TypeCode.Boolean:
                    break;
                case TypeCode.Char:
                    break;
                case TypeCode.Byte:
                    break;
                case TypeCode.Int32:
                    refBox.StateValue = (int)refBox.StateValue + (int)refBox.ProcedureValue;
                    break;
                case TypeCode.Int64:
                    break;
                case TypeCode.Decimal:
                    refBox.StateValue = (decimal)refBox.StateValue + (decimal)refBox.ProcedureValue;
                    break;
                case TypeCode.DateTime:
                    refBox.StateValue = new DateTime((((DateTime)refBox.StateValue).Ticks + ((DateTime)refBox.ProcedureValue).Ticks));
                    break;
                case TypeCode.Double:
                    break;
                case TypeCode.String:
                    refBox.StateValue = string.Concat(refBox.StateValue, refBox.ProcedureValue);
                    break;
                default:
                    break;
            }
        }
    }
}
