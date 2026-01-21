namespace Cancellation;

class Program
{
    static void Main(string[] args)
    {
        using (var cts = new CancellationTokenSource()) {
            Task task = new Task(() => { LongRunningTask(cts.Token); });
            task.Start();
            Console.WriteLine("Operation Performing...");
            if (Console.ReadKey().Key == ConsoleKey.C) {
                Console.WriteLine("Cancelling..");
                cts.Cancel();
            }
            Console.Read();
        }
    }
    private static void LongRunningTask(CancellationToken token) {
        for (int i = 0; i < 10000000; i++)
            if (token.IsCancellationRequested) {
                break;
            } else {
                Console.WriteLine(i);
            }
    }
}