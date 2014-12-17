using CaptainSmackdown.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Humanizer;

namespace CaptainSmackdown.ViewModels.Home
{
    public class CaptainViewInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Votes { get; set; }


        public CaptainViewInfo(Captains capt)
        {
            Id = (int)capt;
            Name = string.Format("Captain {0}", capt.Humanize(LetterCasing.Title));
            Image = string.Format("{0}.png", capt.ToString());
        }
    }
}