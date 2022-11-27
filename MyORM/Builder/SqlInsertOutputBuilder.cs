﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Builder
{
    public class SqlInsertOutputBuilder : SqlReaderBuilder
    {
        private List<string> values = new List<string>();
        private string[] columns = null;
        private string source = null;

        public SqlInsertOutputBuilder(SqlCommand command, MiniORM orm) : base(command, orm)
        {
        }

        public override SqlCommand GetSqlCommand()
        {
            var text = new StringBuilder();
            text.Append("INSERT");
            if (source != null) text.Append($" INTO {source}");
            if (columns != null) text.Append($"\nOUTPUT {string.Join(", ", columns.Select(x => $"inserted.{x}"))}");
            text.Append("\nVALUES\n");
            if (values != null) text.Append(string.Join("\n", values));

            _command.CommandText = text.ToString();
            return _command;
        }

        public SqlInsertOutputBuilder Insert<T>(params T[] objs)
        {
            if (source != null)
                return this;

            source = EntityModel.GetName<T>();

            var properties = EntityModel.GetEditableProperties<T>().ToArray();
            var names = properties.Select(x => EntityModel.GetColumnName(x)).ToArray();
            columns = EntityModel.GetVisibleProperties<T>()
                .Select(x => EntityModel.GetColumnName(x)).ToArray();
            source += $"({string.Join(", ", names)})";

            for (int i = 0; i < objs.Length; i++)
            {
                var paramNames = names.Select(x => $"@{x}{i}").ToArray();
                values.Add($"({string.Join(", ", paramNames)})");
                _command.Parameters.AddRange(properties
                    .Zip(paramNames, (p, n) => new SqlParameter(n, p.GetValue(objs[i])))
                    .ToArray());
            }

            return this;
        }
    }
}
