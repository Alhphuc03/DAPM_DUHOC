//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace quanlytrungtam.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DANGKYLOTRINH
    {
        public int MADK { get; set; }
        public Nullable<int> MAKH { get; set; }
        public Nullable<int> MALT { get; set; }
        public Nullable<System.DateTime> NGAYDK { get; set; }
        public string TRANGTHAI { get; set; }
    
        public virtual KHACHHANG KHACHHANG { get; set; }
        public virtual LOTRINHDUHOC LOTRINHDUHOC { get; set; }
    }
}
