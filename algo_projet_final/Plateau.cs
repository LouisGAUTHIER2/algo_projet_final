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

        public int NbLignes { get { return lignes; } }
        public int NbColonnes { get { return colonnes; } }

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
                int newligne = 0;
                int newcolonne = 0;
                string[] lignesv2 = new string[100]; // suppose max 100 lignes

                while ((ligne = sr.ReadLine()) != null)
                {
                    lignesv2[newligne] = ligne;
                    newligne++;
                }
                if (newligne > 0)
                    newcolonne = lignesv2[0].Split(';').Length;

                lignes = newligne;
                colonnes = newcolonne;
                matrice = new char[lignes, colonnes];

                for (int i = 0; i < lignes; i++)
                {
                    string[] casesLigne = lignesv2[i].Split(';');
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
       

        // Recherche un mot sur le plateau 
        public int[,] Recherche_Mot(string mot)
        {
            if (mot == "") return null;
            
            mot = mot.ToUpper();
            int[,] positions = new int[mot.Length, 2];
            bool[,] visite = new bool[lignes, colonnes];

            int ligneBase = lignes - 1; //  base du plateau

            for (int j = 0; j < colonnes; j++)
            {
                if (matrice[ligneBase, j] == mot[0])
                {
                    if (RechercheRec(ligneBase, j, mot, 0, positions, visite))
                        return positions;
                }
            }

            return null;
        }


        private bool RechercheRec(int i, int j, string mot, int index,
                          int[,] pos, bool[,] présentoupas)
        {
            // mot déjà trouvé
            if (index == mot.Length)
                return true;

            // hors limites
            if (i < 0 || i >= lignes || j < 0 || j >= colonnes)
                return false;

      
            // lettre incorrecte
            if (matrice[i, j] != mot[index])
                return false;

            // marquer la case qu'on vient de faire
            présentoupas[i, j] = true;
            pos[index, 0] = i;
            pos[index, 1] = j; 

            // déplacementq sur la grille
            if (RechercheRec(i - 1, j, mot, index + 1, pos, présentoupas) ||
                RechercheRec(i + 1, j, mot, index + 1, pos, présentoupas) ||
                RechercheRec(i, j - 1, mot, index + 1, pos, présentoupas) ||
                RechercheRec(i, j + 1, mot, index + 1, pos, présentoupas))
            {
                return true;
            }

            // retour arrière
            présentoupas[i, j] = false;
            return false;
        }



        // Met à jour le plateau après suppression d’un mot
        public void Maj_Plateau(int[,] positions)
        {
            if (positions == null) return;

            int len = positions.GetLength(0);

            for (int k = 0; k < len; k++)
            {
                int ligneSuppr = positions[k, 0];
                int col = positions[k, 1];

                // Faire descendre toutes les lettres au-dessus
                for (int i = ligneSuppr; i > 0; i--)
                    matrice[i, col] = matrice[i - 1, col];

                matrice[0, col] = '-'; // case vide en haut
            }
        }

    }
}
