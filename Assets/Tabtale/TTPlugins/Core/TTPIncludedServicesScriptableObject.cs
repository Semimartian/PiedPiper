using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabtale.TTPlugins
{
    public class TTPIncludedServicesScriptableObject : ScriptableObject
    {
        public bool appsFlyer = true;
        public bool analytics = true;
        public bool banners = true;
        public bool interstitials = true;
        public bool rvs = true;
        public bool crashTool = true;
        public bool privacySettings = true;
        public bool rateUs = true;
        public bool openAds = true;
    }

}
