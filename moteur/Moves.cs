using System;
using System.Collections;
using System.Collections.Generic;

namespace Moteur
{
    public class Moves
    {
        private int[] attack_board = new int[64];
        private ArrayList toAttack = new ArrayList(); 

        //Coordonnées des cases
        string[] tabCoord = new string[]
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
        private const int PP = 10; //pion passant

        private const int P = 1; //pion
        private const int TG = 21; //tour gauche (different pour le roque)
        private const int TD = 22; //tour droite
        private const int CG = 31; //cavalier gauche (différents pour l'image)
        private const int CD = 32; //cavalier droit
        private const int F = 4; //fou
        private const int D = 5; //dame
        private const int R = 6; //roi

        private ArrayList mvt_pion(int[] current_board, int pos, int signe)
        {
            ArrayList res = new ArrayList();
            // Mouvement de 1 si la case est libre
            if (pos + signe * (-8) < 64 && pos + signe * (-8) >= 0 && current_board[pos + signe * (-8)] == 0)
                res.Add(pos + signe * (-8));

            // Si premier mouvement : déplacement de 2 possible
            if ((current_board[pos] == P && pos <= 55 && pos >= 48 && current_board[pos-8] == 0 && current_board[pos-16] == 0) ||
                (current_board[pos] == -P && pos <= 15 && pos >= 8 && current_board[pos+8] == 0 && current_board[pos+16] == 0)) res.Add(pos + signe * (-16));

            // Attaque en diagonale
            if (pos + signe * (-9) < 64 && pos + signe * (-9) >= 0 && pos%8 !=0 && signe * current_board[pos + signe * (-9)] < 0)
                res.Add(pos + signe * (-9));
            if (pos + signe * (-7) < 64 && pos + signe * (-7) >= 0 && (pos+1)%8 !=0 && signe * current_board[pos + signe * (-7)] < 0)
                res.Add(pos + signe * (-7));

            return res;
        }
        private ArrayList mvt_pionC(int[] current_board, int pos, int signe)
        {
            ArrayList res = new ArrayList();
            
            // Attaque en diagonale
            if (pos + signe * (-9) < 64 && pos + signe * (-9) >= 0 && pos % 8 != 0 && signe * current_board[pos + signe * (-9)] < 0)
                res.Add(pos + signe * (-9));
            if (pos + signe * (-7) < 64 && pos + signe * (-7) >= 0 && (pos + 1) % 8 != 0 && signe * current_board[pos + signe * (-7)] < 0)
                res.Add(pos + signe * (-7));
            return res;
        }

        private ArrayList mvt_tour(int[] current_board, int pos, int signe)
        {
            ArrayList res = new ArrayList();
            // Attaque colonne vers le haut
            for (int j = pos; j > 0; j -= 8)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] <= 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }
            // Attaque colonne vers le bas
            for (int j = pos; j < 64; j += 8)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] <= 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }
            // Attaque ligne vers la gauche
            for (int j = pos; (j+1) % 8 != 0 || j == pos; j -= 1)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] <= 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }
            // Attaque ligne vers la droite
            for (int j = pos; j% 8 != 0 || j == pos; j += 1)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] <= 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }

            return res;
        }
        private ArrayList mvt_tourC(int[] current_board, int pos, int signe)
        {
            ArrayList res = new ArrayList();
            // Attaque colonne vers le haut
            for (int j = pos; j > 0; j -= 8)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] < 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }
            // Attaque colonne vers le bas
            for (int j = pos; j < 64; j += 8)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] < 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }
            // Attaque ligne vers la gauche
            for (int j = pos; (j + 1) % 8 != 0 || j == pos; j -= 1)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] < 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }
            // Attaque ligne vers la droite
            for (int j = pos; j % 8 != 0 || j == pos; j += 1)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] < 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }

            return res;
        }

        private ArrayList mvt_cavalier(int[] current_board, int pos, int signe)
        {
            ArrayList res = new ArrayList();
            if (pos - 17 > 0 && pos % 8 != 0 && signe * current_board[pos - 17] <= 0) res.Add(pos - 17);
            if (pos - 15 > 0 && (pos + 1) % 8 != 0 && signe * current_board[pos - 15] <= 0) res.Add(pos - 15);
            if (pos - 10 > 0 && (pos) % 8 != 0 && (pos - 1) % 8 != 0 && signe * current_board[pos - 10] <= 0)
                res.Add(pos - 10);
            if (pos - 6 > 0 && (pos + 1) % 8 != 0 && (pos + 2) % 8 != 0 && signe * current_board[pos - 6] <= 0)
                res.Add(pos - 6);
            if (pos + 6 < 64 && (pos) % 8 != 0 && (pos - 1) % 8 != 0 && signe * current_board[pos + 6] <= 0)
                res.Add(pos + 6);
            if (pos + 10 < 64 && (pos + 1) % 8 != 0 && (pos + 2) % 8 != 0 && signe * current_board[pos + 10] <= 0)
                res.Add(pos + 10);
            if (pos + 15 < 64 && pos % 8 != 0 && signe * current_board[pos + 15] <= 0) res.Add(pos + 15);
            if (pos + 17 < 64 && (pos + 1) % 8 != 0 && signe * current_board[pos + 17] <= 0) res.Add(pos + 17);

            return res;
        }
        private ArrayList mvt_cavalierC(int[] current_board, int pos, int signe)
        {
            ArrayList res = new ArrayList();
            if (pos - 17 > 0 && pos % 8 != 0 && signe * current_board[pos - 17] < 0) res.Add(pos - 17);
            if (pos - 15 > 0 && (pos + 1) % 8 != 0 && signe * current_board[pos - 15] < 0) res.Add(pos - 15);
            if (pos - 10 > 0 && (pos) % 8 != 0 && (pos - 1) % 8 != 0 && signe * current_board[pos - 10] < 0)
                res.Add(pos - 10);
            if (pos - 6 > 0 && (pos + 1) % 8 != 0 && (pos + 2) % 8 != 0 && signe * current_board[pos - 6] < 0)
                res.Add(pos - 6);
            if (pos + 6 < 64 && (pos) % 8 != 0 && (pos - 1) % 8 != 0 && signe * current_board[pos + 6] < 0)
                res.Add(pos + 6);
            if (pos + 10 < 64 && (pos + 1) % 8 != 0 && (pos + 2) % 8 != 0 && signe * current_board[pos + 10] < 0)
                res.Add(pos + 10);
            if (pos + 15 < 64 && pos % 8 != 0 && signe * current_board[pos + 15] < 0) res.Add(pos + 15);
            if (pos + 17 < 64 && (pos + 1) % 8 != 0 && signe * current_board[pos + 17] < 0) res.Add(pos + 17);

            return res;
        }

        private ArrayList mvt_fou(int[] current_board, int pos, int signe)
        {
            ArrayList res = new ArrayList();
            // Diagonale haut gauche
            for (int j = pos; j > 0 && (j+1) % 8 != 0; j -= 9)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] <= 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }
            // Diagonale haut droite
            for (int j = pos; j > 0 && j % 8 != 0; j -= 7)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] <= 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }
            // Diagonale bas gauche
            for (int j = pos; j < 64 && (j+1) % 8 != 0; j += 7)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] <= 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }
            // Diagonale bas droite
            for (int j = pos; j < 64 && j % 8 != 0; j += 9)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] <= 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }

            return res;
        }
        private ArrayList mvt_fouC(int[] current_board, int pos, int signe)
        {
            ArrayList res = new ArrayList();
            // Diagonale haut gauche
            for (int j = pos; j > 0 && (j + 1) % 8 != 0; j -= 9)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] < 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }
            // Diagonale haut droite
            for (int j = pos; j > 0 && j % 8 != 0; j -= 7)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] < 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }
            // Diagonale bas gauche
            for (int j = pos; j < 64 && (j + 1) % 8 != 0; j += 7)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] < 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }
            // Diagonale bas droite
            for (int j = pos; j < 64 && j % 8 != 0; j += 9)
            {
                // Case vide ou avec un ennemi
                if (signe * current_board[j] < 0) res.Add(j);
                // Case amie
                if ((signe * current_board[j] > 0 && j != pos) || signe * current_board[j] < 0) break;
            }

            return res;
        }

        private ArrayList mvt_roi(int[] current_board, int pos, int signe)
        {
            ArrayList res = new ArrayList();
            if (pos - 9 > 0 && pos % 8 != 0 && signe*current_board[pos - 9] <= 0 && attack_board[pos - 9] == 0)
                res.Add(pos - 9);
            if (pos - 8 > 0 && signe*current_board[pos - 8] <= 0 && attack_board[pos - 8] == 0) res.Add(pos - 8);
            if (pos - 7 > 0 && (pos + 1) % 8 != 0 && signe*current_board[pos - 7] <= 0 && attack_board[pos - 7] == 0)
                res.Add(pos - 7);
            if (pos - 1 > 0 && pos % 8 != 0 && signe*current_board[pos - 1] <= 0 && attack_board[pos - 1] == 0)
                res.Add(pos - 1);
            if (pos + 1 < 64 && (pos + 1) != 0 && signe*current_board[pos + 1] <= 0 && attack_board[pos + 1] == 0)
                res.Add(pos + 1);
            if (pos + 7 < 64 && pos % 8 != 0 && signe*current_board[pos + 7] <= 0 && attack_board[pos + 7] == 0)
                res.Add(pos + 7);
            if (pos + 8 < 64 && signe*current_board[pos + 8] <= 0 && attack_board[pos + 8] == 0) res.Add(pos + 8);
            if (pos + 9 < 64 && (pos + 1) % 8 != 0 && signe*current_board[pos + 9] <= 0 && attack_board[pos + 9] == 0)
                res.Add(pos + 9);
            return res;
        }
        private ArrayList mvt_roiC(int[] current_board, int pos, int signe)
        {
            ArrayList res = new ArrayList();
            if (pos - 9 > 0 && pos % 8 != 0 && signe * current_board[pos - 9] < 0 && attack_board[pos - 9] == 0)
                res.Add(pos - 9);
            if (pos - 8 > 0 && signe * current_board[pos - 8] < 0 && attack_board[pos - 8] == 0) res.Add(pos - 8);
            if (pos - 7 > 0 && (pos + 1) % 8 != 0 && signe * current_board[pos - 7] < 0 && attack_board[pos - 7] == 0)
                res.Add(pos - 7);
            if (pos - 1 > 0 && pos % 8 != 0 && signe * current_board[pos - 1] < 0 && attack_board[pos - 1] == 0)
                res.Add(pos - 1);
            if (pos + 1 < 64 && (pos + 1) != 0 && signe * current_board[pos + 1] < 0 && attack_board[pos + 1] == 0)
                res.Add(pos + 1);
            if (pos + 7 < 64 && pos % 8 != 0 && signe * current_board[pos + 7] < 0 && attack_board[pos + 7] == 0)
                res.Add(pos + 7);
            if (pos + 8 < 64 && signe * current_board[pos + 8] < 0 && attack_board[pos + 8] == 0) res.Add(pos + 8);
            if (pos + 9 < 64 && (pos + 1) % 8 != 0 && signe * current_board[pos + 9] < 0 && attack_board[pos + 9] == 0)
                res.Add(pos + 9);
            return res;
        }

        // Pb : est-ce que je marque comme attaquable une case avec quelqu'un dessus ?
        private void fill_attack_board(int[] current_board, int signe)
        {
            int monRoi = Array.IndexOf(current_board, signe * R);
            current_board[monRoi] = 0;
            for (int i = 0; i < current_board.Length; i++)
            {
                // Pièces adverses
                switch (signe * current_board[i])
                {
                    case -P:
                        // Attaque en diagonale
                        if (i + 9 < 64 && current_board[i + 9] == 0)
                        {
                            attack_board[i + 9] += 1;
                            if (monRoi == i + 9) toAttack.Add(i);
                        }
                        if (i + 7 < 64 && current_board[i + 7] == 0)
                        {
                            attack_board[i + 7] += 1;
                            if (monRoi == i + 7) toAttack.Add(i);
                        }
                        
                        break;
                    case -TG:
                    case -TD:
                        foreach (int pos in mvt_tour(current_board, i, -signe))
                        {
                            attack_board[pos] += 1;
                            if (monRoi == pos)
                            {
                                toAttack.Add(i);
                            }
                        }
                        attack_board[i] = 0;
                        break;
                    case -CD:
                    case -CG:
                        foreach (int pos in mvt_cavalier(current_board, i, -signe))
                        {
                            attack_board[pos] += 1;
                            if (monRoi == pos)
                            {
                                toAttack.Add(i);
                            }
                        }
                        attack_board[i] = 0;
                        break;
                    case -F:
                        foreach (int pos in mvt_fou(current_board, i, -signe))
                        {
                            attack_board[pos] += 1;
                            if (monRoi == pos)
                            {
                                toAttack.Add(i);
                            }
                        }
                        attack_board[i] = 0;
                        break;
                    case -D:
                        foreach (int pos in mvt_tour(current_board, i, -signe))
                        {
                            attack_board[pos] += 1;
                            if (monRoi == pos)
                            {
                                toAttack.Add(i);
                            }
                        }
                        foreach (int pos in mvt_fou(current_board, i, -signe))
                        {
                            attack_board[pos] += 1;
                        }
                        attack_board[i] = 0;
                        break;
                    case -R:
                        if (i - 9 > 0 && i % 8 != 0 && current_board[i - 9] == 0)
                        {
                            attack_board[i - 9] += 1;
                            if (monRoi == i - 9)
                            {
                                toAttack.Add(i);
                            }
                        }
                        if (i - 8 > 0 && current_board[i - 8] == 0)
                        {
                            attack_board[i - 8] += 1;
                            if (monRoi == i - 8)
                            {
                                toAttack.Add(i);
                            }
                        }
                        if (i - 7 > 0 && (i + 1) % 8 != 0 && current_board[i - 7] == 0)
                        {
                            attack_board[i - 7] += 1;
                            if (monRoi == i - 7)
                            {
                                toAttack.Add(i);
                            }
                        }
                        if (i - 1 > 0 && i % 8 != 0 && current_board[i - 1] == 0)
                        {
                            attack_board[i - 1] += 1;
                            if (monRoi == i - 1)
                            {
                                toAttack.Add(i);
                            }
                        }
                        if (i + 1 < 64 && (i + 1) != 0 && current_board[i + 1] == 0)
                        {
                            attack_board[i + 1] += 1;
                            if (monRoi == i + 1)
                            {
                                toAttack.Add(i);
                            }
                        }
                        if (i + 7 < 64 && i % 8 != 0 && current_board[i + 7] == 0)
                        {
                            attack_board[i + 7] += 1;
                            if (monRoi == i + 7)
                            {
                                toAttack.Add(i);
                            }
                        }
                        if (i + 8 < 64 && current_board[i + 8] == 0)
                        {
                            attack_board[i + 8] += 1;
                            if (monRoi == i + 8)
                            {
                                toAttack.Add(i);
                            }
                        }
                        if (i + 9 < 64 && (i + 1) % 8 != 0 && current_board[i + 9] == 0)
                        {
                            attack_board[i + 9] += 1;
                            if (monRoi == i + 9)
                            {
                                toAttack.Add(i);
                            }
                        }
                        break;
                }
            }
            current_board[monRoi] = signe * R;
        }

        private Queue fill_queue(ArrayList indexTab, int piece, int[] current_board, int cur_pos, Environnement cur_env)
        {
            Queue auxQueue = new Queue();
            foreach (int next_pos in indexTab)
            {
                Environnement env = new Environnement();
                int[] aux_board = (int[]) current_board.Clone();
                aux_board[cur_pos] = 0;
                aux_board[next_pos] = piece;
               /* if (cur_env.getJoueur() == Environnement.enumCouleurJoueur.blanc)
                {
                    env.joueurActuel = Environnement.enumCouleurJoueur.noir;
                }
                else
                {
                    env.joueurActuel = Environnement.enumCouleurJoueur.blanc;
                }*/
                env.board = (int[]) aux_board.Clone();
                env.mvt = new string[] {tabCoord[cur_pos], tabCoord[next_pos], ""};
                env.historiqueMouvement =new List<Environnement>();
                env.historiqueMouvement.Add(cur_env);
               
                auxQueue.Enqueue(env);
            }
            return auxQueue;
        }

        // Fonction principale
        public Queue prochainsEnvironnements(Environnement cur_env, int signe)
        {
            int[] current_board = cur_env.board;
            if (cur_env.getJoueur() == Environnement.enumCouleurJoueur.blanc)
            {
                cur_env.joueurActuel = Environnement.enumCouleurJoueur.noir;
            }else
            {
                cur_env.joueurActuel = Environnement.enumCouleurJoueur.blanc;
            }
            Queue prochainsEnv = new Queue();
            
            fill_attack_board(current_board, signe);
            int monRoi = Array.IndexOf(current_board, signe * R);
            
            // Détection d'un échec double ou plus
            if (attack_board[monRoi] >= 2)
            {
                ArrayList indexR = mvt_roi(current_board, monRoi, signe);
                return fill_queue(indexR, signe * R, current_board, monRoi, cur_env);
            }
            
            for (int i = 0; i < current_board.Length; i++)
            {
                // Pièces adverses
                switch (signe * current_board[i])
                {
                    case P:
                        ArrayList indexP = mvt_pion(current_board, i, signe);
                        if (indexP.Count != 0)
                        {
                            foreach (var e in fill_queue(indexP, signe * P, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case TG:
                        ArrayList indexTG = mvt_tour(current_board, i, signe);
                        if (indexTG.Count != 0) {
                            foreach (var e in fill_queue(indexTG, signe * TG, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case TD:
                        ArrayList indexTD = mvt_tour(current_board, i, signe);
                        if (indexTD.Count != 0) {
                            foreach (var e in fill_queue(indexTD, signe * TD, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case CG:
                        ArrayList indexCG = mvt_cavalier(current_board, i, signe);
                        if (indexCG.Count != 0) {
                            foreach (var e in fill_queue(indexCG, signe * CG, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case CD:
                        ArrayList indexCD = mvt_cavalier(current_board, i, signe);
                        if (indexCD.Count != 0) {
                            foreach (var e in fill_queue(indexCD, signe * CD, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case F:
                        ArrayList indexF = mvt_fou(current_board, i, signe);
                        if (indexF.Count != 0) {
                            foreach (var e in fill_queue(indexF, signe * F, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case D:
                        ArrayList indexD = mvt_tour(current_board, i, signe);
                        indexD.AddRange(mvt_fou(current_board, i, signe));
                        if (indexD.Count != 0) {
                            foreach (var e in fill_queue(indexD, signe * D, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case R:
                        ArrayList indexR = mvt_roi(current_board, i, signe);
                        if (indexR.Count != 0) {
                            foreach (var e in fill_queue(indexR, signe * R, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                }
            }
            
            // Détection d'un échec simple
            if (attack_board[monRoi] == 1)
            {
                Queue aux = new Queue(prochainsEnv);
                prochainsEnv.Clear();
                foreach (Environnement e in aux)
                {
                    if (e.mvt[1].Equals(tabCoord[(int) toAttack[0]]) || current_board[Array.IndexOf(tabCoord, e.mvt[0])] == signe*R)
                    {
                        prochainsEnv.Enqueue(e);
                    }
                }
            }
            
            return prochainsEnv;
        }
        public Queue prochainsEnvironnementsCapture(Environnement cur_env)
        {
            int signe;
            if (cur_env.getJoueur() == Environnement.enumCouleurJoueur.blanc)
            {
                signe = 1;
            }else {
                signe = -1;
            }
            int[] current_board = cur_env.board;
            Queue prochainsEnv = new Queue();
            for (int i = 0; i < current_board.Length; i++)
            {
                // Pièces adverses
                switch (signe * current_board[i])
                {
                    case P:
                        ArrayList indexP = mvt_pionC(current_board, i, signe);
                        if (indexP.Count != 0)
                        {
                            foreach (var e in fill_queue(indexP, signe * P, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case TG:
                        ArrayList indexTG = mvt_tourC(current_board, i, signe);
                        if (indexTG.Count != 0)
                        {
                            foreach (var e in fill_queue(indexTG, signe * TG, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case TD:
                        ArrayList indexTD = mvt_tourC(current_board, i, signe);
                        if (indexTD.Count != 0)
                        {
                            foreach (var e in fill_queue(indexTD, signe * TD, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case CG:
                        ArrayList indexCG = mvt_cavalierC(current_board, i, signe);
                        if (indexCG.Count != 0)
                        {
                            foreach (var e in fill_queue(indexCG, signe * CG, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case CD:
                        ArrayList indexCD = mvt_cavalierC(current_board, i, signe);
                        if (indexCD.Count != 0)
                        {
                            foreach (var e in fill_queue(indexCD, signe * CD, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case F:
                        ArrayList indexF = mvt_fouC(current_board, i, signe);
                        if (indexF.Count != 0)
                        {
                            foreach (var e in fill_queue(indexF, signe * F, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case D:
                        ArrayList indexD = mvt_tourC(current_board, i, signe);
                        indexD.AddRange(mvt_fou(current_board, i, signe));
                        if (indexD.Count != 0)
                        {
                            foreach (var e in fill_queue(indexD, signe * D, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                    case R:
                        ArrayList indexR = mvt_roiC(current_board, i, signe);
                        if (indexR.Count != 0)
                        {
                            foreach (var e in fill_queue(indexR, signe * R, current_board, i, cur_env))
                            {
                                prochainsEnv.Enqueue(e);
                            }
                        }
                        break;
                }
            }
            return prochainsEnv;
        }

    }
}