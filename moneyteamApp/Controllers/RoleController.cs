
using System.Collections.Generic;
using moneyteamApp.Commands;
using moneyteamApp.Commands.RoleCommands;
using moneyteamApp.DataAccess;
using moneyteamApp.models;

namespace moneyteamApp.Controllers
{

    public class RoleController : IController<Role>
    {
        ICommandManager _commandManager;
        private readonly IStore<Role> _store;

        public RoleController(ICommandManager commandManager, IStore<Role> store)
        {
            _commandManager = commandManager;
            _store = store;
        }
        public bool Add(Dictionary<string, string> data)
        {
            return _commandManager.Invoke(new CreateNewRoleCommand(data, _store));
        }
    }
}

