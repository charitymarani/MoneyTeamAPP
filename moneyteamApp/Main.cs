using System;
using Microsoft.Extensions.Logging;
using moneyteamApp.Views;

namespace moneyteamApp
{
    public interface IMain
    {
        void Run();
    }
    public class Main: IMain
    {
        private readonly IView _baseView;
        public Main(ILogger<Main> logger, IView baseView)
        {
            _baseView = baseView;
        }

        public void Run()
        {
            Console.WriteLine("Type a command to proceed e.g \n CC - to create a new Chama \n CG - to create a a new group\nType AC to see all available commands.");
            var input = Console.ReadLine();
            _baseView.ProcessCommand(input);
            ReRun();
        }
        public void ReRun()
        {
            Console.WriteLine("Do you wish to continue? Y/N");
            var res = Console.ReadLine();
            if (res == "Y" || res == "y")
            {
                Run();
            }
            else if (res == "N" || res == "n")
            {
                Console.WriteLine("Thank you for using MoneyTeam App!");
                Console.WriteLine("Good Bye!");
            
            }
            else
            {

                Console.WriteLine("Invalid choice. Please type Y or N. ");
                ReRun();

            }
          
        }

    }
}
