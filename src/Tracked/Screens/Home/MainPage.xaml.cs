﻿using System;
using System.ComponentModel;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Shared.Dtos;
using Tracked.Contexts;
using Xamarin.Forms;

namespace Tracked.Home {
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage {
        public MainPage(MainContext context) {
            InitializeComponent();
            BindingContext = new MainPageViewModel(context);
        }

        public MainPageViewModel ViewModel => BindingContext as MainPageViewModel;

        protected override async void OnAppearing() {
            base.OnAppearing();

            await ViewModel.Load();
        }

        private async void Add_Clicked(object sender, EventArgs e) {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();

            if (status != PermissionStatus.Granted) {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location)) {
                    await DisplayAlert("Location Required", "Location is required to record a ride", "OK");
                }

                status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
            }

            if (status == PermissionStatus.Granted) {
                await ViewModel.GoToCreateRide();
            }
        }

        private async void Ride_ItemTapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToReview(e.Item as RideOverviewDto);
        }

        private async void Profile_Clicked(object sender, EventArgs e) {
            await ViewModel.Context.UI.GoToProfileScreenAsync();
        }
    }
}
