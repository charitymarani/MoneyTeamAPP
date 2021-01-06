
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using moneyteamApp.Controllers;
using moneyteamApp.DataAccess;
using moneyteamApp.models;
using moneyteamApp.Validators;


namespace moneyteamApp.Views
{
    public class MemberView : IView
    {

        ILogger<IView> _logger;
        IController<Person> _memberController;
        private int fieldIdx = 0;
        static string[] fields = new string[9] {"FirstName", "LastName", "GenderId", "PhoneNumber", "EmailAddress", "DateOfBirth", "RoleId", "GroupId", "LocationId" };
        static HashSet<string> foreignKeyFields = new HashSet<string>() { "LocationId", "GroupId", "RoleId" };
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            { "FirstName", null}, {  "LastName", null},{ "GenderId", null},{"PhoneNumber", null},
            {"EmailAddress", null},{ "DateOfBirth", null},{ "RoleId", null},{ "GroupId", null},{ "LocationId", null}

        };
        private readonly InputValidator _validator;
        private readonly IStore<Group> _groupStore;
        private readonly IStore<Role> _roleStore;
        private readonly IStore<Location> _locationStore;
        Dictionary<string, string> _groupOptions = new Dictionary<string, string>();
        Dictionary<string, string> _roleOptions = new Dictionary<string, string>();
        Dictionary<string, string> _locationOptions = new Dictionary<string, string>();
        Dictionary<string, string> _genderOptions = new Dictionary<string, string>() { { "1", "Female" }, { "2", "Male" } };
        public MemberView(ILogger<IView> logger, IController<Person> memberController, InputValidator validator,
            IStore<Group> groupStore, IStore<Role> roleStore, IStore<Location> locationStore)
        {
            _logger = logger;
            _memberController = memberController;
            _validator = validator;
            _locationStore = locationStore;
            _roleStore = roleStore;
            _groupStore = groupStore;
            GenerateOptions();
        }

        public void ProcessCommand(string command)
        {
            switch (command[0])
            {
                case 'C':
                    if (command == "CMF")
                    {
                        ProcessImport();
                    }
                    else
                    {
                        ProcessCreate();
                    }
                    
                    break;
                default:
                    break;
            }
        }
        public void ProcessInput(string input, string field)
        {
            if (Validate(input, field))
            {
                bool isEmailProvided = false;
                if(field == "EmailAddress")
                {
                    if (!string.IsNullOrEmpty(input))
                    {
                        isEmailProvided = true;
                        input = input.ToLower();

                    }
                    
                }
                if (!(field == "EmailAddress" && !isEmailProvided))
                {
                    data[fields[fieldIdx]] = input;
                }
                
                if (fieldIdx == 8)
                {
                    CreateMember(data);
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
        public void ProcessImport()
        {

            Console.WriteLine($"Enter the path to your csv/tsv file: ");
            var input = Console.ReadLine();
            if(Validate(input, "FilePath"))
            {
                var separator = Path.GetExtension(input) == ".csv" ? "," : "\t";
                ReadFile(input, separator);

           }
            else
            {
                ProcessImport();

            }
            

        }
        public void ReadFile(string inputfile, string separator)
        {
            int failed = 0;
            int created = 0;
            using (var reader = new StreamReader(inputfile))
            {
                var header = reader.ReadLine().Split(separator);
                if (header.Length < 9)
                {
                    Console.Error.WriteLine("Please provide all required headers on the first row");
                    _logger.LogError("Please provide all required headers on the first row");
                    return;

                }
                foreach(var item in header)
                {
                    if (!data.ContainsKey(item)){
                        Console.Error.WriteLine($"Invalid header {item}. Make sure all headers valid.");
                        _logger.LogError($"Invalid header {item}. Make sure all headers valid.");
                        return;

                    }

                }
                int linecount = 1;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var row = line.Split(separator);
                    if (row.Length == 9)
                    {
                        Dictionary<string, string> details = new Dictionary<string, string>()
                        {
                            { "FirstName", null}, {  "LastName", null},{ "GenderId", null},{"PhoneNumber", null},
                            {"EmailAddress", null},{ "DateOfBirth", null},{ "RoleId", null},{ "GroupId", null},{ "LocationId", null}

                        };
                        bool validEntry = true;

                        for (int i = 0; i < row.Length; i++)
                        {
                            if(Validate(row[i], header[i]))
                            {
                                if(!(header[i] == "EmailAddress" && string.IsNullOrEmpty(row[i]))){
                                    details[header[i]] = row[i];
                                }

                            }
                            else
                            {
                                validEntry = false;
                                failed++;
                                Console.WriteLine($"Row {linecount} failed due to invalid {header[i]}");
                                _logger.LogError($"Row {linecount} failed due to invalid {header[i]}");
                                linecount++;
                                break;
                            }
                        }
                        if (validEntry)
                        {
                            if(CreateMember(details, true))
                            {
                                created++;
                            }
                            else
                            {
                                failed++;

                            }
                            
                            linecount++;

                        }


                    }
                    else
                    {
                        Console.WriteLine($"Row {linecount} failed due to invalid since it does not provide all entries.");
                        _logger.LogError($"Row {linecount} failed due to invalid since it does not provide all entries.");
                        failed++;
                        linecount++;
                    }

                }      
            }
            Console.Error.WriteLine($"{created} entries were imported successfully. {failed} records failed.");
            _logger.LogError($"{created} entries were imported successfully. {failed} entries failed.");

        }
         public void ProcessCreate()
        {
            
            var field = fields[fieldIdx];
            if (!foreignKeyFields.Contains(field))
            {
                Console.WriteLine($"Enter {field} : ");

            }
            else if(field=="RoleId")
            {
                ProcessOptions("RoleId", _roleOptions);
            }
            else if (field == "LocationId")
            {
                ProcessOptions("LocationId", _locationOptions);

            }
            else if (field == "GroupId")
            {
                ProcessOptions("GroupId", _groupOptions);
            }
            else if (field == "GenderId")
            {
                ProcessOptions("GenderId", _genderOptions);

            }

            var input = Console.ReadLine();
            ProcessInput(input, field);
            
        }
        public void ProcessOptions(string field, Dictionary<string, string> options)
        {
            Console.WriteLine($"Select {field} from the choices below: ");
            if (options.Count == 0)
            {
                Console.WriteLine("No options available.");
                _logger.LogError("No chamas available.");
                return;
            }
            else
            {
                foreach (KeyValuePair<string, string> entry in options)
                {
                    Console.WriteLine($"{entry.Key} - {entry.Value} ");
                }
            }

        }
        

        public bool CreateMember(Dictionary<string,string> data, bool imported=false)
        {


            bool created = _memberController.Add(data);

            if (created)
            {
                Console.WriteLine("Member created successfully.");
                _logger.LogInformation($"Member with name {data["FirstName"]} created successfully");
            }
            else
            {
                Console.WriteLine($"Command failed! PhoneNumber or EmailAddress already exists. Try again.");
                _logger.LogError($"Command failed! PhoneNumber or EmailAddress already exists. Try again.");
                if (!imported)
                {
                    fieldIdx = 0;
                    ProcessCreate();
                }
                

            }
            return created;

        }
        public bool Validate(string value, string field)
        {
            if (!_validator.IsNullorEmpty(value, field))
            {
                return false;

            }
            if ((field == "FirstName" || field=="LastName") && !_validator.IsValidLength(value, field, 3))
            {
                return false;
            }
            if (field == "EmailAddress" && !string.IsNullOrEmpty(value) && !_validator.IsValidEmail(value))
            {
                return false;

            }
            
            if (field=="PhoneNumber" && !_validator.IsValidPhoneNumber(value))
            {
                return false;
            }
            if(field=="DateOfBirth" && !_validator.IsValidDate(value))
            {
                return false;
            }
            if (field == "RoleId" && !_roleOptions.ContainsKey(value))
            {
                Console.WriteLine($"Invalid RoleId. Please try again.");
                _logger.LogError($"Invalid RoleId. Please try again.");
                return false;
            }
            if (field == "LocationId" && !_locationOptions.ContainsKey(value))
            {
                Console.WriteLine($"Invalid LocationId. Please try again.");
                _logger.LogError($"Invalid LocationId. Please try again.");
                return false;

            }
            if (field == "GroupId" && !_groupOptions.ContainsKey(value))
            {
                Console.WriteLine($"Invalid GroupId. Please try again.");
                _logger.LogError($"Invalid GroupId. Please try again.");
                return false;
            }
            if (field == "GenderId" && !_genderOptions.ContainsKey(value))
            {
                Console.WriteLine($"Invalid GenderId. Please try again.");
                _logger.LogError($"Invalid GenderId. Please try again.");
                return false;
            }
            if(field == "FilePath" && !_validator.ValidateFile(value))
            {
                return false;
            }


            return true;
        }
        public void GenerateOptions()
        {
            List<Role> roles = _roleStore.List();
            List<Group> groups = _groupStore.List();
            List<Location> locations = _locationStore.List();
            foreach (Role item in roles)
            {
                _roleOptions[item.Id.ToString()] = item.Name;

            }
            foreach(Group item in groups)
            {
                _groupOptions[item.Id.ToString()] = item.Name;
            }
            foreach (Location item in locations)
            {
                _locationOptions[item.Id.ToString()] = item.Name;
            }


        }
        
    }
}
