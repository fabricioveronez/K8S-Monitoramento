using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Metricas.Api
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

            services.AddScoped(s =>
            {                
                MongoClient mongoClient = new MongoClient($"mongodb://{Environment.GetEnvironmentVariable("MONGO_USERNAME")}:{Environment.GetEnvironmentVariable("MONGO_PASSWORD")}@{Environment.GetEnvironmentVariable("MONGO_URL")}:27017/admin");
                IMongoDatabase database = mongoClient.GetDatabase("admin");
                return database.GetCollection<Pedido>("pedido");
            });

            var metrics = new MetricsBuilder()
                              .Configuration.Configure(
                                  options =>
                                  {
                                      options.AddServerTag();
                                      options.AddEnvTag();
                                      options.AddAppTag();
                                  })
                              .OutputMetrics.AsPrometheusPlainText()
                              .Build();

            services.AddMetrics(metrics);
            services.AddMetricsReportingHostedService();
            services.AddMetricsEndpoints();
            services.AddMetricsTrackingMiddleware();
            services.AddMvc().AddMetrics();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMetricsAllEndpoints();
            app.UseMetricsAllMiddleware();
            app.UseMvc();
        }
    }
}
