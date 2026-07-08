namespace sparplaner
{
    using System;
    public class Sparziel
    {
        public string name { get; set; } = "";
        public decimal Zielbetrag { get; set; }
        public decimal Gespart { get; set; }
        public decimal MonatlicheRate { get; set; }
        public decimal NochOffen()
        {
            return Math.Max(0, Zielbetrag - Gespart);
        }

        public double Prozent()
        {
            if (Zielbetrag <= 0) return 0;
            double prozent = (double)(Gespart / Zielbetrag) * 100;
            return Math.Min(prozent, 100);
        }


        public bool IstErreicht()
        {
            return Gespart >= Zielbetrag;
        }
        public int MonateBisZiel()
        {
            if (IstErreicht())
                return 0;

            if (MonatlicheRate <= 0)
                return -1;

            return (int)Math.Ceiling(NochOffen() / MonatlicheRate);
        }

        public DateTime? Zielmonat()
        {
            int monate = MonateBisZiel();

            if (monate < 0)
                return null;

            return DateTime.Now.AddMonths(monate);
        }
    }
}



