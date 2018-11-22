using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HGGM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly LiteRepository _db;
        public FilesController(LiteRepository db)
        {
            this._db = db;
        }
        [HttpGet] public IActionResult Avatar([FromRoute]string id)
        {
            var file = _db.FileStorage.FindById(id);
            if (file == null)
            {
                return Redirect("~/images/steamlogin.png");
            }
            using (var stream = file.OpenRead())
            {
                return File(stream, file.MimeType);
            }
        }
    }
}
