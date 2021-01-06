using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using moneyteamApp.Controllers;
using moneyteamApp.DataAccess;
using moneyteamApp.models;
using moneyteamApp.Validators;

namespace moneyteamApp.Views
{
    public class GroupView: IView
    {
        
        ILogger<IView> _logger;
        IController<Group> _groupController;
        private readonly InputValidator _validator;
        private readonly IStore<Chama> _chamaStore;
        Dictionary<string, string> _chamaOptions = new Dictionary<string, string>();
        private int fieldIdx = 0;
        static string[] fields = new string[2] { "ChamaId", "Name" };
        string[] data = new string[2];
        public GroupView(ILogger<IView> logger, IController<Group> groupController, InputValidator validator, IStore<Chama> chamaStore)
        {
            _logger = logger;
            _groupController = groupController;
            _validator = validator;
            _chamaStore = chamaStore;
            List<Chama> chamas = _chamaStore.List();
            GenerateOptions(chamas);

        }

        public void ProcessCommand(string command)
        {
            switch (command[0])
            {
                case 'C':
                    ProcessCreate();
                    break;
                default:
                    break;
            }
        }
        public void ProcessInput(string input, string field)
        {
            if (Validate(input, field))
            {
                data[fieldIdx] = input;
                if (fieldIdx == 1)
                {
                    CreateGroup(data);
                    fieldIdx = 0;

                }
                else
                {
                    fieldIdx++;
                    ProcessCreate();

                }
            }
            else
            {
                ProcessCreate();
            }
        }
        public void ProcessCreate()
        {
            var field = fields[fieldIdx];
            if (field == "Name")
            {
                Console.Write($"Enter {field} : ");

            }
            else { 

                
                Console.WriteLine($"Select {field} from the choices below: ");
                if (_chamaOptions.Count == 0)
                {
                    Console.WriteLine("No chamas available in the system");
                    _logger.LogError("No chamas available in the system");
                    return;
                }
                else
                {
                    foreach(KeyValuePair<string, string> entry in _chamaOptions)
                    {
                        Console.WriteLine($"{entry.Key} - {entry.Value} ");
                    }
                } 
            }
            
            var input = Console.ReadLine();
            ProcessInput(input, field);
        }

        public void CreateGroup(string[] data)
        {


           bool  created =_groupController.Add(new Dictionary<string, string>() { { "ChamaId", data[0] }, { "Name", data[1] } });
            if (created)
            {
                Console.WriteLine("Group created successfully.");
                _logger.LogInformation($"Group with name {data[1]} created successfully");
            }
            else
            {
                Console.WriteLine($"Command failed! Name {data[1]} already exists. Try a different one.");
                _logger.LogError($"Command failed! Name {data[1]} already exists. Try a different one.");
                fieldIdx = 0;
                ProcessCreate();

            }

        }
        public bool Validate(string value, string field)
        {
            if (!_validator.IsNullorEmpty(value, field))
            {
                return false;

            }
            if(field=="ChamaId" && !_chamaOptions.ContainsKey(value)){
                Console.WriteLine($"Invalid ChamaId. Please try again.");
                _logger.LogError($"Invalid ChamaId. Please try again.");
                return false;

            }
            if (field =="Name" && !_validator.IsValidLength(value, field , 4))
            {
                return false;
            }
            return true;
        }
        public void GenerateOptions(List<Chama> data)
        {
            foreach(Chama item in data)
            {
                _chamaOptions[item.Id.ToString()] = item.Name;

            }
           
        }
    }
}
