namespace BulkyBook.Dependencies;

using System;

public class RandomStringGenerator
{
    private static Random random = new Random();
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GenerateRandomString(int length)
    {
        char[] result = new char[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = chars[random.Next(chars.Length)];
        }
        return new string(result);
    }
}

public class MyDependency : IMyDependency
{
    private string _name;
    public string Name  // read-write instance property
    {
        get => _name;
        set => _name = value;
    }

    public MyDependency()
    {
        this.Name = RandomStringGenerator.GenerateRandomString(10);;
    }
    public void WriteMessage(string message)
    {
        Console.WriteLine($"MyDependency.WriteMessage Message: {message}");
    }
}