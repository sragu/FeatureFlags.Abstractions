using System;
using FluentAssertions;
using NUnit.Framework;

namespace FeatureFlags.Abstractions.Test
{
    [TestFixture]
    public class When_reading_featureflags
    {
        private readonly BaklavaAppFeatures features;

        public When_reading_featureflags()
        {
            features = FeatureFlags.Map<BaklavaAppFeatures>("featureToggles.xml");
        }

        [Test]
        public void should_read_value_on_for_a_feature()
        {
            features.IceCream.IsOn().Should().BeTrue("should be on as per config");
        }

        [Test]
        public void should_read_value_on_for_a_feature_irrespective_of_case()
        {
            features.Chai.IsOn().Should().BeTrue("should be on as per config");
        }

        [Test]
        public void should_read_value_off_for_a_feature()
        {
            features.Walnut.IsOn().Should().BeFalse("should be off as per config");
        }

        [Test]
        public void should_get_the_feature_flag_name()
        {
            features.IceCream.Name().Should().Be("ice-cream on baklava");
        }

        [Test, Ignore("To be implemented")]
        public void should_check_the_expires_date()
        {
            features.IceCream.Expired(new DateTime(2014, 10, 13)).Should().Be(true);
        }
    }

    [TestFixture]
    public class When_reading_featureflags_that_not_set_in_config
    {
        private readonly BaklavaAppFeatures features;

        public When_reading_featureflags_that_not_set_in_config()
        {
            features = FeatureFlags.Map<BaklavaAppFeatures>("featureToggles.xml");
        }

        [Test]
        public void should_be_set_to_off()
        {
            features.Pistachio.IsOn().Should().BeFalse("feature flag that is not set in xml is off by default");
        }
    }

    [TestFixture]
    public class When_reading_featureflags_from_another_file
    {
        private readonly BaklavaAppFeatures features;

        public When_reading_featureflags_from_another_file()
        {
            features = FeatureFlags.Map<BaklavaAppFeatures>("baklavaFeatures.xml");
        }

        [Test]
        public void should_be_able_to_initialise_toggles()
        {
            features.IceCream.IsOn().Should().BeTrue("feature toggle is on");
        }
    }

    public class BaklavaAppFeatures
    {
        public Feature IceCream { get; internal set; }
        public Feature Pistachio { get; internal set; }
        public Feature Walnut { get; internal set; }
        public Feature Chai { get; internal set; }
    }
}
