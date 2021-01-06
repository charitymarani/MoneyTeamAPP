using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using moneyteamApp.Controllers;
using moneyteamApp.models;
using moneyteamApp.Validators;

namespace moneyteamApp.Views
{
   
    public class ChamaView: IView
    {
        ILogger<IView> _logger;
        IController<Chama> _chamaController;
        InputValidator _validator;
        public ChamaView(ILogger<IView> logger, IController<Chama> chamaController, InputValidator validator)
        {
            _logger = logger;
            _chamaController = chamaController;
            _validator = validator;

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
        public void ProcessInput(string input)
        {
            if (Validate(input))
            {
                CreateChama(input);
            }
            else
            {
                ProcessCreate();
            }
        }
        public void ProcessCreate()
        {
            Console.Write("Enter name : ");
            var input = Console.ReadLine();
            ProcessInput(input);
        }

        public void CreateChama(string name)
        {

            
            bool created = _chamaController.Add(new Dictionary<string, string>() { { "name", name } });
            if (created) {
                Console.WriteLine("Chama created successfully.");
                _logger.LogInformation($"Chama with name {name} created successfully");
            }
            else
            {
                Console.WriteLine($"Command failed! Name {name} already exists. Try a different one.");
                _logger.LogError($"Command failed! Name {name} already exists. Try a different one.");
                ProcessCreate();

            }
             




        }
        public bool Validate(string name)
        {
            if (!_validator.IsNullorEmpty(name, "Name"))
            {
                return false;

            }
            if (!_validator.IsValidLength(name, "Name", 4))
            {
                return false;
            }
            return true;
        }
    }
}
