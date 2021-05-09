using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Test;

namespace Identity
{
    public static class Config
    {
        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "2000 Hola Mundo Blvd",
                    city = "Fullerton",
                    postal_code = 90000,
                    country = "United States of America"
                };

                return new List<TestUser>()
                {
                    new TestUser()
                    {
                        SubjectId = "12345",
                        Username =  "mariomenjr",
                        Password = "password",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Mario Menjivar"),
                            new Claim(JwtClaimTypes.GivenName, "mariomenjr"),
                            new Claim(JwtClaimTypes.Email, "mariomenjr@gmail.com"),
                        }
                    }
                };
            }
        }
        
        public static IEnumerable<IdentityResource> IdentityResources => new[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource()
            {
                Name = "role",
                UserClaims = new List<string> {"role"}
            }
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope("continuee.chain.read", "Read Continuee Chains"),
            new ApiScope("continuee.chain.write", "Write Continuee Chains")
        };

        public static IEnumerable<ApiResource> ApiResources() => new List<ApiResource>
        {
            new ApiResource(name: "continuee_api")
            {
                Scopes = new List<string> {"continuee.chain.read", "continuee.chain.write"},
                ApiSecrets = new List<Secret> {new Secret("ContinueeApiSecret".Sha256())},
                UserClaims = new List<string> {"role"}
            }
        };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "continuee.server",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("ContinueeSecret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "continuee.chain.read", "continuee.chain.write" }
                }
            };
    }
}