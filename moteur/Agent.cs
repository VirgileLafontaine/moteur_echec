using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Moteur
{
    // Actions possibles de l'agent
    // - ASPIRER : l'agent aspire la case dans laquelle il se trouve
    // - RAMASSER : l'agent ramasse un bijou dans la case où il se trouve
    // - HAUT : l'agent se déplace d'une case vers le haut
    // - BAS : l'agent se déplace d'une case vers le bas
    // - DROITE : l'agent se déplace d'une case vers la droite
    // - GAUCHE : l'agent se déplace d'une case vers la gauche
    public enum Action {ASPIRER, RAMASSER, HAUT, BAS, DROITE, GAUCHE, RIEN}
      
    // Algorithmes d'exploration disponibles
    public enum AlgoExploration {LARGEUR, ASTAR}

    public class Agent
    { 
        /* Exploration */
        private Exploration _exploration;
        /* BDI */
        private Bdi _bdi= new Bdi();
        /* Capteur */
        private CapteurObservation _capteur = new CapteurObservation();
        /* Effecteurs */
        private Effecteurs _effecteurs = new Effecteurs();
        /* environnement lié*/
        Environnement _environnement;
        /*estEnVie */
        private volatile Boolean _enVie = true;
        /*temps par action*/
        private int vitesse = 40;
        /*variables apprentissage */
        private int dernierPerf = 0;
        private List<int> deltaPerformances = new List<int>();
        private int tailleListePerf = 100;
        private bool deltaNbAction = false;
        private double alpha = 1.5; //facteur de non prise en compte des anciens deltaPerf
        private double seuil = 1; // seuil de variation pour déclencher une modification de nbaction
        /* Constructeur a utiliser pour placer un agent dans un environnement*/
        public Agent(Environnement env, AlgoExploration exploration)
        {
            _environnement = env;

            switch (exploration)
            {
                case AlgoExploration.LARGEUR:
                    _exploration = new RechercheEnLargeur();
                    break;
                case AlgoExploration.ASTAR:
                    _exploration = new Astar();
                    break;
            }
        }

        /* Dois s'arreter */
        public void Arret()
        {
            _enVie = false;
        }
        /* ------------------------------------------ */
        /*            Fonction principale             */
        /* ------------------------------------------ */
        public void Lancer()
        {
            while (JeSuisEnVie())
            {
                // Observer l'environnement avec mes capteurs
                int[] carteActuelle = _capteur.ObserverCarte(_environnement);
                
                // Mettre à jour mon état (believes de mon BDI)
                MettreAJourBdi(carteActuelle);
                
                // Etablir le plan d'action
                EtablirPlanDAction();
                
                // Exécution du plan d'action
                ExecutionPlanDAction();

                //apprentissage
                miseAJourNBAction();
            }
            Console.WriteLine("thread agent : arrêt");
        }

        /* ------------------------------------------ */
        /*            Fonctions internes              */
        /* ------------------------------------------ */
        private bool JeSuisEnVie()
        {
            return _enVie; // Quand est ce qu'un agent meurt ?
        }

        private void MettreAJourBdi(int[] carteObservee)
        {
            // Actualiser la carte
            Array.Copy(carteObservee,_bdi.Carte,100);
        }

        private void EtablirPlanDAction()
        {
            /* Calcul du nombre de poussières */
            List<int> poussieres = new List<int>();
            List<int> bijoux = new List<int>();
            for(int i = 0; i < 100; i++)
            {
                if(_bdi.Carte[i] == (int) objetCase.POUSSIERE || _bdi.Carte[i] == (int) objetCase.POUSSIEREBIJOUX) {poussieres.Add(i);}
                if(_bdi.Carte[i] == (int) objetCase.BIJOUX || _bdi.Carte[i] == (int) objetCase.POUSSIEREBIJOUX) {bijoux.Add(i);}
            }
            
            Etat etatInitial = new Etat(_bdi.Position,(objetCase) _bdi.Carte[_bdi.Position], poussieres, bijoux);
            _bdi.PlanDAction = _exploration.Explorer(etatInitial, _bdi.NbActions, _bdi.TestBut);
        }

        private void ExecutionPlanDAction()
        {
            //booléen pour stopper le plan en cours
            bool stop = false;
            int cpt = 0;
            // Actualiser le coût
            _bdi.Cout+= _bdi.PlanDAction.Count;
            
            // Exécution des actions
            while (_bdi.PlanDAction.Count != 0 && !stop)
            {
                cpt++;
                if (cpt == _bdi.NbActions)
                {
                    stop = true;
                }
                switch ((int) _bdi.PlanDAction.Dequeue())
                {
                    case (int) Action.ASPIRER:
                        _effecteurs.Aspirer(_bdi.Position,_environnement);
                        break;
                    case (int) Action.RAMASSER:
                        _effecteurs.Ramasser(_bdi.Position,_environnement);
                        break;
                    case (int) Action.HAUT:
                        _bdi.Position = _effecteurs.Haut(_bdi.Position);
                        break;
                    case (int) Action.BAS:
                        _bdi.Position = _effecteurs.Bas(_bdi.Position);
                        break;
                    case (int) Action.GAUCHE:
                        _bdi.Position = _effecteurs.Gauche(_bdi.Position);
                        break;
                    case (int) Action.DROITE:
                        _bdi.Position = _effecteurs.Droite(_bdi.Position);
                        break;
                }
                System.Threading.Thread.Sleep(1000 / vitesse);
            }
        }

        //fonction d'apprentissage
        private void miseAJourNBAction()
        {
            if (deltaPerformances.Count == tailleListePerf)
            {
                deltaPerformances.RemoveAt(tailleListePerf-1);
            }
            int tempPerf = _capteur.ObserverPerformance(_environnement);
            deltaPerformances.Insert(0, tempPerf-dernierPerf);
            dernierPerf = tempPerf;
            double somme = 0;
            for (int i = 0; i < deltaPerformances.Count; i++)
            {
                somme += (double)deltaPerformances[i] / (alpha * Math.Exp((double)i));
            }
            if (deltaPerformances.Count == 1) { _bdi.NbActions--; }
            else if (somme < 0 && Math.Abs(somme)>seuil)
            {
                if (deltaNbAction) { _bdi.NbActions++; }
                else {
                    if (_bdi.NbActions > 1) { _bdi.NbActions--; }
                    }
            }else if (somme > 0 && Math.Abs(somme) > seuil)
            {
                if (deltaNbAction)
                {
                    if (_bdi.NbActions > 1) { _bdi.NbActions--; deltaNbAction = false; }
                }
                else { _bdi.NbActions++; deltaNbAction = true; }
            }
        }
        /* ------------------------------------------ */
        /*             Fonctions publiques            */
        /* ------------------------------------------ */

        public int getPosition()
        {
            return this._bdi.Position;
        }
        public int getNBAction()
        {
            return this._bdi.NbActions;
        }
    }
}