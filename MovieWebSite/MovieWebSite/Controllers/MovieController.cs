using MoviesWebSite.Manager;
using MoviesWebSite.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MovieWebSite.Controllers
{
    public class MovieController : Controller
    {
        // GET: Movie
        ActorManager _actorManager = new ActorManager();
        ProducerManager _producerManager = new ProducerManager();
        MovieManager _movieManager = new MovieManager();
        GenderManager _genderManager = new GenderManager();
        // GET: Actor
        public ActionResult Index()
        {
            return View(_movieManager.GetMovies());
        }

        public ActionResult Create()
        {
            FillActorAndProducerList();
            
            return View();
        }

        private void FillActorAndProducerList()
        {
            var Actors = _actorManager.GetActors();
            List<SelectListItem> actorSelectList = new List<SelectListItem>();
            foreach (var actor in Actors)
            {
                var item = new SelectListItem
                {
                    Value = actor._Id.ToString(),
                    Text = actor._Name,
                };

                actorSelectList.Add(item);
            }
            ViewBag.Actors = actorSelectList;
            ViewBag.Producers = new SelectList(_producerManager.GetProducers(), "_Id", "_Name");
            ViewBag.Gender = new SelectList(_genderManager.GetGenders(), "_Id", "_Name");
        }

        [HttpPost]
        public ActionResult Create(Movie movie,HttpPostedFileBase PosterImage)
        {
            if(ModelState.IsValid)
            {
                if (PosterImage != null)
                {
                    SaveImage(PosterImage, ref movie);

                    _movieManager.Create(movie);

                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [HttpGet]
        //GET: Actor/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = _movieManager.GetMovies(id).FirstOrDefault();
            movie._SelectedActors = movie._Actors.Select(q => q.Key).ToList();
            if (movie == null)
            {
                return HttpNotFound();
            }
            movie._Id = id;
            FillActorAndProducerList();
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Movie movie, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null)
            {
                SaveImage(ImageFile, ref movie);
            }
            _movieManager.UpdateMovie(movie);
            //ViewBag.Gender = new SelectList(_genderManager.GetGenders(), "_Id", "_Name", actor._SelectedGender);
            return RedirectToAction("Index");
        }

        // GET: Actor/Delete/5
        [HttpGet]
        public ActionResult Delete(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = _movieManager.GetMovies(id).FirstOrDefault();
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            _movieManager.DeleteMovie(id);

            return RedirectToAction("Index");
        }

        private void SaveImage(HttpPostedFileBase posterImage, ref Movie movie)
        {
            string fileName = Path.GetFileNameWithoutExtension(posterImage.FileName);
            string extension = Path.GetExtension(posterImage.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            movie._Image.ImagePath = "~/MovieImages/" + fileName;
            movie._Image.Title = "Image of " + movie._Name;
            fileName = Path.Combine(Server.MapPath("~/MovieImages/"), fileName);
            posterImage.SaveAs(fileName);
        }
    }
}