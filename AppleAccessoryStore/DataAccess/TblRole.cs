using System;
using System.Collections.Generic;

#nullable disable

namespace AppleAccessoryStore.DataAccess
{
    public partial class TblRole
    {
        public TblRole()
        {
            TblUsers = new HashSet<TblUser>();
        }

        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<TblUser> TblUsers { get; set; }
    }
}
