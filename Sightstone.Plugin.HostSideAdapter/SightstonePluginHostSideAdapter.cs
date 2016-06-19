using System;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sightstone.Plugin.Contracts;
using Sightstone.Plugin.HostViewAddIn;

namespace Sightstone.Plugin.HostSideAdapter
{
    [HostAdapter]
    public class SightstonePluginHostSideAdapter : ISightstonePlugin
    {
        private readonly ISightstonePluginContract _sightstonePluginContract;
        private readonly ContractHandle _contractHandle;
        public SightstonePluginHostSideAdapter(ISightstonePluginContract sightstonePluginContract)
        {
            _sightstonePluginContract = sightstonePluginContract;
            _contractHandle = new ContractHandle(sightstonePluginContract);
        }
        
        public void Main()
        {
            _sightstonePluginContract.Main();
        }

        public void Main(string[] args)
        {
            _sightstonePluginContract.Main(args);
        }

        public string Programmer => _sightstonePluginContract.Programmer;
        public string Designer => _sightstonePluginContract.Designer;

        public string MainViewModel => _sightstonePluginContract.MainViewModel;
        public string MainView => _sightstonePluginContract.MainView;

        public string SafeDir => _sightstonePluginContract.SafeDir;
    }
}
