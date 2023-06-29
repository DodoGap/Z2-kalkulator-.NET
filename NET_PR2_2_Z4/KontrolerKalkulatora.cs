using System;
using System.ComponentModel;

namespace NET_PR2_2_Z4
{
    internal class KontrolerKalkulatora : INotifyPropertyChanged
    {
        private double? lewyArgument = null;
        private double? prawyArgument = null;
        private string? buforDziałania = null;
        private string? poprzednieDziałanie = null;
        private string? wynik = "0";
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
                Wynik = "0";
            if (Wynik == "0")
                Wynik = cyfra;
            else
                Wynik += cyfra;
        }

        internal void ZmieńZnak()
        {
            if (flagaDziałania)
                Wynik = "0";
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
                Wynik = "0";
            if (Wynik == "0")
                return;
            else if (
                Wynik == "-0,"

            )
                Wynik = "0";
            else
                Wynik = Wynik[..^1];
        }

        internal void WyczyśćWynik()
        {
            Wynik = "0";
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
                wynik = "0";
            }
            else if (buforDziałania == null)
            {
                buforDziałania = działanie;
                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("Bufory")
                    );
                wynik = "0";
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
                wynik = "0";
            }
            else if (buforDziałania == null)
            {
                buforDziałania = działanie;
                OnPropertyChanged(nameof(Bufory));
                wynik = "0";
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
                case "*":
                    Wynik = $"{lewyArgument * prawyArgument}";
                    break;
                case "/":
                    Wynik = $"{lewyArgument / prawyArgument}";
                    break;
                case "^":
                    Wynik = $"{Math.Pow(lewyArgument.Value, prawyArgument.Value)}";
                    break;
                case "%":
                    Wynik = $"{lewyArgument % prawyArgument}";
                    break;
                case "% (procent)":
                    Wynik = $"{(lewyArgument / 100) * prawyArgument}";
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
                case "sqrt":
                    lewyArgument = Math.Sqrt(lewyArgument.Value);
                    break;
                case "sin":
                    lewyArgument = Math.Sin(lewyArgument.Value);
                    break;
                case "cos":
                    lewyArgument = Math.Cos(lewyArgument.Value);
                    break;
                case "tan":
                    lewyArgument = Math.Tan(lewyArgument.Value);
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
