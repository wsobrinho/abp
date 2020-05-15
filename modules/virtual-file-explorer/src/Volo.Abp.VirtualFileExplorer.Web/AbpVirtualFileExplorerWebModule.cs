﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileExplorer.Localization;
using Volo.Abp.VirtualFileExplorer.Web.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.VirtualFileExplorer.Web
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiThemeSharedModule))]
    public class AbpVirtualFileExplorerWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpVirtualFileExplorerWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var virtualFileExplorerOptions = context.Services.ExecutePreConfiguredActions<AbpVirtualFileExplorerOptions>();

            if (virtualFileExplorerOptions.IsEnabled)
            {
                Configure<AbpNavigationOptions>(options =>
                {
                    options.MenuContributors.Add(new VirtualFileExplorerMenuContributor());
                });

                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.AddEmbedded<AbpVirtualFileExplorerWebModule>("Volo.Abp.VirtualFileExplorer.Web");
                });

                Configure<AbpLocalizationOptions>(options =>
                {
                    options.Resources
                        .Add<VirtualFileExplorerResource>("en")
                        .AddBaseTypes(typeof(AbpValidationResource))
                        .AddVirtualJson("/Localization/Resources");
                });
            }
        }
    }
}
