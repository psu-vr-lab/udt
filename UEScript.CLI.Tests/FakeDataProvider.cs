using Bogus;
using UEScript.CLI.Models;

namespace UEScript.CLI.Tests;

public static class FakeDataProvider
{
    public static IEnumerable<UnrealEngineAssociation> GetUnrealEngines()
    {
        return SetupUnrealEngineAssociationFaker().Generate(Random.Shared.Next(2, 10));
    }

    public static UnrealEngineAssociation GetUnrealEngine()
    {
        return SetupUnrealEngineAssociationFaker().Generate();
    }

    private static Faker<UnrealEngineAssociation> SetupUnrealEngineAssociationFaker()
    {
        return new Faker<UnrealEngineAssociation>()
            .RuleFor(association => association.Name, faker => faker.Company.CompanyName())
            .RuleFor(association => association.Path, faker => faker.System.FileName())
            .RuleFor(association => association.Version, faker => faker.Random.Enum<UnrealEngineVersion>());
    }
}