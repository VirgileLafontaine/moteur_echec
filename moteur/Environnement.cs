using System;
using System.Collections;
using System.Collections.Generic;

namespace Moteur
{
    public class Environnement
    {
        public static int joueurBlanc = 1;
        public static int joueurNoir = -1;

        //--------------variables de l'environnement-------------//
        public List<Environnement> historiqueMouvement = new List<Environnement>();
        ulong [] piecesBB; //bitboard des pieces
        ulong videBB;      //bitboard des positions vides
        ulong occupeBB;    //bitboard des positions occupées
        ulong blancBB;     //bitboard des positions joueur blanc
        ulong noirBB;      //bitboard des positions joueur noir
        public enumCouleurJoueur joueurActuel;
        public int[] board;
        public string[] mvt;
        public int score;
        public enum enumPieces    //indice des bitboards des pieces dans le tableau de bitboards piecesBB
        {
            iBlanc,     // une piece blanche
            iNoir,     // une piece noire
            iPionBlanc,
            iPionNoir,
            iCavalierBlanc,
            iCavalierNoir,
            iFouBlanc,
            iFouNoir,
            iTourBlanche,
            iTourNoir,
            iReineBlanche,
            iReineNoir,
            iRoiBlanc,
            iRoiNoir
        }
        public enum enumCouleurJoueur
        {
            blanc,
            noir,
        }
        //-----------------------getters-----------------------//
        public ulong getPieceSet(enumPieces p)
        {
            return piecesBB[(int)p];
        }
        public ulong getPion(enumCouleurJoueur c)
        {
            return piecesBB[(int)enumPieces.iPionBlanc + (int)c];
        }
        public ulong getCavalier(enumCouleurJoueur c)
        {
            return piecesBB[(int)enumPieces.iCavalierBlanc + (int)c];
        }
        public ulong getFou(enumCouleurJoueur c)
        {
            return piecesBB[(int)enumPieces.iFouBlanc + (int)c];
        }
        public ulong getTour(enumCouleurJoueur c)
        {
            return piecesBB[(int)enumPieces.iTourBlanche + (int)c];
        }
        public ulong getReine(enumCouleurJoueur c)
        {
            return piecesBB[(int)enumPieces.iReineBlanche + (int)c];
        }
        public ulong getRoi(enumCouleurJoueur c)
        {
            return piecesBB[(int)enumPieces.iRoiBlanc + (int)c];
        }
        public ulong getOccupe()
        {
            return occupeBB;
        }
        public ulong getBlanc()
        {
            return blancBB;
        }
        public ulong getNoir()
        {
            return noirBB;
        }
        public enumCouleurJoueur getJoueur()
        {
            return joueurActuel;
        }
        
    }
}

