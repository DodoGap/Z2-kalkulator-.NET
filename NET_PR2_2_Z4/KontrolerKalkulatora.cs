using System;
using System.ComponentModel;
using System.Numerics;

namespace NET_PR2_2_Z4
{
    internal class KontrolerKalkulatora : INotifyPropertyChanged
    {
        private double? lewyArgument = null;
        private double? prawyArgument = null;
        private string? buforDziałania = null;
        private string? poprzednieDziałanie = null;
        private string? wynik = null;
        private bool flagaDziałania = false;

        public string Wynik
        {
            get => wynik;
            set
            {
                wynik = value;
                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("Wynik")
                    );
            }
        }

        public string Bufory
        {
            get
            {
                string etykietaPoprzedniegoDziałania = poprzednieDziałanie != null ? $"Poprzednie działanie: {poprzednieDziałanie}" : "";
                if (lewyArgument == null)
                    return etykietaPoprzedniegoDziałania;
                else if (buforDziałania == null)
                    return $"{lewyArgument} {etykietaPoprzedniegoDziałania}";
                else if (prawyArgument == null)
                    return $"{lewyArgument} {buforDziałania} {etykietaPoprzedniegoDziałania}";
                else
                    return $"{lewyArgument} {buforDziałania} {prawyArgument} = {etykietaPoprzedniegoDziałania}";
            }
        }

        internal void WprowadźCyfrę(string cyfra)
        {
            if (flagaDziałania)
                Wynik = null;
            if (Wynik == null)
                Wynik = cyfra;
            else
                Wynik += cyfra;
        }

        internal void ZmieńZnak()
        {
            if (flagaDziałania)
                Wynik = null;
            if (Wynik == "0")
                return;
            else if (Wynik[0] == '-')
                Wynik = Wynik[1..];
            else
                Wynik = "-" + Wynik;
        }

        internal void WprowadźPrzecinek()
        {
            if (flagaDziałania)
                Wynik = "0";
            if (Wynik.Contains(','))
                return;
            else
                Wynik += ",";
        }

        internal void SkasujZnak()
        {
            if (flagaDziałania)
                Wynik = null;
            if (Wynik == "0")
                return;
            else if (
                Wynik == "-0,"

            )
                Wynik = null;
            else
                Wynik = Wynik[..^1];
        }

        internal void WyczyśćWynik()
        {
            Wynik = null;
        }

        internal void WyczyśćWszystko()
        {
            WyczyśćWynik();
            lewyArgument = prawyArgument = null;
            buforDziałania = null;
            poprzednieDziałanie = null;
            OnPropertyChanged(nameof(Bufory));
        }

        internal void WprowadźDziałanieDwuargumentowe(string? działanie)
        {
            if (lewyArgument == null)
            {
                lewyArgument = Convert.ToDouble(Wynik);
                buforDziałania = działanie;
                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("Bufory")
                    );
                wynik = null;
            }
            else if (buforDziałania == null)
            {
                buforDziałania = działanie;
                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("Bufory")
                    );
                wynik = null;
            }
            else
            {
                prawyArgument = Convert.ToDouble(Wynik);
                /*PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("Bufory")
                    );*/
                WykonajDziałanie();
                //jakaś flaga?
                prawyArgument = null;
            }
        }

        internal void WprowadźDziałanie(string działanie)
        {
            if (flagaDziałania)
            {
                buforDziałania = działanie;
                OnPropertyChanged(nameof(Bufory));
                wynik = null;
            }
            else if (buforDziałania == null)
            {
                buforDziałania = działanie;
                OnPropertyChanged(nameof(Bufory));
                wynik = null;
            }
            else
            {
                prawyArgument = double.Parse(Wynik);
                WykonajDziałanie();
                poprzednieDziałanie = $"{lewyArgument} {buforDziałania} {prawyArgument}";
                prawyArgument = null;
            }
        }


        public void WykonajDziałanie()
        {
            if (prawyArgument == null)
                prawyArgument = double.Parse(Wynik);
            OnPropertyChanged(nameof(Bufory));
            switch (buforDziałania)
            {
                case "+":
                    Wynik = $"{lewyArgument + prawyArgument}";
                    break;
                case "-":
                    Wynik = $"{lewyArgument - prawyArgument}";
                    break;
                case "×":
                    Wynik = $"{lewyArgument * prawyArgument}";
                    break;
                case "÷":
                    Wynik = $"{lewyArgument / prawyArgument}";
                    break;
                case "xʸ":
                    Wynik = $"{Math.Pow(lewyArgument.Value, prawyArgument.Value)}";
                    break;
                case "%":
                    Wynik = $"{(lewyArgument / 100) * prawyArgument}";
                    break;
                case "logₓy":
                    Wynik = $"{Math.Log(lewyArgument.Value, prawyArgument.Value)}";
                    break;


            }
            lewyArgument = double.Parse(Wynik);
            flagaDziałania = true;
        }

        internal void WykonajDziałanieJednoargumentowe(string działanie)
        {
            if (lewyArgument == null)
                lewyArgument = double.Parse(Wynik);
            switch (działanie)
            {
                case "1/x":
                    lewyArgument = 1 / lewyArgument;
                    break;
                case "√":
                    lewyArgument = Math.Sqrt(lewyArgument.Value);
                    break;
                case "!x":
                    double wynikSilnia = 1;
                    for (double i = 2; i <= lewyArgument.Value; i++)
                        wynikSilnia *= i;
                    lewyArgument = wynikSilnia;
                    break;
                case "⌈x⌉":
                    lewyArgument = Math.Ceiling(lewyArgument.Value);
                    break;
                case "⌊x⌋":
                    lewyArgument = Math.Floor(lewyArgument.Value);
                    break;
                case "logₓ10":
                    lewyArgument = Math.Log(10, lewyArgument.Value);
                    break;

            }
            Wynik = $"{lewyArgument}";
            flagaDziałania = true;
            buforDziałania = null;
            prawyArgument = null;
            OnPropertyChanged(nameof(Bufory));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
