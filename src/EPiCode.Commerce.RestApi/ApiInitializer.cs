using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using EPiCode.Commerce.RestService.Converters;
using EPiServer.Commerce.Serialization.Json;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Mediachase.Commerce.Catalog.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using MoneyConverter = EPiCode.Commerce.RestService.Converters.MoneyConverter;

namespace EPiCode.Commerce.RestService
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ApiInitializer : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {

            var config = GlobalConfiguration.Configuration;

            // We cannot do this globally, you might do so for debugging
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            // config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            string rootUri = ConfigurationManager.AppSettings["EPiCode.Commerce.Rest.RootUri"];
            if(rootUri == null)
            {
                rootUri = "api";
            }

            config.Routes.MapHttpRoute(
                name: "ServiceApi",
                routeTemplate: rootUri + "/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // other initialization...
            var formatters = config.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            
            jsonFormatter.SerializerSettings.Converters.Add(new PrimaryKeyIdConverter());
            jsonFormatter.SerializerSettings.Converters.Add(new VariationRowConverter());
            jsonFormatter.SerializerSettings.Converters.Add(new MoneyConverter());
            jsonFormatter.SerializerSettings.Converters.Add(new EPiCode.Commerce.RestService.Converters.CustomerPricingConverter());

            // Avoid showing backing fields for objects marked with the Serializable attribute
            var contractResolver = (DefaultContractResolver)jsonFormatter.SerializerSettings.ContractResolver;
            contractResolver.IgnoreSerializableAttribute = true;

            jsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            jsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            //jsonFormatter.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            //jsonFormatter.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;


        }

        public void Preload(string[] parameters) { }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }
    }
    
}