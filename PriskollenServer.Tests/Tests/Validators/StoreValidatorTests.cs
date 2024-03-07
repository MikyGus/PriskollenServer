using FluentAssertions;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Validators;

namespace PriskollenServer.Tests.Tests.Validators;
public class StoreValidatorTests
{
    private StoreRequest _store;
    private IStoreValidator _sut;

    public StoreValidatorTests()
    {
        _store = new StoreRequest()
        {
            Name = "name",
            Image = "TestyImg",
            Latitude = 54.11,
            Longitude = 14.15,
            Address = "address",
            City = "TestCity",
            StoreChain_id = 42
        };
        //_store = new StoreRequest("Testy", "TestyImg", 54.22, 14.23, "Testyroad 3", "TestCity", 42);
        _sut = new StoreValidator();
    }

    [Theory]
    [InlineData("1", false)]
    [InlineData("12", false)]
    [InlineData("123", true)]
    [InlineData("123456789012345678901234567890", true)]
    [InlineData("1234567890123456789012345678901", false)]
    public void StoreValidator_ValidateStoreName_ReturnIfValid(string storeName, bool isValid)
    {
        // Arrange
        //StoreRequest store = _store with { Name = storeName };
        _store.Name = storeName;
        // Act
        bool result = _sut.IsValid(_store, out _);
        // Assert
        _ = result.Should().Be(isValid);
    }

    [Theory]
    [InlineData(-91, false)]
    [InlineData(-90.1, false)]
    [InlineData(-90, true)]
    [InlineData(0, true)]
    [InlineData(90, true)]
    [InlineData(90.1, false)]
    [InlineData(91, false)]
    public void StoreValidator_ValidateStoreLatitude_ReturnIfValid(double latitude, bool isValid)
    {
        // Arrange
        //StoreRequest store = _store with { Latitude = latitude };
        _store.Latitude = latitude;
        // Act
        bool result = _sut.IsValid(_store, out _);
        // Assert
        _ = result.Should().Be(isValid);
    }

    [Theory]
    [InlineData(-181, false)]
    [InlineData(-180.1, false)]
    [InlineData(-180, true)]
    [InlineData(0, true)]
    [InlineData(180, true)]
    [InlineData(180.1, false)]
    [InlineData(181, false)]
    public void StoreValidator_ValidateStoreLongitude_ReturnIfValid(double longitude, bool isValid)
    {
        // Arrange
        //StoreRequest store = _store with { Longitude = longitude };
        _store.Longitude = longitude;
        // Act
        bool result = _sut.IsValid(_store, out _);
        // Assert
        _ = result.Should().Be(isValid);
    }
}