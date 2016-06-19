using System;
using System.AddIn.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sightstone.Plugin.HostViewAddIn;

namespace Sightstone.Plugin.Loader
{
    public sealed class PluginToken<TPlugin> : PluginTokenBase where TPlugin : IHostViewAddIn
    {
        private readonly Lazy<TPlugin> _plugin;

        public PluginToken(AddInToken addInToken) : base(addInToken)
        {
            _plugin = new Lazy<TPlugin>(() => addInToken.Activate<TPlugin>(AddInSecurityLevel.Internet));
        }

        public TPlugin Plugin => _plugin.Value;
    }
}
