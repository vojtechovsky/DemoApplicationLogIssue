using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using NLog;

namespace DemoApplicationLog
{
    internal static class Log
    {
        public static Logger Instance { get; private set; }
        static Log()
        {
            LogManager.ReconfigExistingLoggers();

            Instance = LogManager.GetCurrentClassLogger();
        }
    }

    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Log.Instance.Info("Webapp started.");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Log.Instance.Info("Session_Start");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Log.Instance.Info("Application_BeginRequest");
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            Log.Instance.Info("Application_AuthenticateRequest");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Log.Instance.Info("Application_Error");
            Exception lastException = Server.GetLastError();

            if (lastException != null)
            {
                Log.Instance.Error(lastException,"Application_Error");
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Log.Instance.Info("Session_End");
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Log.Instance.Info("Application_End");
            DelaySavingBySeconds(ConfigurationManager.AppSettings["DelaySavingBySeconds"]);
        }

        private static void DelaySavingBySeconds(string interval)
        {
            Log.Instance.Info($"Delay saving to total {interval} seconds");

            var value = ConfigurationManager.AppSettings["DelaySavingBySeconds"];
            if (string.IsNullOrEmpty(value))
            {
                interval = "0";
            }

            var intervalInSeconds = Convert.ToInt32(interval);

            foreach (var second in Enumerable.Range(1, intervalInSeconds))
            {
                Thread.Sleep(1000);
                Log.Instance.Info($"Delay {second} second passed");
            }

            Log.Instance.Info("Delaying passed!");
        }
    }
}