using Boxing.Contracts;
using Boxing.Contracts.Dto;
using Boxing.Contracts.Extensions;
using Boxing.Contracts.Helpers.Users;
using BoxingWebApp.Extensions;
using BoxingWebApp.Services;
using BoxingWebApp.ViewModels;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
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
        public ActionResult Index([FromUri] int skip = 0, [FromUri] int take = 10, [FromUri] string sort = "fullName", [FromUri] string order = "asc")
        {
            var sorting = UserSortingOptions.FullNameAscending;

            if (sort == "fullName")
            {
                if (order == "asc")
                {
                    sorting = UserSortingOptions.FullNameAscending;
                }
                else
                {
                    sorting = UserSortingOptions.FullNameDescending;
                }
            }
            else
            {
                if (order == "asc")
                {
                    sorting = UserSortingOptions.RatingAscending;
                }
                else
                {
                    sorting = UserSortingOptions.RatingDescending;
                }
            }

            UsersListViewModel model = new UsersListViewModel();

            model.Items = webClient.ExecuteGet<IEnumerable<UserDto>>(new Models.ApiRequest() { EndPoint = $"users?skip={skip}&take={take}&sort={sorting}" })
                ?.Select(q => new UsersListItem() { Id = q.Id, FullName = q.FullName, Username = q.Username, Rating = q.Rating })?.ToList();

            ViewData["Page"] = (skip / take) + 1;
            ViewData["PageSize"] = take;
            ViewData["Sort"] = sort;
            ViewData["Order"] = order;
            return View(model);
        }

        // GET: Users/Details/5
        public ActionResult Details([FromUri] int id)
        {
            var user = webClient.ExecuteGet<UserDto>(new Models.ApiRequest() { EndPoint = string.Format("users/{0}", id) });

            var model = new UsersDetailsViewModel();

            if (user != null)
            {
                model.Id = user.Id;
                model.FullName = user.FullName;
                model.Username = user.Username;
                model.IsAdmin = user.IsAdmin;
                model.Rating = user.Rating;

                ViewBag.Title = model.Username;
            }

            return View(model);
        }

        public ActionResult Register()
        {
            ViewBag.Title = "Register";

            return View();
        }

        [System.Web.Mvc.HttpPost]
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
