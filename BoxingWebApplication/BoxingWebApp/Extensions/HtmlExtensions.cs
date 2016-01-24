using BoxingWebApp.Services;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BoxingWebApp.Extensions
{
    public static class HtmlExtensions
    {
        public static IHtmlString Pager(this HtmlHelper helper, string model, int? page, int? pageSize)
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
                    htmlText += helper.ActionLink((i + 1).ToString(), "Index", new { skip = (pageSize ?? 0) * i, take = (pageSize ?? 0) }).ToString();
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

    }
}
