using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algo_projet_final
{

    public class Plateau
    {
        private char[,] matrice;
        private int lignes;
        private int colonnes;
        private char[] lettres;
        private int[] maxLettres;
        private int nbLettres;

        // Génère le plateau aléatoirement à partir de Lettres.txt
        public Plateau(string fichierLettres, int lignes, int colonnes)
        {
            this.lignes = lignes;
            this.colonnes = colonnes;
            matrice = new char[lignes, colonnes];
            LireLettres(fichierLettres);
            GenererAleatoirement();
        }

        // Initialise le plateau à partir d’un csv
        public Plateau(string fichierCSV)
        {
            ToRead(fichierCSV);
        }

        // Lire les lettres et leur nombre max depuis Lettres.txt
        private void LireLettres(string fichier)
        {
            StreamReader sr = null;
            nbLettres = 0;
            lettres = new char[26];
            maxLettres = new int[26];

            try
            {
                sr = new StreamReader(fichier);
                string ligne;
                while ((ligne = sr.ReadLine()) != null && nbLettres < 26)
                {
                    string[] parts = ligne.Split(',');
                    if (parts.Length >= 2)
                    {
                        lettres[nbLettres] = parts[0][0];
                        maxLettres[nbLettres] = int.Parse(parts[1]);
                        nbLettres++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur de lecture du fichier Lettres : " + ex.Message);
            }
            finally
            {
                if (sr != null) sr.Close();
            }
        }

        // Génère la matrice aléatoirement selon le stock de lettres
        private void GenererAleatoirement()
        {
            int[] stock = new int[nbLettres];
            for (int i = 0; i < nbLettres; i++)
                stock[i] = maxLettres[i];

            Random r = new Random();

            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    int idx;
                    do
                    {
                        idx = r.Next(nbLettres);
                    } while (stock[idx] == 0);

                    matrice[i, j] = lettres[idx];
                    stock[idx]--;
                }
            }
        }

        // Affiche le plateau
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    sb.Append(matrice[i, j]);
                    if (j < colonnes - 1) sb.Append(' ');
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        // Sauvegarde le plateau dans un fichier csv
        public void ToFile(string nomfile)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(nomfile);
                for (int i = 0; i < lignes; i++)
                {
                    for (int j = 0; j < colonnes; j++)
                    {
                        sw.Write(matrice[i, j]);
                        if (j < colonnes - 1) sw.Write(',');
                    }
                    sw.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur d'écriture du fichier : " + ex.Message);
            }
            finally
            {
                if (sw != null) sw.Close();
            }
        }

        // Charge le plateau d'un fichier csv
        public void ToRead(string nomfile)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(nomfile);
                string ligne;
                // On compte le nombre de lignes et de colonnes
                int tempLignes = 0;
                int tempColonnes = 0;
                string[] lignesTemp = new string[100]; // suppose max 100 lignes

                while ((ligne = sr.ReadLine()) != null)
                {
                    lignesTemp[tempLignes] = ligne;
                    tempLignes++;
                }
                if (tempLignes > 0)
                    tempColonnes = lignesTemp[0].Split(',').Length;

                lignes = tempLignes;
                colonnes = tempColonnes;
                matrice = new char[lignes, colonnes];

                for (int i = 0; i < lignes; i++)
                {
                    string[] casesLigne = lignesTemp[i].Split(',');
                    for (int j = 0; j < colonnes; j++)
                    {
                        matrice[i, j] = casesLigne[j][0];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur de lecture du plateau : " + ex.Message);
            }
            finally
            {
                if (sr != null) sr.Close();
            }
        }
        public object Recherche_Mot(string mot)
        {
            mot = mot.ToUpper();
            int len = mot.Length;

            // Parcourt tout le plateau
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    if (matrice[i, j] == mot[0])
                    {
                        int[,] positions = new int[len, 2];
                        bool[,] visite = new bool[lignes, colonnes];

                        // On tente de suivre le mot depuis cette case
                        if (ChercherDepuis(i, j, mot, 0, positions, visite))
                            return positions;
                    }
                }
            }

            return null; // pas trouvé
        }


        private bool ChercherDepuis(int ligne, int col, string mot, int index, int[,] positions, bool[,] visite)
        {
            if (index == mot.Length)
                return true;

            // Conditions de base
            if (ligne < 0 || ligne >= lignes || col < 0 || col >= colonnes)
                return false;
            if (visite[ligne, col])
                return false;
            if (matrice[ligne, col] != mot[index])
                return false;

            // On enregistre la position
            visite[ligne, col] = true;
            positions[index, 0] = ligne;
            positions[index, 1] = col;

            // Déplacements possibles : haut, bas, gauche, droite
            int[,] dirs = {
        {-1, 0},
        { 1, 0},
        { 0,-1},
        { 0, 1}
    };

            // Essayer chaque direction
            for (int k = 0; k < 4; k++)
            {
                int nl = ligne + dirs[k, 0];
                int nc = col + dirs[k, 1];

                if (ChercherDepuis(nl, nc, mot, index + 1, positions, visite))
                    return true;
            }

            // Si aucun chemin ne marche, on annule cette case 
            visite[ligne, col] = false;
            return false;
        }






        // Mise à jour du plateau après suppression d’un mot (lettres qui “glissent”)
        public void Maj_Plateau(object objet)
        {
            if (objet == null) return;
            int[,] positions = (int[,])objet;
            int len = positions.GetLength(0);

            // On traite chaque colonne concernée
            for (int k = 0; k < len; k++)
            {
                int col = positions[k, 1];
                int ligneSuppr = positions[k, 0];
                // On fait descendre toutes les lettres au-dessus de la lettre supprimée
                for (int i = ligneSuppr; i > 0; i--)
                {
                    matrice[i, col] = matrice[i - 1, col];
                }
                matrice[0, col] = '-'; // case vide en haut
            }
        }
    }
}
