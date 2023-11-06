using BlockchainVotingApp.Core.Infrastructure;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BlockchainVotingApp.TagHelpers
{
    [HtmlTargetElement("admin", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AdminTagHelper : TagHelper
    {
        private readonly IAppUserService _appUserService;

        public AdminTagHelper(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var userRoles = await _appUserService.GetUserRoles();

            if (!userRoles.Any(item => item == "Admin"))
            {
                output.SuppressOutput();
                return;
            }
        }
    }
}
