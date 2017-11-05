using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur
{
    public class Effecteurs
    {
        //tuple message
        Tuple<Action, int> _message;
        // Effecteur ASPIRER
        public void Aspirer(int position, Environnement env)
        {
            // Console.WriteLine("Effecteur aspirer");
            // Notifier l'environnement qu'on aspire la pièce X
            _message = Tuple.Create(Action.ASPIRER, position);
            Environnement.fileAction.Enqueue(_message);
        }

        // Effecteur RAMASSER
        public void Ramasser(int position, Environnement env)
        {
            // Console.WriteLine("Effecteur ramasser");
            // Notifier l'environnement qu'on ramasse un bijou dans la pièce X
            _message = Tuple.Create(Action.RAMASSER, position);
            Environnement.fileAction.Enqueue(_message);
        }

        //Effecteurs de déplacement : HAUT, BAS, DROITE, GAUCHE
        // Retour :        la nouvelle position de l'agent
        //                 position en cas d'erreur (mouvement impossible)
        public int Haut(int position)
        {
            // Console.WriteLine("Effecteur haut");
            if (position >= 10) return position - 10;
            else return position;
        }

        public int Bas(int position)
        {
            // Console.WriteLine("Effecteur bas");
            if (position < 90) return position + 10;
            else return position;
        }

        public int Gauche(int position)
        {
            // Console.WriteLine("Effecteur gauche");
            if ((position % 10) != 0) return position - 1;
            else return position;
        }

        public int Droite(int position)
        {
            // Console.WriteLine("Effecteur droite");
            if (((position + 1) % 10) != 0) return position + 1;
            else return position;
        }
    }
}
