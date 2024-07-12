using UnityEngine;

namespace IT.CoreLib.Interfaces
{

    public interface IService
    {
        public void Initialize();
        public void OnInitialized(IBootstrap bootstrap);
    }

}
