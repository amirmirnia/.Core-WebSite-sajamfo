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
    public class msc
    {
        [Key]
        public int Idmsc { get; set; }

        public int IdmscM { get; set; } = 0;

        [DisplayName(" ضریب پیشنهادی ")]
        
        public string? factoroffer { get; set; }

        [DisplayName(" دسته فعالیت ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string CaregouryActivity { get; set; }

        [DisplayName(" فعالیت ")]
        //[Required(ErrorMessage = "{0}را وارد کنید")]
        public string? Activity { get; set; }
        [DisplayName(" حجم کار ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string Volume { get; set; }
        [DisplayName(" واحد  ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string Unit { get; set; }
        [DisplayName(" محل اجرا ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string Location { get; set; }
        [DisplayName(" قیمت تخمینی(ریال) ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string Price { get; set; }

        [DisplayName(" قیمت واحد(ریال) ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string OnePrice { get; set; }

        [DisplayName(" مهلت انجام کار ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string deadline { get; set; }
        [DisplayName(" توضیحات ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0}نمی تواند بیشتر از {1}باشد")]
        public string Description { get; set; }

        [DisplayName("کد رهگیری  ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string letter_number { get; set; }
        [DisplayName("شماره نامه")]
        public int number { get; set; } = 0;

        [DisplayName(" اندیکاتور ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string Andicator { get; set; } = "0";
        [DisplayName("ایجاد کننده")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string author { get; set; }

        [DisplayName("شماره چاه")]
        public string? Well { get; set; }
        [DisplayName("شماره سری")]
        public string? Seri { get; set; }
        [DisplayName("پارسل")]
        public string? parcel { get; set; }

        [DisplayName(" تاریخ ثبت ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public DateTime DetaNews { get; set; }
        public bool Isclose { get; set; }

        [ForeignKey("User")]
        public int IdAuthor { get; set; }
        public virtual User? User { get; set; }
        public virtual List<MscUser>? MscUser { get; set; }

    }

    public class CategoryMsc
    {
        public int Idmsc { get; set; }
        public int Idauthor { get; set; }
        public string Activity { get; set; }
        public string letter_number { get; set; }
        public string author { get; set; }
        public string Volume { get; set; }
        public string Location { get; set; }
        [DisplayName("شماره چاه")]
        public string? Well { get; set; }
        [DisplayName("شماره سری")]
        public string? Seri { get; set; }
        [DisplayName("پارسل")]
        public string? parcel { get; set; }
        public string Price { get; set; }
        public DateTime DetaNews { get; set; }
        public string deadline { get; set; }
        public string Andicator { get; set; } = "0";

    }
    public class ListMscViewModel
    {
        public List<CategoryMsc> cartabl { get; set; }
        public int IdPage { get; set; }
        public int CountPage { get; set; }
    }
    public class Print
    {
        public int[] item { get; set; }
        public int idmsc { get; set; }
        public string filename { get; set; }
        public string SingnacharName { get; set; }
        public string SingnacharFinalapprovName { get; set; }
    }
    
}
