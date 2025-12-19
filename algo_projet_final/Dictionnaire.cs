using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algo_projet_final
{
    internal class Dictionnaire
    {
        private string[] mots;
        private int numMots;
        private string langue;

        public Dictionnaire(string dic_file_path, string langue)
        {
            //On sauvegarde le chemin du fichier et sa langue
            this.langue = langue;

            // On charge les mots du fichier
            StreamReader reader = new StreamReader(dic_file_path);

            // On compte le nombre de mots dans le fichier
            numMots = 0;

            while (!reader.EndOfStream)
            {
                // Pour chaque ligne, on sépare les mots et on les ajoute à la liste
                string ligne = reader.ReadLine();
                string[] mots_ligne = ligne.Split(' ');

                foreach (var mot in mots_ligne)
                {
                    numMots++;
                }
            }

            mots = new string[numMots];
            int k = 0;

            reader.Close();
            // On relit le fichier pour remplir le tableau de mots
            reader = new StreamReader(dic_file_path);

            while (!reader.EndOfStream)
            {
                // Pour chaque ligne, on sépare les mots et on les ajoute à la liste
                string ligne = reader.ReadLine();
                string[] mots_ligne = ligne.Split(' ');

                foreach (var mot in mots_ligne)
                {
                    mots[k] = mot;
                    k++;
                }
            }

            // On met fin au stream
            reader.Close();
        }

        public void Tri_quick_sort(int debut = 0, int fin = int.MaxValue)
        {
            // On vérifie si il y a une erreur, si il faut initialiser fin et si le tableau est déjà trié
            if (fin == int.MaxValue) fin = numMots - 1;
            if (debut > fin || fin - debut <= 1 || debut < 0) return;
            // On choisit le pivot
            int pivot_index = (debut + fin) / 2;

            // On crée une table temporaire pour le pivot
            string[] temp_tab = new string[fin - debut + 1];
            int index_debut = 0;
            int index_fin = fin - debut;

            for (int i = debut; i <= fin; i++)
            {
                if (i == pivot_index) continue;

                if (string.Compare(mots[i], mots[pivot_index]) <= 0)
                {
                    // on ajoute au début du tableau temporaire
                    temp_tab[index_debut] = mots[i];
                    index_debut++;
                }
                else
                {
                    // on ajoute à la fin du tableau temporaire
                    temp_tab[index_fin] = mots[i];
                    index_fin--;
                }
            }

            // On place le pivot à la bonne position
            temp_tab[index_debut] = mots[pivot_index];

            // On recopie le tableau temporaire dans le tableau principal
            for (int i = debut; i <= fin; i++)
            {
                mots[i] = temp_tab[i - debut];
            }

            // On trie les deux sous-tableaux
            Tri_quick_sort(debut, debut + index_debut - 1);
            Tri_quick_sort(debut + index_debut + 1, fin);
        }

        public string toString()
        {
            return $"Dictionnaire de langue {langue} contenant {numMots} mots.";
        }

        public bool RechDichoRecursif(string mot, int start = 0, int end = int.MaxValue)
        {
            // On initialise end si besoin et on vérifie les conditions sorties
            if (end == int.MaxValue) end = numMots - 1;

            if (start > end) return false;
            else if (start == end) return mots[start] == mot.ToUpper();

            // On calcule le milieu et on lance la recherche récursive
            int mid_index = (start + end) / 2;

            return RechDichoRecursif(mot, start, mid_index) || RechDichoRecursif(mot, mid_index + 1, end);
        }

        public void printMots()
        {
            for (int i = 0; i < numMots; i++)
            {
                Console.WriteLine(mots[i]);
            }
        }

        public static void TestUnitaire()
        {
            Dictionnaire dico = new Dictionnaire("Mots_Français.txt", "Français");

            Console.WriteLine("On tri le dictionnaire...");
            dico.Tri_quick_sort();

            Console.WriteLine("On vérifie si 'ARBRE' est dans le dictionnaire : " + dico.RechDichoRecursif("ARBRE"));
            Console.WriteLine("On vérifie si 'ZIG' est dans le dictionnaire : " + dico.RechDichoRecursif("ZIG"));


        }
    }
}
