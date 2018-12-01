using System;
using System.Linq;
using System.Net;
using System.Reactive.Linq;

namespace Luke1
{
    class Program
    {
        static void Main(string[] args)
        {
            var sum = new Test().Run();
            Console.WriteLine(sum);

            //using (new Luke1().Sum().Subscribe(Console.WriteLine)) { }

            Console.ReadLine();
        }
    }
    public class Luke1
    {
        public IObservable<long> Sum()
        {
            var data = GetData();

            return Observable.Return(data)
                .SelectMany(x => x.Split('\n'))
                .Select(x => long.TryParse(x, out var integer) ? (long?)integer : null)
                .Where(x => x.HasValue)
                .Select(x => x.Value)
                .Scan(0L, Math.Max)
                .DistinctUntilChanged()
                .Sum();
        }

        private string GetData()
        {
            using (var client = new WebClient())
            {
                return client.DownloadString("https://s3-eu-west-1.amazonaws.com/knowit-julekalender-2018/input-vekksort.txt");
            }
        }
    }
}