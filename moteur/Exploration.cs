using System;
using System.Collections;
using System.Collections.Generic;
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

        public int evaluer(Environnement env)
        {
            int score = 0;
            foreach ( int i in env.board) {
                switch (i) {
                    case 1 :
                        score += poidsPion;
                        break;
                    case 21:
                        score += poidsTour;
                        break;
                    case 22:
                        score += poidsTour;
                        break;
                    case 31:
                        score += poidsCavalier;
                        break;
                    case 32:
                        score += poidsCavalier;
                        break;
                    case 4:
                        score += poidsFou;
                        break;
                    case 5:
                        score += poidsReine;
                        break;
                    case 6:
                        score += poidsRoi;
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
            //TO DO mobility (nombre de mouvements safe possibles par piece avec facteur
            //score = score materiel + mobilité
            return score;
        }

        public Environnement alphaBeta(Environnement env, int alpha, int beta, int profondeurRestante)
        {
            Queue listeMouvements = new Queue();
            Environnement bestScore = new Environnement();
            bestScore.score = -999999;
            if (alpha > beta) {return env;}
            if (profondeurRestante == 0) return rechercheCalme(alpha, beta, env);
            int joueurActuel;
            if ((int)env.getJoueur() == 0) joueurActuel = 1; else joueurActuel = -1;
            listeMouvements = mov.prochainsEnvironnements(env, joueurActuel); //B
            foreach (Environnement mouvement in listeMouvements)
            {
                if (mouvement.getJoueur() == Environnement.enumCouleurJoueur.blanc)
                {
                    mouvement.joueurActuel = Environnement.enumCouleurJoueur.noir;
                }
                else mouvement.joueurActuel = Environnement.enumCouleurJoueur.blanc;
                env.score = alphaBeta(mouvement,-beta, -alpha, profondeurRestante - 1).score * (-1);
                if (env.score >= beta)
                    return env; 
                if (env.score > bestScore.score)
                {
                    bestScore = env;
                    if (env.score > alpha)
                        alpha = env.score;
                }
            }
            return bestScore;
        }
        public Environnement rechercheCalme(int alpha, int beta, Environnement env)
        {
            int standPat = evaluer(env);
            if (standPat >= beta)
            {
                env.score = beta;
                return env;
            }
            if (alpha < standPat)
            {
                env.score = standPat;
            }
               /*
            until(every_capture_has_been_examined)  {
                MakeCapture();
                score = -rechercheCalme(-beta, -alpha, env);
                TakeBackMove();
                
                if (score >= beta)
                    return beta;
                if (score > alpha)
                    alpha = score;
            }*/
            return env;
            }
        }
    }
