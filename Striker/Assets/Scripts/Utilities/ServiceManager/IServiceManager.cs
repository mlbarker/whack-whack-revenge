//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Utilities.ServiceManager
{
    using System;
    using System.Collections.Generic;

    public interface IServiceManager
    {
        void AddService(IService service);

        void RemoveService(IService service);

        T GetService<T>();

        T TryGetService<T>();

        bool IsServiceAvailable<T>();
    }
}
