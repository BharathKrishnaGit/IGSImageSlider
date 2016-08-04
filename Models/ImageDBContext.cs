using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace IGSImageSlider.Models
{
    public class ImageDBContext : DbContext
    {
        public DbSet<ImageGallery> imageGallery { get; set; }
    }
}