using Boxing.Contracts;
using Boxing.Contracts.Dto;
using BoxingWebApp.Extensions;
using BoxingWebApp.Services;
using BoxingWebApp.ViewModels;
using Newtonsoft.Json.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace BoxingWebApp.Controllers
{
    public class LoginsController : Controller
    {
        private readonly IWebClientService webClient;

        public LoginsController(IWebClientService webClient)
        {
            this.webClient = webClient;
        }

        public ActionResult Login([FromUri] string error = null)
        {
            var model = new UsersDetailsViewModel();

            if (!string.IsNullOrEmpty(error))
            {
                if (error.ToLower() == "unauthorized")
                {
                    ModelState.AddModelError("", "You must login in order to access that page");
                }
            }

            ViewBag.Title = "Login";

            return View(model);
        }

        // POST: Logins/Create
        [System.Web.Mvc.HttpPost]
        public ActionResult Login(UsersDetailsViewModel model)
        {
            try
            {
                var existingLogin = webClient.ExecuteLoginPost<object>(new Models.ApiRequest()
                {
                    EndPoint = string.Format("logins"),
                    Request = new UserDto()
                    {
                        Username = model.Username,
                        Password = model.Password
                    }
                }) as JObject;

                if (existingLogin != null)
                {
                    var parsedLogin = existingLogin.ToObject<LoginDto>();
                    Session[Constants.Headers.AuthTokenHeader] = parsedLogin.AuthToken;

                    var user = webClient.ExecuteGet<UserDto>(new Models.ApiRequest() { EndPoint = string.Format("users/details/{0}", model.Username) });
                    if (user != null && user.IsAdmin)
                    {
                        Session[Constants.Headers.AdminTokenHeader] = parsedLogin.AuthToken;
                        Session[Constants.Headers.AuthTokenHeader] = null;
                    }

                    Session["LoginId"] = parsedLogin.Id;
                    Session["Username"] = model.Username;
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is wrong.");
                    return View();
                }

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ModelState.AddModelError("", "Oops. Something happened.");
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session[Constants.Headers.AuthTokenHeader] = null;
            Session[Constants.Headers.AdminTokenHeader] = null;

            return RedirectToAction("Index", "Home");
        }
    }
}
