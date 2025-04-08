using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ECommerceChatbot.Hubs;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.IO;
using System.Threading.Tasks;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ✅ Load config
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

        var configuration = builder.Configuration;
        var openAiKey = configuration["OpenAI:ApiKey"] ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        // ✅ Services
        builder.Services.AddControllersWithViews(); // Required for Views and TempData
        builder.Services.AddSignalR();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ECommerce Chatbot API",
                Version = "v1",
                Description = "API for chatbot interactions in an e-commerce platform.",
                Contact = new OpenApiContact
                {
                    Name = "Support",
                    Email = "support@example.com",
                    Url = new Uri("https://example.com/support")
                }
            });
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());
        });

        // ✅ OpenAI client setup
        if (!string.IsNullOrEmpty(openAiKey))
        {
            builder.Services.AddHttpClient("OpenAI", client =>
            {
                client.BaseAddress = new Uri("https://api.openai.com/v1/");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiKey}");
            });
        }
        builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider(); // ✅ for TempData support

        builder.Services.AddSession();
        var app = builder.Build();

        // ✅ Middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce Chatbot API v1");
                c.RoutePrefix = "swagger";
            });
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthorization();

        // ✅ MVC route
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // ✅ SignalR
        app.MapHub<ChatHub>("/chatHub");

        // ✅ Redirect root to Home
        app.MapGet("/", context =>
        {
            context.Response.Redirect("/Home");
            return Task.CompletedTask;
        });

        // ✅ Runtime check for TempData services
        var sp = app.Services.CreateScope().ServiceProvider;
        var tempDataFactory = sp.GetService<ITempDataDictionaryFactory>();

        if (tempDataFactory == null)
        {
            Console.WriteLine("❌ ITempDataDictionaryFactory NOT registered!");
        }
        else
        {
            Console.WriteLine("✅ ITempDataDictionaryFactory is registered.");
        }

        app.Run();
    }
}
