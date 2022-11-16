using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyORM.Attributes;
using MyORM;
using System.Data.SqlClient;

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
