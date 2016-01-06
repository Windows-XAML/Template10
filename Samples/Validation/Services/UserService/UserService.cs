using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Utils;
using Template10.Validation;

namespace Sample.Services.UserService
{
    public class UserService
    {
        public IEnumerable<Models.User> GetUsers()
        {
            var id = 1;
            yield return BuildUser(id++, "Jonathan", "Archer");
            yield return BuildUser(id++, "T'Pol", "Main");
            yield return BuildUser(id++, "Charles 'Trip'", "Tucker III");
            yield return BuildUser(id++, "Malcolm", "Reed");
            yield return BuildUser(id++, "Hoshi", "Sato Main");
            yield return BuildUser(id++, "Travis", "Mayweather");
            yield return BuildUser(id++, "Doctor", "Phlox");
            yield return BuildUser(id++, "Thy'lek", "Shran");
            yield return BuildUser(id++, "Maxwell", "Forrest");
            yield return BuildUser(id++, "Matt", "Winston");
        }

        public void ValidateUser(IModel model)
        {
            var user = model as Models.User;

            if (string.IsNullOrEmpty(user.FirstName))
                user.Properties[nameof(user.FirstName)].Errors.Add("First name is required.");
            else if (user.FirstName.Length < 2)
                user.Properties[nameof(user.FirstName)].Errors.Add("First name length is invalid.");

            if (string.IsNullOrEmpty(user.LastName))
                user.Properties[nameof(user.LastName)].Errors.Add("Last name is required.");
            else if (user.LastName.Length < 2)
                user.Properties[nameof(user.LastName)].Errors.Add("Last name length is invalid.");

            if (string.IsNullOrEmpty(user.Email))
                user.Properties[nameof(user.Email)].Errors.Add("Email is required.");
            else if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(user.Email))
                user.Properties[nameof(user.Email)].Errors.Add("A valid Email is required.");

            if (!user.IsAdmin)
            {
                var date = DateTime.Now.Subtract(TimeSpan.FromDays(365 * 20));
                if (user.Birth > date)
                    user.Properties[nameof(user.Birth)].Errors.Add($"Must be older than 20 years; after {date}");
            }

            var admin = user.Properties[nameof(user.IsAdmin)] as Property<bool>;
            if (admin.OriginalValue && !admin.Value)
                admin.Errors.Add("Administrator cannot be demoted.");
        }

        Random _random = new Random((int)DateTime.Now.Ticks);
        Models.User BuildUser(int id, string first, string last)
        {
            var user = new Models.User
            {
                FirstName = first,
                LastName = last,
                Birth = DateTime.Now.Subtract(TimeSpan.FromDays(_random.Next(19, 40) * 365)),
                Email = $"{last}@domain.com",
                IsAdmin = id % 3 == 0,
                Validator = ValidateUser
            };
            user.Validate();
            return user;
        }
    }
}
