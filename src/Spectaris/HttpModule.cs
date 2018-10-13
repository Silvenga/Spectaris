using System;
using System.Web;

namespace Spectaris
{
    public class HttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
        }

        private void BeginRequest(object sender, EventArgs e)
        {
            var application = (HttpApplication) sender;
            var context = application.Context;
            context.Response.AddOnSendingHeaders(httpContext => httpContext.Response.Headers.Add("X-Handled", "true"));
        }

        public void Dispose()
        {
        }
    }
}