using System;
using System.Net;
using System.Threading.Tasks;
using Hangfire.Dashboard;

namespace Hangfire.Core.Dashboard.Management.Support
{
    internal class CommandWithResponseDispatcher : IDashboardDispatcher
    {
        private readonly Func<DashboardContext, Task<bool>> _command;

        public CommandWithResponseDispatcher(Func<DashboardContext, Task<bool>> command)
        {
            this._command = command;
        }

        public async Task Dispatch(DashboardContext context)
        {
            DashboardRequest request = context.Request;
            DashboardResponse response = context.Response;
            if (!"POST".Equals(request.Method, StringComparison.OrdinalIgnoreCase))
            {
                response.StatusCode = 405;
                //return (Task)Task.FromResult<bool>(false);
            }
            if (await this._command(context))
            {
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.OK;
                
            }
            else
            {
                response.StatusCode = 422;
            }
            
            //return (Task)Task.FromResult<bool>(true);
        }
    }
}
