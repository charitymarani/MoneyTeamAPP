
using moneyteamApp.DataAccess;
using moneyteamApp.models;
using System.Collections.Generic;

namespace moneyteamApp.Commands.ChamaCommands
{
    public class CreateNewNoticeCommand : ICommand
    {
        private Notice _notice;
        private readonly Dictionary<string, string> _data;
        private readonly IStore<Notice> _store;
        public CreateNewNoticeCommand(Dictionary<string, string> data, IStore<Notice> store)
        {
            _store = store;
            _data = data;
        }

        public bool CanExecute()
        {
            return true;
        }

        public void Execute()
        {
            _notice = new Notice() { Message = _data["Message"] , ReceiverId=int.Parse(_data["ReceiverId"]) };
            _store.AddNew(_notice);

        }

        public void Undo()
        {
            _store.DeleteItem(_notice.Id);
        }
    }
}
