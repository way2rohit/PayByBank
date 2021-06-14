using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pokedex.Application;
using Pokedex.Domain;
using Pokedex.Domain.Exceptions;
using Pokedex.Domain.Models;
using System;
using System.IO;
using System.Reflection;

namespace Pokedex
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddOptions();
			services.AddHttpClient();

			services.Configure<AppSettings>(Configuration);
			services.AddSingleton<IAppProcessor, AppProcessor>();
			services.AddSingleton<IService, Service>();

			// Set the comments path for the Swagger JSON and UI.
			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen(swagger =>
			{
				swagger.DescribeAllParametersInCamelCase();
				swagger.SwaggerDoc("v1", new OpenApiInfo { Title = nameof(Pokedex), Version = "v1" });
				//swagger.IncludeXmlComments(xmlPath);
			});


			//AddControllers 
			services.AddControllers(options => options.Filters.Add(new HttpResponseExceptionFilter()));


		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				//app.UseDeveloperExceptionPage();
				app.UseExceptionHandler("/error/errorlocaldevelopment");
			}
			else
			{
				app.UseExceptionHandler("/error/error");
			}

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{nameof(Pokedex)} v1 - {env.EnvironmentName}");
			});

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
