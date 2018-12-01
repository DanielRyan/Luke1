using System.Linq;
using System.Net;

namespace Luke1
{
    public class Test
    {
        public long Run()
        {
            var data = GetData();
            var stringList = data.Split('\n').ToList();

            var sum = 0L;
            var highest = 0L;

            foreach (var s in stringList)
            {
                var success = long.TryParse(s, out var current);

                if(!success) continue;

                if (current >= highest)
                {
                    highest = current;
                    sum = sum + current;
                }
            }

            return sum;
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