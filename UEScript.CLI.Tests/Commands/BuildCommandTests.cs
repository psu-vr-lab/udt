using Microsoft.Extensions.Logging;
using Moq;
using UEScript.CLI.Commands.Build;
using UEScript.CLI.Services;

namespace UEScript.CLI.Tests.Commands;

public class BuildCommandTests
{
    private readonly ILogger _logger = new LoggerFactory().CreateLogger("BuildCommand");
    private readonly Mock<IUnrealBuildToolService> _unrealBuildToolService = new Mock<IUnrealBuildToolService>();

    [Fact]
    public void ShouldFailWhenUprojectFileNotFound()
    {
        // Arrange
        var uprojectFile = new FileInfo("UprojectFileNotFound");
        
        // Act
        var result = BuildCommand.Execute(uprojectFile, _unrealBuildToolService.Object, _logger);
        
        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void ShouldSuccessWhenUprojectFileFound()
    {
        // Arrange
        var uprojectFile = new FileInfo("TestData/EmptyUProject/Empty.uproject");
        
        // Act
        var result = BuildCommand.Execute(uprojectFile, _unrealBuildToolService.Object, _logger);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
}