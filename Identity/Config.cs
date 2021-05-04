using IdentityServer4.Models;
using System.Collections.Generic;

namespace Identity
{
  public static class Config
  {
    public static IEnumerable<ApiScope> ApiScopes =>
      new List<ApiScope>
      {
          new ApiScope("continuee_api", "Continuee API")
      };

    public static IEnumerable<Client> Clients =>
      new List<Client>
      {
        new Client
        {
            ClientId = "ContinueeServer",

            // no interactive user, use the clientid/secret for authentication
            AllowedGrantTypes = GrantTypes.ClientCredentials,

            // secret for authentication
            ClientSecrets =
            {
                new Secret("ContinueeSecret".Sha256())
            },

            // scopes that client has access to
            AllowedScopes = { "continuee_api" }
        }
      };
  }

}