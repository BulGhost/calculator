﻿namespace Foxminded.CalculationLibrary.ShuntingYardParser
{
    internal class Operator
    {
        public string Name { get; set; }
        public int Precedence { get; set; }
        public bool RightAssociative { get; set; }
    }
}
