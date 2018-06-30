using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ServidorWeb.Models
{
    public static class Reconstrucao
    {
        
        public static void reconstruir (string h, string g)
        {
            reconstruir(new StreamReader(h), new StreamReader(g));
        }
        public static void reconstruir(StreamReader arquivoH, StreamReader arquivoG)
        {

        }
    }
}