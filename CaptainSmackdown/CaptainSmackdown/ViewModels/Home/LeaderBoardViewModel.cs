using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptainSmackdown.ViewModels.Home
{
    public class LeaderBoardViewModel
    {
        public CaptainViewInfo Leader { get; set; }
        public IEnumerable<CaptainViewInfo> RunnersUp { get; set; }
    }
}