using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SanjeshP.RDC.Web.Areas.Test.Controllers
{
    [Area("Test")]
    [AllowAnonymous]
    public class TestCController : Controller
    {
        // GET: TestCController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TestCController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TestCController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestCController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TestCController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TestCController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TestCController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TestCController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
