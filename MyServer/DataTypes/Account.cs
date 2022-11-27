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
            return Regex.IsMatch(Name, @"^\w{1,50}$") &&
                Regex.IsMatch(Gender, @"^[a-z]{1,20}$") &&
                Regex.IsMatch(Email, @"^[\w.]+@[\w.]+\.[a-z]{2,4}$") && Email.Length < 200 &&
                Regex.IsMatch(Password, @"[a-z]") && Regex.IsMatch(Password, @"[A-Z]") && 
                Regex.IsMatch(Password, @"[0-9]") && Password.Length >= 8 && Password.Length < 50;
        }
    }

    public class Accounts
    {
        private static MiniORM orm = new MiniORM("GavnoGamesDB");

        private static Account? GetSingle(string predicate, params (string, object)[] values)
        {
            return orm.Select<Account>()
                .Where(predicate, values)
                .Go<Account>()
                .SingleOrDefault();
        }

        public static Account? GetAccountBy(int id)
        {
            return GetSingle("Id = @id", ("@id", id));
        }

        public static Account? GetAccountBy(string email, string password)
        {
            return GetSingle("Email = @email AND Password = @password", ("@email", email), ("@password", password));
        }

        public static void InsertAccount(Account account)
        {
            orm.Insert(account).Go();
        }

        public static void Update(Account account, int id)
        {
            orm.Update(account)
                .Where("Id = @id", ("@id", id))
                .Go();
        }
    }
}
