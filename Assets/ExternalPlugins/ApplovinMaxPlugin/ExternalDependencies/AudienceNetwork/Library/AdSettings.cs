﻿using UnityEngine;
using System.Runtime.InteropServices;

namespace AudienceNetwork
{
    public static class AdSettings
    {
        public static void AddTestDevice(string deviceID)
        {
            AdSettingsBridge.Instance.AddTestDevice(deviceID);
        }

        public static void SetUrlPrefix(string urlPrefix)
        {
            AdSettingsBridge.Instance.SetUrlPrefix(urlPrefix);
        }

        public static void SetMixedAudience(bool mixedAudience)
        {
            AdSettingsBridge.Instance.SetMixedAudience(mixedAudience);
        }

        public static void SetDataProcessingOptions(string[] dataProcessingOptions)
        {
            AdSettingsBridge.Instance.SetDataProcessingOptions(dataProcessingOptions);
        }

        public static void SetDataProcessingOptions(string[] dataProcessingOptions, int country, int state)
        {
            AdSettingsBridge.Instance.SetDataProcessingOptions(dataProcessingOptions, country, state);
        }

        public static string GetBidderToken()
        {
            return AdSettingsBridge.Instance.GetBidderToken();
        }
#if UNITY_IOS
        public static void SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled)
        {
            Debug.Log("[SetAdvertiserTrackingEnabled] " + advertiserTrackingEnabled);
            AdSettingsBridge.Instance.SetAdvertiserTrackingEnabled(advertiserTrackingEnabled);
        }
#endif
    }

    internal static class AdLogger
    {

        private enum AdLogLevel
        {
            None,
            Notification,
            Error,
            Warning,
            Log,
            Debug,
            Verbose
        }

        private static AdLogLevel logLevel = AdLogLevel.Log;
        private static readonly string logPrefix = "Audience Network Unity ";

        internal static void Log(string message)
        {
            AdLogLevel level = AdLogLevel.Log;
            if (logLevel >= level)
            {
                Debug.Log(logPrefix + LevelAsString(level) + message);
            }
        }

        internal static void LogWarning(string message)
        {
            AdLogLevel level = AdLogLevel.Warning;
            if (logLevel >= level)
            {
                Debug.LogWarning(logPrefix + LevelAsString(level) + message);
            }
        }

        internal static void LogError(string message)
        {
            AdLogLevel level = AdLogLevel.Error;
            if (logLevel >= level)
            {
                Debug.LogError(logPrefix + LevelAsString(level) + message);
            }
        }

        private static string LevelAsString(AdLogLevel logLevel)
        {
            switch (logLevel)
            {
                case AdLogLevel.Error:
                    {
                        return "<error>: ";
                    }
                case AdLogLevel.Warning:
                    {
                        return "<warn>: ";
                    }
                case AdLogLevel.Log:
                    {
                        return "<log>: ";
                    }
                case AdLogLevel.Debug:
                    {
                        return "<debug>: ";
                    }
                case AdLogLevel.Verbose:
                    {
                        return "<verbose>: ";
                    }
                default:
                    {
                        return "";
                    }
            }
        }
    }

    internal interface IAdSettingsBridge
    {
        void AddTestDevice(string deviceID);
        void SetUrlPrefix(string urlPrefix);
        void SetMixedAudience(bool mixedAudience);
        void SetDataProcessingOptions(string[] dataProcessingOptions);
        void SetDataProcessingOptions(string[] dataProcessingOptions, int country, int state);
        string GetBidderToken();
#if UNITY_IOS
        void SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled);
#endif
    }

    internal class AdSettingsBridge : IAdSettingsBridge
    {

        public static readonly IAdSettingsBridge Instance;

        internal AdSettingsBridge()
        {
        }

        static AdSettingsBridge()
        {
            Instance = CreateInstance();
        }

        private static IAdSettingsBridge CreateInstance()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
#if UNITY_IOS
                return new AdSettingsBridgeIOS();
#elif UNITY_ANDROID
                return new AdSettingsBridgeAndroid();
#endif
            }
            return new AdSettingsBridge();
        }

        public virtual void AddTestDevice(string deviceID)
        {
        }

        public virtual void SetUrlPrefix(string urlPrefix)
        {
        }

        public virtual void SetMixedAudience(bool mixedAudience)
        {
        }

        public virtual void SetDataProcessingOptions(string[] dataProcessingOptions)
        {
        }

        public virtual void SetDataProcessingOptions(string[] dataProcessingOptions, int country, int state)
        {
        }

        public virtual string GetBidderToken()
        {
            return string.Empty;
        }
#if UNITY_IOS
        public virtual void SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled)
        {
        }
#endif
    }

#if UNITY_ANDROID
    internal class AdSettingsBridgeAndroid : AdSettingsBridge
    {

        public override void AddTestDevice(string deviceID)
        {
            AndroidJavaClass adSettings = GetAdSettingsObject();
            adSettings.CallStatic("addTestDevice", deviceID);
        }

        public override void SetUrlPrefix(string urlPrefix)
        {
            AndroidJavaClass adSettings = GetAdSettingsObject();
            adSettings.CallStatic("setUrlPrefix", urlPrefix);
        }

        public override void SetMixedAudience(bool mixedAudience)
        {
            AndroidJavaClass adSettings = GetAdSettingsObject();
            adSettings.CallStatic("setMixedAudience", mixedAudience);
        }

        public override void SetDataProcessingOptions(string[] dataProcessingOptions)
        {
            AndroidJavaClass adSettings = GetAdSettingsObject();
            adSettings.CallStatic("setDataProcessingOptions", (object) dataProcessingOptions);
        }

        public override void SetDataProcessingOptions(string[] dataProcessingOptions, int country, int state)
        {
            AndroidJavaClass adSettings = GetAdSettingsObject();
            adSettings.CallStatic("setDataProcessingOptions", (object) dataProcessingOptions, country, state);
        }

        public override string GetBidderToken()
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaClass bidderTokenProvider = new AndroidJavaClass("com.facebook.ads.BidderTokenProvider");
            return bidderTokenProvider.CallStatic<string>("getBidderToken", context);
        }

        private AndroidJavaClass GetAdSettingsObject()
        {
            return new AndroidJavaClass("com.facebook.ads.AdSettings");
        }

    }
#endif

#if UNITY_IOS
    internal class AdSettingsBridgeIOS : AdSettingsBridge
    {
        [DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeAddTestDevice(string deviceID);

        [DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeSetURLPrefix(string urlPrefix);

        [DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeSetMixedAudience(bool mixedAudience);

        [DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeSetDataProcessingOptions(string[] dataProcessingOptions, int length);

        [DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeSetDetailedDataProcessingOptions(string[] dataProcessingOptions, int length, int country, int state);

        [DllImport("__Internal")]
        private static extern string FBAdSettingsBridgeGetBidderToken();

        [DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeSetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled);

        public override void AddTestDevice(string deviceID)
        {
            FBAdSettingsBridgeAddTestDevice(deviceID);
        }

        public override void SetUrlPrefix(string urlPrefix)
        {
            FBAdSettingsBridgeSetURLPrefix(urlPrefix);
        }

        public override void SetMixedAudience(bool mixedAudience)
        {
            FBAdSettingsBridgeSetMixedAudience(mixedAudience);
        }

        public override void SetDataProcessingOptions(string[] dataProcessingOptions)
        {
            FBAdSettingsBridgeSetDataProcessingOptions(dataProcessingOptions, dataProcessingOptions.Length);
        }

        public override void SetDataProcessingOptions(string[] dataProcessingOptions, int country, int state)
        {
            FBAdSettingsBridgeSetDetailedDataProcessingOptions(dataProcessingOptions, dataProcessingOptions.Length, country, state);
        }

        public override string GetBidderToken()
        {
            return FBAdSettingsBridgeGetBidderToken();
        }

        public override void SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled)
        {
            FBAdSettingsBridgeSetAdvertiserTrackingEnabled(advertiserTrackingEnabled);
        }
    }
#endif
}
