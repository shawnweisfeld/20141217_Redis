using CaptainSmackdown.Models;
using CaptainSmackdown.ViewModels.Home;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Humanizer;

namespace CaptainSmackdown.Controllers
{
    public class HomeController : Controller
    {
        private static ConnectionMultiplexer connection;
        private static ConnectionMultiplexer Connection
        {
            get
            {
                if (connection == null || !connection.IsConnected)
                {
                    connection = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisConnection"]);
                }
                return connection;
            }
        }

        public ActionResult Index()
        {
            var vm = new IndexViewModel();
            vm.Captains = Enum.GetValues(typeof(Captains))
                .Cast<Captains>()
                .Select(x => new CaptainViewInfo(x));

            return View(vm);
        }

        public async Task<ActionResult> LederBoard()
        {
            IDatabase db = Connection.GetDatabase();

            //Get the top 4 vote getting captains
            var tops = (await db.SortedSetRangeByRankWithScoresAsync("votes", 
                stop: 3, order: Order.Descending))
                .Select(x => new CaptainViewInfo((Captains)((int)x.Element))
                { 
                    Votes = (int)x.Score
                });

            return View(new LeaderBoardViewModel() 
            {
                Leader = tops.FirstOrDefault(),
                RunnersUp = tops.Skip(1).Take(3)
            });
        }

        public async Task<ActionResult> Vote(string id)
        {
            IDatabase db = Connection.GetDatabase();
            
            //Add a vote for the selected captain
            await db.SortedSetIncrementAsync("votes", id, 1);

            return RedirectToAction("LederBoard");
        }

        
    }
}