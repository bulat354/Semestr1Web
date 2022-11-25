using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyORM.Attributes;
using MyORM;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace MyServer.DataTypes
{
    [Table("Accounts")]
    public class Account
    {
        [Key, Identity]
        public int Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public string Name { get; set; }
        public string Gender { get; set; }

        public bool IsCorrect()
        {
            return Regex.IsMatch(Name, @"^[А-Яа-яA-Za-z]+$") &&
                Regex.IsMatch(Gender, @"^[a-z]+$") &&
                Regex.IsMatch(Email, @"^[a-z0-9\-\+\._%]+%40[a-z0-9\.\-]+\.[a-z]{2,4}$") && Email.Length < 200 &&
                Regex.IsMatch(Password, @"[a-z]") && Regex.IsMatch(Password, @"[A-Z]") && 
                Regex.IsMatch(Password, @"[0-9]");
        }
    }
}
