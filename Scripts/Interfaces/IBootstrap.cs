namespace IT.CoreLib.Interfaces
{
    public interface IBootstrap 
    {
        public IBootstrap Parent { get; }
        public T GetService<T>() where T : IService;
    }
}
