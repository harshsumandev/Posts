using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonPostsRepositoryService
{
    /// <summary>
    /// Singleton class to Manage all the services
    /// </summary>
    public class ServiceManager
    {
        private static volatile ServiceManager _serviceManager = null;
        private static object _syncLock = new object();

        private ServiceManager() { }


        public static ServiceManager Instance
        {
            get
            {
                if (_serviceManager == null)
                {
                    lock (_syncLock)
                    {
                        if (_serviceManager == null)
                            _serviceManager = new ServiceManager();
                    }
                }

                return _serviceManager;
            }
        }

        /// <summary>
        /// Method to return new service for JsonPostsRepositoryService
        /// </summary>
        /// <returns></returns>
        public IJsonPostsRepositoryService GetService()
        {
            return new JsonPostsRepositoryService();
        }
    }
}
