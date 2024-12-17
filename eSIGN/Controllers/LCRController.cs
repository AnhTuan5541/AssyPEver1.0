using Microsoft.AspNetCore.Mvc;

namespace WaferMapViewer.Controllers
{
    public class LCRController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RequestDetail(string requestID)
        {
            if (!string.IsNullOrEmpty(requestID))
            {
                ViewBag.RequestID = requestID;
                return View();
            }
            else
            {
                return RedirectToAction("NotFound");
            }
        }
        public IActionResult ConfigMap()
        {
            return View();
        }
        public IActionResult NotFound()
        {
            return View();
        }
        
    }
}
