using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MoviesWebSite.Manager;
using MoviesWebSite.Model;

namespace MovieWebSite.Controllers
{
    public class ActorController : Controller
    {
        ActorManager _actorManager = new ActorManager();
        GenderManager _genderManager = new GenderManager();

        // GET: Actor
        public ActionResult Index()
        {
            return View(_actorManager.GetActors());
        }

        // GET: Actor/Details/5
        //public ActionResult Details(decimal id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblActor tblActor = db.tblActors.Find(id);
        //    if (tblActor == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblActor);
        //}

        // GET: Actor/Create
        public ActionResult Create()
        {
            ViewBag.Gender = new SelectList(_genderManager.GetGenders(), "_Id", "_Name");
            return View();
        }

        // POST: Actor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Actor actor, HttpPostedFileBase ImageFile,string controllerName,string actionName)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    SaveImage(ImageFile, ref actor);

                    _actorManager.Create(actor);
                    if (controllerName== null && actionName == null)
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction(actionName, controllerName);
                }
            }

            ViewBag.Gender = new SelectList(_genderManager.GetGenders(), "_Id", "_Name", actor._SelectedGender);
            return View();
        }

        private void SaveImage(HttpPostedFileBase imageFile, ref Actor actor)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
            string extension = Path.GetExtension(imageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            actor._Image.ImagePath = "~/ActorImages/" + fileName;
            actor._Image.Title = "Image of " + actor._Name;
            fileName = Path.Combine(Server.MapPath("~/ActorImages/"), fileName);
            imageFile.SaveAs(fileName);
        }

        //GET: Actor/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = _actorManager.GetActors(id).FirstOrDefault();

            if (actor == null)
            {
                return HttpNotFound();
            }
            actor._Id = id;
            ViewBag.Gender = new SelectList(_genderManager.GetGenders(), "_Id", "_Name", actor._SelectedGender);
            return View(actor);
        }

        //// POST: Actor/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Actor actor, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null)
            {
                SaveImage(ImageFile, ref actor);
            }
            _actorManager.UpdateActor(actor);
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
            Actor actor = _actorManager.GetActors(id).FirstOrDefault();
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: Actor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            int result=_actorManager.DeleteActor(id);
            if(result==0)
            return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError(string.Empty, "Actor is part of a movie cant delete");
                Actor actor = _actorManager.GetActors(id).FirstOrDefault();
                return View(actor);
            }
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
