using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DaGym.Models;

namespace DaGym.Controllers
{
    [Authorize]
    public class GymClassesController : Controller
    {
        private bool indexShowHistory { get; set; }

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GymClasses
        [AllowAnonymous]
        public ActionResult Index_org()
        {           
        return View(db.GymClasses.ToList());
        }

        // GET: GymClasses
        [AllowAnonymous]
        public ActionResult Index()
        {
            DateTime loDate;
            if (indexShowHistory)
            {
                loDate = DateTime.Parse("1901-01-01 00:00:00");
            }
            else
            {
                loDate = DateTime.Now;
            }

            var gymClasses = db.GymClasses
                .Where(s => s.StartTime >= loDate)
                .OrderByDescending(s => s.StartTime)
                .ToList();

            ApplicationUser CurrentUser = db.Users
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();

            List<GymClassView> gymClassList = new List<GymClassView>();

            if (gymClasses.Count() > 0)
            {
                foreach (var item in gymClasses)
                {
                    var gymClassView = new GymClassView();
                    gymClassView.Id = item.Id;
                    gymClassView.Name = item.Name;
                    gymClassView.StartTime = item.StartTime;
                    gymClassView.Duration = item.Duration;
                    gymClassView.Description = item.Description;
                    gymClassView.AttendingMembers = item.AttendingMembers;

                    if (gymClassView.AttendingMembers.Contains(CurrentUser))
                    {
                        gymClassView.Booked = true;
                    }
                    else
                    {
                        gymClassView.Booked = false;
                    }
                    gymClassList.Add(gymClassView);
                }
            }
            return View(gymClassList);
        }








        // GET: GymClasses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymClass gymClass = db.GymClasses.Find(id);
            if (gymClass == null)
            {
                return HttpNotFound();
            }
            return View(gymClass);
        }

        // GET: GymClasses/Create
        [Authorize (Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                db.GymClasses.Add(gymClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymClass gymClass = db.GymClasses.Find(id);
            if (gymClass == null)
            {
                return HttpNotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gymClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymClass gymClass = db.GymClasses.Find(id);
            if (gymClass == null)
            {
                return HttpNotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GymClass gymClass = db.GymClasses.Find(id);
            db.GymClasses.Remove(gymClass);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult BookingToggle(int id)
        {
            GymClass CurrentClass = db.GymClasses.Where(g => g.Id == id).FirstOrDefault();
            ApplicationUser CurrentUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (CurrentClass.AttendingMembers.Contains(CurrentUser))
            {
                CurrentClass.AttendingMembers.Remove(CurrentUser);
                db.SaveChanges();
            }
            else
            {
                CurrentClass.AttendingMembers.Add(CurrentUser);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult IndexToggleHistory(IndexShowHistory)
        {
            indexShowHistory = !indexShowHistory;
            return RedirectToAction("Index", new { });
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
