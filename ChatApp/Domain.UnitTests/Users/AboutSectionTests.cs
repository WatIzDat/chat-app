using Domain.Users;
using SharedKernel;

namespace Domain.UnitTests.Users;

public class AboutSectionTests
{
    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        Result<AboutSection> result = AboutSection.Create("This is a test.");

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_ReplaceLineEndings()
    {
        Result<AboutSection> result = AboutSection.Create("This is a test\n");

        result.IsSuccess.Should().BeTrue();

        result.Value.Should().Be(AboutSection.Create("This is a test\r\n").Value);
    }

    [Fact]
    public void Create_Should_ReturnTooLong_WhenAboutSectionIsLongerThanMaxLength()
    {
        Result<AboutSection> result = AboutSection.Create("This about section is too long aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

        result.Error.Should().Be(AboutSectionErrors.TooLong);
    }
}
