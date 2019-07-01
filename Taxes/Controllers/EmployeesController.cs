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
    public class EmployeesController : Controller
    {
        private TaxesContext db = new TaxesContext();

        // GET: Employees
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Boss).Include(e => e.Department).Include(e => e.DocumentType).Include(e => e.Municipality);
            return View(employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.BossId = new SelectList(db.Employees, "EmployeeId", "FullName");
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description");
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == db.Departments.FirstOrDefault().DepartmentId)
                .OrderBy(m => m.Name), "MunicipalityId", "Name");
            return View();
        }

        // POST: Employees/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeId,FirstName,LastName,UserName,Phone,DepartmentId,MunicipalityId,Address,DocumentTypeId,Document,BossId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BossId = new SelectList(db.Employees, "EmployeeId", "FullName", employee.BossId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", employee.DepartmentId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", employee.DocumentTypeId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == db.Departments.FirstOrDefault().DepartmentId)
                .OrderBy(m => m.Name), "MunicipalityId", "Name");
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.BossId = new SelectList(db.Employees, "EmployeeId", "FullName", employee.BossId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", employee.DepartmentId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", employee.DocumentTypeId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == db.Departments.FirstOrDefault().DepartmentId)
                .OrderBy(m => m.Name), "MunicipalityId", "Name");
            return View(employee);
        }

        // POST: Employees/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,FirstName,LastName,UserName,Phone,DepartmentId,MunicipalityId,Address,DocumentTypeId,Document,BossId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BossId = new SelectList(db.Employees, "EmployeeId", "FullName", employee.BossId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", employee.DepartmentId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes, "DocumentTypeId", "Description", employee.DocumentTypeId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == db.Departments.FirstOrDefault().DepartmentId)
                .OrderBy(m => m.Name), "MunicipalityId", "Name");
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
