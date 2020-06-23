
using Pmedian.CoreData.DataStruct;
using Pmedian.Model;
using Pmedian.Model.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Windows.Documents;

namespace Pmedian.CoreData.Genetic.Function
{
    /// <summary>
    /// Статический класс проверки на сходимость ГА.
    /// </summary>
    public static class Solution
    {    


        public static int Answer(Cost cost, Chromosome chromosome, ProblemData problemData, MainGraph graph)
        {
            if (chromosome == null || chromosome.fitness == double.MaxValue)
                return 0;
            List<List<int>> answerList = new List<List<int>>();
            List<int> clinicList = new List<int>();
            List<int> medicList = new List<int>();
            List<int> array = new List<int>();
            
            for (int i = 0; i < chromosome.SizeChromosome; i++)
            {
                if (cost.unmarketVertex[i] * chromosome.chromosomeArray[i] != 0) 
                {
                    array.Add(cost.unmarketVertex[i]);                    
                }
            }


            if (array.Count == 1)
                graph.Vertices.ElementAt(array.First()).Color = VertexColor.GroupeClinic;
            else if (array.Count % 2 == 1 && array.Count != 1)
            {
                int i = (array.Count / 2);
                double a = Math.Abs(array[i + 1] - array[i]);
                double b = Math.Abs(array[i - 1] - array[i]);
                if (a > b)
                {
                    graph.Vertices.ElementAt(array[i]).Color = VertexColor.GroupeClinic;
                }
                else
                    graph.Vertices.ElementAt(array[i]).Color = VertexColor.GroupeMedic;
            }
            for (int i = 0, j = array.Count - 1; i < array.Count / 2; i++, j--)
            {
                

                if (graph.Vertices.ElementAt(array[i]).vertexCost < graph.Vertices.ElementAt(array[j]).vertexCost)
                {
                    graph.Vertices.ElementAt(array[i]).Color = VertexColor.GroupeMedic;
                    graph.Vertices.ElementAt(array[j]).Color = VertexColor.GroupeClinic;
                } else
                {
                    graph.Vertices.ElementAt(array[i]).Color = VertexColor.GroupeClinic;
                    graph.Vertices.ElementAt(array[j]).Color = VertexColor.GroupeMedic;
                }

            }
            return 1;       
        }

        public static bool isConverget(Chromosome bestCromosome, Chromosome worstChromosome)
        {
            double diff = Math.Abs(worstChromosome.fitness - bestCromosome.fitness);
            if (diff >= 0 && diff <= 1) return true;
            else return false;
        }

        /// <summary>
        /// Ищет среднюю приспособленность популяции.
        /// </summary>
        /// <param name="population">Популяция.</param>
        /// <returns>Средняя приспособленность.</returns>
        public static double MediumFitnessPopulation(Population population)
        {
            double sum = 0;
            foreach (var ch in population.populationList)
                sum += ch.fitness;

            double MFP = sum / population.SizePopulation;

            return MFP;
        }


        public static double isAnswer(Chromosome chromosome, Cost cost, ProblemData problemData)
        {
            
            double fitness = 0;
            int sumMedian = 0;
            int[][] X = XMultiplicationChromosome(cost.arrayX, chromosome.chromosomeArray);

            for (int i = 0; i < chromosome.SizeChromosome; i++)
            {
                sumMedian += chromosome.chromosomeArray[i];

                for (int j = 0; j < cost.vertexCount; j++)
                {
                    if (cost.costEdgeArray[i][j].EmptyCost)
                        continue;



                    fitness += (
                        (cost.costEdgeArray[i][j].roadKm * problemData.RoadCost) * X[i][j]);
                }
                fitness += cost.costVertexArray[i] * chromosome.chromosomeArray[i];

            }
            

            return fitness;
        }

        public static bool isAnswerTrue(Chromosome chromosome, Cost cost, ProblemData problemData)
        {
            if (chromosome.fitness == double.MaxValue)
                return false;
            double fitness = 0;
            int sumMedian = 0;
            int[][] X = XMultiplicationChromosome(cost.arrayX, chromosome.chromosomeArray);

            for (int i = 0; i < chromosome.SizeChromosome; i++)
            {
                sumMedian += chromosome.chromosomeArray[i];

                for (int j = 0; j < cost.vertexCount; j++)
                {
                    if (cost.costEdgeArray[i][j].EmptyCost)
                        continue;

                    if (!(X[i][j] <= chromosome.chromosomeArray[i]))
                    {
                        return false;
                    }
                    
                    if (cost.costEdgeArray[i][j].timeM > problemData.MedicTime)
                    {
                        return false;
                    }
                    if (cost.costEdgeArray[i][j].timeС > problemData.AmbulanceTime)
                    {
                        return false;
                    }

                    
                }

            }
            if (sumMedian != problemData.P)
            {           
                return false;
            }            

            return true;
        }

        private static int[][] XMultiplicationChromosome(int[][] arrayX, int[] chromosomeArray)
        {

            int[][] array = new int[arrayX.Length][];

            for (int i = 0; i < arrayX.Length; i++)
            {
                array[i] = new int[arrayX[i].Length];
                for (int j = 0; j < arrayX[i].Length; j++)
                {
                    array[i][j] = arrayX[i][j] * chromosomeArray[i];
                }
            }

            return array;
        }
    }
}
