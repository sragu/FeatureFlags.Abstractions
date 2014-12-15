using System;
using FluentAssertions;
using NUnit.Framework;

namespace FeatureFlags.Abstractions.Test
{
    [TestFixture]
    public class When_feature_flags_expire
    {
        private readonly ExpiringAppFeatures features;

        public When_feature_flags_expire()
        {
            features = FeatureFlags.Map<ExpiringAppFeatures>("featureTogglesExpiration.xml");
        }

        [Test]
        public void feature_is_expired_if_no_date_is_set()
        {
            features.IceCream.Expired(DateTime.Today).Should().BeTrue("by default feature is expired");
        }

        [Test]
        public void feature_toggle_is_not_expired_until_the_date_mentioned()
        {
            features.Chai.Expired(new DateTime(2014, 12, 12)).Should().BeFalse();   
        }

        [Test]
        public void feature_is_expires_on_the_day()
        {
            features.Chai.Expired(new DateTime(2014, 12, 14)).Should().BeTrue("this feature expires on 12/14");
        }

        [Test]
        public void feature_toggle_past_its_date_is_expired()
        {
            features.Chai.Expired(new DateTime(2014, 12, 25)).Should().BeTrue("this feature expires on 12/14");
        }
    }

    internal class ExpiringAppFeatures 
    {
        public Feature IceCream { get; internal set; }
        public Feature Chai { get; internal set; }
    }
}
