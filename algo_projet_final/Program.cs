using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace algo_projet_final
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            TerminalClass terminal = new TerminalClass();

            Console.Write($"{terminal.SetTextColor(255, 255, 0)}{terminal.SetBold()}Bienvenu veuillez rentrer vos {terminal.SetUnderline()}noms\n{terminal.ResetEffect()}");

            List<Joueur> joueurs = new List<Joueur>();

            Console.Write("Entrez le nom du premier joueur : ");
            string nom1 = Console.ReadLine();

            Joueur joueur1 = new Joueur(nom1);
            joueurs.Add(joueur1);

            string nom2;
            do
            {
                Console.Write("Entrez le nom du second joueur : ");
                nom2 = Console.ReadLine();
                if (nom2.ToLower() == nom1.ToLower())
                {
                    Console.WriteLine("Ce nom est déjà pris, choisissez-en un autre.");
                }
            }
            while (nom2.ToLower() == nom1.ToLower());


            Console.ReadKey();
        }
    }
}
