﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using TurfTankRegistrationApplication.Model;
using Xamarin.Forms;
using TurfTankRegistrationApplication.Pages;

namespace TurfTankRegistrationApplication.ViewModel
{
    public interface IRegistrateRobot
    {
        void NavigateToScanPage();
        void RegistrateChassis();
        void RegistrateControllerAsync();
        void RegistrateRover();
        void RegistrateBase();
        void RegistrateTablet();
        void SaveRobot();
    }

    public class RobotRegistrationViewModel : INotifyPropertyChanged, IRegistrateRobot
    {
        public RobotPackage robotItem = new RobotPackage();

        public string ChassisSN { get; set; } = "";
        public string ControllerSN { get; set; } = "";
        public string RoverSN { get; set; } = "";
        public string BaseSN { get; set; } = "";
        public string TabletSN { get; set; } = "";

        public INavigation Navigation { get; set; }

        public RobotRegistrationViewModel(INavigation navigation)
        {
            robotItem.Controller.ID = "123456";
            robotItem.SerialNumber = "7891012";
            robotItem.RoverGPS.ID = "13141516";
            robotItem.BaseGPS.ID = "17181920";
            robotItem.Tablet.ID = "212223345";

            this.Navigation = navigation;
            robotItem.SetAsSelected();

            DidChangeChassisSN = new Command(NavigateToScanPage);
            DidChangeControllerSN = new Command(NavigateToScanPage);
            DidChangeRoverSN = new Command(RegistrateRover);
            DidChangeBaseSN = new Command(RegistrateBase);
            DidChangeTabletSN = new Command(RegistrateTablet);

            MessagingCenter.Subscribe<ScanPage, string>(this, "Result", (sender, data) =>
            {
                if (data.Contains("https://"))
                {
                    ChassisSN = data;
                    OnPropertyChanged(nameof(ChassisSN));

                }
                else if (data.Contains("SSID"))
                {
                    ControllerSN = data;
                    OnPropertyChanged(nameof(ControllerSN));
                }
            });

        }

        public Command DidChangeChassisSN { get; }
        public Command DidChangeControllerSN { get; }
        public Command DidChangeRoverSN { get; }
        public Command DidChangeBaseSN { get; }
        public Command DidChangeTabletSN { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NavigateToScanPage()
        {
            Navigation.PushAsync(new ScanPage());
        }

        public void RegistrateChassis()
        {
            
        }

        public async void RegistrateControllerAsync()
        {
            ControllerSN = robotItem.Controller.GetSerialNumber();
            if (ControllerSN.Length > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Success!", "Controller Scanned", "Ok");
                OnPropertyChanged(nameof(ControllerSN));
            }
            else
            {
                Console.WriteLine("Error retrieving Controller SN");
            }
        }

        public async void RegistrateRover()
        {
            RoverSN = robotItem.RoverGPS.GetSerialNumber();
            if (RoverSN.Length > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Success!", "Rover Scanned", "Ok");
                OnPropertyChanged(nameof(RoverSN));
            }
            else
            {
                Console.WriteLine("Error retrieving Rover SN");
            }
        }

        public async void RegistrateBase()
        {
            BaseSN = robotItem.BaseGPS.GetSerialNumber();
            if (BaseSN.Length > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Success!", "Base Scanned", "Ok");
                OnPropertyChanged(nameof(BaseSN));
            }
            else
            {
                Console.WriteLine("Error retrieving Base SN");
            }
        }

        public async void RegistrateTablet()
        {
            TabletSN = robotItem.Tablet.GetSerialNumber();
            if (TabletSN.Length > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Success!", "Tablet Scanned", "Ok");
                OnPropertyChanged(nameof(TabletSN));
            }
            else
            {
                Console.WriteLine("Error retrieving Tablet SN");
            }
        }

        public void SaveRobot()
        {
            // Not implemented
        }
    }
}
