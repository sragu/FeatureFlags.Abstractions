FeatureFlags.Abstractions
=========================

abstraction over feature flags for applications build with .net

A simple, small code to handle feature flags. No impressive features, but will be dead simple to use.

__Configure the toggles (baklavaFeatures.xml)__

````
<?xml version="1.0" encoding="utf-8" ?>
<featureToggles>
  <IceCream name="ice-cream on baklava" value="on"/>
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

__TODO__:

* add support for feature flag expiration
* add support to derive value from request cookies