using Harmony;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PeterHan.PLib;
using System;
using System.IO;
using System.Reflection;

namespace ModFramework {
    public static class ModLocalization {
        public delegate void ModLocalizationCompleteDelegate(string languageCode);
        public static event ModLocalizationCompleteDelegate LocalizationCompleteEvent;

        private static string _localizationFolder;
        public static string LocalizationFolder {
            get {
                if (_localizationFolder == null) {
                    _localizationFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "languages");
                }

                return _localizationFolder;
            }
        }

        public static string DefaultLocalizationCode { get; set; } = "en";
        public static string[] DefaultLocalization { get; set; }

        public static bool LoadLocalization(string languageCode) {
            if (languageCode.IsNullOrWhiteSpace()) {
                return false;
            }

            if (!Directory.Exists(LocalizationFolder)) {
                Directory.CreateDirectory(LocalizationFolder);
                return false;
            }

            string[] languageFiles = Directory.GetFiles(LocalizationFolder, "*" + languageCode + "*", SearchOption.TopDirectoryOnly);
            if (languageFiles.Length == 0) {
                PUtil.LogDebug("can`t find localization files");
                return false;
            }

            string languageFile = languageFiles[0];
            for (int i = 0; i < languageFiles.Length; ++i) {
                string languageFile0 = languageFiles[0];

                if (languageFile0 == languageCode) {
                    languageFile = languageFile0;
                    break;
                }
            }
            
            try {
                using (StreamReader reader = File.OpenText(languageFile))
                using (JsonTextReader jsonReader = new JsonTextReader(reader)) {
                    JObject rootObject = (JObject) JToken.ReadFrom(jsonReader).Root;

                    foreach (JProperty property in rootObject.Properties()) {
                        JToken value = property.Value;

                        if (value != null && value.Type == JTokenType.String) { 
                            Strings.Add(property.Name, value.Value<string>());
                        }
                    }
                }

                return true;
            }

            catch (Exception exception) {
                Debug.Log("Error when reading localization: " + languageFile + ",\n" + nameof(exception) + ": " + exception.Message);
            }

            return false;
        }

        public static void TryWriteTemplate() {
            if (!Directory.Exists(LocalizationFolder)) {
                Directory.CreateDirectory(LocalizationFolder);
            }

            if (DefaultLocalization == null) {
                return;
            }

            string templateFile = Path.Combine(LocalizationFolder, "locale.template");
            if (!File.Exists(templateFile)) {
                using TextWriter textWriter = File.CreateText(templateFile);
                using JsonTextWriter jsonWriter = new JsonTextWriter(textWriter) {
                    Formatting = Formatting.Indented
                };

                jsonWriter.WriteStartObject();

                for (int i = 0; i < DefaultLocalization.Length - 1; i += 2) {
                    jsonWriter.WritePropertyName(DefaultLocalization[i]);
                    jsonWriter.WriteValue(DefaultLocalization[i + 1]);
                }

                jsonWriter.WriteEndObject();
            }
        }

        [HarmonyPatch(typeof(Localization), "Initialize")]
        public static class Localization_Initialize {
            public static void Postfix(Localization.Locale ___sLocale) {
                TryWriteTemplate();
                if (___sLocale == null || !(LoadLocalization(___sLocale.Code) || LoadLocalization(___sLocale.Code + ".json"))) {
                    if (DefaultLocalization != null) {
                        for (int i = 0; i < DefaultLocalization.Length - 1; i += 2) {
                            Strings.Add(DefaultLocalization[i], DefaultLocalization[i + 1]);
                        }

                        LocalizationCompleteEvent?.Invoke(DefaultLocalizationCode);
                    }

                    else {
                        LocalizationCompleteEvent?.Invoke(null);
                    }
                }

                else {
                    LocalizationCompleteEvent?.Invoke(___sLocale.Code);
                }
            }
        }
    }
}
