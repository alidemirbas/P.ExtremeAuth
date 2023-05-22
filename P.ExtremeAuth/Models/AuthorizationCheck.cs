namespace P.ExtremeAuth.Models
{
    public class AuthorizationCheck
    {
        public Guid AuthOfId { get; set; }
        public AuthorizationTo[] AuthorizationTos { get; set; }
    }
}
