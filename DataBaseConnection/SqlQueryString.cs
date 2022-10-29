using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.DataBaseConnection
{
    public struct SqlQueryString
    {
        private string queryString;

        public SqlQueryString(string queryString)
        {
            this.queryString = queryString == null ? "" : queryString;
        }

        public string GetQueryString() => queryString;

        #region Селекторы
        public SqlQueryString Select(params string[] columns) =>
            AddElement("SELECT", columns.Enumerate());
        public SqlQueryString SelectDistinct(params string[] columns) =>
            AddElement("SELECT DISTINCT", columns.Enumerate());
        public SqlQueryString From(params string[] tables) =>
            AddElement("FROM", tables.Enumerate());
        public SqlQueryString Join(string table) =>
            AddElement("JOIN", table);
        #endregion

        #region Выборка
        public SqlQueryString Where(string condition) =>
            AddElement("WHERE", condition);
        public SqlQueryString Having(string condition) =>
            AddElement("HAVING", condition);
        public SqlQueryString On(string condition) =>
            AddElement("ON", condition);
        #endregion

        #region Группировка и сортировка
        public SqlQueryString GroupBy(params string[] columns) =>
            AddElement("GROUP BY", columns.Enumerate());
        public SqlQueryString OrderBy(params string[] columns) =>
            AddElement("ORDER BY", columns.Enumerate());
        public SqlQueryString OrderByDescending(params string[] columns) =>
            AddElement("ORDER BY", columns.Enumerate(), "DESC");
        #endregion


        #region Агрегатные функции
        public string Max(string column, bool isDistinct = false) =>
            isDistinct ? GetFuncDistinct("MAX", column) : GetFunc("MAX", column);
        public string Min(string column, bool isDistinct = false) =>
            isDistinct ? GetFuncDistinct("MIN", column) : GetFunc("MIN", column);
        public string Count(string column, bool isDistinct = false) =>
            isDistinct ? GetFuncDistinct("COUNT", column) : GetFunc("COUNT", column);
        public string Sum(string column, bool isDistinct = false) =>
            isDistinct ? GetFuncDistinct("SUM", column) : GetFunc("SUM", column);
        public string Avg(string column, bool isDistinct = false) =>
            isDistinct ? GetFuncDistinct("AVG", column) : GetFunc("AVG", column);
        #endregion


        #region Подзапрос
        public string GetSubqueryString(SqlQueryString queryString) =>
            $"(\n{queryString.GetQueryString()}\n)";
        #endregion


        #region Условия
        public string Brackets(string value) =>
            $"({value})";
        public string String(string value) =>
            $"'{value}'";
        public string And(string first, string second) =>
            $"{first} AND {second}";
        public string Or(string first, string second) =>
            $"{first} OR {second}";
        public string Between(string value, string start, string end) =>
            $"{value} BETWEEN {And(start, end)}";
        public string Like(string value, string pattern) =>
            $"{value} LIKE {pattern}";
        public string In(string value, params string[] values) =>
            $"{value} IN {List(values)}";
        #endregion


        #region Создание и изменение данных
        public SqlQueryString Insert(string tableName, params string[] columnsList) =>
            AddElement("INSERT INTO", $"{tableName}{List(columnsList)}");
        public SqlQueryString Insert(string tableName) =>
            AddElement("INSERT INTO", tableName);
        public SqlQueryString Delete(string tableName) =>
            AddElement("DELETE FROM", tableName);
        public SqlQueryString Values(params string[] values) =>
            AddElement("VALUES", string.Join(",\n", values));

        public SqlQueryString Update(string table) =>
            AddElement("UPDATE", table);
        public SqlQueryString Set(params string[] set) =>
            AddElement("SET", set.Enumerate());
        #endregion

        #region Остальное
        public string List(params string[] values) =>
            $"({string.Join(", ", values)})";
        public string Set(string column, string value) =>
            $"{column}={value}";
        #endregion

        #region Добавление элементов запроса
        public SqlQueryString AddElement(string elementName, string value) =>
            new SqlQueryString($"{queryString}\n{elementName} {value}");
        public SqlQueryString AddElement(string elementName, string value, string ending) =>
            new SqlQueryString($"{queryString}\n{elementName} {value} {ending}");
        public SqlQueryString AddElement(string element) =>
            new SqlQueryString($"{queryString}\n{element}");
        #endregion

        #region Создание агрегатной функции
        public string GetFunc(string name, string value) =>
            $"{name.ToUpper()}({value})";
        public string GetFuncDistinct(string name, string value) =>
            $"{name.ToUpper()}(DISTINCT {value})";
        #endregion
    }

    public static class ArrayExtensions
    {
        public static string Enumerate(this string[] elements) =>
            string.Join(", ", elements);
    }
}
