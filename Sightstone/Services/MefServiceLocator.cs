using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace SightStone.Services
{
    [Export(typeof(IServiceLocator))]
    public class MefServiceLocator : IServiceLocator
    {
        private readonly CompositionContainer _compositionContainer;

        [ImportingConstructor]
        public MefServiceLocator(CompositionContainer compositionContainer)
        {
            _compositionContainer = compositionContainer;
        }

        public T GetInstance<T>() where T : class
        {
            var instance = _compositionContainer.GetExportedValue<T>();
            if (instance != null)
            {
                return instance;
            }

            throw new Exception($"Could not locate any instances of contract {typeof (T)}.");
        }
    }
}