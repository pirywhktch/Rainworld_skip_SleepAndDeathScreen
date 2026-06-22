


using BepInEx;
using Menu;
using UnityEngine;
using Menu.Remix.MixedUI;

[BepInPlugin("watcherfix", "Watcher Fix", "1.3")]
public class WatcherFix : BaseUnityPlugin
{
    // GLOBAL FLAGS
    private static bool wantShowDialog = false;
    private static bool wantContinue = false;
    private static bool wantExitMenu = false;
    private static bool wantExitGame = false;
    private bool initialized;
    public static WatcherFixOptions Options;

    private void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);

        if (initialized) return;
        initialized = true;

        Options = new WatcherFixOptions();
        MachineConnector.SetRegisteredOI("watcherfix", Options);

        Logger.LogInfo("WATCHERFIX: Remix menu registered");
    }
    public void OnEnable()
    {
        On.RainWorld.OnModsInit += RainWorld_OnModsInit;

        Options = new WatcherFixOptions();
        MachineConnector.SetRegisteredOI("watcherfix", Options);

        // 1. Create a basic instance of Random
        System.Random rand = new System.Random();
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



        // --- OG SLEEPSCREEN SKIP ---
        On.ProcessManager.RequestMainProcessSwitch_ProcessID += (orig, self, next) =>
        {
            if (next == ProcessManager.ProcessID.SleepScreen)
            {
                if (WatcherFix.Options.EnableMod.Value)
                {
                    Logger.LogInfo("WATCHERFIX: Intercepted SleepScreen → skipping");

                    if (WatcherFix.Options.SkipSleepScreen.Value)
                    {
                        self.currentMainLoop.framesPerSecond = 1;
                        wantShowDialog = true; // show dialog AFTER game loads
                    }
                    orig(self, ProcessManager.ProcessID.Game);
                    return;
                }
                Logger.LogInfo("WATCHERFIX: Intercepted SleepScreen but the enable mod toggle said no");
            }

            orig(self, next);
        };

        // --- FRAME UPDATE ---
        On.RainWorld.Update += RainWorld_Update;
    }

    public void OnDisable()
    {
        On.RainWorld.Update -= RainWorld_Update;
    }

    // FRAME UPDATE LOOP
    public void RainWorld_Update(On.RainWorld.orig_Update orig, RainWorld self)
    {
        orig(self);

        ProcessManager pm = self.processManager;
        if (pm == null) return;

        // Show dialog AFTER game loads
        if (wantShowDialog && pm.currentMainLoop is RainWorldGame)
        {
            wantShowDialog = false;

            pm.ShowDialog(new DialogSleep(
                "next cycle?",
                pm,
                () => { wantContinue = true; },
                () => { wantExitMenu = true; },
                () => { wantExitGame = true; }
            ));
        }

        // CONTINUE → do nothing (skip already happened)
        if (wantContinue)
        {

            wantContinue = false;
            pm.currentMainLoop.framesPerSecond = 60;
            Logger.LogInfo("WATCHERFIX: CONTINUE pressed");



        }

        // EXIT TO MENU
        if (wantExitMenu)
        {
            wantExitMenu = false;
            pm.currentMainLoop.framesPerSecond = 60;
            Logger.LogInfo("WATCHERFIX: EXIT TO MENU pressed");
            pm.RequestMainProcessSwitch(ProcessManager.ProcessID.MainMenu);
        }

        // EXIT GAME
        if (wantExitGame)
        {
            wantExitGame = false;
            Logger.LogInfo("WATCHERFIX: EXIT GAME pressed");
            Application.Quit();
        }
    }
}


public class WatcherFixOptions : OptionInterface
{
    //BUTTON INIT
    public Configurable<bool> SkipSleepScreen;
    public Configurable<bool> EnableMod;
    //BUTTON BIND
    public WatcherFixOptions()
    {
        SkipSleepScreen = config.Bind("skip_sleep", true);
        EnableMod = config.Bind("enable_mod", true);
    }


    public override void Initialize()
    {
        Tabs = new[]
        {
        new OpTab(this, "General")
    };

        float x = 10f;
        float y = 550f;
        float step = 40f;

        // Title
        Tabs[0].AddItems(new UIelement[]
        {
        new OpLabel(new Vector2(x, y), new Vector2(300, 30), "WatcherFix Settings")
        });
        y -= step;

        // Enable Mod
        Tabs[0].AddItems(new UIelement[]
        {
        new OpCheckBox(EnableMod, new Vector2(x, y)),
        new OpLabel(new Vector2(x + 30f, y), new Vector2(300, 30), "Enable Mod")
        });
        y -= step;

        // Enable Sleep Dialog
        Tabs[0].AddItems(new UIelement[]
        {
        new OpCheckBox(SkipSleepScreen, new Vector2(x, y)),
        new OpLabel(new Vector2(x + 30f, y), new Vector2(300, 30), "Enable Sleep Dialog")
        });
        y -= step;
    }

}

// --- THREE BUTTON DIALOG ---
public class DialogSleep : Dialog
{
    protected SimpleButton continueButton;
    protected SimpleButton exitMenuButton;
    protected SimpleButton exitGameButton;

    public System.Action onContinue;
    public System.Action onExitMenu;
    public System.Action onExitGame;

    public float timeOut;

    public DialogSleep(string description, ProcessManager manager,
        System.Action onContinue, System.Action onExitMenu, System.Action onExitGame)
        : base(description, manager)
    {
        Init(onContinue, onExitMenu, onExitGame);
    }

    private void Init(System.Action onContinue, System.Action onExitMenu, System.Action onExitGame)
    {
        this.onContinue = onContinue;
        this.onExitMenu = onExitMenu;
        this.onExitGame = onExitGame;

        float w = 110f;
        float h = 30f;
        float spacing = 20f;

        float totalWidth = 3 * w + 2 * spacing;
        float left = pos.x + (size.x - totalWidth) / 2;
        float y = pos.y + Mathf.Max(size.y * 0.04f, 7f);

        continueButton = new SimpleButton(
            this, pages[0], Translate("CONTINUE"), "CONTINUE",
            new Vector2(left, y),
            new Vector2(w, h)
        );
        pages[0].subObjects.Add(continueButton);

        exitMenuButton = new SimpleButton(
            this, pages[0], Translate("EXIT MENU"), "EXITMENU",
            new Vector2(left + w + spacing, y),
            new Vector2(w, h)
        );
        pages[0].subObjects.Add(exitMenuButton);

        exitGameButton = new SimpleButton(
            this, pages[0], Translate("EXIT GAME"), "EXITGAME",
            new Vector2(left + 2 * (w + spacing), y),
            new Vector2(w, h)
        );
        pages[0].subObjects.Add(exitGameButton);
    }


    public override void Update()
    {
        base.Update();

        timeOut -= 0.025f;
        if (timeOut < 0f) timeOut = 0f;

        bool grey = timeOut > 0f;

        continueButton.buttonBehav.greyedOut = grey;
        exitMenuButton.buttonBehav.greyedOut = grey;
        exitGameButton.buttonBehav.greyedOut = grey;
    }

    public override void Singal(MenuObject sender, string message)
    {
        switch (message)
        {
            case "CONTINUE":
                onContinue?.Invoke();
                manager.StopSideProcess(this);
                break;

            case "EXITMENU":
                onExitMenu?.Invoke();
                manager.StopSideProcess(this);
                break;

            case "EXITGAME":
                onExitGame?.Invoke();
                manager.StopSideProcess(this);
                break;
        }
    }
}

