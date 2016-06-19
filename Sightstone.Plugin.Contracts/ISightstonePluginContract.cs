using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace Sightstone.Plugin.Contracts
{
    [AddInContract]
    public interface ISightstonePluginContract : IContract
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
