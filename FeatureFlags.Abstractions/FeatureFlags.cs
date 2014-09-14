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
            var togglesFile =
                XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/featureToggles.xml").Element("featureToggles") ??
                new XElement("featureToggles");
            var toggles = togglesFile.Elements().ToDictionary(e => e.Name.LocalName);
            var features = new T();

            toggles.Keys.ToList()
                .ForEach(
                    k =>
                        features.GetType()
                            .GetProperty(k, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                            .SetValue(features,
                                new Feature(toggles[k].Attribute("name").Value,
                                    toggles[k].Attribute("value").Value.Equals("on", StringComparison.OrdinalIgnoreCase))));

            return features;
        }
    }

    public class Feature
    {
        private readonly string _name;
        private readonly bool _state;

        public Feature(string name, bool state)
        {
            _name = name;
            _state = state;
        }

        public bool IsOn()
        {
            return _state;
        }

        public string Name()
        {
            return _name;
        }
    }
}