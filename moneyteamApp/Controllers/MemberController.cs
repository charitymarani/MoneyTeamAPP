
using System.Collections.Generic;
using moneyteamApp.Commands;
using moneyteamApp.Commands.MemberCommands;
using moneyteamApp.DataAccess;
using moneyteamApp.models;

namespace moneyteamApp.Controllers
{

    public class MemberController : IController<Person>
    {
        ICommandManager _commandManager;
        private readonly IStore<Person> _store;

        public MemberController(ICommandManager commandManager, IStore<Person> store)
        {
            _commandManager = commandManager;
            _store = store;
        }
        public bool Add(Dictionary<string, string> data)
        {
            return _commandManager.Invoke(new CreateNewMemberCommand(data, _store));
        }
    }
}
