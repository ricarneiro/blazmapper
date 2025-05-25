using BlazMapper.DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazMapper.DomainSample.ValueObjects
{
    public class CompleteName : AValueObject
    {
        public CompleteName(string fullName)
        {
            this.FullName = fullName;
            var names = fullName.Split(' ');
            if (names.Length >= 2)
            {
                this.FirstName = names[0];
                this.LastName = names[names.Length - 1];
            }
            else
            {
                this.FirstName = fullName;
                this.LastName = string.Empty;
            }
            GetValidationExpression();
        }

        public CompleteName(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.FullName = $"{firstName} {lastName}";
            GetValidationExpression();
        }

        public string FullName { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;

        public override bool GetValidationExpression()
        {
            return this.IsValid = this.IsValidName(FullName);
        }

        public override string ToString()
        {
            return this.FullName;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.FullName;
        }

        private bool IsValidName(string fullName)
        {
            return !string.IsNullOrWhiteSpace(fullName) && fullName.Contains(' ');
        }

        public static implicit operator string(CompleteName name) => name.FullName;
        public static implicit operator CompleteName(string fullname) => new CompleteName(fullname);
    }
}
