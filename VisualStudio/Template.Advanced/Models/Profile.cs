using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Validation;

namespace Sample.Models
{
    public class Profile : ValidatableModelBase
    {
        public Profile() : base()
        {
            Validator = e =>
            {
                if (string.IsNullOrEmpty(FirstName))
                {
                    Errors.Add("First Name is required.");
                }
                else if (FirstName.Length < 5)
                {
                    Errors.Add("First Name must be at least 5 characters.");
                }
                if (string.IsNullOrEmpty(LastName))
                {
                    Errors.Add("Last Name is required.");
                }
                else if (LastName.Length < 5)
                {
                    Errors.Add("Last Name must be at least 5 characters.");
                }
                if (!TestEmail(Email))
                {
                    Errors.Add("Email is not valid.");
                }
                if (string.IsNullOrEmpty(Web))
                {
                    Errors.Add("Web is required.");
                }
            };
        }

        Property<string> _key = new Property<string>();
        public string Key
        {
            get => _key.Value;
            set => _key.Value = value;
        }

        Property<string> _firstName = new Property<string>();
        public string FirstName
        {
            get => _firstName.Value;
            set => _firstName.Value = value;
        }

        Property<string> _lastName = new Property<string>();
        public string LastName
        {
            get => _lastName.Value;
            set => _lastName.Value = value;
        }

        Property<string> _email = new Property<string>();
        public string Email
        {
            get => _email.Value;
            set => _email.Value = value;
        }

        Property<string> _web = new Property<string>();
        public string Web
        {
            get => _web.Value;
            set => _web.Value = value;
        }

        bool TestEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            try
            {
                new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
