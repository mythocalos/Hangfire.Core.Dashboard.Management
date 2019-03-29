using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Core.Dashboard.Management.Metadata;
using Hangfire.Core.Dashboard.Management.Pages;
using Hangfire.Core.Dashboard.Management.Support;
using Hangfire.Dashboard;

namespace Hangfire.Core.Dashboard.Management
{
    public static class GlobalConfigurationExtension
    {
        public static void UseManagementPages(this IGlobalConfiguration config, Assembly assembly, List<ManagementPageNavigation> navigation)
        {
            JobsHelper.GetAllJobs(assembly);
            CreateManagement(navigation);
        }

        private static void CreateManagement(List<ManagementPageNavigation> navigationPages)
        {

            foreach (var page in navigationPages)
            {
                ManagementBasePage.BuildApiRoutesAndHandlersForAllJobs(page.Section);

                ManagementSidebarMenu.Items.Add(p => new MenuItem(page.SidebarName, p.Url.To($"{ManagementPage.UrlRoute}/{page.SidebarName.ToLower().Replace(" ", String.Empty)}"))
                {
                    Active = p.RequestPath.StartsWith($"{ManagementPage.UrlRoute}/{page.SidebarName.ToLower().Replace(" ", String.Empty)}")
                });

                DashboardRoutes.Routes.AddRazorPage($"{ManagementPage.UrlRoute}/{page.SidebarName.ToLower().Replace(" ", String.Empty)}", x => new ManagementBasePage(page.Title, page.Title, page.Section));
            }
            
            //note: have to use new here as the pages are dispatched and created each time. If we use an instance, the page gets duplicated on each call
            DashboardRoutes.Routes.AddRazorPage(ManagementPage.UrlRoute, x => new ManagementPage());
            
            // can't use the method of Hangfire.Console as it's usage overrides any similar usage here. Thus
            // we have to add our own endpoint to load it and call it from our code. Actually is a lot less work
            DashboardRoutes.Routes.Add("/jsm", new EmbeddedResourceDispatcher(Assembly.GetExecutingAssembly(), "Hangfire.Core.Dashboard.Management.Content.management.js"));

            NavigationMenu.Items.Add(page => new MenuItem(ManagementPage.Title, page.Url.To(ManagementPage.UrlRoute))
            {
                Active = page.RequestPath.StartsWith(ManagementPage.UrlRoute)
            });

        }
    }
}
