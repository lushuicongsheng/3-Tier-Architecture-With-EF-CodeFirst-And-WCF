using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyEF.Contract;
using EasyEF.WCFClientProxy;
using EasyEF.Models;
using EasyEF.Common;
using EasyEF.Utility;
using EasyEF.WCFExtension;

namespace EasyEF.Web.Controllers
{
    public class HomeController : Controller
    {
        public IServiceFactory ServiceFactory
        {
            get
            {
                //Need to inject dynamic later
                return new RemoteServiceFactory();
            }
        }
        
        public IService Service
        {
            get
            {
                return this.ServiceFactory.CreateService(); 
            }
        }

        public int PageSize
        {
            get
            {
                return 2;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            WCFContext.Current.Operater = new Operater()
            {
                Name = "guozili",
                Time = DateTime.Now,
                IP = Fetch.UserIp,
                LoginToken = Guid.NewGuid(),
                Method = filterContext.ActionDescriptor.ActionName
            };
        }
        
        //
        // GET: /Home/

        public ActionResult Index(int id = 1)
        {
            var products = this.Service.GetProducts(PageSize, id);
            return View(products);
        }

        public ActionResult Create()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionName("Create")]
        public ActionResult DoCreate(Product product)
        {
            if (ModelState.IsValid)
            {
                this.Service.CreateProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult IndexTest(int id = 1)
        {
            WCFClientProxy.HomeService.PagedListOfProduct6Lx8ofhX products = HomeServiceClass.GetProducts(2, 1, 0);
            return View(products);
        }
    }
}
