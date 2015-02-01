FeatureFlags.Abstractions
=========================

abstraction over feature flags for applications build with .net

A simple, small code to handle feature flags. No impressive features, but will be dead simple to use.


__Install__

````
PM> Install-Package FeatureFlags.Abstractions
````

[Nuget Relase Latest](https://www.nuget.org/packages/FeatureFlags.Abstractions)


__Configure the toggles (baklavaFeatures.xml)__

````
<?xml version="1.0" encoding="utf-8" ?>
<featureToggles>
  <IceCream name="ice-cream on baklava" value="on" expires="2014-12-12"/>
</featureToggles>

````
__Use the toggles to manage features__
````
 var features = FeatureFlags.Map<BaklavaAppFeatures>("baklavaFeatures.xml");
 
 if(features.IceCream.IsOn())
 {
    //best combination ever
 }
 
 public class BaklavaAppFeatures
 {
    public Feature IceCream { get; internal set; }
 }
````

Feature toggle expiration acts as a reminder to remove them, or to the extend them conciously. Feature toggle should be dead, long live the feature toggles.

````
[Test]
public void icecream_feature_is_set()
{
    features.IceCream.Expired(DateTime.Today)
    	.Should().BeFalse("time to get rid off this toggle, and make the feature default");
}
````

Also you can set feature toggle value programmaticaly for testing purposes.

````    
    var features = new BaklavaAppFeatures {Chai = FeatureWith.StateOn(), IceCream = FeatureWith.StateOff()};
	
````

* Expiration does not affect the value of the feature toggle.

__TODO__:

* add support to derive value from request cookies

__NOTES__:

* project naming is inspired by [System.Configuration.Abstractions](https://github.com/davidwhitney/System.Configuration.Abstractions)
