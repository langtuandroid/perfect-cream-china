#if UNITY_IOS
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace Gadsme.Editor
{
    public class IOSBuildScript
    {
        public static string[] adNetworkIdentifiers = {
            "4cnglc92ub.skadnetwork",
            "uw77j35x4d.skadnetwork",
            "7ug5zh24hu.skadnetwork",
            "hs6bdukanm.skadnetwork",
            "3sh42y64q3.skadnetwork",
            "klf5c3l5u5.skadnetwork",
            "5l3tpt7t6e.skadnetwork",
            "yclnxrl5pm.skadnetwork",
            "wg4vff78zm.skadnetwork",
            "9rd848q2bz.skadnetwork",
            "kbd757ywx3.skadnetwork",
            "ydx93a7ass.skadnetwork",
            "4pfyvq9l8r.skadnetwork",
            "2u9pt9hc89.skadnetwork",
            "c6k4g5qg8m.skadnetwork",
            "prcb7njmu6.skadnetwork",
            "cj5566h2ga.skadnetwork",
            "mls7yz5dvl.skadnetwork",
            "5a6flpkh64.skadnetwork",
            "4468km3ulz.skadnetwork",
            "ejvt5qm6ak.skadnetwork",
            "578prtvx9j.skadnetwork",
            "ppxm28t8ap.skadnetwork",
            "v72qych5uu.skadnetwork",
            "f73kdq92p3.skadnetwork",
            "8m87ys6875.skadnetwork",
            "3rd42ekr43.skadnetwork",
            "8s468mfl3y.skadnetwork",
            "7rz58n8ntl.skadnetwork",
            "488r3q3dtq.skadnetwork",
            "tl55sbb4fm.skadnetwork",
            "f38h382jlk.skadnetwork",
            "zmvfpc5aq8.skadnetwork",
            "44jx6755aq.skadnetwork",
            "mlmmfzh3r3.skadnetwork",
            "97r2b46745.skadnetwork",
            "m8dbw4sv7c.skadnetwork",
            "6xzpu9s2p8.skadnetwork",
            "cg4yq2srnc.skadnetwork",
            "g28c52eehv.skadnetwork",
            "52fl2v3hgk.skadnetwork",
            "pwa73g5rt2.skadnetwork",
            "9nlqeag3gk.skadnetwork",
            "p78axxw29g.skadnetwork",
            "4fzdc2evr5.skadnetwork",
            "523jb4fst2.skadnetwork",
            "ggvn48r87g.skadnetwork",
            "24t9a8vw3c.skadnetwork",
            "cstr6suwn9.skadnetwork",
            "wzmmz9fp6w.skadnetwork",
            "5lm9lj6jb7.skadnetwork",
            "feyaarzu9v.skadnetwork",
            "9t245vhmpl.skadnetwork",
            "n9x2a789qt.skadnetwork",
            "glqzh8vgby.skadnetwork",
            "424m5254lk.skadnetwork",
            "gta9lk7p23.skadnetwork",
            "5tjdwbrq8w.skadnetwork",
            "22mmun2rn5.skadnetwork",
            "mtkv5xtk9e.skadnetwork",
            "t38b2kh725.skadnetwork",
            "6g9af3uyq4.skadnetwork",
            "u679fj5vs4.skadnetwork",
            "275upjj5gd.skadnetwork",
            "rx5hdcabgc.skadnetwork",
            "x44k69ngh6.skadnetwork",
            "32z4fx6l9h.skadnetwork",
            "w9q455wk68.skadnetwork",
            "av6w8kgt66.skadnetwork",
            "e5fvkxwrpn.skadnetwork",
            "m5mvw97r93.skadnetwork",
            "89z7zv988g.skadnetwork",
            "k674qkevps.skadnetwork",
            "hb56zgv37p.skadnetwork",
            "rvh3l7un93.skadnetwork",
            "mp6xlyr22a.skadnetwork",
            "qqp299437r.skadnetwork",
            "b9bk5wbcq9.skadnetwork",
            "bxvub5ada5.skadnetwork",
            "kbmxgpxpgc.skadnetwork",
            "6v7lgmsu45.skadnetwork",
            "m297p6643m.skadnetwork",
            "294l99pt4k.skadnetwork",
            "n6fk4nfna4.skadnetwork",
            "r45fhb6rf7.skadnetwork",
            "2fnua5tdw4.skadnetwork",
            "3l6bd9hu43.skadnetwork",
            "vcra2ehyfk.skadnetwork",
            "3qcr597p9d.skadnetwork",
            "74b6s63p6l.skadnetwork"
        };

        [PostProcessBuild(700)]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.iOS)
            {
                return;
            }

            // Update project
            string projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            var project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));
#if UNITY_2019_3_OR_NEWER
            string targetGuid = project.GetUnityFrameworkTargetGuid();
#else
            string targetGuid = project.TargetGuidByName("Unity-iPhone");
#endif
            project.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
            project.AddBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
            project.AddFrameworkToProject(targetGuid, "AppTrackingTransparency.framework", true);
            project.AddFrameworkToProject(targetGuid, "AdSupport.framework", false);
            project.AddFrameworkToProject(targetGuid, "SafariServices.framework", false);
            project.AddFrameworkToProject(targetGuid, "Webkit.framework", false);
            project.AddFrameworkToProject(targetGuid, "AVFoundation.framework", false);
            project.AddFrameworkToProject(targetGuid, "AudioToolbox.framework", false);
            File.WriteAllText(projectPath, project.WriteToString());

            // Update plist
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            PlistElementDict rootDict = plist.root;

            // Add NSUserTrackingUsageDescription
            if (rootDict["NSUserTrackingUsageDescription"] == null)
            {
                rootDict.SetString("NSUserTrackingUsageDescription", "Your data will be used to deliver personalized ads to you");
            }

            // Add SKAdNetworkItems
            PlistElementArray adNetworksItems = rootDict["SKAdNetworkItems"] == null
                ? rootDict.CreateArray("SKAdNetworkItems")
                : rootDict["SKAdNetworkItems"].AsArray();

            List<string> existingValues = new List<string>();

            foreach (PlistElement element in adNetworksItems.values)
            {
                existingValues.Add(element.AsDict().values["SKAdNetworkIdentifier"].AsString());
            }

            foreach (string adNetwokIdentifier in adNetworkIdentifiers)
            {
                if (!existingValues.Contains(adNetwokIdentifier))
                {
                    PlistElementDict adNetworksItem = adNetworksItems.AddDict();
                    adNetworksItem.SetString("SKAdNetworkIdentifier", adNetwokIdentifier);
                }
            }

            plist.WriteToFile(plistPath);
        }
    }
}
#endif