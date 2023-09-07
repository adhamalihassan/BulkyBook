namespace BulkyBook.Dependencies;

public interface IMyDependency
{
    string Name { get; set; }
    void WriteMessage(string message);
}