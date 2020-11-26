﻿using Xamarin.Forms;
using TurfTankRegistrationApplication.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TurfTankRegistrationApplication
{
	public interface IWifiConnector
	{
		bool ConnectToWifi();
		List<string> GetAvailableNetworks();
	};

	public class App : Application
	{
		
		public App()
		{
			// dette skal med fordi at Markup stadig er experimental.
			Device.SetFlags(new string[] { "Markup_Experimental" });
			MainPage = new NavigationPage (new SignInPage());
		}

		protected override void OnStart()
		{
			System.Console.WriteLine("\n*************\nProgram will start up now enjoy the ride!\n****************\n");
		}

		protected override void OnSleep()
		{
			System.Console.WriteLine("\n*************\nYour tablet is sooooo tired and will go take a morfar nap!\n****************\n");
		}

		protected override void OnResume()
		{
			System.Console.WriteLine("\n*************\nHey there, Im Freshhhh!\n****************\n");
		}

	}
}

