#region License
//
// Copyright (c) 2007-2018, Sean Chambers <schambers80@gmail.com>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System.Collections.Generic;
using System.IO;
using FluentMigrator.Infrastructure;

namespace FluentMigrator.Expressions
{
    public class ExecuteSqlScriptExpression : MigrationExpressionBase
    {
        public string SqlScript { get; set; }

        /// <summary>
        /// Gets or sets parameters to be replaced before script execution
        /// </summary>
        public IDictionary<string, string> Parameters { get; set; }

        public override void ExecuteWith(IMigrationProcessor processor)
        {
            string sqlText;
            using (var reader = File.OpenText(SqlScript))
                sqlText = reader.ReadToEnd();

            sqlText = SqlScriptTokenReplacer.ReplaceSqlScriptTokens(sqlText, Parameters);

            processor.Execute(sqlText);
        }

        public override void ApplyConventions(IMigrationConventions conventions)
        {
            SqlScript = Path.Combine(conventions.GetWorkingDirectory(), SqlScript);
        }

        public override void CollectValidationErrors(ICollection<string> errors)
        {
            if (string.IsNullOrEmpty(SqlScript))
                errors.Add(ErrorMessages.SqlScriptCannotBeNullOrEmpty);
        }

        public override string ToString()
        {
            return base.ToString() + SqlScript;
        }
    }
}
