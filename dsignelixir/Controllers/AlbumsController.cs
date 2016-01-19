using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RozichMurals.Web.Models;
using dsignelixir.Models;

namespace dsignelixir.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IAlbumRepository albumRepository;
        private readonly IImageRepository imageRepository;
        // If you are using Dependency Injection, you can delete the following constructor
        public AlbumsController()
            : this(new CategoryRepository(), new AlbumRepository(), new ImageRepository())
        {
        }

        public AlbumsController(ICategoryRepository categoryRepository, IAlbumRepository albumRepository, ImageRepository imageRepository)
        {
            this.categoryRepository = categoryRepository;
            this.albumRepository = albumRepository;
            this.imageRepository = imageRepository;

        }

        //
        // GET: /Albums/

        public ViewResult Index()
        {
            return View(albumRepository.AllIncluding(album => album.Category, album => album.Images));
        }

        //
        // GET: /Albums/Details/5

        public ViewResult Details(int id)
        {
            return View(albumRepository.Find(id));
        }

        //
        // GET: /Albums/Create

        public ActionResult Create()
        {
            ViewBag.PossibleCategories = categoryRepository.All;
            return View();
        }

        //
        // POST: /Albums/Create

        [HttpPost]
        public ActionResult Create(Album album, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                if (files.First() != null)
                {
                    foreach (var file in files.Where(file => file.ContentLength > 0))
                    {
                        using (var inputStream = file.InputStream)
                        {
                            var memoryStream = inputStream as MemoryStream;
                            if (memoryStream == null)
                            {
                                memoryStream = new MemoryStream();
                                inputStream.CopyTo(memoryStream);
                            }

                            album.AlbumThumb = memoryStream.ToArray();
                            memoryStream = null;


                        }

                    }
                }
                var nextId = albumRepository.All.Count(a => a.CategoryId == album.CategoryId) +1;
                album.OrderNumber = nextId;
                albumRepository.InsertOrUpdate(album);
                albumRepository.Save();
                return RedirectToAction("Details",new { id = album.Id});
            }
            else
            {
                ViewBag.PossibleCategories = categoryRepository.All;
                return View();
            }
        }

        //
        // GET: /Albums/Edit/5

        public ActionResult Edit(int id)
        {
            ViewBag.PossibleCategories = categoryRepository.All;
            return View(albumRepository.Find(id));
        }

        //
        // POST: /Albums/Edit/5

        [HttpPost]
        public ActionResult Edit(Album album, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                if (files.First() != null)
                {
                    foreach (var file in files.Where(file => file.ContentLength > 0))
                    {
                        using (var inputStream = file.InputStream)
                        {
                            var memoryStream = inputStream as MemoryStream;
                            if (memoryStream == null)
                            {
                                memoryStream = new MemoryStream();
                                inputStream.CopyTo(memoryStream);
                            }

                            album.AlbumThumb = memoryStream.ToArray();
                            memoryStream = null;
                        }

                    }
                }

                albumRepository.InsertOrUpdate(album);
                albumRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleCategories = categoryRepository.All;
                return View();
            }
        }

        //
        // GET: /Albums/Delete/5

        public ActionResult Delete(int id)
        {
            return View(albumRepository.Find(id));
        }

        //
        // POST: /Albums/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            albumRepository.Delete(id);
            albumRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                categoryRepository.Dispose();
                albumRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult QuickDelete(int id)
        {

            imageRepository.Delete(id);
            imageRepository.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateOrder(int id, int? fromPosition, int toPosition, string direction)
        {
            var i = albumRepository.Find(id);
            var albums = albumRepository.All.Where(a=>a.CategoryId == i.CategoryId);
            if (direction == "back")
            {
                var move = albums.Where(c => (toPosition <= c.OrderNumber && c.OrderNumber <= fromPosition))
                       .ToList();

                foreach (var m in move)
                {
                    m.OrderNumber++;
                    albumRepository.InsertOrUpdate(m);
                    albumRepository.Save();
                }

            }
            else
            {
                var move = albums.Where(c => (fromPosition <= c.OrderNumber && c.OrderNumber <= toPosition))
                       .ToList();

                foreach (var m in move)
                {
                    m.OrderNumber--;
                    albumRepository.InsertOrUpdate(m);
                    albumRepository.Save();
                }
            }

            i.OrderNumber = toPosition;
            albumRepository.InsertOrUpdate(i);
            albumRepository.Save();

            return Content("chunk uploaded", "text/plain");
        }

        public FileContentResult GetImage(int id)
        {
            var byteArray = albumRepository.Find(id).AlbumThumb;
            return byteArray != null ? new FileContentResult(byteArray, "image/jpeg") : null;
        }
    }
}

