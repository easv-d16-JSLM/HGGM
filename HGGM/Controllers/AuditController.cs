using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HGGM.Models;
using HGGM.Services;
using HGGM.Services.Authorization;
using HGGM.Services.Authorization.Simple;
using HGGM.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HGGM.Controllers
{
    [Permission(SimplePermissionType.GetAuditLog)]
    public class AuditController : Controller
    {
        private readonly AuditService _auditService;

        public AuditController(AuditService auditService)
        {
            _auditService = auditService;
        }

        public IActionResult Details(Guid id)
        {
            var entry = _auditService.Get(id);
            var props = new Dictionary<string, string>();
            foreach (var propertyInfo in entry.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var name = propertyInfo.Name;
                if (propertyInfo.CanRead == false || name == nameof(AuditEntryBase.Id) ||
                    name == nameof(AuditEntryBase.Message) || name == nameof(AuditEntryBase.Time)) continue;
                var value = propertyInfo.GetValue(entry);
                if (value is string s)
                    props.Add(name, s);
                else
                    props.Add(name, JsonConvert.SerializeObject(value));
            }

            return View(new AuditDetailsViewModel
                {Id = entry.Id, Time = entry.Time, Properties = props, message = entry.Message});
        }

        // GET: Audit
        public IActionResult Index()
        {
            return View(_auditService.GetAll().OrderByDescending(a => a.Time));
        }
    }
}