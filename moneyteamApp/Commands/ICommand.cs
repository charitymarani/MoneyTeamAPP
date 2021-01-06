using System;
namespace moneyteamApp.Commands
{
    public interface ICommand
    {
        void Execute();
        bool CanExecute();
        void Undo();
    }
}
