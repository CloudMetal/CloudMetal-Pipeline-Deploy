using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Script.Serialization;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.Records;
using Orchard.Core.Common.Utilities;
using Orchard.Data.Conventions;

namespace Onestop.Seo.Models {
    public class SeoGlobalSettingsPart : ContentPart<SeoGlobalSettingsPartRecord>, ISeoGlobalSettings {
        public string HomeTitle {
            get { return Record.HomeTitle; }
            set { Record.HomeTitle = value; }
        }

        public string HomeDescription {
            get { return Record.HomeDescription; }
            set { Record.HomeDescription = value; }
        }

        public string HomeKeywords {
            get { return Record.HomeKeywords; }
            set { Record.HomeKeywords = value; }
        }

        private readonly LazyField<IEnumerable<ContentTypeDefinition>> _seoContentTypes = new LazyField<IEnumerable<ContentTypeDefinition>>();
        public LazyField<IEnumerable<ContentTypeDefinition>> SeoContentTypesField { get { return _seoContentTypes; } }
        public IEnumerable<ContentTypeDefinition> SeoContentTypes {
            get { return _seoContentTypes.Value; }
        }

        #region SEO patterns
        public IDictionary<string, string> TitlePatternsViewDictionary {
            get { return GetSeoPatternsViewDictionary(SeoParameterType.Title); }

            set { SetSeoPatternsViewDictionary(SeoParameterType.Title, value); }
        }

        public IDictionary<string, string> DescriptionPatternsViewDictionary {
            get { return GetSeoPatternsViewDictionary(SeoParameterType.Description); }

            set { SetSeoPatternsViewDictionary(SeoParameterType.Description, value); }
        }

        public IDictionary<string, string> KeywordsPatternsViewDictionary {
            get { return GetSeoPatternsViewDictionary(SeoParameterType.Keywords); }

            set { SetSeoPatternsViewDictionary(SeoParameterType.Keywords, value); }
        }

        /// <summary>
        /// Only for model binding
        /// </summary>
        private IDictionary<SeoParameterType, IDictionary<string, string>> _seoPatternsViewDictionary;
        private IDictionary<SeoParameterType, IDictionary<string, string>> SeoPatternsViewDictionary {
            get {
                return _seoPatternsViewDictionary ??
                       (_seoPatternsViewDictionary = new Dictionary<SeoParameterType, IDictionary<string, string>>());
            }
            set { _seoPatternsViewDictionary = value; }
        }

        private IDictionary<string, string> GetSeoPatternsViewDictionary(SeoParameterType type) {
            if (!SeoPatternsViewDictionary.ContainsKey(type)) {
                var viewDictionary = SeoPatternsViewDictionary[type] = SeoContentTypes.ToDictionary(definition => definition.Name, definition => "");

                if (SeoPatternsDictionary.Count != 0 && SeoPatternsDictionary.ContainsKey(type)) {
                    foreach (var pattern in SeoPatternsDictionary[type]) {
                        viewDictionary[pattern.Key] = pattern.Value;
                    }
                }
            }

            return SeoPatternsViewDictionary[type];
        }

        private void SetSeoPatternsViewDictionary(SeoParameterType type, IDictionary<string, string> dictionary) {
            SeoPatternsViewDictionary[type] = dictionary;
            SeoPatternsDictionary[type] = dictionary;
            SaveSeoPatternsDictionary();
        }


        public void SetSeoPattern(SeoParameterType type, string contentType, string pattern) {
            if (!SeoPatternsDictionary.ContainsKey(type)) SeoPatternsDictionary[type] = new Dictionary<string, string>();
            SeoPatternsDictionary[type][contentType] = pattern;
            SaveSeoPatternsDictionary();
        }

        public string GetSeoPattern(SeoParameterType type, string contentType) {
            if (!SeoPatternsDictionary.ContainsKey(type) || !SeoPatternsDictionary[type].ContainsKey(contentType)) return null;
            return SeoPatternsDictionary[type][contentType];
        }

        private IDictionary<SeoParameterType, IDictionary<string, string>> _seoPatternsDictionary;
        private IDictionary<SeoParameterType, IDictionary<string, string>> SeoPatternsDictionary {
            get {
                if (_seoPatternsDictionary == null) {
                    if (String.IsNullOrEmpty(SeoPatternsDefinition)) {
                        _seoPatternsDictionary = new Dictionary<SeoParameterType, IDictionary<string, string>>();
                    }
                    else {
                        var serializer = new JavaScriptSerializer();
                        var tempDictionary = serializer.Deserialize<Dictionary<string, IDictionary<string, string>>>(SeoPatternsDefinition);
                        _seoPatternsDictionary = tempDictionary.ToDictionary(entry => serializer.ConvertToType<SeoParameterType>(entry.Key), entry => entry.Value);
                    }
                }

                return _seoPatternsDictionary;
            }

            set {
                _seoPatternsDictionary = value;
                SaveSeoPatternsDictionary();
            }
        }

        private void SaveSeoPatternsDictionary() {
            // This converting from enum-keyed to string-keyed dictionary is necessary for JavaScriptSerializer.
            // See: http://stackoverflow.com/questions/2892910/problems-with-json-serialize-dictionaryenum-int32
            var tempDictionary = SeoPatternsDictionary.Keys.ToDictionary(key => key.ToString(), key => SeoPatternsDictionary[key]);
            SeoPatternsDefinition = new JavaScriptSerializer().Serialize(tempDictionary);
        }

        /// <summary>
        /// Serialized title patterns
        /// </summary>
        public string SeoPatternsDefinition {
            get { return Record.SeoPatternsDefinition; }
            set { Record.SeoPatternsDefinition = value; }
        }
        #endregion

        public string SearchTitlePattern {
            get { return Record.SearchTitlePattern; }
            set { Record.SearchTitlePattern = value; }
        }

        public bool EnableCanonicalUrls {
            get { return Record.EnableCanonicalUrls; }
            set { Record.EnableCanonicalUrls = value; }
        }
    }

    public class SeoGlobalSettingsPartRecord : ContentPartVersionRecord {
        [StringLength(1024)]
        public virtual string HomeTitle { get; set; }

        [StringLengthMax]
        public virtual string HomeDescription { get; set; }

        [StringLengthMax]
        public virtual string HomeKeywords { get; set; }

        [StringLengthMax]
        public virtual string SeoPatternsDefinition { get; set; }

        [StringLength(1024)]
        public virtual string SearchTitlePattern { get; set; }

        public virtual bool EnableCanonicalUrls { get; set; }
    }
}