namespace Sightstone.Plugin
{
    public interface IPluginLoader
    {
        /// <summary>
        /// The name of the application
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Put the starting code in here
        /// </summary>
        void Action();
    }
}
