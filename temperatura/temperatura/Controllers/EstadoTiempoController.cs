using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;
using System.Web.Script.Serialization;
using temperatura.Models;

namespace temperatura.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class EstadoTiempoController : ApiController
    {
        public async Task<object> CallApiClima (string city)
        {
            String apikey = "f13d2045ac13671fc6a48db2fd2b5bbe";

            UriBuilder builder = new UriBuilder("api.openweathermap.org/data/2.5/weather");
            builder.Query = "q="+city+ "&appid="+apikey;

            //Create a query
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(builder.Uri);
      
            string result = await response.Content.ReadAsStringAsync();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic item = serializer.Deserialize<object>(result);
            
            //close out the client
            client.Dispose();
            return (item);
        }

        public async Task<object> CallApiNews(string city)
        {
            String apikey = "de6fefa781854d89bff52d41eb80d8d1";

            UriBuilder builder = new UriBuilder("https://newsapi.org/v2/everything");
            builder.Query = "apiKey=" + apikey + "&q=" + city;

            //Create a query
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(builder.Uri);

            string result = await response.Content.ReadAsStringAsync();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic item = serializer.Deserialize<object>(result);

            //close out the client
            client.Dispose();
            return(item["articles"][0]);

        }


        [HttpGet]

        public async Task<IHttpActionResult> Get(string city)
        {
            var resultClima = await CallApiClima(city);
            object resultNews = await CallApiNews(city);
            var EstadoNoticiasClima = new
            {
                article = resultNews,
                weather = resultClima,
            };
            using (Models.ReportajeEntities1 db = new Models.ReportajeEntities1())
            {
                var Historial = db.Set<Historiales>();
                var count = (from d in db.Historiales
                             select d).Count();
                Historial.Add(new Historiales { id = count+1, nombreciudad = city, tipoinfo = "info" });
                db.SaveChanges();
            }
            return Ok(EstadoNoticiasClima); 

        }
    }
}
