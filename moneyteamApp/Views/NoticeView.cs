
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using moneyteamApp.Controllers;
using moneyteamApp.DataAccess;
using moneyteamApp.models;
using moneyteamApp.Validators;
using System.Linq;

namespace moneyteamApp.Views
{
    public class NoticeView : IView
    {

        ILogger<IView> _logger;
        IController<Notice> _noticeController;
        private readonly InputValidator _validator;
        private readonly IStore<Group> _groupStore;
        private readonly IStore<Person> _memberStore;
        Dictionary<string, string> _groupOptions = new Dictionary<string, string>();
        Dictionary<string, string> _memberOptions = new Dictionary<string, string>();
        private int fieldIdx = 0;
        static string[] _groupNoticeFields = new string[2] { "GroupId", "Message" };
        static  string[] _individualNoticeFields = new string[2] { "MemberId", "Message" };
        string[] data = new string[2];
        public NoticeView(ILogger<IView> logger, IController<Notice> noticeController, InputValidator validator, IStore<Person> memberStore, IStore<Group> groupStore)
        {
            _logger = logger;
            _noticeController = noticeController;
            _validator = validator;
            _groupStore = groupStore;
            _memberStore = memberStore;
            GenerateOptions();

        }

        public void ProcessCommand(string command)
        {
            switch (command[0])
            {
                case 'C':
                    ProcessCreate(command);
                    break;
                default:
                    break;
            }
        }
        public void ProcessInput(string input, string field, string command)
        {
            if (Validate(input, field))
            {
                data[fieldIdx] = input;
                if (fieldIdx == 1)
                {
                    bool isGroup = false;
                    if(command == "CNG")
                    {
                        isGroup = true;
                    }
                    CreateNotice(data, isGroup);
                    fieldIdx = 0;

                }
                else
                {
                    fieldIdx++;
                    ProcessCreate(command);

                }
            }
            else
            {
                ProcessCreate(command);
            }
        }
        public void ProcessCreate(string command)
        {
            string field;
            bool isGroup = false;
            string type = "members";
            if(command == "CNG")
            {
                field = _groupNoticeFields[fieldIdx];
                isGroup = true;
                type = "groups";
            }
            else
            {
                field = _individualNoticeFields[fieldIdx];
            }

            if (field == "Message")
            {
                Console.Write($"Enter {field} : ");

            }
            else
            {
      
                Console.WriteLine($"Select {field} from the choices below: ");
                var options = _memberOptions;
                if (isGroup)
                {
                    options = _groupOptions;
                }
              
                if (options.Count == 0)
                {
                    Console.WriteLine($"No {type} available in the system");
                    _logger.LogError($"No {type} available in the system");
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

            var input = Console.ReadLine();
            ProcessInput(input, field, command);
        }

        public void CreateNotice(string[] data, bool isGroup)
        {
            if (isGroup)
            {
                var members = _memberStore.List().Where(item => item.GroupId == int.Parse(data[0]));
                foreach (var member in members)
                {
                    _noticeController.Add(new Dictionary<string, string>() { { "ReceiverId", member.Id.ToString() }, { "Message", data[1] } });
                }
            }
            else
            {
                _noticeController.Add(new Dictionary<string, string>() { { "ReceiverId", data[0] }, { "Message", data[1] } });

            }

            Console.WriteLine("Notices sent successfully.");
            _logger.LogInformation("Notices sent successfully.");
         }
            

        
        public bool Validate(string value, string field)
        {
            if (!_validator.IsNullorEmpty(value, field))
            {
                return false;

            }
            if (field == "GroupId" && !_groupOptions.ContainsKey(value))
            {
                Console.WriteLine($"Invalid GroupId. Please try again.");
                _logger.LogError($"Invalid GroupId. Please try again.");
                return false;

            }
            if (field == "Message" && !_validator.IsValidLength(value, field, 10))
            {
                return false;
            }
            if(field =="MemberId" && !_memberOptions.ContainsKey(value))
            {
                Console.WriteLine($"Invalid MemberId. Please try again.");
                _logger.LogError($"Invalid MemberId. Please try again.");
                return false;
            }
            return true;
        }
        public void GenerateOptions()
        {
            List<Person> members= _memberStore.List();
            List<Group> groups = _groupStore.List();
            foreach (Person item in members)
            {
                _memberOptions[item.Id.ToString()] = $"{item.FirstName}  {item.LastName}";

            }
            foreach(Group item in groups)
            {
                _groupOptions[item.Id.ToString()] = item.Name;
            }

        }
    }
}

