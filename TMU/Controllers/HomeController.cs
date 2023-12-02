using Core.TMU.Service.ITMUService;
using Data.TMU.Model.Gallery;
using Data.TMU.Model.News;
using Data.TMU.Model.Page;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Net.Mime.MediaTypeNames;

namespace TMU.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private IUser _User;
        //private IPermision _permision;
        //private IViewRenderService _renderService;
        private INews _news;
        private IGallery _gallery;
        private IGeneric<News> _gNews;
        private IGeneric<Gallery> _ggallery;
        //private IGeneric<FileNews> _gFileNews;
        //private IGeneric<Menu> _gMenu;
        //private IGeneric<Slider> _gSlider;
        private Imenu _Menu;
        private ISlider _Slider;
        private IGeneric<Page> _gPage;
        public HomeController(IUser User,ISlider Slider, INews news , IGeneric<News> gNews, Imenu Menu, IGeneric<Page> gPage, IGeneric<Gallery> ggallery, IGallery gallery)
        {
            _news = news;
            _gallery = gallery;
            _User = User;
            //_gSlider = gSlider;
            //_permision = permision;
            //_renderService = renderService;
            //_gFileNews = gFileNews;
            _gNews = gNews;
            _ggallery = ggallery;
            _Menu = Menu;
            _Slider = Slider;
            _gPage = gPage;
        }
        [Route("Error")]
        public IActionResult Error()
        {


            return View();
        }
        public IActionResult Index()
        {
            //ViewBag.slider = _Slider.GetAllSlider(1, null, 10);
         
            return View();
        }

        //[Route("MoreNews")]
        //public IActionResult MoreNews(int pageid = 1, string filtertitel = null, string tag = null)
        //{
        //    ViewBag.filtertitel = filtertitel;
        //    ViewBag.tag = tag;
        //    return View(_news.GetAllNews(pageid, filtertitel, tag, 20, false));
        //}
        //[Route("News/{id}/{string?}")]
        //public IActionResult News(int id)
        //{
        //    var news = _gNews.GetById(id);
        //    news.CountView++;
        //    _gNews.Update(news);
        //    return View(news);
        //}


        [Route("MoreGallery")]
        public IActionResult MoreGallery(int pageid = 1, string filtertitel = null, string tag = null)
        {
            ViewBag.filtertitel = filtertitel;
            ViewBag.tag = tag;
            if (_gallery.GetAllGallery(pageid, filtertitel, tag, 7).ListGallery.Count == 0)
            {
                return Redirect("/");
            }
            return View(_gallery.GetAllGallery(pageid, filtertitel, tag, 20));
        }
        [Route("Gallery/{id}/{string?}")]
        public IActionResult Gallery(int id)
        {
            var gallery = _ggallery.GetById(id);
            gallery.View++;
            _ggallery.Update(gallery);
            return View(gallery);
        }

        //[Route("Search")]
        //public IActionResult Search(int pageid = 1, string NameForest = null, string tag = null, string Parcel = null, int Class = 0, int speiesvalue = 0, string conservationstatus = null, string specieslists = null)
        //{
        //    Getconservationstatus();
        //    GetParcel();
        //    GetForest();
        //    RefreshMenu();
        //    SearchListViewModel h = new SearchListViewModel()
        //    {
        //        nameforest = NameForest,
        //        parcel = Parcel,
        //        conservationstatus = conservationstatus
        //    };
        //    //ViewBag.NameForest = j;
        //    var t = _Forest.GetAllForest(pageid, NameForest, Parcel, Class, speiesvalue, conservationstatus, specieslists, 8);
        //    return View(new Tuple<ListForestViewModel, SearchListViewModel>(_Forest.GetAllForest(pageid, NameForest, Parcel, Class, speiesvalue, conservationstatus, specieslists, 12, tag), h));
        //}
        //[Route("ListUser")]
        //public IActionResult ListUser(int pageid = 1, string filterfullname = null, string email = null, string filteridcode = null, string tag = null)
        //{


        //    return View(_User.GetAllUser(pageid, filterfullname, email, filteridcode, tag, 10));
        //}
        //[Route("Page/{id}/{titel?}")]
        //public IActionResult Page(int id)
        //{
        //    var page = _gPage.GetById(id);
        //    page.CountView++;
        //    _gPage.Update(page);
        //    return View(_gPage.GetById(id));
        //}
        //public void RefreshMenu()
        //{
        //    ViewBag.select1 = new SelectList(_Menu.GetSelect1(), "Value", "Text");
        //}

        //public ActionResult GetSelect2(int id)
        //{
        //    var subgroup = _Menu.GetSelect2(id);
        //    if (subgroup.Count() != 0)
        //    {
        //        return Json(new SelectList(subgroup, "Value", "Text"));
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}
        //public void GetForest()
        //{
        //    var forest = _gForest.GetAll().Select(v => new SelectListItem
        //    {
        //        Text = v.NameForest,
        //        Value = v.NameForest
        //    });
        //    ViewBag.Forest = new SelectList(forest, "Value", "Text");
        //}
        //public void GetParcel()
        //{
        //    var parcel = _gForest.GetAll().Select(v => new SelectListItem
        //    {
        //        Text = v.Parcel,
        //        Value = v.Parcel
        //    });
        //    ViewBag.parcel = new SelectList(parcel, "Value", "Text");
        //}
        //public void Getconservationstatus()
        //{
        //    var conservationstatus = _gForest.GetAll().Select(v => new SelectListItem
        //    {
        //        Text = v.conservationstatus,
        //        Value = v.conservationstatus
        //    });
        //    ViewBag.conservationstatus = new SelectList(conservationstatus, "Value", "Text");
        //}

    }
}
