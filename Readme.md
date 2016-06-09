# RollbarDotNet

Rollbar support for your .NET Core projects, relies on dependency injection and hooks up to your ASP.NET Core pipeline for easy 

Inspired by RollbarSharp, great library, just required too many tweaks to make play with .NET core well in my opinion.

# Required services

Please make sure the following services are available for the various builder modules.

``` csharp
app.UseRollbarExceptionHandler();
```

# Using in ASP.NET Core

Place the following after your error handling code in Startup.Configure:

``` csharp
app.UseRollbarExceptionHandler();
```

Place the following in your Startup.ConfigureServices section:

``` csharp
services.AddRollbarWeb();
services.AddSingleton(Configuration); // Add IConfigurationRoot service.
```

There is also one that doesn't load the builders for building out environment information for web servers (this will not attempt to crawl for server/client/request information):

``` csharp
services.AddRollbar();
services.AddSingleton(Configuration); // Add IConfigurationRoot service.
```

## Getting Occurrence UUIDs

Getting the occurrence UUID is easy, just get it from the HttpContext Feature collection:

``` csharp
public IActionResult Error()
{
    var response = HttpContext.Features.Get<IRollbarResponseFeature>();
    return Content(response.Uuid);
}
```

The UUID can be looked up directly via https://rollbar.com/occurrence/uuid/?uuid=[UUID HERE]. This may be really useful if you want to let your users report errors to you, you can include this UUID automatically in the report.

You can check if Rollbar has reported the exception via the IRollbarResponseFeature.Handled boolean.

# Calling Directly

You can also post messages/exceptions directly if you so wish.

``` csharp
// Send an exception
var response = await this.Rollbar.SendException(exception);
response.Uuid //Event UUID that can be looked up on the rollbar site.


// Send a message
var response = await this.Rollbar.SendMessage("Hello World!", RollbarLevels.Message);
```



# Building your own builders



## To do

### Implement stack frames

As of writing this .NET Core does not support walking the stack frames of the exception, means our error messages are pretty weak.

### Log4net support

As far as I know log4net is _currently_ implementing .NET Core support.

### Break out into seperate libraries?

.NET Core is all about keeping things slim, do we put ASPNETCore code in a different lib?

### .NET 4.5.1 support

Would be nice for this to support .NET 4.5.1, no testing and no real effort outside of some basic preprocessor stuff in place.