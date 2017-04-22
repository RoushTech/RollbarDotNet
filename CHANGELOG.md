# 0.5.2
<sup>Released: 2017/4/21</sup>

## Features

 - Include symbols in package. #55
 - Updated documentation. #49
 - Can now disable Rollbar from configuration. #48

## Bug Fixes

 - Fix issue with trying to access Request.Form variables during POSTs that may not actually support form variables. #54

# 0.5.1
<sup>Released: 2017/3/10</sup>

## Features

 - More robust testing of sending exceptions to Rollbar (see readme.md for instructions on configuring token for testing).

## Bug Fixes

 - Fixed issue sending Rollbar exceptions.

# 0.5.0
<sup>Released: 2017/2/26</sup>

## Features

- IHttpContextAccessor is no longer assumed to be included as part of `AddIdentity()`, we'll add it.
- Testing of dependency injection for web platforms (console apps coming soon).
- Switching to the use of `IOptions<RollbarOptions>` instead of `IConfigurationRoot`.
- Testing for SendMessage calls.
- Person record support (thank you mkdabrowski).
- Exception builder is now injectable.

## Bug Fixes

- Sending a message to Rollbar no longer results in a null reference exception being thrown.


# 0.4.0
<sup>Released: 2016/9/30</sup>

## Features

- Upgrade for .NET Core 1.0 release.

# 0.3.0
<sup>Released: 2016/6/12</sup>

## Features

- Added support for cookies on request body.
- Added blacklisting variables by name.

# 0.2.0
<sup>Released: 2016/6/9</sup>

## Features

- Rollbar message UUID available after error is reported.
- Removed custom parameters from message body.
- Documentation for required configuration variables 


# 0.1.0
<sup>Released: 2016/5/31</sup>

Initial release.

## Features

- Ability to send basic logs to Rollbar
