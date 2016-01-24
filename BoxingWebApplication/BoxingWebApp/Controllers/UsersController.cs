using Boxing.Contracts;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Extensions;
using BoxingWebApp.Extensions;
using BoxingWebApp.Services;
using BoxingWebApp.ViewModels;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BoxingWebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IWebClientService webClient;

        public UsersController(IWebClientService webClient)
        {
            this.webClient = webClient;
        }

        // GET: Users
        public ActionResult Index()
        {
            UsersListViewModel model = new UsersListViewModel();

            model.Items = webClient.ExecuteGet<IEnumerable<UserDto>>(new Models.ApiRequest() { EndPoint = "users?skip=0&take=10" })
                ?.Select(q => new UsersListItem() { Id = q.Id, FullName = q.FullName, Username = q.Username })?.ToList();

            return View(model);
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            var user = webClient.ExecuteGet<UserDto>(new Models.ApiRequest() { EndPoint = string.Format("users/{0}", id) });

            var model = new UsersDetailsViewModel();

            model.Id = user.Id;
            model.FullName = user.FullName;
            model.Username = user.Username;
            model.IsAdmin= user.IsAdmin;
            model.Rating= user.Rating;

            ViewBag.Title = model.Username;

            return View(model);
        }

        public ActionResult Register()
        {
            ViewBag.Title = "Register";

            return View();
        }

        [HttpPost]
        public ActionResult Register(UsersDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Session[Constants.Headers.AuthTokenHeader] = "registertoken";

                    var existingLogin = webClient.ExecutePost<object>(new Models.ApiRequest()
                    {
                        EndPoint = string.Format("users"),
                        Request = new UserDto()
                        {
                            FullName = model.FullName,
                            Username = model.Username,
                            Password = model.Password,
                            IsAdmin = false
                        }
                    }) as JObject;


                    if (existingLogin != null)
                    {
                        var parsedLogin = existingLogin.ToObject<LoginDto>();
                        Session[Constants.Headers.AuthTokenHeader] = parsedLogin.AuthToken;
                        Session["LoginId"] = parsedLogin.Id;
                        Session["Username"] = model.Username;
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch (HttpException ex)
                {
                    if (ex.GetHttpCode() != (int)HttpStatusCode.BadRequest)
                    {
                        throw ex;
                    }

                    ModelState.AddModelError("", "This username is already taken! Please choose another one.");
                    Session[Constants.Headers.AuthTokenHeader] = null;
                    return View();
                }
                catch
                {
                    Session[Constants.Headers.AuthTokenHeader] = null;
                    return View();
                }
            }

            return View();
        }
    }
}
