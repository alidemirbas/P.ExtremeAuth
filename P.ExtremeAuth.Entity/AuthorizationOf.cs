using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.ExtremeAuth.Entity
{
    public class AuthorizationOf 
    {
        public Guid Id { get; set; }
        public Guid RefId { get; set; }

        public Guid AuthorizationId { get; set; }
        public virtual Authorization Authorization { get; set; }

        public virtual ICollection<AuthorizationTo> AuthorizationTos { get; set; }
    }
}
