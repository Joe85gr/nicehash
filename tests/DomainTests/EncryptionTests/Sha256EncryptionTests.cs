using Bogus;
using Domain.Encryption;
using TestHelpers;

namespace DomainTests.EncryptionTests;

public class Sha256EncryptionTests
{
    public Sha256EncryptionTests()
    {
        Helper.ConfigureFakeEnvironmentalVariables();
    }
    [Fact]
    public void GenerateTextToHash_ParsesTextCorrectly()
    {
        // Arrange
        var faker = new Faker();
        var time = faker.Random.Int().ToString();
        var endpoint = faker.Random.Word();
        var parameter = faker.Random.Word();
        var guid = faker.Random.Guid().ToString();
        var method = HttpMethod.Get;
        var hashStructure = new HashStructure(time, $"{endpoint}?{parameter}", method, guid);
        var apiKey = Environment.GetEnvironmentVariable("NICEHASH_API_KEY");
        var orgId = Environment.GetEnvironmentVariable("NICEHASH_ORG_ID");
        // Act
        var expectedText = $"{apiKey} {time} {guid}  {orgId}  {method.ToString().ToUpper()} {endpoint} {parameter}";
        var actualText = Sha256Encryption.GenerateTextToHash(hashStructure);
        // Assess
        Assert.Equal(expectedText, actualText);
    }
    
    [Fact]
    public void GenerateHash_ParsesTextCorrectly()
    {
        // Arrange
        const string text = " 12345678 some_guid    GET FancyEndpoint fancyParameter=fancyValue";
        // Note: do not change this as it will affect the expected hash!
        const string key = "some_fancy_key";
        // Act
        const string expectedHash = "127d2a27d6bdf794dfe1a11ce7c90efb46432bf76f04af5d4df29057079120c2";
        var actualHash = Sha256Encryption.GenerateHash(text, key);
        // Assess
        Assert.Equal(expectedHash, actualHash);
    }
}