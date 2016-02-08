using Boxing.Contracts.Resources;
using BoxingWebApp.Services;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace BoxingWebApp.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// This extension generates html string for the main toolbar in the grid
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="addable"></param>
        /// <param name="searchable"></param>
        /// <param name="searchString"></param>
        /// <param name="additionalObjects">The first element is the mvc html string for the control to create, 
        /// the second is the id of this control and the third is the text in the label we are supposed to create for this control.
        /// If second and third are empty, we only create the mvc control without label</param>
        /// <returns></returns>
        public static IHtmlString MainToolbar(this HtmlHelper helper, bool addable = true, bool searchable = false, string searchString = null,
                                              params Tuple<MvcHtmlString, string, string>[] additionalObjects)
        {
            var userIsAdmin = AuthorizeExtensions.CurrentUserIsAdmin();

            var htmlText = "<div class='mainToolbar'>";

            if (userIsAdmin && addable)
            {
                htmlText += "<div>";
                htmlText += helper.ActionLink(BoxingResources.AddNew, "Create", null, new { @class = "boxingButton" }).ToString();
                htmlText += "</div>";
            }

            if (searchable)
            {
                htmlText += "<div>";
                htmlText += helper.TextBox("Search", searchString, htmlAttributes: new { @class = "form-control boxingInput searchInput" }).ToString();
                htmlText += helper.ActionLink("\u26B2", "", "", htmlAttributes: new { href = "javascript:void(0)", @class = "boxingButton searchButton" }).ToString();
                htmlText += "</div>";
            }

            if (additionalObjects != null)
            {
                foreach (var htmlObject in additionalObjects)
                {
                    htmlText += "<div class='additionalToolbarObjectTd'>";
                    // Then we should add label for the html object
                    if (!string.IsNullOrEmpty(htmlObject.Item2) && !string.IsNullOrEmpty(htmlObject.Item3))
                    {
                        htmlText += "<label for='" + htmlObject.Item2 + "'>" + htmlObject.Item3 + "</label>";
                    }
                    htmlText += htmlObject.Item1;
                    htmlText += "</div>";
                }
            }

            htmlText += "</div>";

            return MvcHtmlString.Create(htmlText);
        }

        public static IHtmlString Pager(this HtmlHelper helper, string model, int? page, int? pageSize, string sort = null, string order = null, string search = null,
                                        Dictionary<string, object> otherQueryParams = null)
        {
            var webClient = new WebClientService(new ConfigurationService());
            var count = webClient.ExecuteGet<int>(new Models.ApiRequest() { EndPoint = $"count?model={model}" });

            var batches = count / pageSize;

            if ((count % pageSize) == 0 && count > 0)
            {
                batches--;
            }

            var htmlText = $"<div class='pageDescriptionDiv'>Page {((page != null && page > 0) ? page.ToString() : "1") } of { (batches > 0 ? (batches + 1).ToString() : "1") }</div>";
            htmlText += "<ul class='boxingPager'>";

            for (var i = 0; i <= batches; i++)
            {
                if ((i + 1) == page)
                {
                    htmlText += "<li class='active'>";
                    htmlText += helper.ActionLink((i + 1).ToString(), "", "", new { href = "javascript:void(0)" }).ToString();
                }
                else
                {
                    htmlText += "<li>";
                    var routeValues = CreateRouteValues(i, pageSize, sort, order, search, otherQueryParams);
                    htmlText += helper.ActionLink((i + 1).ToString(), "Index", routeValues).ToString();
                }
                htmlText += "</li>";
            }

            htmlText += "</ul>";

            htmlText += helper.DropDownList("pageSize",
                                            new SelectList(new int[] { 10, 20, 50, 100 }),
                                            "Page Size: ",
                                            htmlAttributes: new
                                            {
                                                @class = "boxingDropDownList pageSizeDropDown"
                                            }).ToString();

            return MvcHtmlString.Create(htmlText);
        }

        private static RouteValueDictionary CreateRouteValues(int count, int? pageSize, string sort = null, string order = null, string search = null, Dictionary<string, object> otherQueryParams = null)
        {
            var routeValues = new Dictionary<string, object>();

            routeValues.Add("skip", (pageSize ?? 0) * count);
            routeValues.Add("take", (pageSize ?? 0));

            if (!string.IsNullOrEmpty(sort))
            {
                routeValues.Add("sort", sort);
            }

            if (!string.IsNullOrEmpty(order))
            {
                routeValues.Add("order", order);
            }
            if (!string.IsNullOrEmpty(search))
            {
                routeValues.Add("search", search);
            }

            if (otherQueryParams != null)
            {
                foreach (var queryParam in otherQueryParams)
                {
                    routeValues.Add(queryParam.Key, queryParam.Value);
                }
            }

            return new RouteValueDictionary(routeValues);
        }

    }
}
