using System;
using System.Collections.Generic;

#nullable disable

namespace AppleAccessoryStore.DataAccess
{
    public partial class TblComment
    {
        public TblComment()
        {
            TblProducts = new HashSet<TblProduct>();
        }

        public int CommentId { get; set; }
        public int? UserId { get; set; }
        public string Description { get; set; }

        public virtual TblUser User { get; set; }
        public virtual ICollection<TblProduct> TblProducts { get; set; }
    }
}
