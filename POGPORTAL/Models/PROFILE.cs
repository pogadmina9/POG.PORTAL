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
    
    public partial class PROFILE
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> RolesId { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public Nullable<System.DateTime> BeginActiveDate { get; set; }
        public Nullable<System.DateTime> EndActiveDate { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
    }
}
