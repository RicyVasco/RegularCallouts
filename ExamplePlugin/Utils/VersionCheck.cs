using Rage;
using RegularCallouts.Stuff;
using System;
using System.Net;

namespace RegularCallouts.VersionCheck
{
    public class Check
    {
        public static bool isUpdateAvailable()
        {
            string curVersion = Settings.CalloutVersion;

            Uri latestVersionUri = new Uri("https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=43654&textOnly=1"); //Use instead of "43654" your file number on lcpdfr.com
            WebClient webClient = new WebClient();
            string receivedData = string.Empty;

            try
            {
                receivedData = webClient.DownloadString("https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=43654&textOnly=1").Trim(); //Use instead of "43654" your file number on lcpdfr.com
            }
            catch (WebException)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~RegularCallout Warning", "~y~Failed to check for a update", "Please check if you are ~o~online~w~, or try to reload the plugin.");
                Game.Console.Print("Regular Callout: Failed to check for a update.");
                Game.Console.Print("Regular Callout: Please check if you are online, or try to reload the plugin.");
            }
            if (receivedData != Settings.CalloutVersion)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~RegularCallout Warning", "~y~A new Update is available!", "Current Version: ~r~" + curVersion + "~w~<br>New Version: ~o~" + receivedData);
                Game.Console.Print("Regular Callout: A new version of UnitedCallouts is available! Update the Version, or play on your own risk.");
                Game.Console.Print("Regular Callout: Current Version:  " + curVersion);
                Game.Console.Print("Regular Callout: New Version:  " + receivedData);
                return true;
            }
            else
            {
                Game.DisplayNotification("Detected newest version of Regular Callouts.<br>Installed Version: ~g~" + curVersion + "");
                return false;
            }
        }
    }
}
