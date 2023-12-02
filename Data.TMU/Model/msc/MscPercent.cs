using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.TMU.Model.msc
{
    public class MscPercent
    {
        public int Id { get; set; }
        public string? PercentUser { get; set; }
        public string? Percent { get; set; }
        public bool status { get; set; }
        public string IdCodeUser { get; set; }
        public DateTime DetaNews { get; set; }
        public string Year { get; set; }
        public bool statusstatusface { get; set; } = false;
        [ForeignKey("Msc")]
        public int Idmsc { get; set; }
        public virtual msc? Msc { get; set; }
    }
    public class CategoryMscPercent
    {
        public int Id { get; set; }
        public int Idmsc { get; set; }
        public string? PercentUser { get; set; }
        public string? Percent { get; set; }
        public bool status { get; set; }
        public string IdCodeUser { get; set; }
        public string Activity { get; set; }
        public string letter_number { get; set; }
        public DateTime DetaNews { get; set; }
        public string Volume { get; set; }


    }

    public class ListMscPersentViewModel
    {
        public List<CategoryMscPercent> category { get; set; }
        public int IdPage { get; set; }
        public int CountPage { get; set; }
    }
}
