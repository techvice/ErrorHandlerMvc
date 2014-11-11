ErrorHandlerMvc
by Daniel Calkins http://dcalkins.com - dcalkins@outlook.com

Forked from NotFoundMvc
by Andrew Davey - http://aboutcode.net/ - andrew@equin.co.uk

Provides a user-friendly error page whenever a controller, action or route is not found or encounters an error in your ASP.NET MVC5 application.
A view called NotFound or Error is rendered instead of the default ASP.NET error page.

Add via nuget using:
Install-Package ErrorHandlerMvc

Take a look at the sample application for basic usage. Essentially you just have to reference the ErrorHandlerMvc assembly and add a NotFound and Error view to your app.

ErrorHandlerMvc automatically installs itself during web application start-up. 

It handles all the different ways a 404 HttpException is usually thrown by ASP.NET MVC. This includes a missing controller, action and route. 
It will also handle any exceptions thrown from your code during actions to prevent the default ASP.NET error page.

A catch-all route is added to the end of RouteTable.Routes.
The controller factory is wrapped to catch when controller is not found or encounters an error.
The action invoker of Controller is wrapped to catch when the action method is not found or encounters an error.
