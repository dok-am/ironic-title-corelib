namespace IT.CoreLib.Interfaces
{
    public interface IService
    {
        public void Initialize() { }
        public void OnInitialized(IContext context) { }
        public void Destroy() { }
        public void OnPaused(bool paused) { }
    }
}
