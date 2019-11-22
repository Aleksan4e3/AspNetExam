using Exam.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Exam.Controllers
{
    public class CategoriesController : Controller
    {
        private ExamContext db = new ExamContext();

        // GET: Categories
        public async Task<ActionResult> Index()
        {
            var categories = db.Categories.Include(c => c.Parent);
            return View(await categories.ToListAsync());
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,ParentId")] Category category)
        {
            Category categoryFind = await db.Categories.FindAsync(category.Id);
            if (categoryFind != null)
            {
                ModelState.AddModelError("Id", "Category with the same Id already exists!");
                category.Id = 0;
            }
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ParentId = new SelectList(db.Categories, "Id", "Name", category.ParentId);
            return View("Edit", category);
        }

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            Category category;
            if (id == null)
            {
                category = new Category();
            }
            else
            {
                category = await db.Categories.FindAsync(id);
                if (category == null)
                {
                    return HttpNotFound();
                }
            }

            ViewBag.ParentId = new SelectList(db.Categories, "Id", "Name", category.ParentId);

            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,ParentId")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ParentId = new SelectList(db.Categories, "Id", "Name", category.ParentId);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            DeleteSubCategories(category);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [NonAction]
        private void DeleteSubCategories(Category category)
        {
            if (category.SubCategories.Count > 0)
            {
                for (int i = category.SubCategories.Count - 1; i >= 0; i--)
                {
                    DeleteSubCategories(category.SubCategories[i]);
                }
                db.Categories.Remove(category);
            }
            else
            {
                db.Categories.Remove(category);
            }
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
