using System.Runtime.Remoting.Messaging;

namespace JerryPlat.Utils.Helpers
{
    public static class SingleInstanceHelper
    {
        private static readonly object _lockObj = new object();

        public static T GetInstance<T>() where T : class, new()
        {
            string strInstanceName = typeof(T).FullName;
            T _SingleInstance = CallContext.GetData(strInstanceName) as T;
            if (_SingleInstance == null)
            {
                lock (_lockObj)
                {
                    if (_SingleInstance == null)
                    {
                        _SingleInstance = new T();
                    }
                }

                CallContext.SetData(strInstanceName, _SingleInstance);
            }
            return _SingleInstance;
        }
    }
}