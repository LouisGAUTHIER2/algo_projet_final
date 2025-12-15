using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algo_projet_final
{
    internal class Jeu
    {
        private Joueur joueur1;
        private Joueur joueur2;
        private Dictionnaire dico;
        private Plateau plateau;
        private Joueur joueurActuel;

        public Jeu(Joueur joueur1, Joueur joueur2, Dictionnaire dico, Plateau plateau)
        {
            // On initialise les attributs de base
            this.joueur1 = joueur1;
            this.joueur2 = joueur2;
            this.dico = dico;
            this.plateau = plateau;

            this.joueurActuel = joueur1;

        }

        public void ChangePlayer()
        {
            if (this.joueurActuel.Nom == joueur1.Nom) this.joueurActuel = joueur2;
            else this.joueurActuel = joueur1;
        }

        public bool motScored(string mot)
        {
            var pos_mot = plateau.Recherche_Mot(mot);

            if (mot == null || !dico.RechDichoRecursif(mot) || joueurActuel.Contient(mot) || pos_mot == null) return false;

            plateau.Maj_Plateau(pos_mot);
            joueurActuel.Add_Mot(mot);
            joueurActuel.Add_Score(mot.Length);

            return true;
        } 

        public Joueur JoueurActuel
        {
            get { return joueurActuel; }
        }

        public Joueur Joueur1
        {
            get { return joueur1; }
        }

        public Joueur Joueur2
        {
            get { return joueur2; }
        }
    }
}
