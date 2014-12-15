//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Utilities.ServiceManager
{
    using System;
    using System.Collections.Generic;

    public interface IService
    {
        string Name { get; }

        ICollection<Type> ServiceTypes { get; }

        void Initialize();
    }
}
