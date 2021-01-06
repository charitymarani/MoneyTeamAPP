
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using moneyteamApp.Controllers;
using moneyteamApp.models;
using moneyteamApp.Validators;

namespace moneyteamApp.Views
{
    public class RoleView : IView
    {

        ILogger<IView> _logger;
        IController<Role> _roleController;
        InputValidator _validator;
        public RoleView(ILogger<IView> logger, IController<Role> roleController, InputValidator validator)
        {
            _logger = logger;
            _roleController = roleController;
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
                CreateRole(input);
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

        public void CreateRole(string name)
        {


            bool created = _roleController.Add(new Dictionary<string, string>() { { "name", name } });
            if (created)
            {
                Console.WriteLine("Role created successfully.");
                _logger.LogInformation($"Role with name {name} created successfully");
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

