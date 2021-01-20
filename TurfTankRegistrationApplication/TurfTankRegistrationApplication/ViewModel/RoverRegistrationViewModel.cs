﻿using System;
using System.Net.Http;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using TurfTankRegistrationApplication.Model;
using Xamarin.Forms;
using TurfTankRegistrationApplication.Pages;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using TurfTankRegistrationApplication.Helpers;
using System.Net;
using System.Text;
using System.IO;

namespace TurfTankRegistrationApplication.ViewModel
{
    public class RoverRegistrationViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public HttpClient http { get; set; }
        public bool testing { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public Action<object, string> RoverQRCallback;
        public Color RoverQRButtonColor { get; set; } = Color.Yellow;

        public Command ChangeRoverSimcard { get; }
        public Command ChangeRoverSN { get; }
        

        public RoverRegistrationViewModel(INavigation navigation)
        {
            this.testing = false;
            this.http = App.WifiClient;
            this.Navigation = navigation;
            ChangeRoverSimcard = new Command(() => GetRoverSimcard("rover"));
            ChangeRoverSN = new Command(() =>  NavigateToRoverSN());
            RoverQRCallback = new Action<object, string>(ScanCallback);
            MessagingCenter.Subscribe<ScanPage, string>(this, "Result", RoverQRCallback);

        }

        public RoverRegistrationViewModel(INavigation navigation, HttpClient http)
        {
            this.testing = true;
            this.http = http;
            this.Navigation = navigation;
        }


        private void ScanCallback(object sender, string data)
        {
            if (data != null)
            {
                RoverQRButtonColor = Color.DarkGreen;
                OnPropertyChanged(nameof(RoverQRButtonColor));
            }
            MessagingCenter.Unsubscribe<ScanPage, string>(this, "Result");
        }

        public void GetRoverSimcard(string component)
        {
            ScanPage scanPage = new ScanPage(returntypeKey: "Result");
            scanPage.vm.Title = "Scanning " + component;
            scanPage.QRMustContain = component;
            Navigation.PushAsync(scanPage);
        }

        public void NavigateToRoverSN()
        {
            RoverSerialNumberPage roverSNPage = new RoverSerialNumberPage();
            Navigation.PushAsync(roverSNPage);
        }

        
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}



//Test
//var response = await http.GetAsync("https://jsonplaceholder.typicode.com/users");
//if (response.IsSuccessStatusCode)
//{
//    string StringContent = await response.Content.ReadAsStringAsync();
//    dynamic json = JsonConvert.DeserializeObject(StringContent);
//    Console.WriteLine("!!!!!!!! ------- THIS IS WHAT WE GOT: \n" + json);

//    RoverResponse = json[0]["name"];
//    Console.WriteLine("THIS IS ROVER RESPONSE: \n" + RoverResponse);
//    await Application.Current.MainPage.DisplayAlert("Success!", "Got Serial Number for rover: " + RoverResponse, "Add to Robot");
//    await Navigation.PopAsync();
//}
//else
//{
//    await Application.Current.MainPage.DisplayAlert("ERROR!", "Could not retrieve serial number from rover", "OK");
//    Console.WriteLine("BAD RESPONSE CODE!!!!!");
//}