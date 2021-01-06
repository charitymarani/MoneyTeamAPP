
using moneyteamApp.DataAccess;
using moneyteamApp.models;
using System.Collections.Generic;

namespace moneyteamApp.Commands.LocationCommands
{
    public class CreateNewLocationCommand : ICommand
    {
        private Location _location;
        private readonly Dictionary<string, string> _data;
        private readonly IStore<Location> _store;
        public CreateNewLocationCommand(Dictionary<string, string> data, IStore<Location> store)
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
            _location = new Location() { Name = _data["name"], Latitude=double.Parse(_data["lat"]), Longitude=double.Parse(_data["long"]) };
            _store.AddNew(_location);

        }

        public void Undo()
        {
            _store.DeleteItem(_location.Id);
        }
    }
}

