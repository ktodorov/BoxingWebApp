﻿using Boxing.Contracts.Dto;
using BoxingWebApp.Extensions;
using BoxingWebApp.Services;
using BoxingWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace BoxingWebApp.Controllers
{
    public class MatchesController : Controller
    {
        private readonly IWebClientService webClient;

        public MatchesController(IWebClientService webClient)
        {
            this.webClient = webClient;
        }

        // GET: Matches
        public ActionResult Index([FromUri] int skip = 0, [FromUri] int take = 10, [FromUri] string search = null, [FromUri] bool? pastUnfinished = null)
        {
            if (pastUnfinished.HasValue && pastUnfinished.Value && !AuthorizeExtensions.CurrentUserIsAdmin())
            {
                throw new UnauthorizedAccessException();
            }

            MatchesListViewModel model = new MatchesListViewModel();

            var searchQueryParam = string.Empty;
            if (!string.IsNullOrEmpty(search))
            {
                searchQueryParam = $"&search={search}";
                ViewData["SearchString"] = search;
            }

            var pastUnfinishedQueryParam = string.Empty;
            if (pastUnfinished.HasValue)
            {
                pastUnfinishedQueryParam = $"&pastUnfinished={pastUnfinished.Value}";
                ViewData["PastUnfinished"] = pastUnfinished.Value;
            }

            model.Items = webClient.ExecuteGet<IEnumerable<MatchDto>>(new Models.ApiRequest() { EndPoint = $"matches?skip={skip}&take={take}{searchQueryParam}{pastUnfinishedQueryParam}" })
            ?.Select(q => new MatchesListItem()
            {
                Id = q.Id,
                Boxer1Id = q.Boxer1Id,
                Boxer2Id = q.Boxer2Id,
                Boxer1 = new BoxersListItem(q.Boxer1.Name),
                Boxer2 = new BoxersListItem(q.Boxer2.Name),
                Address = q.Address,
                Time = q.Time,
                Description = q.Description,
                WinnerId = q.WinnerId
            })?.ToList();

            var currentUser = AuthorizeExtensions.GetCurrentUser();
            var currentUserId = currentUser != null ? currentUser.Id : 0;

            var predictions = webClient.ExecuteGet<IEnumerable<PredictionDto>>(new Models.ApiRequest() { EndPoint = "predictions" })
                ?.Select(q => new PredictionsListItem()
                {
                    Id = q.Id,
                    PredictedBoxerId = q.PredictedBoxerId,
                    MatchId = q.MatchId,
                    UserId = q.UserId
                })?.Where(p => p.UserId == currentUserId)?.ToList();

            ViewData["Predictions"] = predictions;
            ViewData["Page"] = (skip / take) + 1;
            ViewData["PageSize"] = take;
            return View(model);
        }

        // GET: Matches/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Matches/Create
        public ActionResult Create()
        {
            MatchesDetailsViewModel model = new MatchesDetailsViewModel();

            ViewBag.Title = "Create";

            var boxers = webClient.ExecuteGet<IEnumerable<BoxerDto>>(new Models.ApiRequest() { EndPoint = "boxers" })
               ?.Select(q => new BoxersListItem() { Id = q.Id, Name = q.Name })?.ToList();

            if (boxers != null)
            {
                SelectList list = new SelectList(boxers, "Id", "Name", 1);
                ViewData["Boxers"] = list;
            }

            return View(model);
        }

        // POST: Matches/Create
        [System.Web.Mvc.HttpPost]
        public ActionResult Create(MatchesDetailsViewModel model)
        {
            try
            {
                object newMatch = webClient.ExecutePost<object>(new Models.ApiRequest()
                {
                    EndPoint = string.Format("matches"),
                    Request = new MatchDto()
                    {
                        Boxer1Id = model.Boxer1Id,
                        Boxer2Id = model.Boxer2Id,
                        Address = model.Address,
                        Time = model.Time,
                        Description = model.Description
                    }
                });

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Matches/Edit/5
        public ActionResult Edit(int id)
        {
            var match = webClient.ExecuteGet<MatchDto>(new Models.ApiRequest() { EndPoint = string.Format("matches/{0}", id) });

            MatchesDetailsViewModel model = new MatchesDetailsViewModel();

            model.Boxer1Id = match.Boxer1Id;
            model.Boxer2Id = match.Boxer2Id;
            model.Address = match.Address;
            model.Time = match.Time;
            model.Description = match.Description;

            ViewBag.Title = "Edit";

            var boxers = webClient.ExecuteGet<IEnumerable<BoxerDto>>(new Models.ApiRequest() { EndPoint = "boxers" })
               ?.Select(q => new BoxersListItem() { Id = q.Id, Name = q.Name })?.ToList();

            if (boxers != null)
            {
                SelectList list = new SelectList(boxers, "Id", "Name", model.Boxer1Id);
                ViewData["Boxers1"] = list;
                list = new SelectList(boxers, "Id", "Name", model.Boxer2Id);
                ViewData["Boxers2"] = list;
            }

            return View("Create", model);
        }

        // POST: Matches/Edit/5
        [System.Web.Mvc.HttpPost]
        public ActionResult Edit(int id, MatchesDetailsViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var response = webClient.ExecutePut<object>(new Models.ApiRequest()
                        {
                            EndPoint = string.Format("matches/{0}", model.Id),
                            Request = new MatchDto()
                            {
                                Boxer1Id = model.Boxer1Id,
                                Boxer2Id = model.Boxer2Id,
                                Address = model.Address,
                                Time = model.Time,
                                Description = model.Description
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

        // GET: Matches/Delete/5
        public ActionResult Delete(int id)
        {
            webClient.ExecuteDelete(new Models.ApiRequest() { EndPoint = string.Format("matches/{0}", id) });

            return RedirectToAction("Index");
        }

        // GET: Matches/Finish/5
        public ActionResult Finish(int id)
        {
            if (!AuthorizeExtensions.CurrentUserIsAdmin())
            {
                return View();
            }

            var match = webClient.ExecuteGet<MatchDto>(new Models.ApiRequest() { EndPoint = $"matches/{id}" });

            if (match == null)
            {
                return View();
            }

            ViewBag.Title = "Finish " + match.Boxer1?.Name + " vs " + match.Boxer2?.Name;

            var model = new MatchesListItem()
            {
                Id = match.Id,
                Boxer1Id = match.Boxer1Id,
                Boxer1 = new BoxersListItem(match.Boxer1.Name),
                Boxer2Id = match.Boxer2Id,
                Boxer2 = new BoxersListItem(match.Boxer2.Name),
                WinnerId = match.WinnerId
            };

            return View(model);
        }

        // POST: Matches/Finish/5
        [System.Web.Mvc.HttpPost]
        public ActionResult Finish(int id, int winnerId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var response = webClient.ExecutePut<object>(new Models.ApiRequest()
                        {
                            EndPoint = $"matches/{id}/finish",
                            Request = winnerId
                        });

                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("generalError", e.Message);

                        return View();
                    }
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Cancel(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var response = webClient.ExecutePut<object>(new Models.ApiRequest()
                        {
                            EndPoint = $"matches/{id}/cancel",
                        });

                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("generalError", e.Message);

                        return View();
                    }
                }

                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
