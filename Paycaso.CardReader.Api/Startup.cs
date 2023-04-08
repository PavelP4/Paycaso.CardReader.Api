using FluentValidation;
using FluentValidation.AspNetCore;
using NLog.Extensions.Logging;
using Paycaso.CardReader.Application.Queries.GetCard;
using System.Reflection;
using MediatR;
using Paycaso.CardReader.Application.Commands.SetCardBalance;
using Paycaso.CardReader.Api.RequestBehaviours;
using Paycaso.CardReader.Application.Managers;
using Paycaso.CardReader.Api.HostedServices;
using Paycaso.CardReader.Api.Middlewares;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace Paycaso.CardReader.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options => options
                .AddDefaultPolicy(builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    ));

            services.AddHealthChecks();

            services.AddLogging(config =>
            {
                config.ClearProviders();
                config.AddNLog();
            });

            services.AddSwaggerGen(options =>
            {
                options.IncludeXmlComments(Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "xml"), includeControllerXmlComments: true);
            });

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<SetCardBalanceCommandValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetCardQueryHandler).GetTypeInfo().Assembly));

            services.AddSingleton<ICardReaderManager, CardReaderManager>();

            services.AddWindowsService(options =>
            {
                options.ServiceName = "Paycaso.CardReader.Api";
            });
            services.AddHostedService<CardReaderHostedService>();

            services.AddSingleton(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigureExceptionHadler(app);

            app.UseRouting();
            app.UseCors();

            if (IsHttpsConfigured)
            {
                app.UseHttpsRedirection();
            }

            app.UseMiddleware<KnownExceptionsHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        private bool IsHttpsConfigured => !string.IsNullOrEmpty(Configuration["Kestrel:Endpoints:Https:Url"]);

        private void ConfigureExceptionHadler(IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (exceptionHandlerPathFeature?.Error is ValidationException validationException)
                    {
                        context.Response.StatusCode = 400;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            validationException.Message,
                            validationException.Errors
                        }));
                    }
                    else
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            context.TraceIdentifier,
                            Message = exceptionHandlerPathFeature?.Error.ToString()
                        }));
                    }
                });
            });
        }
    }
}