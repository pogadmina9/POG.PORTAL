using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using POGPORTAL.Models;
using POGPORTAL.Repositories;

namespace POGPORTAL.Controllers
{
    public class HomeController : Controller
    {
        private DbContextNew db;
        private string sql;

        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public async Task<ActionResult> Login()
        {
            List<MenuRepository> ShowComp = new List<MenuRepository>();

            using (DbContextNew ctx = new DbContextNew())
            {
                ShowComp.Add(new MenuRepository { CompanyName = "-", CompanyCode = null, OrderdNumber = 0 });

                int i = 0;
                foreach (var comp in (await ctx.COMPANies.ToListAsync())) {

                    ShowComp.Add(new MenuRepository
                    {
                        CompanyName = comp.CompanyName.Trim(),
                        CompanyCode = comp.CompanyCode.Trim(),
                        OrderdNumber = i++,
                    });
                }
            }

            ViewBag.Comp = new SelectList(ShowComp.OrderBy(s=>s.OrderdNumber).ToList(), "CompanyCode", "CompanyName");

            return PartialView();
        }


        public JsonResult ProcessLogin(string username, string password, string companycode)
        {
            ProcessLogin pl = new ProcessLogin();
            JsonResult js;
            try
            {
                using (DbContextNew ctx = new DbContextNew())
                {
                    var Id = ctx.COMPANies.Where(w=>w.CompanyCode.Equals(companycode)).Select(s=>s.Id).FirstOrDefault();
                    var Menus = ctx.MENUs.Where(w => Id.Equals((long)w.CompanyId)).Count();
                    var profile = ctx.PROFILEs.Where(w =>
                    w.Username.Equals(username) 
                    && w.Password.Equals(password)
                    && Id.Equals((long)w.CompanyId)
                    ).FirstOrDefault();

                    pl.IsLogon = Menus > 0 && profile != null ? true : false;
                    pl.SaytoUser = Menus > 0 && profile != null ? profile.FirstName : "";
                    pl.errMsg = Menus > 0 ? profile != null ? "" : "User Not Found" : "Company Not Found";
                }
            }
            catch (Exception ex)
            {
                pl.errMsg = Convert.ToString(ex.Message);
            }

            js = Json(new { pl.errMsg, pl }, JsonRequestBehavior.AllowGet);
            js.MaxJsonLength = Int32.MaxValue;

            return js;
        }



        public async Task<List<MenuRepository>> NavContentMenu()
        {
            List<MenuRepository> ShowMenu = new List<MenuRepository>();
            List<MenuRepository> MenuActDate;
            try
            {
                using (DbContextNew ctx = new DbContextNew())
                {
                    var Id = await ctx.COMPANies.Select(s=>s.Id).ToListAsync();
                    var Comp = await ctx.COMPANies.ToListAsync();
                    var Menus = await ctx.MENUs.Where(w => Id.Contains((long)w.CompanyId)).ToListAsync();

                    int i = 0;
                    foreach (var menu in Menus)
                    {
                        ShowMenu.Add(new MenuRepository
                        {
                            Id = menu.Id,
                            GroupCode = Comp.Where(w=>w.Id == menu.CompanyId)?.Select(s=>s.GroupCode).FirstOrDefault().Trim(),
                            CompanyCode = Comp.Where(w => w.Id == menu.CompanyId)?.Select(s => s.CompanyCode).FirstOrDefault().Trim(),
                            CompanyName = Comp.Where(w => w.Id == menu.CompanyId)?.Select(s => s.CompanyName).FirstOrDefault().Trim(),
                            GroupName = Comp.Where(w=> w.Id == menu.CompanyId)?.Select(s => s.GroupName).FirstOrDefault().Trim(),
                            MenuCode = menu.MenuCode?.Trim(),
                            ModuleNameParent = menu.ModuleNameParent?.Trim(),
                            ModuleName = menu.ModuleName?.Trim(),
                            ModuleDescription = menu.ModuleDescription?.Trim(),
                            Path = menu.Path?.Trim(),
                            OrderdNumber = menu.OrderedNumber ?? 0,
                            InitialModuleName = Convert.ToString("InitialModuleNameParent"),
                            BeginActiveDate = menu.BeginActiveDate,
                            EndActiveDate = menu.EndActiveDate,
                            IsHide = menu.IsHide
                        });
                        i++;
                    }

                    MenuActDate = ShowMenu.Where(w => w.BeginActiveDate <= DateTime.Now.Date && w.EndActiveDate >= DateTime.Now.Date && w.IsHide == false).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return MenuActDate;
        }

        /**
        public async Task<List<MenuRepository>> ContentMenu()
        {
            List<MenuRepository> ShowCtnMenu = new List<MenuRepository>();

            try
            {
                using (DbContextNew ctx = new DbContextNew())
                {
                    var Id = await ctx.COMPANies.Select(s => s.Id).ToListAsync();
                    var Comp = await ctx.COMPANies.ToListAsync();
                    var Menus = await ctx.MENUs.Where(w => Id.Contains((long)w.CompanyId)).ToListAsync();

                    int i = 0;
                    foreach (var menu in Menus)
                    {
                        ShowCtnMenu.Add(new MenuRepository
                        {
                            MenuCode = menu.MenuCode?.Trim(),
                            ModuleNameParent = menu.ModuleNameParent?.Trim(),
                            ModuleName = menu.ModuleName?.Trim(),
                            Path = menu.Path?.Trim(),
                            OrderdNumber = menu.OrderedNumber ?? 0,
                            InitialModuleName = Convert.ToString("InitialModuleNameParent")
                        });
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ShowCtnMenu;
        }
        **/


        public void DefinitionField(List<MenuRepository> ShowMenu)
        {
            ViewBag.CompanyName = ShowMenu.Select(s => s.CompanyName).Distinct();
            ViewBag.GroupName = ShowMenu.Select(s => s.GroupName).Distinct();
            ViewBag.filelogocompany = Convert.ToString("logo-"+ ShowMenu.Select(s => s.CompanyCode).Distinct().FirstOrDefault().ToLower() + ".png");
        }

        public async Task<ActionResult> Dashboard()
        {
            var ShowMenu = await NavContentMenu();
            Session["NavContentMenu"] = ShowMenu;
            DefinitionField(ShowMenu);

            return View(ShowMenu);
        }


        public async Task<ActionResult> InitialModuleNameParent1()
        {
            List<MenuRepository> ShowMenu = new List<MenuRepository>();
            if (Session.Count == 0) return RedirectToAction("Login");
            if (Session["NavContentMenu"] == null) ShowMenu = await NavContentMenu();
            else ShowMenu.AddRange((List<MenuRepository>)Session["NavContentMenu"]);
            ViewBag.ModuleNameParent1 = ShowMenu.Select(s => s.ModuleNameParent ??  string.Empty).Distinct().ToArray()[0];
            DefinitionField(ShowMenu);

            return View(ShowMenu);
        }

        public async Task<ActionResult> InitialModuleNameParent2()
        {
            List<MenuRepository> ShowMenu = new List<MenuRepository>();
            if (Session.Count == 0) return RedirectToAction("Login");
            if (Session["NavContentMenu"] == null) ShowMenu = await NavContentMenu();
            else ShowMenu.AddRange((List<MenuRepository>)Session["NavContentMenu"]);
            ViewBag.ModuleNameParent2 = ShowMenu.Select(s => s.ModuleNameParent ?? string.Empty).Distinct().ToArray()[1];
            DefinitionField(ShowMenu);

            return View(ShowMenu);
        }
        public async Task<ActionResult> InitialModuleNameParent3()
        {
            List<MenuRepository> ShowMenu = new List<MenuRepository>();
            if (Session.Count == 0) return RedirectToAction("Login");
            if (Session["NavContentMenu"] == null) ShowMenu = await NavContentMenu();
            else ShowMenu.AddRange((List<MenuRepository>)Session["NavContentMenu"]);
            ViewBag.ModuleNameParent3 = ShowMenu.Select(s => s.ModuleNameParent ?? string.Empty).Distinct().ToArray()[2];
            DefinitionField(ShowMenu);

            return View(ShowMenu);
        }

        public async Task<ActionResult> InitialModuleNameParent4()
        {
            List<MenuRepository> ShowMenu = new List<MenuRepository>();
            if (Session.Count == 0) return RedirectToAction("Login");
            if (Session["NavContentMenu"] == null) ShowMenu = await NavContentMenu();
            else ShowMenu.AddRange((List<MenuRepository>)Session["NavContentMenu"]);
            ViewBag.ModuleNameParent4 = ShowMenu.Select(s => s.ModuleNameParent ?? string.Empty).Distinct().ToArray()[3];
            DefinitionField(ShowMenu);

            return View(ShowMenu);
        }

    }
}