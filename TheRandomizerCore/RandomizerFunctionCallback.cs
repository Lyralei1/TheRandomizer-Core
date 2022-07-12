using Sims3.Gameplay.Actors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lyralei.TheRandomizerCore
{
    public class RandomizerFunctionCallback
    {

        // TEMPLATE:

        // target can be null! So please check that in code if it's heavily target dependant.
        //public static void FunctionNameHere(Sim actor, Sim target)
        //{
        //}


        /// <summary>
        /// Template that your Random event function needs to be like. target can be null! So please check that in code if it's heavily target dependant.
        /// </summary>
        /// <param name="actor">Active sim or sim in active household.</param>
        /// <param name="target">Randomly assigned Target, if the XML has "NeedsInvolvedTarget" set to true.</param>
        public delegate void EventTypeRandomizerFunctionOption(Sim actor, Sim target);

        /// <summary>
        /// Test version of your method. here we check if the event is relevant for the sim/lot
        /// </summary>
        /// <param name="actor">Active sim or sim in active household.</param>
        /// <param name="target">Randomly assigned Target, if the XML has "NeedsInvolvedTarget" set to true.</param>
        public delegate bool EventTypeRandomizerFunctionTest(Sim actor, Sim target);

    }
}
