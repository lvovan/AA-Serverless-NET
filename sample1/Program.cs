namespace sample1;

public class Personne
{
    public string? Nom { get; set; }
    public int Age { get; set; }

    public string Hello(bool isLowercase)
    {
        string raw = $"hello {this.Nom}, you are {this.Age}";

        if (isLowercase) {
            return raw.ToLower();
        } else {
            return raw.ToUpper();
        }
    }

}

class Program
{
    static void Main(string[] args)
    {
        var james = new Personne
        {
            Nom = "James",
            Age = 43
        };

        Console.WriteLine(james.Hello(isLowercase: true));

    }
}
