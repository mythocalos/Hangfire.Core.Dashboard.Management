using System;

namespace Hangfire.Core.Dashboard.Management.Metadata
{
    public class ManagementPageNavigation
    {
        public string Section { get; }
        public string Route { get; }
        public string SidebarName { get; }
        public string Title { get; }
        

        public ManagementPageNavigation( string section, string route, string title, string sidebarName)
        {
            if (string.IsNullOrWhiteSpace(section))
            {
                throw new ArgumentException($"ManagementPageNavigation.section cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(route))
            {
                route = section.ToLower().Replace(' ', '-');
            }

            Route = route;
            Section = section;
            SidebarName = string.IsNullOrWhiteSpace(sidebarName) ? route : sidebarName;
            Title = string.IsNullOrWhiteSpace(title)? route : title;

        }
    }
}