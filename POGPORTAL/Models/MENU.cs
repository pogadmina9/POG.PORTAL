//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace POGPORTAL.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MENU
    {
        public long Id { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public string ModuleNameParent { get; set; }
        public string ModuleName { get; set; }
        public string MenuCode { get; set; }
        public string ModuleDescription { get; set; }
        public string Path { get; set; }
        public string IconPath { get; set; }
        public Nullable<bool> IsHide { get; set; }
        public Nullable<bool> IsMobile { get; set; }
        public Nullable<int> OrderedNumber { get; set; }
        public Nullable<System.DateTime> BeginActiveDate { get; set; }
        public Nullable<System.DateTime> EndActiveDate { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
    }
}
