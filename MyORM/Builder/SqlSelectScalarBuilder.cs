using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Builder
{
    public class SqlSelectScalarBuilder : SqlScalarBuilder
    {
        private bool isDistinct = false;
        private string function = null;
        private string source = null;
        private string searchCondition = null;
        private string column = null;

        public SqlSelectScalarBuilder(SqlCommand command, MiniORM orm) : base(command, orm)
        {
        }

        public override SqlCommand GetSqlCommand()
        {
            var text = new StringBuilder();
            text.Append($"SELECT {function}(");
            if (isDistinct) text.Append("DISTINCT ");
            if (column != null) text.Append($"{column}");
            else text.Append($"*");
            text.Append(')');
            if (source != null) text.Append($"\nFROM {source}");
            if (searchCondition != null) text.Append($"\nWHERE {searchCondition}");

            _command.CommandText = text.ToString();

            return _command;
        }

        public SqlSelectScalarBuilder Count<T>(string column = null)
        {
            if (source != null)
                return this;

            source = EntityModel.GetName<T>();
            function = "COUNT";
            return this;
        }

        public SqlSelectScalarBuilder Min<T>(string column = null)
        {
            if (source != null)
                return this;

            source = EntityModel.GetName<T>();
            function = "MIN";
            return this;
        }

        public SqlSelectScalarBuilder Max<T>(string column = null)
        {
            if (source != null)
                return this;

            source = EntityModel.GetName<T>();
            function = "MAX";
            return this;
        }

        public SqlSelectScalarBuilder Avg<T>(string column = null)
        {
            if (source != null)
                return this;

            source = EntityModel.GetName<T>();
            function = "AVG";
            return this;
        }

        public SqlSelectScalarBuilder Sum<T>(string column = null)
        {
            if (source != null)
                return this;

            source = EntityModel.GetName<T>();
            function = "SUM";
            return this;
        }

        public SqlSelectScalarBuilder Distinct()
        {
            isDistinct = true;
            return this;
        }

        public SqlSelectScalarBuilder Where(string condition, params (string, object)[] parameters)
        {
            if (searchCondition != null)
                return this;

            searchCondition = condition;
            _command.Parameters.AddRange(parameters
                                            .Select(x => new SqlParameter(x.Item1, x.Item2))
                                            .ToArray());
            return this;
        }
    }
}
