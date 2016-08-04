using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IGSImageSlider.Models;

namespace IGSImageSlider.Controllers
{
    public class ImageSliderController : Controller
    {
        // GET: ImageSlider
        public ActionResult Index()
        {
            //IEnumerable<int> imageIds = new List<int> { 1, 2, 3 };
            //using (ImageDBContext dbContext = new ImageDBContext())
            //{
            //    foreach (var id in imageIds)
            //    {
            //        var image = dbContext.imageGallery.Single(s => s.Id == id);
            //        string imagePath = Server.MapPath(image.ImagePath);
            //        dbContext.imageGallery.Remove(image);

            //        if (System.IO.File.Exists(imagePath))
            //        {
            //            System.IO.File.Delete(imagePath);
            //        }
            //    }
            //    dbContext.SaveChanges();
            //}

            using (ImageDBContext dbContext = new ImageDBContext())
            {
                return View(dbContext.imageGallery.ToList());
            }

            //return View();
        }

        public ActionResult AddImage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddImage(HttpPostedFileBase imagePath)
        {
            if(imagePath != null)
            {
                // ideal to upload images of the same size
                System.Drawing.Image uploadStream = System.Drawing.Image.FromStream(imagePath.InputStream);
                //if(uploadStream.Width != 800 || uploadStream.Height != 356)
                //{
                //    ModelState.AddModelError("", "Image Resolution must be 800 * 356 pixels");
                //    return View();
                //}

                // Upload the image
                string uploadImage = System.IO.Path.GetFileName(imagePath.FileName);
                string uploadImagePath = System.IO.Path.Combine(Server.MapPath("~/Content/images/"), uploadImage);
                imagePath.SaveAs(uploadImagePath);

                using (ImageDBContext dbContext = new ImageDBContext())
                {
                    ImageGallery imgGallery = new ImageGallery { ImagePath = "~/Content/images/" + uploadImage };
                    dbContext.imageGallery.Add(imgGallery);
                    dbContext.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteImages()
        {
            using(ImageDBContext dbContext = new ImageDBContext())
            {
                return View(dbContext.imageGallery.ToList());
            }
        }

        [HttpPost]
        public ActionResult DeleteImages(IEnumerable<int> imageIds)
        {
            using(ImageDBContext dbContext = new ImageDBContext())
            {
                foreach(var id in imageIds)
                {
                    var image = dbContext.imageGallery.Single(s => s.Id == id);
                    string imagePath = Server.MapPath(image.ImagePath);
                    dbContext.imageGallery.Remove(image);

                    if(System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                dbContext.SaveChanges();
            }

            return RedirectToAction("DeleteImages");
        }
    }
}