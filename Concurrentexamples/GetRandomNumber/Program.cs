namespace GetRandomNumber;

class Program
{
    static void Main(string[] args) {
        Console.WriteLine("Hello, Task!");
        var task = new Task(() => GetRandomNumber());
        task.Start();
        var task2 = Task.Factory.StartNew(()=>GetRandomNumber());
        var task3 = Task.Run(() => GetRandomNumber());
        Task<int> task4 = Task.Run(() => GetRandomNumber());
        Console.WriteLine("Start the program...");
        Console.WriteLine("result of task4: "+ task4.Result);
        Console.ReadLine();
    }
    static int GetRandomNumber() {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        var threadPool = Thread.CurrentThread.IsThreadPoolThread;
        Console.WriteLine($"The thread #{threadId}, use a thread pool {threadPool}");
        Thread.Sleep(1000);
        int randomNumber = (new Random()).Next(1, 100);
        Console.WriteLine($"The random number is {randomNumber}");
        return randomNumber;
    }

}