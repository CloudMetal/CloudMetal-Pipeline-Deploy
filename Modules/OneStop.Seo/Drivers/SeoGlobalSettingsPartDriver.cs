using Onestop.Seo.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;

namespace Onestop.Seo.Drivers {
    public class SeoGlobalSettingsPartDriver : ContentPartDriver<SeoGlobalSettingsPart> {
        protected override string Prefix {
            get { return "Onestop.Seo.GlobalSettingsPart"; }
        }

        protected override DriverResult Editor(SeoGlobalSettingsPart part, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_SeoGlobalSettings_Homepage_Edit",
                             () => shapeHelper.EditorTemplate(
                                 TemplateName: "Parts.SeoGlobalSettings.Homepage",
                                 Model: part,
                                 Prefix: Prefix)),
                ContentShape("Parts_SeoGlobalSettings_TitlePatterns_Edit",
                             () => shapeHelper.EditorTemplate(
                                 TemplateName: "Parts.SeoGlobalSettings.TitlePatterns",
                                 Model: part,
                                 Prefix: Prefix)),
                ContentShape("Parts_SeoGlobalSettings_DescriptionPatterns_Edit",
                             () => shapeHelper.EditorTemplate(
                                 TemplateName: "Parts.SeoGlobalSettings.DescriptionPatterns",
                                 Model: part,
                                 Prefix: Prefix)),
                ContentShape("Parts_SeoGlobalSettings_KeywordsPatterns_Edit",
                             () => shapeHelper.EditorTemplate(
                                 TemplateName: "Parts.SeoGlobalSettings.KeywordsPatterns",
                                 Model: part,
                                 Prefix: Prefix)),
                ContentShape("Parts_SeoGlobalSettings_OtherSettings_Edit",
                             () => shapeHelper.EditorTemplate(
                                 TemplateName: "Parts.SeoGlobalSettings.OtherSettings",
                                 Model: part,
                                 Prefix: Prefix))
                );
        }

        protected override DriverResult Editor(SeoGlobalSettingsPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(SeoGlobalSettingsPart part, ExportContentContext context) {
            var partName = part.PartDefinition.Name;

            context.Element(partName).SetAttributeValue("HomeTitle", part.HomeTitle);
            context.Element(partName).SetAttributeValue("HomeDescription", part.HomeDescription);
            context.Element(partName).SetAttributeValue("HomeKeywords", part.HomeKeywords);
            context.Element(partName).SetAttributeValue("SeoPatternsDefinition", part.SeoPatternsDefinition);
            context.Element(partName).SetAttributeValue("SearchTitlePattern", part.SearchTitlePattern);
            context.Element(partName).SetAttributeValue("EnableCanonicalUrls", part.EnableCanonicalUrls);
        }

        protected override void Importing(SeoGlobalSettingsPart part, ImportContentContext context) {
            var partName = part.PartDefinition.Name;

            context.ImportAttribute(partName, "HomeTitle", value => part.HomeTitle = value);
            context.ImportAttribute(partName, "HomeDescription", value => part.HomeDescription = value);
            context.ImportAttribute(partName, "HomeKeywords", value => part.HomeKeywords = value);
            context.ImportAttribute(partName, "SeoPatternsDefinition", value => part.SeoPatternsDefinition = value);
            context.ImportAttribute(partName, "SearchTitlePattern", value => part.SearchTitlePattern = value);
            context.ImportAttribute(partName, "EnableCanonicalUrls", value => part.EnableCanonicalUrls = bool.Parse(value));
        }
    }
}