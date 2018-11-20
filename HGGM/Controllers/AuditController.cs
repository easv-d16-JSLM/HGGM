using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Services;
using Microsoft.AspNetCore.Http;
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
            return View(_auditService.GetAll());
        }
    }
}