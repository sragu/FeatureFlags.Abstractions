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
            var featureExpires = element.Attribute("expires");

            var expiresOn = featureExpires != null ? DateTime.Parse(featureExpires.Value) : DateTime.Today;
            return new Feature(featureName, "on".Equals(featureValue, StringComparison.OrdinalIgnoreCase), expiresOn);
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
        private readonly DateTime expiresOn;

        public Feature(string name, bool state, DateTime? expiresOn = null)
        {
            this.name = name;
            this.state = state;
            this.expiresOn = expiresOn.GetValueOrDefault(DateTime.Today);
        }

        public bool IsOn()
        {
            return state;
        }

        public string Name()
        {
            return name;
        }

        public bool Expired(DateTime dateTime)
        {
            return dateTime.Date.CompareTo(expiresOn.Date) >= 0;
        }
    }

    public static class FeatureWith
    {
        public static Feature StateOn()
        {
            return new Feature(string.Empty, true);
        }

        public static Feature StateOff()
        {
            return new Feature(string.Empty, false);
        }
    }
}