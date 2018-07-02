using System;
using System.Collections.Generic;
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
        
        public async static Task Reconstruir (String pasta, double f0, int iteracoes)
        {
            await Reconstruir(pasta, new StreamReader(pasta+"H-1.txt"), new StreamReader(pasta + "g-1.txt"), f0, iteracoes);
        }
        public async static Task Reconstruir(String pasta, StreamReader arquivoH, StreamReader arquivoG, double f0, int iteracoes)
        {
            StreamWriter img = new StreamWriter(pasta + "img.txt");
            try
            {
                var M = Matrix<double>.Build;
                
                Matrix<double> g = DenseMatrix.OfArray(dadosMatrix(arquivoG, 50816, 1));
                var d = dadosMatrix(arquivoH, 50816, 3600);
                Matrix<double> h = DenseMatrix.OfArray(d);
                int tes = 0;
                Matrix<double> f = M.Dense(h.ColumnCount, 1, f0);                
                Matrix<double> r = g.Subtract(h.Multiply(f0));
                Matrix<double> p = h.Transpose().Multiply(r);
                Matrix<double> rnext;
                double alpha;                
                double beta;
                for (int i = 1; i < iteracoes; i++)
                {
                    alpha = Convert.ToDouble(r.TransposeAndMultiply(r).Multiply(p.TransposeAndMultiply(p).Inverse()));
                    f = f + p.Multiply(alpha);
                    rnext = r.Subtract(alpha * h.Multiply(p));
                    beta = Convert.ToDouble(rnext.TransposeAndMultiply(rnext).Multiply(r.TransposeAndMultiply(r).Inverse()));
                    p = h.TransposeAndMultiply(rnext) + beta * p;
                    r = rnext;
                }
                
                for(int i = 0; i<p.RowCount; i++) 
                {
                    Vector<double> lin = p.Row(i);
                    if (i > 1)
                    {
                        await img.WriteAsync("\n");
                    }
                    for (int j = 0; j < lin.Count; j++)
                    {
                        await img.WriteAsync(lin[j].ToString());
                        if (j < lin.Count - 1)
                        {
                            await img.WriteAsync(",");
                        }

                    }
                }
                    
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            finally
            {
                img.Close();
            }
            
        }

        private static double[,] dadosMatrix(StreamReader arquivo, int linhas, int colunas)
        {
            double[,] dadosDouble = new double[linhas, colunas];
            try
            {
                int i = 0;
                String line;
                while ((line = arquivo.ReadLine()) != null || i < linhas)
                {
                    String[] linha = line.Split(',');
                    for (int j = 0; j < linha.Length; j++)
                    {
                        String[] atual = linha[j].Split('e');
                        if (atual.Length > 1)
                        {
                            Double a = Convert.ToDouble(atual[0]) * Math.Pow(10, Convert.ToDouble(atual[1]));
                            dadosDouble[i, j] = Convert.ToDouble(a);
                        }
                    }
                    i++;
                    if (i > 50816)
                    {
                        Console.Write("xD");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return dadosDouble;
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
