using HtmlEngineLibrary.TemplateRendering;
using MyORM;
using MyServer.Attributes;
using MyServer.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Controllers
{
    [ApiController("data")]
    public class DataController : ControllerBase
    {
        [HttpGet("searchgames")]
        public void SearchGames(string text)
        {
            GenerateAndSend("templates/game-search-result.html",
                new GameSearchResult() { Games = GameInfos.GetAll(text).Select(GameInfos.LoadImage).ToArray() });
        }

        [HttpGet("search")]
        public void SearchArticles(string text, int page)
        {
            GenerateAndSend("templates/search-result.html",
                new ArticleSearchResult() { Articles = Articles.GetAll(2, (page - 1) * 2, text).Select(Articles.LoadImage).ToArray() });
        }

        [HttpGet("count")]
        public int GetPagesCount(string text)
        {
            return Articles.GetCount(text) / 2;
        }

        [HttpGet("comment")]
        public void GetComment(int id)
        {
            SendComment(Comments.GetBy(id));
        }

        [HttpGet("allcomments")]
        public void GetComments(int id)
        {
            var comments = Comments.GetAll(id)
                .Select(x => new CommentContent() 
                { 
                    Comment = Comments.LoadAccount(x), 
                    IsCreator = x.AccountId == CurrentSession.AccountId  
                });

            var template = Template.Create(File.ReadAllText("templates/comment.html"));

            var result = new StringBuilder();
            foreach (var comment in comments)
            {
                result.AppendLine(template.Render(comment));
            }

            Ok(result.ToString());
        }

        private void SendComment(Comment comment)
        {
            GenerateAndSend("templates/comment.html",
                new CommentContent() 
                { 
                    Comment = Comments.LoadAccount(comment), 
                    IsCreator = comment.AccountId == CurrentSession.AccountId 
                });
        }

        [HttpPost("addcomment")]
        public void AddComment(string text, int id)
        {
            var comment = new Comment() 
            { 
                Content = text, 
                ArticleId = id, 
                AccountId = CurrentSession.AccountId, 
                Date = DateTime.Now 
            };

            try
            {
                comment = Comments.Insert(comment).Single();
            }
            finally
            {
                SendComment(comment);
            }
        }

        [HttpPost("editcomment")]
        public void EditComment(int id, string text)
        {
            if (!IsAuthorized)
                return;

            var comment = Comments.GetBy(id);
            if (comment.AccountId != CurrentSession.AccountId)
                return;

            if (text != null && text.Length > 0)
            {
                comment.Content = text;
                comment.Date = DateTime.Now;

                try
                {
                    Comments.Update(comment, id);
                }
                catch { }
            }

            SendComment(comment);
        }

        class GameSearchResult
        {
            public GameInfo[] Games;
        }

        class ArticleSearchResult
        {
            public Article[] Articles;
        }

        class CommentContent
        {
            public Comment Comment;
            public bool IsCreator = false;
        }
    }
}
