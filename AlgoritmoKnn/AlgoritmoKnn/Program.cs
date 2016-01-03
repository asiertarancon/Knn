using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmosMachineLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            //Knn
            var caracteristicasFlores = new List<Caracteristica>()
                {
                    new Caracteristica("sepallength", Tipo.continuo),
                    new Caracteristica("sepalwidth", Tipo.continuo),
                    new Caracteristica("petallength", Tipo.continuo),
                    new Caracteristica("petalwidth", Tipo.continuo),
                    new Caracteristica("Class", Tipo.salida),
                };

            KnnTest("./datos/flores.arff", "./datos/resultado_knn_flores.txt", caracteristicasFlores, new Dato(new List<double>() { 4.1, 4.5, 2.4, 1.2 }));
            KnnTest("./datos/flores.arff", "./datos/resultado_knn_flores.txt", caracteristicasFlores, new Dato(new List<double>() { 4.9, 2.4, 3.3, 1.0 }));

            var caracteristicasGolf = new List<Caracteristica>()
            {
                new Caracteristica("Previsión", Tipo.discreto),
                new Caracteristica("Temperatura", Tipo.continuo),
                new Caracteristica("Humedad", Tipo.continuo),
                new Caracteristica("Viento", Tipo.continuo),
                new Caracteristica("Jugamos", Tipo.salida),
            };

            KnnTest("./datos/prediccion.arff", "./datos/resultado_knn_prediccion.txt", caracteristicasGolf, new Dato(new List<double>() { 0, 86, 85, 0 }));
            KnnTest("./datos/prediccion.arff", "./datos/resultado_knn_prediccion.txt", caracteristicasGolf, new Dato(new List<double>() { 2, 68, 80, 0 }));

            //Kmeans
            KMeansTest("./datos/flores_clustering.arff", "./datos/resultado_kmeans_floresClustering.txt");
            KMeansTest("./datos/baloncesto.arff", "./datos/resultado_kmeans_baloncesto.txt");
            Console.ReadLine();
        }

        private static void KMeansTest(string ficheroEntrada, string ficheroSalida)
        {
            Comun.MuestraYGuarda("\nAlgoritmo Kmeans. Asier Tarancón. Fichero: " + Path.GetFileName(ficheroSalida), ficheroSalida);
            Comun.MuestraYGuarda("Hora: " + DateTime.Now, ficheroSalida);
            var conjuntoDatos = CargaDatosDeFichero(ficheroEntrada);

            Comun.MuestraYGuarda("Datos iniciales:\n", ficheroSalida);
            Comun.MuestraYGuarda("-------------------", ficheroSalida);
            AlgoritmoKMeans.MostrarDatos(conjuntoDatos, 1, true, true, texto => Comun.MuestraYGuarda(texto, ficheroSalida));

            int numClusters = 3;
            Comun.MuestraYGuarda("\nEstablecemos el número de clústers a " + numClusters, ficheroSalida);

            int[] clusters = AlgoritmoKMeans.Clusteriza(conjuntoDatos, numClusters);

            Comun.MuestraYGuarda("\nAgrupamiento Kmeans terminado:\n", ficheroSalida);                       
            AlgoritmoKMeans.MostrarClusterizado(conjuntoDatos, clusters, numClusters, 1, texto=>Comun.MuestraYGuarda(texto, ficheroSalida));            
        }
              
        private static void KnnTest(string ficheroEntrada, string ficheroSalida, List<Caracteristica> caracteristicas, Dato dato)
        {           
            ConjuntoDatos conjuntoDatos = new ConjuntoDatos();
            conjuntoDatos.Caracteristicas = new List<Caracteristica>(caracteristicas);
            conjuntoDatos.Datos = CargaDatosDeFichero(ficheroEntrada);

            AlgoritmoKnn knn = new AlgoritmoKnn();
            knn.ConjuntoDatos = conjuntoDatos;
            knn.K = 3;

            Comun.MuestraYGuarda("\nAlgoritmo Knn. Asier Tarancón. Fichero: " + Path.GetFileName(ficheroSalida), ficheroSalida);
            Comun.MuestraYGuarda("Hora: " + DateTime.Now, ficheroSalida);
            Comun.MuestraYGuarda("Con el vector inicial: " + string.Join(" ", dato.Select(i => i)), ficheroSalida);         
            Dato nuevoAClasificar = dato;
            double clasificadoComo = knn.ClasificaNuevoDato(nuevoAClasificar, new List<int>() { 0, 1, 2, 3, 4 });

            Comun.MuestraYGuarda("El elemento nuevo está clasificado como " + clasificadoComo, ficheroSalida);
        }

        private static List<Dato> CargaDatosDeFichero(string fichero)
        {
            try
            {
                List<Dato> datosCargados = new List<Dato>();
                var lineasDatos = File.ReadAllLines(fichero).ToList();
                while (lineasDatos.FirstOrDefault() != null && lineasDatos.FirstOrDefault().ToLower() != "@data")
                    lineasDatos.RemoveAt(0);
                lineasDatos.RemoveAt(0);//Eliminamos la linea @data

                foreach (var linea in lineasDatos)
                {
                    try
                    {
                        datosCargados.Add(new Dato(linea.Split(',').ToList().Select(d => Convert.ToDouble(d.Replace('.', ','))).ToList()));
                    }
                    catch (Exception)
                    {//Si hay algún problema en la línea que continue a las siguientes
                    }
                    
                }
                return datosCargados;
            }
            catch (Exception ex)
            {
                return new List<Dato>();
            }            
        }

        
    }
}
