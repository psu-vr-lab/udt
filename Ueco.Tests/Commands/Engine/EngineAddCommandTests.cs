using Microsoft.Extensions.Logging;
using Moq;
using Ueco.Commands.Engine.Add;
using Ueco.Models;
using Ueco.Services;

namespace Ueco.Tests.Commands.Engine;

public class EngineAddCommandTests
{
    private readonly ILogger _logger = new LoggerFactory().CreateLogger("Engine.ListCommand");
    private readonly Mock<IUnrealEngineAssociationRepository> _unrealEngineAssociationRepository = new Mock<IUnrealEngineAssociationRepository>();

    [Fact]
    public void ShouldFailWhenDirectoryIsNotUnrealEngine()
    {
        // Arrange
        var unrealEngineAssociation = FakeDataProvider.GetUnrealEngine();
        
        // Act
        var result = AddCommand.Execute(
            unrealEngineAssociation.Name, 
            new FileInfo(unrealEngineAssociation.Path), 
            unrealEngineAssociation.IsDefault, 
            _unrealEngineAssociationRepository.Object, 
            _logger);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(AddCommandError.DirectoryHasWrongName(unrealEngineAssociation.Path).ToString(), result.GetErrors().First().ToString());
    }
}