using System.ComponentModel.DataAnnotations;

namespace P.ExtremeAuth.Entity.Enums
{
    public enum ConditionOperator
    {
        None,
        Equals,

        [Display(Name = "Not Equals")]
        NotEquals,

        [Display(Name = "Greater Than")]
        GreaterThan,

        [Display(Name = "Less Than")]
        LessThan,

        [Display(Name = "Greater Than Or Equal")]
        GreaterThanOrEqual,

        [Display(Name = "Less Than Or Equal")]
        LessThanOrEqual,

        Contains,

        [Display(Name = "Not Contain")]
        NotContain,

        [Display(Name = "Starts With")]
        StartsWith,

        [Display(Name = "Ends With")]
        EndsWith
    }
}
