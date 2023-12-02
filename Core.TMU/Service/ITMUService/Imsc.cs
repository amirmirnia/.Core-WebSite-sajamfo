using Data.TMU.Model.msc;
using Data.TMU.User;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.TMU.Service.ITMUService
{
    public interface Imsc
    {



        #region msc
        Task updatetask(msc msc);
        #endregion
        #region Mscuser
        string GetUserSenderinMscuserToUserWhitRole(Role Role,int idmsc);
        void Addmscuser(msc model, string usersenderidcode, string userreciveridcode, bool validation,int view=1);
        void Updatemscuser(int Idmu, msc model, string usersenderidcode, string userreciveridcode);
        void Sendmscuserauthor(int idmsc, string usersenderidcode, string userreciveridcode);
        MscUser GetRowstatus(int idmsc, bool status = true);
        MscUser GetFirstrowmscuser(int idmsc);
        MscUser GetMscUser(int idmsc, string idcode, int view=0);
        Tuple<int,List<int>> GetMscUser(string idcode, int view = 1);
        Tuple<int, List<int>> GetMscUserr(string idcodereciver, bool status, bool validition);
        MscUser GetFirstRowsender(int idmsc, string userreciveridcode);
        ListMscUserViewModel GetAllMscUser(int pageid = 1, string filtertitel = null, string tag = null, int take = 0, string reciveridcode = null, bool status = true, string senderidcode = null,bool activestatus=true, bool Validition = false, bool activeValidition = false,string view=null, string year = null);
        ListMscViewModel GetAllMsc(int pageid = 1, string filtertitel = null, string tag = null, int take = 0, string author = null, string Andicator = "0");
        int GetAllMscUser(int IdAuthor);
        bool FindMscUser(int idmsc, string idreciver, bool status = true);
        void SetLastStatus(int idmsc);
        void SetLastStatusvalidation(int idmsc ,bool validation);
        bool GetStatusValidition(int idmsc, bool status, bool validition);
        MscUser GetMscUser(int idmsc, bool status, bool validition);
        MscUser GetMscUserbyid(int id);
        List<MscUser> ListMscUserbyid(int idmsc);
        int GetMscForUser(int IdAuthor, bool validandicator);
        int GetAllMscForUser(bool validandicator);
        int Maxletternember();
        int ContMSCWhitLeterNumber(string leternumber);


        #endregion

        #region MscPersent
        ListMscPersentViewModel GetAllMscPersent(int pageid = 1, string filtertitel = null, string tag = null, int take = 0, string idcodeuser = null, bool status = false, int idmsc=0,bool activestatus=true);
        MscPercent FindMscPersent(int idMsc);
        MscPercent FindMscPersent(int idMsc,string idcode);
        bool setLastPercentUser(int idMsc,string percent);
        bool setPercent(int idMsc, string percent);
        bool setPercentUser(int idMsc, string percent);
        bool IsFinalMscProject(int idMsc);
        bool IsEmptyMscPersent();
        bool IsequalPercent(int idMsc);
        List<SelectListItem> GetAllUserinDb();
        List<MscPercent> GetAllProjectWhitUserInMP(string Usercode, string year = null);
        #endregion

        #region AndicatorUser
        int MaxletternemberAU();
        AndicatorUser getbyleternumber(string leternumber);
        IEnumerable<AndicatorUser> GetAllAndicatorUserByIdCode(string idcode);
        #endregion

        #region pricelist
        ListpricelistViewModel GetAllPricelist(int pageid = 1, string filtertitel = null, string tag = null, int take = 0, string year = null,bool ActivitiPrice=false,string namePriceActivity=null);

        void DeleteAllpricelist();
        List<SelectListItem> CategouryActivity();
        List<SelectListItem> Activity();
        List<SelectListItem> unit();
        List<SelectListItem> Price();
        List<SelectListItem> factoroffer();
        List<SelectListItem> PriceActivity();
        List<PriceList> GetCategouryActivity();
        List<SelectListItem> GetActivityUnderPriceActivity(int id);
        PriceList getPricelistbyid(int id);

        double SumTotalPriceCategouryActivity(string CategouryActivity = null);
        PriceList FirstgetPricelistbyid(string Activity);

        #endregion

        Task AddInStutusfacsprint(List<statusfaceprint> statusfaceprint);
        List<statusfaceprint> GetStutusfacsprintByLeternumber(string leternumber);
    }
}
