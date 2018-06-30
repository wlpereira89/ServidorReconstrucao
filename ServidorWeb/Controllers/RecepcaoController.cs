using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ServidorWeb.Models;

namespace ServidorWeb.Controllers
{
    [RoutePrefix("api/recepcao")]
    public class RecepcaoController : ApiController
    {
        [AcceptVerbs("POST")]
        [Route("arquivo")]
        public Task<HttpResponseMessage> ReceberArquivo()
        {
            
            HttpRequestMessage request = Request;

            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            
            string root = System.Web.HttpContext.Current.Server.MapPath("~/Data/"+DateTime.Now.ToString().Replace('/', '-').Replace(':', ' '));
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            MultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(root);
            var task = request.Content.ReadAsMultipartAsync(provider);
            return task.ContinueWith(o =>
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("Arquivo recebido com sucesso!")
                    };
                }
            );
            
        }
    }
}
