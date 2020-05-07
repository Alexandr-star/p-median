using Pmedian.CoreData.DataStruct;
using Pmedian.CoreData.Genetic.Сrossover;
using Pmedian.Model;
using Pmedian.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic.Algorithm
{
    class GenitorGA : AbstractGeneticAlgorithm
    {
        /// <summary>
        /// Граф, задаваемый списком смежности вершин.
        /// </summary>
        private AdjacencyList adjacencyList;


        /// <summary>
        /// Вариант кроссовера. 
        /// </summary>
        private CrossoverMethod crossoverMethod;

        /// <summary>
        /// Вероятность кроссовера.
        /// </summary>
        private double CrossoverProbability;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="IterationSize">Количество итераций генетического алгоритма.</param>
        public GenitorGA(int IterationSize, int PopulationSize, CrossoverMethod crossoverMethod, double CrossoverProbability) : base(IterationSize, PopulationSize)
        {
            this.crossoverMethod = crossoverMethod;
            this.CrossoverProbability = CrossoverProbability;
        }

        /// <summary>
        /// Реализация генетического алгоритма "Genitoe".
        /// </summary>
        /// <param name="graph">Граф.</param>
        public override void GeneticAlgorithm(MainGraph graph)
        {
            //Инициализация основных структур.
            adjacencyList = AdjacencyList.GenerateList(graph);
            Chromosome[] startPopulation = new Population(PopulationSize, adjacencyList).populationArray;
            var crossover = ChosenCrossoveMethod();
        }

        private AbstractCrossover ChosenCrossoveMethod()
        {
            AbstractCrossover crossover = null;
            switch (crossoverMethod)
            {
                case CrossoverMethod.OneDot:
                    crossover = new OneDotCrossover(CrossoverProbability);
                    break;
                case CrossoverMethod.TwoDot:
                    break;
            }

            return crossover;
        }
    }
}
