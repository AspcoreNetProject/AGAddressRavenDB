using AGAddressRavenDB.Persistence;
using Microsoft.OpenApi.Models;
using static AGAddressRavenDB.Controllers.PersonController;
using AutoMapper;
using AGAddressRavenDB.PersonDetail;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Runtime.Serialization;

namespace AGAddressRavenDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(Personprofile));
            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
             builder.Services.AddSwaggerGen();
            //builder.Services.AddSwaggerGen(c =>
            //{
            //   // c.SwaggerDoc("v1", new OpenApiInfo { Title = "swaggertest", Version = "v1" });
            //    //c.SchemaFilter<MySwaggerSchemaFilter>();
            //});
            builder.Services.AddSingleton<IRavenDBContext, RavenDbConext>();
            builder.Services.Configure<PersistenceSettings>(builder.Configuration.GetSection("Database"));
            builder.Services.AddSingleton (typeof(IRepository<>), typeof(RavenDbRepository<>) );
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

            //void RegisterServices(IServiceCollection services)
            //{
            //    services.Configure<PersistenceSettings>(builder.Configuration.GetSection("Database"));
            //    services.AddSingleton<ProductService>();
            //    services.AddSingleton(typeof(IRavenDBRepository<>), typeof(RavenDbRepository<>));
            //    services.AddSingleton<IRavenDBContext, RavenDBContext>();
            //    services.AddSingleton(new MapperConfiguration(config =>
            //    {
            //        config.CreateMap<ProductModel, Product>();
            //        config.CreateMap<Product, ProductModel>();
            //    }).CreateMapper());
            //    services.AddEndpointsApiExplorer();
            //    services.AddSwaggerGen(c =>
            //    {
            //        c.SwaggerDoc("v1", new OpenApiInfo
            //        {
            //            Title = "Shop",
            //            Description = "Shop administration",
            //            Version = "v1"
            //        });
            //    });
            //}
        }
    }

    public class PersistenceSettings
    {
        public string? DatabaseName { get; set; }
        public string[]? Urls { get; set; }
    }

    public class Personprofile : Profile
    {
        public Personprofile()
        {
            CreateMap<PersonToPut, Person>();
            CreateMap<PersonToGet, Person>();
            CreateMap<Person, PersonToGet>();
            CreateMap<Person, PersonToPut>();
        }


    }
    public class MySwaggerSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null)
            {
                return;
            }

            var ignoreDataMemberProperties = context.Type.GetProperties()
                .Where(t => t.GetCustomAttribute<IgnoreDataMemberAttribute>() != null);

            foreach (var ignoreDataMemberProperty in ignoreDataMemberProperties)
            {
                var propertyToHide = schema.Properties.Keys
                    .SingleOrDefault(x => x.ToLower() == ignoreDataMemberProperty.Name.ToLower());

                if (propertyToHide != null)
                {
                    schema.Properties.Remove(propertyToHide);
                   //schema.Properties.IsReadOnly(schema.Properties[ignoreDataMemberProperty.Name]);
                    //schema.Properties.i;
                }
            }
        }
    }
}