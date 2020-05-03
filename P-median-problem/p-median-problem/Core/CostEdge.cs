﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p_median_problem.Core
{
    /// <summary>
    /// Класс, реализующий расходы, задаваемый временем скорой помощи и временем фельдшеров.
    /// </summary>
    class CostEdge
    {
        /// <summary>
        /// Время скорой помощи.
        /// </summary>
        public double timeAmbulance;

        /// <summary>
        /// Время фельдшеров
        /// </summary>
        public double timeMedic;

        public double roadCost;

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="timeAmbulance"></param>
        /// <param name="timeMedic"></param>
        public CostEdge(double weidthRoad, double timeAmbulance, double timeMedic)
        {
            this.roadCost = weidthRoad;
            this.timeAmbulance = timeAmbulance;
            this.timeMedic = timeMedic;
        }

        public CostEdge() { }
    }
}
