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
    public class AndicatorUser
    {

        [Key]
        public int IdAU { get; set; }
        [DisplayName(" شماره نامه ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string LetterNumber { get; set; }
        [DisplayName(" اندیکاتور  ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string Andicator { get; set; }

        [DisplayName(" تاریخ ثبت ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public DateTime Deta { get; set; }

        [DisplayName(" شماره ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public int Number { get; set; } = 0;
        [DisplayName(" کد ملی  ")]
        [Required(ErrorMessage = "{0}را وارد کنید")]
        public string IdCode { get; set; }
        public string IdMSC { get; set; }

        [ForeignKey("User")]
        public int IDUser { get; set; }
        public virtual User? User { get; set; }


    }
}
