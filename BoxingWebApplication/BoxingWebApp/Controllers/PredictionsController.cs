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

            model.Items = webClient.ExecuteGet<IEnumerable<PredictionDto>>(new Models.ApiRequest() { EndPoint = "predictions?skip=0&take=10" })
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

            var matchListItem = new MatchesListItem()
            {
                Id = match.Id,
                Address = match.Address,
                Boxer1Id = match.Boxer1Id,
                Boxer2Id = match.Boxer2Id
            };

            ViewData["Match"] = matchListItem;

            var currentUser = AuthorizeExtensions.GetCurrentUser();
            if (currentUser == null)
            {
                return View();
            }

            model.UserId = currentUser.Id;

            var prediction = webClient.ExecuteGet<IEnumerable<PredictionDto>>(new Models.ApiRequest() { EndPoint = "predictions?skip=0&take=10" })
               ?.Select(q => new PredictionsListItem() { Id = q.Id, UserId = q.UserId, MatchId = q.MatchId, PredictedBoxerId = q.PredictedBoxerId })?.
                 FirstOrDefault(p => p.MatchId == match.Id && p.UserId == currentUser.Id);

            if (prediction != null)
            {
                model.Id = prediction.Id;
                model.PredictedBoxerId = prediction.PredictedBoxerId;
            }

            var boxers = webClient.ExecuteGet<IEnumerable<BoxerDto>>(new Models.ApiRequest() { EndPoint = "boxers?skip=0&take=10" })
               ?.Select(q => new BoxersListItem() { Id = q.Id, Name = q.Name })?.Where(b => b.Id == match.Boxer1Id || b.Id == match.Boxer2Id)?.ToList();

            if (boxers != null)
            {
                if (model.PredictedBoxerId != 0)
                {
                    SelectList list = new SelectList(boxers, "Id", "Name", model.PredictedBoxerId);
                    ViewData["Boxers"] = list;
                }
                else
                {
                    SelectList list = new SelectList(boxers, "Id", "Name", 1);
                    ViewData["Boxers"] = list;
                }
            }

            if (boxers.Count == 2)
            {
                matchListItem.Boxer1 = boxers.FirstOrDefault(b => b.Id == match.Boxer1Id);
                matchListItem.Boxer2 = boxers.FirstOrDefault(b => b.Id == match.Boxer2Id);

                ViewData["Match"] = matchListItem;
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
                if (currentUser == null)
                {
                    return View();
                }

                var prediction = webClient.ExecuteGet<IEnumerable<PredictionDto>>(new Models.ApiRequest() { EndPoint = "predictions?skip=0&take=10" })?.
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

        // GET: Predictions/Delete/5
        public ActionResult Delete(int id)
        {
            webClient.ExecuteDelete(new Models.ApiRequest() { EndPoint = string.Format("predictions/{0}", id) });

            return RedirectToAction("Index");
        }
    }
}
