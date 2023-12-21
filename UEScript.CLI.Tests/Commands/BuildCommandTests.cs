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
    public void ShouldFail_WhenPathToDirIsInvalid_ProvidedAsPathToDir()
    {
        // Arrange
        var uprojectFile = new FileInfo("UprojectFileNotFound");
        
        // Act
        var result = BuildCommand.Execute(uprojectFile, _unrealBuildToolService.Object, _logger);
        
        // Assert
        Assert.False(result.IsSuccess);
    }
    
    [Fact]
    public void ShouldFail_WhenUprojectFileNotFound_ProvidedAsPathToUProjectFile()
    {
        // Arrange
        var uprojectFile = new FileInfo("TestData/UprojectFileNotFound.uproject");
        
        // Act
        var result = BuildCommand.Execute(uprojectFile, _unrealBuildToolService.Object, _logger);
        
        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void ShouldSuccess_WhenUprojectFileFound_ProvidedAsFullPathToUProject()
    {
        // Arrange
        var uprojectFile = new FileInfo("TestData/EmptyUProject/Empty.uproject");
        
        // Act
        var result = BuildCommand.Execute(uprojectFile, _unrealBuildToolService.Object, _logger);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void ShouldSuccess_WhenUprojectFileFound_ProvidedAsPathToDirContainsUProject()
    {
        // Arrange
        var uprojectFile = new FileInfo("TestData/EmptyUProject/");
        
        // Act
        var result = BuildCommand.Execute(uprojectFile, _unrealBuildToolService.Object, _logger);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
}