using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using UEScript.CLI.Common;
using UEScript.CLI.Models;

namespace UEScript.CLI.Commands.Engine.List;

public sealed class EngineInstallsTableView : StackLayoutView
{
    private TextSpanFormatter _formatter { get; } = new TextSpanFormatter();

    public EngineInstallsTableView(IEnumerable<UnrealEngineAssociation> engineAssociations)
    {
        var arrayEngineAssociations = engineAssociations.ToList();
        
        Console.ResetColor();
        
        Add(new ContentView(Span($"Engines installed: {arrayEngineAssociations.Count()}")));
        Add(new ContentView("\n"));

        var tableView = new TableView<UnrealEngineAssociation>();
        tableView.AddColumn(association => arrayEngineAssociations.IndexOf(association) + 1, new ContentView("#".Underline()));

        tableView.Items = arrayEngineAssociations;
        
        tableView.AddColumn(
            association => association.Name.White(), 
            new ContentView("Name".Underline()));
        
        tableView.AddColumn(
            association => association.Path.White(),
            new ContentView("Path".Underline()));
        
        tableView.AddColumn(
            association => association.Version.ToString().ToLower().White(),
            new ContentView("Version".Underline()));
        
        tableView.AddColumn(
            association => association.IsDefault ? "*" : "",
            new ContentView("Default".Underline()));
        
        Add(tableView);
        
        _formatter.AddFormatter<DateTime>(d => $"{d:d} {ForegroundColorSpan.DarkGray()}{d:t}{ForegroundColorSpan.Reset()}");
    }

    private TextSpan Span(FormattableString formattableString)
    {
        return _formatter.ParseToSpan(formattableString);
    }
}