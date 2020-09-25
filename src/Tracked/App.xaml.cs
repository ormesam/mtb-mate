﻿using System;
using Akavache;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Shared;
using Tracked.Contexts;
using Tracked.Screens.Login;
using Tracked.Screens.Master;
using Xamarin.Forms;

namespace Tracked {
    public partial class App : Application {
        public MainContext MainContext { get; }
        public static new App Current => (App)Application.Current;
        public static MasterScreen RootPage => Current.MainPage as MasterScreen;

        public App() {
            InitializeComponent();

            MainContext = new MainContext();

            if (MainContext.Security.IsLoggedIn) {
                MainPage = new MasterScreen(MainContext);
            } else {
                MainPage = new LoginScreen(MainContext);
            }
        }

        protected override void OnStart() {
            AppCenter.Start($"android={Constants.AppCenterKey};", typeof(Analytics), typeof(Crashes));

            BlobCache.EnsureInitialized();
            BlobCache.ApplicationName = Shared.Constants.AppName;
            BlobCache.ForcedDateTimeKind = DateTimeKind.Utc;
        }

        protected override void OnSleep() {
            MainContext.Storage.Storage.Flush();
        }
    }
}
