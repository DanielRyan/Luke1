using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;

namespace Luke2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (new Luke2().Run().Subscribe(Console.WriteLine)) { }

            Console.ReadKey();
        }
    }

    class Luke2
    {
        public IObservable<int> Run()
        {
            // (446,767);(933,757)
            var data = GetData();

            return Observable.Return(data)
                .SelectMany(GetOneLine)
                .Where(IsValid)
                .Select(PrettifyString)
                .Select(ToStringArray)
                .Select(ToIntArray)
                .Select(ToLine)
                .Select(ToSlope)
                .GroupBy(slope => slope)
                .SelectMany(slopes => slopes.Count())
                .Max();
        }

        private static IEnumerable<string> GetOneLine(string x)
        {
            return x.Split('\n');
        }

        private static bool IsValid(string x)
        {
            return !string.IsNullOrWhiteSpace(x);
        }

        private static int[] ToIntArray(string[] x)
        {
            return x.Select(int.Parse).ToArray();
        }

        private static string[] ToStringArray(string x)
        {
            return x.Split(',');
        }

        private static Tuple<Point, Point> ToLine(int[] a)
        {
            return new Tuple<Point, Point>(new Point(a[0], a[1]), new Point(a[2], a[3]));
        }

        private static double ToSlope(Tuple<Point, Point> c)
        {
            return (double)(c.Item2.Y - c.Item1.Y) / (c.Item2.X - c.Item1.X);
        }

        private string PrettifyString(string x)
        {
            return x
                .Replace("(", "")
                .Replace(")", "")
                .Replace(';', ',');
        }

        private string GetData()
        {
            using (var client = new WebClient())
            {
                return client.DownloadString("https://s3-eu-west-1.amazonaws.com/knowit-julekalender-2018/input-rain.txt");
            }
        }
    }

    class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
