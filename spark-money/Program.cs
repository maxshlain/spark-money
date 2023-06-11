using BlazorSpark.Library.Environment;
using spark_money.Application.Startup;

EnvManager.LoadConfig();

var builder = WebApplication.CreateBuilder(args);
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
