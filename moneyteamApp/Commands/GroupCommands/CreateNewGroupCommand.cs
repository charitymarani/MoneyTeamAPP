
using moneyteamApp.DataAccess;
using moneyteamApp.models;
using System.Collections.Generic;

namespace moneyteamApp.Commands.GroupCommands
{
    public class CreateNewGroupCommand : ICommand
    {
        private Group _group;
        private readonly Dictionary<string, string> _data;
        private readonly IStore<Group> _store;
        public CreateNewGroupCommand(Dictionary<string, string> data, IStore<Group> store)
        {
            _store = store;
            _data = data;
        }

        public bool CanExecute()
        {
            return _store.FindByField("Name", _data["Name"]) == -1;
        }

        public void Execute()
        {
            _group = new Group() { Name = _data["Name"] , ChamaId = int.Parse(_data["ChamaId"])};
            _store.AddNew(_group);

        }

        public void Undo()
        {
            _store.DeleteItem(_group.Id);
        }
    }
}
