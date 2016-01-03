using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmosMachineLearning
{    
    class AlgoritmoKnn
    {
        /// <summary>
        /// Conjunto de datos de entrenamiento en el que se basará el algoritmo
        /// </summary>
        public ConjuntoDatos ConjuntoDatos { get; set; }
        /// <summary>
        /// Número de vecinos más cercanos que comprobará el algoritmo
        /// </summary>
        public int K { get; set; }
        /// <summary>
        /// Dado una instancia de datos nueva encuentra los k vecinos más cercanos        
        /// </summary>
        /// <param name="dato">Dato a clasificar</param>
        /// <param name="indices">Índices de los datos de entrenamiento que utilizar</param>
        /// <param name="k">Número de vecinos más cercanos a utilizar</param>
        /// <returns>Lista de parejas clave-valor con la media y los datos</returns>
        public List<KeyValuePair<double, Dato>> EncuentraKVecinosMasCercanos(Dato dato, List<int> indices, int k)
        {
            var vecinos = new List<KeyValuePair<double, Dato>>();
            foreach (Dato entrenamiento in ConjuntoDatos.Datos)
            {
                if (entrenamiento == dato) continue;
                double distancia = CalculaDistancia(dato, entrenamiento, indices);
                vecinos.Add(new KeyValuePair<double, Dato>(distancia, entrenamiento));
            }
            return vecinos.OrderBy(n => n.Key).Take(k).ToList();
        }

        /// <summary>
        /// Clasifica el nuevo dato en función de los datos de entrenamiento 
        /// </summary>
        /// <param name="nuevoAClasificar"></param>
        /// <returns></returns>
        public double ClasificaNuevoDato(Dato nuevoAClasificar, List<int> indices)
        {
            var dic = EncuentraKVecinosMasCercanos(nuevoAClasificar, indices, K);

            var indiceSalida = ConjuntoDatos.Caracteristicas.IndexOf(ConjuntoDatos.Caracteristicas.First(s => s.Tipo == Tipo.salida));

            var moda = dic.Select(a => a.Value[indiceSalida])
                          .GroupBy(n => n)
                          .OrderByDescending(g => g.Count())
                          .Select(g => g.Key).FirstOrDefault();
            return moda;
        }

        /// <summary>
        /// Calcula la distancia Euclídea dadas dos instancias de datos completas, usando las características según los índices pasados
        /// </summary>
        /// <param name="indices">Indices de las características a usar</param>
        /// <param name="dato">El dato a clasificar</param>
        /// <param name="entrenamiento">Uno de los datos de entrenamiento</parm>
        /// <returns>Double</returns>
        private double CalculaDistancia(Dato dato, Dato entrenamiento, List<int> indices)
        {
            double d = 0;
            foreach (int i in indices)
            {
                switch (ConjuntoDatos.Caracteristicas[i].Tipo)
                {
                    case Tipo.continuo:
                        d += Comun.CalculaDistanciaEuclidea(dato[i], entrenamiento[i]);
                        break;
                    case Tipo.discreto:
                        d += (dato[i] == entrenamiento[i]) ? 0 : 1;
                        break;
                    case Tipo.salida:
                    default:
                        break;
                }
            }
            return Math.Sqrt(d);
        }        
    }
}
