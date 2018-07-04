using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Modelo.PN;

namespace ServidorWeb.Controllers
{
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        [AcceptVerbs("GET")]
        [Route("dir")]
        public List<String> DiretoriosContidos()
        {
            List<String> lista = new List<String>();
            DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Data/"));
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
               lista.Add(d.Name);               
            }
            return lista;
        }

        [AcceptVerbs("GET")]
        [Route("{path}/{tipo}")]
        public List<String> ArquivosContidos(String path, String tipo)
        {
            List<String> lista = new List<String>();
            DirectoryInfo dir;
            if (!path.Equals("root"))
                dir = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Data/" + path + "/"));
            else
                dir = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Data/"));

            foreach(FileInfo file in dir.GetFiles())
            {
                if (file.FullName.Split('.').Last() == tipo)
                {
                    lista.Add(file.Name);
                } 
            }
            return lista;
        }
        [AcceptVerbs("GET")]
        [Route("{path}")]
        public List<String> ArquivosContidos(String path)
        {
            List<String> lista = new List<String>();
            DirectoryInfo dir;
            if (!path.Equals("root"))
                dir = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Data/" + path + "/"));
            else
                dir = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Data/"));

            
            foreach (FileInfo file in dir.GetFiles())
            {
                lista.Add(file.Name);                
            }
            return lista;
        }
        [AcceptVerbs("GET")]
        [Route("baixar/{path}/{arquivo}/{extensao}")]
        public string RegistraEnviaArquivo(String path, String arquivo, String extensao)
        {
            PNRegistro.registrar(path + "\\" + arquivo + "." + extensao, DateTime.Now, "");
            var request = HttpContext.Current.Request;
            String f;
            if (path.Equals("root"))
                f = string.Format("{0}://{1}/data/{2}.{3}", request.Url.Scheme, request.Url.Authority, arquivo, extensao);
            else
                f = string.Format("{0}://{1}/data/{2}/{3}.{4}", request.Url.Scheme, request.Url.Authority, path, arquivo, extensao); 
            return f;
        }
    }

}
