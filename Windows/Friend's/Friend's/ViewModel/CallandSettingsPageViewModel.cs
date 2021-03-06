﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Contacts;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Friend_s.ViewModel
{
    public class CallandSettingsPageViewModel : BaseViewModel
    {
        public RelayCommand LocalStorageSettingsRetrieverCommand { get; private set; }
        public RelayCommand PasswordVaultRetrieverCommand { get; private set; }
        public RelayCommand<object> EditContactButtonHandlerCommand { get; private set; }
        public RelayCommand<object> SocialIntegrationRemoverCommand { get; }
        public RelayCommand ToastToggledCommand { get; private set; }
        public RelayCommand ThemeToggledCommand { get; private set; }
        public RelayCommand SliderValueChangedCommand { get; private set; }

        public CallandSettingsPageViewModel()
        {
            LocalStorageSettingsRetrieverCommand = new RelayCommand(LocalStorageSettingsRetriever);
            PasswordVaultRetrieverCommand = new RelayCommand(PasswordVaultRetrieverMethod);
            EditContactButtonHandlerCommand = new RelayCommand<object>(EditContactButtonHandler);
            SocialIntegrationRemoverCommand = new RelayCommand<object>(PasswordVaultRemoverMethod);
            ToastToggledCommand = new RelayCommand(ToastMakerToggledButton);
            ThemeToggledCommand = new RelayCommand(ThemeChangerToggledButton);
            SliderValueChangedCommand = new RelayCommand(SliderValueControllerMethod);
        }


        private string _themeColor;
        private string _notificationStatus;
        private string FacebookConnected { get; set; }
        private string TwitterConnected { get; set; }
        public string FirstContactName { get; private set; }
        public string SecondContactName { get; private set; }
        public string ThirdContactName { get; private set; }
        public bool ToggleSwitchIsOn { get; private set; }
        public bool ToastToggleSwitchIsOn { get; private set; }
        private bool IsAppFirstTimeOn { get; set; }
        public Visibility TwitterPlusIconVisibility { get; private set; }
        public Visibility TwitterRemoveIconVisibility { get; private set; }
        public double SliderValue { get; set; }


        private void LocalStorageSettingsRetriever()
        {
            MessengerInstance.Send(new NotificationMessage("ProgressBarEnable"));
            try
            {
                var applicationData = ApplicationData.Current;
                var localsettings = applicationData.LocalSettings;
                if (localsettings.Values == null) return;
                if (localsettings.Values.ContainsKey("FirstContactName"))
                    FirstContactName = localsettings.Values["FirstContactName"] as string;
                if (localsettings.Values.ContainsKey("SecondContactName"))
                    SecondContactName = localsettings.Values["SecondContactName"] as string;
                if (localsettings.Values.ContainsKey("ThirdContactName"))
                    ThirdContactName = localsettings.Values["ThirdContactName"] as string;
                if (localsettings.Values.ContainsKey("FacebookConnect"))
                    FacebookConnected = localsettings.Values["FacebookConnect"] as string;
                if (localsettings.Values.ContainsKey("TwitterConnect"))
                    TwitterConnected = localsettings.Values["TwitterConnect"] as string;
                if (localsettings.Values.ContainsKey("ThemeColor"))
                    _themeColor = localsettings.Values["ThemeColor"] as string;
                if (localsettings.Values.ContainsKey("ToastNotification"))
                    _notificationStatus = localsettings.Values["ToastNotification"] as string;
                if (localsettings.Values.ContainsKey("TimerTime"))
                    SliderValue = (double) localsettings.Values["TimerTime"];


                if (_themeColor == "#18BC9C")
                {
                    ToggleSwitchIsOn = false;
                }
                else if (_themeColor == "#BA4C63")
                {
                    ToggleSwitchIsOn = true;
                    IsAppFirstTimeOn = true;
                }
                if (_notificationStatus == "Off")
                {
                    ToastToggleSwitchIsOn = false;
                }
                else if (_notificationStatus == "On")
                {
                    ToastToggleSwitchIsOn = true;
                }

                RaisePropertyChanged(() => FirstContactName);
                RaisePropertyChanged(() => SecondContactName);
                RaisePropertyChanged(() => ThirdContactName);
                RaisePropertyChanged(() => ToggleSwitchIsOn);
                RaisePropertyChanged(() => ToastToggleSwitchIsOn);
                RaisePropertyChanged(() => SliderValue

                    );
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            MessengerInstance.Send(new NotificationMessage("ProgressBarDisable"));
        }

        private async void EditContactButtonHandler(object parameter)
        {
            var applicationData = ApplicationData.Current;
            var localsettings = applicationData.LocalSettings;
            switch (int.Parse(parameter.ToString()))
            {
                case 1:
                    var contactPicker = new ContactPicker();
                    // Ask the user to pick contact phone numbers.
                    contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
                    var contacts = await contactPicker.PickContactAsync();
                    if (!localsettings.Values.ContainsKey("FirstContactName"))
                    {
                        localsettings.Values.Add("FirstContactName", contacts.DisplayName);
                        localsettings.Values.Add("FirstContactNumber", contacts.YomiDisplayName);
                        FirstContactName = contacts.DisplayName;
                        RaisePropertyChanged(() => FirstContactName);
                    }
                    else
                    {
                        localsettings.Values.Remove("FirstContactName");
                        localsettings.Values.Remove("FirstContactNumber");
                        localsettings.Values.Add("FirstContactName", contacts.DisplayName);
                        localsettings.Values.Add("FirstContactNumber", contacts.YomiDisplayName);
                        FirstContactName = contacts.DisplayName;
                        RaisePropertyChanged(() => FirstContactName);

                    }
                    break;

                case 2:
                    var contactPicker1 = new ContactPicker();
                    // Ask the user to pick contact phone numbers.
                    contactPicker1.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
                    var contacts1 = await contactPicker1.PickContactAsync();
                    if (!localsettings.Values.ContainsKey("SecondContactName"))
                    {
                        localsettings.Values.Add("SecondContactName", contacts1.DisplayName);
                        localsettings.Values.Add("SecondContactNumber", contacts1.YomiDisplayName);
                        SecondContactName = contacts1.DisplayName;
                        RaisePropertyChanged(() => SecondContactName);
                    }
                    else
                    {
                        localsettings.Values.Remove("SecondContactName");
                        localsettings.Values.Remove("SecondContactNumber");
                        localsettings.Values.Add("SecondContactName", contacts1.DisplayName);
                        localsettings.Values.Add("SecondContactNumber", contacts1.YomiDisplayName);
                        SecondContactName = contacts1.DisplayName;
                        RaisePropertyChanged(() => SecondContactName);
                    }
                    break;

                case 3:
                    var contactPicker2 = new ContactPicker();
                    // Ask the user to pick contact phone numbers.
                    contactPicker2.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
                    var contacts2 = await contactPicker2.PickContactAsync();
                    if (!localsettings.Values.ContainsKey("ThirdContactName"))
                    {
                        localsettings.Values.Add("ThirdContactName", contacts2.DisplayName);
                        localsettings.Values.Add("ThirdContactNumber", contacts2.YomiDisplayName);
                        ThirdContactName = contacts2.DisplayName;
                        RaisePropertyChanged(() => ThirdContactName);
                    }
                    else
                    {
                        localsettings.Values.Remove("ThirdContactName");
                        localsettings.Values.Remove("ThirdContactNumber");
                        localsettings.Values.Add("ThirdContactName", contacts2.DisplayName);
                        localsettings.Values.Add("ThirdContactNumber", contacts2.YomiDisplayName);
                        ThirdContactName = contacts2.DisplayName;
                        RaisePropertyChanged(() => ThirdContactName);
                    }
                    break;

                default:
                    Debug.WriteLine("Default Case Hit!");
                    break;
            }
        }

        private void PasswordVaultRemoverMethod(object obj)
        {
            switch (int.Parse(obj.ToString()))
            {
                case 1:
                    var vault = new PasswordVault();
                    try
                    {
                        var credentialList = vault.FindAllByUserName("Twitter");
                        if (credentialList.Count <= 0) return;
                        var credential = vault.Retrieve("Friend", "Twitter");
                        vault.Remove(new PasswordCredential("Friend", "Twitter", credential.Password));
                        TwitterPlusIconVisibility = Visibility.Visible;
                        TwitterRemoveIconVisibility = Visibility.Collapsed;
                    }

                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                    break;

                case 2:
                    //TODO: Handle Facebook's Integration
                    break;

                default:
                    break;
            }
            RaisePropertyChanged(() => TwitterRemoveIconVisibility);
            RaisePropertyChanged(() => TwitterPlusIconVisibility);
        }

        private static async void BackgroundProcessRegisterer()
        {
            const string taskName = "ActionCenterToastMaker";

            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

            if (backgroundAccessStatus != BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity &&
                backgroundAccessStatus != BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity) return;
            if (BackgroundTaskRegistration.AllTasks.Any(task => task.Value.Name == taskName))
            {
                return;
            }

            var taskBuilder = new BackgroundTaskBuilder();
            taskBuilder.Name = taskName;
            taskBuilder.TaskEntryPoint = typeof (BackgroundProcesses.ActionCenterToastMaker).FullName;
            taskBuilder.SetTrigger(new TimeTrigger(500, false));

            var register = taskBuilder.Register();
        }

        private static async void BackgroundProcessRemover()
        {
            const string taskName = "ActionCenterToastMaker";

            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

            if (backgroundAccessStatus != BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity &&
                backgroundAccessStatus != BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity) return;
            foreach (var task in BackgroundTaskRegistration.AllTasks.Where(task => task.Value.Name == taskName))
            {
                task.Value.Unregister(true);
            }
        }

        private async void ToastMakerToggledButton()
        {
            MessengerInstance.Send(new NotificationMessage("ProgressBarEnable"));
            var localData = ApplicationData.Current.LocalSettings;
            var roamData = ApplicationData.Current.RoamingSettings;

            if (localData.Values.ContainsKey("ToastNotification"))
                _notificationStatus = localData.Values["ToastNotification"] as string;

            const string taskName = "ActionCenterToastMaker";

            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

            if (backgroundAccessStatus != BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity &&
                backgroundAccessStatus != BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity) return;
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                ToastToggleSwitchIsOn = task.Value.Name == taskName;
            }
            if (!ToastToggleSwitchIsOn)
            {
                if (!localData.Values.ContainsKey("ToastNotification") ||
                    !roamData.Values.ContainsKey("ToastNotification"))
                {
                    localData.Values.Add("ToastNotification", "On");
                    roamData.Values.Add("ToastNotification", "On");
                }
                else
                {
                    localData.Values.Remove("ToastNotification");
                    roamData.Values.Remove("ToastNotification");
                    localData.Values.Add("ToastNotification", "On");
                    roamData.Values.Add("ToastNotification", "On");
                }
                ToastToggleSwitchIsOn = true;
                BackgroundProcessRegisterer();
            }
            else
            {
                if (!localData.Values.ContainsKey("ToastNotification") ||
                    !roamData.Values.ContainsKey("ToastNotification"))
                {
                    localData.Values.Add("ToastNotification", "Off");
                    roamData.Values.Add("ToastNotification", "Off");
                }
                else
                {
                    localData.Values.Remove("ToastNotification");
                    roamData.Values.Remove("ToastNotification");
                    localData.Values.Add("ToastNotification", "Off");
                    roamData.Values.Add("ToastNotification", "Off");
                }
                ToastToggleSwitchIsOn = false;
                BackgroundProcessRemover();
            }

            RaisePropertyChanged(() => ToastToggleSwitchIsOn);
            MessengerInstance.Send(new NotificationMessage("ProgressBarDisable"));
        }

        private void ThemeChangerToggledButton()
        {
            MessengerInstance.Send(new NotificationMessage("ProgressBarEnable"));
            var localData = ApplicationData.Current.LocalSettings;
            var roamData = ApplicationData.Current.RoamingSettings;

            if (ToggleSwitchIsOn)
            {
                if (IsAppFirstTimeOn)
                {
                    IsAppFirstTimeOn = false;
                    return;
                }
                if (!localData.Values.ContainsKey("ThemeColor") && !roamData.Values.ContainsKey("ThemeColor"))
                {
                    localData.Values.Add("ThemeColor", "#18BC9C");
                    roamData.Values.Add("ThemeColor", "#18BC9C");
                }
                else
                {
                    localData.Values.Remove("ThemeColor");
                    roamData.Values.Remove("ThemeColor");
                    localData.Values.Add("ThemeColor", "#18BC9C");
                    roamData.Values.Add("ThemeColor", "#18BC9C");
                }
                ToggleSwitchIsOn = false;
                _themeColor = "#18BC9C";
            }
            else
            {
                if (!localData.Values.ContainsKey("ThemeColor") && !roamData.Values.ContainsKey("ThemeColor"))
                {
                    localData.Values.Add("ThemeColor", "#BA4C63");
                    roamData.Values.Add("ThemeColor", "#BA4C63");
                }
                else
                {
                    localData.Values.Remove("ThemeColor");
                    roamData.Values.Remove("ThemeColor");
                    localData.Values.Add("ThemeColor", "#BA4C63");
                    roamData.Values.Add("ThemeColor", "#BA4C63");
                }
                ToggleSwitchIsOn = true;
                _themeColor = "#BA4C63";
            }

            RaisePropertyChanged(() => ToggleSwitchIsOn);
            MessengerInstance.Send(new NotificationMessage(_themeColor));
            MessengerInstance.Send(new NotificationMessage("ProgressBarDisable"));
        }

        private void PasswordVaultRetrieverMethod()
        {
            var vault = new PasswordVault();
            try
            {
                var credentialList = vault.FindAllByUserName("Twitter");
                if (credentialList.Count <= 0) return;
                TwitterPlusIconVisibility = Visibility.Collapsed;
                TwitterRemoveIconVisibility = Visibility.Visible;
            }

            catch (Exception ex)
            {
                TwitterPlusIconVisibility = Visibility.Visible;
                TwitterRemoveIconVisibility = Visibility.Collapsed;
            }

            RaisePropertyChanged(() => TwitterPlusIconVisibility);
            RaisePropertyChanged(() => TwitterRemoveIconVisibility);

        }

        public void UserNameSaver(string userName)
        {
            MessengerInstance.Send(new NotificationMessage("ProgressBarEnable"));
            try
            {
                MessengerInstance.Send(new NotificationMessage(userName));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            MessengerInstance.Send(new NotificationMessage("ProgressBarDisable"));
        }

        private void SliderValueControllerMethod()
        {
            MessengerInstance.Send(new NotificationMessage("ProgressBarEnable"));
            var currentSliderValue = SliderValue;
            var localData = ApplicationData.Current.LocalSettings;

            if (localData.Values.ContainsKey("TimerTime")) localData.Values.Remove("TimerTime");
            localData.Values.Add("TimerTime", currentSliderValue);

            try
            {
                MessengerInstance.Send(new NotificationMessage(currentSliderValue.ToString()));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            MessengerInstance.Send(new NotificationMessage("ProgressBarDisable"));
        }
    }


}
