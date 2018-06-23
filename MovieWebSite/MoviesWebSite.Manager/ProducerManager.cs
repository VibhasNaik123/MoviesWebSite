using MoviesWebSite.Model;
using MovieWebSite.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesWebSite.Manager
{
    public class ProducerManager
    {
        public bool Create(Producer producer)
        {
            try
            {
                using (MoviesEntities db = new MoviesEntities())
                {

                    tblProducer tProducer = new tblProducer();
                    UpdateEntity(ref tProducer, producer);
                    tProducer.CreatedDateTime = DateTime.Now;
                    db.tblProducers.Add(tProducer);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Producer> GetProducers(Decimal? producerId = 0)
        {
            using (MoviesEntities db = new MoviesEntities())
            {
                var producerList = db.tblProducers.Include("tblGender").Select(q => new Producer
                {
                    _Id = q.ProducerId,
                    _Name = q.ProducerName,
                    _DateOfBirth = q.DateOfBirth,
                    _Bio = q.Bio,
                    _Gender = new Gender()
                    {
                        _Name = q.tblGender.GenderName,
                        _Id = q.tblGender.GenderId,
                    }

                }).ToList();
                if (producerId != 0)
                    producerList = producerList.Where(q => q._Id == producerId).ToList();
                return producerList;
            }
        }

        private void UpdateEntity(ref tblProducer tProducer, Producer producer)
        {
            tProducer.ProducerName = producer._Name;
            tProducer.Bio = producer._Bio;
            tProducer.DateOfBirth = producer._DateOfBirth;
            tProducer.IsDeleted = false;
            tProducer.GenderId = producer._SelectedGender;
            tProducer.LastModfiedDateTime = DateTime.Now;
        }

        public void UpdateProducer(Producer producer)
        {
            using (MoviesEntities db = new MoviesEntities())
            {
                tblProducer tProducer = db.tblProducers.Where(q => q.ProducerId == producer._Id).FirstOrDefault();
                UpdateEntity(ref tProducer, producer);
                db.SaveChanges();
            }
        }

        public int DeleteProducer(decimal id)
        {
            using (MoviesEntities db = new MoviesEntities())
            {
                var tProducer = db.tblMovies.Select(q => q.MovieId == id).ToList();
                if (tProducer.Count == 0)
                {
                    tblProducer tblProducer = db.tblProducers.Find(id);
                    db.tblProducers.Remove(tblProducer);
                    db.SaveChanges();
                    return 0;
                }
                return -1;
            }
        }
    }
}
