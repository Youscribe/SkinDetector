using SkinDetector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sample.SkinDetector
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = "img";

            if (args.Length > 0)
                path = args[0];

            var files = new string[0];
            FileAttributes attr = File.GetAttributes(path);

            //detect whether its a directory or file
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                files = Directory.GetFiles(path);
            else
                files = new[] { path };

            var skinDetectorService = new SkinDetectorService();
            foreach (var file in files)
            {
                Console.WriteLine("file " + file);
                var percent = skinDetectorService.GetPercentageOfSkinInImage(file);
                Console.WriteLine(percent + "% skin detected");
            }
            Console.ReadLine();
        }
    }
}
