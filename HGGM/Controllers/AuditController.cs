using System.Linq;
using HGGM.Services;
using Microsoft.AspNetCore.Mvc;

namespace HGGM.Controllers
{
    public class AuditController : Controller
    {
        private readonly AuditService _auditService;

        public AuditController(AuditService auditService)
        {
            _auditService = auditService;
        }

        // GET: Audit
        public ActionResult Index()
        {
            return View(_auditService.GetAll().OrderByDescending(a => a.Time));
        }
    }
}