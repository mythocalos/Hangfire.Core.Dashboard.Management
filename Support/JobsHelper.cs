using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Hangfire.Core.Dashboard.Management.Metadata;

namespace Hangfire.Core.Dashboard.Management.Support
{
    public static class JobsHelper
    {
        public static List<JobMetadata> Jobs { get; private set; }
        internal static List<ManagementPageNavigation> Pages { get; set; }

        internal static void GetAllJobs(Assembly assembly)
        {
            Jobs = new List<JobMetadata>();
            Pages = new List<ManagementPageNavigation>();

            foreach (Type ti in  assembly.GetTypes().Where(x => !x.IsInterface && typeof(IJob).IsAssignableFrom(x) && x.Name != (typeof(IJob).Name)))
            {
                var q="default";

//                if (ti.GetCustomAttributes(true).OfType<ManagementPageAttribute>().Any())
//                {
//                    var attr = ti.GetCustomAttribute<ManagementPageAttribute>();
//                    q =  attr.Queue;
//                    if (!Pages.Any(x => x.MenuName == attr.MenuName)) Pages.Add(attr);
//                }
                

                foreach (var methodInfo in ti.GetMethods().Where(m => m.DeclaringType == ti))
                {
                    var jobData = new JobMetadata
                    {
                        Type = ti,
                        Queue = q //Defaulted to value from Class, can be override at method level
                    };
                    jobData.MethodInfo = methodInfo;
                    if (methodInfo.GetCustomAttributes(true).OfType<DescriptionAttribute>().Any())
                    {
                        jobData.Description = methodInfo.GetCustomAttribute<DescriptionAttribute>().Description;
                    }

                    if (methodInfo.GetCustomAttributes(true).OfType<ManagementPageSectionAttribute>().Any())
                    {
                        jobData.ManagementPageSection = methodInfo.GetCustomAttribute<ManagementPageSectionAttribute>().Section;
                    }

                    if (methodInfo.GetCustomAttributes(true).OfType<DisplayNameAttribute>().Any())
                    {
                        jobData.DisplayName = methodInfo.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                    }
                    else
                    {
                        jobData.DisplayName = methodInfo.Name;
                    }

                    if (methodInfo.GetCustomAttributes(true).OfType<QueueAttribute>().Any())
                    {
                        jobData.Queue = methodInfo.GetCustomAttribute<QueueAttribute>().Queue;
                    }

                    Jobs.Add(jobData);
                }
            }
        }
    }
}
