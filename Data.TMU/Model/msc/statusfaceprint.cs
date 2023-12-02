using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.TMU.Model.msc
{
    public class statusfaceprint
    {
        [Key]
        public int Idmsc { get; set; }
        public string activity { get; set; }
        public string numberEblagh { get; set; }
        public string dateEblagh { get; set; }
        public string volume { get; set; }
        public string volumeunit { get; set; }
        public string location { get; set; }
        public string priceOne { get; set; }
        public string price { get; set; }
        public string deadline { get; set; }
        public string description { get; set; }
        public string listpersentUser { get; set; }
        public string listpersent { get; set; }
        public string FullnameReciver { get; set; }
        public string Fullnamefinallapprov { get; set; }
        public string DateConfraim { get; set; }
        public string Leternumber { get; set; }

    }
}
