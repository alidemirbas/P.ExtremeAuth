namespace P.ExtremeAuth.Processors.Enums
{
    public enum Runtime
    {
        BeforeAuthority,//islemleri uygula ve kontrol et kosullar saglandiysa izin ver
        AfterAuthority,//kosullar saglandiysa izni ver ve islemleri uygula
    }
}
