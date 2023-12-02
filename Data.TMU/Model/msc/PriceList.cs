using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.TMU.Model.msc
{
    public class PriceList
    {
        [Key]
        public int IdPL { get; set; }

        [DisplayName(" ضریب پیشنهادی ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string factoroffer { get; set; }
        [DisplayName(" واحد ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string unit { get; set; }


        [DisplayName(" مقدار ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string month { get; set; }

        [DisplayName(" مقدار ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string value { get; set; }

        [DisplayName(" مقدار انجام شده ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string valueUse { get; set; } = "0";

        [DisplayName(" دسته بندی ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string CaregouryActivity { get; set; }


        [DisplayName(" فعالیت ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string Activity { get; set; }
        [DisplayName(" هزینه واحد ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string Price { get; set; }

        [DisplayName(" هزینه کل ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string TotalPrice { get; set; }

        public string IdCode { get; set; }

        [DisplayName(" تاریخ ثبت ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public DateTime DetaNews { get; set; }
        public string Year { get; set; }


    }

    public class ListpricelistViewModel
    {
        public List<PriceList> priceLists { get; set; }
        public int IdPage { get; set; }
        public int CountPage { get; set; }
    }
}
