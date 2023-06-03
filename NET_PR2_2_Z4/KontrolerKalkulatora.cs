using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET_PR2_2_Z4;
internal class KontrolerKalkulatora : INotifyPropertyChanged
{
	private double?
		lewyArgument = null,
		prawyArgument = null
		;
	private string?
		buforDziałania = null,
		wynik = "0"
		;

	public string Wynik {
		get => wynik;
		set { 
			wynik = value;
			PropertyChanged?.Invoke(
				this,
				new PropertyChangedEventArgs("Wynik")
				);
		}
	}
	internal void WprowadźCyfrę(string cyfra)
	{
		if (Wynik == "0")
			Wynik = cyfra;
		else
			Wynik += cyfra;
	}
	internal void ZmieńZnak()
	{
		if (Wynik == "0")
			return;
		else if (Wynik[0] == '-')
			Wynik = Wynik.Substring(1);
		else
			Wynik = "-" + Wynik;
	}
	internal void WprowadźPrzecinek()
	{
		if (Wynik.Contains(','))
			return;
		else
			Wynik += ",";
	}

	internal void SkasujZnak()
	{
		if (Wynik == "0")
			return;
		else if (
			Wynik == "-0,"
			|| Wynik.Length == 1
			|| (Wynik.Length == 2 && Wynik[0] == '-')
			)
			Wynik = "0";
		else
			Wynik = Wynik.Substring(0,Wynik.Length-1);
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
	}

	public event PropertyChangedEventHandler? PropertyChanged;
}
