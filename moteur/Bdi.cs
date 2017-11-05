using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur
{
    public class Bdi
    {
        /*---------------------------------*/
        /*       Believes - Croyances      */
        /*---------------------------------*/
        // Etat de la carte récupéré lors de la phase d'exploration
        public int[] Carte { get; set; }

        // Position de l'agent sur la carte
        public int Position { get; set; }

        // Nombre d'actions à exécuter avant de faire une nouvelle exploration
        // Initialement, ce paramètre sera fixé à 1
        public int NbActions { get; set; }

        /*---------------------------------*/
        /*             Intentions          */
        /*---------------------------------*/
        // Plan d'action à réaliser
        public Queue PlanDAction;

        // Coût total
        public int Cout { get; set; }

        /*---------------------------------*/
        /*             Desires             */
        /*---------------------------------*/
        // Objectif : aspirer toutes les poussières
        // L'objectif est formulé sous la forme d'une fonction de test de but
        public bool TestBut(Etat etat, Etat etatInitial)
        {
            return (etat.ListePoussiere.Count() < etatInitial.ListePoussiere.Count()
                    || etat.ListeBijoux.Count() < etatInitial.ListeBijoux.Count()
                    || etat.ListePoussiere.Count() == 0);
        }

        // Constructeur du BDI
        public Bdi()
        {
            Carte = new int[100];
            Position = 45;
            NbActions = 10;
            PlanDAction = new Queue();
        }
    }
}
