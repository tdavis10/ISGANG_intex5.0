using System;
using AuthLab2_RyanPinkney.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
// Authors Jacob Poor, Ryan Pinkney, Kevin Gutierrez, Tanner Davis
namespace AuthLab2_RyanPinkney.Infrastructure
{


    [HtmlTargetElement("div", Attributes = "page-blah-model")]
    public class PaginationTagHelper : TagHelper
    {

        // Dynamically create the page link

        private IUrlHelperFactory uhf;

        public PaginationTagHelper(IUrlHelperFactory temp)
        {
            uhf = temp;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext vc { get; set; }


        public PageInfo PageBlahModel { get; set; }
        public string PageAction { get; set; }


        // Stuff for styling
        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        // Override the basic template
        public override void Process(TagHelperContext thc, TagHelperOutput tho)
        {
            IUrlHelper uh = uhf.GetUrlHelper(vc);

            TagBuilder final = new TagBuilder("div");

            int pagesDisplayed = PageBlahModel.iCurrentPage;
            int currentPage = PageBlahModel.iCurrentPage;
            int totalPages = PageBlahModel.iTotalPages;

            while (currentPage < pagesDisplayed + 10)
            {

                TagBuilder tb = new TagBuilder("a");

                tb.Attributes["href"] = uh.Action(PageAction, new { iPageNum = currentPage });

                // Use the tag helper to store properties about the styling
                if (PageClassesEnabled)
                {
                    tb.AddCssClass(PageClass);
                    tb.AddCssClass(currentPage == PageBlahModel.iCurrentPage
                        ? PageClassSelected : PageClassNormal);
                }




                tb.InnerHtml.Append(currentPage.ToString());
                final.InnerHtml.AppendHtml(tb);

                // prevents pages greater than the max page
                if (currentPage > totalPages - 1)
                {
                    break;
                }
                currentPage += 1;
            }
            tho.Content.AppendHtml(final.InnerHtml);


        }
    }
}
