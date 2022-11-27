using HtmlEngineLibrary;
using MyORM;
using MyServer.Attributes;
using MyServer.DataTypes;
using MyServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Controllers
{
    [ApiController("html")]
    public class PagesController : PageControllerBase
    {
        private static MiniORM orm = new MiniORM("GavnoGamesDB");

        [HttpGet("signin")]
        public void GetSignInPage()
        {
            GenerateAndSend("signin.html", Init(new PageContent(), "Войти", "signin"));
        }

        [HttpGet("signup")]
        public void GetSignUpPage()
        {
            GenerateAndSend("signup.html", Init(new PageContent(), "Регистрация", "signin"));
        }

        [HttpGet("profile")]
        public void GetProfilePage()
        {
            if (!IsAuthorized)
            {
                Unauthorized();
                return;
            }

            var account = Accounts.GetAccountBy(CurrentSession.AccountId);
            if (account == null)
            {
                RemoveCookie("SessionId");
                Unauthorized();
            }

            GenerateAndSend("profile.html", Init(new ProfilePageContent() { Account = account }, "Профиль", "signin"));
        }

        class ProfilePageContent : PageContent
        {
            public Account Account { get; set; }
        }

        [HttpGet("game")]
        public void GetGamePage(int id)
        {
            var game = GameInfos.GetGameBy(id);
            if (game == null)
            {
                NotFound();
                return;
            }    

            GenerateAndSend("game.html", Init(new GamePageContent() { Game = GameInfos.LoadImage(game) }, game.Title, "game"));
        }

        [HttpGet("article")]
        public void GetArticlePage(int id)
        {
            var article = Articles.GetArticleBy(id);
            if (article == null)
            {
                NotFound();
                return;
            }

            GenerateAndSend("article.html", Init(new ArticlePageContent() { Article = Articles.LoadImage(article) }, article.Title, "article"));
        }

        class GamePageContent : PageContent
        {
            public GameInfo Game { get; set; }
        }

        class ArticlePageContent : PageContent
        {
            public Article Article { get; set; }
        }

        [HttpGet("games")]
        public void GetGamesPage()
        {
            GenerateAndSend("games.html", Init(new PageContent(), "Игры", "search", "games"));
        }

        [HttpGet("search")]
        public void GetSearchPage()
        {
            GenerateAndSend("search.html", Init(new PageContent(), "Новости", "search"));
        }
    }
}
