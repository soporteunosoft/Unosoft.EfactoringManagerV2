using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace Unosoft.EFactoringManager.Localization;

public static class EFactoringManagerLocalizationConfigurer
{
    public static void Configure(ILocalizationConfiguration localizationConfiguration)
    {
        localizationConfiguration.Sources.Add(
            new DictionaryBasedLocalizationSource(EFactoringManagerConsts.LocalizationSourceName,
                new XmlEmbeddedFileLocalizationDictionaryProvider(
                    typeof(EFactoringManagerLocalizationConfigurer).GetAssembly(),
                    "Unosoft.EFactoringManager.Localization.SourceFiles"
                )
            )
        );
    }
}
