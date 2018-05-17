# Auth0.OidcClient

Example project authenticating against Auth0 using IdentityModel libraries

https://github.com/IdentityModel/IdentityModel.OidcClient2

Based on the example project in 

https://github.com/IdentityModel/IdentityModel.OidcClient.Samples/tree/master/XamarinForms

Some gotcha's.

The android project must use packages.config. At least at the time I was trying this, without it there was linking errors when deploying to Android.

This issue put me on the right path to get this working.

https://github.com/xamarin/xamarin-android/issues/1196

Requires removing the package requirements from the sln, then readding the nuget packages. Also need to remove any xforms references in the sln after this.

To add social logins i.e. google, follow https://auth0.com/docs/connections/social/google

/swagger to hit the swagger definition. Use the authorize button to put in the token: i.e. bearer <token>
  
To obtain a token, use the UWP version, once logged in the JWT token can be copied from the editbox and placed into swagger.

