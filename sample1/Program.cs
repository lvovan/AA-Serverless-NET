namespace sample1;

// using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        // var james = new Personne
        // {
        //     Nom = "James",
        //     Age = 43
        // };

        // string serialized = JsonConvert.SerializeObject(james, formatting: Formatting.Indented);
        // Console.WriteLine(serialized);
        using (Image image = Image.Load(@"img\robot.jpeg"))
        {
            image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));

            image.Save(@"img\robot-resized.jpeg");
        }

        // Test de la fonction OilPaintImage
        OilPaintImage(@"img\robot.jpeg");

        // Construction de la liste des fichiers à benchmarker
        string[] fileNames = Directory.GetFiles(@"img/benchmark");

        var watch = Stopwatch.StartNew();
        Array.ForEach(fileNames, OilPaintImage);
        watch.Stop();

        var watchForParallel = Stopwatch.StartNew();
        Parallel.ForEach(fileNames, OilPaintImage);
        watchForParallel.Stop();

        Console.WriteLine($"Classical foreach loop | Time Taken : {watch.ElapsedMilliseconds} ms.");
        Console.WriteLine($"Parallel.ForEach loop  | Time Taken : {watchForParallel.ElapsedMilliseconds} ms.");


    }

    static void OilPaintImage(string path)
    {
        using (Image image = Image.Load(path))
        {
            image.Mutate(x => x.OilPaint());
            string outputPath = $"{Path.GetDirectoryName(path)}\\{Path.GetFileNameWithoutExtension(path)}_oil{Path.GetExtension(path)}";
            image.Save(outputPath);
        }
    }
}

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
