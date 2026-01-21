namespace AmazonWarehouse;

public class AmazonWarehouse {
    private readonly CancellationTokenSource cts
        = new CancellationTokenSource();
    bool available = true;
    public static void Main(string[] args) {
        AmazonWarehouse aw = new AmazonWarehouse();
        aw.Run();
    }

    private void Run()
    {
        bool stillrunning = true;
        while (stillrunning)
        {
            Console.WriteLine("COLLECTION OF TASKS:");
            Task<string>[] workflow =
            {
                Task<string>.Factory.StartNew(() => ReceiveOrder(cts.Token)), 
                Task<string>.Factory.StartNew(() => Fillorder()),
                Task<string>.Factory.StartNew(() => Shipping())
            };
            {
                string[] results = new string[workflow.Length];
                string resultstring = "order received";
                for (int i = 0; i < workflow.Length; i++)
                {
                    results[i] = workflow[i].Result;
                    resultstring += results[i];
                    Console.Write($"Task: {i}");
                    Console.WriteLine($"\tResult: {results[1]}");
                    if(cts.IsCancellationRequested == true)
                    {
                        break;
                    }
                }

                Console.WriteLine(resultstring);
            }
        }
    }

    private string ReceiveOrder(CancellationToken token)
    {
        Console.WriteLine("Press 'a' for avaible, Press 'q' for unavaible");
        char pro = Console.ReadKey().KeyChar;
        Console.WriteLine("order received");
        switch (pro)
        {
            case 'a': Console.WriteLine("Order exists");
                available = true;
                break;
            case 'q': Console.WriteLine("Order does not exist");
                available = false;
                cts.Cancel();
                break;
            default: goto case 'q';
        }

        if (token.IsCancellationRequested == true)
        {
            return "Product not avaible: Cancellation requested";
        }
        else if (available == true && token.IsCancellationRequested == false)
        {
            return "Product available";
        }
        return "";
    }
    private string Fillorder() {
        return ", shippment ready";
    }
    private string Shipping() {
        return ", product sent";
    }
}


