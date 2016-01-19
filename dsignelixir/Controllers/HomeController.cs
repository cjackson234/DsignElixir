using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dsignelixir.Models;

namespace dsignelixir.Controllers
{

    public class HomeController : Controller
    {

        private readonly IAlbumRepository albumRepository;
		private readonly IImageRepository imageRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public HomeController() : this(new AlbumRepository(), new ImageRepository())
        {
        }

        public HomeController(IAlbumRepository albumRepository, IImageRepository imageRepository)
        {
			this.albumRepository = albumRepository;
			this.imageRepository = imageRepository;
        }

        public ActionResult Index()
        {
            var albums = albumRepository.All.Where(a => a.Category.Name == "Homepage");
            return View(albums);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View(new Contact());
        }

        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return View(contact);
            }


            new Email().Send(contact);

            return RedirectToAction("ContactConfirm");
        }

        public ActionResult ContactConfirm()
        {
            return View();
        }

        public ActionResult TheElixerFixer()
        {
            var albums = albumRepository.All.Where(a => a.Category.Name == "TheElixerFixer").OrderBy(b=>b.OrderNumber);
            return View(albums);
        }

        public ActionResult Apparel()
        {
            var albums = albumRepository.All.Where(a => a.Category.Name == "Apparel").OrderBy(b => b.OrderNumber);
            return View(albums);
        }


        public ActionResult Signs()
        {
            var albums = albumRepository.All.Where(a => a.Category.Name == "Signs").OrderBy(b => b.OrderNumber);
            return View(albums);
        }

        public ActionResult GraphicDesign()
        {
            var albums = albumRepository.All.Where(a => a.Category.Name == "GraphicDesign").OrderBy(b => b.OrderNumber);
            return View(albums);
        }

        public ActionResult WebDesign()
        {
            var albums = albumRepository.All.Where(a => a.Category.Name == "WebDesign").OrderBy(b => b.OrderNumber);
            return View(albums);
        }


    }
}
