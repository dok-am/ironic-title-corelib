namespace IT.CoreLib.Interfaces
{
    public interface ISceneBinder 
    {
        public T GetManager<T>() where T : IManager;
    }
}