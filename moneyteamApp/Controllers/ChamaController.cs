
using System.Collections.Generic;
using moneyteamApp.Commands;
using moneyteamApp.DataAccess;
using moneyteamApp.models;
using moneyteamApp.Commands.ChamaCommands;

namespace moneyteamApp.Controllers
{
   
    public class ChamaController: IController<Chama>
    {
        ICommandManager _commandManager;
        private readonly IStore<Chama> _store;
       
        public ChamaController(ICommandManager commandManager, IStore<Chama> store)
        {
            _commandManager = commandManager;
            _store = store;
        }
        public bool Add( Dictionary<string, string> data)
        {
            return _commandManager.Invoke(new CreateNewChamaCommand(data, _store));
        }
    }
}
