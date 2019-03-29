using System;

namespace Hangfire.Core.Dashboard.Management.Metadata
{
    public class ManagementPageSectionAttribute : Attribute
    {
        public string Section { get; }

        public ManagementPageSectionAttribute(string section)
        {
            Section = section;
        }
    }
}