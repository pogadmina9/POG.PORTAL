using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POGPORTAL.Repositories
{
    public class MenuRepository
    {
        public long Id { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ModuleNameParent { get; set; }
        public string ModuleName { get; set; }
        public string MenuCode { get; set; }
        public string Path { get; set; }
        public int? OrderdNumber { get; set; }
        public string InitialModuleName { get; set; }
    }


    public class NavMenuRepository
    {
        public string GroupCode { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ModuleNameParent { get; set; }
        public string MenuCode { get; set; }
        public string ModuleNameOrderNumber { get; set; }

    }


    public class ContentMenuRepository
    {
        public string ModuleNameParent { get; set; }
        public string ModuleName { get; set; }
        public string MenuCode { get; set; }
        public string Path { get; set; }
        public string IconPath { get; set; }
        public string IsHide { get; set; }
        public DateTime BeginActiveDate { get; set; }
        public DateTime EndActiveDate { get; set; }
        public int? OrderdNumber { get; set; }
        public string ModuleNameOrderNumber { get; set; }
    }


    public class ProcessLogin
    {
        public bool IsLogon { get; set; }
        public string SaytoUser { get; set; }
        public string errMsg { get; set; }
    }
}