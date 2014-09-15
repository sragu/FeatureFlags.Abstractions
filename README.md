FeatureFlags.Abstractions
=========================

abstraction over feature flags for applications build with .net

A simple, small code to handle feature flags. No impressive features, but will be dead simple to use.

````
 var features = FeatureFlags.Map<BaklavaAppFeatures>();
 
 if(features.IceCream.IsOn())
 {
    //best combination ever
 }
 
 public class BaklavaAppFeatures
 {
    public Feature IceCream { get; internal set; }
 }
````

TODO:

* add support for feature flag expiration
* add support to derive value from request cookies
