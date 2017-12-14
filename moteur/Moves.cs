using System;
using System.Collections;
using System.Collections.Generic;

namespace Moteur
{
    public class Moves
    {
        private int[] _attackBoard = new int[64];
        private ArrayList _toAttack = new ArrayList(); 

        //Coordonnées des cases
        string[] _tabCoord = new string[]
        {
            "a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8",
            "a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7",
            "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6",
            "a5", "b5", "c5", "d5", "e5", "f5", "g5", "h5",
            "a4", "b4", "c4", "d4", "e4", "f4", "g4", "h4",
            "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3",
            "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2",
            "a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1"
        };

        // c/c de Echiquier.cs
        private const int Pp = 10; //pion passant
        private const int P = 1; //pion
        private const int Tg = 21; //tour gauche (different pour le roque)
        private const int Td = 22; //tour droite
        private const int Cg = 31; //cavalier gauche (différents pour l'image)
        private const int Cd = 32; //cavalier droit
        private const int F = 4; //fou
        private const int D = 5; //dame
        private const int R = 6; //roi

        private ArrayList mvt_pion(int[] currentBoard, int pos, int signe, bool attackOnly)
        {
            ArrayList res = new ArrayList();
            // Mouvement de 1 si la case est libre
            if (!attackOnly && pos + signe * (-8) < 64 && pos + signe * (-8) >= 0 && currentBoard[pos + signe * (-8)] == 0)
                res.Add(pos + signe * (-8));

            // Si premier mouvement : déplacement de 2 possible
            if (!attackOnly && ((currentBoard[pos] == P && pos <= 55 && pos >= 48 && currentBoard[pos-8] == 0 && currentBoard[pos-16] == 0) ||
                (currentBoard[pos] == -P && pos <= 15 && pos >= 8 && currentBoard[pos+8] == 0 && currentBoard[pos+16] == 0))) res.Add(pos + signe * (-16));

            // Attaque en diagonale
            if (pos + signe * (-9) < 64 && pos + signe * (-9) >= 0 && pos%8 !=0 && signe * currentBoard[pos + signe * (-9)] < 0)
                res.Add(pos + signe * (-9));
            if (pos + signe * (-7) < 64 && pos + signe * (-7) >= 0 && (pos+1)%8 !=0 && signe * currentBoard[pos + signe * (-7)] < 0)
                res.Add(pos + signe * (-7));

            return res;
        }

        private ArrayList mvt_tour(int[] currentBoard, int pos, int signe, bool attackOnly)
        {
            ArrayList res = new ArrayList();
            // Attaque colonne vers le haut
            for (int j = pos; j >= 0 || j == pos; j -= 8)
            {
                // Case vide
                if (!attackOnly && signe * currentBoard[j] == 0) res.Add(j);
                // Case avec un ennemi
                if (signe * currentBoard[j] < 0) res.Add(j);
                // Case amie
                if ((signe * currentBoard[j] > 0 && j != pos) || signe * currentBoard[j] < 0) break;
            }
            // Attaque colonne vers le bas
            for (int j = pos; j < 64 || j == pos; j += 8)
            {
                // Case vide
                if (!attackOnly && signe * currentBoard[j] == 0) res.Add(j);
                // Case avec un ennemi
                if (signe * currentBoard[j] < 0) res.Add(j);
                // Case amie
                if ((signe * currentBoard[j] > 0 && j != pos) || signe * currentBoard[j] < 0) break;
            }
            // Attaque ligne vers la gauche
            for (int j = pos; (j+1) % 8 != 0 || j == pos; j -= 1)
            {
                // Case vide
                if (!attackOnly && signe * currentBoard[j] == 0) res.Add(j);
                // Case avec un ennemi
                if (signe * currentBoard[j] < 0) res.Add(j);
                // Case amie
                if ((signe * currentBoard[j] > 0 && j != pos) || signe * currentBoard[j] < 0) break;
            }
            // Attaque ligne vers la droite
            for (int j = pos; j% 8 != 0 || j == pos; j += 1)
            {
                // Case vide
                if (!attackOnly && signe * currentBoard[j] == 0) res.Add(j);
                // Case avec un ennemi
                if (signe * currentBoard[j] < 0) res.Add(j);
                // Case amie
                if ((signe * currentBoard[j] > 0 && j != pos) || signe * currentBoard[j] < 0) break;
            }

            return res;
        }

        private ArrayList mvt_cavalier(int[] currentBoard, int pos, int signe, bool attackOnly)
        {
            ArrayList res = new ArrayList();
            if (!attackOnly)
            {
                if (pos - 17 > 0 && pos % 8 != 0 && signe * currentBoard[pos - 17] == 0) res.Add(pos - 17);
                if (pos - 15 > 0 && (pos + 1) % 8 != 0 && signe * currentBoard[pos - 15] == 0) res.Add(pos - 15);
                if (pos - 10 > 0 && (pos) % 8 != 0 && (pos - 1) % 8 != 0 && signe * currentBoard[pos - 10] == 0)
                    res.Add(pos - 10);
                if (pos - 6 > 0 && (pos + 1) % 8 != 0 && (pos + 2) % 8 != 0 && signe * currentBoard[pos - 6] == 0)
                    res.Add(pos - 6);
                if (pos + 6 < 64 && (pos) % 8 != 0 && (pos - 1) % 8 != 0 && signe * currentBoard[pos + 6] == 0)
                    res.Add(pos + 6);
                if (pos + 10 < 64 && (pos + 1) % 8 != 0 && (pos + 2) % 8 != 0 && signe * currentBoard[pos + 10] == 0)
                    res.Add(pos + 10);
                if (pos + 15 < 64 && pos % 8 != 0 && signe * currentBoard[pos + 15] == 0) res.Add(pos + 15);
                if (pos + 17 < 64 && (pos + 1) % 8 != 0 && signe * currentBoard[pos + 17] == 0) res.Add(pos + 17);
            }
            if (pos - 17 > 0 && pos % 8 != 0 && signe * currentBoard[pos - 17] < 0) res.Add(pos - 17);
            if (pos - 15 > 0 && (pos + 1) % 8 != 0 && signe * currentBoard[pos - 15] < 0) res.Add(pos - 15);
            if (pos - 10 > 0 && (pos) % 8 != 0 && (pos - 1) % 8 != 0 && signe * currentBoard[pos - 10] < 0)
                res.Add(pos - 10);
            if (pos - 6 > 0 && (pos + 1) % 8 != 0 && (pos + 2) % 8 != 0 && signe * currentBoard[pos - 6] < 0)
                res.Add(pos - 6);
            if (pos + 6 < 64 && (pos) % 8 != 0 && (pos - 1) % 8 != 0 && signe * currentBoard[pos + 6] < 0)
                res.Add(pos + 6);
            if (pos + 10 < 64 && (pos + 1) % 8 != 0 && (pos + 2) % 8 != 0 && signe * currentBoard[pos + 10] < 0)
                res.Add(pos + 10);
            if (pos + 15 < 64 && pos % 8 != 0 && signe * currentBoard[pos + 15] < 0) res.Add(pos + 15);
            if (pos + 17 < 64 && (pos + 1) % 8 != 0 && signe * currentBoard[pos + 17] < 0) res.Add(pos + 17);

            return res;
        }

        private ArrayList mvt_fou(int[] currentBoard, int pos, int signe, bool attackOnly)
        {
            //Console.WriteLine("Fou"+pos);
            ArrayList res = new ArrayList();
            // Diagonale haut gauche
            for (int j = pos; (j >= 0 && (j+1) % 8 != 0) || j == pos; j -= 9)
            {
                // Case vide
                if (!attackOnly && signe * currentBoard[j] == 0) res.Add(j);
                // Case avec un ennemi
                if (signe * currentBoard[j] < 0) res.Add(j);
                // Case amie
                if ((signe * currentBoard[j] > 0 && j != pos) || signe * currentBoard[j] < 0) break;
            }
            // Diagonale haut droite
            for (int j = pos; j >= 0 && j % 8 != 0 || j == pos; j -= 7)
            {
                // Case vide
                if (!attackOnly && signe * currentBoard[j] == 0) res.Add(j);
                // Case avec un ennemi
                if (signe * currentBoard[j] < 0) res.Add(j);
                // Case amie
                if ((signe * currentBoard[j] > 0 && j != pos) || signe * currentBoard[j] < 0) break;
            }
            // Diagonale bas gauche
            for (int j = pos; j < 64 && (j+1) % 8 != 0 || j == pos; j += 7)
            {
                // Case vide
                if (!attackOnly && signe * currentBoard[j] == 0) res.Add(j);
                // Case avec un ennemi
                if (signe * currentBoard[j] < 0) res.Add(j);
                // Case amie
                if ((signe * currentBoard[j] > 0 && j != pos) || signe * currentBoard[j] < 0) break;
            }
            // Diagonale bas droite
            for (int j = pos; j < 64 && j % 8 != 0 || j == pos; j += 9)
            {
                // Case vide
                if (!attackOnly && signe * currentBoard[j] == 0) res.Add(j);
                // Case avec un ennemi
                if (signe * currentBoard[j] < 0) res.Add(j);
                // Case amie
                if ((signe * currentBoard[j] > 0 && j != pos) || signe * currentBoard[j] < 0) break;
            }

            return res;
        }

        private ArrayList mvt_roi(int[] currentBoard, int pos, int signe, bool attackOnly)
        {
            ArrayList res = new ArrayList();
            if (!attackOnly)
            {
                if (pos - 9 > 0 && pos % 8 != 0 && signe*currentBoard[pos - 9] == 0 && _attackBoard[pos - 9] == 0)
                    res.Add(pos - 9);
                if (pos - 8 > 0 && signe*currentBoard[pos - 8] == 0 && _attackBoard[pos - 8] == 0) res.Add(pos - 8);
                if (pos - 7 > 0 && (pos + 1) % 8 != 0 && signe*currentBoard[pos - 7] == 0 && _attackBoard[pos - 7] == 0)
                    res.Add(pos - 7);
                if (pos - 1 > 0 && pos % 8 != 0 && signe*currentBoard[pos - 1] == 0 && _attackBoard[pos - 1] == 0)
                    res.Add(pos - 1);
                if (pos + 1 < 64 && (pos + 1) != 0 && signe*currentBoard[pos + 1] == 0 && _attackBoard[pos + 1] == 0)
                    res.Add(pos + 1);
                if (pos + 7 < 64 && pos % 8 != 0 && signe*currentBoard[pos + 7] == 0 && _attackBoard[pos + 7] == 0)
                    res.Add(pos + 7);
                if (pos + 8 < 64 && signe*currentBoard[pos + 8] == 0 && _attackBoard[pos + 8] == 0) res.Add(pos + 8);
                if (pos + 9 < 64 && (pos + 1) % 8 != 0 && signe*currentBoard[pos + 9] == 0 && _attackBoard[pos + 9] == 0)
                    res.Add(pos + 9);
            }
            
            if (pos - 9 > 0 && pos % 8 != 0 && signe*currentBoard[pos - 9] < 0 && _attackBoard[pos - 9] == 0)
                res.Add(pos - 9);
            if (pos - 8 > 0 && signe*currentBoard[pos - 8] < 0 && _attackBoard[pos - 8] == 0) res.Add(pos - 8);
            if (pos - 7 > 0 && (pos + 1) % 8 != 0 && signe*currentBoard[pos - 7] < 0 && _attackBoard[pos - 7] == 0)
                res.Add(pos - 7);
            if (pos - 1 > 0 && pos % 8 != 0 && signe*currentBoard[pos - 1] < 0 && _attackBoard[pos - 1] == 0)
                res.Add(pos - 1);
            if (pos + 1 < 64 && (pos + 1) != 0 && signe*currentBoard[pos + 1] < 0 && _attackBoard[pos + 1] == 0)
                res.Add(pos + 1);
            if (pos + 7 < 64 && pos % 8 != 0 && signe*currentBoard[pos + 7] < 0 && _attackBoard[pos + 7] == 0)
                res.Add(pos + 7);
            if (pos + 8 < 64 && signe*currentBoard[pos + 8] < 0 && _attackBoard[pos + 8] == 0) res.Add(pos + 8);
            if (pos + 9 < 64 && (pos + 1) % 8 != 0 && signe*currentBoard[pos + 9] < 0 && _attackBoard[pos + 9] == 0)
                res.Add(pos + 9);
            
            return res;
        }

        // Pb : est-ce que je marque comme attaquable une case avec quelqu'un dessus ?
        private int[] fill_attack_board(int[] currentBoard, int signe)
        {
            int monRoi = Array.IndexOf(currentBoard, signe * R);
            int[] attackBoard = new int[64];
            int[] tmpBoard = (int[]) currentBoard.Clone();
            tmpBoard[monRoi] = 0;
            for (int i = 0; i < tmpBoard.Length; i++)
            {
                // Pièces adverses
                switch (signe * tmpBoard[i])
                {
                    case -P:
                    case -Pp:
                        // Attaque en diagonale
                        if (i + 9 < 64 && tmpBoard[i + 9] == 0)
                        {
                            attackBoard[i + 9] += 1;
                            if (monRoi == i + 9) _toAttack.Add(i);
                        }
                        if (i + 7 < 64 && tmpBoard[i + 7] == 0)
                        {
                            attackBoard[i + 7] += 1;
                            if (monRoi == i + 7) _toAttack.Add(i);
                        }
                        
                        break;
                    case -Tg:
                    case -Td:
                        foreach (int pos in mvt_tour(tmpBoard, i, -signe, false))
                        {
                            attackBoard[pos] += 1;
                            if (monRoi == pos)
                            {
                                _toAttack.Add(i);
                            }
                        }
                        attackBoard[i] = 0;
                        break;
                    case -Cd:
                    case -Cg:
                        foreach (int pos in mvt_cavalier(tmpBoard, i, -signe, false))
                        {
                            attackBoard[pos] += 1;
                            if (monRoi == pos)
                            {
                                _toAttack.Add(i);
                            }
                        }
                        attackBoard[i] = 0;
                        break;
                    case -F:
                        foreach (int pos in mvt_fou(tmpBoard, i, -signe, false))
                        {
                            attackBoard[pos] += 1;
                            if (monRoi == pos)
                            {
                                _toAttack.Add(i);
                            }
                        }
                        attackBoard[i] = 0;
                        break;
                    case -D:
                        foreach (int pos in mvt_tour(tmpBoard, i, -signe, false))
                        {
                            attackBoard[pos] += 1;
                            if (monRoi == pos)
                            {
                                _toAttack.Add(i);
                            }
                        }
                        foreach (int pos in mvt_fou(tmpBoard, i, -signe, false))
                        {
                            attackBoard[pos] += 1;
                        }
                        attackBoard[i] = 0;
                        break;
                    case -R:
                        if (i - 9 > 0 && i % 8 != 0 && tmpBoard[i - 9] == 0)
                        {
                            attackBoard[i - 9] += 1;
                            if (monRoi == i - 9)
                            {
                                _toAttack.Add(i);
                            }
                        }
                        if (i - 8 > 0 && tmpBoard[i - 8] == 0)
                        {
                            attackBoard[i - 8] += 1;
                            if (monRoi == i - 8)
                            {
                                _toAttack.Add(i);
                            }
                        }
                        if (i - 7 > 0 && (i + 1) % 8 != 0 && tmpBoard[i - 7] == 0)
                        {
                            attackBoard[i - 7] += 1;
                            if (monRoi == i - 7)
                            {
                                _toAttack.Add(i);
                            }
                        }
                        if (i - 1 > 0 && i % 8 != 0 && tmpBoard[i - 1] == 0)
                        {
                            attackBoard[i - 1] += 1;
                            if (monRoi == i - 1)
                            {
                                _toAttack.Add(i);
                            }
                        }
                        if (i + 1 < 64 && (i + 1) != 0 && tmpBoard[i + 1] == 0)
                        {
                            attackBoard[i + 1] += 1;
                            if (monRoi == i + 1)
                            {
                                _toAttack.Add(i);
                            }
                        }
                        if (i + 7 < 64 && i % 8 != 0 && tmpBoard[i + 7] == 0)
                        {
                            attackBoard[i + 7] += 1;
                            if (monRoi == i + 7)
                            {
                                _toAttack.Add(i);
                            }
                        }
                        if (i + 8 < 64 && tmpBoard[i + 8] == 0)
                        {
                            attackBoard[i + 8] += 1;
                            if (monRoi == i + 8)
                            {
                                _toAttack.Add(i);
                            }
                        }
                        if (i + 9 < 64 && (i + 1) % 8 != 0 && tmpBoard[i + 9] == 0)
                        {
                            attackBoard[i + 9] += 1;
                            if (monRoi == i + 9)
                            {
                                _toAttack.Add(i);
                            }
                        }
                        break;
                }
            }
            tmpBoard[monRoi] = signe * R;
            /*
            Console.WriteLine("attackBoard");
            for(int i =0; i < attackBoard.Length; i++)
            {
                if (i%8 == 0) Console.WriteLine("");
                Console.Write(attackBoard[i]+" ");
            }
            Console.WriteLine("\n------------------");
            */

            return attackBoard;
        }

        private Queue fill_queue(ArrayList indexTab, int piece, int[] currentBoard, int curPos, Environment curEnv)
        {
            Queue auxQueue = new Queue();
            foreach (int nextPos in indexTab)
            {
                // Next board
                int[] auxBoard = (int[]) currentBoard.Clone();
                auxBoard[curPos] = 0;
                auxBoard[nextPos] = piece;
                int[] tmpAttackBoard = fill_attack_board(auxBoard, curEnv.CurrentPlayer);
                int monRoi = Array.IndexOf(currentBoard, curEnv.CurrentPlayer * R);
                
                /*
                Console.WriteLine("tmpAttackBoard");
                for(int i =0; i < tmpAttackBoard.Length; i++)
                {
                    if (i%8 == 0) Console.WriteLine("");
                    Console.Write(tmpAttackBoard[i]+" ");
                }
                Console.WriteLine("\n------------------");
                */
                
                if (tmpAttackBoard[monRoi] == 0)
                {
                    // Creating environment
                    Environment env = new Environment(-curEnv.CurrentPlayer,auxBoard,curEnv,new[] {_tabCoord[curPos], _tabCoord[nextPos], ""});
                    auxQueue.Enqueue(env);   
                }
            }
            return auxQueue;
        }

        // Fonction principale
        public Queue ProchainsEnvironnements(Environment curEnv, int signe, bool attackOnly)
        {
            int[] currentBoard = curEnv.Board;
            Queue prochainsEnv = new Queue();
            int monRoi = Array.IndexOf(currentBoard, signe * R);
            if (monRoi == -1)
            {
                Environment echecEnv = new Environment();
                echecEnv.Score = -999999;
                echecEnv.Mvt = null;
                prochainsEnv.Enqueue(echecEnv);
                return prochainsEnv;
            }
            _attackBoard = fill_attack_board(currentBoard, signe);
            
            // Détection d'un échec double ou plus
            if (_attackBoard[monRoi] >= 2)
            {
                ArrayList indexR = mvt_roi(currentBoard, monRoi, signe, attackOnly);
                return fill_queue(indexR, signe * R, currentBoard, monRoi, curEnv);
            }
            
            for (int i = 0; i < currentBoard.Length; i++)
            {
                // Pièces adverses
                switch (signe * currentBoard[i])
                {
                    case P:
                        ArrayList indexP = mvt_pion(currentBoard, i, signe, attackOnly);
                        if (indexP.Count != 0)
                        {
                            foreach (var e in fill_queue(indexP, signe * P, currentBoard, i, curEnv))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case Tg:
                        ArrayList indexTg = mvt_tour(currentBoard, i, signe, attackOnly);
                        if (indexTg.Count != 0) {
                            foreach (var e in fill_queue(indexTg, signe * Tg, currentBoard, i, curEnv))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case Td:
                        ArrayList indexTd = mvt_tour(currentBoard, i, signe, attackOnly);
                        if (indexTd.Count != 0) {
                            foreach (var e in fill_queue(indexTd, signe * Td, currentBoard, i, curEnv))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case Cg:
                        ArrayList indexCg = mvt_cavalier(currentBoard, i, signe, attackOnly);
                        if (indexCg.Count != 0) {
                            foreach (var e in fill_queue(indexCg, signe * Cg, currentBoard, i, curEnv))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case Cd:
                        ArrayList indexCd = mvt_cavalier(currentBoard, i, signe, attackOnly);
                        if (indexCd.Count != 0) {
                            foreach (var e in fill_queue(indexCd, signe * Cd, currentBoard, i, curEnv))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case F:
                        ArrayList indexF = mvt_fou(currentBoard, i, signe, attackOnly);
                        if (indexF.Count != 0) {
                            foreach (var e in fill_queue(indexF, signe * F, currentBoard, i, curEnv))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case D:
                        ArrayList indexD = mvt_tour(currentBoard, i, signe, attackOnly);
                        indexD.AddRange(mvt_fou(currentBoard, i, signe, attackOnly));
                        if (indexD.Count != 0) {
                            foreach (var e in fill_queue(indexD, signe * D, currentBoard, i, curEnv))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case R:
                        ArrayList indexR = mvt_roi(currentBoard, i, signe, attackOnly);
                        if (indexR.Count != 0) {
                            foreach (var e in fill_queue(indexR, signe * R, currentBoard, i, curEnv))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                }
            }
            
            // Détection d'un échec simple
            if (_attackBoard[monRoi] == 1)
            {
                Queue aux = new Queue(prochainsEnv);
                prochainsEnv.Clear();
                foreach (Environment e in aux)
                {
                    if ( _toAttack.Count != 0 && (e.Mvt[1].Equals(_tabCoord[(int) _toAttack[0]]) || currentBoard[Array.IndexOf(_tabCoord, e.Mvt[0])] == signe*R))
                    {
                        prochainsEnv.Enqueue(e);
                    }
                }
            }
            
            return prochainsEnv;
        }
    }
}