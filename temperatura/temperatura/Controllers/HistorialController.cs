using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace temperatura.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HistorialController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get ()
        {
            List<ViewModels.HistorialViewModel> lst = new List<ViewModels.HistorialViewModel>();
            using (Models.ReportajeEntities1 db = new Models.ReportajeEntities1())
            {
                lst = (from d in db.Historiales
                       select new ViewModels.HistorialViewModel
                       {
                           CiudadNombre = d.nombreciudad,
                           TipoInfo = d.tipoinfo
                       }).ToList();
            }

            return Ok(lst);
        }
    }
}
