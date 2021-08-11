using UnityEngine;
using System;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using G2.Sdk.Services.Ads;
using RocketTeam.Sdk.Services.Interfaces;
using RocketTeam.Sdk.Services.Manager;

//using G2.Sdk.Utils;
public enum RewardAdStatus
{
    NoInternet,
    ShowVideoReward,
    ShowInterstitialReward,
    NoVideoNoInterstitialReward
}
namespace RocketTeam.Sdk.Services.Ads
{
    public class AdManager : MonoBehaviour, IAdsManager
    {
        public static AdManager Instance { get; set; }

        //public IronSourceController ironSource;
        public MaxMediationController MaxMediation;

        public Action onLoaded { get; set; }
        public Action<string> onFailedToLoad { get; set; }
        public Action onOpening { get; set; }
        public Action onClosed { get; set; }
        public Action onLeavingApplication { get; set; }
        public Action<int> onGetReward { get; set; }

        void Awake()
        {
            if (Instance == null)
            {
                SdkManager.Instance.RegisterService(ServiceType.ADVERTISEMENT, this);
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                if (this != Instance)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void Init()
        {
            MaxMediation.Init();
        }

        void OnDestroy()
        {
            SdkManager.Instance.UnregisterService(ServiceType.ADVERTISEMENT, this);
        }

        #region CONNECT_TO_SERVER

        public void RegisterInterstitialListener(Action onOpened, Action onClosed, Action onLeavingApplication,
            Action<int> onGetReward)
        {
            this.onOpening = onOpened;
            this.onClosed = onClosed;
            this.onLeavingApplication = onLeavingApplication;
            this.onGetReward = onGetReward;
        }

        public bool IsInterstitialLoaded(int id)
        {
            switch (id)
            {
                case (int)AdEnums.ShowType.NO_AD:
                    if (onClosed != null)
                    {
                        onClosed();
                    }

                    return true;

                case (int)AdEnums.ShowType.INTERSTITIAL:
                    if (!MaxMediation.IsLoadInterstitial())
                    {
                        MaxMediation.LoadInterstitial();
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                case (int)AdEnums.ShowType.VIDEO_REWARD:

                    if (!MaxMediation.IsLoadRewardedAd())
                    {
                        MaxMediation.LoadRewardedAd();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                default:
                    return false;
            }
        }

        public bool ShowInterstitial(string _placement, int id = 1)
        {
            if (!IsInterstitialLoaded(id))
            {
                return false;
            }

            switch (id)
            {
                case (int)AdEnums.ShowType.NO_AD:

                    return true;

                case (int)AdEnums.ShowType.INTERSTITIAL:
                    MaxMediation.ShowInterstitial();
                    return true;

                case (int)AdEnums.ShowType.VIDEO_REWARD:
                    MaxMediation.ShowRewardedAd(_placement);
                    return true;
                default:
                    return false;
            }
        }

        public bool ShowInterstitial(string _placement, int id = 1, Action onOpened = null, Action onClosed = null,
            Action onLeavingApplication = null, Action<int> onGetReward = null)
        {
            SdkManager.Instance.AdsManager.RegisterInterstitialListener(onOpened, onClosed, onLeavingApplication,
                onGetReward);
            return ShowInterstitial(_placement, id);
        }

        internal bool IsReady()
        {
            var result = MaxMediation.IsLoadRewardedAd();
            if (!result)
            {
                if (MaxMediation.TypeAdsUse.HasFlag(TypeAdsMax.Inter_Reward))
                {
                    MaxMediation.LoadRewardedInterstitialAd();
                }
                else
                {
                    MaxMediation.LoadInterstitial();
                }

            }

            return result;

        }
        public RewardAdStatus ShowAdsReward(Action<int> OnRewarded, string _placement, bool isAutoLog = true)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                //Debug.LogError("Ads_NoInternet");
                if (isAutoLog)
                {
                    Analytics.LogEventByName("Monetize_reward_no_internet");
                    Analytics.LogEventByName("Monetize_interstitial_no_internet");
                }
                return RewardAdStatus.NoInternet;
            }

            if (IsReady())
            {


                ShowInterstitial(_placement, (int)AdEnums.ShowType.VIDEO_REWARD, null, null, null, _x =>
                {
                    if (OnRewarded != null)
                    {
                        Analytics.LogEventWatchVideo(_placement);
                        OnRewarded(_x);

                    }

                }
                );

                return RewardAdStatus.ShowVideoReward;
            }
            else
            {
                if (isAutoLog)
                {
                    Analytics.LogEventByName("Monetize_no_reward");
                }

                if (MaxMediation.TypeAdsUse.HasFlag(TypeAdsMax.Inter_Reward))
                {
                    if (MaxMediation.IsRewardedInterstitialAdReady())
                    {

                        ShowInterstitial(_placement, (int)AdEnums.ShowType.INTERSTITIAL_REWARD, null, null, null, _x =>
                        {
                            if (OnRewarded != null)
                            {
                                OnRewarded(_x);

                            }

                        });

                        return RewardAdStatus.ShowInterstitialReward;
                    }
                    else
                    {
                        if (isAutoLog)
                        {
                            Analytics.LogEventByName("Monetize_no_interstitial");
                            Analytics.LogEventByName("Monetize_no_reward_no_interstitial");
                        }
                        return RewardAdStatus.NoVideoNoInterstitialReward;
                    }
                }
                else
                {
                    if (MaxMediation.IsLoadInterstitial())
                    {

                        ShowInterstitial(_placement, (int)AdEnums.ShowType.INTERSTITIAL, null, null, null, _x =>
                        {
                            if (OnRewarded != null)
                            {
                                OnRewarded(_x);

                            }

                        });

                        return RewardAdStatus.ShowInterstitialReward;
                    }
                    else
                    {
                        if (isAutoLog)
                        {
                            Analytics.LogEventByName("Monetize_no_interstitial");
                            Analytics.LogEventByName("Monetize_no_reward_no_interstitial");
                        }
                        return RewardAdStatus.NoVideoNoInterstitialReward;
                    }
                }


            }
        }
        public bool HideInterstitial(int id = 1)
        {
            //try
            //{
            //    if (dictAdLoaded.ContainsKey(id))
            //    {
            //        int rewardValue = 0;
            //        if (dictRewardValue.ContainsKey(id))
            //        {
            //            rewardValue = dictRewardValue[id];
            //        }
            //        lastShowAdId = id;
            //        switch (dictAdLoaded[id])
            //        {
            //            case (int)AdEnums.ShowType.NO_AD:

            //                return true;
            //            default:
            //                return false;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DebugCustom.Log("Exception " + ex);
            //}

            return false;
        }

        /// <summary>
        /// OnBack == true => AdsManager used this 'Back Event'
        /// OnBack == false => AdsManager did not use this 'Back Event', you can use this
        /// </summary>
        /// <returns></returns>

        #endregion

        #region Banner

        public void RegisterBannerListener(Action onOpened, Action onClosed, Action onLeavingApplication)
        {
        }

        public bool ShowBanner(int bannerId = 1)
        {
            try
            {

                return MaxMediation.ShowBanner();
            }
            catch (Exception ex)
            {
                DebugCustom.Log(ex);
                return false;
            }
        }



        public bool HideBanner(int bannerId = 1)
        {
            MaxMediation.HideBanner();
            return true;
        }

        public int GetBannerHeight()
        {
            throw new NotImplementedException();
            //return admob.GetBannerAdsHeight();
            return 1;
        }

        public int GetBannerWidth()
        {
            throw new NotImplementedException();
            //return admob.GetBannerAdsHeight();
            return 1;
        }

        public void LoadInterstitial(int adsId = 1, bool isRefresh = false)
        {
            throw new NotImplementedException();
        }

        public bool OnBack()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}