//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PIK_UP_REST_API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LikeProduct
    {
        public int likeId { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public string DateTime { get; set; }
        public string Condition { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual UserTable UserTable { get; set; }
    }
}
