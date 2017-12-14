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
        //poids d'importance des pieces sur le terrain
        public int poidsPion = 100, poidsCavalier = 320, poidsFou = 330, poidsTour = 500, poidsReine = 900, poidsRoi = 20000;

        const UInt64 k1 = 0x5555555555555555; /*  -1/3   */
        const UInt64 k2 = 0x3333333333333333; /*  -1/5   */
        const UInt64 k4 = 0x0f0f0f0f0f0f0f0f; /*  -1/17  */
        const UInt64 kf = 0x0101010101010101; /*  -1/255 */

        public int count (UInt64 x) {
                x = x - ((x >> 1) & k1); /* put count of each 2 bits into those 2 bits */
                x = (x & k2) + ((x >> 2) & k2); /* put count of each 4 bits into those 4 bits */
                x = (x + (x >> 4)) & k4; /* put count of each 8 bits into those 8 bits */
                x = (x * kf) >> 56; /* returns 8 most significant bits of x + (x<<8) + (x<<16) + (x<<24) + ...  */
                return (int)x;
            }

        public Moves mov = new Moves();

    //evaluation d'un environnement
    public int evaluerbb(Environnement env)
        {
            Environnement.enumCouleurJoueur adv;
            Environnement.enumCouleurJoueur joueur = env.getJoueur();
            if (joueur == Environnement.enumCouleurJoueur.blanc){
                adv = Environnement.enumCouleurJoueur.noir;
            }
            else
            {
                adv = Environnement.enumCouleurJoueur.blanc;
            }
            int score = poidsRoi * (count(env.getRoi(joueur) - env.getRoi(adv)))
              + poidsReine * (count(env.getReine(joueur) - env.getReine(adv)))
              + poidsTour * (count(env.getTour(joueur) - env.getTour(adv)))
              + poidsCavalier * (count(env.getCavalier(joueur) - env.getCavalier(adv)))
              + poidsFou * (count(env.getFou(joueur) - env.getFou(adv)))
              + poidsPion * (count(env.getPion(joueur) - env.getPion(adv)));
            //TO DO mobility (nombre de mouvements safe possibles par piece avec facteur
            //score = score materiel + mobilité
            return score;
        }

        public int evaluer(Environnement env, bool debutPartie)
        {
            int score = 0;
            int ind = -1;
            int scorePosition = 0;
            if (env.getJoueur() == Environnement.enumCouleurJoueur.blanc) {
               
                foreach (int i in env.board)
                {
                    ind++;
                    switch (i)
                    {
                        case 1:
                            score += poidsPion;
                            scorePosition += TablesEvalMouvement.pion[ind];
                            break;
                        case 21:
                            score += poidsTour;
                            scorePosition += TablesEvalMouvement.tour[ind];
                            break;
                        case 22:
                            score += poidsTour;
                            scorePosition += TablesEvalMouvement.tour[ind];
                            break;
                        case 31:
                            score += poidsCavalier;
                            scorePosition += TablesEvalMouvement.cavalier[ind];
                            break;
                        case 32:
                            score += poidsCavalier;
                            scorePosition += TablesEvalMouvement.cavalier[ind];
                            break;
                        case 4:
                            score += poidsFou;
                            scorePosition += TablesEvalMouvement.fou[ind];
                            break;
                        case 5:
                            score += poidsReine;
                            scorePosition += TablesEvalMouvement.reine[ind];
                            break;
                        case 6:
                            score += poidsRoi;
                            if (debutPartie)
                            {
                                scorePosition += TablesEvalMouvement.roiDebut[ind];
                            }else
                            {
                                scorePosition += TablesEvalMouvement.roiFin[ind];
                            }
                            break;
                        case -1:
                            score -= poidsPion;
                            break;
                        case -21:
                            score -= poidsTour;
                            break;
                        case -22:
                            score -= poidsTour;
                            break;
                        case -31:
                            score -= poidsCavalier;
                            break;
                        case -32:
                            score -= poidsCavalier;
                            break;
                        case -4:
                            score -= poidsFou;
                            break;
                        case -5:
                            score -= poidsReine;
                            break;
                        case -6:
                            score -= poidsRoi;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                foreach (int i in env.board)
                {
                    ind++;
                    switch (i)
                    {
                        case 1:
                            score -= poidsPion;
                            break;
                        case 21:
                            score -= poidsTour;
                            break;
                        case 22:
                            score -= poidsTour;
                            break;
                        case 31:
                            score -= poidsCavalier;
                            break;
                        case 32:
                            score -= poidsCavalier;
                            break;
                        case 4:
                            score -= poidsFou;
                            break;
                        case 5:
                            score -= poidsReine;
                            break;
                        case 6:
                            score -= poidsRoi;
                            break;
                        case -1:
                            score += poidsPion;
                            scorePosition += TablesEvalMouvement.pion_adv[ind];
                            break;
                        case -21:
                            score += poidsTour;
                            scorePosition += TablesEvalMouvement.tour_adv[ind];
                            break;
                        case -22:
                            score += poidsTour;
                            scorePosition += TablesEvalMouvement.tour_adv[ind];
                            break;
                        case -31:
                            score += poidsCavalier;
                            scorePosition += TablesEvalMouvement.cavalier_adv[ind];
                            break;
                        case -32:
                            score += poidsCavalier;
                            scorePosition += TablesEvalMouvement.cavalier_adv[ind];
                            break;
                        case -4:
                            score += poidsFou;
                            scorePosition += TablesEvalMouvement.fou_adv[ind];
                            break;
                        case -5:
                            score += poidsReine;
                            scorePosition += TablesEvalMouvement.fou_adv[ind];
                            break;
                        case -6:
                            score += poidsRoi;
                            if (debutPartie)
                            {
                                scorePosition += TablesEvalMouvement.roiDebut_adv[ind];
                            }
                            else
                            {
                                scorePosition += TablesEvalMouvement.roiFin_adv[ind];
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            //TO DO mobility (nombre de mouvements safe possibles par piece avec facteur
            //score = score materiel + mobilité
            return score + scorePosition;
        }

        public Environnement alphaBeta(Environnement env, int alpha, int beta, int profondeurRestante)
        {
            Queue listeMouvements = new Queue();
            if (profondeurRestante == 0) { return rechercheCalme(alpha, beta, env); }
            else
            {
                Environnement bestScore = new Environnement();
                Environnement val = new Environnement();
                bestScore.score = -99999999;
                int joueurActuel;
                if (env.getJoueur() == Environnement.enumCouleurJoueur.blanc)
                {
                    joueurActuel = 1;
                }
                else
                {
                    joueurActuel = -1;
                }
                listeMouvements = mov.prochainsEnvironnements(env, joueurActuel);
                foreach (Environnement mouvement in listeMouvements)
                {
                    if (mouvement.getJoueur() == Environnement.enumCouleurJoueur.blanc)
                    {
                        mouvement.joueurActuel = Environnement.enumCouleurJoueur.noir;
                    }else
                    {
                        mouvement.joueurActuel = Environnement.enumCouleurJoueur.blanc;
                    }
                    val = alphaBeta(mouvement, -beta, -alpha, profondeurRestante - 1);
                    val.score = val.score * (-1);
                    /*if (env.score >= beta)
                        return env;*/
                    if (val.score > bestScore.score)
                    {
                        bestScore = val;
                        if (bestScore.score > alpha)
                        {
                            alpha = bestScore.score;
                            if (alpha >= beta)
                            {
                                return bestScore;
                            }
                        }
                    }
                }
                return bestScore;
            }
        }
        public Environnement rechercheCalme(int alpha, int beta, Environnement env)
        {
            Environnement tmp = new Environnement();
            int standPat = evaluer(env, true);
            Queue mouvementsCapture = new Queue();
            if (standPat >= beta)
            {
                env.score = beta;
                return env;
            }
            if (alpha < standPat)
            {
                env.score = standPat;
                alpha = standPat;
            }
            mouvementsCapture = mov.prochainsEnvironnementsCapture(env);
            if ( mouvementsCapture.Count == 0)
            {
                return env;
            }
            foreach (Environnement captures in mouvementsCapture)
            {
                if (captures.getJoueur() == Environnement.enumCouleurJoueur.blanc)
                {
                    captures.joueurActuel = Environnement.enumCouleurJoueur.noir;
                }
                else
                {
                    captures.joueurActuel = Environnement.enumCouleurJoueur.blanc;
                }
                tmp = rechercheCalme(-beta, -alpha, captures);
                tmp.score = tmp.score * (-1);
                if (tmp.score >= beta) {
                    tmp.score = beta;
                    return tmp;
                }
                if (tmp.score > alpha)
                {
                    alpha = tmp.score;
                }
                tmp.score = alpha;
                return tmp;
            }
            return env;
            }
        }
    }
