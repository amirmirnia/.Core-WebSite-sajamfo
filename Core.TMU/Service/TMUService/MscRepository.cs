using Core.TMU.Service.ITMUService;
using Data.TMU.Context;
using Data.TMU.Model;
using Data.TMU.Model.msc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.TMU.Genarator;
using Data.TMU.User;
using System.Globalization;

namespace Core.TMU.Service.TMUService
{
    public class MscRepository : Imsc
    {

        private ContextTMU _db;
        private IGeneric<msc> _gmsc;
        private IGeneric<MscUser> _gmscuser;
        private IGeneric<PriceList> _gPriceList;
        private IGeneric<MscPercent> _gMscPercent;
        private IGeneric<statusfaceprint> _gstatusfaceprint;
        private IUser _User;
        public MscRepository(ContextTMU db, IGeneric<statusfaceprint> gstatusfaceprint, IGeneric<PriceList> gPriceList, IGeneric<MscPercent> gMscPercent, IGeneric<msc> gmsc, IGeneric<MscUser> gmscuser, IUser User)
        {
            _db = db;
            _gmsc = gmsc;
            _gmscuser = gmscuser;
            _User = User;
            _gMscPercent = gMscPercent;
            _gPriceList = gPriceList;
            _gstatusfaceprint = gstatusfaceprint;
        }


        public void Addmscuser(msc model, string usersenderidcode, string userreciveridcode, bool validation, int view = 1)
        {
            MscUser mscUser = new MscUser()
            {
                Year = DateTime.Now.ToPeString("yyyy"),
                Deta = model.DetaNews,
                view = view,
                status = true,
                validation = validation,
                idsender = _User.FindUser(usersenderidcode).IdCode,
                idreciver = userreciveridcode,
                Idmsc = model.Idmsc,
                Description = model.Description,
                factoroffer=model.factoroffer,
                Volume=model.Volume,
                TotalPrice= model.Price,

            };

            _db.MscUsers.Add(mscUser);
            _db.SaveChanges();
        }

        public bool FindMscUser(int idmsc, string idreciver, bool status = true)
        {
            if (_db.MscUsers.FirstOrDefault(p => p.status == status && p.idreciver == idreciver && p.Idmsc == idmsc) != null)
            {
                return true;
            }
            return false;
        }

        public ListMscViewModel GetAllMsc(int pageid = 1, string filtertitel = null, string tag = null, int take = 0, string author = null, string Andicator = "0")
        {
            IQueryable<msc> result = _db.Msc;
            if (!string.IsNullOrEmpty(filtertitel))
            {
                result = result.Where(p => p.Activity.Contains(filtertitel));
            }
            if (!string.IsNullOrEmpty(author))
            {
                result = result.Where(n => n.author.Contains(author));
            }
            if (Andicator == "1")
            {
                result = result.Where(n => n.Andicator != "0");
            }

            int skip = (pageid - 1) * take;

            var count = result.Include(n => n.MscUser).Select(n => new CategoryMsc
            {
                Idmsc = n.Idmsc,
                author = n.author,
                DetaNews = n.DetaNews,
                Activity = n.Activity,
                letter_number = n.letter_number,
                Volume = n.Volume,
                Location = n.Location,
                Price = n.Price,
                Idauthor = n.IdAuthor,
                deadline = n.deadline,
                Andicator = n.Andicator,
                parcel = n.parcel,
                Seri = n.Seri,
                Well = n.Well




            }).Count();
            var list = result.Include(n => n.MscUser).Select(n => new CategoryMsc
            {
                Idmsc = n.Idmsc,
                author = n.author,
                DetaNews = n.DetaNews,
                Activity = n.Activity,
                letter_number = n.letter_number,
                Volume = n.Volume,
                Location = n.Location,
                Price = n.Price,
                Idauthor = n.IdAuthor,
                deadline = n.deadline,
                Andicator = n.Andicator,
                parcel = n.parcel,
                Seri = n.Seri,
                Well = n.Well



            }).OrderByDescending(n => n.DetaNews).Skip(skip).Take(take).ToList();
            return new ListMscViewModel()
            {
                cartabl = list,
                CountPage = (count / take) + 1,
                IdPage = pageid
            };
        }

        public ListMscUserViewModel GetAllMscUser(int pageid = 1, string filtertitel = null, string tag = null, int take = 0, string reciveridcode = null, bool status = true, string senderidcode = null, bool activestatus = true, bool Validition = false, bool activeValidition = false, string view = null, string year = null)
        {
            IQueryable<MscUser> result = _db.MscUsers;
            //if ( !string.IsNullOrEmpty(filtertitel))
            //{
            //    result = result.Where(p => p..Contains(filtertitel));
            //}
            if (!string.IsNullOrEmpty(reciveridcode))
            {
                result = result.Where(n => n.idreciver.Contains(reciveridcode));
            }
            if (!string.IsNullOrEmpty(senderidcode))
            {
                result = result.Where(n => n.idsender.Contains(senderidcode));
            }
            if (!string.IsNullOrEmpty(view))
            {
                result = result.Where(n => n.view == int.Parse(view));
            }
            if (!string.IsNullOrEmpty(year))
            {
                result = result.Where(n => n.Year.Contains(year));
            }
            if (!string.IsNullOrEmpty(reciveridcode) && !string.IsNullOrEmpty(senderidcode))
            {
                result = result.Where(n => n.idsender.Contains(senderidcode) || n.idreciver.Contains(reciveridcode));
            }
            else
            {
                if (!string.IsNullOrEmpty(reciveridcode))
                {
                    result = result.Where(n => n.idreciver.Contains(reciveridcode));
                }
                if (!string.IsNullOrEmpty(senderidcode))
                {
                    result = result.Where(n => n.idsender.Contains(senderidcode));
                }
            }
            if (activestatus == true)
            {
                result = result.Where(n => n.status == status);
            }
            if (activeValidition == true)
            {
                result = result.Where(n => n.validation == Validition);
            }





            int skip = (pageid - 1) * take;

            var count = result.Include(n => n.Msc).Select(n => new CategoryUserMsc
            {
                author = n.idsender,
                reciver = n.idreciver,
                DetaNews = n.Deta,
                Activity = n.Msc.Activity,
                letter_number = n.Msc.letter_number,
                status = n.status,
                idmsc = n.Idmsc,
                sender = n.idsender,
                view = n.view,
                IdMU = n.IdMU,
                Andicator = n.Msc.Andicator,
                deadline = n.Msc.deadline,
                IdAU = n.IdAU,
                Isclose=n.Msc.Isclose




            }).Count();
            var list = result.Include(n => n.Msc).Select(n => new CategoryUserMsc
            {
                author = n.idsender,
                reciver = n.idreciver,
                DetaNews = n.Deta,
                Activity = n.Msc.Activity,
                letter_number = n.Msc.letter_number,
                status = n.status,
                idmsc = n.Idmsc,
                sender = n.idsender,
                view = n.view,
                IdMU = n.IdMU,
                Andicator = n.Msc.Andicator,
                deadline = n.Msc.deadline,
                IdAU = n.IdAU,
                Isclose = n.Msc.Isclose




            }).OrderByDescending(n => n.DetaNews).Skip(skip).Take(take).ToList();
            return new ListMscUserViewModel()
            {
                Projrct = list,
                CountPage = (count / take) + 1,
                IdPage = pageid
            };
        }

        public MscUser GetFirstRowsender(int idmsc, string userreciveridcode)
        {

            if (!string.IsNullOrEmpty(userreciveridcode))
            {
                return _db.MscUsers.First(p => p.Idmsc == idmsc && p.idreciver == userreciveridcode);

            }
            return null;
        }

        public MscUser GetRowstatus(int idmsc, bool status = true)
        {
            IQueryable<MscUser> result = _db.MscUsers;


            return result.OrderByDescending(p => p.Idmsc == idmsc && p.status == true).First();


        }

        public MscUser GetMscUser(int idmsc, string idcode, int view = 0)
        {
            if (_db.MscUsers.FirstOrDefault(p => p.Idmsc == idmsc && p.idreciver == idcode && p.view == view) != null)
            {
                return _db.MscUsers.FirstOrDefault(p => p.Idmsc == idmsc && p.idreciver == idcode && p.view == view);
            }
            return null;
        }

        public bool GetStatusValidition(int idmsc, bool status, bool validition)
        {
            var mscuser = _db.MscUsers.FirstOrDefault(p => p.validation == validition && p.status == status && p.Idmsc == idmsc);
            if (mscuser != null)
            {
                return true;
            }
            return false;
        }

        public void Sendmscuserauthor(int idmsc, string usersenderidcode, string userreciveridcode)
        {
            var model = _gmscuser.GetEntity(p => p.status == true && p.Idmsc == idmsc);
            model.validation = true;
            model.status = false;
            _gmscuser.Update(model);
            Addmscuser(_gmsc.GetById(idmsc), userreciveridcode, "تعیین نشده", false);

        }

        public void SetLastStatus(int idmsc)
        {
            var model = _gmscuser.GetEntity(p => p.Idmsc == idmsc && p.status == true);
            model.status = false;
            _gmscuser.Update(model);

        }

        public void Updatemscuser(int Idmu, msc model, string usersenderidcode, string userreciveridcode)
        {
            var mscUser = _gmscuser.GetById(Idmu);
            mscUser.idreciver = userreciveridcode;
            mscUser.Idmsc = _gmsc.GetEntity(p => p.letter_number == model.letter_number).Idmsc;
            mscUser.idsender = _User.FindUser(usersenderidcode).IdCode;
            mscUser.Deta = model.DetaNews;
            mscUser.view = 0;
            mscUser.status = true;
            mscUser.validation = false;

            _db.Update(mscUser);
            _db.SaveChanges();
        }

        public Tuple<int, List<int>> GetMscUser(string idcode, int view = 1)
        {
            var count = _db.MscUsers.Where(p => p.idreciver == idcode && p.view == view).ToList().Count();
            var list = _db.MscUsers.Where(p => p.idreciver == idcode && p.view == view).Select(p => p.Idmsc).ToList();
            return new Tuple<int, List<int>>(count, list);
        }

        public int GetAllMscUser(int IdAuthor)
        {
            return _db.Msc.Where(p => p.IdAuthor == IdAuthor).Count();
        }

        public int GetMscForUser(int IdAuthor, bool validandicator)
        {
            if (validandicator == true)
            {
                return _db.Msc.Where(p => p.IdAuthor == IdAuthor && p.Andicator != "0").Count();
            }
            return _db.Msc.Where(p => p.IdAuthor == IdAuthor && p.Andicator == "0").Count();

        }

        public int GetAllMscForUser(bool validandicator)
        {
            if (validandicator == true)
            {
                return _db.Msc.Where(p => p.Andicator != "0").Count();
            }
            return _db.Msc.Where(p => p.Andicator == "0").Count();
        }

        public MscUser GetMscUser(int idmsc, bool status, bool validition)
        {
            var mscuser = _db.MscUsers.FirstOrDefault(p => p.validation == validition && p.status == status && p.Idmsc == idmsc);
            if (mscuser != null)
            {
                return mscuser;
            }
            return null;
        }

        public MscPercent FindMscPersent(int idMsc)
        {
            var mscPercents = _db.mscPercents.FirstOrDefault(p => p.Idmsc == idMsc);
            if (mscPercents != null)
            {
                return mscPercents;
            }
            return null;
        }

        public ListMscPersentViewModel GetAllMscPersent(int pageid = 1, string filtertitel = null, string tag = null, int take = 0, string idcodeuser = null, bool status = false, int idmsc = 0, bool activestatus = true)
        {
            IQueryable<MscPercent> result = _db.mscPercents;
            if (!string.IsNullOrEmpty(idcodeuser))
            {
                result = result.Where(n => n.IdCodeUser.Contains(idcodeuser));
            }
            if (idmsc != 0)
            {
                result = result.Where(n => n.Idmsc == idmsc);
            }

            if (activestatus == true)
            {
                result = result.Where(n => n.status == status);
            }

            int skip = (pageid - 1) * take;

            var count = result.Include(n => n.Msc).Select(n => new CategoryMscPercent
            {
                Percent = n.Percent,
                status = n.status,
                IdCodeUser = n.IdCodeUser,
                PercentUser = n.PercentUser,
                Activity = n.Msc.Activity,
                letter_number = n.Msc.letter_number,
                Id = n.Id,
                DetaNews = n.DetaNews,
                Volume = n.Msc.Volume,
                Idmsc = n.Idmsc




            }).Count();
            var list = result.Include(n => n.Msc).Select(n => new CategoryMscPercent
            {
                Percent = n.Percent,
                status = n.status,
                IdCodeUser = n.IdCodeUser,
                PercentUser = n.PercentUser,
                Activity = n.Msc.Activity,
                letter_number = n.Msc.letter_number,
                Id = n.Id,
                DetaNews = n.DetaNews,
                Volume = n.Msc.Volume,
                Idmsc = n.Idmsc



            }).OrderByDescending(n => n.DetaNews).Skip(skip).Take(take).ToList();
            return new ListMscPersentViewModel()
            {
                category = list,
                CountPage = (count / take) + 1,
                IdPage = pageid
            };
        }

        public MscUser GetMscUserbyid(int id)
        {
            var mscuser = _db.MscUsers.FirstOrDefault(p => p.IdMU == id);
            if (mscuser != null)
            {
                return mscuser;
            }
            return null;
        }

        public List<MscUser> ListMscUserbyid(int idmsc)
        {
            return _db.MscUsers.Where(p => p.Idmsc == idmsc).OrderBy(p => p.IdMU).ToList();
        }

        public int Maxletternember()
        {
            try
            {
                return _db.Msc.Max(p => p.number);
            }
            catch (Exception)
            {

                return 0;
            }
        }

        public List<SelectListItem> GetAllUserinDb()
        {
            return _db.mscPercents.Select(p => new SelectListItem()
            {
                //Text=_User.FindUser(p.IdCodeUser).FullName,
                Text = p.IdCodeUser,
                Value = p.IdCodeUser
            }).Distinct().ToList();
        }

        public List<MscPercent> GetAllProjectWhitUserInMP(string Usercode, string year = null)
        {
            if (!string.IsNullOrEmpty(year))
            {
                return _db.mscPercents.Where(p => p.IdCodeUser == Usercode && p.Year == year).ToList();
            }
            return _db.mscPercents.Where(p => p.IdCodeUser == Usercode).ToList();

        }

        public int MaxletternemberAU()
        {
            try
            {
                return _db.AndicatorUsers.Max(p => p.Number);
            }
            catch (Exception)
            {

                return 0;
            }
        }

        public IEnumerable<AndicatorUser> GetAllAndicatorUserByIdCode(string idcode)
        {
            return _db.AndicatorUsers.Where(p => p.IdCode == idcode).ToList();
        }

        public Tuple<int, List<int>> GetMscUserr(string idcodereciver, bool status, bool validition)
        {
            var count = _db.MscUsers.Where(p => p.idreciver == idcodereciver && p.status == true && p.validation == validition).ToList().Count();
            var list = _db.MscUsers.Where(p => p.idreciver == idcodereciver && p.status == true && p.validation == validition).Select(p => p.Idmsc).ToList();
            return new Tuple<int, List<int>>(count, list);
        }

        public bool IsEmptyMscPersent()
        {
            var r = _db.mscPercents.Count();
            if (r != 0)
            {
                return false;
            }
            return true;
        }

        public MscPercent FindMscPersent(int idMsc, string idcode)
        {
            var mscPercents = _db.mscPercents.FirstOrDefault(p => p.Idmsc == idMsc && p.IdCodeUser == idcode);
            if (mscPercents != null)
            {
                return mscPercents;
            }
            return null;
        }

        public AndicatorUser getbyleternumber(string leternumber)
        {
            return _db.AndicatorUsers.First(p => p.LetterNumber == leternumber);
        }

        public int ContMSCWhitLeterNumber(string leternumber)
        {
            return _db.Msc.Where(p => p.letter_number == leternumber).Count();
        }

        public bool IsequalPercent(int idMsc)
        {
            var mscpersent = FindMscPersent(idMsc);
            if (mscpersent != null)
            {

                var listpersent = SpelitTag.tag(mscpersent.Percent);
                var listpersentUser = SpelitTag.tag(mscpersent.PercentUser);

                if (listpersent.Count() + 1 == listpersentUser.Count() + 1)
                {
                    return true;
                }

            }
            return false;
        }

        public string GetUserSenderinMscuserToUserWhitRole(Role Role, int idmsc)
        {

            if (Role != null)
            {
                var listUser = _User.GetUserWhitLevel(Role.Level);
                if (listUser != null)
                {
                    foreach (var item in listUser)
                    {
                        var mscuser = _db.MscUsers.FirstOrDefault(p => p.Idmsc == idmsc && p.idsender == item.IdCode);
                        if (mscuser != null)
                        {
                            return mscuser.idsender;
                        }
                    }
                }
            }
            return null;
        }

        public bool setLastPercentUser(int idMsc, string percent)
        {
            try
            {
                var mscpersentt = FindMscPersent(idMsc);
                var listpersent = SpelitTag.tag(mscpersentt.PercentUser);
                if (listpersent.Count() == 1)
                {
                    mscpersentt.PercentUser += "-" + percent;
                }
                else if (IsequalPercent(idMsc) == true)
                {
                    mscpersentt.PercentUser += "-" + percent;
                }
                else
                {
                    listpersent[listpersent.Length - 1] = percent;
                    mscpersentt.PercentUser = "0";
                    int i = 0;
                    foreach (var item in listpersent)
                    {
                        if (i!=0)
                        {
                            mscpersentt.PercentUser += "-" + item;
                        }
                        
                        i++;

                    }
                }
                mscpersentt.DetaNews = DateTime.Now;
                mscpersentt.Year = DateTime.Now.ToPeString("yyyy");
                _gMscPercent.Update(mscpersentt);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool setPercent(int idMsc, string percent)
        {
            try
            {
                var mscpersentt = FindMscPersent(idMsc);
                mscpersentt.Percent += "-" + percent;
                mscpersentt.DetaNews = DateTime.Now;
                mscpersentt.Year = DateTime.Now.ToPeString("yyyy");
                _gMscPercent.Update(mscpersentt);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool IsFinalMscProject(int idMsc)
        {
            var msc = _gmsc.GetById(idMsc);
            var mscpersent = FindMscPersent(idMsc);
            if (mscpersent != null)
            {

                var listpersent = SpelitTag.tag(mscpersent.Percent);
                var listpersentUser = SpelitTag.tag(mscpersent.PercentUser);
                double totalpersent = 0.0;
                if (listpersent.Count() + 1 == listpersentUser.Count() + 1)
                {

                    for (int i = 0; i < listpersent.Count(); i++)
                    {
                          totalpersent += double.Parse(listpersent[i]);
                       

                    }
                    var t = totalpersent.ToString();
                    if (msc.Volume ==t )
                    {
                        mscpersent.status = true;
                        mscpersent.statusstatusface = true;
                        _gMscPercent.Update(mscpersent);
                        return true;
                    }
                    else
                    {
                        mscpersent.statusstatusface = true;
                        _gMscPercent.Update(mscpersent);
                        mscpersent.status = false;
                        return false;
                    }

                }

            }
            return false;
        }

        public bool setPercentUser(int idMsc, string percent)
        {
            try
            {
                var mscpersentt = FindMscPersent(idMsc);
                mscpersentt.PercentUser += "-" + percent;
                mscpersentt.DetaNews = DateTime.Now;
                mscpersentt.Year = DateTime.Now.ToPeString("yyyy");
                _gMscPercent.Update(mscpersentt);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task updatetask(msc msc)
        {
            _db.Attach(msc);
            _db.Entry(msc).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeleteAllpricelist()
        {
            var rowsToDelete = _db.PriceList.ToList();

            // Delete the rows
            _db.PriceList.RemoveRange(rowsToDelete);
            _db.SaveChanges();
        }

        public List<SelectListItem> Activity()
        {
            return _db.PriceList.Where(p=>p.Price!="-").Select(p => new SelectListItem()
            {
                Value = (p.IdPL).ToString(),
                Text = p.Activity
            }).ToList();
        }

        public List<SelectListItem> unit()
        {
            return _db.PriceList.Select(p => new SelectListItem()
            {
                Value = (p.IdPL).ToString(),
                Text = p.unit
            }).ToList();
        }

        public List<SelectListItem> Price()
        {
            return _db.PriceList.Select(p => new SelectListItem()
            {
                Value = (p.IdPL).ToString(),
                Text = p.Price
            }).ToList();
        }

        public List<SelectListItem> factoroffer()
        {
            return _db.PriceList.Select(p => new SelectListItem()
            {
                Value = (p.IdPL).ToString(),
                Text = p.factoroffer
            }).ToList();
        }

        public PriceList getPricelistbyid(int id)
        {
           return _db.PriceList.SingleOrDefault(p => p.IdPL == id);
        }

        public PriceList FirstgetPricelistbyid(string Activity)
        {
            return _db.PriceList.FirstOrDefault(p => p.Activity.Contains(Activity));
        }

        public ListpricelistViewModel GetAllPricelist(int pageid = 1, string filtertitel = null, string tag = null, int take = 0, string year = null, bool ActivitiPrice = false,string namePriceActivity=null)
        {
            IQueryable<PriceList> result = _db.PriceList;
            //if ( !string.IsNullOrEmpty(filtertitel))
            //{
            //    result = result.Where(p => p..Contains(filtertitel));
            //}
            
            if (!string.IsNullOrEmpty(year))
            {
                result = result.Where(n => n.Year.Contains(year));
            }
            if (!string.IsNullOrEmpty(namePriceActivity))
            {

                result = result.Where(n => n.CaregouryActivity.Contains(namePriceActivity));
            }
            if (ActivitiPrice==false)
            {
                result = result.Where(n => n.Price!="-");
            }
            else
            {
                result = result.Where(n => n.Price == "-");
            }

            int skip = (pageid - 1) * take;

            var count = result.Select(n => new PriceList
            {
                Activity=n.Activity,
                Year=n.Year,
                DetaNews=n.DetaNews,
                factoroffer=n.factoroffer,
                IdCode=n.IdCode,
                month=n.month,
                Price=n.Price,
                TotalPrice=n.TotalPrice,
                unit=n.unit,
                value=n.value,
                IdPL=n.IdPL,
                CaregouryActivity=n.CaregouryActivity,
                valueUse=n.valueUse

            }).Count();
            var list = result.Select(n => new PriceList
            {
                Activity = n.Activity,
                Year = n.Year,
                DetaNews = n.DetaNews,
                factoroffer = n.factoroffer,
                IdCode = n.IdCode,
                month = n.month,
                Price = n.Price,
                TotalPrice = n.TotalPrice,
                unit = n.unit,
                value = n.value,
                IdPL = n.IdPL,
                CaregouryActivity = n.CaregouryActivity,
                valueUse = n.valueUse


            }).OrderByDescending(n => n.DetaNews).Skip(skip).Take(take).ToList();
            return new ListpricelistViewModel()
            {
                priceLists = list,
                CountPage = (count / take) + 1,
                IdPage = pageid
            };
        }

        public List<SelectListItem> CategouryActivity()
        {
            return _db.PriceList.Select(p => new SelectListItem()
            {
                Value = p.CaregouryActivity,
                Text = p.CaregouryActivity
            }).ToList();
        }

        public List<SelectListItem> PriceActivity()
        {
            return _db.PriceList.Where(p=>p.Price=="-").Select(p => new SelectListItem()
            {
                Value = (p.IdPL).ToString(),
                Text = p.Activity
            }).ToList();
        }

        public List<SelectListItem> GetActivityUnderPriceActivity(int id)
        {
           var t= _db.PriceList.Where(p => p.CaregouryActivity == (id).ToString()).Select(p => new SelectListItem()
            {
                Value = (p.IdPL).ToString(),
                Text = p.Activity
            }).ToList();
            t.Insert(0,new SelectListItem()
            {
                Text = "انتخاب کنید",
                Value = ""
            });
            return t;
        }

        public List<PriceList> GetCategouryActivity()
        {
            return _db.PriceList.Where(p => p.CaregouryActivity == "-").ToList();
        }

        public double SumTotalPriceCategouryActivity(string CategouryActivity = null)
        {
            double total = 0;
            var getGA = _db.PriceList.Where(p => p.CaregouryActivity != "-").ToList();
            if (CategouryActivity== null)
            {
                
                foreach (var item in getGA)
                {


                    total = total + double.Parse(item.TotalPrice.Replace("٬", ""));
                }

            }
            else
            {
                foreach (var item in getGA)
                {
                    if (item.CaregouryActivity==CategouryActivity.ToString())
                    {
                        total = total + double.Parse(item.TotalPrice, new CultureInfo("fa-IR"));
                    }
                }
            }
            return total;
        }

        public async Task AddInStutusfacsprint(List<statusfaceprint> statusfaceprint)
        {
            foreach (var item in statusfaceprint)
            {
                _gstatusfaceprint.Insert(item);
            }
        }

        public List<statusfaceprint> GetStutusfacsprintByLeternumber(string leternumber)
        {


            return _db.statusfaceprint.Where(p => p.Leternumber==leternumber).ToList();
        }

        public MscUser GetFirstrowmscuser(int idmsc)
        {
            
                return _db.MscUsers.First(p => p.Idmsc == idmsc);
        }

        public void SetLastStatusvalidation(int idmsc, bool validation)
        {
            var model = _gmscuser.GetEntity(p => p.Idmsc == idmsc && p.status == true && p.validation== validation);
            model.status = false;
            model.view = 0;
            _gmscuser.Update(model);
        }
    }
}
