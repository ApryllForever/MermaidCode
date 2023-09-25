
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Quests;
using StardewModdingAPI.Utilities;
using Microsoft.Xna.Framework.Graphics;
using UtilitiesStuff;
using RestStopCode;
using StardewValley.Tools;
using SpaceCore.Events;

namespace RestStopLocations.Quests
{
	public static class QuestController
	{

		public static int VikaBug1Counter = 0;

		static IModHelper Helper;
		static IMonitor Monitor;
		//static Lazy<Texture2D> questionMarkSprite = new Lazy<Texture2D>(() => Helper.Content.Load<Texture2D>(PathUtilities.NormalizePath("assets/questMark.png"), ContentSource.ModFolder));

		//store available quest data for each user
		internal static readonly PerScreen<QuestData> dailyQuestData = new PerScreen<QuestData>();

		private static readonly PerScreen<LocationForMarkers> CurrentLocationFormarkers = new(() => LocationForMarkers.Other);

		//is board unlocked in the current map (if there is one)
		private static readonly PerScreen<bool> SOBoardUnlocked = new();
		private static readonly PerScreen<bool> QuestBoardUnlocked = new();
		private static readonly PerScreen<bool> OrdersGenerated = new();

		internal static readonly PerScreen<HashSet<string>> FinishedQuests = new(() => new());

		private enum LocationForMarkers
		{
			SapphireSprings,
			RangerStation,
			Other
		}

		internal static void Initialize(IMod ModInstance)
		{
			Helper = ModInstance.Helper;
			Monitor = ModInstance.Monitor;
			//TileActionHandler.RegisterTileAction("MermaidBoard", OpenQuestBoard);
			TileActionHandler.RegisterTileAction("MermaidSpecialOrderBoard", OpenSOBoard);

			TileActionHandler.RegisterTileAction("SapphireQuestBoard", OpenSapphireQuestBoard);
			TileActionHandler.RegisterTileAction("OksanaBoard", OpenOksanaBoard);

			Helper.ConsoleCommands.Add("MermaidSOBoard.refresh", "", (s1, s2) => {
				MermaidSpecialOrderBoard.UpdateAvailableMermaidSpecialOrders(force_refresh: true);
				Log.Info("Rise of the Mermaids Special Orders refreshed.");
			});

			/* 1.6. Now how do you feel about yourself???????
			Helper.ConsoleCommands.Add("Mermaids.AddQuest", "", (s1, s2) => {
				var quest = QuestFactory.getQuestFromId(int.Parse(s2[0]));
				if (quest != null)
				{
					Game1.player.questLog.Add(quest);
					Log.Info("Quest added.");
				}
				else
				{

					Log.Info("Quest not found.");
				}
			});  */

			Helper.ConsoleCommands.Add("Mermaid.QuestState", "", (s1, s2) => PrintQuestState());
			Helper.ConsoleCommands.Add("Mermaid.CheckQuests", "", (s1, s2) => CheckQuests());
			Helper.Events.GameLoop.DayStarted += OnDayStarted;
			Helper.Events.Player.Warped += OnWarped;
			Helper.Events.Display.RenderedWorld += RenderQuestMarkersIfNeeded;
			Helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
			Helper.Events.GameLoop.DayEnding += OnDayEnding;
			SpaceEvents.OnEventFinished += OnEventFinished;


			OrdersGenerated.Value = false;
		}

		private static void CheckQuests() //1.6
		{
			var questData = Helper.GameContent.Load<Dictionary<int, string>>(PathUtilities.NormalizeAssetName("Data/Quests"));
			foreach (var key in questData.Keys)
			{
				try
				{
					Log.Debug($"Checking {key} DOES NOT WORK, 1.6");
					//QuestFactory.getQuestFromId(key);

				}
				catch (Exception e)
				{
					Log.Error($"Failed for quest ID {key}. Stacktrace in Trace");
					Log.Trace(e.Message);
					Log.Trace(e.StackTrace);
				}
			}
		}

		static void PrintQuestState()
		{
			foreach (var data in dailyQuestData.GetActiveValues())
			{
				var questData = data.Value;
				//Log.Debug($"MermaidQuest: {questData?.dailySapphireQuest?.id}");
				//Log.Debug($"Mermaidquest accepted: {questData.acceptedDailySapphireQuest}");
				//Log.Debug($"RangerQuest: {questData?.dailyRangerQuest?.id}");
				//Log.Debug($"RangerQuest accepted: {questData.acceptedDailyRangerQuest}");
			

				Log.Debug($"SapphireBoardQuest: {questData?.dailySapphireBoardQuest?.id}");
				Log.Debug($"SapphireBoardquest accepted: {questData.acceptedDailySapphireBoardQuest}");
				
				Log.Debug($"OksanaQuest: {questData?.dailyOksanaQuest?.id}");
				Log.Debug($"Oksanaquest accepted: {questData.acceptedDailyOksanaQuest}");
                Log.Debug($"Quests done: {string.Join(",", FinishedQuests.Value)}");
            }
		}

		//save the players dailies
		private static void OnDayEnding(object sender, DayEndingEventArgs e)
		{
			Game1.player.modData["RiseoftheMermaids.DailiesDone"] = string.Join(",", FinishedQuests.Value);
			if (OrdersGenerated.Value && Game1.dayOfMonth % 7 == 0 && Game1.player.IsMainPlayer)
			{
				OrdersGenerated.Value = false;
			}

		}

		//load the player's finished quests
		private static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
		{
			if (Game1.player.modData.TryGetValue("RS.DailiesDone", out string dailyisDone))
			{

				Log.Trace($"dailies Done: {dailyisDone}");
				FinishedQuests.Value = new HashSet<string>();
				if (!string.IsNullOrEmpty(dailyisDone))
				{
					foreach (string id in dailyisDone.Split(","))
					{
						FinishedQuests.Value.Add(id);
					}
				}
			}
		}

		private static void OnWarped(object sender, WarpedEventArgs e)
		{
			if (e.Player == Game1.player)
			{
				if (e.NewLocation.Name.Equals(MermaidConstants.L_SAPPHIRESPRINGS))
				{
					CurrentLocationFormarkers.Value = LocationForMarkers.SapphireSprings;
					SOBoardUnlocked.Value = true; //Game1.player.eventsSeen.Contains(MermaidConstants.E_SAPPHIREBOARD);

				}
				else if (e.NewLocation.Name.Equals(MermaidConstants.L_RANGERSTATION))
				{
					CurrentLocationFormarkers.Value = LocationForMarkers.RangerStation;
					SOBoardUnlocked.Value = true; //Game1.player.eventsSeen.Contains(MermaidConstants.E_RANGERBOARD);
					QuestBoardUnlocked.Value = true; //Game1.player.eventsSeen.Contains(MermaidConstants.E_RANGERQUESTS);
				}
				else
				{
					CurrentLocationFormarkers.Value = LocationForMarkers.Other;
					SOBoardUnlocked.Value = true;
					QuestBoardUnlocked.Value = true;
				}
			}

		}

		private static void RenderQuestMarkersIfNeeded(object sender, RenderedWorldEventArgs e)
		{
			SpriteBatch sb = e.SpriteBatch;
			switch (CurrentLocationFormarkers.Value)
			{
				case LocationForMarkers.SapphireSprings:
					float offset = 4f * (float)Math.Round(Math.Sin(Game1.currentGameTime.TotalGameTime.TotalMilliseconds / 250.0), 2);
					
					/*if (!dailyQuestData.Value.acceptedDailySapphireQuest)
					{
						sb.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(66.5f * 64f + 32f, 77.5f * 64f + 32f)),
							new Rectangle(395, 497, 3, 8), Color.White, 0f, new Vector2(1f, 4f), 4f + Math.Max(0f, 0.25f - offset / 16f), SpriteEffects.None, 1f);

					}*/

					if ( !Game1.player.team.acceptedSpecialOrderTypes.Contains("SapphireSO") && Game1.player.team.GetAvailableSpecialOrder(type: "SapphireSO") != null)
					{
						Vector2 questMarkPosition = new Vector2(56f * 64f + 27f, 68f * 64f);
						sb.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(56f * 64f + 32f, 68f * 64f + 32f)),
							new Rectangle(395, 497, 3, 8), Color.White, 0f, new Vector2(1f, 4f), 4f + Math.Max(0f, 0.25f - offset / 16f), SpriteEffects.None, 1f);
					}
					if (!dailyQuestData.Value.acceptedDailySapphireBoardQuest)
					{
						sb.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(66.5f * 64f + 32f, 77.5f * 64f + 32f)),
							new Rectangle(395, 497, 3, 8), Color.White, 0f, new Vector2(1f, 4f), 4f + Math.Max(0f, 0.25f - offset / 16f), SpriteEffects.None, 1f);

					}

					break;
				case LocationForMarkers.RangerStation:
					offset = 4f * (float)Math.Round(Math.Sin(Game1.currentGameTime.TotalGameTime.TotalMilliseconds / 250.0), 2);
					
					/*if (!dailyQuestData.Value.acceptedDailyRangerQuest)
					{
						sb.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(6f * 64f + 32f, 5.5f * 64f + offset)),
							new Rectangle(395, 497, 3, 8), Color.White, 0f, new Vector2(1f, 4f), 4f + Math.Max(0f, 0.25f - offset / 16f), SpriteEffects.None, 1f);

					}*/
					if ( /*SOBoardUnlocked.Value && */!Game1.player.team.acceptedSpecialOrderTypes.Contains("RangerSO") && Game1.player.team.GetAvailableSpecialOrder(type: "RangerSO") != null)
					{
						sb.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(3f * 64f + 32f, 3f * 64f + offset)),
							new Rectangle(395, 497, 3, 8), Color.White, 0f, new Vector2(1f, 4f), 4f + Math.Max(0f, 0.25f - offset / 16f), SpriteEffects.None, 1f);
					}


					offset = 4f * (float)Math.Round(Math.Sin(Game1.currentGameTime.TotalGameTime.TotalMilliseconds / 250.0), 2);
					if (!dailyQuestData.Value.acceptedDailyOksanaQuest)
					{
						sb.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(6f * 64f + 32f, 5.5f * 64f + offset)),
							new Rectangle(395, 497, 3, 8), Color.White, 0f, new Vector2(1f, 4f), 4f + Math.Max(0f, 0.25f - offset / 16f), SpriteEffects.None, 1f);

					}


					break;
			}
			return;
		}
		/*
		private static void OpenQuestBoard(string name, Vector2 position)
		{
			string type = name.Split()[^1];
			Log.Trace($"Opening MermaidBoard {type}");
			Log.Trace(dailyQuestData.ToString());
			Game1.activeClickableMenu = new MermaidBoard(dailyQuestData.Value, type);
		}*/

		private static void OpenSapphireQuestBoard(string name, Vector2 position)
		{
			string type = name.Split()[^1];
			Log.Trace($"Opening SapphireQuestBoard {type}");
			Log.Trace(dailyQuestData.ToString());
			Game1.activeClickableMenu = new SapphireQuestBoard(dailyQuestData.Value, type);
		}

		private static void OpenOksanaBoard(string name, Vector2 position)
		{
			string type = name.Split()[^1];
			Log.Trace($"Opening OksanaQuestBoard {type}");
			Log.Trace(dailyQuestData.ToString());
			Game1.activeClickableMenu = new OksanaBoard(dailyQuestData.Value, type);
		}

		private static void OpenSOBoard(string name, Vector2 position)
		{
			string type = name.Split()[^1];
			Log.Trace($"Opening MermaidSOBoard {type}");
			Game1.activeClickableMenu = new MermaidSpecialOrderBoard(type);
		}


		[EventPriority(EventPriority.Low - 101)]
		private static void OnDayStarted(object sender, DayStartedEventArgs e)
		{
			//if monday, update special orders
			if (Game1.dayOfMonth % 7 == 1 && Game1.player.IsMainPlayer)
			{
				MermaidSpecialOrderBoard.UpdateAvailableMermaidSpecialOrders(force_refresh: false);
			}
			try
			{
				Log.Trace($"Player has done following quests: {String.Join(",", FinishedQuests.Value)}");
				//Quest sapphireQuest = QuestFactory.GetDailyQuest();
				//Quest rangerQuest = QuestFactory.GetDailyRangerQuest();
				Quest sapphireBoardQuest = QuestFactory.GetDailyQuest();
				Quest oksanaQuest = QuestFactory.GetDailyRangerQuest();

				//Quest rangerQuest = null;
				//if (Game1.player.eventsSeen.Contains(75160187))
				//{
				//rangerQuest = QuestFactory.GetDailyRangerQuest();
				//}
				dailyQuestData.Value = new QuestData(/*sapphireQuest, rangerQuest,*/ sapphireBoardQuest, oksanaQuest);
			}
			catch
			{
				dailyQuestData.Value = new QuestData(/*null, null,*/ null, null);
				Log.Trace("Failed parsing new quests.");
			}
		}



		static void OnEventFinished(object sender, EventArgs e)
		{
			if (!Game1.player.IsMainPlayer)
				return;

			switch (Game1.CurrentEvent.id)
			{
				case "17333001":
					//UtilFunctions.TryRemoveQuest(MermaidConstants.Q_PREPCOMPLETE);
					//UtilFunctions.TryCompleteQuest(MermaidConstants.Q_NINJANOTE);
					Game1.player.addQuest("1733301");
					break;

				case "17333002":

                    //string putrescence =               //ExternalAPIs.JA.GetWeaponId("Sword of Putresence");

                    Game1.player.completeQuest("1733301");
					Game1.player.mailForTomorrow.Add("Mermaid.17333LeahKnife");
					Game1.player.addItemToInventory(new MeleeWeapon("ApryllForever.RiseMermaids_SwordofPutrescence)"));
                    break;


				case "12764002":
					Game1.player.completeQuest("1276401");

                    Game1.player.Money += 696;

                    break;

				case "11429701":




                    Game1.player.modData["ApryllForever.RestStopCode/VikaBug1Counter"] = VikaBug1Counter.ToString();



                    break;

				case "Mermaid.Alla8HeartEvent":

					if (Game1.player.mailReceived.Contains("MermaidAllaDatingYes"))
						{
						Friendship friendship = Game1.player.friendshipData["Alla"];
						friendship.Status = FriendshipStatus.Dating;
					}
                    break;





                    /*
				case MermaidConstants.E_OPENPORTAL:
					UtilFunctions.TryCompleteQuest(MermaidConstants.Q_OPENPORTAL);
					if (Game1.player.IsMainPlayer)
					{
						Game1.player.team.specialOrders.Add(SpecialOrder.GetSpecialOrder(MermaidConstants.SO_CLEANSING, null));
					}
					break;  */


            }
		}
	}


	internal class QuestData
	{
		//internal Quest dailySapphireQuest;
		//internal bool acceptedDailySapphireQuest;
		//internal Quest dailyRangerQuest;
		//internal bool acceptedDailyRangerQuest;

		internal Quest dailySapphireBoardQuest;
		internal bool acceptedDailySapphireBoardQuest;

		internal Quest dailyOksanaQuest;
		internal bool acceptedDailyOksanaQuest;

		internal QuestData(/*Quest dailySQuest, Quest dailyRSQuest,*/ Quest dailySapphireBoardQuest, Quest dailyOksanaQuest)
		{
			//this.dailySapphireQuest = dailySQuest;
			//this.acceptedDailySapphireQuest = dailySQuest is null;
			//this.dailyRangerQuest = dailyRSQuest;
			//this.acceptedDailyRangerQuest = dailyRSQuest is null;

			this.dailySapphireBoardQuest = dailySapphireBoardQuest;
			this.acceptedDailySapphireBoardQuest = dailySapphireBoardQuest is null;


			this.dailyOksanaQuest = dailyOksanaQuest;
			this.acceptedDailyOksanaQuest = dailyOksanaQuest is null;
		}

		public override string ToString()
		{
			return $"Quest data:  SapphireBoard ID {dailySapphireBoardQuest?.id} {acceptedDailySapphireBoardQuest}, OksanaBoard ID {dailyOksanaQuest?.id} {acceptedDailyOksanaQuest}";  //Sapphire ID {dailySapphireQuest?.id} {acceptedDailySapphireQuest}, Ranger ID {dailyRangerQuest?.id} {acceptedDailyRangerQuest},
        }
	}
}
