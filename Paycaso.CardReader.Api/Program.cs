using Paycaso.CardReader.Api;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webbuilder =>
    {
        //webbuilder.UseKestrel();
        webbuilder.UseStartup<Startup>();
        //webbuilder.ConfigureAppConfiguration((hostingContext, config) => config.AddEnvironmentVariables("PCAPI_"));
    })
    .UseWindowsService()
    .Build();

host.Run();