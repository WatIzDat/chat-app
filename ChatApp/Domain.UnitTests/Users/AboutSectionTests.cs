using Domain.Users;
using SharedKernel;

namespace Domain.UnitTests.Users;

public class AboutSectionTests
{
    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        // Arrange
        string validValue = "This is a test.";

        // Act
        Result<AboutSection> result = AboutSection.Create(validValue);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_ReplaceLineEndings()
    {
        // Arrange
        string unnormalizedLineEndings = "This is a test\n";

        // Act
        Result<AboutSection> result = AboutSection.Create(unnormalizedLineEndings);

        // Assert
        string normalizedLineEndings = "This is a test\r\n";

        result.Value.Should().Be(AboutSection.Create(normalizedLineEndings).Value);
    }

    [Fact]
    public void Create_Should_ReturnTooLong_WhenAboutSectionIsLongerThanMaxLength()
    {
        // Arrange
        string longerThanMaxLength = string.Empty.PadLeft(AboutSection.MaxLength + 1);

        // Act
        Result<AboutSection> result = AboutSection.Create(longerThanMaxLength);

        // Assert
        result.Error.Should().Be(AboutSectionErrors.TooLong);
    }
}
