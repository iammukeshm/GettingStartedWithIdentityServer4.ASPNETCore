using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebClient.Services
{
public class TokenService : ITokenService
{
    private DiscoveryDocumentResponse _discDocument {get;set;}
    public TokenService()
    {
        using(var client = new HttpClient())
        {
            _discDocument = client.GetDiscoveryDocumentAsync("https://localhost:44322/.well-known/openid-configuration").Result;
        }
    }
    public async Task<TokenResponse> GetToken(string scope)
    {
        using (var client = new HttpClient())
        {
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = _discDocument.TokenEndpoint,
                ClientId = "cwm.client",
                Scope = scope,
                ClientSecret = "secret"
            });
            if(tokenResponse.IsError)
            {
                throw new Exception("Token Error");
            }
            return tokenResponse;
        }
    }
}
}
