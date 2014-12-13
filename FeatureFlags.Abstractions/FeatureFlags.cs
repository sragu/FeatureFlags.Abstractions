using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace FeatureFlags.Abstractions
{
    public class FeatureFlags
    {
        public static T Map<T>(string togglesPath) where T : new()
        {
            var features = new T();
            var togglesFile = XDocument.Load(togglesPath).Element("featureToggles") ?? new XElement("featureToggles");
            var toggles = togglesFile.Elements().ToDictionary(x => x.Name.LocalName, XmlElementToFeature);

            var flags = features.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.PropertyType == typeof (Feature))
                    .ToList();

            flags.ForEach(f => f.SetValue(features, toggles.GetOrDefault(f.Name, new Feature(f.Name, false))));
            return features;
        }

        private static Feature XmlElementToFeature(XElement element)
        {
            var featureName = element.Attribute("name").Value;
            var featureValue = element.Attribute("value").Value;

            return new Feature(featureName, "on".Equals(featureValue, StringComparison.OrdinalIgnoreCase));
        }
    }

    public static class DictionaryExtensions
    {
        public static T GetOrDefault<T>(this IReadOnlyDictionary<string, T> toggles, string key,  T defaultFeature)
        {
            return toggles.ContainsKey(key) ? toggles[key] : defaultFeature;
        }
    }

    public class Feature
    {
        private readonly string name;
        private readonly bool state;

        public Feature(string name, bool state)
        {
            this.name = name;
            this.state = state;
        }

        public bool IsOn()
        {
            return state;
        }

        public string Name()
        {
            return name;
        }
    }
}