using System;
using System.AddIn.Hosting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sightstone.Plugin.HostViewAddIn;

namespace Sightstone.Plugin.Loader
{
    public sealed class PluginManager : IPluginManager
    {
        private readonly string _addInRootPath;
        private readonly IDictionary<Type, IList<PluginTokenBase>> _plugins;

        public PluginManager(string addInRootPath)
        {
            _addInRootPath = addInRootPath;
            _plugins = new Dictionary<Type, IList<PluginTokenBase>>();
            AddInStore.Update(addInRootPath);
        }

        public TPlugin Load<TPlugin>() where TPlugin : class, IHostViewAddIn
        {
            TPlugin plugin = this.GetPlugin<TPlugin>();

            if (plugin != null)
            {
                return plugin;
            }

            plugin = this.LoadPlugin<TPlugin>();

            return plugin;
        }

        public bool IsLoaded<TPlugin>() where TPlugin : class, IHostViewAddIn
        {
            return this.GetPlugin<TPlugin>() != null;
        }

        public void Unload<TPlugin>() where TPlugin : class, IHostViewAddIn
        {
            TPlugin plugin = this.GetPlugin<TPlugin>();

            if (plugin == null)
            {
                throw new InvalidOperationException("The plugin has already been unloaded.");
            }

            this.UnloadInternal(plugin);
        }

        private void UnloadInternal<TPlugin>(TPlugin plugin) where TPlugin : class, IHostViewAddIn
        {
            _plugins[typeof(TPlugin)] = new List<PluginTokenBase>();

            var addInController = AddInController.GetAddInController(plugin);
            addInController.Shutdown();
        }

        private TPlugin GetPlugin<TPlugin>() where TPlugin : class, IHostViewAddIn
        {
            PluginToken<TPlugin> pluginToken = this.GetPluginToken<TPlugin>();

            if (pluginToken != null)
            {
                return pluginToken.Plugin;
            }

            return null;
        }

        private PluginToken<TPlugin> GetPluginToken<TPlugin>() where TPlugin : class, IHostViewAddIn
        {
            IList<PluginTokenBase> tokens;

            if (_plugins.TryGetValue(typeof(TPlugin), out tokens))
            {
                return tokens.FirstOrDefault() as PluginToken<TPlugin>;
            }

            return null;
        }

        private TPlugin LoadPlugin<TPlugin>() where TPlugin : class, IHostViewAddIn
        {
            Collection<AddInToken> tokens = AddInStore.FindAddIns(typeof(ISightstonePlugin), _addInRootPath);

            IList<PluginTokenBase> pluginTokens = tokens.OrderBy(p => p.Version).Select(p => (PluginTokenBase)(new PluginToken<TPlugin>(p))).ToList();

            _plugins[typeof(TPlugin)] = pluginTokens;

            var pluginToken = pluginTokens.FirstOrDefault() as PluginToken<TPlugin>;

            if (pluginToken == null)
            {
                throw new InvalidOperationException("The plugin has not been found");
            }

            return pluginToken.Plugin;
        }
    }
}