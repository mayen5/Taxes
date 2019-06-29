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
    [Authorize(Roles = "Admin")]
    public class TaxesController : Controller
    {
        private TaxesContext db = new TaxesContext();

        public ActionResult GenerateTaxes()
        {
            int year = DateTime.Now.Year;
            var properties = db.Properties.ToList();
            var taxes = db.Taxes.ToList();

            using (var transaction = db.Database.BeginTransaction())
            {

                try
                {
                    foreach (var property in properties)
                    {
                        var taxProperty = db.TaxProperties
                            .Where(tp => tp.PropertyId == property.PropertyId && tp.Year == year)
                            .FirstOrDefault();

                        if (taxProperty == null)
                        {
                            var rate = taxes.Where(t => t.Stratum == property.Stratum).FirstOrDefault();

                            if (rate != null)
                            {
                                taxProperty = new TaxProperty
                                {
                                    DateGenerated = DateTime.Now,
                                    IsPay = false,
                                    PropertyId = property.PropertyId,
                                    Year = year,
                                    Value = property.Value * (decimal)rate.Rate
                                };

                               
                                db.TaxProperties.Add(taxProperty);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    String error = ex.Message;
                    return RedirectToAction("Fail", "Taxes", error);
                }

                transaction.Commit();
            }
            return RedirectToAction("Success");
        }
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Fail(String message)
        {
            ViewBag.Error = message;
            return View();

        }
        // GET: Taxes
        public ActionResult Index()
        {
            return View(db.Taxes.ToList());
        }

        // GET: Taxes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = db.Taxes.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            return View(tax);
        }

        // GET: Taxes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Taxes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaxId,Stratum,Rate")] Tax tax)
        {
            if (ModelState.IsValid)
            {
                db.Taxes.Add(tax);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tax);
        }

        // GET: Taxes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = db.Taxes.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            return View(tax);
        }

        // POST: Taxes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaxId,Stratum,Rate")] Tax tax)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tax).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tax);
        }

        // GET: Taxes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = db.Taxes.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            return View(tax);
        }

        // POST: Taxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tax tax = db.Taxes.Find(id);
            db.Taxes.Remove(tax);
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
