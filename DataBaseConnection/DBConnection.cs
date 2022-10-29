using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections.Specialized;

namespace MyServer.DataBaseConnection
{
    public class DBConnection : IDisposable
    {
        public readonly string connectionString;
        private SqlConnection connection;

        public DBConnection(string connectionString = null)
        {
            if (connectionString == null)
                this.connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AppDB;Integrated Security=True";
            connection = new SqlConnection(this.connectionString);
        }

        /// <summary>
        /// query without return value
        /// </summary>
        public void Post(SqlQueryString queryString)
        {
            connection.Open();
            Debug.SendQueryToDBMsg();

            var command = new SqlCommand(queryString.GetQueryString(), connection);
            var result = command.ExecuteNonQuery();
            connection.Close();

            if (result > 0)
                Debug.QueryToDBSuccesMsg();
            else
                Debug.QueryToDBErrorMsg();
        }

        public object GetScalar(SqlQueryString queryString)
        {
            connection.Open();
            Debug.SendQueryToDBMsg();

            var command = new SqlCommand(queryString.GetQueryString(), connection);
            var result = command.ExecuteScalar();
            connection.Close();

            return result;
        }

        public T Get<T>(SqlQueryString queryString, Func<SqlDataReader, T> reader)
        {
            connection.Open();
            Debug.SendQueryToDBMsg();

            var command = new SqlCommand(queryString.GetQueryString(), connection);
            var result = reader(command.ExecuteReader());
            connection.Close();

            return result;
        }

        public void Get(SqlQueryString queryString, Action<SqlDataReader> reader)
        {
            connection.Open();
            Debug.SendQueryToDBMsg();

            var command = new SqlCommand(queryString.GetQueryString(), connection);
            reader(command.ExecuteReader());
            connection.Close();
        }

        public List<Dictionary<string, object>> Get(SqlQueryString queryString)
        {
            connection.Open();
            Debug.SendQueryToDBMsg();

            var command = new SqlCommand(queryString.GetQueryString(), connection);
            var result = ToList(command.ExecuteReader());
            connection.Close();

            return result;
        }

        private List<Dictionary<string, object>> ToList(SqlDataReader reader)
        {
            var count = reader.FieldCount;
            var list = new List<Dictionary<string, object>>();

            var names = new string[count];
            for (int i = 0; i < count; i++)
                names[i] = reader.GetName(i);

            while (reader.Read())
            {
                var collection = new Dictionary<string, object>();
                for (int i =0; i < count; i++)
                    collection.Add(names[i], reader.GetValue(i));
                list.Add(collection);
            }

            return list;
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
