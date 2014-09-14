using FluentAssertions;
using NUnit.Framework;

namespace FeatureFlags.Abstractions.Test
{
    [TestFixture]
    public class When_reading_featureflags
    {
        [Test]
        public void should_read_value_for_feature_flag()
        {
            var features = FeatureFlags.Map<BaklavaAppFeatures>();

            features.IceCream.IsOn().Should().Be(true);
            features.IceCream.Name().Should().Be("ice-cream on baklava");
        }
    }

    public class BaklavaAppFeatures
    {
        public Feature IceCream { get; internal set; }
    }
}
