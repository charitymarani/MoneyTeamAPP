
using System.Collections.Generic;
using moneyteamApp.Commands;
using moneyteamApp.Commands.LocationCommands;
using moneyteamApp.DataAccess;
using moneyteamApp.models;

namespace moneyteamApp.Controllers
{

    public class LocationController : IController<Location>
    {
        ICommandManager _commandManager;
        private readonly IStore<Location> _store;

        public LocationController(ICommandManager commandManager, IStore<Location> store)
        {
            _commandManager = commandManager;
            _store = store;
        }
        public bool Add(Dictionary<string, string> data)
        {
            return _commandManager.Invoke(new CreateNewLocationCommand(data, _store));
        }
    }
}
