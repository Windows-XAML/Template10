using System;
using Template10.Validation;

namespace Template10.Samples.ValidationSample.Models
{
    public class User : ModelBase
    {
        public int Id { get; set; }
        public string FirstName { get { return Read<string>(); } set { Write(value); } }
        public string LastName { get { return Read<string>(); } set { Write(value); } }
        public string Email { get { return Read<string>(); } set { Write(value); } }
        public DateTimeOffset Birth { get { return Read<DateTimeOffset>(); } set { Write(value); } }
        public bool IsAdmin { get { return Read<bool>(); } set { Write(value); } }
        public override string ToString() => $"{FirstName} {LastName}";
    }
}
