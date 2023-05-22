using P.ExtremeAuth.Entity;
using System;

namespace P.ExtremeAuth.DTO
{
    public interface IAuthProcessor
    {
        ProcedureDefinition Definition { get; }

        //yetki value ile islem value arasindaki islemi yap
        //void Execute(ref object mVal, object pVal);//olamayacagi icin RefBox var
        void Execute(RefBox refBox);
        //RefBox iki degerden fazla olmaz cunku Procedure Entity 1 val icerir
    }
}
