using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptainSmackdown.ViewModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<CaptainViewInfo> Captains { get; set; }
    }
}