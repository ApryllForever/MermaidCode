using StardewModdingAPI;
using System;
using StardewValley;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;


namespace RestStopCode
{
    internal static class WarpTotem4
    {
        static readonly string Destination = "Custom_BluebellaAnteroom";
        static readonly int Dest_X = 8;
        static readonly int Dest_Y = 8;

        public static string Totem = null;
        static Color color = Color.Indigo;

        static IModHelper Helper;
        static IMonitor Monitor;

        internal static void Initialize(IMod ModInstance)
        {
            Helper = ModInstance.Helper;
            Monitor = ModInstance.Monitor;
            Helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            Helper.Events.Input.ButtonPressed += OnButtonPressed;

        }

        private static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Totem = "(O)ApryllForever.RiseMermaids_BluebellaTotem";
        }

        private static void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
            {
                return;
            }
            if (e.Button.IsActionButton())
            {
                try
                {
                    if (Game1.player.CurrentItem.QualifiedItemId == Totem)
                    {
                        Monitor.Log($"RestStop: Using Warp Totem: Bluebella");
                        Game1.player.reduceActiveItemByOne();
                        DoTotemWarpEffects(Game1.player, (f) => DirectWarp());
                    }
                }
                catch (Exception ex)
                {
                    Monitor.Log($"Could not find Bluebella Dungeon warp totem ID. Error: {ex}");
                }
            }
        }

        public static bool DirectWarp()
        {
            if (!(Game1.getLocationFromName(Destination) is null) || !Game1.isFestival())
            {
                // Don't go if player is at a festival
                if (!(Game1.timeOfDay > 2550))
                {
                    //VolcanoDungeon.activeLevels.Add(new VolcanoDungeon(1142901));
                    Game1.warpFarmer(Destination, Dest_X, Dest_Y, flip: false);
                    return true;
                }
                else
                {
                    Monitor.Log("Failed to warp to '" + Destination + "': Festival not ready.");
                    Game1.drawObjectDialogue(Game1.parseText(Helper.Translation.Get("RestStop.WarpFestival")));
                    return false;
                }
            }
            else
            {
                Monitor.Log("Failed to warp to '" + Destination + "': Location not found or player is at festival.");
                Game1.drawObjectDialogue(Game1.parseText(Helper.Translation.Get("RestStop.WarpFail")));
                return false;
            }
        }

        private static void DoTotemWarpEffects(Farmer who, Func<Farmer, bool> action)
        {
            who.jitterStrength = 1f;
            who.currentLocation.playSound("stardrop", null, null, StardewValley.Audio.SoundContext.Default);
            who.faceDirection(2);
            who.canMove = false;
            who.temporarilyInvincible = true;
            who.temporaryInvincibilityTimer = -4000;
            Game1.changeMusicTrack("none", false, StardewValley.GameData.MusicContext.Default);
            who.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[2]
            {
                new FarmerSprite.AnimationFrame(57, 2000, false, false,  null, false),
                new FarmerSprite.AnimationFrame( (short) who.FarmerSprite.CurrentFrame, 0, false, false, new AnimatedSprite.endOfAnimationBehavior((f) => {
                    if (action(f))
                    {
                    } else
                    {
                        who.temporarilyInvincible = false;
                        who.temporaryInvincibilityTimer = 0;
                    }
                }), true)
            }, null);
            // reflection
            Multiplayer mp = ModEntry.Helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();
            // --
            mp.broadcastSprites(who.currentLocation,
            new TemporaryAnimatedSprite(354, 9999f, 1, 999, who.Position + new Vector2(0.0f, -96f), false, false, false, 0.0f)
            {
                motion = new Vector2(0.0f, -1f),
                scaleChange = 0.01f,
                alpha = 1f,
                alphaFade = 0.0075f,
                shakeIntensity = 1f,
                initialPosition = who.Position + new Vector2(0.0f, -96f),
                xPeriodic = true,
                xPeriodicLoopTime = 1000f,
                xPeriodicRange = 4f,
                layerDepth = 1f
            },
            new TemporaryAnimatedSprite(354, 9999f, 1, 999, who.Position + new Vector2(-64f, -96f), false, false, false, 0.0f)
            {
                motion = new Vector2(0.0f, -0.5f),
                scaleChange = 0.005f,
                scale = 0.5f,
                alpha = 1f,
                alphaFade = 0.0075f,
                shakeIntensity = 1f,
                delayBeforeAnimationStart = 10,
                initialPosition = who.Position + new Vector2(-64f, -96f),
                xPeriodic = true,
                xPeriodicLoopTime = 1000f,
                xPeriodicRange = 4f,
                layerDepth = 0.9999f
            },
            new TemporaryAnimatedSprite(354, 9999f, 1, 999, who.Position + new Vector2(64f, -96f), false, false, false, 0.0f)
            {
                motion = new Vector2(0.0f, -0.5f),
                scaleChange = 0.005f,
                scale = 0.5f,
                alpha = 1f,
                alphaFade = 0.0075f,
                delayBeforeAnimationStart = 20,
                shakeIntensity = 1f,
                initialPosition = who.Position + new Vector2(64f, -96f),
                xPeriodic = true,
                xPeriodicLoopTime = 1000f,
                xPeriodicRange = 4f,
                layerDepth = 0.9988f
            });
            Game1.screenGlowOnce(color, false, 0.005f, 0.3f);
            Utility.addSprinklesToLocation(who.currentLocation, Convert.ToInt32(who.Tile.X), Convert.ToInt32(who.Tile.Y), 16, 16, 1300, 20, color, null, true);
        }

    }


}