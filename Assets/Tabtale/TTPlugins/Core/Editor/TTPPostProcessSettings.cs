#if UNITY_IOS
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode.Extensions;

namespace Tabtale.TTPlugins
{
    public class TTPPostProcessSettings
    {
        private static PlistElementDict rootDict;

        [PostProcessBuild(40005)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            var pbxProjectPath = UnityEditor.iOS.Xcode.PBXProject.GetPBXProjectPath(path);
            var pbxProject = new UnityEditor.iOS.Xcode.PBXProject();
            pbxProject.ReadFromString(System.IO.File.ReadAllText(pbxProjectPath));

            Debug.Log("TTPPostProcessSettings::Add swift support for mopub and ecpm");
            pbxProject.AddBuildProperty(GetTargetGUID(pbxProject), "LIBRARY_SEARCH_PATHS", "$(TOOLCHAIN_DIR)/usr/lib/swift/$(PLATFORM_NAME)");
            pbxProject.AddBuildProperty(GetTargetGUID(pbxProject), "LIBRARY_SEARCH_PATHS", "$(SDKROOT)/usr/lib/swift");
            pbxProject.SetBuildProperty(GetTargetGUID(pbxProject), "LD_RUNPATH_SEARCH_PATHS", "/usr/lib/swift $(inherited) @executable_path/Frameworks");
            pbxProject.SetBuildProperty(GetTargetGUID(pbxProject), "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
            pbxProject.SetBuildProperty(GetTargetGUID(pbxProject), "SWIFT_VERSION", "5");
#if UNITY_2019_3_OR_NEWER
            var unityFrameworkTarget = pbxProject.GetUnityFrameworkTargetGuid();
            pbxProject.AddBuildProperty(unityFrameworkTarget, "LIBRARY_SEARCH_PATHS", "$(TOOLCHAIN_DIR)/usr/lib/swift/$(PLATFORM_NAME)");
            pbxProject.AddBuildProperty(unityFrameworkTarget, "LIBRARY_SEARCH_PATHS", "$(SDKROOT)/usr/lib/swift");
            pbxProject.SetBuildProperty(unityFrameworkTarget, "LD_RUNPATH_SEARCH_PATHS", "/usr/lib/swift $(inherited) @executable_path/Frameworks");
            pbxProject.SetBuildProperty(unityFrameworkTarget, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
            pbxProject.SetBuildProperty(unityFrameworkTarget, "SWIFT_VERSION", "5");

            var mainTargetLinkFrameworksId = pbxProject.GetFrameworksBuildPhaseByTarget(pbxProject.GetUnityMainTargetGuid());
            var unityFrameworkBuildProductId = pbxProject.GetTargetProductFileRef(pbxProject.GetUnityFrameworkTargetGuid());
            Debug.Log("Linking unity framework to main target to support unity 2020 - " + mainTargetLinkFrameworksId + ", " + unityFrameworkBuildProductId);
            pbxProject.AddFileToBuildSection(pbxProject.GetUnityMainTargetGuid(), mainTargetLinkFrameworksId, unityFrameworkBuildProductId);
            
            
#endif

#if UNITY_2019_3_OR_NEWER

            //Add BUAdASDK.bundle
            string pathToBUAdSDK = "Data/Raw/Bundle/BUAdSDK.bundle";
            string absPathToBUADSDK = Path.Combine(path, pathToBUAdSDK);

            if (Directory.Exists(absPathToBUADSDK))
            {
                Debug.Log("TTPAddTiktokBundle.cs :: Adding BUAdSDK.bundle");
                pbxProject.AddFileToBuild(GetTargetGUID(pbxProject), pbxProject.AddFile(pathToBUAdSDK, "BUAdSDK.bundle"));
            }
            //END BUAdSDK
            
            string pathToTapJoyBundle = "Data/Raw/Bundle/TapjoyResources.bundle";
            string absPathTapJoyBundle = Path.Combine(path, pathToBUAdSDK);

            if (Directory.Exists(absPathTapJoyBundle))
            {
                Debug.Log("TTPPostProcessSettings :: Adding TapjoyResources.bundle");
                pbxProject.AddFileToBuild(GetTargetGUID(pbxProject), pbxProject.AddFile(pathToTapJoyBundle, "TapjoyResources.bundle"));
            }
#endif

            File.WriteAllText(pbxProjectPath, pbxProject.WriteToString());

            var plistPath = Path.Combine(path, "Info.plist");
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);
            rootDict = plist.root;

            // rootDict.SetBoolean("GOOGLE_ANALYTICS_DEFAULT_ALLOW_AD_PERSONALIZATION_SIGNALS", false);
            // rootDict.SetBoolean("FIREBASE_ANALYTICS_COLLECTION_ENABLED", false);
            
            
            // Add AppLovinSdkKey
            if (Application.identifier == "com.tabtaleint.ttplugins" ||
                Application.identifier == "com.tabtaleint.ttplugins" ||
                Application.identifier == "com.tabtaleint.ttplugins")
            {
                rootDict.SetString("AppLovinSdkKey", "yRHC8kgWwG5S4lOh7Dx_pZB2iEBLVWMSzde5MKbGahifQ6MTKIT7tk9ZzLvTsFwptZvDuVTTBB8cHU9bohkeQu");
            }
            else
            {
                rootDict.SetString("AppLovinSdkKey", "TREvWeSbneklepMTdxWL5KCqUD57xezP4CIarlBcOwM1kiVMe0hkLvTq7dy3HwSL6mxyV7Tu1wwlcP5FQo-nhW");
            }


            var array = rootDict.CreateArray("SKAdNetworkItems");
            //admob
            array.AddDict().SetString("SKAdNetworkIdentifier","cstr6suwn9.skadnetwork");
            //applovin
            array.AddDict().SetString("SKAdNetworkIdentifier","ludvb6z3bs.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","");
            //ironsource
            array.AddDict().SetString("SKAdNetworkIdentifier","SU67R6K2V3.skadnetwork");
            //adcolony
            array.AddDict().SetString("SKAdNetworkIdentifier","4PFYVQ9L8R.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","4FZDC2EVR5.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","4468KM3ULZ.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","3RD42EKR43.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","M8DBW4SV7C.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","EJVT5QM6AK.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","MTKV5XTK9E.skadnetwork");
            //chartboost
            array.AddDict().SetString("SKAdNetworkIdentifier","F38H382JLK.skadnetwork");
            //facebook
            array.AddDict().SetString("SKAdNetworkIdentifier","v9wttpbfk9.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","n38lu8286q.skadnetwork");
            //hypermx
            array.AddDict().SetString("SKAdNetworkIdentifier","NU4557A4JE.skadnetwork");
            //inmobi
            array.AddDict().SetString("SKAdNetworkIdentifier","WZMMZ9FP6W.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","uw77j35x4d.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","7UG5ZH24HU.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","hs6bdukanm.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","FZDC2EVR5.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","ggvn48r87g.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","5lm9lj6jb7.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","9RD848Q2BZ.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","c6k4g5qg8m.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","3sh42y64q3.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","YCLNXRL5PM.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","f73kdq92p3.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","ydx93a7ass.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","W9Q455WK68.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","prcb7njmu6.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","wg4vff78zm.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","mlmmfzh3r3.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","TL55SBB4FM.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","t38b2kh725.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","5l3tpt7t6e.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","7rz58n8ntl.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","KLF5C3L5U5.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","cg4yq2srnc.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","av6w8kgt66.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","9T245VHMPL.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","V72QYCH5UU.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","2U9PT9HC89.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","44jx6755aq.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","8s468mfl3y.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","p78axxw29g.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","ppxm28t8ap.skadnetwork");
            //mintegral
            array.AddDict().SetString("SKAdNetworkIdentifier","KBD757YWX3.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","GLQZH8VGBY.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","6xzpu9s2p8.skadnetwork");
            //pangle
            array.AddDict().SetString("SKAdNetworkIdentifier","22mmun2rn5.skadnetwork"); //non cn
            array.AddDict().SetString("SKAdNetworkIdentifier","238da6jt44.skadnetwork"); //cn
            //tapjoy
            array.AddDict().SetString("SKAdNetworkIdentifier","ECPZ2SRF59.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","578prtvx9j.skadnetwork");
            //unity ads
            array.AddDict().SetString("SKAdNetworkIdentifier","4DZT52R2T5.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","488r3q3dtq.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","zmvfpc5aq8.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","v79kvwwj4g.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","lr83yxwka7.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","44n7hlldy6.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","424M5254LK.skadnetwork");
            //vungle
            array.AddDict().SetString("SKAdNetworkIdentifier","GTA9LK7P23.skadnetwork");
            //mopub
            array.AddDict().SetString("SKAdNetworkIdentifier","cDkw7geQsH.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","qyJfv329m4.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","3qy4746246.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","523jb4fst2.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","5a6flpkh64.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","737z793b9f.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","7953JERFZD.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","97r2b46745.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","9yg77x724h.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","bvpn9ufa9b.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","CJ5566H2GA.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","gvmwg8q7h5.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","mls7yz5dvl.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","n66cz3y3bx.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","n9x2a789qt.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","nzq8sh4pbs.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","pu4na253f3.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","u679fj5vs4.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","XY9T38CT57.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","z4gj7hsk7h.skadnetwork");

            array.AddDict().SetString("SKAdNetworkIdentifier","s39g8k73mm.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","24t9a8vw3c.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","yrqqpx2mcb.skadnetwork");
            array.AddDict().SetString("SKAdNetworkIdentifier","52fl2v3hgk.skadnetwork");
            
            
            
            
            // fix problem with statusbar on iOS 14
            if (!rootDict.values.ContainsKey("UIViewControllerBasedStatusBarAppearance"))
            {
                rootDict.SetBoolean("UIViewControllerBasedStatusBarAppearance", false);
            }

            // rootDict.SetString("NSUserTrackingUsageDescription", "Please press Allow, and all the ads you'll see will be personalized to you");

            File.WriteAllText(plistPath, plist.WriteToString());
        }

        private static string GetTargetGUID(PBXProject proj)
        {
#if UNITY_2019_3_OR_NEWER
        return proj.GetUnityMainTargetGuid();
#else
            return proj.TargetGuidByName("Unity-iPhone");
#endif
        }


    }

}
#endif
