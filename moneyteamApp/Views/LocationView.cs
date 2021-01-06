
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using moneyteamApp.Controllers;
using moneyteamApp.models;
using moneyteamApp.Validators;

namespace moneyteamApp.Views
{
    public class LocationView : IView
    {

        ILogger<IView> _logger;
        IController<Location> _locationController;
        private int fieldIdx = 0;
        static string[] fields = new string[3] { "Name", "Latitude", "Longitude" };
        string[] data = new string[3];
        InputValidator _validator;
        public LocationView(ILogger<IView> logger, IController<Location> locationController, InputValidator validator)
        {
            _logger = logger;
            _locationController = locationController;
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
        public void ProcessInput(string input, string field)
        {
            if (Validate(input, field))
            {
                data[fieldIdx] = input;
                if (fieldIdx == 2)
                {
                    CreateLocation(data);
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
            Console.Write($"Enter {fields[fieldIdx]}: ");
            var input = Console.ReadLine();
            ProcessInput(input, fields[fieldIdx]);
        }

        public void CreateLocation(string[] data)
        {


            bool created = _locationController.Add(new Dictionary<string, string>() {
                { "name", data[0] },
                {"lat", data[1] },
                {"long", data[2] }
                
            });
            if (created)
            {
                Console.WriteLine("Role created successfully.");
                _logger.LogInformation($"Role with name {data[0]} created successfully");
            }
            else
            {
                Console.WriteLine($"Command failed! Name {data[0]} already exists. Try a different one.");
                _logger.LogError($"Command failed! Name {data[0]} already exists. Try a different one.");
                fieldIdx = 0;
                ProcessCreate();

            }





        }
        public bool Validate(string value, string field )
        {
            if (!_validator.IsNullorEmpty(value, field))
            {
                return false;

            }
            if (field=="Name" && !_validator.IsValidLength(value, field, 4))
            {
                return false;
            }
            if (field != "Name" && !_validator.IsValidLatLong(value, field))
            {
                return false;

            }
            
            return true;
        }
    }
}


