using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace Sightstone.Plugin
{
    [AddInContract]
    public interface IPluginLoader : IContract
    {
        /// <summary>
        /// The name of the application
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Name of the Author
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Put the starting code in here
        /// </summary>
        void Main();
    }
}
