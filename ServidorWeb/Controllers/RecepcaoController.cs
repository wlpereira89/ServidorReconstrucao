using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using NUnrar.Archive;
using NUnrar.Common;
using ServidorWeb.Models;

namespace ServidorWeb.Controllers
{
    [RoutePrefix("api/recepcao")]
    public class RecepcaoController : ApiController
    {
        [AcceptVerbs("GET")]
        [Route("arquivo/{nome}")]
        public string UnRar(string nome)
        {
            string root = System.Web.HttpContext.Current.Server.MapPath("~/Data/" + nome + "/"); //.Replace(':', ' ') + DateTime.Now.ToShortDateString().Replace('/', '-')
            try
            {
                RarArchive.WriteToDirectory(root + "Imagem-A.part001.rar", root, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
            }
            catch(FileNotFoundException ex)
            {
                
            }
            return nome + " Recebimento feito com sucesso";
        }
        
        [AcceptVerbs("POST")]
        [Route("arquivo/{nome}")]
        public async Task<HttpResponseMessage> ReceberArquivoAsync(string nome)
        {
            
            HttpRequestMessage request = Request;

            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/Data/" + nome +"/"); //.Replace(':', ' ') + DateTime.Now.ToShortDateString().Replace('/', '-')
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            MultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(root); 
            var task = request.Content.ReadAsMultipartAsync(provider);
            return await task.ContinueWith(o =>
                {
                    if (o.IsFaulted || o.IsCanceled)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, o.Exception);
                    } 
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("Arquivo recebido com sucesso!")
                    };
                }
            );
            
        }
    }
}
