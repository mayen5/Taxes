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
    public class TaxpayersController : Controller
    {
        private TaxesContext db = new TaxesContext();

        [Authorize(Roles = "Taxpayer")]
        public ActionResult MyTaxes()
        {
            var taxpayer = db.Taxpayers.Where(t => t.UserName == this.User.Identity.Name).FirstOrDefault();

            decimal total = 0;

            foreach (var property in taxpayer.Properties.ToList())
            {
                foreach (var taxProperty in property.TaxProperties.ToList())
                {
                    if (taxProperty.IsPay)
                    {
                        property.TaxProperties.Remove(taxProperty);
                    }
                    else
                    {
                        total += taxProperty.Value;
                    }
                }
            }

            var view = new TaxpayerWithTotal
            {
                Taxpayer = taxpayer,
                Total = total,
            };

            return View(view);
        }



        [Authorize(Roles = "Taxpayer")]
        public ActionResult MyProperties()
        {
            var taxpayer = db.Taxpayers.Where(t => t.UserName == this.User.Identity.Name).FirstOrDefault();

            if (taxpayer != null)
            {
                
                return View(taxpayer.Properties);

            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Taxpayer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MySettings(Taxpayer taxpayer)
        {
            if (ModelState.IsValid)
            {

                db.Entry(taxpayer).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    //TODO: Improve message
                    ModelState.AddModelError(String.Empty, ex.Message);
                    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", taxpayer.DepartmentId);
                    ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", taxpayer.DocumentTypeId);
                    ViewBag.MunicipalityId = new SelectList(db.Municipalities
                        .Where(m => m.DepartmentId == db.Departments.FirstOrDefault().DepartmentId)
                        .OrderBy(m => m.Name), "MunicipalityId", "Name", taxpayer.MunicipalityId);
                    return View(taxpayer);
                }
                return RedirectToAction("Index", "Home");


            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", taxpayer.DepartmentId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", taxpayer.DocumentTypeId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == taxpayer.DepartmentId)
                .OrderBy(m => m.Name), "MunicipalityId", "Name", taxpayer.MunicipalityId);

            return View(taxpayer);

        }

        [Authorize(Roles = "Taxpayer")]
        public ActionResult MySettings()
        {
            var taxpayer = db.Taxpayers.Where(t => t.UserName == this.User.Identity.Name).FirstOrDefault();

            if (taxpayer != null)
            {
                ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", taxpayer.DepartmentId);
                ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", taxpayer.DocumentTypeId);
                ViewBag.MunicipalityId = new SelectList(db.Municipalities
                    .Where(m => m.DepartmentId == taxpayer.DepartmentId)
                    .OrderBy(m => m.Name), "MunicipalityId", "Name", taxpayer.MunicipalityId);

                return View(taxpayer);

            }

            return RedirectToAction("Index", "Home");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProperty(Property view)
        {
            if (ModelState.IsValid)
            {
                db.Properties.Add(view);
                try
                {
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    //TODO: Catch Errors to improve the messages
                    this.ReturnView(view, ex.Message);
                    return View(view);
                }
                return RedirectToAction(String.Format("Details/{0}", view.TaxpayerId));

            }

            this.ReturnView(view, String.Empty);
            return View(view);

        }

        private void ReturnView(Property view, string error)
        {
            if (!String.IsNullOrEmpty(error))
            {
                ModelState.AddModelError(String.Empty, error);
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", view.DepartmentId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == view.DepartmentId)
                .OrderBy(m => m.Name), "MunicipalityId", "Name", view.MunicipalityId);
            ViewBag.DocumentTypeId = new SelectList(db.PropertyTypes, "PropertyTypeId", "Description", view.PropertyTypeId);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AddProperty(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var view = new Property
            {
                TaxpayerId = id.Value,

            };

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == db.Departments.FirstOrDefault().DepartmentId)
                .OrderBy(m => m.Name), "MunicipalityId", "Name");
            ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes, "PropertyTypeId", "Description");

            return View(view);
        }

        // GET: Taxpayers
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var taxpayers = db.Taxpayers.Include(t => t.Department).Include(t => t.DocumentType).Include(t => t.Municipality).Include(p => p.Properties).ToList();
            return View(taxpayers.ToList());
        }

        // GET: Taxpayers/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taxpayer taxpayer = db.Taxpayers.Find(id);

            taxpayer.Properties.OrderBy(p => p.Department.Name).ThenBy(p => p.Municipality.Name).ThenBy(p => p.PropertyType.Description).ThenBy(p => p.Address);
            if (taxpayer == null)
            {
                return HttpNotFound();
            }
            return View(taxpayer);
        }

        // GET: Taxpayers/Create
        [Authorize(Roles = "Admin")]
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
                db.Taxpayers.Add(taxpayer);
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taxpayer taxpayer = db.Taxpayers.Find(id);
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
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taxpayer taxpayer = db.Taxpayers.Find(id);
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
            Taxpayer taxpayer = db.Taxpayers.Find(id);
            db.Taxpayers.Remove(taxpayer);
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
