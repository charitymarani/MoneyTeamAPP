using System;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.IO;


namespace moneyteamApp.Validators
{
    public  class InputValidator
    {
        ILogger<InputValidator> _logger;
        public InputValidator(ILogger<InputValidator> logger)
        {
            _logger = logger;
           
        }
        public bool IsNullorEmpty(string input ,string field)
        {
            if (field != "EmailAddress" && string.IsNullOrEmpty(input))
            {
                Console.Error.WriteLine($"{field} can not be null or empty! \n Please try again.");
                _logger.LogError($"{field} can not be null or empty! Please try again.");
                return false;

            }
            return true;

        }
        public bool IsValidLength(string input, string field , int length)
        {
            if (input.Length < length)
            {
                Console.Error.WriteLine($"{field} length must be {length} or more character! \n Please try again.");
                _logger.LogError($"{field} length must be {length} or more character! Please try again.");
                return false;
            }
            return true;

        }

        
        public bool IsValidLatLong(string value, string field)
        {
            double output;
            if (double.TryParse(value, out output))
            {
                if(output>=0.0 && output <= 0.9)
                {
                    return true;
                }
                else

                {
                    Console.Error.WriteLine($"Invalid {field}. Please enter a {field} value within Nairobi i.e between 0.0 and 0.9 ");
                    _logger.LogError($"Invalid {field}. Please enter a {field} value within Nairobi i.e between 0.0 and 0.9 ");
                    return false;
                }
                
            }
            else
            {
                Console.Error.WriteLine($"{field} must be of type double \n Please try again with a value between 0.0 and 0.9 ");
                _logger.LogError($"{field} must be of type double \n Please try again with a value between 0.0 and 0.9 ");
                return false;
            }
        }

        public bool IsValidEmail(string value)
        {
            Regex regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            bool isValid = regex.IsMatch(value);
            if (!isValid)
            {
                Console.Error.WriteLine($"Invalid Email. Please enter a valid email e.g test@gmail.com ");
                _logger.LogError($"Invalid Email. Please enter a valid email e.g test@gmail.com ");

            }
            return isValid;

        }
        public bool IsValidPhoneNumber(string value)
        {

            Regex regex = new Regex(@"^(?:0)(7(?:(?:[0-9][0-9]))[0-9]{6})$");
            bool isValid = regex.IsMatch(value);
            if (!isValid)
            {
                Console.Error.WriteLine($"Invalid phone number. Please enter a valid phone number e.g 0700123456");
                _logger.LogError($"Invalid phone number. Please enter a valid phone number e.g 0700123456");
            }
            return isValid;
        }
        public bool IsValidDate(string value)
        {
            DateTime date;
            if(!DateTime.TryParse(value, out date))
            {
                Console.Error.WriteLine($"Invalid date. Please enter a valid date e.g 6/9/2021");
                _logger.LogError($"Invalid date. Please enter a valid date e.g 6/9/2021");
                return false;
            }
            DateTime today = DateTime.Today;
            int age = today.Year - date.Year;
            if (age < 18)
            {
                Console.Error.WriteLine("Invalid age. All members age  must be 18+ ");
                _logger.LogError("Invalid age. All members age  must be 18+ ");
                return false;

            }
            return true;
        }
        public bool ValidateFile(string path)
        {
            if (!File.Exists(@path))
            {
                Console.WriteLine("Invalid file path! File does not exists.");
                _logger.LogError("Invalid file path! File does not exists.");
                return false;
            }
            string extension = Path.GetExtension(path);
            if(extension!=".csv" && extension != ".tsv")
            {
                Console.WriteLine("Invalid file format! Only csv and tsv files allowed.");
                _logger.LogError("Invalid file format! Only csv and tsv files allowed .");
                return false;

            }
            return true;
        }

    }
}
