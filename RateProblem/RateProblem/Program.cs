using System.Threading.Tasks;

namespace RateProblem // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static List<NativeRate> ratesToGetAndUpdate = new List<NativeRate>();
        static async Task Main(string[] args)
        {
            RatesStorage storage = new RatesStorage();
            for (int i = 1; i < 10; i++)
            { 
                var rateToAdd = new NativeRate() { Ask = i, Bid = i, Symbol = i.ToString(), Time = DateTime.Now};
                ratesToGetAndUpdate.Add(rateToAdd);
            }
            var tasks = new Task[1000];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() => ThreadProc(storage));
            }
            await Task.WhenAll(tasks);
            Console.WriteLine("Finished");
        }

        static void ThreadProc(RatesStorage storage)
        {
            Random rnd = new Random();
            double action = rnd.NextDouble();
            if (action < .6)
            {
                var rate = storage.GetRate(ratesToGetAndUpdate[rnd.Next(9)].Symbol);
                if (rate is not null)
                    Console.WriteLine($"{rate.Bid}/{rate.Ask}");
            }
            else
            {
                var rateIndex = rnd.Next(9);
                ratesToGetAndUpdate[rateIndex].Ask++;
                ratesToGetAndUpdate[rateIndex].Bid++;
                storage.UpdateRate(ratesToGetAndUpdate[rnd.Next(9)]);
            }
        }
    }
}

