namespace Platformer.Managers
{
    public interface IManager
    {
        ManagerStatus Status { get; }

        void Startup();
    }
}
