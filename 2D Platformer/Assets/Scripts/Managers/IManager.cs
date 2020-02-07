using System.Collections;

namespace Platformer.Managers
{
    public interface IManager
    {
        ManagerStatus Status { get; }

        void Startup();

        IEnumerator LoadAndCache();

        IEnumerator Load();
    }
}
