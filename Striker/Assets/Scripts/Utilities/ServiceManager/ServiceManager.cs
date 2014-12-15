//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Utilities.ServiceManager
{
    using System;
    using System.Collections.Generic;

    public class ServiceManager : IServiceManager
    {
        #region Private Members

        private Dictionary<Type, IService> m_services = new Dictionary<Type, IService>();

        private static ServiceManager m_instance;

        #endregion

        #region Constructor

        private ServiceManager()
        {
        }

        public static IServiceManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new ServiceManager();
                }

                return m_instance;
            }
        }

        #endregion

        #region IServiceManager methods

        public void AddService(IService service)
        {
            if(m_services.ContainsValue(service))
            {
                string message = string.Format("AddService() failed to add {0} | service already exists.", service.Name);
                throw new ServiceException(message);
            }

            foreach (Type serviceType in service.ServiceTypes)
            {
                m_services.Add(serviceType, service);
            }
        }

        public T GetService<T>()
        {
            if(m_services.ContainsKey(typeof(T)))
            {
                return (T)m_services[typeof(T)];
            }

            throw new ServiceNotFoundException(typeof(T).ToString());
        }

        public T TryGetService<T>()
        {
            if (m_services.ContainsKey(typeof(T)))
            {
                return (T)m_services[typeof(T)];
            }

            return default(T);
        }

        public bool IsServiceAvailable<T>()
        {
            return m_services.ContainsKey(typeof(T));
        }

        public void RemoveService(IService service)
        {
            if(!m_services.ContainsValue(service))
            {
                string message = string.Format("RemoveService() failed to remove {0} | service does not exist.", service.Name);
                throw new ServiceNotFoundException(message);
            }

            List<Type> serviceKeys = new List<Type>(service.ServiceTypes);
            foreach(Type serviceType in serviceKeys)
            {
                if(m_services[serviceType] == service)
                {
                    m_services.Remove(serviceType);
                }
            }
        }

        #endregion
    }
}
