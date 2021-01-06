using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace moneyteamApp.Commands
{
    public interface ICommandManager
    {
        bool Invoke(ICommand command);
        void Undo();
    }
    public class CommandManager: ICommandManager
    {
        private Stack<ICommand> commands = new Stack<ICommand>();
        private readonly ILogger<CommandManager> _logger;
        public CommandManager(ILogger<CommandManager> logger)
        {
            _logger = logger;
        }

        public bool Invoke(ICommand command)
        {
            if (command.CanExecute())
            {
                commands.Push(command);
                command.Execute();
                return true;
                
            }
            else
            {
                return false;

            }
        }

        public void Undo()
        {
            var command = commands.Pop();
            command.Undo();
        }
    }
}
