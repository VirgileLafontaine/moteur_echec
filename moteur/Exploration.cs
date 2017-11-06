using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur
{
    public class Exploration
    {

        public int alphaBeta(Environnement env, int alpha, int beta, int profondeurRestante)
        {
            Queue<Environnement> listeMouvements = new Queue<Environnement>();
            int score = 0;
            if (alpha > beta) {return -1;}
            int bestscore = -99999;
            if (profondeurRestante == 0) return rechercheCalme(alpha, beta);
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
        public int rechercheCalme(int alpha, int beta)
        {
            //TO DO : quiescence search
            return 0;
        }
    }
}
