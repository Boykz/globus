using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EntGlobus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnOffController : ControllerBase
    {




        /// <summary>
        ///                   API - для включение и отключение онлайн поиска
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        public JsonResult OnOffFirst()
        {
            OnOffResult oor = new OnOffResult { Open=true };

            return new JsonResult(oor);
        }
    }

    public class OnOffResult
    {
        public bool Open { get; set; }
    }
}