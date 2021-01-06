
using System.Collections.Generic;
using moneyteamApp.Commands;
using moneyteamApp.Commands.ChamaCommands;
using moneyteamApp.DataAccess;
using moneyteamApp.models;

namespace moneyteamApp.Controllers
{

    public class NoticeController : IController<Notice>
    {
        ICommandManager _commandManager;
        private readonly IStore<Notice> _store;

        public NoticeController(ICommandManager commandManager, IStore<Notice> store)
        {
            _commandManager = commandManager;
            _store = store;
        }
        public bool Add(Dictionary<string, string> data)
        {
            return _commandManager.Invoke(new CreateNewNoticeCommand(data, _store));
        }
    }
}

