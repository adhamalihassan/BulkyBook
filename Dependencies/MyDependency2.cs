namespace BulkyBook.Dependencies;

public class MyDependency2 : IMyDependency
{
    private string _name;
    public string Name  // read-write instance property
    {
        get => _name;
        set => _name = value;
    }

    public MyDependency2()
    {
        this.Name = RandomStringGenerator.GenerateRandomString(10);;
    }
    public void WriteMessage(string message)
    {
        Console.WriteLine($"MyDependency.WriteMessage Message: {message}");
    }
}