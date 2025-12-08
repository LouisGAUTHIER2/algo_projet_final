using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algo_projet_final
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // test Dictionnaire
            Dictionnaire dico = new Dictionnaire("./Mots_Français.txt","Français");
            dico.Tri_quick_sort();
            //dico.printMots();

            Console.WriteLine($"On vérifie si ZUT existe : {dico.RechDichoRecursif("zut")}");
            Console.WriteLine($"On vérifie si TETRTT existe : {dico.RechDichoRecursif("TETRTT")}");
        }
    }
}
