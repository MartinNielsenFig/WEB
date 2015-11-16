﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using Obligatorisk1.Models;
using Obligatorisk1.Viewmodels;

namespace Obligatorisk1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ComponentsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Components
        public ActionResult Index(string category, string search)
        {
            ViewBag.Categories = db.Categories.AsNoTracking().ToList();
            if (category.IsNullOrWhiteSpace() && search.IsNullOrWhiteSpace())
            {
                return View(db.Components.Include(x=>x.Category).Include(x=>x.SpecificComponent).ToList());
            }
            if (!category.IsNullOrWhiteSpace())
            {
                return View(db.Components.Include(x => x.Category).Include(x => x.SpecificComponent).Where(x => x.Category.Value == category).ToList());
            }
            if (!search.IsNullOrWhiteSpace())
            {
                return View(db.Components.Include(x => x.Category).Include(x => x.SpecificComponent).Where(x => x.ComponentName.Contains(search)||x.ComponentInfo.Contains(search)).ToList());
            }
            return HttpNotFound();
        }

        // GET: Components/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return HttpNotFound();
            }
            return View(component);
        }

        // GET: Components/Create
        public ActionResult Create()
        {
            ViewBag.Categories = db.Categories.AsNoTracking().ToList();
            return View();
        }

        // POST: Components/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateComponentViewmodel componentVm)
        {
            
            if (ModelState.IsValid)
            {
                componentVm.Component.SpecificComponent = new List<SpecificComponent>();
                componentVm.Component.SpecificComponent = new JavaScriptSerializer().Deserialize<List<SpecificComponent>>(componentVm.SpecificComponentListAsJson);

                if (componentVm.Image != null)
                {
                    componentVm.Component.ImageMimeType = componentVm.Image.ContentType;
                    componentVm.Component.Image = new byte[componentVm.Image.ContentLength];
                    componentVm.Image.InputStream.Read(componentVm.Component.Image, 0, componentVm.Image.ContentLength);
                }
                db.Components.Add(componentVm.Component);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(componentVm);
        }

        // GET: Components/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Component component = db.Components.Include(x=>x.SpecificComponent).First(x=>x.Id==id);
            if (component == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = db.Categories.AsNoTracking().ToList();
            return View(component);
        }

        // POST: Components/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ComponentName,ComponentInfo,Datasheet,Image,ManufacturerLink,CategoryId")] Component component)
        {

            if (ModelState.IsValid)
            {
                db.Entry(component).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(component);
        }

        // GET: Components/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return HttpNotFound();
            }
            return View(component);
        }

        // POST: Components/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Component component = db.Components.Include(x=>x.SpecificComponent).Include(x=>x.Category).FirstOrDefault(x=>x.Id==id);
            db.Components.Remove(component);
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

        public ActionResult Lend(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Component component = db.Components.Include("SpecificComponent.LoanInformation").First(x => x.Id == id);
            
            if (component == null)
            {
                return HttpNotFound();
            }
            return View(component);
        }
        public FileContentResult GetImage(int componentId)
        {
            Component component = db.Components.FirstOrDefault(x => x.Id == componentId);
            if (component != null)
            {
                return File(component.Image, component.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        public ActionResult EditLoanInformation(int componentId, int specificId, LoanInformation loanInformation)
        {
            if (ModelState.IsValid)
            {
                Component component = db.Components.Include(x => x.SpecificComponent).First(x => x.Id == componentId);
                SpecificComponent spComp = component.SpecificComponent.First(x => x.Id == specificId);
                spComp.LoanInformation = loanInformation;
                db.Entry(component).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Lend", new { id = componentId });
        }
    }
}
