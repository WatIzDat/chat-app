using Domain.Users;
using SharedKernel;

namespace Domain.UnitTests.Users;

public class EmailTests
{
    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        // Arrange
        string validValue = "test@test.com";

        // Act
        Result<Email> result = Email.Create(validValue);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_Should_ReturnEmailErrorsEmpty_WhenEmailIsNullOrEmpty(string value)
    {
        // Act
        Result<Email> result = Email.Create(value);

        // Assert
        result.Error.Should().Be(EmailErrors.Empty);
    }

    [Theory]
    [InlineData("testtest")]
    [InlineData("test@test.")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@testttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt.com")]
    public void Create_Should_ReturnInvalidFormat_WhenEmailHasInvalidFormat(string value)
    {
        // Act
        Result<Email> result = Email.Create(value);

        // Assert
        result.Error.Should().Be(EmailErrors.InvalidFormat);
    }
}
