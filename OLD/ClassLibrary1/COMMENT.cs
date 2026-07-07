

/*
public class WatcherFixOptions : OptionInterface
{
    public readonly Configurable<bool> SkipSleepScreen;
    public readonly Configurable<bool> SkipDeathScreen;
    public readonly Configurable<bool> FastShelterDoor;
    public readonly Configurable<bool> DisableBlinking;
    public readonly Configurable<bool> InstantEat;

    public WatcherFixOptions()
    {
        SkipSleepScreen = config.Bind("skip_sleep", true, new ConfigurableInfo(
            "Skip the Sleep Screen entirely",
            null,
            "Skip Sleep Screen"
        ));

        SkipDeathScreen = config.Bind("skip_death", false, new ConfigurableInfo(
            "Skip the Death Screen",
            null,
            "Skip Death Screen"
        ));

        FastShelterDoor = config.Bind("fast_door", true, new ConfigurableInfo(
            "Instant shelter door open",
            null,
            "Fast Shelter Door"
        ));

        DisableBlinking = config.Bind("no_blink", false, new ConfigurableInfo(
            "Disable slugcat idle blinking",
            null,
            "Disable Blinking"
        ));

        InstantEat = config.Bind("instant_eat", false, new ConfigurableInfo(
            "Remove chewing animation",
            null,
            "Instant Eat"
        ));
    }

    public override void Initialize()
    {
        base.Initialize();

        Tabs = new[]
        {
            new OpTab(this, "General")
        };

        AddItems(
            new OpLabel(10f, 550f, "WatcherFix Settings", true),

            new OpCheckBox(SkipSleepScreen, 10f, 500f),
            new OpLabel(40f, 500f, "Skip Sleep Screen"),

            new OpCheckBox(SkipDeathScreen, 10f, 460f),
            new OpLabel(40f, 460f, "Skip Death Screen"),

            new OpCheckBox(FastShelterDoor, 10f, 420f),
            new OpLabel(40f, 420f, "Fast Shelter Door"),

            new OpCheckBox(DisableBlinking, 10f, 380f),
            new OpLabel(40f, 380f, "Disable Blinking"),

            new OpCheckBox(InstantEat, 10f, 340f),
            new OpLabel(40f, 340f, "Instant Eat")
        );
    }
}
*/


/*
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
   */
// test dialog
/*    On.ProcessManager.RequestMainProcessSwitch_ProcessID += (orig, self, next) =>
    {
        orig(self, next);

        if (next == ProcessManager.ProcessID.MainMenu)
        {
            self.ShowDialog(new DialogSleep(
                "Downpour DLC has been added by WatcherFix.",
                self,
                () => { }
            ));
        }
    };*/
//
//TWO BUTTON DIALOG

/*
public class DialogSleep : Dialog
{
    protected SimpleButton continueButton;
    protected SimpleButton exitButton;

    public System.Action onContinue;
    public System.Action onExit;

    public float timeOut;

    public DialogSleep(string description, ProcessManager manager,
        System.Action onContinue, System.Action onExit)
        : base(description, manager)
    {
        Init(onContinue, onExit);
    }

    private void Init(System.Action onContinue, System.Action onExit)
    {
        this.onContinue = onContinue;
        this.onExit = onExit;

        float w = 110f;
        float h = 30f;

        continueButton = new SimpleButton(
            this, pages[0], Translate("CONTINUE"), "CONTINUE",
            new Vector2(pos.x + size.x * 0.5f - w - 10f, pos.y + Mathf.Max(size.y * 0.04f, 7f)),
            new Vector2(w, h)
        );
        pages[0].subObjects.Add(continueButton);

        exitButton = new SimpleButton(
            this, pages[0], Translate("EXIT"), "EXIT",
            new Vector2(pos.x + size.x * 0.5f + 10f, pos.y + Mathf.Max(size.y * 0.04f, 7f)),
            new Vector2(w, h)
        );
        pages[0].subObjects.Add(exitButton);
    }

    public override void Update()
    {
        base.Update();

        timeOut -= 0.025f;
        if (timeOut < 0f) timeOut = 0f;

        bool grey = timeOut > 0f;

        if (continueButton != null)
            continueButton.buttonBehav.greyedOut = grey;

        if (exitButton != null)
            exitButton.buttonBehav.greyedOut = grey;
    }

    public override void Singal(MenuObject sender, string message)
    {
        switch (message)
        {
            case "CONTINUE":
                onContinue?.Invoke();
                manager.StopSideProcess(this);
                break;

            case "EXIT":
                onExit?.Invoke();
                manager.StopSideProcess(this);
                break;
        }
    }
}


*/