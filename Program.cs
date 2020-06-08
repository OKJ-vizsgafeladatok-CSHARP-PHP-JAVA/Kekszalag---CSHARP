using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KekszalagCSHARP
{
    class Hajo
    {
        public int helyezes { get; set; }
        public string kategoria { get; set; }
        public string hajo { get; set; }
        public string klub { get; set; }
        public string kormanyos { get; set; }
        public int nap { get; set; }
        public int ora { get; set; }
        public int perc { get; set; }

        public Hajo(int helyezes, string kategoria, string hajo, string klub, string kormanyos, int nap, int ora, int perc)
        {
            this.helyezes = helyezes;
            this.kategoria = kategoria;
            this.hajo = hajo;
            this.klub = klub;
            this.kormanyos = kormanyos;
            this.nap = nap;
            this.ora = ora;
            this.perc = perc;
        }
    }

    class Program
    {
        public static List<Hajo> lista = beolvas();
        public static List<Hajo> beolvas()
        {
            List<Hajo> list = new List<Hajo>();
            try
            {
                using (StreamReader sr=new StreamReader(new FileStream("kekszalag.csv",FileMode.Open),Encoding.UTF8))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var sor = sr.ReadLine().Split(';');
                        Hajo o = new Hajo(
                                Convert.ToInt32(sor[0]),
                                sor[1],
                                sor[2],
                                sor[3],
                                sor[4],
                                Convert.ToInt32(sor[5]),
                                Convert.ToInt32(sor[6]),
                                Convert.ToInt32(sor[7])
                            );
                        list.Add(o);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba a beolvasásnál. "+e);
            }
            return list;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("1. feladat: \n\tA beolvasás és a tárolás kész!");
            #region 2. feladat
            Console.WriteLine("2. feladat: \n\tÖsszesen {0} adatot tartalmaz az állomány",lista.Count());
            #endregion
            #region 3. feladat
            Console.WriteLine("3. feladat:");
            var elsotiz = lista
                .Where(x => x.helyezes <= 10)
                .Select(y => new
                    {
                        helyezes=y.helyezes,
                        hajonev = y.hajo,
                        klub = y.klub,
                        perc = (y.nap * 24 * 60) + (y.ora * 60) + y.perc
                    }
                ).ToList();
            elsotiz.ForEach(x =>
                Console.WriteLine($"\t{x.helyezes}. {x.hajonev} - {x.perc}")
            );
            #endregion

            #region 4. feladat
            Console.WriteLine("4. feladat:\nA verseny kategóriái:");
            var katList = lista.GroupBy(x => x.kategoria).ToList();
            katList.ForEach(x=>
                Console.WriteLine("\t"+x.Key)
            );
            Console.WriteLine("\tÖsszesen {0} kategória szerepel az adatok között.",katList.Count());
            #endregion

            #region 5. feladat
            Console.WriteLine("5. feladat: \nAz első három hajó átlagsebessége:");
            var atlList = lista
                .Where(x => x.helyezes < 4)
                .Select(y => new {
                    helyezes=y.helyezes,
                    atlagseb= 
                        Math.Round(
                              (
                                   160/
                                   ((double)((y.nap * 24 * 60) + (y.ora * 60) + y.perc)
                                   / (double)60)
                              )
                              ,1)
                }).ToList();

            atlList.ForEach(x=>
                Console.WriteLine("\t"+x.helyezes+". "+x.atlagseb+" km/h")
            );
            #endregion

            #region 6. feladat
            Console.WriteLine("6. feladat:");
            var katamaran = (7 * 60) + 13;
            var kekszalagMax = elsotiz.First().perc;
            Console.WriteLine("\tA leggyorsabb hajó {0} perccel maradt el az abszolút rekordtól.",kekszalagMax-katamaran);
            #endregion

            #region 7. feladat
            var hajonevek = lista
            .Select(y => new
            {
                nev = y.hajo,
                klub = y.klub,
                ido = y.nap + ":" + y.ora + ":" + y.perc
            })
            .ToList();
            using (StreamWriter sw=new StreamWriter(new FileStream("hajonevek.txt",FileMode.Create),Encoding.UTF8))
            {
                hajonevek.ForEach(x=>
                        sw.WriteLine(x.nev+";"+x.klub+";"+x.ido)
                    );
            }
            Console.WriteLine("7. feladat:");
            Console.WriteLine("\tA fájlba írás sikeresen megtörtént!");
            #endregion

            Console.ReadKey();
        }
    }
}
