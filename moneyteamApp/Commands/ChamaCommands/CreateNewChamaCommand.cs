using System;
using moneyteamApp.DataAccess;
using moneyteamApp.models;
using System.Collections.Generic;

namespace moneyteamApp.Commands.ChamaCommands
{
    public class CreateNewChamaCommand: ICommand
    {
        private  Chama _chama;
        private readonly Dictionary<string, string> _data;
        private readonly IStore<Chama> _store;
        public CreateNewChamaCommand(Dictionary<string,string> data, IStore<Chama> store)
        {
            _store = store;
            _data = data;
        }

        public bool CanExecute()
        {
            return _store.FindByField("Name", _data["name"])==-1;
        }

        public void Execute()
        {
            _chama = new Chama(){ Name = _data["name"] };
            _store.AddNew(_chama);
            
        }

        public void Undo()
        {
            _store.DeleteItem(_chama.Id);
        }
    }
}
