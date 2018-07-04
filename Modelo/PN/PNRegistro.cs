using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo.DAO;

namespace Modelo.PN
{
    public static class PNRegistro
    {
        public static bool registrar(String arquivo, DateTime data, String info)
        {
            try
            {
                DadosReconstrucaoEntities db = new DadosReconstrucaoEntities();
                Reconstrucoes novo = new Reconstrucoes
                {
                    Arquivo = arquivo,
                    DataHora = data,
                    Informacoes = info
                };
                db.Reconstrucoes.Add(novo);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
