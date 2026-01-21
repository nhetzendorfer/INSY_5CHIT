namespace Parallel;

class Program
{
    static void Main(string[] args)
    {
        System.Threading.Tasks.Parallel.Invoke(Driver1,
            () =>
            {
                Console.WriteLine($"Fahrer Charles Leclerc ist im Ziel " +
                                  $"(Thread{Thread.CurrentThread.ManagedThreadId})");
            },
            delegate()
            {
                Console.WriteLine($"Fahrer Carlos Sainz ist jetzt im Ziel " +
                                  $"(Thread{Thread.CurrentThread.ManagedThreadId})");
            },
            Driver2,
            delegate()
            {
                Console.WriteLine($"Fahrer Lando Norris ist jetzt im Ziel " +
                                  $"(Thread{Thread.CurrentThread.ManagedThreadId})");
            },
            () =>
            {
                Console.WriteLine($"Fahrer George Russell ist im Ziel " +
                                  $"(Thread{Thread.CurrentThread.ManagedThreadId})");
            });

        static void Driver1()
        {
            Console.WriteLine($"Fahrer Max Verstappen ist im Ziel " +
                              $"(Thread{Thread.CurrentThread.ManagedThreadId})");
        }

        static void Driver2()
        {
            Console.WriteLine($"Fahrer Lewis Hamilton ist im Ziel " +
                              $"(Thread{Thread.CurrentThread.ManagedThreadId})");
        }
        Race.MakeRace();
        Pizza.MakePizza();
    }
}
class Race {
    public static void MakeRace() {
        Console.WriteLine("Das Rennen startet");
        int x = 0;
        List<int> racepos = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,
            12, 13, 14, 15, 16, 17, 18, 19, 20 };
        var res = System.Threading.Tasks.Parallel.ForEach(racepos, (i) => {
            if (i == 3) {
                Console.WriteLine($"Du bist aktuell auf dem Podest - Platz: {i}");
                x++;
            } else if (i == 2) {
                Console.WriteLine($"Du bist aktuell an der Spitze dran - Platz {i}");
                x++;
            } else if (i == 1) {
                Console.WriteLine($"Du bist aktuell dem Sieg sehr nahe - Platz {i}");
                x++;
            } else if (i == 20) {
                Console.WriteLine($"Nicht aufgeben - Platz {i}");
                x++;
            } else {
                Console.WriteLine($"Aktueller Platz: {i}");
                x++;
            }
            if (x == 20) {
                Console.WriteLine();
                Console.WriteLine($"Rennen beendet - endgültige Position: {i}");
            }
        });
    }
}
class Pizza {
    public static void MakePizza() {
        Console.WriteLine("Pizza Factory");
        Console.WriteLine();
        var res = System.Threading.Tasks.Parallel.For(1, 11, (i) => {
            Console.WriteLine($"Pizza{i} backen");
            if (i == 1) {
                Console.WriteLine("Erste Pizza des Tages");
                Console.WriteLine($"Pizza{i} fertig gebacken");
            } else if (i == 10) {
                Console.WriteLine($"Gratis Pizza!!! {i}!!!");
                Console.WriteLine($"Pizza{i} fertig gebacken");
            } else {
                Console.WriteLine($"Pizza{i} fertig gebacken");
            }
        });
    }
}

