using Core.TMU.Convertor;
using Core.TMU.FileSite;
using Core.TMU.Genarator;
using Core.TMU.Img;
using Core.TMU.Security;
using Core.TMU.Sender;
using Core.TMU.Service.ITMUService;
using Core.TMU.Service.TMUService;
using Data.TMU.Context;

using Data.TMU.Model;
using Data.TMU.Model.Gallery;
using Data.TMU.Model.Image;
using Data.TMU.Model.Menu;
using Data.TMU.Model.msc;
using Data.TMU.Model.Nav;
using Data.TMU.Model.News;
using Data.TMU.Model.Page;
using Data.TMU.Model.Slider;
using System;
using Data.TMU.Permissions;
using Data.TMU.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;


namespace TMU.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class PanelController : Controller
    {
        private IUser _User;
        private IPermision _permision;
        private IViewRenderService _renderService;
        private INews _news;
        private IGallery _gallery;
        private IPage _Page;
        private Imsc _msc;
        private IHttpContextAccessor _accessor;
        private IGeneric<News> _gNews;
        private IGeneric<msc> _gmsc;
        private IGeneric<MscUser> _mscuser;
        private IGeneric<MscPercent> _gMscPercent;
        private IGeneric<AndicatorUser> _gAndicatorUser;
        private IGeneric<MscUser> _gMscUser;
        private IGeneric<PriceList> _gPriceList;
        private IGeneric<Gallery> _gGallery;
        private IGeneric<FileNews> _gFileNews;
        private IGeneric<FilePage> _gFilePage;
        private IGeneric<FileGallery> _gFileGallery;
        private IGeneric<Menu> _gMenu;
        private IGeneric<Page> _gPage;
        private IGeneric<ImagePage> _gImagePage;
        private IGeneric<Slider> _gSlider;
        private IGeneric<Navbar> _gNavbar;
        private IGeneric<Role> _grole;
        private Imenu _Menu;
        private ISlider _Slider;
        private INavbar _Navbar;
        public PanelController(IUser User, IPermision permision, Imsc msc, IGeneric<PriceList> gPriceList, IGeneric<MscUser> mscuser, IGeneric<Role> grole, IGeneric<msc> gmsc, IGeneric<MscUser> gMscUser, IGeneric<MscPercent> gMscPercent, IGeneric<AndicatorUser> gAndicatorUser, IViewRenderService renderService, INews news, IGeneric<FilePage> gFilePage, IGeneric<News> gNews, IGeneric<FileNews> gFileNews, IGeneric<Menu> gMenu, Imenu Menu, IGeneric<Slider> gSlider, ISlider Slider, IGeneric<Page> gPage, IPage Page, INavbar Navbar, IGeneric<Navbar> gNavbar, IGeneric<ImagePage> gImagePage, IGeneric<Gallery> gGallery, IGeneric<FileGallery> gFileGallery, IGallery gallery)
        {
            _news = news;
            _User = User;
            _msc = msc;
            _mscuser = mscuser;
            _gPriceList = gPriceList;
            _grole = grole;
            _gMscPercent = gMscPercent;
            _gAndicatorUser = gAndicatorUser;
            _gMscUser = gMscUser;
            _gmsc = gmsc;
            _gSlider = gSlider;
            _permision = permision;
            _renderService = renderService;
            _gFileNews = gFileNews;
            _gFilePage = gFilePage;
            _gImagePage = gImagePage;
            _gNews = gNews;
            _gMenu = gMenu;
            _Menu = Menu;
            _Slider = Slider;
            _gPage = gPage;
            _Page = Page;
            _Navbar = Navbar;
            _gNavbar = gNavbar;
            _gGallery = gGallery;
            _gFileGallery = gFileGallery;
            _gallery = gallery;


        }
        public IActionResult Index()
        {



            //var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            //ViewBag.Ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            ViewBag.AllProject = _msc.GetAllMsc(1, null, null, 8, null, "1");
            ViewBag.AllUserProject = _msc.GetAllMsc(1, null, null, 4, _User.FindUser(User.Identity.Name).FullName);
            ViewBag.AllUser = _User.GetAllUser(1, null, null, null, null, 6);
            ViewBag.SendMsc = _msc.GetAllMscUser(1, null, null, 7, null, false, _User.FindUser(User.Identity.Name).IdCode, false);
            //صورت وضعیت
            ViewBag.FormPrint = _msc.GetAllMscUser(1, null, null, 7, _User.FindUser(User.Identity.Name).IdCode, true, null, true, true);

            return View();
        }

        #region rejisteruser whit admin
        [permissionChecker(2)]
        [Route("panel/registerUser")]
        public IActionResult registerUser()
        {
            return View();
        }
        [permissionChecker(2)]
        [Route("panel/registerUser")]
        [HttpPost]
        public IActionResult registerUser(registerUser user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            if (!_User.ComparePassword(user.Password, user.RePassword))
            {
                ModelState.AddModelError("Password", "کلمه عبور با تکرار کلمه عبور همخوانی ندارد");
                return View(user);
            }
            if (_User.IsExistEmail(user.Email))
            {
                ModelState.AddModelError("Email", "همچین ایمیلی قبلا ثبت نام شده است");
                return View(user);
            }
            if (_User.IsExistEmail(user.Mobile))
            {
                ModelState.AddModelError("Mobile", "همچین شماره همراهی قبلا ثبت نام شده است");
                return View(user);
            }
            User AddUser = new User()
            {
                Email = user.Email,
                FullName = user.FullName,
                Mobile = user.Mobile,
                Password = (user.Password).EncodePasswordMd5(),
                RegisterDate = DateTime.Now,
                IsActive = false,
                IdCode = user.IdCode,
                ActiveCode = Code.UniqCode(),
                UserAvatar = "DefultAvatar.jpg",
                post = "0"

            };
            _User.AddUser(AddUser);
            //Unit.Save();
            string Body = _renderService.RenderToStringAsync("_ActiveEmail", user);
            SendEmail.Send(user.Email, "فعال سازی پنل دانشکده منابع طبیعی و علوم دریایی تربیت مدرس ", Body);
            return Redirect("ListUser");

        }


        [permissionChecker(2)]
        [Route("panel/ListUser")]
        public IActionResult ListUser(List<string> checkdelete, string IdActive = "none", int pageid = 1, string filterfullname = null, string email = null, string filteridcode = null, string tag = null)
        {
            if (IdActive != "none")
            {
                _User.ActiveUserIdCode(IdActive);
            }
            if (checkdelete.Count() != 0)
            {
                _User.DeleteListUser(checkdelete);
            }
            ViewBag.filterfullname = filterfullname;
            ViewBag.tag = filteridcode;
            //ViewBag.tag = tag;
            ViewBag.AllUser = _User.GetAllUser(1, null, null, null, null, 20);
            return View(_User.GetAllUser(pageid, filterfullname, email, filteridcode, tag, 200));
        }

        [permissionChecker(2)]
        [Route("panel/DeleteListUser/{codeid}")]
        public IActionResult DeleteListUser(List<string> checkdelete, string codeid)
        {
            if (checkdelete.Count != 0)
            {
                _User.DeleteListUser(checkdelete);
            }
            else
            {
                _User.DeleteUser(codeid);
            }
            return Redirect("/Panel/listuser");
        }

        [permissionChecker(2)]
        [Route("panel/Resetpassworduser")]
        public IActionResult Resetpassworduser()
        {
            return View();
        }

        [permissionChecker(2)]
        [Route("panel/Resetpassworduser")]
        [HttpPost]
        public IActionResult Resetpassworduser(ChangePasswordUserViewModel model, string idcode)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _User.FindUser(idcode);
            if (!_User.ComparePassword(model.NewPassword, model.ConfarinPassword))
            {
                ModelState.AddModelError("Password", "کلمه عبور با تکرار کلمه عبور همخوانی ندارد");
                return View(user);
            }
            user.Password = model.NewPassword.EncodePasswordMd5();
            _User.UpdateUser(user);
            return Redirect("/Listuser");
        }
        #endregion

        #region profile

        [Route("panel/ProfileHome")]
        public IActionResult ProfileHome()
        {
            return View(_User.FindUser(User.Identity.Name));
        }
        [Route("panel/profilesettings")]
        public IActionResult profilesettings(string idcode)
        {
            ViewBag.Role = new SelectList(_permision.GetRole(), "Value", "Text");
            var user = _User.FindUserForUpdate(User.Identity.Name);
            if (idcode != null)
            {
                user = _User.FindUserForUpdate(idcode);
                ViewBag.isEnabelpost = true;

            }
            else
            {
                ViewBag.isEnabelpost = false;
            }
            return View(user);
        }

        [HttpPost]
        [Route("panel/profilesettings")]
        public IActionResult profilesettings(UpdateregisterViewModel updateregister, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Role = new SelectList(_permision.GetRole(), "Value", "Text");
                return View(updateregister);
            }
            var user = _User.FindUser(updateregister.IdCode);
            user.Email = updateregister.Email;
            user.FullName = updateregister.FullName;
            user.Mobile = updateregister.Mobile;
            user.post = updateregister.post;
            if (updateregister.post != "0")
            {
                user.level = _permision.GetRoleById(int.Parse(updateregister.post)).Level;//پست همان رول ای دی است
                List<int> listrole = new List<int>()
                {
                    int.Parse(updateregister.post)
                };
                AddUserRoles(user.IdCode, listrole);
            }
            else
            {
                user.level = "0";
            }
            user.CodePerseneli = updateregister.CodePerseneli;
            if (user.CodePerseneli != updateregister.CodePerseneli || updateregister.CodePerseneli == null)
            {
                if (_User.FindUserwhitcodeperseneli(updateregister.CodePerseneli) || updateregister.CodePerseneli == null)
                {
                    ModelState.AddModelError("CodePerseneli", "در وارد کردن کد پرسنلی دقت نمایید");
                    ViewBag.Role = new SelectList(_permision.GetRole(), "Value", "Text");
                    return View(updateregister);
                }
            }

            //user.level = updateregister.level;
            if (file != null)
            {
                if (file.Isimage())
                {
                    user.UserAvatar = SaveImage.SaveImageProfile(file, user.UserAvatar, "wwwroot/Img/Userprofile").Item2;
                }
                else
                {
                    ModelState.AddModelError("UserAvatar", "فایل را درست انتخاب کنید");
                }
            }
            _User.UpdateUser(user);
            //Logout();
            return Redirect("/panel/listuser");

        }
        [Route("panel/ResetPassword")]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [Route("panel/ResetPassword")]
        public IActionResult ResetPassword(ChangePasswordViewModel changePassword)
        {

            if (!ModelState.IsValid)
            {
                return View(changePassword);
            }
            var user = _User.FindUser(User.Identity.Name);

            if (changePassword.OldPassword.EncodePasswordMd5() != user.Password)
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور قدیمی را درست وارد کنید");
                return View(changePassword);
            }

            if (!_User.ComparePassword(changePassword.NewPassword, changePassword.ConfarinPassword))
            {
                ModelState.AddModelError("Password", "کلمه عبور با تکرار کلمه عبور همخوانی ندارد");
                return View(user);
            }
            user.Password = changePassword.NewPassword.EncodePasswordMd5();
            _User.UpdateUser(user);
            Logout();
            ViewBag.IsResetpassword = true;
            return Redirect("/Login?IsResetpassword=true");

        }
        #endregion

        #region permition role
        [permissionChecker(1)]
        [Route("panel/GetInfopermitionRole")]
        public IActionResult GetInfopermitionRole(int radiostackedRoleid)
        {
            ViewData["Role"] = _permision.GetAllRole();
            ViewData["Permision"] = _permision.GetAllPermission();
            //ViewData["Permisionselect"] = _permision.GetpermissionWhitRoleId(radiostackedRoleid);

            return View();
        }

        [permissionChecker(1)]
        [Route("panel/permision")]
        public IActionResult permision()
        {
            Getinfopagepermistion();
            return View();
        }

        [permissionChecker(1)]
        [Route("panel/permision")]
        [HttpPost]
        public IActionResult permision(List<int> SelectListItem, string NameRole, Role role, int radiostackedRoleid, bool DeleteRole, bool final, bool PPrint, bool EndP, bool Signuchure)
        {

            if (radiostackedRoleid != 0)
            {

                Getinfopagepermistion();
                ViewData["Permisionselect"] = _permision.GetpermissionWhitRoleId(radiostackedRoleid);
                ViewBag.roleid = radiostackedRoleid;
                ViewBag.RoleSelect = _permision.GetRoleById(radiostackedRoleid);
                ViewBag.Isfinal = _permision.GetRoleById(radiostackedRoleid).Finalapproval;//تایید نهایی
                ViewBag.EndProcess = _permision.GetRoleById(radiostackedRoleid).EndProcess;//چرخه اخر
                ViewBag.Signuchure = _permision.GetRoleById(radiostackedRoleid).Signuchure;//امضا
                ViewBag.Checkout = _permision.GetRoleById(radiostackedRoleid).Checkout;//Gis
                ViewBag.IsPermissionPrint = _permision.GetRoleById(radiostackedRoleid).PermissionPrint;// سطح 1 -کارشناس

                return View(_permision.GetRoleById(radiostackedRoleid));
            }
            //کارشناس gis است
            if (SelectListItem.Contains(7))
            {
                role.Checkout = true;
            }
            else
            {
                role.Checkout = false;
            }

            if (int.Parse(role.Level) > _permision.GetMaxLevel() + 1)
            {
                ModelState.AddModelError("RoleTitle", "سطح را زیاد انتخاب کرده اید");
                Getinfopagepermistion();
                return View();
            }
            if (_permision.GetRoleWhitLevel(role.Level) == true && role.RoleId == null)
            {
                ModelState.AddModelError("RoleTitle", "چنین سطحی قبلا تعریف شده است عدد سطح را تغییر دهید");
                Getinfopagepermistion();
                return View();
            }
            else
            {
                if (EndP == true)
                {
                    role.EndProcess = true;
                }
                if (Signuchure == true)
                {
                    role.Signuchure = true;
                }
                if (final == true)
                {
                    if (_permision.IsTrueFinalaproval())
                    {

                        _permision.GetFinalRole().Finalapproval = final;

                    }
                    if (_permision.GetPermitionPrintRole() != null)
                    {
                        if (int.Parse(_permision.GetPermitionPrintRole().Level) < int.Parse(role.Level))
                        {
                            role.Finalapproval = final;
                        }
                        else
                        {
                            ModelState.AddModelError("RoleTitle", "سطح تایید کننده دستور کار باید بالاتر از تایید کننده نهایی باشد");
                            Getinfopagepermistion();
                            return View();

                        }
                    }
                    else
                    {
                        ModelState.AddModelError("RoleTitle", "تایید کننده نهایی در سیتم ابتدا باید ثبت شود");
                        Getinfopagepermistion();
                        return View();

                    }


                }
                if (PPrint == true)
                {
                    if (_permision.IsTruePermitionPrint())
                    {
                        _permision.GetPermitionPrintRole().PermissionPrint = PPrint;

                    }
                    if (_permision.GetFinalRole() != null)
                    {
                        if (int.Parse(_permision.GetFinalRole().Level) > int.Parse(role.Level))
                        {
                            role.PermissionPrint = PPrint;
                        }
                        else
                        {
                            ModelState.AddModelError("RoleTitle", "سطح تایید کننده دستور کار باید کمتر از تایید کننده نهایی باشد");
                            Getinfopagepermistion();
                            return View();
                        }
                    }
                    else
                    {
                        role.PermissionPrint = PPrint;
                    }



                }

                if (role == null)
                {
                    int RoleId = _permision.CreatRole(new Role
                    {
                        RoleTitle = NameRole,
                        IsDelete = false,

                    });


                    _permision.AddPermission(SelectListItem, RoleId);
                }
                else
                {
                    if (DeleteRole != true)
                    {
                        if (!string.IsNullOrEmpty(role.RoleTitle))
                        {
                            _permision.UpdateRoleTask(role);
                            //_grole.Update(role)
                            _permision.UpdatePermissionRole(SelectListItem, role.RoleId);
                        }
                        else
                        {
                            ModelState.AddModelError("RoleTitle", "نام نقش را درست وارد کنید.");

                        }
                    }
                    else
                    {
                        if (_User.IsExistPost(role.RoleId) != true)
                        {
                            var u = _permision.GetRoleById(role.RoleId);
                            u.IsDelete = true;
                            _permision.UpdateRoleTask(u);
                            //_permision.DeleteRole(role);
                        }

                    }


                }
                Getinfopagepermistion();
                return Redirect("/panel/permision");
            }
        }

        //[permissionChecker(1)]
        //[Route("panel/AccesslevelUser")]
        //public IActionResult AccesslevelUser(string urlidcode)
        //{
        //    ViewBag.idcode = urlidcode;
        //    ViewData["roleuser"] = _permision.GetRoleUserWhitRoleId(_User.getPkId(urlidcode));
        //    return View(_permision.GetAllRole());
        //}

        //[permissionChecker(1)]
        //[Route("panel/AccesslevelUser")]
        //[HttpPost]
        //public IActionResult AccesslevelUser(string idcode, List<int> SelectListItem)
        //{

        //    _permision.AddRole(SelectListItem, _User.getPkId(idcode));
        //    var level = "";
        //    foreach (var item in SelectListItem)
        //    {
        //        level += _permision.GetlevelRole(item) + "-";
        //    }
        //    _User.InsertLevelUser(level, idcode);

        //    return Redirect("ListUser");
        //}


        #endregion



        #region news
        //[permissionChecker(2)]
        //[Route("panel/ListNews")]
        //public IActionResult ListNews(List<string> checkdelete, int pageid = 1, string filtertitel = null, string tag = null)
        //{

        //    if (checkdelete.Count() != 0)
        //    {
        //        //_news.DeleteL(checkdelete);
        //    }
        //    ViewBag.filtertitel = filtertitel;
        //    ViewBag.tag = tag;
        //    return View(_news.GetAllNews(pageid, filtertitel, tag, 7));
        //}

        //[permissionChecker(2)]
        //[Route("panel/DeleteNews/{id}")]
        //public IActionResult DeleteNews(int id)
        //{
        //    if (_news.GetImageNews(id).Count != 0)
        //    {
        //        foreach (var item in _news.GetImageNews(id))
        //        {
        //            DeleteImage.Deleteimage(_news.FindImageNews(item.Id).PathFile, "wwwroot/Img/News/Org");
        //            DeleteImage.Deleteimage(_news.FindImageNews(item.Id).PathFile, "wwwroot/Img/News/thum");

        //            _gFileNews.Delete(item.Id);

        //        }
        //    }


        //    _gNews.Delete(id);
        //    return Redirect("/panel/ListNews");
        //}

        //[permissionChecker(2)]
        //[Route("panel/DetailImageNews/{id}")]
        //public IActionResult DetailImageNews(int id)
        //{
        //    ViewBag.ID = id;

        //    if (_news.NewsIsImage(id))
        //    {
        //        ViewBag.IsImageNews = true;
        //        return View(_news.GetImageNews(id));
        //    }
        //    return Redirect("/panel/ListNews");
        //}

        //[permissionChecker(2)]
        //[Route("panel/DetailImageNews/{id}")]
        //[HttpPost]
        //public IActionResult DetailImageNews(List<FileNews> fileNews, string? SelectFirst, List<int>? CheckDelete, int id)
        //{
        //    if (!string.IsNullOrEmpty(SelectFirst))
        //    {
        //        _news.SetFirstImage(int.Parse(SelectFirst), id);
        //        return Redirect("/panel/DetailImageNews/{id}");
        //    }
        //    if (CheckDelete.Count() != 0)
        //    {
        //        foreach (var item in CheckDelete)
        //        {
        //            DeleteImage.Deleteimage(_news.FindImageNews(item).PathFile, "wwwroot/Img/News/Org");
        //            DeleteImage.Deleteimage(_news.FindImageNews(item).PathFile, "wwwroot/Img/News/thum");
        //            _gFileNews.DeleteWhitList(CheckDelete);
        //        }
        //        if (!ModelState.IsValid == true)
        //        {
        //            return View(fileNews);
        //        }
        //        // to do change filenews
        //    }
        //    return Redirect("/panel/ListNews");
        //}

        //[permissionChecker(2)]
        //[Route("panel/CreatImageNews/{id}")]
        //public IActionResult CreatImageNews(int id)
        //{
        //    return View();
        //}

        //[permissionChecker(2)]
        //[Route("panel/CreatImageNews/{id}")]
        //[HttpPost]
        //public IActionResult CreatImageNews(FileNewsViewModel file, int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(file);
        //    }
        //    foreach (var item in file.files)
        //    {
        //        var image = SaveImage.SaveImageNews(item, "wwwroot/Img/News/Org");
        //        convertorimage.Image_resize("wwwroot/Img/News/Org/" + image.Item2, "wwwroot/Img/News/thum/" + image.Item2, 500);
        //        FileNews fileNews = new FileNews()
        //        {
        //            IdN = id,
        //            PathFile = image.Item2
        //        };
        //        _gFileNews.Insert(fileNews);
        //    }
        //    return Redirect("/panel/DetailImageNews/{id}");
        //}

        //[permissionChecker(2)]
        //[Route("panel/CreatNews")]
        //public IActionResult CreatNews()
        //{
        //    RefreshMenu();
        //    return View();
        //}

        //[permissionChecker(2)]
        //[Route("panel/CreatNews")]
        //[HttpPost]
        //public IActionResult CreatNews(News model, bool chk1)
        //{
        //    RefreshMenu();
        //    model.CountView = 0;
        //    model.IsSearch = false;
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    _gNews.Insert(model);
        //    return Redirect("/panel/ListNews");
        //}

        //[permissionChecker(2)]
        //[Route("panel/UpdateNews/{id?}")]
        //public IActionResult UpdateNews(int id)
        //{
        //    RefreshMenu();

        //    var News = _gNews.GetById(id);

        //    return View(News);
        //}

        //[permissionChecker(2)]
        //[Route("panel/UpdateNews/{id?}")]
        //[HttpPost]
        //public IActionResult UpdateNews(News model, bool chk1)
        //{
        //    RefreshMenu();
        //    //if (model.Forest.Class!=0)
        //    //{
        //    //    GetSelect2(model.Forest.Class);
        //    //}   

        //    //if (!ModelState.IsValid)
        //    //{
        //    //    return View(model);
        //    //}

        //    if (!chk1)
        //    {
        //        _gNews.Update(model);
        //        model.IsSearch = false;

        //    }
        //    else
        //    {

        //            ModelState.AddModelError("Forest.NameForest", "اطلاعات جستجو پیشرفته را کامل کنید");
        //            return View(model);
        //    }


        //    return Redirect("/panel/ListNews");
        //}
        #endregion

        #region Galery


        [permissionChecker(5)]
        [Route("panel/ListGallery")]
        public IActionResult ListGallery(List<string> checkdelete, int pageid = 1, string filtertitel = null, string tag = null)
        {

            if (checkdelete.Count() != 0)
            {
                //_news.DeleteL(checkdelete);
            }
            ViewBag.filtertitel = filtertitel;
            ViewBag.tag = tag;
            return View(_gallery.GetAllGallery(pageid, filtertitel, tag, 7));
        }

        [permissionChecker(5)]
        [Route("panel/DeleteGalery/{id}")]
        public IActionResult DeleteGalery(int id)
        {
            if (_gallery.GetImageGallery(id).Count != 0)
            {
                foreach (var item in _gallery.GetImageGallery(id))
                {
                    DeleteImage.Deleteimage(_gallery.FindImageGallery(item.Id).PathFile, "wwwroot/Img/Gallery/Org");
                    DeleteImage.Deleteimage(_gallery.FindImageGallery(item.Id).PathFile, "wwwroot/Img/Gallery/thum");

                    _gFileGallery.Delete(item.Id);

                }
            }

            _gGallery.Delete(id);
            return Redirect("/panel/Listgallery");
        }

        [permissionChecker(5)]
        [Route("panel/DetailImageGallery/{id}")]
        public IActionResult DetailImageGallery(int id)
        {
            ViewBag.ID = id;

            if (_gallery.GalleryIsImage(id))
            {
                ViewBag.IsImagegallery = true;
                return View(_gallery.GetImageGallery(id));
            }
            return Redirect("/panel/ListGallery");
        }

        [permissionChecker(5)]
        [Route("panel/DetailImageGallery/{id}")]
        [HttpPost]
        public IActionResult DetailImageGallery(List<FileGallery> fileGalleries, string? SelectFirst, List<int>? CheckDelete, int id)
        {
            if (!string.IsNullOrEmpty(SelectFirst))
            {
                _gallery.SetFirstImage(int.Parse(SelectFirst), id);
                return Redirect("/panel/DetailImageGallery/{id}");
            }
            if (CheckDelete.Count() != 0)
            {
                foreach (var item in CheckDelete)
                {
                    DeleteImage.Deleteimage(_gallery.FindImageGallery(item).PathFile, "wwwroot/Img/Gallery/Org");
                    DeleteImage.Deleteimage(_gallery.FindImageGallery(item).PathFile, "wwwroot/Img/Gallery/thum");
                    _gFileGallery.DeleteWhitList(CheckDelete);
                }
                if (!ModelState.IsValid == true)
                {
                    return View(fileGalleries);
                }
                // to do change filenews
            }
            return Redirect("/panel/ListGallery");
        }

        [permissionChecker(5)]
        [Route("panel/CreatImageGallery/{id}")]
        public IActionResult CreatImageGallery(int id)
        {
            return View();
        }

        [permissionChecker(5)]
        [Route("panel/CreatImageGallery/{id}")]
        [HttpPost]
        public IActionResult CreatImageGallery(FileGalleryViewModel file, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(file);
            }
            foreach (var item in file.files)
            {
                var image = SaveImage.SaveImageNews(item, "wwwroot/Img/Gallery/Org");
                convertorimage.Image_resize("wwwroot/Img/Gallery/Org/" + image.Item2, "wwwroot/Img/Gallery/thum/" + image.Item2, 500);
                FileGallery FileGallery = new FileGallery()
                {
                    IdG = id,
                    PathFile = image.Item2
                };
                _gFileGallery.Insert(FileGallery);
            }
            return Redirect("/panel/DetailImageGallery/{id}");
        }

        [permissionChecker(5)]
        [Route("panel/CreatGallery")]
        public IActionResult CreatGalery()
        {

            return View();
        }

        [permissionChecker(5)]
        [Route("panel/CreatGallery")]
        [HttpPost]
        public IActionResult CreatGalery(Gallery model, bool chk1)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.View = 0;
            _gGallery.Insert(model);

            return Redirect("/panel/ListGallery");
        }

        [permissionChecker(5)]
        [Route("panel/UpdateGallery/{id?}")]
        public IActionResult UpdateGalery(int id)
        {
            return View(_gGallery.GetById(id));
        }

        [permissionChecker(5)]
        [Route("panel/UpdateGalery/{id?}")]
        [HttpPost]
        public IActionResult UpdateGalery(Gallery model, bool chk1)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _gGallery.Update(model);
            return Redirect("/panel/ListGallery");
        }


        #endregion

        #region page
        //[permissionChecker(1003)]
        //public IActionResult ListPage(List<string> checkdelete, int pageid = 1, string filtertitel = null, string tag = null)
        //{

        //    if (checkdelete.Count() != 0)
        //    {
        //        //_news.DeleteL(checkdelete);
        //    }
        //    ViewBag.filtertitel = filtertitel;
        //    ViewBag.tag = tag;
        //    return View(_Page.GetAllPage(pageid, filtertitel, tag, 7));
        //}

        //[permissionChecker(1003)]
        //[Route("panel/DeletePage/{id}")]
        //public IActionResult DeletePage(int id)
        //{
        //    if (_Page.GetImagePage(id).Count != 0)
        //    {
        //        foreach (var item in _Page.GetImagePage(id))
        //        {
        //            DeleteImage.Deleteimage(_Page.FindImagePage(item.Id).PathFile, "wwwroot/Img/Page/Org");
        //            DeleteImage.Deleteimage(_Page.FindImagePage(item.Id).PathFile, "wwwroot/Img/Page/thum");

        //            _gFilePage.Delete(item.Id);

        //        }
        //    }


        //    _gPage.Delete(id);
        //    return Redirect("/panel/ListPage");
        //}

        //[permissionChecker(1003)]
        //[Route("Panel/CreatPage")]
        //public IActionResult CreatPage()
        //{

        //    return View();
        //}

        //[permissionChecker(1003)]
        //[Route("Panel/CreatPage")]
        //[HttpPost]
        //public IActionResult CreatPage(Page page)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(page);
        //    }
        //    _gPage.Insert(page);
        //    return Redirect("/Panel/ListPage");
        //}

        //[permissionChecker(1003)]
        //[Route("panel/UpdatePage/{id?}")]
        //public IActionResult UpdatePage(int id)
        //{
        //    return View(_gPage.GetById(id));
        //}

        //[permissionChecker(1003)]
        //[Route("panel/UpdatePage/{id?}")]
        //[HttpPost]
        //public IActionResult UpdatePage(Page model)
        //{


        //    //if (!ModelState.IsValid)
        //    //{
        //    //    return View(model);
        //    //}
        //    _gPage.Update(model);
        //    return Redirect("/Panel/ListPage");
        //}



        //[permissionChecker(1003)]
        //[Route("panel/DetailImagePage/{id}")]
        //public IActionResult DetailImagePage(int id)
        //{
        //    ViewBag.ID = id;

        //    if (_Page.PageIsImage(id))
        //    {
        //        ViewBag.IsImagePage = true;
        //        return View(_Page.GetImagePage(id));
        //    }
        //    return Redirect("/panel/ListPage");
        //}

        //[permissionChecker(1003)]
        //[Route("panel/DetailImagePage/{id}")]
        //[HttpPost]
        //public IActionResult DetailImagePage(List<FilePage> filePage, string? SelectFirst, List<int>? CheckDelete, int id)
        //{
        //    if (!string.IsNullOrEmpty(SelectFirst))
        //    {
        //        _Page.SetFirstImage(int.Parse(SelectFirst), id);
        //        return Redirect("/panel/DetailImagePage/{id}");
        //    }
        //    if (CheckDelete.Count() != 0)
        //    {
        //        foreach (var item in CheckDelete)
        //        {
        //            DeleteImage.Deleteimage(_Page.FindImagePage(item).PathFile, "wwwroot/Img/Page/Org");
        //            DeleteImage.Deleteimage(_Page.FindImagePage(item).PathFile, "wwwroot/Img/Page/thum");
        //            _gFilePage.DeleteWhitList(CheckDelete);
        //        }
        //        if (!ModelState.IsValid == true)
        //        {
        //            return View(filePage);
        //        }
        //        // to do change filenews
        //    }
        //    return Redirect("/panel/ListPage");
        //}

        //[permissionChecker(1003)]
        //[Route("panel/CreatImagePage/{id}")]
        //public IActionResult CreatImagePage(int id)
        //{
        //    return View();
        //}

        //[permissionChecker(1003)]
        //[Route("panel/CreatImagePage/{id}")]
        //[HttpPost]
        //public IActionResult CreatImagePage(FilePageViewModel file, int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(file);
        //    }
        //    foreach (var item in file.files)
        //    {
        //        var image = SaveImage.SaveImageNews(item, "wwwroot/Img/Page/Org");
        //        convertorimage.Image_resize("wwwroot/Img/Page/Org/" + image.Item2, "wwwroot/Img/Page/thum/" + image.Item2, 500);
        //        FilePage filePage = new FilePage()
        //        {
        //            IdP = id,
        //            PathFile = image.Item2
        //        };
        //        _gFilePage.Insert(filePage);
        //    }
        //    return Redirect("/panel/DetailImagePage/{id}");
        //}


        //[permissionChecker(1003)]
        //[Route("panel/InsertImagePage")]
        //public IActionResult InsertImagePage()
        //{
        //    return View();
        //}
        //[permissionChecker(1003)]
        //[Route("panel/InsertImagePage")]
        //[HttpPost]
        //public IActionResult InsertImagePage(ImagePageViewModel file)
        //{
        //    //if (!ModelState.IsValid)
        //    //{
        //    //    return View(file);
        //    //}
        //    foreach (var item in file.files)
        //    {
        //        var image = SaveImage.SaveImageNews(item, "wwwroot/Img/ImageSite/Org");
        //        convertorimage.Image_resize("wwwroot/Img/ImageSite/Org/" + image.Item2, "wwwroot/Img/ImageSite/thum/" + image.Item2, 500);
        //        file.ImagePage.PathFile = image.Item2;
        //        _gImagePage.Insert(file.ImagePage);
        //    }
        //    return Redirect("/panel/ShowImagePage");
        //}


        //[permissionChecker(1003)]
        //[Route("panel/ShowImagePage")]
        //public IActionResult ShowImagePage(int pageid = 1, string filtertitel = null, string tag = null)
        //{
        //    return View(_Page.GetAllImagePage(pageid, filtertitel, tag, 12));
        //}
        //[permissionChecker(1003)]
        //[Route("panel/DeleteImagePage")]
        //[HttpPost]
        //public IActionResult DeleteImagePage(List<FilePage> filePage, List<int>? CheckDelete)
        //{
        //    //if (!string.IsNullOrEmpty(SelectFirst))
        //    //{
        //    //    _Page.SetFirstImage(int.Parse(SelectFirst), id);
        //    //    return Redirect("/panel/DetailImagePage/{id}");
        //    //}
        //    if (CheckDelete.Count() != 0)
        //    {
        //        foreach (var item in CheckDelete)
        //        {
        //            DeleteImage.Deleteimage(_Page.FindImageSite(item).PathFile, "wwwroot/Img/ImageSite/Org");
        //            DeleteImage.Deleteimage(_Page.FindImageSite(item).PathFile, "wwwroot/Img/ImageSite/thum");
        //            _gImagePage.DeleteWhitList(CheckDelete);
        //        }
        //        //if (!ModelState.IsValid == true)
        //        //{
        //        //    return View(filePage);
        //        //}
        //        // to do change filenews
        //    }
        //    return Redirect("/panel/ShowImagePage");
        //}

        #endregion

        #region navbar

        //[permissionChecker(1004)]
        //[Route("panel/ListNavbar/{filtertitel?}")]
        //public IActionResult ListNavbar(List<string> checkdelete, int pageid = 1, string filtertitel = null)
        //{

        //    if (checkdelete.Count() != 0)
        //    {
        //        //_news.DeleteL(checkdelete);
        //    }
        //    ViewBag.filtertitel = filtertitel;
        //    return View(_Navbar.GetAllNavbar(pageid, filtertitel, 20,"خبری"));
        //}

        //[permissionChecker(1004)]
        //[Route("/panel/CreatNavbar")]
        //public IActionResult CreatNav()
        //{
        //    RefreshNavbar();
        //    return View();
        //}

        //[permissionChecker(1004)]
        //[Route("/panel/CreatNavbar")]
        //[HttpPost]
        //public IActionResult CreatNav(Navbar nav, bool chk1, bool chk2,string ParentId2)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(nav);
        //    }
        //    if (chk1 && chk2)
        //    {
        //        nav.ParentId = int.Parse(ParentId2);
        //    }

        //    _gNavbar.Insert(nav);
        //    return Redirect("/Panel/ListNavbar");
        //}
        //[permissionChecker(1004)]
        //[Route("/panel/UpdateNavbar/{id?}")]
        //public IActionResult UpdateNavbar(int id)
        //{
        //    RefreshNavbar();
        //    if (id != 0)
        //    {
        //        return View(_gNavbar.GetById(id));
        //    }
        //    else
        //    {
        //        return Redirect("/Panel/ListNavbar");
        //    }
        //}


        //[permissionChecker(1004)]
        //[Route("/panel/UpdateNavbar/{id?}")]
        //[HttpPost]
        //public IActionResult UpdateNavbar(Navbar nav, bool chk1)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(nav);
        //    }
        //    if (chk1 == false && !_Navbar.IsParent(nav.Id))
        //    {
        //        nav.ParentId = 0;
        //    }
        //    _gNavbar.Update(nav);
        //    return Redirect("/Panel/ListNavbar");
        //}

        //[permissionChecker(1004)]
        //[Route("/panel/DeleteNavbar/{id?}")]
        //public IActionResult DeleteNavbar(int id)
        //{
        //    if (id != 0 && !_Navbar.IsParent(id))
        //    {
        //        _gNavbar.Delete(id);

        //    }
        //    return Redirect("/Panel/ListNavbar");
        //}

        //#endregion

        //#region menu
        //[permissionChecker(2)]
        //[Route("panel/Menu")]
        //public IActionResult AddMenu()
        //{
        //    RefreshMenu();
        //    return View();
        //}
        //[permissionChecker(2)]
        //[Route("panel/Menu")]
        //[HttpPost]
        //public IActionResult AddMenu(Menu menu, string? ParentId2, bool chk1, bool chk2)
        //{
        //    RefreshMenu();
        //    if (!ModelState.IsValid)
        //    {
        //        return View(menu);
        //    }
        //    if (chk1 == false)
        //    {
        //        menu.ParentId = 0;
        //    }
        //    if (!string.IsNullOrEmpty(ParentId2) && chk2 == true && chk1 == true)
        //    {
        //        menu.ParentId = int.Parse(ParentId2);
        //    }
        //    _gMenu.Insert(menu);

        //    return Redirect("/panel/ListMenu");
        //}

        //[permissionChecker(2)]
        //[Route("panel/UpdateMenu/{id}")]
        //public IActionResult UpdateMenu(int id)
        //{
        //    RefreshMenu();
        //    ViewBag.id = id;
        //    var f = _gMenu.GetById(id);
        //    ViewBag.infoMenu = f;
        //    return View(f);
        //}

        //[permissionChecker(2)]
        //[Route("panel/UpdateMenu/{id}")]
        //[HttpPost]
        //public IActionResult UpdateMenu(Menu menu, string? ParentId2, bool chk1, bool chk2)
        //{
        //    RefreshMenu();
        //    if (!ModelState.IsValid)
        //    {
        //        return View(menu);
        //    }
        //    //if (chk1 == false)
        //    //{
        //    //    menu.ParentId = 0;
        //    //}
        //    //if (!string.IsNullOrEmpty(ParentId2) && chk2 == true && chk1 == true)
        //    //{
        //    //    menu.ParentId = int.Parse(ParentId2);
        //    //}
        //    _gMenu.Update(menu);

        //    return Redirect("/panel/ListMenu");
        //}

        //[permissionChecker(2)]
        //[Route("panel/ListMenu/{filtertitel?}")]
        //public IActionResult ListMenu(List<string> checkdelete, int pageid = 1, string filtertitel = null)
        //{

        //    if (checkdelete.Count() != 0)
        //    {
        //        //_news.DeleteL(checkdelete);
        //    }
        //    ViewBag.filtertitel = filtertitel;
        //    return View(_Menu.GetAllMenu(pageid, filtertitel, 7));
        //}

        //[permissionChecker(2)]
        //[Route("panel/DeleteMenu/{id}")]
        //public IActionResult DeleteMenu(int id)
        //{
        //    if (_gMenu.GetById(id) != null)
        //    {
        //        _gMenu.Delete(id);
        //    }
        //    return View("/Panel/ListMenu");
        //}
        #endregion

        #region slider

        //[permissionChecker(2)]
        //[Route("panel/ListSlider")]
        //public IActionResult ListSlider(List<string> checkdelete, int pageid = 1, string filtertitel = null)
        //{

        //    if (checkdelete.Count() != 0)
        //    {
        //        //_news.DeleteL(checkdelete);
        //    }
        //    ViewBag.filtertitel = filtertitel;

        //    return View(_Slider.GetAllSlider(pageid, filtertitel, 7));
        //}

        //[permissionChecker(2)]
        //[Route("panel/CreatSlider")]
        //public IActionResult CreatSlider()
        //{

        //    return View();
        //}

        //[permissionChecker(2)]
        //[Route("panel/CreatSlider")]
        //[HttpPost]
        //public IActionResult CreatSlider(SliderViewModel model)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    model.slider.DetaSlider = DateTime.Now;
        //    var image = SaveImage.SaveImageNews(model.fileimage, "wwwroot/Img/Slider/Org");
        //    convertorimage.Image_resize("wwwroot/Img/Slider/Org/" + image.Item2, "wwwroot/Img/Slider/thum/" + image.Item2, 500);
        //    model.slider.FileName = image.Item2;
        //    _gSlider.Insert(model.slider);


        //    return Redirect("/panel/ListSlider");
        //}

        //[permissionChecker(2)]
        //[Route("panel/UpdateSlider/{id?}")]
        //public IActionResult UpdateSlider(int id)
        //{
        //    SliderViewModel model = new SliderViewModel()
        //    {
        //        fileimage = null,
        //        slider = _gSlider.GetEntity(p => p.Id == id)
        //    };
        //    return View(model);
        //}

        //[permissionChecker(2)]
        //[Route("panel/UpdateSlider/{id?}")]
        //[HttpPost]
        //public IActionResult UpdateSlider(SliderViewModel model)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    model.slider.DetaSlider = DateTime.Now;
        //    if (model.fileimage != null)
        //    {
        //        DeleteImage.Deleteimage(model.slider.FileName, "wwwroot/Img/Slider/Org");
        //        DeleteImage.Deleteimage(model.slider.FileName, "wwwroot/Img/Slider/thum");
        //        var image = SaveImage.SaveImageNews(model.fileimage, "wwwroot/Img/Slider/Org");
        //        convertorimage.Image_resize("wwwroot/Img/Slider/Org/" + image.Item2, "wwwroot/Img/Slider/thum/" + image.Item2, 500);
        //        model.slider.FileName = image.Item2;

        //    }
        //    _gSlider.Update(model.slider);
        //    return Redirect("/panel/ListSlider");
        //}

        //[permissionChecker(2)]
        //[Route("panel/DeleteSlider/{id}")]
        //public IActionResult DeleteSlider(int id)
        //{
        //    var slider = _gSlider.GetById(id);
        //    DeleteImage.Deleteimage(slider.FileName, "wwwroot/Img/Slider/Org");
        //    DeleteImage.Deleteimage(slider.FileName, "wwwroot/Img/Slider/thum");
        //    _gSlider.Delete(id);
        //    return Redirect("/panel/ListSlider");
        //}
        #endregion



        #region projrct

        [permissionChecker(4)]
        [Route("panel/Statusface/{codeid?}")]
        public IActionResult Statusface(string Contractor = null, string year = null, string codeid = null)
        {
            GetAllUserDBMP();
            //codeid = User.Identity.Name;

            ViewBag.user = Contractor;
            if (Contractor != null)
            {
                return View(_msc.GetAllProjectWhitUserInMP(Contractor, year));
            }
            //if (codeid != null)
            //{
            //    ViewBag.user = codeid;
            //    return View(_msc.GetAllProjectWhitUserInMP(codeid, year));

            //}
            ViewBag.user = _msc.GetAllUserinDb().First().Value;
            ViewBag.year = year;
            return View(_msc.GetAllProjectWhitUserInMP(_msc.GetAllUserinDb().First().Value, year));
        }
        [permissionChecker(4)]
        [Route("panel/StatusfacePrint/{Letternumber?}")]
        public IActionResult StatusfacePrint(string Letternumber = null)
        {

            return View(_msc.GetStutusfacsprintByLeternumber(Letternumber));
        }

        [permissionChecker(4)]
        [Route("panel/Statusface")]
        [HttpPost]
        public IActionResult Statusface(int[] itemCk, IFormFile? filefactor, string namefile)
        {

            if (itemCk.Count() == 0 && filefactor != null)
            {
                UploadFileFactor(filefactor, namefile);
            }
            var leternumber = "";
            var IDcode = "0";
            //GetAllUserDBMP();
            if (itemCk.Count() != 0)
            {
                leternumber = "67100-" + _User.FindUser(_msc.GetMscUser(itemCk[0], true, true).idsender).CodePerseneli + "-" + (_msc.MaxletternemberAU() + 1).ToString();
                var idmsc = "";
                foreach (var item in itemCk)
                {
                    var mscpersent = _msc.FindMscPersent(item);
                    idmsc += item + "-";
                    mscpersent.statusstatusface = false;
                    _gMscPercent.Update(mscpersent);

                }

                var mscU = _gmsc.GetById(itemCk[0]);
                //ثبت گروهی مجموعه ای از نامه ها به نام شخص در دیتابیس
                IDcode = _User.FindUser(_msc.GetMscUser(mscU.Idmsc, true, true).idreciver).IdCode;
                AndicatorUser AU = new AndicatorUser()
                {
                    LetterNumber = leternumber,
                    Deta = DateTime.Now,
                    IdCode = IDcode,
                    Andicator = _User.FindUser(IDcode).CodePerseneli + "-" + (_msc.MaxletternemberAU() + 1),
                    Number = _msc.MaxletternemberAU() + 1,
                    IDUser = _User.FindUser(IDcode).Id,
                    IdMSC = idmsc

                };
                _gAndicatorUser.Insert(AU);

                //var listUser = _User.GetUserWhitLevel(_permision.GetminmaxLevel().Item2);//ارسال به سمت کارشناس gis
                //if (listUser!=null)
                //{

                //}
                var role = _permision.GetFinalRole();
                var codeidfinalapprov = _msc.GetUserSenderinMscuserToUserWhitRole(role, itemCk[0]);
                Print p = new Print();
                p.idmsc = itemCk[0];
                p.item = itemCk;
                p.filename = leternumber;
                p.SingnacharName = _User.FindUser(IDcode).FullName;
                p.SingnacharFinalapprovName = _User.Getname(codeidfinalapprov);


                //var listcheckoutuser = _permision.Getcheckouttrue();//گرفتن نقشی که gis است
                //if (listcheckoutuser != null)
                //{
                //    var userid = _permision.GetUserWhitRoleId(listcheckoutuser.RoleId);
                //    foreach (var item in userid)
                //    {
                //        MscUser mscUser = new MscUser()
                //        {
                //            idsender = "System",
                //            idreciver = _User.FindUser(item).IdCode,
                //            status = true,
                //            validation = false,
                //            Description = "از طرف سیستم ارسال شد",
                //            Deta = DateTime.Now,
                //            Year = DateTime.Now.ToPeString("yyyy"),
                //            view = 1,
                //            Idmsc = itemCk[0],//یک مقدار الکی داده شد
                //            statusface = leternumber,
                //            IdAU = _msc.getbyleternumber(leternumber).IdAU

                //        };
                //        _gMscUser.Insert(mscUser);
                //    }
                //}
                return View("Print", p);

                //SpireWord.ConvertToPdf(@"wwwroot\Status-face\Group", @"wwwroot\Status-face\Group\Pdf\", leternumber.ToString());
                //return Redirect("/panel/Statusface/" + IDcode);
                //return Redirect("/panel/Statusface/");

            }
            return Redirect("/panel/Statusface");
        }

        [permissionChecker(4)]
        [Route("panel/Print")]
        public IActionResult Print()
        {

            return View();
        }

        [permissionChecker(4)]
        [Route("panel/Print")]
        [HttpPost]
        public async Task<IActionResult> Print(IFormFile file, string namefile)
        {

            var status = Save.SaveFile(file, @"wwwroot\file\Status-face\", namefile + ".pdf");

            if (status.Item1)
            {
                return Redirect("/panel/Statusface/");
            }

            return Redirect("/Login");
        }
        //[permissionChecker(4)] 
        [Route("panel/Project")]
        public IActionResult Project(int pageid = 1, int pageidAll = 1, string filtertitel = null, string tag = null)
        {

            ViewBag.filtertitel = filtertitel;
            ViewBag.tag = tag;

            ViewBag.AllProject = _msc.GetAllMsc(pageidAll, null, null, 7, null);
            return View(_msc.GetAllMscPersent(pageid, filtertitel, tag, 7, _User.FindUser(User.Identity.Name).IdCode, false, 0, false));
        }

        [Route("panel/ListUnfinished")]
        public IActionResult ListUnfinished(int pageid = 1, string filtertitel = null, string tag = null)
        {

            ViewBag.filtertitel = filtertitel;
            ViewBag.tag = tag;
            if (_msc.GetAllMscPersent(pageid, filtertitel, tag, 100, _User.FindUser(User.Identity.Name).IdCode, false, 0, false).category.Count == 0)
            {
                //ViewBag.NotiError = "پروژه ناتمامی برای شما در سیستم یافت نشده است";
                return Redirect("/panel");
            }
            return View(_msc.GetAllMscPersent(pageid, filtertitel, tag, 100, _User.FindUser(User.Identity.Name).IdCode, false, 0, false));
        }

        //متمم
        //[permissionChecker(8)]
        //[Route("panel/CreatProject/{letternember}/{idmsc}")]
        //public IActionResult CreatProject(string letternember, string idmsc)
        //{
        //    var model = _gmsc.GetById(int.Parse(idmsc));
        //    model.Activity = (_msc.FirstgetPricelistbyid(model.Activity).IdPL).ToString();
        //    PriceActivity();
        //    GetUserWhitLevel();
        //    ViewBag.disabel = "متمم";
        //    ViewBag.A = model.Activity;
        //    return View(model);
        //}


        //[permissionChecker(8)]
        //[Route("panel/CreatProject/{letternember?}/{idmsc?}")]
        //[HttpPost]
        //public IActionResult CreatProject(msc model, string? userreciver, List<int> well, List<int> parcel, [FromRoute] string idmsc)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        PriceActivity();
        //        GetUserWhitLevel();
        //        return View(model);
        //    }
        //    if (model.Activity == "" || model.CaregouryActivity == "")
        //    {
        //        return View(model);
        //    }

        //    switch (model.Location)
        //    {
        //        case "فولاد مبارکه":
        //            model.Seri = null;
        //            model.parcel = null;
        //            model.Well = null;
        //            break;

        //        case "سری":
        //            model.parcel = null;
        //            model.Well = null;
        //            break;

        //        case "پارسل":

        //            model.Well = null;
        //            model.parcel = "";
        //            foreach (var item in parcel)
        //            {
        //                model.parcel += item + "-";
        //            }
        //            break;

        //        case "چاه":
        //            model.Seri = null;
        //            model.parcel = null;
        //            model.Well = "";
        //            foreach (var item in well)
        //            {
        //                model.Well += item + "-";
        //            }
        //            break;

        //        default:
        //            break;
        //    }

        //    model.DetaNews = DateTime.Now;
        //    _gmsc.Insert(model);
        //    //قبلا چنین پروژه ای در سیستم ثبت بود و این متمم است
        //    if (_gmsc.GetById(int.Parse(idmsc)) != null)
        //    {
        //        MscPercent p = new MscPercent()
        //        {
        //            DetaNews = DateTime.Now,
        //            Idmsc = model.Idmsc,
        //            IdCodeUser = _msc.FindMscPersent(int.Parse(idmsc)).IdCodeUser,
        //            PercentUser = "0",
        //            Percent = "0",
        //            status = false,
        //            Year = DateTime.Now.ToPeString("yyyy")
        //        };
        //        _gMscPercent.Insert(p);
        //    }

        //    #region insertPersentEndProcess
        //    if (_permision.GetRoleById(int.Parse(_User.FindUser(User.Identity.Name).post)).EndProcess == true)
        //    {
        //        var role = _permision.Getsignuchure();
        //        userreciver = _msc.GetUserSenderinMscuserToUserWhitRole(role, int.Parse(idmsc));
        //        _msc.setPercentUser(model.Idmsc, model.Volume);

        //    }


        //    #endregion

        //    #region send to who is permition PPrint
        //    //اگر درست بود و خودش دسترسی امضا دارد برو به کسی که دسترسی پرینت یا ناظر است
        //    if (_permision.GetRoleById(int.Parse(_User.FindUser(User.Identity.Name).post)).Signuchure == true)
        //    {
        //        var role = _permision.GetPermitionPrintRole();
        //        userreciver = _msc.GetUserSenderinMscuserToUserWhitRole(role, int.Parse(idmsc));
        //        _msc.setPercentUser(model.Idmsc, model.Volume);
        //    }
        //    #endregion
        //    #region send to who is permition FimalPrint
        //    //اگر درست بود و خودش دسترسی امضا دارد برو به کسی که رئیس است
        //    if (_permision.GetRoleById(int.Parse(_User.FindUser(User.Identity.Name).post)).PermissionPrint == true)
        //    {
        //        var role = _permision.GetFinalRole();
        //        userreciver = _msc.GetUserSenderinMscuserToUserWhitRole(role, int.Parse(idmsc));
        //        _msc.setPercentUser(model.Idmsc, model.Volume);

        //    }
        //    #endregion

        //    #region confraim  permition FimalPrint
        //    //اگر درست بود و خودش دسترسی امضا دارد برو به کسی که رئیس است
        //    if (_permision.GetRoleById(int.Parse(_User.FindUser(User.Identity.Name).post)).Finalapproval == true)
        //    {
        //        var role = _permision.GetFinalRole();
        //        userreciver = _msc.GetUserSenderinMscuserToUserWhitRole(role, int.Parse(idmsc));
        //        _msc.setPercentUser(model.Idmsc, model.Volume);
        //        _msc.setPercent(model.Idmsc, model.Volume);
        //        var f = _msc.IsFinalMscProject(model.Idmsc);//ایا پروژه تمام شد؟
        //        if (f == true)
        //        {
        //            //پروژه تمام شده
        //        }

        //    }
        //    #endregion



        //    _msc.Addmscuser(model, _User.FindUser(User.Identity.Name).IdCode, userreciver, true);


        //    return Redirect("/Panel/Project");
        //}




        [permissionChecker(3)]
        [Route("panel/CreatProject")]
        public IActionResult CreatProject()
        {
            //Activity();
            PriceActivity();
            GetUserWhitLevel();
            return View();
        }

        [permissionChecker(3)]
        [Route("panel/CreatProject")]
        [HttpPost]
        public IActionResult CreatProject(msc model, string userreciver, List<int> well, List<int> parcel, string? activityname)
        {
            if (model.CaregouryActivity == "00")
            {
                model.Activity = activityname;
            }
            var Result = Core.TMU.Convertor.ConvertTime.Compare( DateTime.Parse(model.deadline), DateTime.Parse(DateTime.Now.ToPeString("yyyy/MM/dd")));
            if (Result==-1)
            {
                ModelState.AddModelError("deadline", "چنیین نام فعالیتی قبلا ثبت شده است");
                return View(model);

            }
            if (!ModelState.IsValid)
            {
                //Activity();
                PriceActivity();
                GetUserWhitLevel();
                return View(model);
            }
            if (model.Activity == "" || model.CaregouryActivity == "")
            {
                return View(model);
            }
            if (model.CaregouryActivity != "00")
            {
                model.Activity = _msc.getPricelistbyid(int.Parse(model.Activity)).Activity;
            }

            switch (model.Location)
            {
                case "فولاد مبارکه":
                    model.Seri = null;
                    model.parcel = null;
                    model.Well = null;
                    break;

                case "سری":
                    model.parcel = null;
                    model.Well = null;
                    break;

                case "پارسل":

                    model.Well = null;
                    model.parcel = "";
                    foreach (var item in parcel)
                    {
                        model.parcel += item + "-";
                    }
                    break;

                case "چاه":
                    model.Seri = null;
                    model.parcel = null;
                    model.Well = "";
                    foreach (var item in well)
                    {
                        model.Well += item + "-";
                    }
                    break;

                default:
                    break;
            }

            model.DetaNews = DateTime.Now;
            _gmsc.Insert(model);
            _msc.Addmscuser(model, _User.FindUser(User.Identity.Name).IdCode, userreciver, false);
            return Redirect("/Panel/Project");
        }



        //[permissionChecker(1009)]
        [Route("panel/pricelist")]
        public IActionResult pricelist(int pageid = 1, string CaregouryActivityinput = null, string year = null, string filtertitel = null, string tag = null, int take = 20)
        {
            PriceActivity();
            ViewBag.filtertitel = filtertitel;
            ViewBag.tag = tag;
            ViewBag.year = year;
            ViewBag.IdPL = CaregouryActivityinput;
            ViewBag.take = take;
            //ViewBag.total = _msc.SumTotalPriceCategouryActivity(CaregouryActivityinput);
            return View(_msc.GetAllPricelist(pageid, filtertitel, tag, 10, year, false, CaregouryActivityinput));
        }
        [permissionChecker(1009)]
        [Route("panel/priceActivitylist")]
        public IActionResult priceActivitylist(int pageid = 1, string year = null, string filtertitel = null, string tag = null, int take = 20)
        {
            ViewBag.filtertitel = filtertitel;
            ViewBag.tag = tag;
            ViewBag.year = year;
            ViewBag.take = take;


            return View(_msc.GetAllPricelist(pageid, filtertitel, tag, 10, year, true));
        }

        [permissionChecker(1009)]
        [Route("panel/CreatPriceActivity")]
        public IActionResult CreatPriceActivity()
        {

            return View();
        }
        [permissionChecker(1009)]
        [Route("panel/CreatPriceActivity")]
        [HttpPost]
        public IActionResult CreatPriceActivity(PriceList model)
        {
            model.DetaNews = DateTime.Now;

            if (!ModelState.IsValid)
            {

                return View(model);
            }
            if (_gPriceList.GetEntity(p => p.Activity == model.Activity) != null)
            {
                ModelState.AddModelError("Activity", "چنیین نام فعالیتی قبلا ثبت شده است");
                return View(model);
            }
            _gPriceList.Insert(model);
            return Redirect("/panel/priceActivitylist");
        }
        [permissionChecker(1009)]
        [Route("panel/UpdatePriceActivity/{id}")]
        public IActionResult UpdatePriceActivity(int id)
        {
            var pricelist = _gPriceList.GetById(id);
            if (pricelist != null)
            {
                return View(pricelist);
            }
            return Redirect("/panel/priceActivitylist");
        }

        [permissionChecker(1009)]
        [Route("panel/UpdatePriceActivity/{id}")]
        [HttpPost]
        public IActionResult UpdatePriceActivity(PriceList model)
        {
            model.DetaNews = DateTime.Now;
            if (!ModelState.IsValid)
            {

                return View(model);
            }
            _gPriceList.Update(model);
            return Redirect("/panel/priceActivitylist");
        }
        [permissionChecker(1009)]
        [Route("panel/Creatpricelist")]
        public IActionResult Creatpricelist()
        {
            PriceActivity();
            return View();
        }
        [permissionChecker(1009)]
        [Route("panel/Creatpricelist")]
        [HttpPost]
        public IActionResult Creatpricelist(PriceList model)
        {
            model.DetaNews = DateTime.Now;
            if (!ModelState.IsValid)
            {
                PriceActivity();
                return View(model);
            }
            if (_gPriceList.GetEntity(p => p.Activity == model.Activity) != null)
            {
                ModelState.AddModelError("Activity", "چنیین نام فعالیتی قبلا ثبت شده است");
                return View(model);
            }
            _gPriceList.Insert(model);
            return Redirect("/panel/pricelist");
        }

        [permissionChecker(1009)]
        [Route("panel/Updatepricelist/{id}")]
        public IActionResult Updatepricelist(int id)
        {
            PriceActivity();
            var pricelist = _gPriceList.GetById(id);
            if (pricelist != null)
            {
                return View(pricelist);
            }
            return Redirect("/panel/pricelist");
        }

        [permissionChecker(1009)]
        [Route("panel/Updatepricelist/{id}")]
        [HttpPost]
        public IActionResult Updatepricelist(PriceList model)
        {
            model.DetaNews = DateTime.Now;
            if (!ModelState.IsValid)
            {
                PriceActivity();
                return View(model);
            }
            _gPriceList.Update(model);
            return Redirect("/panel/pricelist");
        }

        [permissionChecker(1009)]
        [Route("panel/DeletePricelist/{id}")]
        public IActionResult DeletePricelist(int id)
        {
            if (_gPriceList.GetById(id) != null)
            {
                _gPriceList.Delete(id);
            }

            return Redirect("/panel/pricelist");
        }
        [permissionChecker(1009)]
        [Route("panel/DeleteActivityPrice/{id}")]
        public IActionResult DeleteActivityPrice(int id)
        {
            if (_gPriceList.GetById(id) != null)
            {
                _gPriceList.Delete(id);
            }

            return Redirect("/panel/priceActivitylist");
        }


        [Route("panel/UpdateProject/{letternember}/{name?}/{idmsc?}")]
        public IActionResult UpdateProject(string letternember, [FromRoute] string idmsc)
        {
            PriceActivity();
            GetUserWhitLevel();
            var model = _gmsc.GetEntity(p => p.Idmsc == int.Parse(idmsc));
            var mscuser = _msc.GetMscUser(model.Idmsc, _User.FindUser(User.Identity.Name).IdCode, 1);
            if (mscuser != null)
            {

                mscuser.view = 0;
                _gMscUser.Update(mscuser);
            }

            ViewBag.Description = _msc.GetRowstatus(model.Idmsc, true).Description;
            model.Description = "";
            if (Core.TMU.Genarator.SpelitTag.tag(model.Activity).Count() > 1)
            {
                ViewBag.disabel = "متمم";
            }
            #region sender system   
            if (_msc.GetMscUser(model.Idmsc, true, false) != null && _msc.GetMscUser(model.Idmsc, true, false).idsender == User.Identity.Name && _msc.GetMscUser(model.Idmsc, true, false).idsender == "System")
            {
                var mscusersystem = _msc.GetMscUser(model.Idmsc, true, false);
                mscusersystem.status = false;
                mscusersystem.view = 0;
                _gMscUser.Update(mscusersystem);

            }
            #endregion
            if (model.CaregouryActivity != "00")
            {
                model.Activity = (_msc.FirstgetPricelistbyid(Core.TMU.Genarator.SpelitTag.tag(model.Activity)[0]).IdPL).ToString();

            }

            ViewBag.A = model.Activity;
            return View(model);
        }

        [Route("panel/UpdateProject/{letternember}/{name?}/{idmsc?}")]
        [HttpPost]
        public IActionResult UpdateProject(msc model, List<int> well, string? activityname, List<int> parcel, string? userreciver, bool check = false, bool validation = false, string persent = "0")
        {

            try
            {
                if (model.Isclose == true)
                {
                    return Redirect("/Panel/Statusface");
                }
                if (model.CaregouryActivity == "00")
                {
                    model.Activity = activityname;
                }
                if (!ModelState.IsValid)
                {
                    PriceActivity();
                    GetUserWhitLevel();
                    ViewBag.A = (_msc.FirstgetPricelistbyid(Core.TMU.Genarator.SpelitTag.tag(model.Activity)[0]).IdPL).ToString();
                    ViewBag.Description = _msc.GetRowstatus(model.Idmsc, true).Description;
                    model.Description = "";
                    return View(model);
                }
                if (model.Activity == "" || model.CaregouryActivity == "")
                {
                    return View(model);
                }
                //var dd = float.Parse(model.Volume.Replace(".", "/")) - float.Parse(persent);
                //if (0<dd && dd < 1)
                //{
                //    //نباید کمتر از 1 باشد
                //    return View(model);
                //}
                int view = 1;
                if (model.CaregouryActivity != "00")
                {
                    model.Activity = _msc.getPricelistbyid(int.Parse(model.Activity)).Activity;
                }
                if (model.IdmscM != 0)
                {
                    model.Activity = model.Activity + "-متمم";
                }
                switch (model.Location)
                {
                    case "فولاد مبارکه":
                        model.Seri = null;
                        model.parcel = null;
                        model.Well = null;
                        break;

                    case "سری":
                        model.parcel = null;
                        model.Well = null;
                        break;

                    case "پارسل":

                        model.Well = null;
                        model.parcel = "";
                        foreach (var item in parcel)
                        {
                            model.parcel += item + "-";
                        }
                        break;

                    case "چاه":
                        model.Seri = null;
                        model.parcel = null;
                        foreach (var item in well)
                        {
                            model.Well += item + "-";
                        }
                        break;

                    default:
                        break;
                }
                if (model.Description == null)
                {
                    model.Description = "توضیحاتی درج نشد!!";
                }

                #region return
                if (check == true)
                {
                    //در اینجا با لول هر کاربری که بخواهیم ولی ما لول سطح پایین تر را می خوایم پستش را بدست می اوریم  
                    var role = _permision.GetRoleWhitLevellist((int.Parse(_User.FindUser(User.Identity.Name).level) - 1).ToString());
                    //در اینجا شخصی که با ان لول و سطح پیامی برای ما ارسال کرد به ان پاس می دهیم
                    userreciver = _msc.GetUserSenderinMscuserToUserWhitRole(role, model.Idmsc);
                    model.DetaNews = DateTime.Now;
                    _msc.updatetask(model);
                    _msc.SetLastStatus(model.Idmsc);
                    _msc.Addmscuser(model, _User.FindUser(User.Identity.Name).IdCode, userreciver, false);
                    return Redirect("/Panel/Cartable/send");
                }
                #endregion

                #region sender system   
                if (_msc.GetMscUser(model.Idmsc, true, false) != null && _msc.GetMscUser(model.Idmsc, true, false).idsender == "System")
                {
                    var mscusersystem = _msc.GetMscUser(model.Idmsc, true, false);
                    mscusersystem.status = false;
                    mscusersystem.view = 0;
                    _gMscUser.Update(mscusersystem);
                    return Redirect("/Panel");
                }
                #endregion

                #region insertAndicator
                var t = _permision.GetRoleById(int.Parse(_User.FindUser(User.Identity.Name).post)).Finalapproval;
                if (_permision.GetRoleById(int.Parse(_User.FindUser(User.Identity.Name).post)).Finalapproval == true && model.Andicator == "0")
                {
                    model.number = _msc.Maxletternember() + 1;
                    model.Andicator = User.Identity.Name + "-" + model.number;
                    MscPercent p = new MscPercent()
                    {
                        DetaNews = DateTime.Now,
                        Idmsc = model.Idmsc,
                        IdCodeUser = User.Identity.Name,//چون معلوم نیست امضا کننده کیست
                        PercentUser = persent,
                        Percent = "0",
                        status = false,
                        Year = DateTime.Now.ToPeString("yyyy")
                    };
                    _gMscPercent.Insert(p);

                }
                #endregion

                #region insertPersentEndProcess
                if (_permision.GetRoleById(int.Parse(_User.FindUser(User.Identity.Name).post)).EndProcess == true)
                {
                    if (_msc.FindMscPersent(model.Idmsc) != null)
                    {
                        validation = true;
                        _msc.setPercentUser(model.Idmsc, persent);

                    }
                }

                #endregion

                #region send to who is permition PPrint
                //اگر درست بود و خودش دسترسی امضا دارد برو به کسی که دسترسی پرینت یا ناظر است
                if (_msc.GetStatusValidition(model.Idmsc, true, true) == true && (_permision.GetRoleById(int.Parse(_User.FindUser(User.Identity.Name).post)).Signuchure == true))
                {
                    validation = true;
                    Role role = new Role();
                    if (_gMscPercent.GetEntity(p => p.Idmsc == model.Idmsc).statusstatusface == true)
                    {
                        view = 0;
                        role = _permision.Getsignuchure();//اگر مدیر تایید کرد و رسید به دست کسی که امضا می کند که بعد تایید برای خودش بایگانی شد
                        userreciver = _msc.GetUserSenderinMscuserToUserWhitRole(role, model.Idmsc);
                        if (userreciver == null)
                        {
                            userreciver = _msc.GetUserSenderinMscuserToUserWhitRole(role, model.IdmscM);
                        }
                    }
                    else
                    {
                        userreciver = _msc.GetFirstrowmscuser(model.Idmsc).idsender;
                        _msc.setLastPercentUser(model.Idmsc, persent);
                    }





                }
                #endregion

                #region send firstperson to who is permition PPrint
                if (_msc.GetStatusValidition(model.Idmsc, true, true) == true && _msc.GetFirstrowmscuser(model.Idmsc).idsender == User.Identity.Name)
                {
                    validation = true;
                    Role role = new Role();

                    role = _permision.GetPermitionPrintRole();//روال سیکل خودش را جلو می برد
                    _msc.setLastPercentUser(model.Idmsc, persent);



                    userreciver = _msc.GetUserSenderinMscuserToUserWhitRole(role, model.Idmsc);
                    if (userreciver == null)
                    {
                        userreciver = _msc.GetUserSenderinMscuserToUserWhitRole(role, model.IdmscM);
                    }
                }
                #endregion

                #region send to who is permition FimalPrint
                //اگر درست بود و خودش دسترسی امضا دارد برو به کسی که رئیس است
                if (_msc.GetStatusValidition(model.Idmsc, true, true) == true && (_permision.GetRoleById(int.Parse(_User.FindUser(User.Identity.Name).post)).PermissionPrint == true))
                {
                    validation = true;
                    var role = _permision.GetFinalRole();
                    userreciver = _msc.GetUserSenderinMscuserToUserWhitRole(role, model.Idmsc);
                    if (userreciver == null)
                    {
                        userreciver = _msc.GetUserSenderinMscuserToUserWhitRole(role, model.IdmscM);
                    }
                    _msc.setLastPercentUser(model.Idmsc, persent);

                }
                #endregion

                #region confraim  permition FimalPrint
                //اگر درست بود و تایید نهایی بود بده به کسی که حق امضا دارد
                if (_msc.GetStatusValidition(model.Idmsc, true, true) == true && (_permision.GetRoleById(int.Parse(_User.FindUser(User.Identity.Name).post)).Finalapproval == true))
                {
                    validation = true;
                    var role = _permision.Getsignuchure();
                    var mscpercet = _gMscPercent.GetEntity(p => p.Idmsc == model.Idmsc);
                    userreciver = _msc.GetUserSenderinMscuserToUserWhitRole(role, model.Idmsc);
                    mscpercet.IdCodeUser = userreciver;//صورت وضعیت به نام امضا کننده می خورد

                    if (model.CaregouryActivity != "00")
                    {
                        var pricelist = _gPriceList.GetEntity(p => p.Activity == model.Activity);
                        float minesvalue = float.Parse(pricelist.value) - float.Parse(persent);//کم کردن از مقدار حجم اولیه  از مفدار تایید شده
                        float plusvalue = float.Parse(pricelist.valueUse) + float.Parse(persent);//اضافه کردن به کار انجام شده
                        pricelist.value = (minesvalue).ToString();
                        pricelist.valueUse = (plusvalue).ToString();
                        _gPriceList.Update(pricelist);
                    }

                    #region send to Gis

                    sendgis(model.Idmsc, model.letter_number);
                    #endregion

                    if (userreciver == null)
                    {
                        userreciver = _msc.GetUserSenderinMscuserToUserWhitRole(role, model.IdmscM);

                    }
                    _gMscPercent.Update(mscpercet);
                    _msc.setPercent(model.Idmsc, persent);
                    model.Isclose = true;
                    _gmsc.Update(model);
                    var f = _msc.IsFinalMscProject(model.Idmsc);//ایا پروژه تمام شد؟
                    if (f == true)
                    {
                        //پروژه تمام شده
                    }

                }
                #endregion

                model.DetaNews = DateTime.Now;
                _msc.updatetask(model);
                _msc.SetLastStatus(model.Idmsc);
                if (_gMscPercent.GetEntity(p => p.Idmsc == model.Idmsc) != null && SpelitTag.tag(_gMscPercent.GetEntity(p => p.Idmsc == model.Idmsc).PercentUser).Count() > 1)//جهت ثبت در بایگانی هر شخص mscuser
                {
                    model.Volume = persent;
                }
                _msc.Addmscuser(model, _User.FindUser(User.Identity.Name).IdCode, userreciver, validation, view);
                return Redirect("/Panel/Cartable/send");
            }
            catch (Exception)
            {

                return Redirect("/Panel/Statusface");
            }
        }



        [Route("panel/SubmitGIS")]
        public IActionResult SubmitGIS(string letternember, string idmsc)
        {

            var model = _msc.GetMscUser(int.Parse(idmsc), true, false);
            if (model != null && model.idsender == "System")
            {
                model.idreciver = model.idsender;
                model.idsender = User.Identity.Name;
                model.status = false;
                model.Description = "ثبت شد";
                model.Deta = DateTime.Now;
                model.factoroffer = "-";
                model.Volume = "-";
                model.TotalPrice = "-";
                _gMscUser.Update(model);
                return Redirect("/Panel/Cartable/send");
            }
            else
            {
                return Redirect("/Panel/Cartable/reciver");
            }


        }


        [Route("panel/Submitproject")]
        public IActionResult Submitproject(string letternember, string idmsc)
        {

            //var model = _msc.GetMscUser(int.Parse(idmsc), true, true);
            //if (model != null)
            //{
            string userreciver = "";
            Role role = new Role();
            //if (_gMscPercent.GetEntity(p => p.Idmsc == int.Parse(idmsc)).statusstatusface == true)
            //{
                role = _permision.Getsignuchure();//اگر مدیر تایید کرد و رسید به دست کسی که امضا می کند که بعد تایید برای خودش بایگانی شد
                userreciver = _msc.GetUserSenderinMscuserToUserWhitRole(role, int.Parse(idmsc));
                if (userreciver == null)
                {
                    userreciver = _msc.GetUserSenderinMscuserToUserWhitRole(role, int.Parse(idmsc));
                }

                _msc.SetLastStatusvalidation(int.Parse(idmsc), true);
                MscUser model = new MscUser();
                model.idreciver = userreciver;
                model.idsender = userreciver;
                model.Idmsc = int.Parse(idmsc);
                model.status = true;
                model.validation = true;
                model.Description = "ثبت شد";
                model.Deta = DateTime.Now;
                model.factoroffer = "-";
                model.Volume = "-";
                model.TotalPrice = "-";
                model.Year = DateTime.Now.ToPeString("yyyy");
                _gMscUser.Insert(model);
                return Redirect($"/Panel/Statusface?year={DateTime.Now.ToPeString("yyyy")}&Contractor={User.Identity.Name}");
            //}
            //else
            //{
            //    return Redirect("/Panel/Cartable/reciver");
            //}


        }


        [Route("panel/send")]
        [HttpPost]
        public IActionResult Send(string Sender, string reciver, int idmsc)
        {
            _msc.Sendmscuserauthor(idmsc, Sender, reciver);
            return Redirect("/Panel/Cartable");
        }
        [Route("panel/Cartable/{name?}")]
        public IActionResult Cartable(int pageid = 1, string year = null, string filtertitel = null, string tag = null, string name = null, int take = 20)
        {

            var Filtersend = "";
            var Filterrecive = "";
            ViewBag.filtertitel = filtertitel;
            ViewBag.tag = tag;
            ViewBag.year = year;
            ViewBag.take = take;
            if (name == "send")
            {
                Filtersend = _User.FindUser(User.Identity.Name).IdCode;
                return View(_msc.GetAllMscUser(pageid, filtertitel, tag, 10, null, false, Filtersend, false, false, false, null, year));

            }
            else
            {
                Filterrecive = _User.FindUser(User.Identity.Name).IdCode;
                return View(_msc.GetAllMscUser(pageid, filtertitel, tag, 10, Filterrecive, true, null, true, false, false, null, year));

            }

        }

        #endregion


        public void Getinfopagepermistion()
        {
            ViewData["Role"] = _permision.GetAllRole();
            ViewData["Permision"] = _permision.GetAllPermission();


        }
        public void Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        }
        #region msc 

        public void sendgis(int idmsc, string leternumber)
        {
            var listcheckoutuser = _permision.Getcheckouttrue();//گرفتن نقشی که gis است
            if (listcheckoutuser != null)
            {
                var userid = _permision.GetUserWhitRoleId(listcheckoutuser.RoleId);
                foreach (var item in userid)
                {
                    MscUser mscUser = new MscUser()
                    {
                        idsender = "System",
                        idreciver = _User.FindUser(item).IdCode,
                        status = true,
                        validation = false,
                        Description = "از طرف سیستم ارسال شد",
                        Deta = DateTime.Now,
                        Year = DateTime.Now.ToPeString("yyyy"),
                        view = 1,
                        Idmsc = idmsc,//یک مقدار الکی داده شد
                        statusface = leternumber,
                        IdAU = 2,
                        TotalPrice = "0",
                        factoroffer = "1",
                        Volume = "1"
                        //IdAU = _msc.getbyleternumber(leternumber).IdAU

                    };
                    _gMscUser.Insert(mscUser);
                }
            }
        }
        public void PriceActivity()
        {
            var subgroup = _msc.PriceActivity();

            ViewBag.PriceActivity = new SelectList(subgroup, "Value", "Text");


        }
        public void Activity()
        {
            var subgroup = _msc.Activity();

            ViewBag.Activity = new SelectList(subgroup, "Value", "Text");


        }
        [HttpGet]
        public ActionResult GetActivityUnderPriceActivity(string id)
        {
            try
            {
                var subgroup = _msc.GetActivityUnderPriceActivity(int.Parse(id));
                if (subgroup.Count() != 0)
                {
                    return Json(new SelectList(subgroup, "Value", "Text"));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }

        }
        public void CategouryActivity()
        {
            var subgroup = _msc.CategouryActivity();

            ViewBag.CategouryActivity = new SelectList(subgroup, "Value", "Text");


        }
        //public void unit()
        //{
        //    var subgroup = _msc.unit();

        //    ViewBag.unit = new SelectList(subgroup, "Value", "Text");


        //}
        //public void Price()
        //{
        //    var subgroup = _msc.Price();
        //    ViewBag.Price = new SelectList(subgroup, "Value", "Text");
        //}
        //public void factoroffer()
        //{
        //    var subgroup = _msc.factoroffer();
        //    ViewBag.factoroffer = new SelectList(subgroup, "Value", "Text");

        //}
        [HttpGet]
        public ActionResult factoroffer(string id)
        {
            try
            {
                var subgroup = _msc.getPricelistbyid(int.Parse(id)).factoroffer;
                if (subgroup.Count() != 0)
                {
                    return Content(subgroup);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }

        }
        [HttpGet]
        public ActionResult price(string id)
        {
            try
            {
                var subgroup = _msc.getPricelistbyid(int.Parse(id)).Price;
                if (subgroup.Count() != 0)
                {
                    return Content(subgroup);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }

        }
        [HttpGet]
        public ActionResult unit(string id)
        {
            try
            {
                var subgroup = _msc.getPricelistbyid(int.Parse(id)).unit;
                if (subgroup.Count() != 0)
                {
                    return Content(subgroup);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }

        }
        public void GetUserWhitLevel()
        {
            var p = SpelitTag.tag(_User.FindUser(User.Identity.Name).level);
            if (p.Last() == "")
            {
                Array.Resize(ref p, p.Length - 1);

            }
            ViewBag.reciveruser = new SelectList(_User.finduserwhitpostandlevel(null, ((Array.ConvertAll(p, int.Parse).Max()) + 1).ToString()), "Value", "Text");

        }

        public void GetAllUserDBMP()
        {
            ViewBag.GetAllUserDBMP = new SelectList(_msc.GetAllUserinDb(), "Value", "Text");
        }
        #endregion

        public void RefreshMenu()
        {
            ViewBag.select1 = new SelectList(_Menu.GetSelect1(), "Value", "Text");
        }
        public void RefreshNavbar()
        {
            ViewBag.select1courseNavbar = new SelectList(_Navbar.GetSelect1("آموزشی"), "Value", "Text");
            ViewBag.select1Navbar = new SelectList(_Navbar.GetSelect1(), "Value", "Text");
            ViewBag.select2Navbar = new SelectList(_Navbar.GetSelect2(int.Parse(_Navbar.GetSelect1().First().Value)), "Value", "Text");
        }
        public ActionResult GetNavbar2(int id)
        {
            var subgroup = _Navbar.GetSelect2(id);
            if (subgroup.Count() != 0)
            {
                return Json(new SelectList(subgroup, "Value", "Text"));
            }
            else
            {
                return null;
            }

        }
        public ActionResult GetSelect2(int id)
        {
            var subgroup = _Menu.GetSelect2(id);
            if (subgroup.Count() != 0)
            {
                return Json(new SelectList(subgroup, "Value", "Text"));
            }
            else
            {
                return null;
            }

        }

        [Route("panel/UploadFile")]
        public ActionResult UploadFile(IFormFile? file)
        {
            if (file != null)
            {
                if (IsExist.ExistFile("gharardad.pdf", @"wwwroot\file\") == true)
                {
                    Delete.DeleteFile("gharardad.pdf", @"wwwroot\file\");
                }
                Save.SaveFile(file, @"wwwroot\file\", "gharardad.pdf");
                return Redirect("/panel");
            }
            return Redirect("ProfileHome");

        }
        [Route("panel/UploadFileFactor")]
        public ActionResult UploadFileFactor(IFormFile? file, string namefile)
        {
            if (file != null)
            {
                if (IsExist.ExistFile(namefile + ".pdf", @"wwwroot\file\Factor\") == true)
                {
                    Delete.DeleteFile(namefile + ".pdf", @"wwwroot\file\Factor\");
                }
                Save.SaveFile(file, @"wwwroot\file\Factor\", namefile + ".pdf");
                return Redirect("/Statusface");
            }
            return Redirect("Statusface");

        }


        //ثبت نقش به کاربر
        public IActionResult AddUserRoles(string idcode, List<int> SelectListItem)
        {

            _permision.AddRole(SelectListItem, _User.getPkId(idcode));
            return Redirect("ListUser");
        }

    }
}
