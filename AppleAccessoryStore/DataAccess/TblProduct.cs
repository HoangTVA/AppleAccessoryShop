﻿using System;
using System.Collections.Generic;

#nullable disable

namespace AppleAccessoryStore.DataAccess
{
    public partial class TblProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public int? UnitInStock { get; set; }
        public int? CommentId { get; set; }
        public string ProductImage { get; set; }
        public virtual TblComment Comment { get; set; }
    }
}
