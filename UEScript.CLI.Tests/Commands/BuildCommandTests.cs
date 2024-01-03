using Microsoft.Extensions.Logging;
using Moq;
using UEScript.CLI.Commands;
using UEScript.CLI.Commands.Build;
using UEScript.CLI.Models;
using UEScript.CLI.Services;
using UEScript.Utils.Results;

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
        _unrealBuildToolService.Setup(mock
                => mock.Build(It.IsAny<FileInfo>(), It.IsAny<UnrealEngineAssociation>()))
            .Returns(Result<string, CommandError>.Ok("Unreal Engine project was built"));

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
        _unrealBuildToolService.Setup(mock
                => mock.Build(It.IsAny<FileInfo>(), It.IsAny<UnrealEngineAssociation>()))
            .Returns(Result<string, CommandError>.Ok("Unreal Engine project was built"));
        
        // Arrange
        var uprojectFile = new FileInfo("TestData/EmptyUProject/");

        // Act
        var result = BuildCommand.Execute(uprojectFile, _unrealBuildToolService.Object, _logger);

        // Assert
        Assert.True(result.IsSuccess);
    }
}