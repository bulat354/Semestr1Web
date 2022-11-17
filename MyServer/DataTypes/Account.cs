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
        private static MiniORM orm = new MiniORM("GavnoGamesDB");

        [Key, Identity]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Password { get; set; }
        [Required, MaxLength(50)]
        public string Email { get; set; }

        public string Surname { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Level { get; set; }
        public string Nickname { get; set; }
        public string Tel { get; set; }

        public bool IsCorrect()
        {
            return Regex.IsMatch(Surname, @"^([А-Яа-яA-Za-z]+\-)?[А-Яа-яA-Za-z]+$") &&
                Regex.IsMatch(Name, @"^[А-Яа-яA-Za-z]+$") &&
                BirthDate.CompareTo(DateTime.Now) < 0 &&
                Regex.IsMatch(Gender, @"^[a-z]+$") &&
                Regex.IsMatch(Level, @"^[a-z]+$") &&
                !Regex.IsMatch(Nickname, @"--") &&
                Regex.IsMatch(Tel, @"^\+?[0-9]{1,3}\(?[0-9]{3}\)?[0-9]{3}\-?[0-9]{2}\-?[0-9]{2}$") &&
                Regex.IsMatch(Email, @"^[a-z0-9\-\+\._%]+%40[a-z0-9\.\-]+\.[a-z]{2,4}$") && Email.Length < 200 &&
                Regex.IsMatch(Password, @"[a-z]") && Regex.IsMatch(Password, @"[A-Z]") && 
                Regex.IsMatch(Password, @"[0-9]");
        }

        #region With ORM only
        public static List<Account> GetAll()
        {
            return orm
                .Select<Account>()
                .Go<Account>()
                .ToList();
        }
        public static Account? GetAccountById(int id)
        {
            return orm
                .Select<Account>()
                .Where("Id = @id", ("@id", id))
                .Go<Account>()
                .SingleOrDefault();
        }
        public static Account? CheckAccount(Account account)
        {
            return orm
                .Select<Account>()
                .Where("Email = @email AND Password = @pass",
                    ("@email", account.Email),
                    ("@pass", account.Password))
                .Go<Account>()
                .SingleOrDefault();
        }
        public static bool Delete(Account account)
        {
            return orm
                .Delete<Account>()
                .Where("Id = @id", ("@id", account.Id))
                .Go() > 0;
        }
        public static bool Insert(Account account)
        {
            return orm
                .Insert(account)
                .Go() > 0;
        }
        public static bool Update(Account account)
        {
            return orm
                .Update(account)
                .Where("Id = @id", ("@id", account.Id))
                .Go() > 0;
        }
        #endregion
    }
}
