namespace IT.CoreLib.Interfaces
{
    public interface IContext 
    {
        public IContext Parent { get; }
        public T GetService<T>() where T : IService;
        public T GetSceneBinder<T>() where T : ISceneBinder;
    }
}
