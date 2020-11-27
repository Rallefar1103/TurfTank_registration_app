﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using TurfTankRegistrationApplication.Model;
using Xamarin.Forms;
using TurfTankRegistrationApplication.Pages;
using System.Collections.Generic;
using System.Linq;

namespace TurfTankRegistrationApplication.ViewModel
{
    public class RoverRegistrationViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public List<string> wifiResults { get; set; }
        public Command DidChangeRoverSimcard { get; }
        public Command DidChangeRoverSN { get;  }
        public Command ScanForWifi { get; }
        public Command ConnectToSelectedWifi { get; }
        public bool HasNotStartedLoading { get; set; } = true;
        public bool IsDoneLoading { get; set; }
        public bool WifiListIsReady { get; set; } = false;
        public string SelectedNetwork { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public Action<IWifiConnector, List<string>> Callback { get; set; }

        public RoverRegistrationViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            DidChangeRoverSimcard = new Command(() => NavigateToScanPage("Rover"));
            DidChangeRoverSN = new Command(() => NavigateToWifiPage());
            ScanForWifi = new Command(() => Scanner());
            ConnectToSelectedWifi = new Command(() => GetRoverSerialNumber());
            
        }

        public void NavigateToWifiPage()
        {
            ListWifiPage wifiListPage = new ListWifiPage();
            Navigation.PushAsync(wifiListPage);
        }

        
        public void NavigateToScanPage(string component)
        {
            ScanPage scanPage = new ScanPage();
            scanPage.vm.Title = "Scanning " + component;
            scanPage.QRMustContain = component;
            Navigation.PushAsync(scanPage);
        }

        public bool TryConnecting()
        {
            bool result = false;
            result = DependencyService.Get<IWifiConnector>().ConnectToWifi();
            return result;
        }

        // Needs to wait for the result to finish
        public async void Scanner()
        {
            Task<List<string>> wifiTask = DependencyService.Get<IWifiConnector>().GetAvailableNetworks();
            HasNotStartedLoading = false;
            OnPropertyChanged(nameof(HasNotStartedLoading));

            IsDoneLoading = true;
            OnPropertyChanged(nameof(IsDoneLoading));

            await Task.Delay(8000);
            if (wifiTask.Status == TaskStatus.RanToCompletion)
            {
                Console.WriteLine("DONE");
                IsDoneLoading = false;
                OnPropertyChanged(nameof(IsDoneLoading));

                wifiResults = wifiTask.Result;
                OnPropertyChanged(nameof(wifiResults));

                foreach (var network in wifiResults)
                {
                    Console.WriteLine("Result: " + network);
                }

                WifiListIsReady = true;
                OnPropertyChanged(nameof(WifiListIsReady));

            } else if (wifiTask.Status == TaskStatus.Running)
            {

                Console.WriteLine("RUNNING");

            } else if (wifiTask.Status == TaskStatus.WaitingToRun)
            {
                Console.WriteLine("WAITING");
            }

        }

        public string GetCorrectWifi(string ssid)
        {
            ssid.Trim();
            SelectedNetwork = wifiResults.Find(wifi => wifi == ssid);
            OnPropertyChanged(nameof(SelectedNetwork));
            return SelectedNetwork;
        }

        // We might need to move this logic to the Model
        public async void GetRoverSerialNumber()
        {
            string ssid = GetCorrectWifi(SelectedNetwork);
            bool gotConnected = false;
            Console.WriteLine("Start connecting to wifi: " + ssid);
            gotConnected = TryConnecting();
            if (gotConnected)
            {
                Console.WriteLine("WE ARE CONNECTED!");
                await Application.Current.MainPage.DisplayAlert("Success!", "You are connected to: " + ssid, "OK");
                RemovePreviousPage();

            } else
            {
                Console.WriteLine("OOPS WE END UP HERE!");
            }
            MessagingCenter.Send(this, "RoverSerialNumber", "Rover1234");
            
        }

        private async void RemovePreviousPage()
        {
            var listWifiPage = Navigation.NavigationStack.FirstOrDefault(p => p is ListWifiPage);
            if (listWifiPage != null)
            {
                Navigation.RemovePage(listWifiPage);
            }
            await Navigation.PopAsync();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
