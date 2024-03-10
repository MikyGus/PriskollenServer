using FluentAssertions;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Validators;

namespace PriskollenServer.Tests.Tests.Validators;
public class StoreChainValidatorTests
{
    private StoreChainRequest _storeChain;
    private IStoreChainValidator _sut;

    public StoreChainValidatorTests()
    {
        _storeChain = new StoreChainRequest("Testy", "TestyImg");
        _sut = new StoreChainValidator();
    }

    [Theory]
    [InlineData("1", false)]
    [InlineData("12", false)]
    [InlineData("123", true)]
    [InlineData("123456789012345678901234567890", true)]
    [InlineData("1234567890123456789012345678901", false)]
    public void StoreChainValidator_ValidateStoreChainName_ReturnIfValid(string storeChainName, bool isValid)
    {
        // Arrange
        StoreChainRequest storeChain = _storeChain with { Name = storeChainName };
        // Act
        bool result = _sut.IsValid(storeChain, out _);
        // Assert
        _ = result.Should().Be(isValid);
    }
}