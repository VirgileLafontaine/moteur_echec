using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur
{
    public class Exploration
    {
        private const int Pp = 10; //pion passant
        private const int P = 1; //pion
        private const int Tg = 21; //tour gauche (different pour le roque)
        private const int Td = 22; //tour droite
        private const int Cg = 31; //cavalier gauche (différents pour l'image)
        private const int Cd = 32; //cavalier droit
        private const int F = 4; //fou
        private const int D = 5; //dame

        private const int R = 6; //roi

        //poids d'importance des pieces sur le terrain
        public int PoidsPion = 100,
            PoidsCavalier = 320,
            PoidsFou = 330,
            PoidsTour = 500,
            PoidsReine = 900,
            PoidsRoi = 20000;

        public Moves MovesCalculator = new Moves();

        public int Evaluer(Environment env, bool endingParty)
        {
            int score = 0;
            int scorePosition = 0;
            for (int i = 0; i < env.Board.Length; i++)
            {
                switch (env.CurrentPlayer * env.Board[i])
                {
                    case P:
                    case Pp:
                        score += env.CurrentPlayer * PoidsPion;
                        if (env.CurrentPlayer == 1 && env.Board[i] >= 0) scorePosition += MoveEvalTables.Pion[i];
                        if (env.CurrentPlayer == -1 && env.Board[i] <= 0) scorePosition += MoveEvalTables.PionAdv[i];
                        break;
                    case Tg:
                    case Td:
                        score += env.CurrentPlayer * PoidsTour;
                        if (env.CurrentPlayer == 1 && env.Board[i] >= 0) scorePosition += MoveEvalTables.Tour[i];
                        if (env.CurrentPlayer == -1 && env.Board[i] <= 0) scorePosition += MoveEvalTables.TourAdv[i];
                        break;
                    case Cg:
                    case Cd:
                        score += env.CurrentPlayer * PoidsCavalier;
                        if (env.CurrentPlayer == 1 && env.Board[i] >= 0) scorePosition += MoveEvalTables.Cavalier[i];
                        if (env.CurrentPlayer == -1 && env.Board[i] <= 0)
                            scorePosition += MoveEvalTables.CavalierAdv[i];
                        break;
                    case F:
                        score += env.CurrentPlayer * PoidsFou;
                        if (env.CurrentPlayer == 1 && env.Board[i] >= 0) scorePosition += MoveEvalTables.Fou[i];
                        if (env.CurrentPlayer == -1 && env.Board[i] <= 0) scorePosition += MoveEvalTables.FouAdv[i];
                        break;
                    case D:
                        score += env.CurrentPlayer * PoidsReine;
                        if (env.CurrentPlayer == 1 && env.Board[i] >= 0) scorePosition += MoveEvalTables.Reine[i];
                        if (env.CurrentPlayer == -1 && env.Board[i] <= 0) scorePosition += MoveEvalTables.ReineAdv[i];
                        break;
                    case R:
                        score += env.CurrentPlayer * PoidsRoi;
                        if (endingParty)
                        {
                            if (env.CurrentPlayer == 1 && env.Board[i] >= 0) scorePosition += MoveEvalTables.RoiFin[i];
                            if (env.CurrentPlayer == -1 && env.Board[i] <= 0)
                                scorePosition += MoveEvalTables.RoiFinAdv[i];
                        }
                        else
                        {
                            if (env.CurrentPlayer == 1 && env.Board[i] >= 0)
                                scorePosition += MoveEvalTables.RoiDebut[i];
                            if (env.CurrentPlayer == -1 && env.Board[i] <= 0)
                                scorePosition += MoveEvalTables.RoiDebutAdv[i];
                        }
                        break;
                }
            }
            //TO DO mobility (nombre de mouvements safe possibles par piece avec facteur
            //score = score materiel + mobilité
            return score + scorePosition;
        }

        public Environment AlphaBeta(Environment env, int alpha, int beta, int remainingDepth)
        {
            Environment bestScore = new Environment(-999999);
            if (alpha > beta)
            {
                return env;
            }
            if (remainingDepth == 0) return RechercheCalme(alpha, beta, env);
            Queue listEnvironments = MovesCalculator.ProchainsEnvironnements(env, env.CurrentPlayer, false); // false : all moves, not only captures
            foreach (Environment mouvement in listEnvironments)
            {
                Environment val = AlphaBeta(mouvement, -beta, -alpha, remainingDepth - 1);
                val.Score = env.Score * (-1);
                if (val.Score > bestScore.Score)
                {
                    bestScore = val;
                    if (bestScore.Score > alpha)
                    {
                        alpha = bestScore.Score;
                        if (alpha >= beta)
                        {
                            return bestScore;
                        }
                    }
                }
            }
            return bestScore;
        }

        public Environment RechercheCalme(int alpha, int beta, Environment env)
        {
            int standPat = Evaluer(env, false);
            if (standPat >= beta)
            {
                env.Score = beta;
                return env;
            }
            if (alpha < standPat)
            {
                env.Score = standPat;
                alpha = standPat;
            }
            Queue mouvementsCapture = MovesCalculator.ProchainsEnvironnements(env, env.CurrentPlayer, true); // true : only capture moves
            if (mouvementsCapture.Count == 0)
            {
                return env;
            }
            foreach (Environment captures in mouvementsCapture)
            {
                Environment tmp = RechercheCalme(-beta, -alpha, captures);
                tmp.Score = tmp.Score * (-1);
                if (tmp.Score >= beta)
                {
                    tmp.Score = beta;
                    return tmp;
                }
                if (tmp.Score > alpha)
                {
                    alpha = tmp.Score;
                }
                tmp.Score = alpha;
                return tmp;
            }
            return env;
        }
    }
    }
