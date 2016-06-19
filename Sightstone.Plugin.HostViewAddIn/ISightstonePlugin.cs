using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sightstone.Plugin.HostViewAddIn
{
    public interface ISightstonePlugin : IHostViewAddIn
    {
        /// <summary>
        /// Entry point of plug-in
        /// </summary>
        void Main();

        /// <summary>
        /// Entry point of plug-in
        /// </summary>
        /// <param name="args">Sightstone startup args</param>
        void Main(string[] args);

        string Programmer { get; }
        string Designer { get; }

        string MainViewModel { get; }
        string MainView { get; }

        string SafeDir { get; }
    }
}
