using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Identity.Utils
{
    public static class AboutUtils
    {
        public const string ProjectUrl = "https://identity.mariomenjr.com/";

        public static string GetHtmlWelcomePage(string hostName)
        {
            var body = string.Join(
                "<br />",
                new string[]
                {
                    $"{DateTimeOffset.Now.ToString()}/{IdentityInfo.Configuration}",
                    string.Empty,
                    $"{IdentityInfo.Product}@v{IdentityInfo.InformationalVersion}",
                    $" {IdentityInfo.Description}",
                    $"<a href=\"{IdentityInfo.RepositoryUrl}\">{IdentityInfo.RepositoryUrl}</a>",
                    string.Empty, 
                    "Discovery document",
                    $"<a href=\"{hostName}.well-known/openid-configuration\">{hostName}.well-known/openid-configuration</a>"
                }
            );
            
            return $"<html><body>{body}</body></html>";
        }
        
        private static class IdentityInfo
        {
            public static string Product { get { return GetExecutingAssemblyAttribute<AssemblyProductAttribute>(a => a.Product); } }
            public static string Description { get { return GetExecutingAssemblyAttribute<AssemblyDescriptionAttribute>(a => a.Description); } }
            public static string Configuration { get { return GetExecutingAssemblyAttribute<AssemblyConfigurationAttribute>(a => a.Configuration); } }
            public static string RepositoryUrl { get { return GetExecutingAssemblyAttribute<AssemblyMetadataAttribute>(a => a.Value); } }
            public static string InformationalVersion { get { return GetExecutingAssemblyAttribute<AssemblyInformationalVersionAttribute>(a => a.InformationalVersion); } }
            // public static Version Version { get { return Assembly.GetExecutingAssembly().GetName().Version; } }

            private static string GetExecutingAssemblyAttribute<T>(Func<T, string> value) where T : Attribute
            {
                T attribute = (T)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T));
                return value.Invoke(attribute);
            }
        }
    }

   
}