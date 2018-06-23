using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoviesWebSite.Model;
using MovieWebSite.DataService;

namespace MoviesWebSite.Manager
{
    public class MovieManager
    {
        public bool Create(Movie movie)
        {
            ImageManager imageManager = new ImageManager();
            decimal imageId = imageManager.SaveImage(movie._Image.ImagePath, movie._Image.Title);
            try
            {
                if (imageId != 0)
                {
                    using (MoviesEntities db = new MoviesEntities())
                    {
                        tblMovy tMovie = new tblMovy();
                        UpdateMovieEntity(ref tMovie, movie);
                        tMovie.CreatedDateTime = DateTime.Now;
                        tMovie.ImageId = imageId;                        
                        db.tblMovies.Add(tMovie);
                        db.SaveChanges();
                        MapMovieAndActors(tMovie.MovieId, movie._SelectedActors,db);
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void MapMovieAndActors(decimal movieId, List<Decimal> selectedActors,MoviesEntities db)
        {

            List<tblActorMovy> tActorMovieList = new List<tblActorMovy>();
            foreach (var actor in selectedActors)
            {
                tblActorMovy tActorMovy = new tblActorMovy();
                tActorMovy.MovieId = movieId;
                tActorMovy.ActorId = actor;
                tActorMovy.CreatedDateTime = DateTime.Now;
                tActorMovy.LastModifiedDateTime = DateTime.Now;
                tActorMovy.IsDeleted = false;
                tActorMovieList.Add(tActorMovy);
            }
            db.tblActorMovies.AddRange(tActorMovieList);
            db.SaveChanges();
        }

        private void UpdateMovieEntity(ref tblMovy tMovie, Movie movie)
        {
            tMovie.MovieName = movie._Name;
            tMovie.Plot = movie._Plot;
            tMovie.YearOfRelease = movie._YearOfRelease;
            tMovie.IsDeleted = false;
            tMovie.ProducerId = movie._SelectedProducer;
            tMovie.LastModifiedDateTime = DateTime.Now;
        }
        public List<Movie> GetMovies(Decimal? movieId=0)
        {
            using (MoviesEntities db = new MoviesEntities())
            {
                var movieList = db.tblMovies.Include("tblActorMovies").Select(q => new
                {
                    Movie1 = new Movie
                    {
                        _Id = q.MovieId,
                        _Name = q.MovieName,
                        _YearOfRelease = q.YearOfRelease,
                        _Plot = q.Plot,
                        _Image = new Image()
                        {
                            ImageID = q.ImageId,
                            ImagePath = q.tblImage.ImagePath,
                            Title = q.tblImage.ImageTitle,
                        },

                        _ProducerName = q.tblProducer.ProducerName,
                        _SelectedProducer = q.tblProducer.ProducerId,
                    },
                    _Actor = q.tblActorMovies.Select(r => new { r.ActorId, r.tblActor.ActorName })
                }).AsEnumerable().Select(p =>
                {
                    p.Movie1._Actors = p._Actor.ToDictionary(r => r.ActorId, r => r.ActorName);
                    return p.Movie1;
                });
                if (movieId != 0)
                    movieList = movieList.Where(q => q._Id == movieId).ToList();
                return movieList.ToList();
            }
        }

        public void DeleteMovie(decimal id)
        {
            using (MoviesEntities db = new MoviesEntities())
            {
                DeleteMovieAndActors(id, db);
                tblMovy tMovie = db.tblMovies.Find(id);
                tblImage tblImage = tMovie.tblImage;
                db.tblMovies.Remove(tMovie);
                db.SaveChanges();
                db.tblImages.Remove(tblImage);
                db.SaveChanges();
            }
        }

        public void UpdateMovie(Movie movie)
        {
            ImageManager imageManager = new ImageManager();
            using (MoviesEntities db = new MoviesEntities())
            {
                Decimal imageId = 0;
                tblMovy tMovie = db.tblMovies.Where(q => q.MovieId == movie._Id).FirstOrDefault();
                if (movie._Image.ImagePath != null)
                    imageId = imageManager.SaveImage(movie._Image.ImagePath, movie._Image.Title);
                UpdateMovieEntity(ref tMovie, movie);
                if (imageId != 0)
                    tMovie.ImageId = imageId;
                db.SaveChanges();
                DeleteMovieAndActors(movie._Id, db);
                MapMovieAndActors(movie._Id, movie._SelectedActors, db);
            }
        }

        private void DeleteMovieAndActors(Decimal movieId,MoviesEntities db)
        {
            List<tblActorMovy> tActorMovieList = db.tblActorMovies.Where(q => q.MovieId == movieId).ToList();
            if (tActorMovieList != null && tActorMovieList.Count > 0)
                db.tblActorMovies.RemoveRange(tActorMovieList);
            db.SaveChanges();
        }
    }
}
