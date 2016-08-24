using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Sightstone.Helpers;
using Sightstone.ViewModels;
using SightStone.Helpers;

namespace SightStone
{
    public class AppBootstrapper : BootstrapperBase
    {
        private CompositionContainer _container;

        public AppBootstrapper()
        {
            Initialize();
        }

        /*
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            var assemblies = base.SelectAssemblies().ToList();
            assemblies.Add(typeof(ShellViewModel).GetTypeInfo().Assembly);
            assemblies.Add(typeof(Sightstone.Views.ShellView).GetTypeInfo().Assembly);
            return assemblies;
        }
        //*/

        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance);
        }

        /// <summary>
        ///     By default, we are configured to use MEF
        /// </summary>
        protected override void Configure()
        {

            ConventionManager.ApplyValueConverter = ValueTypeConverter.ApplyValueConverter;

            var catalog = new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)));

            _container = new CompositionContainer(catalog);

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(_container);
            batch.AddExportedValue(catalog);

            _container.Compose(batch);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = _container.GetExportedValues<object>(contract);

            var enumerable = exports as object[] ?? exports.ToArray();
            if (enumerable.Any())
            {
                return enumerable.First();
            }

            throw new Exception($"Could not locate any instances of contract {contract}.");
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var startupTasks =
                GetAllInstances(typeof(StartupTask))
                .Cast<ExportedDelegate>()
                .Select(exportedDelegate => (StartupTask)exportedDelegate.CreateDelegate(typeof(StartupTask)));

            startupTasks.Apply(s => s());
            var settings = new Dictionary<string, object>
            {
                { "Icon", new BitmapImage(new Uri("pack://application:,,,/Sightstone;component/Icon.ico")) },
                { "ResizeMode", ResizeMode.CanResize },
                { "Width", 1200 },
                { "Height", 750 },
                { "MinWidth", 900 },
                { "MinHeight", 650 }
            };

            DisplayRootViewFor<IShell>(settings);
        }
    }
}
