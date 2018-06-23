using MoviesWebSite.Model;
using MovieWebSite.DataService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesWebSite.Manager
{
    public class ActorManager
    {
        public bool Create(Actor actor)
        {
            ImageManager imageManager = new ImageManager();
            decimal imageId= imageManager.SaveImage(actor._Image.ImagePath,actor._Image.Title);
            try
            {
                if (imageId != 0)
                {
                    using (MoviesEntities db = new MoviesEntities())
                    {
                        tblActor tActor = new tblActor();
                        UpdateActorEntity(ref tActor,actor);
                        tActor.CreatedDateTime = DateTime.Now;
                        tActor.ImageId = imageId;
                        db.tblActors.Add(tActor);
                        db.SaveChanges();
                    }
                    return true;
                }
                else
                    return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        

        private void UpdateActorEntity(ref tblActor tActor,Actor actor)
        {
            tActor.ActorName = actor._Name;
            tActor.Bio = actor._Bio;
            tActor.DateOfBirth = actor._DateOfBirth;
            tActor.IsDeleted = false;
            tActor.GenderId = actor._SelectedGender;
            tActor.LastModifiedDateTime = DateTime.Now;
        }


        public List<Actor> GetActors(Decimal? actorId=0)
        {
            using (MoviesEntities db = new MoviesEntities())
            {
                var actorList = db.tblActors.Include("tblGender").Select(q => new Actor
                {
                    _Id=q.ActorId,
                    _Name = q.ActorName,
                    _DateOfBirth = q.DateOfBirth,
                    _Bio = q.Bio,
                    _Image=new Image()
                    {
                        ImageID=q.ImageId,
                        ImagePath=q.tblImage.ImagePath,
                        Title=q.tblImage.ImageTitle,
                    },
                    _Gender = new Gender()
                    {
                        _Name = q.tblGender.GenderName,
                        _Id = q.tblGender.GenderId,
                    }

                }).ToList();
                if (actorId != 0)
                    actorList = actorList.Where(q => q._Id == actorId).ToList();
                return actorList;
            }
        }

        public void UpdateActor(Actor actor)
        {
            ImageManager imageManager = new ImageManager();
            using (MoviesEntities db = new MoviesEntities())
            {
                Decimal imageId = 0;
                tblActor tActor = db.tblActors.Where(q => q.ActorId == actor._Id).FirstOrDefault();
                if (actor._Image.ImagePath != null)
                    imageId = imageManager.SaveImage(actor._Image.ImagePath, actor._Image.Title);
                UpdateActorEntity(ref tActor, actor);
                if (imageId != 0)
                    tActor.ImageId = imageId;
                db.SaveChanges();
            }

        }

        public int DeleteActor(decimal id)
        {
            using (MoviesEntities db = new MoviesEntities())
            {
                var tActorMovy = db.tblActorMovies.Select(q => q.ActorId == id).ToList();
                if (tActorMovy.Count == 0)
                {
                    tblActor tblActor = db.tblActors.Find(id);
                    tblImage tblImage = tblActor.tblImage;
                    db.tblActors.Remove(tblActor);
                    db.SaveChanges();
                    db.tblImages.Remove(tblImage);
                    db.SaveChanges();
                    return 0;
                }
                return -1;
            }
        }

        private void DeleteActor()
        {

        }
    }
}
