﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;

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
	private bool
		flagaDziałania = false
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
	public string Bufory
	{
		get
		{
			if (lewyArgument == null)
				return "";
			else if (buforDziałania == null)
				return $"{lewyArgument}";
			else if (prawyArgument == null)
				return $"{lewyArgument} {buforDziałania}";
			else
				return $"{lewyArgument} {buforDziałania} {prawyArgument} =";
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
		if(flagaDziałania)
			Wynik= "0";
		if (Wynik == "0")
			return;
		else if (Wynik[0] == '-')
			Wynik = Wynik.Substring(1);
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
		PropertyChanged?.Invoke(
			this,
			new PropertyChangedEventArgs("Bufory")
			);
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
		else if(buforDziałania == null)
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

	public void WykonajDziałanie()
	{
		if(prawyArgument == null)
			prawyArgument = Convert.ToDouble(Wynik);
		PropertyChanged?.Invoke(
			this,
			new PropertyChangedEventArgs("Bufory")
			);
		if (buforDziałania == "+")
			Wynik = $"{lewyArgument + prawyArgument}";
		lewyArgument = Convert.ToDouble(Wynik);
		flagaDziałania = true;
	}

	internal void WykonajDziałanieJednoargumentowe(string? działanie)
	{
		if(lewyArgument == null)
			lewyArgument = Convert.ToDouble(Wynik);
		if (działanie == "1/x")
			lewyArgument = 1 / lewyArgument;
		Wynik = $"{lewyArgument}";
		flagaDziałania = true;
		buforDziałania = null;
		prawyArgument = null;
		PropertyChanged?.Invoke(
			this,
			new PropertyChangedEventArgs("Bufory")
			);
	}

	public event PropertyChangedEventHandler? PropertyChanged;
}
