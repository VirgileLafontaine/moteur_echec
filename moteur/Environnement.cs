using System;
using System.Collections;
using System.Collections.Generic;

namespace Moteur
{
    public class Environment
    {
        public const int White = 1;
        public const int Black = -1;

        //--------------variables de l'environnement-------------//
        public Environment PreviousEnvironment;
        public int CurrentPlayer;
        public int[] Board;
        public string[] Mvt;
        public int Score;

        public Environment(int joueurActuel, int[] board, Environment previous,string[] mvt)
        {
            CurrentPlayer = joueurActuel;
            Board = (int[]) board.Clone();
            PreviousEnvironment = previous;
            Mvt = (string []) mvt.Clone();
        }

        public Environment(int score)
        {
            Score = score;
        }

        public Environment()
        {
            
        }
    }
}

