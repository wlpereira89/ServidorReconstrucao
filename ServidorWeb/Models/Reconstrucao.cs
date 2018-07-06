using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ServidorWeb.Models
{
    public static class Reconstrucao
    {
       
        static StreamReader arquivoH;
        //static double[,] d = ;
        //static Matrix<double> h;
        //static Matrix<double> ht;

       

        public static string Reconstruir (String pasta, int linhas, int colunas, double ganho, int iteracoes)
        {
            return Reconstruir(pasta, new StreamReader(pasta + "g-1.txt"), linhas, colunas, ganho, iteracoes);
        }
        public static string Reconstruir(String pasta, StreamReader arquivoG, int linhas, int colunas, double ganho, int iteracoes)
        {
            string ret = "";
            DateTime inicio = DateTime.Now;
            try
            {
                Matrix<double> h;
                var V = Vector<double>.Build;                
                Vector<double> r = dadosVector(arquivoG, 50816);
                r = r.Multiply(ganho);
                arquivoH = new StreamReader(HttpContext.Current.Server.MapPath("~/Models/" + "H-3.txt"));                
                h = dadosMatrix(arquivoH, 50816, 3600);
                if (h == null)
                {
                    return "Estouro de memória";
                }
                Vector<double> f = V.Dense(linhas * colunas, 0);
                Vector<double> p = V.Dense(linhas * colunas);
                p = h.TransposeThisAndMultiply(r);
                Vector<double> rnext = V.Dense(50816, 0); 
                double alpha;                
                double beta;
                for (int i = 1; i <= iteracoes; i++)
                {
                    alpha = r * r / p * p;
                    f = f + p.Multiply(alpha);
                    rnext = r.Subtract(alpha * h.Multiply(p));
                    beta = rnext * rnext / (r * r);           
                    p = h.TransposeThisAndMultiply(rnext) + (beta * p);
                    rnext = r.Clone();
                }                
                StreamWriter arquivo = new StreamWriter(pasta + "img_" + iteracoes + ".txt");
                Bitmap imagem = new Bitmap(linhas, colunas);                
                for (int i = 0; i<linhas; i++)
                {
                    String linha = f[i * colunas].ToString();
                    for (int j = 1; j < colunas; j++)
                    {
                        linha += ","+f[j + i * colunas];
                    }
                    arquivo.WriteLine(linha);
                }
            }
            catch (Exception ex)
            {
                return ret + ex.Message;
            }
            finally
            {
                GC.Collect();
            }
            TimeSpan decorido = (DateTime.Now - inicio);
            return  "Tempo decorrido " + Convert.ToInt32(decorido.TotalMinutes) + " minutos " + decorido.Seconds + " segundos. Imagem gerada com sucesso";
        }
        private static double multiplicarVetorPorTransposto(Vector<double> r)
        {
            double calc = 0;
            for (int i = 0; i < r.Count; i++)
            {
                for (int j=0; j< r.Count; j++)
                {
                    calc += r[i] * r[j];
                }
                    
            }
            return calc;
        }
        private static Matrix<double> dadosMatrix(StreamReader arquivo, int linhas, int colunas)
        {
            Matrix<double> ret;
            try
            {

                var M = Matrix<double>.Build;
                ret = M.Dense(linhas, colunas);
                
                int i = 0;
                String line;
                while (((line = arquivo.ReadLine()) != null) && (i < linhas))
                {                    
                    String[] linha = line.Split(',');
                    for (int j = 0; j < linha.Length; j++)
                    {
                        String[] atual = linha[j].Split('e');
                        if (atual.Length > 1)
                        {
                            Double a = Convert.ToDouble(atual[0]) * Math.Pow(10, Convert.ToDouble(atual[1]));
                            ret[i, j] = Convert.ToDouble(a);
                        }
                    }
                    i++;
                }
                return ret;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return null;
        }

        private static Vector<double> dadosVector(StreamReader arquivo, int linhas)
        {
            var V = Vector<double>.Build;
            Vector<double> dadosDouble = V.Dense(linhas);
            try
            {
                int i = 0;
                String line;
                while ((line = arquivo.ReadLine()) != null && (i < linhas))
                {
                    String[] atual = line.Split('e');
                    if (atual.Length > 1)
                    {
                        Double a = Convert.ToDouble(atual[0]) * Math.Pow(10, Convert.ToDouble(atual[1]));
                        dadosDouble[i] = Convert.ToDouble(a);
                    }                
                    i++;
                }
                return dadosDouble;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return null;
        }
    }
}  
    

/*private static double[,] dadosMatrixG(StreamReader arquivo, int linhas, int colunas)
   {
       double[,] dadosDouble = new double[linhas, colunas];
       try
       {
           for (int i = 0; i < linhas; i++)
           {
               for (int j = 0; j < colunas; j++)
               {
                   String[] atual = arquivo.ReadLine().Split('e');
                   var linha = arquivo.ReadLine();
                   if (atual.Length > 1)
                   {
                       Double a = Convert.ToDouble(atual[0]) * Math.Pow(10, Convert.ToDouble(atual[1]));
                       dadosDouble[i, j] = Convert.ToDouble(a);
                   }
                   else
                   {
                       dadosDouble[i, j] = Convert.ToDouble(atual[0]);
                   }
               }
           }
       }
       catch (Exception ex)
       {
           Console.Write(ex);
       }
       return dadosDouble;
   }*/
