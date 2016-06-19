using System.AddIn.Hosting;

namespace Sightstone.Plugin.Loader
{
    public abstract class PluginTokenBase
    {
        protected PluginTokenBase(AddInToken addInToken)
        {
            Token = addInToken;
        }

        protected AddInToken Token { get; }
    }
}