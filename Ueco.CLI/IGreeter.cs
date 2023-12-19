namespace Ueco;

public interface IGreeter
{
    void Greet(string name) => Console.WriteLine("Hello, {0}!", name);
}

public class Greeter : IGreeter
{
}