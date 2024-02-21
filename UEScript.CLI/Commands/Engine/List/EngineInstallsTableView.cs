using Spectre.Console;
using UEScript.CLI.Models;

namespace UEScript.CLI.Commands.Engine.List;

public static class EngineInstallsTableView
{
    public static void ToTable(IEnumerable<UnrealEngineAssociation> engineAssociations)
    {
        var table = new Table();
        table.AddColumn("Name");
        table.AddColumn("Path");
        table.AddColumn("Version");

        foreach (var association in engineAssociations)
        {
            if (association.IsDefault)
            {
                table.AddRow("[green]"+association.Name + "[/]", "[green]"+association.Path + "[/]", "[green]"+association.Version + "[/]");
                continue;
            }
            
            table.AddRow(association.Name, association.Path, association.Version.ToString());
        }
        
        AnsiConsole.Write(table);
    }
}