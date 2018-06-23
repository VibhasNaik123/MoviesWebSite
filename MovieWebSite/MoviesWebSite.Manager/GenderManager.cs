using MoviesWebSite.Model;
using MovieWebSite.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesWebSite.Manager
{
    public class GenderManager
    {
        public List<Gender> GetGenders()
        {
            using (MoviesEntities db = new MoviesEntities())
            {
                return db.tblGenders.Select(q => new Gender
                {
                    _Name = q.GenderName,
                    _Id = q.GenderId,
                }).ToList();
            }
        }
    }
}
