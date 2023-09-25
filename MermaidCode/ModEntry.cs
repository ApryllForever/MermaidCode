using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using xTile;
using RestStopLocations.Quests;
using HarmonyLib;
using SpaceShared.APIs;



namespace RestStopCode

{
    public class ModEntry : Mod
    {
        internal static IMonitor ModMonitor { get; set; }
        internal new static IModHelper Helper { get; set; }

        private string? LastEventId;
        public static int VikaBug1Counter = 0;

        public bool AllaMail8Heart = false;

        private Patcher Patcher;

        public override void Entry(IModHelper helper)
        {
            var harmony = new Harmony(this.ModManifest.UniqueID);
            ModMonitor = Monitor;
            Helper = helper;
            // helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            //Helper.Events.Specialized.LoadStageChanged += OnLoadStageChanged;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            //helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            //ModEntry.Helper.Events.Content.AssetRequested += this.OnAssetRequested; //c# mapv edit
            TouchActionProperties.Enable(helper, Monitor);
            Helper.Events.GameLoop.DayEnding += OnDayEnding;
            //Assets.Load(helper.ModContent);
            //Helper.Events.Content.AssetRequested += OnAssetRequested;

            helper.Events.GameLoop.UpdateTicked += this.UpdateTicked;


            helper.Events.Content.AssetRequested += ModEntry.OnAssetRequested; //C# map edit from SMAPI Wiki

            AddSpecialOrdersAfterEvents.Enable(helper, Monitor);

            HarmonyPatch_UntimedSpecialOrders.ApplyPatch(harmony, helper, Monitor);

            SpecialOrderNPCIcons.Enable(helper, Monitor);

            Patcher = new Patcher(this);
            Patcher.PerformPatching();
            SpecialOrders.Initialize(this);

            QuestController.Initialize(this);

        }

        //private void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
        //  => Assets.ApplyEdits(e);
        private void OnGameLaunched(object sender, EventArgs e)
        {
           // RSDoors.Setup(Helper);

            TileActionHandler.Initialize(Helper);
            RestStopTileActions.Setup(Helper);
            //ExternalAPIs.Initialize(Helper);
            WarpTotem.Initialize(this);
            WarpTotem2.Initialize(this);
            WarpTotem3.Initialize(this);
            WarpTotem4.Initialize(this);
            WarpTotemSapphire.Initialize(this);




            var sc = Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");

            sc.RegisterSerializerType(typeof(MermaidItemDeliveryQuest));
            sc.RegisterSerializerType(typeof(MermaidResourceCollectionQuest));
            sc.RegisterSerializerType(typeof(MermaidCraftingQuest));


        }

        private void UpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
          //  string? newId = Game1.CurrentEvent.id;

            //if (this.LastEventId == "11429701" && newId is null)
                //Game1.player.modData["ApryllForever.RestStopCode/VikaBug1Counter"] = VikaBug1Counter.ToString();

            //this.LastEventId = newId;
        }

        private void OnDayEnding(object sender, DayEndingEventArgs e)
        {
            if (Game1.player.friendshipData["Alla"].Points > 1999 && AllaMail8Heart == false)
                Game1.player.mailForTomorrow.Add("MermaidRise.Alla8HeartInvite");

     
            if (Game1.player.mailReceived.Contains("VikaBug1Mail"))
            {

                var counterstring = Game1.player.modData["ApryllForever.RestStopCode/VikaBug1Counter"];


                int counter = int.Parse(counterstring);
            
                counter ++;

                Game1.player.modData["ApryllForever.RestStopCode/VikaBug1Counter"] = counter.ToString();

                if (counter == 7)
                {
                    Game1.player.mailForTomorrow.Add("startVikaBug2");

                }

            }

            if (Game1.player.mailReceived.Contains("VikaBug2Mail"))
            {

                var counterstring = Game1.player.modData["ApryllForever.RestStopCode/VikaBug2Counter"];


                int counter = int.Parse(counterstring);

                counter++;

                Game1.player.modData["ApryllForever.RestStopCode/VikaBug2Counter"] = counter.ToString();

                if (counter == 13)
                {
                    Game1.player.mailForTomorrow.Add("startPierreSabotage");

                }

            }


        }

        private static void  OnAssetRequested(object sender, AssetRequestedEventArgs e)  //C# SMAPI
        {
            var characters = new StardewValley.GameLocation();
            if (e.Name.IsEquivalentTo("Maps/Custom_RealmofSpirits"))
            {
                e.Edit(asset =>
                {
                    IAssetDataForMap editor = asset.AsMap();
                    Map map = editor.Data;


                });
            }
        }


        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

        }
    }
}