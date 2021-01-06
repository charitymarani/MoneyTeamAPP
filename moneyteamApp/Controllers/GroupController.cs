
using System.Collections.Generic;
using moneyteamApp.Commands;
using moneyteamApp.Commands.GroupCommands;
using moneyteamApp.DataAccess;
using moneyteamApp.models;

namespace moneyteamApp.Controllers
{
    
    public class GroupController : IController<Group>
    {
        ICommandManager _commandManager;
        private readonly IStore<Group> _store;

        public GroupController(ICommandManager commandManager, IStore<Group> store)
        {
            _commandManager = commandManager;
            _store = store;
        }
        public bool Add(Dictionary<string, string> data)
        {
            return _commandManager.Invoke(new CreateNewGroupCommand(data, _store));
        }
    }
}

