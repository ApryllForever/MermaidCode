using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewValley;
using xTile.Dimensions;
using StardewModdingAPI;
using StardewModdingAPI.Events;
namespace RestStopCode

{
    public class RestStopTileActions
    {
        static IModHelper Helper;

        static HashSet<Vector2> SpiritAltarTriggeredToday = new HashSet<Vector2>();
        internal static void Setup(IModHelper Helper)
        {
            RestStopTileActions.Helper = Helper;
            Helper.Events.GameLoop.DayStarted += OnDayStarted;
            TileActionHandler.RegisterTileAction("RestStop_SpiritAltar", Trigger);
        }


        public virtual bool performAction(string action, Farmer who, Location tileLocation)
        {
            if (action != null && who.IsLocalPlayer)
            {
                string[] actionParams = action.Split(' ');
                switch (actionParams[0])
                {
                    case "RestStop_SpiritAltar":
                        if (who.ActiveObject != null && Game1.player.team.sharedDailyLuck.Value != -0.12 && Game1.player.team.sharedDailyLuck.Value != 0.12)
                        {
                            if (who.ActiveObject.Price >= 60)
                            {
                                // temporarySprites.Add(new TemporaryAnimatedSprite(352, 70f, 2, 2, new Vector2(tileLocation.X * 64, tileLocation.Y * 64), flicker: false, flipped: false));
                                Game1.player.team.sharedDailyLuck.Value = 0.12;
                                Game1.playSound("money");
                            }
                            else
                            {
                                // temporarySprites.Add(new TemporaryAnimatedSprite(362, 50f, 6, 1, new Vector2(tileLocation.X * 64, tileLocation.Y * 64), flicker: false, flipped: false));
                                Game1.player.team.sharedDailyLuck.Value = -0.12;
                                Game1.playSound("thunder");
                            }
                            who.ActiveObject = null;
                            who.showNotCarrying();
                        }
                        break;

                    default:
                        return false;

                }
                return true;
            }
            if (action != null && !who.IsLocalPlayer)
            {
                switch (action.ToString().Split(' ')[0])
                {

                    /*    case "Minecart":
                            openChest(tileLocation, 4, Game1.random.Next(3, 7));
                            break;
                        case "RemoveChest":
                            map.GetLayer("Buildings").Tiles[tileLocation.X, tileLocation.Y] = null;
                            break;
                        case "Door":
                            openDoor(tileLocation, playSound: true);
                            break;
                        case "TV":
                            Game1.tvStation = Game1.random.Next(2);
                            break; */
                }
            }
            return false;
        }
    
        private static void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            SpiritAltarTriggeredToday.Clear();

        }


        public static void Trigger(string tileAction, Vector2 position)
        {
            if (SpiritAltarTriggeredToday.Contains(position))
            {
                return;
            }
            SpiritAltarTriggeredToday.Add(position);
            GameLocation location = Game1.currentLocation;
            Game1.stats.Increment("SpiritAltarChecked", 1);

              if (Game1.player.ActiveObject != null && Game1.player.team.sharedDailyLuck.Value != -0.12 && Game1.player.team.sharedDailyLuck.Value != 0.12)
                            {
                                if (Game1.player.ActiveObject.Price >= 60)
                                {
                                  
                                    Game1.player.team.sharedDailyLuck.Value = 0.12;
                                    Game1.playSound("discoverMineral");
                                }
                                else
                                {
                                   
                                    Game1.player.team.sharedDailyLuck.Value = -0.12;
                                    Game1.playSound("thunder");
                                }
                Game1.player.ActiveObject = null;
                Game1.player.showNotCarrying();
                            }
      
                    }
                } 
        }
    



