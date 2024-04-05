namespace sample1;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Lauching resizing ...");

        using (Image image = Image.Load(@"img\robot.jpeg"))
        {
            image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));

            image.Save(@"img\robot-resized.jpeg");
        }

        Console.WriteLine("Image resized successfully !");
    }

}

