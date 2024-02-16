using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityToolkit.Mvvm.Helper
{
    public static class ExceptionExtensions
    {
        private static List<Type> frameworkExceptionTypes = new List<Type>();

        public static void RegisterFrameworkExceptionType(Type frameworkExceptionType)
        {
            if (frameworkExceptionType == null)
            {
                throw new ArgumentNullException("frameworkExceptionType");
            }

            if (!frameworkExceptionTypes.Contains(frameworkExceptionType))
            {
                frameworkExceptionTypes.Add(frameworkExceptionType);
            }
        }

        public static bool IsFrameworkExceptionRegistered(Type frameworkExceptionType)
        {
            return frameworkExceptionTypes.Contains(frameworkExceptionType);
        }

        public static Exception GetRootException(this Exception exception)
        {
            Exception ex = exception;
            try
            {
                while (true)
                {
                    if (ex == null)
                    {
                        ex = exception;
                        break;
                    }

                    if (IsFrameworkException(ex))
                    {
                        ex = ex.InnerException;
                        continue;
                    }

                    break;
                }
            }
            catch (Exception)
            {
                ex = exception;
            }

            return ex;
        }

        private static bool IsFrameworkException(Exception exception)
        {
            bool num = frameworkExceptionTypes.Contains(exception.GetType());
            bool flag = false;
            if (exception.InnerException != null)
            {
                flag = frameworkExceptionTypes.Contains(exception.InnerException.GetType());
            }

            return num || flag;
        }
    }
}
