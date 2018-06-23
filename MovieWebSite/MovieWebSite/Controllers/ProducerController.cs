using MoviesWebSite.Manager;
using MoviesWebSite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MovieWebSite.Controllers
{
    public class ProducerController : Controller
    {
        // GET: Producer
        GenderManager _genderManager = new GenderManager();
        ProducerManager _producerManager = new ProducerManager();
        public ActionResult Index()
        {
            return View(_producerManager.GetProducers());
        }
        // GET: Producer
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
        public ActionResult Create(Producer producer)
        {
            if (ModelState.IsValid)
            {
                  _producerManager.Create(producer);

                   return RedirectToAction("Index");
            }

            ViewBag.Gender = new SelectList(_genderManager.GetGenders(), "_Id", "_Name", producer._SelectedGender);
            return View();
        }

        public ActionResult Edit(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producer producer = _producerManager.GetProducers(id).FirstOrDefault();

            if (producer == null)
            {
                return HttpNotFound();
            }
            producer._Id = id;
            ViewBag.Gender = new SelectList(_genderManager.GetGenders(), "_Id", "_Name", producer._SelectedGender);
            return View(producer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Producer producer)
        {
            _producerManager.UpdateProducer(producer);
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
            Producer producer = _producerManager.GetProducers(id).FirstOrDefault();
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }
        // POST: Actor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            int result=_producerManager.DeleteProducer(id);
            if (result == 0)
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError(string.Empty, "Producer is part of a movie cant delete");
                Producer producer = _producerManager.GetProducers(id).FirstOrDefault();
                return View(producer);
            }
        }
    }
}