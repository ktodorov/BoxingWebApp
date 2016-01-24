using Boxing.Contracts.Dto;
using BoxingWebApp.Services;
using BoxingWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;
using System.Web.Http;

namespace BoxingWebApp.Controllers
{
    public class BoxersController : Controller
    {
        private readonly IWebClientService webClient;

        public BoxersController(IWebClientService webClient)
        {
            this.webClient = webClient;
        }

        // GET: Boxers
        public ActionResult Index([FromUri] int skip = 0, [FromUri] int take = 10)
        {
            BoxersListViewModel model = new BoxersListViewModel();

            model.Items = webClient.ExecuteGet<IEnumerable<BoxerDto>>(new Models.ApiRequest() { EndPoint = $"boxers?skip={skip}&take={take}" })
                ?.Select(q => new BoxersListItem() { Id = q.Id, Name = q.Name })?.ToList();

            //int pageSize = 1;
            //int pageNumber = (page ?? 1);

            ViewData["Page"] = (skip / take) + 1;
            ViewData["PageSize"] = take;
            return View(model);//.ToPagedList(pageNumber, pageSize));
        }

        // GET: Boxers/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Boxers/Create
        public ActionResult Create()
        {
            BoxersDetailsViewModel model = new BoxersDetailsViewModel();

            ViewBag.Title = "Create";

            return View(model);
        }

        // POST: Boxers/Create
        [System.Web.Mvc.HttpPost]
        public ActionResult Create(BoxersDetailsViewModel model)
        {
            try
            {
                object existingTest = webClient.ExecutePost<object>(new Models.ApiRequest()
                {
                    EndPoint = string.Format("boxers"),
                    Request = new BoxerDto()
                    {
                        Name = model.Name
                    }
                });

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Boxers/Edit/5
        public ActionResult Edit(int id)
        {
            var boxer = webClient.ExecuteGet<BoxerDto>(new Models.ApiRequest() { EndPoint = string.Format("boxers/{0}", id) });

            BoxersDetailsViewModel model = new BoxersDetailsViewModel();

            model.Name = boxer.Name;

            ViewBag.Title = "Edit";

            return View("Create", model);
        }

        // POST: Boxers/Edit/5
        [System.Web.Mvc.HttpPost]
        public ActionResult Edit(int id, BoxersDetailsViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var response = webClient.ExecutePut<object>(new Models.ApiRequest()
                        {
                            EndPoint = string.Format("boxers/{0}", model.Id),
                            Request = new BoxerDto()
                            {
                                Name = model.Name
                            }
                        });

                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("generalError", e.Message);

                        return View(model);
                    }
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Boxers/Delete/5
        public ActionResult Delete(int id)
        {
            webClient.ExecuteDelete(new Models.ApiRequest() { EndPoint = string.Format("boxers/{0}", id) });

            return RedirectToAction("Index");
        }
    }
}
