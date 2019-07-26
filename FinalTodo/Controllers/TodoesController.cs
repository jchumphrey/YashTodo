using FinalTodo.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace FinalTodo.Controllers
{
    public class TodoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Todoes
        public ActionResult Index()
        {

            return View();
        }

        private IEnumerable<Todo> GetMyTodoes()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault
                (x => x.Id == currentUserId);
            IEnumerable<Todo> myTodoes = db.Todos.ToList().Where(x => x.User == currentUser);

            int CompletecCount = 0;
            foreach (Todo todo in myTodoes)
            {
                if (todo.IsDone)
                {
                    CompletecCount++;
                }
                TimeSpan TimeUntil = todo.DueDate.Subtract(DateTime.Now);
                string Formatted = "";
                if (todo.IsDone)
                {
                    Formatted = "Complete";
                }
                else if (TimeUntil.Days < 0)
                {
                    Formatted = (TimeUntil.Days * -1) + " Days Overdue";
                }
                else if (TimeUntil.Days > 0)
                {
                    Formatted = TimeUntil.Days + " Days until Due";
                }

                else
                {
                    Formatted = "Due Today";
                }
                todo.TimeUntil = Formatted;
            }
            ViewBag.Percent = Math.Round(100f * ((float)CompletecCount / (float)myTodoes.Count()));
            
            return myTodoes;
        }

        public ActionResult BuildTodoTable()
        {
            return PartialView("_TodoTable", GetMyTodoes());
        }

        // GET: Todoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todos.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            return View(todo);
        }

        // GET: Todoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Todoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IsDone,Description,DueDate")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault
                    (x => x.Id == currentUserId);
                todo.User = currentUser;

                TimeSpan TimeUntil = todo.DueDate.Subtract(DateTime.Now);
                string Formatted = "";
                if (TimeUntil.Days < 0)
                {
                    Formatted = (TimeUntil.Days * -1) + " Days Overdue";
                }
                else if (TimeUntil.Days > 0)
                {
                    Formatted = TimeUntil.Days + " Days until Due";
                }
                else
                {
                    Formatted = "Due Today";
                }
                todo.TimeUntil = Formatted;

                db.Todos.Add(todo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(todo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCreate([Bind(Include = "Id, Description,DueDate")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault
                    (x => x.Id == currentUserId);
                todo.User = currentUser;

                TimeSpan TimeUntil = todo.DueDate.Subtract(DateTime.Now);
                string Formatted = "";
                if (TimeUntil.Days < 0)
                {
                    Formatted = (TimeUntil.Days * -1) + " Days Overdue";
                }
                else if (TimeUntil.Days > 0)
                {
                    Formatted = TimeUntil.Days + " Days until Due";
                }
                else
                {
                    Formatted = "Due Today";
                }
                todo.TimeUntil = Formatted;

                db.Todos.Add(todo);
                db.SaveChanges();
            }

            return PartialView("_TodoTable", GetMyTodoes());
        }

        // GET: Todoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todos.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault
                (x => x.Id == currentUserId);
            todo.User = currentUser;

            if (todo.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(todo);
        }

        // POST: Todoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IsDone,Description,DueDate")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(todo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(todo);
        }

        [HttpPost]
        public ActionResult AJAXEdit(int? id, bool value)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todos.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            else
            {
                todo.IsDone = value;
                db.Entry(todo).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_TodoTable", GetMyTodoes());
            }
        }

        // GET: Todoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todos.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            return View(todo);
        }

        // POST: Todoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Todo todo = db.Todos.Find(id);
            db.Todos.Remove(todo);
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
