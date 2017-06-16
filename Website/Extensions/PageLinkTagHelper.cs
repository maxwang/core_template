using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Models;

namespace Website.Extensions
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory _urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            _urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PagingInfo PageModel { get; set; }

        public string PageAction { get; set; }

        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            TagBuilder result = new TagBuilder("div");

            int start = 1;
            int last = PageModel.TotalPages;
            int current = PageModel.CurrentPage;

            start = (current / 10) * 10 + 1;

            //display 10 page buttons
            int end = start + 10;

            if (end > PageModel.TotalPages)
            {
                end = PageModel.TotalPages;
            }


            if (start != 1)
            {
                result.InnerHtml.AppendHtml(CreateLinkButton(urlHelper, 1));
            }

            //need add first, last, .... link
            if (start > 1)
            {
                result.InnerHtml.AppendHtml(CreateLinkButton(urlHelper, start - 1, "..."));
            }

            for (int i = start; i <= end; i++)
            {
                result.InnerHtml.AppendHtml(CreateLinkButton(urlHelper, i));
            }

            if (end < PageModel.TotalPages - 1)
            {
                result.InnerHtml.AppendHtml(CreateLinkButton(urlHelper, end + 1, "..."));
            }


            if (end != PageModel.TotalPages)
            {

                result.InnerHtml.AppendHtml(CreateLinkButton(urlHelper, PageModel.TotalPages));
            }

            output.Content.AppendHtml(result.InnerHtml);
        }

        private TagBuilder CreateLinkButton(IUrlHelper urlHelper, int page, string linkText = "")
        {
            TagBuilder tag = new TagBuilder("a");
            tag.Attributes["href"] = urlHelper.Action(PageAction,
               new { page = page, pageSize = PageModel.ItemsPerPage });
            if (PageClassesEnabled)
            {
                tag.AddCssClass(PageClass);
                tag.AddCssClass(page == PageModel.CurrentPage
                    ? PageClassSelected : PageClassNormal);
            }

            tag.InnerHtml.Append(string.IsNullOrEmpty(linkText) ? page.ToString() : linkText);

            return tag;
        }


    }
}
