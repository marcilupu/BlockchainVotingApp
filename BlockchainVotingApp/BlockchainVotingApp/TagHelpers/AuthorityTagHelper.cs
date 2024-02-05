using BlockchainVotingApp.Core.Infrastructure;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BlockchainVotingApp.TagHelpers
{
    [HtmlTargetElement("authority", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AuthorityTagHelper : TagHelper
    {
        private readonly IAppUserService _appUserService;

        public AuthorityTagHelper(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var userRoles = await _appUserService.GetUserRoles();

            if (!userRoles.Any(item => item == "Authority"))
            {
                output.SuppressOutput();
                return;
            }
        }
    }
}
