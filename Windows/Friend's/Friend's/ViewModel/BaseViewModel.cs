﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Friend_s.ViewModel
{
    public class BaseViewModel:ViewModelBase
    {
        public BaseViewModel()
        {
            UniversalSettingsCommand = new RelayCommand(UniversalSettingsRetriever);
            MessengerInstance.Register<NotificationMessage>(this,NotifyMe );
        }

        private string _themeColor;
        private string _userName;
        private bool _isLoading;
        
        public RelayCommand UniversalSettingsCommand { get; private set; }
        public virtual bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged();
            }
        }
        public string UserName
        {
            get { return _userName;}
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    RaisePropertyChanged("UserName");
                }
            }
        }
        public string ThemeColor
        {
            get { return _themeColor; }
            set { _themeColor = value; RaisePropertyChanged(()=>ThemeColor); }
        }
        public string CommandBarQuote { get; set; }
        public bool IsProgressBarEnabled { get; set; }
        public Visibility IsProgressBarVisibile { get; set; }

        private void RaisePropertyChangedBase()
        {
            RaisePropertyChanged(()=>ThemeColor);
            RaisePropertyChanged(()=>UserName);
            RaisePropertyChanged(()=>IsProgressBarEnabled);
            RaisePropertyChanged(()=>IsProgressBarVisibile);
        }

        private void UniversalSettingsRetriever()
        {
            try
            {
               var applicationData = Windows.Storage.ApplicationData.Current;
                var localsettings = applicationData.LocalSettings;
                if (localsettings.Values.ContainsKey("UserName"))
                    _userName = localsettings.Values["UserName"] as string;
                if (localsettings.Values.ContainsKey("ThemeColor"))
                {
                    _themeColor = localsettings.Values["ThemeColor"] as string;
                }
                else
                {
                    localsettings.Values.Add("ThemeColor", "#18BC9C");
                    _themeColor = localsettings.Values["ThemeColor"] as string;
                }
                IsProgressBarEnabled = false;
                IsProgressBarVisibile = Visibility.Collapsed;
                RaisePropertyChangedBase();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

            try
            {
                var quotes = new List<string>
                {
                    "Be Yourself",
                    "Move On",
                    "Free Yourself",
                    "Look Up :)",
                    "Dream Big",
                    "Start Living!",
                    "Define Yourself",
                    "Be Happy",
                    "Be Fearless",
                    "Accept Yourself",
                    "Trust Yourself",
                    "Stay Positive",
                    "Don't Stop",
                    "Enjoy Life"
                };

                var random = new Random();
                var number = random.Next(1, 13);

                CommandBarQuote = quotes[number];

                RaisePropertyChanged(() => CommandBarQuote);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

        }

        private void NotifyMe(NotificationMessage obj)
        {
            var notification = obj.Notification;
            double result;
            double.TryParse(notification, out result);

            if (result != 0) return;
            if (notification == "#18BC9C" || notification == "#BA4C63")
            {
                _themeColor = notification;
            }

            else if (notification == "ProgressBarEnable" || notification == "ProgressBarDisable")
            {
                if (notification == "ProgressBarEnable")
                {
                    IsProgressBarEnabled = true;
                    IsProgressBarVisibile = Visibility.Visible;
                }
                else
                {
                    IsProgressBarEnabled = false;
                    IsProgressBarVisibile = Visibility.Collapsed;
                }
            }

            else
            {
                _userName = notification;
            }
            RaisePropertyChangedBase();
        }
    }
}
