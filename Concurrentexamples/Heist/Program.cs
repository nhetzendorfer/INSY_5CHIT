using System.Text;

namespace Heist;

class Program
{
    static void Main(string[] args)
    {
       Heist heist = new Heist();
       heist.EvaluateHeist();
    }
}
public enum ELog
{
    HIRE_CREW,
    GET_BANK_PLAN,
    BRIBE_BANK_EMPLOYEE,
    BUY_GETAWAY_CAR,
    ENTER_BANK,
    ROB_COUNTER_1,
    ROB_COUNTER_2,
    ROB_COUNTER_3,
    LEAVE_BANK,
    LOOSE_POLICE
}
public class Heist
{
    public void EvaluateHeist()
    {
        string Log;
        var tasks1 = Task.WhenAll(Task.Run(() => HireCrew()), Task.Run(() => GetBankPlans()), Task.Run(() => BribeBankEmployee()), Task.Run(() => BuyGetawayCar()));
        Log= AggregateLog(tasks1.Result);
        var task = Task<string>.WhenAll(tasks1);
        Console.WriteLine(task.ToString());
        var task2 = Task<string>.Run(()=>EnterBank());
        task2.Wait();
        Log+=AggregateLog(task2.Result);
        var tasks2=Task<string>.WhenAll(Task.Run(()=>RobCounter1()),Task.Run(()=>RobCounter2()),Task.Run(()=>RobCounter3()));
        Log+= AggregateLog(tasks2.Result);
        var task3 =Task<string>.Run(() => LeaveBank());
        task3.Wait();
        Log+=AggregateLog(task3.Result);
        var task4=Task<string>.Run(() => LoosePolice());
        task4.Wait();
        Log+= AggregateLog(task4.Result);
        Console.WriteLine(Log);
    }

    private string AggregateLog(string message)
    {
        StringBuilder str = new StringBuilder();
        str.Append(message);
        str.AppendLine();
        return str.ToString();
    }
    private string AggregateLog(string[] message)
    {
        StringBuilder str = new StringBuilder();
        foreach (var item in message)
        {
            str.Append(item);
            str.Append(" ");
        }
        str.AppendLine();
        return str.ToString();
    }
    private string BribeBankEmployee() {
        Thread.Sleep(300);
        return ELog.BRIBE_BANK_EMPLOYEE.ToString();
    }

    private string BuyGetawayCar() {
        Thread.Sleep(200);
        return ELog.BUY_GETAWAY_CAR.ToString();
    }

    private string GetBankPlans() {
        Thread.Sleep(200);
        return ELog.GET_BANK_PLAN.ToString();
    }

    private string HireCrew() {
        Thread.Sleep(400);
        return ELog.HIRE_CREW.ToString();
    }

    private string EnterBank() {
        Thread.Sleep(100);
        return ELog.ENTER_BANK.ToString();
    }

    private string RobCounter1() {
        Thread.Sleep(300);
        return ELog.ROB_COUNTER_1.ToString();
    }

    private string RobCounter2() {
        Thread.Sleep(300);
        return ELog.ROB_COUNTER_2.ToString();
    }

    private string RobCounter3() {
        Thread.Sleep(300);
        return ELog.ROB_COUNTER_3.ToString();
    }

    private string LeaveBank() {
        Thread.Sleep(120);
        return ELog.LEAVE_BANK.ToString();
    }

    private string LoosePolice() {
        Thread.Sleep(300);
        return ELog.LOOSE_POLICE.ToString();
    }

}