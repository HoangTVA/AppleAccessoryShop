using System;
using System.Collections.Generic;

#nullable disable

namespace AppleAccessoryStore.DataAccess
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblComments = new HashSet<TblComment>();
            TblOrders = new HashSet<TblOrder>();
        }

        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string RoleId { get; set; }

        public virtual TblRole Role { get; set; }
        public virtual ICollection<TblComment> TblComments { get; set; }
        public virtual ICollection<TblOrder> TblOrders { get; set; }
    }
}
