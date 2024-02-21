using Microsoft.Extensions.Logging;
using Moq;
using UEScript.CLI.Commands;
using UEScript.CLI.Commands.Engine.Add;
using UEScript.CLI.Services;

namespace UEScript.CLI.Tests.Commands.Engine;

public class EngineAddCommandTests
{
    private readonly ILogger _logger = new LoggerFactory().CreateLogger("Engine.ListCommand");
    private readonly Mock<IUnrealEngineAssociationRepository> _unrealEngineAssociationRepository = new Mock<IUnrealEngineAssociationRepository>();

    [Fact]
    public void ShouldFail_WhenDirectory_IsNotUnrealEngine()
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
        Assert.Equal(CommandError.DirectoryHasWrongName(unrealEngineAssociation.Path).ToString(), result.GetError()!.ToString());
    }
}