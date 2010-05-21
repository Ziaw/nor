using System;
using Spark.Compiler;

namespace Spark.Nemerle
{
    public class NemerleLanguageFactory: DefaultLanguageFactory
    {
        public override ViewCompiler CreateViewCompiler(ISparkViewEngine engine, SparkViewDescriptor descriptor)
        {
            var pageBaseType = engine.Settings.PageBaseType;
            if (string.IsNullOrEmpty(pageBaseType))
                pageBaseType = engine.DefaultPageBaseType;


            var language = descriptor.Language;
            if (language == LanguageType.Default)
                language = engine.Settings.DefaultLanguage;

            switch (language)
            {
                case LanguageType.Default:
                case LanguageType.CSharp:
                case LanguageType.VisualBasic:
                case LanguageType.Javascript:
                    return base.CreateViewCompiler(engine, descriptor);
            }

            ViewCompiler viewCompiler = new NemerleViewCompiler();
            viewCompiler.BaseClass = pageBaseType;
            viewCompiler.Descriptor = descriptor;
            viewCompiler.Debug = engine.Settings.Debug;
            viewCompiler.NullBehaviour = engine.Settings.NullBehaviour;
            viewCompiler.UseAssemblies = engine.Settings.UseAssemblies;
            viewCompiler.UseNamespaces = engine.Settings.UseNamespaces;
            return viewCompiler;
        }
    }
}