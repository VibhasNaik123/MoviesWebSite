using MovieWebSite.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesWebSite.Manager
{
    public class ImageManager
    {
        public decimal SaveImage(string imagePath,string title)
        {
            using (MoviesEntities db = new MoviesEntities())
            {
                tblImage tImage = new tblImage();
                tImage.ImagePath = imagePath;
                tImage.ImageTitle = title;
                tImage.IsDeleted = false;
                tImage.CreatedDateTime = DateTime.Now;
                tImage.LastModifedDateTime = DateTime.Now;
                db.tblImages.Add(tImage);
                db.SaveChanges();
                return tImage.ImageId;
            }
        }
    }
}
