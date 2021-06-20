using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Identity.Utils
{
    public static class About
    {
        public static string GetHtmlWelcomePage()
        {
            var body = string.Join(
                "<br />",
                new string[]
                {
                    $"{DateTimeOffset.Now.ToString()}/{IdentityInfo.Configuration}",
                    "<br />",
                    $"{IdentityInfo.Product}@v{IdentityInfo.InformationalVersion}",
                    $" {IdentityInfo.Description}",
                    $"<a href=\"{IdentityInfo.RepositoryUrl}\">{IdentityInfo.RepositoryUrl}</a>",
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