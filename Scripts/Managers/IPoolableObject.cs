
using IT.CoreLib.Interfaces;
using System;

namespace IT.CoreLib.Managers
{
    public interface IPoolableObject : IIdentifiable
    {
        public event Action<IPoolableObject> ReleaseFromPool;
    }
}