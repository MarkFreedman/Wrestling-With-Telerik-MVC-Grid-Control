using System;

namespace Sample.WebUI.Infrastructure
{
    public static class Utility
    {
        public static Exception GetInnermostException(Exception exception)
        {
            while (exception.InnerException != null)
            {
                exception = GetInnermostException(exception.InnerException);
            }

            return exception;
        }
    }
}