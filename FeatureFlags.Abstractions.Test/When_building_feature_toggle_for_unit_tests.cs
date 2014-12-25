using FluentAssertions;
using NUnit.Framework;

namespace FeatureFlags.Abstractions.Test
{
    [TestFixture]
    public class When_building_feature_toggle_for_unit_tests
    {

        [Test]
        public void should_be_able_to_set_feature_value()
        {
            var baklavaAppFeatures = new BaklavaAppFeatures
            {
                IceCream = FeatureWith.StateOn() 
            };

            baklavaAppFeatures.IceCream.IsOn().Should().BeTrue();
        }

        [Test]
        public void should_be_able_turn_off_a_feature()
        {
            var baklavaAppFeatures = new BaklavaAppFeatures
            {
                Pistachio = FeatureWith.StateOff()
            };

            baklavaAppFeatures.Pistachio.IsOn().Should().BeFalse();
        }
    }
}
