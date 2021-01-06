
using moneyteamApp.DataAccess;
using moneyteamApp.models;
using System.Collections.Generic;

namespace moneyteamApp.Commands.RoleCommands
{
    public class CreateNewRoleCommand : ICommand
    {
        private Role _role;
        private readonly Dictionary<string, string> _data;
        private readonly IStore<Role> _store;
        public CreateNewRoleCommand(Dictionary<string, string> data, IStore<Role> store)
        {
            _store = store;
            _data = data;
        }

        public bool CanExecute()
        {
            return _store.FindByField("Name", _data["name"]) == -1;
        }

        public void Execute()
        {
            _role = new Role() { Name = _data["name"] };
            _store.AddNew(_role);

        }

        public void Undo()
        {
            _store.DeleteItem(_role.Id);
        }
    }
}
