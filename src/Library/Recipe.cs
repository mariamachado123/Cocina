//-------------------------------------------------------------------------
// <copyright file="Recipe.cs" company="Universidad Católica del Uruguay">
// Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Threading.Timer;

namespace Full_GRASP_And_SOLID
{
    public class Recipe : IRecipeContent // Modificado por DIP
    {
        // Cambiado por OCP
        private IList<BaseStep> steps = new List<BaseStep>();
        public bool Cooked { get;  private set; } = false;
        public bool Empezo = false;
        

        public Product FinalProduct { get; set; }

        // Agregado por Creator
        public void AddStep(Product input, double quantity, Equipment equipment, int time)
        {
            Step step = new Step(input, quantity, equipment, time);
            this.steps.Add(step);
        }

        // Agregado por OCP y Creator
        public void AddStep(string description, int time)
        {
            WaitStep step = new WaitStep(description, time);
            this.steps.Add(step);
        }

        public void RemoveStep(BaseStep step)
        {
            this.steps.Remove(step);
        }

        // Agregado por SRP
        public string GetTextToPrint()
        {
            string result = $"Receta de {this.FinalProduct.Description}:\n";
            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetTextToPrint() + "\n";
            }

            // Agregado por Expert
            result = result + $"Costo de producción: {this.GetProductionCost()}";

            return result;
        }

        // Agregado por Expert
        public double GetProductionCost()
        {
            double result = 0;

            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetStepCost();
            }

            return result;
        }

        public int GetCookTime()
        {
            int counter = 0;
            foreach (var step in steps)
            {
                counter += step.Time;
            }

            return counter;
        }

        public void SetCooked()
        {
            Cooked = true;
        }

        public async void Cook()
        {
            if (Empezo)
            {
                throw new InvalidOperationException("La receta ya empezo a cocinarse");
            }
            int cookTime = GetCookTime();
            await Task.Delay(cookTime);
            Empezo = true;
            Adaptador adaptador = new Adaptador(this);
            adaptador.TimeOut();
            
        }
    }
}