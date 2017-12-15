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
        public static Hashtable hashtbl = new Hashtable();
        private const int Pp = 10; //pion passant
        private const int P = 1; //pion
        private const int Tg = 21; //tour gauche (different pour le roque)
        private const int Td = 22; //tour droite
        private const int Cg = 31; //cavalier gauche (différents pour l'image)
        private const int Cd = 32; //cavalier droit
        private const int F = 4; //fou
        private const int D = 5; //dame
        private const int R = 6; //roi
        private Random rng = new Random();
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
            if (Math.Abs(env.Score) == 999999) return env.Score;
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
        public ArrayList Randomize(Queue mvts)
        {
            ArrayList res = new ArrayList(mvts);
            int n = mvts.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = res[k];
                res[k] = res[n];
                res[n] = value;
            }
            return res;
        }
        public Environment AlphaBeta(Environment env, int alpha, int beta, int remainingDepth)
        {
            if (hashtbl.Contains(env.Board))
            {
                Queue q = new Queue();
                Environment Bscore = (Environment)q.Dequeue();
                int a = (int)q.Dequeue();
                int b = (int)q.Dequeue();
                int rdepth = (int)q.Dequeue();
                if (rdepth < remainingDepth)
                {
                    env.Score = Bscore.Score;
                    alpha = a;
                    beta = b;
                    return env;
                }
            }
            int localAlpha = alpha;
            Environment bestScore = new Environment(-9999999);
            if (alpha > beta)
            {
                return env;
            }
            if (remainingDepth == 0) return RechercheCalme(alpha, beta, env);
            Queue listEnvironments = MovesCalculator.ProchainsEnvironnements(env, env.CurrentPlayer, false); // false : all moves, not only captures
            ArrayList listEnvironmentsRng = Randomize(listEnvironments);
            foreach (Environment mouvement in listEnvironmentsRng)
            {
                Environment val = AlphaBeta(mouvement, -beta, -localAlpha, remainingDepth - 1);
                val.Score = val.Score * (-1);

                if (bestScore.Score < val.Score)
                {
                    bestScore = val;
                }
                if (bestScore.Score >= beta)
                    break;
                if (bestScore.Score > localAlpha)
                    localAlpha = bestScore.Score;
            /*if (val.Score > bestScore.Score)
            {
                bestScore = val;
                if (bestScore.Score > localAlpha)
                {
                    alpha = bestScore.Score;
                    if (localAlpha >= beta)
                    {
                        return bestScore;
                    }
                }
            }*/
        }
            Queue l = new Queue();
            l.Enqueue(bestScore);
            l.Enqueue(alpha);
            l.Enqueue(beta);
            l.Enqueue(remainingDepth);
            if (hashtbl.Contains(bestScore.Board))
            {
                Queue q2 = new Queue();
                q2 = (Queue)hashtbl[bestScore.Board];
                Environment Bscore = (Environment)q2.Dequeue();
                int a = (int)q2.Dequeue();
                int b = (int)q2.Dequeue();
                int depth = (int)q2.Dequeue();
                if (depth > remainingDepth)
                {
                    hashtbl.Remove(bestScore.Board);
                    hashtbl.Add(bestScore.Board, l);
                    return bestScore;
                }
            }else
            {
                hashtbl.Add(bestScore.Board, l);
                return bestScore;
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
                }/*
                tmp.Score = alpha;
                return tmp;*/
            }
            return env;
        }
    }
}
