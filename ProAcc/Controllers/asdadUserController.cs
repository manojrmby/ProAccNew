using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProACC_DB;
using PagedList;
using PagedList.Mvc;
using ProAcc.BL;

namespace ProAcc.Controllers
{

    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin,Consultant")]
    public class asdadUserController : Controller
    {
        private ProAccEntities db = new ProAccEntities();

        // GET: User
        public ActionResult Index()
        {
            Guid AdminUser = Guid.Parse("25FB1790-A743-464E-9E75-0C48A75CF308");
            var User = db.UserMasters.Where(a => a.isActive == true)
                .OrderByDescending(x => x.Cre_on).Where(x => x.UserId != AdminUser).ToList();
                //.Where(x => x.Name.StartsWith(search) || search == null).ToPagedList(i ?? 1, 5);
            //var consultants = db.Consultants.Include(c => c.User_Master).Where(a => a.isActive == true);
            return View(User);
        }

        // GET: Consultants/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMaster consultant = db.UserMasters.Find(id);
            if (consultant == null)
            {
                return HttpNotFound();
            }
            return View(consultant);
        }

       

        // GET: Consultants/Create
        public ActionResult Create()
        {
            var val = db.User_Type.Where(x => x.isActive == true).ToList();
            ViewBag.UserTypeID = new SelectList(val, "Id", "UserType");

            


            return View();
        }

       

        // GET: Consultants/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMaster consultant = db.UserMasters.Find(id);
            if (consultant == null)
            {
                return HttpNotFound();
            }
            var val = db.User_Type.Where(a => a.isActive).Where(a => a.UserTypeID == 1 || a.UserTypeID == 2);
            ViewBag.UserTypeID = new SelectList(val, "Id", "UserType", consultant.UserTypeID);
            var Team = db.RoleMasters.Where(a => a.isActive).Where(a => a.RoleId != 1);
            ViewBag.TeamID = new SelectList(Team, "Id", "TeamName", consultant.RoleID);
            return View(consultant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserMaster con)
        {
            if (ModelState.IsValid)
            {
                con.Modified_On = DateTime.Now;
                //consultant.Cre_on = DateTime.Now;
                con.isActive = true;
                if (con.UserTypeID == 1)
                {
                    con.RoleID = 1;
                }
                db.Entry(con).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserTypeID = new SelectList(db.User_Type, "Id", "UserType", con.UserTypeID);
            ViewBag.TeamID = new SelectList(db.RoleMasters, "Id", "TeamName", con.RoleID);
            return View(con);
        }

        // GET: Consultants/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMaster consultant = db.UserMasters.Find(id);
            if (consultant == null)
            {
                return HttpNotFound();
            }
            return View(consultant);
        }

        // POST: Consultants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UserMaster consultant = db.UserMasters.Find(id);
            if(consultant.UserId==id)
            {
                consultant.isActive = false;
                consultant.IsDeleted = true;
                db.Entry(consultant).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
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
