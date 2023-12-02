using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.TMU.Model.msc
{
    public class MscUser
    {
        [Key]
        public int IdMU { get; set; }

        public int IdAU { get; set; }

        [DisplayName(" ایجاد کننده ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string idsender { get; set; }
        public string idreciver{ get; set; }
        public DateTime Deta { get; set; }
        public bool status { get; set; }
        public bool validation { get; set; }
        public string? statusface { get; set; }
        public int view { get; set; }
        [DisplayName("  توضیحات ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0}نمی تواند بیشتر از {1}باشد")]
        public string Description { get; set; }
        public string Year { get; set; }
        public string Volume { get; set; }
        public string TotalPrice { get; set; }
        public string? factoroffer { get; set; }

        [ForeignKey("Msc")]
        public int Idmsc { get; set; }
        public virtual msc? Msc { get; set; }
        //public virtual MscPercent? MscPercent { get; set; }

    }
    public class CategoryUserMsc
    {
        public int IdMU { get; set; }
        public int IdAU{ get; set; }
        public string Activity { get; set; }
        public string letter_number { get; set; }
        public string author { get; set; }
        public string reciver { get; set; }
        public string sender { get; set; }
        public DateTime DetaNews { get; set; }
        public bool status { get; set; }
        public int view { get; set; }
        public int idmsc { get; set; }
        public string Description { get; set; }
        public string Andicator { get; set; }
        public string deadline { get; set; }
        public bool Isclose { get; set; }

    }
    public class ListMscUserViewModel
    {
        public List<CategoryUserMsc> Projrct { get; set; }
        public int IdPage { get; set; }
        public int CountPage { get; set; }
    }
}
