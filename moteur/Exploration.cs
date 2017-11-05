using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moteur
{
    public abstract class Exploration
    {
        protected List<Etat> DejaVisites = new List<Etat>();

        // Insère le noeud dans la frange, représentée sous forme de liste
        protected abstract List<Noeud> InsererNoeud(List<Noeud> frange, Noeud noeud);

        // Fonction générique d'exploration
        // Retourne null en cas d'erreur
        public Queue Explorer(Etat etatInitial, int nbAction, Func<Etat, Etat, bool> testBut)
        {
            /* Création du graphe */
            Graphe arbreRecherche = new Graphe(etatInitial);

            /* Initialisation de la frange */
            List<Noeud> frange = new List<Noeud> { arbreRecherche.Racine };


            /* Boucle de construction de la frange */
            // En cas d'échec, retourne un plan d'action vide
            while (true)
            {
                // Cas d'échec
                if (frange.Count == 0) return null;

                // Test de but
                Noeud noeud = frange.First();
                frange.RemoveAt(0);
                if (testBut(noeud.EtatNoeud, etatInitial) || noeud.Profondeur == nbAction) return arbreRecherche.SequenceActions(noeud);

                // Expansion du noeud
                DejaVisites.Add(noeud.EtatNoeud);
                foreach (Noeud n in noeud.FonctionSuccession())
                {
                    frange = InsererNoeud(frange, n);
                    noeud.AjouterEnfant(n);
                }
            }
        }

    }

    public class RechercheEnLargeur : Exploration
    {
        protected override List<Noeud> InsererNoeud(List<Noeud> frange, Noeud noeud)
        {
            if (!DejaVisites.Exists(n => n.Equals(noeud.EtatNoeud))) { frange.Add(noeud); }
            return frange;
        }
    }

    public class Astar : Exploration
    {

        private int DistanceManhattan(int pos1, int pos2)
        {
            return Math.Abs(pos1 % 10 - pos2 % 10) + Math.Abs(pos1 / 10 - pos2 / 10);
        }

        private int CalculHeuristique(Noeud noeud)
        {
            int min = 200;

            foreach (int x in noeud.EtatNoeud.ListePoussiere)
            {
                int distance = DistanceManhattan(noeud.EtatNoeud.Position, x);
                if (distance < min)
                {
                    min = distance;
                }
            }
            if (noeud.ActionParent == Action.ASPIRER)
            {
                min = 0;
            }


            return min + noeud.EtatNoeud.ListePoussiere.Count + noeud.EtatNoeud.ListeBijoux.Count;
        }

        private static int ComparaisonAStar(Noeud n1, Noeud n2)
        {
            int evaluationN1 = n1.Heuristique + n1.CoutChemin;
            int evaluationN2 = n2.Heuristique + n2.CoutChemin;
            return evaluationN1.CompareTo(evaluationN2);
        }

        protected override List<Noeud> InsererNoeud(List<Noeud> frange, Noeud noeud)
        {
            noeud.Heuristique = CalculHeuristique(noeud);
            frange.Add(noeud);
            frange.Sort(ComparaisonAStar);
            return frange;
        }
    }
}
