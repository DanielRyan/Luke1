using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;

namespace Luke1
{
    class Program
    {
        static void Main(string[] args)
        {
            new Luke1().Run();

            Console.ReadLine();
        }
    }
    public class Luke1
    {
        public void Run()
        {
            var data = GetData();

            var obs = Observable.Return(data);

            var obs2 = obs
                .SelectMany(x => x.Split('\n'))
                .Select(x =>
                {
                    var success = int.TryParse(x, out var tall);
                    return new { isTall = success, tall };
                })
                .Where(x => x.isTall)
                .Select(x => x.tall)
                .Scan(
                    new Stack<int>(new List<int> { 0 }),
                    (stack, current) =>
                    {
                        if (stack.Peek() < current)
                            stack.Push(current);

                        return stack;
                    }
                )
                .DistinctUntilChanged(x => x.Count)
                .Select(x => x.Peek());



            var annet = obs2.Subscribe(x =>
            {
                Console.WriteLine(x);
            });
        }

        public string GetData()
        {
            using (var client = new WebClient())
            {
                string url = "https://s3-eu-west-1.amazonaws.com/knowit-julekalender-2018/input-vekksort.txt";
                string content = client.DownloadString(url);
                //Console.WriteLine(content);

                var noe = content.Contains("\n");

                return content;
            }
        }
    }

}