using Microsoft.Extensions.Logging;
using Ueco.Commands.Build;

namespace Ueco.Tests.Commands.Build;

public class BuildCommandTests
{
    private readonly ILogger _logger = new LoggerFactory().CreateLogger("BuildCommand");

    [Fact]
    public void ShouldFailWhenUprojectFileNotFound()
    {
        // Arrange
        var uprojectFile = new FileInfo("UprojectFileNotFound");
        
        // Act
        var result = BuildCommand.Execute(uprojectFile, _logger);
        
        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void ShouldSuccessWhenUprojectFileFound()
    {
        // Arrange
        var uprojectFile = new FileInfo("TestData/EmptyUProject/Empty.uproject");
        
        // Act
        var result = BuildCommand.Execute(uprojectFile, _logger);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
}