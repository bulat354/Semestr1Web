using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Data.Sql;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyServer.Controllers;

namespace MyServer.TemplateEngines
{
    public static class TemplateEngine
    {
        public static string[] GenerateDocument(string[] template, Dictionary<string, string> data)
        {
            var namesRegex = new Regex(@"{{[A-Za-z0-9\.]+?}}");
            for (int i = 0; i < template.Length; i++)
            {
                var line = template[i];
                var newLine = new StringBuilder();
                var startIndex = 0;
                var matches = namesRegex.Matches(line);
                foreach (Match match in matches)
                {
                    var str = match.ToString().Substring(2, match.Length - 4);
                    if (data.ContainsKey(str))
                    {
                        newLine.Append(line.Substring(startIndex, match.Index - startIndex));
                        newLine.Append(data[str]);
                        startIndex = match.Index + match.Length;
                    }
                }
                newLine.Append(line.Substring(startIndex));
                template[i] = newLine.ToString();
            }
            return template;
        }

        public static HttpResponse MethodHandler(string filePath, ITemplateData data)
        {
            filePath = HttpServer.configs.LocalRoot + filePath;
            if (!File.Exists(filePath))
                return HttpResponse.GetNotFoundResponse();
            var result = string.Join(' ', GenerateDocument(File.ReadAllLines(filePath), data.ToDictionary()));
            return new HttpResponse(System.Net.HttpStatusCode.OK, result, FileManager.GetContentType(filePath));
        }

        public static string OpenBrackets(string template, Dictionary<string, string> data, bool isAuthorized)
        {
            var temp1 = OpenUserBrackets(template, data, isAuthorized);
            var temp2 = OpenRepeatBrackets(temp1, data);
            var temp3 = OpenValueBrackets(temp2, data);
            return OpenConditionBrackets(temp3, data);
        }

        /// <summary>
        /// Если пользователь авторизован на сайте, то области выделенные так: '|!%some code here%|' исчезнут, а области '|%some code here%|' останутся.
        /// Если же пользователь не авторизован, то все наоборот.
        /// </summary>
        public static string OpenUserBrackets(string template, Dictionary<string, string> data, bool isAuthorized)
        {
            var startPos = 0;
            var result = new StringBuilder();
            foreach (Match match in Regex.Matches(template, @"\|!?%[^(|!%)(|%)(%|)]+?%\|", RegexOptions.Singleline))
            {
                var brackets = new UserBrackets(match);
                if (brackets.IsAuthorized == isAuthorized)
                {
                    startPos = result.AppendReplacedMatch(startPos, template, brackets);
                }
                else
                {
                    startPos = result.AppendWithoutMatch(startPos, template, brackets);
                }
            }
            result.Append(template.Substring(startPos));
            return result.ToString();
        }

        public static string OpenRepeatBrackets(string template, Dictionary<string, string> data)
        {
            var startPos = 0;
            var result = new StringBuilder();
            foreach (Match match in Regex.Matches(template, @"\[\d+\[[^\]\[]+?\]\d+\]", RegexOptions.Singleline))
            {
                var brackets = new RepeatBrackets(match);
                startPos = result.AppendWithoutMatch(startPos, template, brackets);
                for (int i = brackets.StartIndex; i < brackets.EndIndex; i++)
                {
                    if (data.Any(x => Regex.IsMatch(x.Key, i.ToString())))
                    {
                        var str = brackets.GetContent(i);
                        result.Append(str);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            result.Append(template.Substring(startPos));
            return result.ToString();
        }

        public static string OpenValueBrackets(string template, Dictionary<string, string> data)
        {
            var startPos = 0;
            var result = new StringBuilder();
            foreach (Match match in Regex.Matches(template, @"{{[^({{)(}})\=\?\:]+?}}", RegexOptions.Singleline))
            {
                var brackets = new ValueBrackets(match);
                startPos = result.AppendReplacedMatch
                    (startPos, template, brackets, brackets.GetContent(data));
            }
            result.Append(template.Substring(startPos));
            return result.ToString();
        }

        public static string OpenConditionBrackets(string template, Dictionary<string, string> data)
        {
            var startPos = 0;
            var result = new StringBuilder();
            foreach (Match match in Regex.Matches
                (template, 
                @"{{[^({{)(}})\=\?\:]*=[^({{)(}})\=\?\:]*\?[^({{)(}})\=\?\:]*:[^({{)(}})\=\?\:]*}}", 
                RegexOptions.Singleline))
            {
                var brackets = new ConditionBrackets(match);
                startPos = result.AppendReplacedMatch
                    (startPos, template, brackets, brackets.GetContent(data));
            }
            result.Append(template.Substring(startPos));
            return result.ToString();
        }

        private static int AppendWithoutMatch
            (this StringBuilder builder, int start, string template, Brackets br)
        {
            builder.Append(template.Substring(start, br.Index - start));
            return br.Index + br.Length;
        }

        private static int AppendReplacedMatch
            (this StringBuilder builder, int start, string template, Brackets br)
        {
            return builder.AppendReplacedMatch(start, template, br, br.Content);
        }

        private static int AppendReplacedMatch
            (this StringBuilder builder, int start, string template, Brackets br, string replace)
        {
            builder.Append(template.Substring(start, br.Index - start));
            builder.Append(replace);
            return br.Index + br.Length;
        }

        private abstract class Brackets
        {
            public string Content;
            public int Index;
            public int Length;

            public Brackets(Match match)
            {
                Index = match.Index;
                Length = match.Length;
            }
        }

        /// <summary>
        /// |%code%| or |!%code%|
        /// </summary>
        private class UserBrackets : Brackets
        {
            public bool IsAuthorized;

            public UserBrackets(Match match) : base(match)
            {
                var str = match.ToString();
                if (str.StartsWith("|!%"))
                {
                    Content = str.Substring(3, str.Length - 5);
                    IsAuthorized = false;
                }
                else if (str.StartsWith("|%"))
                {
                    Content = str.Substring(2, str.Length - 4);
                    IsAuthorized = true;
                }
                else
                    Content = str;
            }
        }

        /// <summary>
        /// {{key}}
        /// </summary>
        private class ValueBrackets : Brackets
        {
            public ValueBrackets(Match match) : base(match)
            {
                var str = match.ToString();
                Content = str.Substring(2, str.Length - 4);
            }

            public string GetContent(Dictionary<string, string> pairs)
            {
                if (pairs.ContainsKey(Content))
                {
                    return pairs[Content];
                }

                return Content;
            }
        }

        /// <summary>
        /// {{key=value?true:false}}
        /// </summary>
        private class ConditionBrackets : Brackets
        {
            public string Key;
            public string Value;
            public string True;
            public string False;

            public ConditionBrackets(Match match) : base(match)
            {
                var str = match.ToString();
                Content = str.Substring(2, str.Length - 4);
                var data = Content.Split('=', '?', ':');
                if (data.Length != 4)
                    throw new ArgumentException();
                Key = data[0];
                Value = data[1];
                True = data[2];
                False = data[3];
            }

            public string GetContent(Dictionary<string, string> data)
            {
                if (data.ContainsKey(Key))
                {
                    var value = data[Key];
                    return Value.Equals(value) ? True : False;
                }

                return Content;
            }
        }

        /// <summary>
        /// [start[code]maxcount]
        /// </summary>
        private class RepeatBrackets : Brackets
        {
            public int StartIndex;
            public int EndIndex;

            public RepeatBrackets(Match match) : base(match)
            {
                var str = match.ToString();
                var temp = str.Substring(1, str.Length - 2);
                var first = temp.IndexOf('[');
                var last = temp.LastIndexOf(']');
                StartIndex = int.Parse(temp.Substring(0, first));
                EndIndex = int.Parse(temp.Substring(last + 1, temp.Length - last - 1));
                Content = temp.Substring(first + 1, last - first - 1);
            }

            public string GetContent(int i)
            {
                return Content.Replace("@", i.ToString());
            }
        }
    }
}
