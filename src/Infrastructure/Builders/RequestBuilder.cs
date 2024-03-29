using Domain.Encryption;

namespace Infrastructure.Builders;

public class RequestBuilder
{
    private readonly HttpRequestMessage _request = new();

    public RequestBuilder(string baseUrl, string endpoint)
    {
        _request.RequestUri = new Uri(baseUrl + endpoint);
    }
    
    public RequestBuilder WithHeaders(string serverTime, HashStructure hashStructure)
    {
        var text = Sha256Encryption.GenerateTextToHash(hashStructure);
        var encryptedHash = Sha256Encryption.GenerateHash(text, hashStructure.ApiSecret);
    
        _request.Headers.Add("X-Time", serverTime);
        _request.Headers.Add("X-Nonce", hashStructure.Nonce);
        _request.Headers.Add("X-Auth", hashStructure.ApiKey + ":" + encryptedHash);
        _request.Headers.Add("X-Organization-Id", hashStructure.OrgId);

        return this;
    }
    
    public RequestBuilder WithMethod(HttpMethod method)
    {
        _request.Method = method;
        
        return this;
    }
    
    public HttpRequestMessage Build() => _request;
}