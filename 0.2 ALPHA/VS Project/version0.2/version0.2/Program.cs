using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zadatak64
{
    class Program
    {
        struct Predmet
        {
            public string Ime;
            public double ATK, DEF;
            public Predmet(string a, int aa, int bb)
            {
                Ime = a;
                ATK = aa;
                DEF = bb;
            }
        }
        class Takmicar
        {
            public string Ime;
            public double ATK, DEF, HP;
            public Predmet[] PlayerItem = new Predmet[3];
            public Takmicar(string a, double cc, Predmet aaa, Predmet bbb, Predmet ccc)
            {
                Ime = a;
                HP = cc;
                PlayerItem[0] = aaa;
                PlayerItem[1] = bbb;
                PlayerItem[2] = ccc;
            }
        }
        static void IspisSvihTakm(Takmicar[] Players) {
            for (int i = 0; i < 7; i = i + 2) {
                string s1 = string.Empty, s2 = string.Empty, s3 = string.Empty;
                if (Players[i].PlayerItem[0].Ime == "Mač") {
                    s1 = "        ";
                }
                if (Players[i].PlayerItem[1].Ime == "Mač") {
                    s2 = "        ";
                }
                if (Players[i].PlayerItem[2].Ime == "Mač") {
                    s3 = "        ";
                }
                Console.WriteLine();
                Console.WriteLine("\t Igrač broj {0} \t         Igrač broj {1}", i + 1, i + 2);
                Console.WriteLine("\t Ime : {0} \t         Ime : {1}", Players[i].Ime, Players[i+1].Ime);
                Console.WriteLine("\t Ukupan ATK : {0} \t Ukupan ATK : {1}", Players[i].ATK, Players[i+1].ATK);
                Console.WriteLine("\t Ukupan DEF : {0} \t Ukupan DEF : {1}", Players[i].DEF, Players[i+1].DEF);
                Console.WriteLine("\t HP : {0} \t         HP : {1}", Players[i].HP, Players[i+1].HP);
                Console.WriteLine("\t Item1 : {0} \t {2}Item1 : {1}", Players[i].PlayerItem[0].Ime, Players[i+1].PlayerItem[0].Ime,s1);
                Console.WriteLine("\t Item2 : {0} \t {2}Item2 : {1}", Players[i].PlayerItem[1].Ime, Players[i+1].PlayerItem[1].Ime,s2);
                Console.WriteLine("\t Item3 : {0} \t {2}Item3 : {1}", Players[i].PlayerItem[2].Ime, Players[i+1].PlayerItem[2].Ime,s3);
                Console.WriteLine();
            }
            Console.ReadKey();
        }
        static void SviItemi(ref Predmet[] Items)
        {
            for (int i = 0; i <= 49; i++)
            {
                if (i >= 0 && i < 10)
                {
                    Items[i] = new Predmet("Mač", 500, 100);
                    /*Items[i].Ime = "Mač";
                    Items[i].ATK = 500;
                    Items[i].DEF = 100;*/
                }
                else if (i >= 10 && i < 20)
                {
                    Items[i] = new Predmet("Magicni stap", 600, 0);
                    /*Items[i].Ime = "Magicni stap";-
                    Items[i].ATK = 600;
                    Items[i].DEF = 0;*/
                }
                else if (i >= 20 && i < 30)
                {
                    Items[i] = new Predmet("Sekira", 400, 300);
                    /*Items[i].Ime = "Sekira";
                    Items[i].ATK = 400;
                    Items[i].DEF = 300;*/
                }
                else if (i >= 30 && i < 40)
                {
                    Items[i] = new Predmet("Laki stit", 200, 600);
                    /*Items[i].Ime = "Laki stit";
                    Items[i].ATK = 200;
                    Items[i].DEF = 600;*/
                }
                else
                {
                    Items[i] = new Predmet("Teski stit", 0, 1200);
                    /*Items[i].Ime = "Teski stit";
                    Items[i].ATK = 0;
                    Items[i].DEF = 1000;*/
                }
            }
        }
        static void IspisItem(Predmet item)
        {
            Console.WriteLine("\t Ime : {0}", item.Ime);
        }
        static void IspisTakm(Takmicar[] plays, int redni)
        {
            Console.WriteLine("\t Igrač broj {0}", redni + 1);
            Console.WriteLine("\t Ime : {0}", plays[redni].Ime);
            Console.WriteLine("\t Ukupan ATK : {0}", plays[redni].ATK);
            Console.WriteLine("\t Ukupan DEF : {0}", plays[redni].DEF);
            Console.WriteLine("\t HP : {0}", plays[redni].HP);
            for (int i = 0; i <= 2; i++)
            {
                Console.WriteLine("\t Item {0}", i + 1);
                IspisItem(plays[redni].PlayerItem[i]);
            }
        }
        static Takmicar[] UnosTakmicara(Predmet[] Items)
        {
            Takmicar[] Players = new Takmicar[8];
            Boolean[] ItemsUsed = new Boolean[50];
            for (int i = 0; i <= 49; i++)
            {
                ItemsUsed[i] = false;
                if (i % 10 == 0)
                {
                    Console.WriteLine();
                }
            }
            for (int i = 0; i <= 7; i++)
            {
                Console.WriteLine();
                Console.WriteLine("\t Unesite {0}. igrača : ", i + 1);
                Console.Write("\t Ime : ");
                string s = Console.ReadLine();
                // Console.Write("ATK: ");
                // Players[i].ATK = Convert.ToInt32(Console.ReadLine());
                // Console.Write("DEF: ");
                // Players[i].DEF = Convert.ToInt32(Console.ReadLine());
                // Console.Write("HP: ");
                // Players[i].HP = Convert.ToInt32(Console.ReadLine());
                Random rnd = new Random();
                int x3 = rnd.Next(8000, 10001);
                while (x3 % 100 != 0)
                {
                    x3 = rnd.Next(8000, 10001);
                }
                Predmet[] nizitema = new Predmet[3];
                for (int z = 0; z < 3; z++)
                {
                    int itemnum = rnd.Next(50);
                    while (ItemsUsed[itemnum] == true)
                    {
                        itemnum = rnd.Next(50);
                    }
                    nizitema[z] = Items[itemnum];
                }
                Players[i] = new Takmicar(s, x3, nizitema[0], nizitema[1], nizitema[2]);
                Players[i].ATK = nizitema[0].ATK + nizitema[1].ATK + nizitema[2].ATK;
                Players[i].DEF = nizitema[0].DEF + nizitema[1].DEF + nizitema[2].DEF;
                Console.WriteLine("\t Igrac {0}, definisan!", i + 1);
                if (Players[i].ATK <= 0)
                {
                    x3 = rnd.Next(300, 1001);
                    while (x3 % 100 != 0)
                    {
                        x3 = rnd.Next(300, 1001);
                    }
                    Players[i].ATK = x3;
                }
                if (Players[i].DEF <= 0)
                {
                    x3 = rnd.Next(300, 1001);
                    while (x3 % 100 != 0)
                    {
                        x3 = rnd.Next(300, 1001);
                    }
                    Players[i].DEF = x3;
                }
                Console.WriteLine();
            }
            return Players;
        }
        static Boolean Bitka(Takmicar a1, Takmicar a2)
        {
            var xx = a1.HP;
            var yy = a1.DEF;
            var zz = a2.HP;
            var gg = a2.DEF;
            var hh = a1.ATK;
            var jj = a2.ATK;
            Random crit = new Random();
            Console.WriteLine();
            Console.WriteLine("         BITKA! Igrač {0} (- ATK : {1}, DEF : {2}, HP : {3})", a1.Ime, a1.ATK, a1.DEF, a1.HP);
            Console.WriteLine("         BITKA! Igrač {0} (- ATK : {1}, DEF : {2}, HP : {3})", a2.Ime, a2.ATK, a2.DEF, a2.HP);
            Console.WriteLine();
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("         Ko igra prvi?");
            Console.WriteLine();
            System.Threading.Thread.Sleep(2000);
            Random rnd = new Random();
            int n = rnd.Next(0, 2);
            if (n == 0)
            {
                Console.WriteLine("         Prvi igra Igrač 1 - {0}", a1.Ime);
            }
            else
            {
                Console.WriteLine("         Prvi igra Igrač 2 - {0}", a2.Ime);
            }
            System.Threading.Thread.Sleep(2000);
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine();
            }
            double damage;
            double h;
            int cc;
            System.Threading.Thread.Sleep(2000);
            while (a1.HP > 0 && a2.HP > 0)
            {
                if (n % 2 == 0)
                {
                    double decay = 0.95;
                    if (a1.DEF >= 1700) // IF HE IS TANK
                    {
                        a1.ATK = a1.ATK + a1.ATK * 0.05;
                        decay = 1;
                    }
                    double bonusatk = 0.95;
                    if (a1.ATK >= 1500) { // IF HE IS ATK
                        a1.DEF = a1.DEF + a1.DEF * 0.006;
                        bonusatk = 1;
                    }
                    //IF HE IS BALANCED DEF
                    if (a1.ATK <= 1000 && a1.DEF < 1700)
                    {
                        decay = 1.05;
                    }
                    //IF HE IS BALANCED ATK
                    if (a1.ATK <= 1500 && a1.ATK > 1000 && a1.DEF < 1500)
                    {
                        bonusatk = 0.90;
                    }
                    // POTEZ PRVOG
                    h = a2.HP;
                    cc = crit.Next(0, 101);
                    double pc = (a2.DEF * 0.7) / 3600;
                    damage = a1.ATK * (1 - pc);
                    a1.ATK = a1.ATK + a1.ATK*(1 - bonusatk);
                    if (damage < 0)
                    {
                        damage = 0;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\t {0} - MISS!", a1.Ime);
                        Console.ResetColor();
                    }
                    else if (cc % 6 == 0)
                    {
                        damage = damage * 1.25 + 100;
                        a1.ATK += 100;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\t {0} - CRIT!", a1.Ime);
                        Console.ResetColor();
                    }
                    else if (cc % 7 == 0)
                    {
                        damage = damage * 0;
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\t {0} - MISS!", a1.Ime);
                        Console.ResetColor();
                    }
                    else if (cc % 8 == 0)
                    {
                        damage = damage * 2 + 40;
                        a1.ATK += 40;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t {0} - CRIT!", a1.Ime);
                        Console.ResetColor();
                    }
                    else if (cc % 9 == 0)
                    {
                        damage = damage * 1.5 + 25;
                        a1.ATK += 25;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\t {0} - CRIT!", a1.Ime);
                        Console.ResetColor();
                    }
                    else if (cc % 10 == 0)
                    {
                        damage = damage * 0;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\t {0} - DODGE!", a2.Ime);
                        Console.ResetColor();
                    }
                    else if (n / 2 == 0 && n % 2 == 0)
                    {
                        damage = damage * 0.5;
                    }
                    a2.HP = a2.HP - damage;
                    a2.DEF = a2.DEF * decay;
                    Console.WriteLine("\t {2} udara za {0} dmg! {3}u ostaje {1} HP-a", Math.Round(h - a2.HP, 0), Math.Round(a2.HP, 0), a1.Ime, a2.Ime);
                }
                else
                {
                    double decay = 0.95;
                    if (a2.DEF >= 1700) // IF HE IS TANK
                    {
                        a2.ATK = a2.ATK + a2.ATK * 0.05;
                        decay = 1;
                    }
                    double bonusatk = 0.95;
                    if (a2.ATK >= 1500)
                    { // IF HE IS ATK
                        a2.DEF = a2.DEF + a2.DEF * 0.006;
                        bonusatk = 1;
                    }
                    //IF HE IS BALANCED DEF
                    if (a2.ATK <= 1000 && a2.DEF < 1700)
                    {
                        decay = 1.05;
                    }
                    //IF HE IS BALANCED ATK
                    if (a2.ATK <= 1500 && a2.ATK > 1000 && a2.DEF < 1500)
                    {
                        bonusatk = 0.90;
                    }
                    cc = crit.Next(0, 101);
                    h = a1.HP;
                    double pc = (a1.DEF * 0.7) / 3600;
                    // POTEZ DRUGOG
                    damage = a2.ATK * (1 - pc);
                    a2.ATK = a2.ATK + a2.ATK*(1 - bonusatk);
                    if (damage < 0)
                    {
                        damage = 0;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\t {0} - MISS!", a2.Ime);
                        Console.ResetColor();
                    }
                    else if (cc % 6 == 0)
                    {
                        damage = damage * 1.25 + 100;
                        a2.ATK += 100;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\t {0} - CRIT!", a2.Ime);
                        Console.ResetColor();
                    }
                    else if (cc % 7 == 0)
                    {
                        damage = damage * 0;
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\t {0} - MISS!", a2.Ime);
                        Console.ResetColor();
                    }
                    else if (cc % 8 == 0)
                    {
                        damage = damage * 2 + 40;
                        a2.ATK += 40;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t {0} - CRIT!", a2.Ime);
                        Console.ResetColor();
                    }
                    else if (cc % 9 == 0)
                    {
                        damage = damage * 1.5 + 25;
                        a2.ATK += 25;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\t {0} - CRIT!", a2.Ime);
                        Console.ResetColor();
                    }
                    else if (cc % 10 == 0)
                    {
                        damage = damage * 0;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\t {0} - DODGE!", a1.Ime);
                        Console.ResetColor();
                    }
                    else if (n / 2 == 0 && n % 2 == 1)
                    {
                        damage = damage * 0.5;
                    }
                    a1.HP = a1.HP - damage;
                    a2.DEF = a2.DEF * decay;
                    Console.WriteLine("\t {2} udara za {0} dmg! {3}u ostaje {1} HP-a", Math.Round(h - a1.HP, 0), Math.Round(a1.HP, 0), a2.Ime, a1.Ime);
                }
                System.Threading.Thread.Sleep(1500);
                n++;
            }
            if (a1.HP <= 0 && a2.HP > 0)
            {
                a1.HP = 0;
            }
            else if (a1.HP > 0 && a2.HP <= 0)
            {
                a2.HP = 0;
            }
            bool sh = false;
            Console.WriteLine("\t {0} HP vs {1} HP", Math.Round(a1.HP, 0), Math.Round(a2.HP, 0));
            if (a1.HP <= 0 && a2.HP > 0)
            {
                Console.WriteLine("\t {0} je pao, {1} pobedjuje!", a1.Ime, a2.Ime);
                sh = false;
            }
            else if (a1.HP > 0 && a2.HP <= 0)
            {
                Console.WriteLine("\t {0} je pao, {1} pobedjuje!", a2.Ime, a1.Ime);
                sh = true;
            }
            a1.HP = xx;
            a1.DEF = yy;
            a2.HP = zz;
            a2.DEF = gg;
            a1.ATK = hh;
            a2.ATK = jj;
            System.Threading.Thread.Sleep(2000);
            return sh;
        }
        static void Tournament(Takmicar[] Igrac)
        {
            Random rnd = new Random();
            Boolean[] JeRasp = new Boolean[8];
            for (int i = 0; i < 8; i++)
            {
                JeRasp[i] = false;
            }
            int[] Raspored = new int[8];
            int x = 0;
            for (int i = 0; i < 8; i++)
            {
                int n = rnd.Next(0, 7);
                while (JeRasp[n] == true)
                {
                    n = rnd.Next(8);
                }
                JeRasp[n] = true;
                Raspored[x] = n;
                x++;
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\t Parovi su ({0},{1}) - ({2},{3}) - ({4},{5}) - ({6},{7})", Igrac[Raspored[0]].Ime, Igrac[Raspored[1]].Ime, Igrac[Raspored[2]].Ime, Igrac[Raspored[3]].Ime, Igrac[Raspored[4]].Ime, Igrac[Raspored[5]].Ime, Igrac[Raspored[6]].Ime, Igrac[Raspored[7]].Ime);
            Console.ResetColor();
            Console.WriteLine();
            // RASPOREDNJIVANJE DO OVDE
            int j = 0;
            int[] Polufinale = new int[4];
            int p = 0;
            // PROLAZAK U POLUFINALE
            for (j = 0; j < 8; j = j + 2)
            {
                int n1 = Raspored[j];
                int n2 = Raspored[j + 1];
                bool kopro = Bitka(Igrac[n1], Igrac[n2]);
                if (kopro)
                {
                    Polufinale[p] = n1;
                    p++;
                }
                else
                {
                    Polufinale[p] = n2;
                    p++;
                }
            }
            System.Threading.Thread.Sleep(5000);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\t Kraj prvog kruga!");
            Console.WriteLine("\t Sledeci krug (polufinale) : ");
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("\t Prvi par - ({0},{1}) ---- Drugi par ({2},{3})", Igrac[Polufinale[0]].Ime, Igrac[Polufinale[1]].Ime, Igrac[Polufinale[2]].Ime, Igrac[Polufinale[3]].Ime);
            Console.ResetColor();
            System.Threading.Thread.Sleep(2000);
            System.Threading.Thread.Sleep(2000);
            System.Threading.Thread.Sleep(2000);
            // POLUFINALE
            int[] Finale = new int[2];
            int f = 0;
            for (int i = 0; i < 4; i = i + 2)
            {
                bool whowon = Bitka(Igrac[Polufinale[i]], Igrac[Polufinale[i + 1]]);
                if (whowon)
                {
                    Finale[f] = Polufinale[i];
                    f++;
                }
                else
                {
                    Finale[f] = Polufinale[i + 1];
                    f++;
                }
            }
            System.Threading.Thread.Sleep(2000);
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\t U finale su prosli - ({0},{1})", Igrac[Finale[0]].Ime, Igrac[Finale[1]].Ime);
            Console.ResetColor();
            Console.WriteLine();
            System.Threading.Thread.Sleep(2000);
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("\t FINALE!");
            bool finalewon = Bitka(Igrac[Finale[0]], Igrac[Finale[1]]);
            System.Threading.Thread.Sleep(2000);
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Green;
            if (finalewon)
            {
                Console.WriteLine("\t POBEDIO JE - {0}", Igrac[Finale[0]].Ime);
            }
            else
            {
                Console.WriteLine("\t POBEDIO JE - {0}", Igrac[Finale[1]].Ime);
            }
            Console.ResetColor();
            System.Threading.Thread.Sleep(2000);
            System.Threading.Thread.Sleep(2000);
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("\t Done !");
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            for(int i = 0; i < 4; i++) {
                Console.WriteLine();
            }
            Console.WriteLine("\t \t Tournament Game - Version {0}", 0.2);
            Predmet[] Items = new Predmet[50];
            SviItemi(ref Items);
            Takmicar[] Players = new Takmicar[8];
            Players = UnosTakmicara(Items);
            /*for (int i = 0; i < 8; i++)
            {
                IspisTakm(Players, i);
                Console.WriteLine();
                System.Threading.Thread.Sleep(2000);
            }*/
            IspisSvihTakm(Players);
            Tournament(Players);
            Console.ReadKey();

        }
    }
}
