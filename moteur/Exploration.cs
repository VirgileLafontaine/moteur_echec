using System;
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

    //evaluation d'un environnement
    public int evaluer(Environnement env)
        {
            Environnement.enumCouleurJoueur adv;
            Environnement.enumCouleurJoueur joueur = env.getJoueur();
            if (joueur == Environnement.enumCouleurJoueur.blanc){
                adv = Environnement.enumCouleurJoueur.noir;
            }
            else
            {
                adv = Environnement.enumCouleurJoueur.noir;
            }
            int score = poidsRoi * (count(env.getRoi(joueur) - env.getRoi(adv)))
              + poidsReine * (count(env.getReine(joueur) - env.getReine(adv)))
              + poidsTour * (count(env.getTour(joueur) - env.getTour(adv)))
              + poidsCavalier * (count(env.getCavalier(joueur) - env.getCavalier(adv)))
              + poidsFou * (count(env.getFou(joueur) - env.getFou(adv)))
              + poidsPion * (count(env.getPion(joueur) - env.getPion(adv)));

            return score;
        }

        public int alphaBeta(Environnement env, int alpha, int beta, int profondeurRestante)
        {
            Queue<Environnement> listeMouvements = new Queue<Environnement>();
            int score = 0;
            if (alpha > beta) {return -1;}
            int bestscore = -99999;
            if (profondeurRestante == 0) return rechercheCalme(alpha, beta, env);
            //TO DO : générer les environnements avec le dernier mouvement et historique mis a jour
            foreach (Environnement mouvement in listeMouvements)
            {
                score = -alphaBeta(mouvement,-beta, -alpha, profondeurRestante - 1);
                if (score >= beta)
                    return score; 
                if (score > bestscore)
                {
                    bestscore = score;
                    if (score > alpha)
                        alpha = score;
                }
            }
            return bestscore;
        }
        public int rechercheCalme(int alpha, int beta, Environnement env)
        {
            int score = 0;//pas sur
            int standPat = evaluer(env);
            if (standPat >= beta)
                return beta;
            if (alpha < standPat)
                alpha = standPat;
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
            return alpha;
            }
        }
    }
}
