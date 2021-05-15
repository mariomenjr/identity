using System;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;
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
                        Password = "ThisIsATest",
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
                UserClaims = new List<string> { "role" }
            }
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope("continuee.chain.read", "Read Continuee Chains"),
            new ApiScope("continuee.chain.write", "Write Continuee Chains")
        };

        public static IEnumerable<ApiResource> ApiResources() => new List<ApiResource>
        {
            new ApiResource(name: Environment.GetEnvironmentVariable("CONTINUEE__API_RESOURCE_NAME"))
            {
                Scopes = ApiScopes.Select(s => s.Name).ToList(),
                ApiSecrets = new List<Secret> {new(Environment.GetEnvironmentVariable("CONTINUEE__API_RESOURCE_SECRET").Sha256())},
                UserClaims = new List<string> {"role"}
            }
        };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = Environment.GetEnvironmentVariable("CONTINUEE__CLIENT_ID"),
                    AccessTokenType = AccessTokenType.Jwt,

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret(Environment.GetEnvironmentVariable("CONTINUEE__CLIENT_SECRET").Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = ApiScopes.Select(s => s.Name).ToList(),
                }
            };
    }
}