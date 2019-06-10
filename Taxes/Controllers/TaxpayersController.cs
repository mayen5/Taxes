using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Taxes.Clasess;
using Taxes.Models;

namespace Taxes.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TaxpayersController : Controller
    {
        private TaxesContext db = new TaxesContext();

        // GET: Taxpayers
        public ActionResult Index()
        {
            var taxPaers = db.TaxPaers.Include(t => t.Department).Include(t => t.DocumentType).Include(t => t.Municipality);
            return View(taxPaers.ToList());
        }

        // GET: Taxpayers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taxpayer taxpayer = db.TaxPaers.Find(id);
            if (taxpayer == null)
            {
                return HttpNotFound();
            }
            return View(taxpayer);
        }

        // GET: Taxpayers/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description");
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == db.Departments.FirstOrDefault().DepartmentId)
                .OrderBy(m => m.Name), "MunicipalityId", "Name");
            return View();
        }

        // POST: Taxpayers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaxpayerId,FirstName,LastName,UserName,Phone,DepartmentId,MunicipalityId,Address,DocumentTypeId,Document")] Taxpayer taxpayer)
        {
            if (ModelState.IsValid)
            {
                db.TaxPaers.Add(taxpayer);
                try
                {
                    db.SaveChanges();
                    Utilities.CreateUserASP(taxpayer.UserName, "taxpayer");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", taxpayer.DepartmentId);
                    ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", taxpayer.DocumentTypeId);
                    ViewBag.MunicipalityId = new SelectList(db.Municipalities
                        .Where(m => m.DepartmentId == db.Departments.FirstOrDefault().DepartmentId)
                        .OrderBy(m => m.Name), "MunicipalityId", "Name", taxpayer.MunicipalityId);
                    return View(taxpayer);
                }
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", taxpayer.DepartmentId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", taxpayer.DocumentTypeId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == db.Departments.FirstOrDefault().DepartmentId)
                .OrderBy(m => m.Name), "MunicipalityId", "Name", taxpayer.MunicipalityId);
            return View(taxpayer);
        }

        // GET: Taxpayers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taxpayer taxpayer = db.TaxPaers.Find(id);
            if (taxpayer == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", taxpayer.DepartmentId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", taxpayer.DocumentTypeId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == taxpayer.DepartmentId)
                .OrderBy(m => m.Name), "MunicipalityId", "Name", taxpayer.MunicipalityId);
            return View(taxpayer);
        }

        // POST: Taxpayers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaxpayerId,FirstName,LastName,UserName,Phone,DepartmentId,MunicipalityId,Address,DocumentTypeId,Document")] Taxpayer taxpayer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taxpayer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", taxpayer.DepartmentId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", taxpayer.DocumentTypeId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == taxpayer.DepartmentId)
                .OrderBy(m => m.Name), "MunicipalityId", "Name", taxpayer.MunicipalityId);
            return View(taxpayer);
        }

        // GET: Taxpayers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taxpayer taxpayer = db.TaxPaers.Find(id);
            if (taxpayer == null)
            {
                return HttpNotFound();
            }
            return View(taxpayer);
        }

        // POST: Taxpayers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Taxpayer taxpayer = db.TaxPaers.Find(id);
            db.TaxPaers.Remove(taxpayer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetMunicipalities(int departmentId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var municipalities = db.Municipalities
                .Where(m => m.DepartmentId == departmentId)
                .OrderBy(m => m.Name);
            return Json(municipalities);
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
