using BepInEx;
using On;
using System;
[BepInPlugin("watcherfix", "Watcher Fix", "1.0")]
public class WatcherFix : BaseUnityPlugin
{
    public void OnEnable()
    {
        // 1. Create a basic instance of Random
        Random rand = new Random();
        int length = 8;
        string pool = "0123456789";
        string randomString = "";
        for (int i = 0; i < length; i++)
        {
            // Pick a random position inside the pool string
            int index = rand.Next(0, pool.Length);

            // Add that character to our result
            randomString += pool[index];
        }
        Logger.LogInfo("WATCHERFIX_" + randomString + " LOADED SUCCESSFULLY");


        // Intercept process switches
        On.ProcessManager.RequestMainProcessSwitch_ProcessID += (orig, self, next) =>
        {
            if (next == ProcessManager.ProcessID.SleepScreen)
            {
                Logger.LogInfo("WATCHERFIX: Intercepted SleepScreen → skipping to next cycle");
                orig(self, ProcessManager.ProcessID.Game);
                return;
            }

            orig(self, next);
        };

    }


    private void SkipSleepScreen(
        On.ProcessManager.orig_RequestMainProcessSwitch_ProcessID orig,
        ProcessManager self,
        ProcessManager.ProcessID next)
    {
        if (next == ProcessManager.ProcessID.SleepScreen)
        {
            Logger.LogInfo("WATCHERFIX: Intercepted SleepScreen → skipping to next cycle");
            orig(self, ProcessManager.ProcessID.Game);
            return;
        }

        orig(self, next);
    }
}
