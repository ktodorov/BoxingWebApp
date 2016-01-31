using Boxing.Contracts.Dto;
using BoxingWebApp.Extensions;
using BoxingWebApp.Services;
using BoxingWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BoxingWebApp.Controllers
{
    public class PredictionsController : Controller
    {
        private readonly IWebClientService webClient;

        public PredictionsController(IWebClientService webClient)
        {
            this.webClient = webClient;
        }

        // GET: Predictions
        public ActionResult Index()
        {
            PredictionsListViewModel model = new PredictionsListViewModel();

            model.Items = webClient.ExecuteGet<IEnumerable<PredictionDto>>(new Models.ApiRequest() { EndPoint = "predictions" })
                ?.Select(q => new PredictionsListItem()
                {
                    Id = q.Id,
                    PredictedBoxerId = q.PredictedBoxerId,
                    MatchId = q.MatchId
                })?.ToList();

            return View(model);
        }

        // GET: Predictions/Predict?parentId=5
        public ActionResult Predict(int parentId)
        {
            PredictionsDetailsViewModel model = new PredictionsDetailsViewModel();
            model.MatchId = parentId;

            ViewBag.Title = "Predict";

            var match = webClient.ExecuteGet<MatchDto>(new Models.ApiRequest() { EndPoint = $"matches/{parentId}" });

            if (match == null)
            {
                return View();
            }

            var currentUser = AuthorizeExtensions.GetCurrentUser();
            if (currentUser == null)
            {
                throw new UnauthorizedAccessException();
            }

            model.UserId = currentUser.Id;

            var prediction = webClient.ExecuteGet<IEnumerable<PredictionDto>>(new Models.ApiRequest() { EndPoint = "predictions" })
               ?.Select(q => new PredictionsListItem() { Id = q.Id, UserId = q.UserId, MatchId = q.MatchId, PredictedBoxerId = q.PredictedBoxerId })?.
                 FirstOrDefault(p => p.MatchId == match.Id && p.UserId == currentUser.Id);

            if (prediction != null)
            {
                model.Id = prediction.Id;
                model.PredictedBoxerId = prediction.PredictedBoxerId;
            }

            var boxers = webClient.ExecuteGet<IEnumerable<BoxerDto>>(new Models.ApiRequest() { EndPoint = "boxers" })
               ?.Select(q => new BoxersListItem() { Id = q.Id, Name = q.Name })?.Where(b => b.Id == match.Boxer1Id || b.Id == match.Boxer2Id)?.ToList();

            if (boxers != null && boxers.Count == 2)
            {
                if (model.PredictedBoxerId != 0)
                {
                    ViewData["PredictedBoxer"] = boxers.FirstOrDefault(b => b.Id == model.PredictedBoxerId);
                    var boxersList = new List<BoxersListItem>();
                    if (model.PredictedBoxerId == match.Boxer1Id)
                    {
                        boxersList.Add(null);
                    }
                    boxersList.Add(boxers.FirstOrDefault(b => b.Id != model.PredictedBoxerId));
                    if (model.PredictedBoxerId == match.Boxer2Id)
                    {
                        boxersList.Add(null);
                    }
                    ViewData["Boxers"] = boxersList;
                }
                else
                {
                    ViewData["Boxers"] = boxers;
                }
            }

            return View(model);
        }

        // POST: Predictions/Predict
        [HttpPost]
        public ActionResult Predict(PredictionsDetailsViewModel model)
        {
            try
            {
                var currentUser = AuthorizeExtensions.GetCurrentUser();
                var match = webClient.ExecuteGet<MatchDto>(new Models.ApiRequest() { EndPoint = $"matches/{model.MatchId}" });
                if (currentUser == null || match == null)
                {
                    return View();
                }

                if (Request.Form[match.Boxer1Id.ToString()] != null)
                {
                    model.PredictedBoxerId = match.Boxer1Id;
                }
                else if (Request.Form[match.Boxer2Id.ToString()] != null)
                {
                    model.PredictedBoxerId = match.Boxer2Id;
                }
                else
                {
                    return View();
                }

                var prediction = webClient.ExecuteGet<IEnumerable<PredictionDto>>(new Models.ApiRequest() { EndPoint = "predictions" })?.
                     FirstOrDefault(p => p.MatchId == model.MatchId && p.UserId == currentUser.Id);

                if (prediction != null)
                {
                    webClient.ExecutePut<object>(new Models.ApiRequest()
                    {
                        EndPoint = string.Format("predictions/{0}", prediction.Id),
                        Request = new PredictionDto()
                        {
                            UserId = model.UserId,
                            MatchId = model.MatchId,
                            PredictedBoxerId = model.PredictedBoxerId
                        }
                    });
                }
                else
                {
                    webClient.ExecutePost<object>(new Models.ApiRequest()
                    {
                        EndPoint = string.Format("predictions"),
                        Request = new PredictionDto()
                        {
                            UserId = model.UserId,
                            MatchId = model.MatchId,
                            PredictedBoxerId = model.PredictedBoxerId
                        }
                    });
                }

                return RedirectToAction("Index", controllerName: "Matches");
            }
            catch
            {
                return View();
            }
        }

        // DELETE: Predictions/Delete/5
        public ActionResult Delete(int id)
        {
            webClient.ExecuteDelete(new Models.ApiRequest() { EndPoint = string.Format("predictions/{0}", id) });

            return RedirectToAction("Index", controllerName: "Matches");
        }
    }
}
