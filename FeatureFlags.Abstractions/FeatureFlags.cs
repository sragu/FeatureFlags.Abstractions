using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace FeatureFlags.Abstractions
{
    public class FeatureFlags
    {
        public static T Map<T>() where T : new()
        {
            var features = new T();
            var togglesFile = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/featureToggles.xml").Element("featureToggles") ?? new XElement("featureToggles");
            var toggles = togglesFile.Elements().ToDictionary(x => x.Name.LocalName, ConvertToFeature);
            var flags = features.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType == typeof (Feature)).ToList();

            flags.ForEach(f => f.SetValue(features, toggles.ContainsKey(f.Name) ? toggles[f.Name] : new Feature(f.Name, false)));
            return features;
        }

        private static Feature ConvertToFeature(XElement element)
        {
            return new Feature(element.Attribute("name").Value, element.Attribute("value").Value.Equals("on", StringComparison.OrdinalIgnoreCase));
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

        public bool Expired(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
}