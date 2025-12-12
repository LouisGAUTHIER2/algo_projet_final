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

        public Jeu(Joueur joueur1, Joueur joueur2, Dictionnaire dico, Plateau plateau, string score_file_path)
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

       /* public bool MakePlayerMove(string mot)
        {
            if (joueurActuel.Contient(mot) || !dico.RechDichoRecursif(mot) || plateau.Recherche_Mot(mot)) return false;

            joueurActuel.Add_Mot(mot);
            joueurActuel.Add_Score(mot.Length);
            plateau.Maj_Plateau(mot);

            return true;
        }*/
    }
}
