using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
using XXI;

namespace Test
{

    class Program
    {
        static void Main(string[] args)
        {
            Activity workflow1 = new Workflow1();
            WorkflowInvoker.Invoke(workflow1);

            ExchageRates er = new ExchageRates();
            
            er.StartDownloadTimer(DateTime.Now);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            //decimal rate = er.GetRate(826, DateTime.Now.Date);
        }
    }
}
