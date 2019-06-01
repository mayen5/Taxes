using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Taxes.Models;

namespace Taxes.Controllers
{
    public class PropertyTypesController : Controller
    {
        private TaxesContext db = new TaxesContext();

        // GET: PropertyTypes
        public ActionResult Index()
        {
            return View(db.PropertyTypes.OrderBy(pt => pt.Description).ToList());
        }

        // GET: PropertyTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyType propertyType = db.PropertyTypes.Find(id);
            if (propertyType == null)
            {
                return HttpNotFound();
            }
            return View(propertyType);
        }

        // GET: PropertyTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropertyTypes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PropertyTypeId,Description,Notes")] PropertyType propertyType)
        {
            if (ModelState.IsValid)
            {
                db.PropertyTypes.Add(propertyType);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && 
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("Index")) 
                    {
                        ModelState.AddModelError(String.Empty, "The are a record with the same description");
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, ex.Message);
                    }
                    
                    return View(propertyType);
                }

                return RedirectToAction("Index");
            }

            return View(propertyType);
        }

        // GET: PropertyTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyType propertyType = db.PropertyTypes.Find(id);
            if (propertyType == null)
            {
                return HttpNotFound();
            }
            return View(propertyType);
        }

        // POST: PropertyTypes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PropertyTypeId,Description,Notes")] PropertyType propertyType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(propertyType).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("Index"))
                    {
                        ModelState.AddModelError(String.Empty, "The are a record with the same description");
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, ex.Message);
                    }

                    return View(propertyType);
                }

                return RedirectToAction("Index");
            }
            return View(propertyType);
        }

        // GET: PropertyTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var propertyType = db.PropertyTypes.Find(id);

            if (propertyType == null)
            {
                return HttpNotFound();
            }
            return View(propertyType);
        }

        // POST: PropertyTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PropertyType propertyType = db.PropertyTypes.Find(id);
            db.PropertyTypes.Remove(propertyType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
