using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEPAMIAR_SGC.Models;

namespace SEPAMIAR_SGC.Controllers
{
    public class CampanaMarketingController : Controller
    {
        private _SGCModel db = new _SGCModel();

        // GET: CampanaMarketing
        public async Task<ActionResult> Index()
        {
            return View(await db.campana_marketing.ToListAsync());
        }

        // GET: CampanaMarketing/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            campana_marketing campana_marketing = await db.campana_marketing.FindAsync(id);
            if (campana_marketing == null)
            {
                return HttpNotFound();
            }
            return View(campana_marketing);
        }

        // GET: CampanaMarketing/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CampanaMarketing/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,nombre,activo,dscto,created_at,updated_at,deleted_at")] campana_marketing campana_marketing)
        {
            if (ModelState.IsValid)
            {
                db.campana_marketing.Add(campana_marketing);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(campana_marketing);
        }

        // GET: CampanaMarketing/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            campana_marketing campana_marketing = await db.campana_marketing.FindAsync(id);
            if (campana_marketing == null)
            {
                return HttpNotFound();
            }
            return View(campana_marketing);
        }

        // POST: CampanaMarketing/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,nombre,activo,dscto,created_at,updated_at,deleted_at")] campana_marketing campana_marketing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campana_marketing).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(campana_marketing);
        }

        // GET: CampanaMarketing/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            campana_marketing campana_marketing = await db.campana_marketing.FindAsync(id);
            if (campana_marketing == null)
            {
                return HttpNotFound();
            }
            return View(campana_marketing);
        }

        // POST: CampanaMarketing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            campana_marketing campana_marketing = await db.campana_marketing.FindAsync(id);
            db.campana_marketing.Remove(campana_marketing);
            await db.SaveChangesAsync();
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

		public async Task<ActionResult> GetCampaignInfo(int id)
		{
			campana_marketing c = await db.campana_marketing.Where(m => m.id == id).FirstOrDefaultAsync();

			Dictionary<string, object> json = new Dictionary<string, object>();
			json.Add("dscto", c.dscto);

			return Json(json, JsonRequestBehavior.AllowGet);
		}
    }
}
