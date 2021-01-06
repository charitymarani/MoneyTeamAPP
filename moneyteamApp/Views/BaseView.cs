using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using moneyteamApp.Controllers;
using moneyteamApp.DataAccess;
using moneyteamApp.models;
using moneyteamApp.Validators;

namespace moneyteamApp.Views
{
    public interface IView
    {
        void ProcessCommand(string command);
    }

   
    public class BaseView: IView
    {
        private static Dictionary<string, string> _availableCommands = new Dictionary<string, string>()
        {
            { "CC", "Create Chama" },
            { "CG","Create Group" },
            { "CM", "Create Member" },
            {"CMF" , "Import members from csv/ tsv file"},
            { "CR", "Create Role" },
            { "CL", "Create Location" },
            { "CN", "Create Notice" },
            { "CNG", "Create Notices for a Group." },
            { "AC", "List all available commands" }
            

        };
        private readonly ILogger<IView> _logger;
        private readonly IViewManager _viewManager;
        private readonly IController<Chama> _chamaController;
        private readonly IController<Group> _groupController;
        private readonly IController<Role> _roleController;
        private readonly IController<Location> _locationController;
        private readonly IController<Person> _memberController;
        private readonly IStore<Chama> _chamaStore;
        private readonly IStore<Group> _groupStore;
        private readonly IStore<Person> _memberStore;
        private readonly IStore<Role> _roleStore;
        private readonly IStore<Location> _locationStore;
        private readonly IController<Notice> _noticeController;
        InputValidator _validator;
        public BaseView(ILogger<IView> logger, IViewManager viewmanager, IController<Chama> chamaController,
            IController<Group> groupController ,InputValidator validator, IController<Person> memberController,
            IController<Location> locationController, IController<Role> roleController, IStore<Chama> chamaStore,
            IStore<Group> groupStore,IStore<Role> roleStore, IStore<Location> locationStore, IController<Notice> noticeController, IStore<Person> memberStore)
        {
            _logger = logger;
            _viewManager = viewmanager;
            _chamaController = chamaController;
            _validator = validator;
            _groupController = groupController;
            _roleController = roleController;
            _locationController = locationController;
            _memberController = memberController;
            _chamaStore = chamaStore;
            _groupStore = groupStore;
            _roleStore = roleStore;
            _locationStore = locationStore;
            _noticeController = noticeController;
            _memberStore = memberStore;
        }

        public void ProcessCommand(string command)
        {
            if (!_availableCommands.ContainsKey(command)){
                _logger.LogError($"{command} is an invalid command.Type AC to see available options.");
                Console.Error.WriteLine($"{command} is an invalid command.\nType AC to see available options.");
                var input = Console.ReadLine();
                ProcessCommand(input);


            }
            else if(command == "AC")
            {
                foreach (KeyValuePair<string, string> entry in _availableCommands)
                {
                    Console.WriteLine($"{entry.Key} - {entry.Value} ");
                }
            }
            else
            {

                switch (command[1])
                {
                    case 'C':
                        _viewManager.Invoke(new ChamaView(_logger, _chamaController, _validator), command);
                        break;
                    case 'G':
                        _viewManager.Invoke(new GroupView(_logger, _groupController, _validator, _chamaStore), command);
                        break;
                    case 'R':
                        _viewManager.Invoke(new RoleView(_logger, _roleController, _validator), command);
                        break;
                    case 'L':
                        _viewManager.Invoke(new LocationView(_logger, _locationController, _validator), command);
                        break;
                    case 'M':
                        _viewManager.Invoke(new MemberView(_logger, _memberController, _validator, _groupStore, _roleStore, _locationStore), command);
                        break;
                    case 'N':
                        _viewManager.Invoke(new NoticeView(_logger, _noticeController, _validator, _memberStore, _groupStore), command);
                        break;
                    default:
                        break;

                }
            }
        }
    }
}
