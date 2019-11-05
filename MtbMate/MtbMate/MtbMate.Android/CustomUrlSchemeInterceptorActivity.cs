﻿using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using MtbMate.Auth;

namespace MtbMate.Droid {
    [Activity(Theme = "@style/MainTheme.Interceptor", Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataSchemes = new[] { ApiKeysLocal.GoogleAuthRedirectUrl },
        DataPath = "/oauth2redirect")]
    public class CustomUrlSchemeInterceptorActivity : Activity {
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            global::Android.Net.Uri uriAndroid = Intent.Data;

            Uri uriNetfx = new Uri(uriAndroid.ToString());

            AuthenticationState.Authenticator.OnPageLoading(uriNetfx);

            var intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(intent);

            this.Finish();

            return;
        }
    }
}