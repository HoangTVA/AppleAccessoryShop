using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject
{
    public partial class TblOrder
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? Total { get; set; }

        public virtual TblUser User { get; set; }
    }
}
