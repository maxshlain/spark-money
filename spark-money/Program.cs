using BlazorSpark.Library.Environment;
using Serilog;
using Serilog.Events;
using spark_money.Application.Startup;

EnvManager.LoadConfig();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, c) =>
	c.Enrich.FromLogContext()
		.MinimumLevel.Debug()
		.WriteTo.Console()
		// Add Sentry integration with Serilog
		// Two levels are used to configure it.
		// One sets which log level is minimally required to keep a log message as breadcrumbs
		// The other sets the minimum level for messages to be sent out as events to Sentry
		.WriteTo.Sentry(s =>
		{
			s.Dsn = "https://34711fa19c7d4a65bd4d807cf5a89fda@o4505341641621504.ingest.sentry.io/4505341643784192";
			s.MinimumBreadcrumbLevel = LogEventLevel.Debug;
			s.MinimumEventLevel = LogEventLevel.Warning;
		}));

builder.Configuration.AddEnvironmentVariables();

// Add all services to the container.
builder.Services.AddAppServices(builder.Configuration);
builder.WebHost.UseSentry(options =>
{
	options.Dsn = "https://34711fa19c7d4a65bd4d807cf5a89fda@o4505341641621504.ingest.sentry.io/4505341643784192";
	options.Debug = true;
	options.TracesSampleRate = 1.0;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
	// Do something only in dev environments
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Services.RegisterScheduledJobs();
app.Services.RegisterEvents();

app.Run();
