using System.Collections.Generic;
using System.Web.Mvc;
using Spark.Web.Mvc;

namespace Spark.Nemerle
{
    public static class SparkRubyEngineStarter
    {
        public static void ConfigureContainer(ISparkServiceContainer container)
        {
            container.SetServiceBuilder<IViewEngine>(c => new SparkViewFactory(c.GetService<ISparkSettings>()));
            container.SetServiceBuilder<ISparkLanguageFactory>(c => new NemerleLanguageFactory());
        }

        public static ISparkServiceContainer CreateContainer()
        {
            var container = new SparkServiceContainer();
            ConfigureContainer(container);
            return container;
        }

        public static ISparkServiceContainer CreateContainer(ISparkSettings settings)
        {
            var container = new SparkServiceContainer(settings);
            ConfigureContainer(container);
            return container;
        }

        /// <summary>
        /// Creates a spark IViewEngine with Nemerle as the default language.
        /// Settings come from config or are defaulted.
        /// </summary>
        /// <returns>An IViewEngine interface of the SparkViewFactory</returns>
        public static IViewEngine CreateViewEngine()
        {
            return CreateContainer().GetService<IViewEngine>();
        }

        /// <summary>
        /// Creates a spark IViewEngine with Nemerle as the default language.
        /// </summary>
        /// <param name="settings">Typically an instance of SparkSettings object</param>
        /// <returns>An IViewEngine interface of the SparkViewFactory</returns>
        public static IViewEngine CreateViewEngine(ISparkSettings settings)
        {
            return CreateContainer(settings).GetService<IViewEngine>();
        }

        /// <summary>
        /// Installs the Nemerle view engine. Settings come from config or are defaulted.
        /// </summary>
        public static void RegisterViewEngine()
        {
            ViewEngines.Engines.Add(CreateViewEngine());
        }

        /// <summary>
        /// Installs the Nemerle view engine. Settings passed in.
        /// </summary>
        public static void RegisterViewEngine(ISparkSettings settings)
        {
            ViewEngines.Engines.Add(CreateViewEngine(settings));
        }

        /// <summary>
        /// Installs the Spark view engine. Service container passed in.
        /// </summary>
        public static void RegisterViewEngine(ISparkServiceContainer container)
        {
            ViewEngines.Engines.Add(container.GetService<IViewEngine>());
        }

        /// <summary>
        /// Installs the Nemerle view engine. Settings come from config or are defaulted.
        /// </summary>
        /// <param name="engines">Typically in the ViewEngines.Engines collection</param>
        public static void RegisterViewEngine(ICollection<IViewEngine> engines)
        {
            engines.Add(CreateViewEngine());
        }

        /// <summary>
        /// Installs the Nemerle view engine. Settings passed in.
        /// </summary>
        /// <param name="engines">Typically in the ViewEngines.Engines collection</param>
        /// <param name="settings">Typically an instance of SparkSettings object</param>
        public static void RegisterViewEngine(ICollection<IViewEngine> engines, ISparkSettings settings)
        {
            engines.Add(CreateViewEngine(settings));
        }

        /// <summary>
        /// Install the view engine from the container. Typical usage is to call CreateContainer,
        /// provide additinal service builder functors to override certain classes, then call this
        /// method.
        /// </summary>
        /// <param name="engines">Typically the ViewEngines.Engines collection</param>
        /// <param name="container">A service container, often created with CreateContainer</param>
        public static void RegisterViewEngine(ICollection<IViewEngine> engines, ISparkServiceContainer container)
        {
            engines.Add(container.GetService<IViewEngine>());
        }
    }
}