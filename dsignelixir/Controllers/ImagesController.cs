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
    public class ImagesController : Controller
    {
		private readonly IAlbumRepository albumRepository;
		private readonly IImageRepository imageRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public ImagesController() : this(new AlbumRepository(), new ImageRepository())
        {
        }

        public ImagesController(IAlbumRepository albumRepository, IImageRepository imageRepository)
        {
			this.albumRepository = albumRepository;
			this.imageRepository = imageRepository;
        }

        //
        // GET: /Images/

        public ViewResult Index()
        {
            return View(imageRepository.AllIncluding(image => image.Album));
        }

        //
        // GET: /Images/Details/5

        public ViewResult Details(int id)
        {
            return View(imageRepository.Find(id));
        }

        //
        // GET: /Images/Create

        public ActionResult Create()
        {
			ViewBag.PossibleAlbums = albumRepository.All;
            return View();
        } 

        //
        // POST: /Images/Create

        [HttpPost]
        public ActionResult Create(Image image)
        {
            if (ModelState.IsValid) {
                imageRepository.InsertOrUpdate(image);
                imageRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleAlbums = albumRepository.All;
				return View();
			}
        }
        
        //
        // GET: /Images/Edit/5
 
        public ActionResult Edit(int id)
        {
			ViewBag.PossibleAlbums = albumRepository.All;
             return View(imageRepository.Find(id));
        }

        //
        // POST: /Images/Edit/5

        [HttpPost]
        public ActionResult Edit(Image image)
        {
            if (ModelState.IsValid) {
                imageRepository.InsertOrUpdate(image);
                imageRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleAlbums = albumRepository.All;
				return View();
			}
        }

        //
        // GET: /Images/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(imageRepository.Find(id));
        }

        

        //
        // POST: /Images/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            imageRepository.Delete(id);
            imageRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                albumRepository.Dispose();
                imageRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        public FileContentResult GetImage(int id)
        {
            var byteArray = imageRepository.Find(id).Bytes;
            return byteArray != null ? new FileContentResult(byteArray, "image/jpeg") : null;
        }

        [HttpPost]
        public ActionResult Upload(int? chunk, string name, int Id)
        {
            var fileUpload = Request.Files[0];
            var next = albumRepository.Find(Id).Images.Count() + 1;
            var i = new Image {Bytes = ImageResult(fileUpload), AlbumId = Id, OrderNumber = next};
            imageRepository.InsertOrUpdate(i);
            imageRepository.Save();
            return Content("chunk uploaded", "text/plain");
        }

        [HttpPost]
        public ActionResult UpdateOrder(int id, int? fromPosition, int? toPosition, string direction)
        {
            var i = imageRepository.Find(id);
            var images = i.Album.Images;
            if (direction == "back")
            {
                var move = images.Where(c => (toPosition <= c.OrderNumber && c.OrderNumber <= fromPosition))
                       .ToList();

                foreach (var m in move)
                {
                    m.OrderNumber++;
                    imageRepository.InsertOrUpdate(m);
                    imageRepository.Save();
                }
                
            }
            else
            {
                var move = images.Where(c => (fromPosition <= c.OrderNumber && c.OrderNumber <= toPosition))
                       .ToList();

                foreach (var m in move)
                {
                    m.OrderNumber--;
                    imageRepository.InsertOrUpdate(m);
                    imageRepository.Save();
                }
            }

            i.OrderNumber = toPosition;
            imageRepository.InsertOrUpdate(i);
            imageRepository.Save();

            return Content("chunk uploaded", "text/plain");
        }


        public Byte[] ImageResult(HttpPostedFileBase input)
        {
            if (input == null) return null;
            using (var inputStream = input.InputStream)
            {
                var memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
                else
                {
                    return null;
                }

            }
        }
    }
}

