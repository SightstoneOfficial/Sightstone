using Sightstone.Plugin.HostViewAddIn;

namespace Sightstone.Plugin.Loader
{
    public interface IPluginManager
    {
        TPlugin Load<TPlugin>() where TPlugin : class, IHostViewAddIn;
    }
}
