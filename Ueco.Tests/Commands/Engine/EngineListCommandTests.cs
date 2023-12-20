using Microsoft.Extensions.Logging;
using Moq;
using Ueco.Commands.Engine.List;
using Ueco.Models;
using Ueco.Services;

namespace Ueco.Tests.Commands.Engine;

public class EngineListCommandTests
{
    private readonly ILogger _logger = new LoggerFactory().CreateLogger("Engine.ListCommand");
    private readonly Mock<IUnrealEngineAssociationRepository> _unrealEngineAssociationRepository = new Mock<IUnrealEngineAssociationRepository>();

    [Fact]
    public void ShouldFailWhenNoEnginesAreFound()
    {
        // Arrange
        _unrealEngineAssociationRepository
            .Setup(repository => repository.GetUnrealEngines())
            .Returns(Array.Empty<UnrealEngineAssociation>());
        
        // Act
        var result = ListCommand.Execute(_unrealEngineAssociationRepository.Object, _logger);
        
        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void ShouldSuccessWhenEnginesAreFound()
    {
        // Arrange
        _unrealEngineAssociationRepository
            .Setup(repository => repository.GetUnrealEngines())
            .Returns(FakeDataProvider.GetUnrealEngines());
        
        // Act
        var result = ListCommand.Execute(_unrealEngineAssociationRepository.Object, _logger);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
}