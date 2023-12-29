using Domain.Users;
using SharedKernel;

namespace Domain.UnitTests.Users;

public class EmailTests
{
    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        Result<Email> result = Email.Create("test@test.com");

        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_Should_ReturnEmailErrorsEmpty_WhenEmailIsNullOrEmpty(string value)
    {
        Result<Email> result = Email.Create(value);

        result.Error.Should().Be(EmailErrors.Empty);
    }

    [Theory]
    [InlineData("testtest")]
    [InlineData("test@test.")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@testttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt.com")]
    public void Create_Should_ReturnInvalidFormat_WhenEmailHasInvalidFormat(string value)
    {
        Result<Email> result = Email.Create(value);

        result.Error.Should().Be(EmailErrors.InvalidFormat);
    }
}
