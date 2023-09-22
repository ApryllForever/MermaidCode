
using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Quests;
using RestStopCode;


namespace RestStopLocations.Quests
{

	static internal class QuestFactory
	{

		static IModHelper Helper;

		static Dictionary<int, string> sapphireQuestList = new Dictionary<int, string>();

		static internal Quest GetDailyQuest()
		{

			if (Game1.random.NextDouble() < 0.2)
			{
				return null;
			}
			else
			{
				return GetMermaidResourceCollectionQuest();
				//return GetFishingQuest();
			}
		}


		static internal Quest GetDailyRangerQuest()
		{

            if (Game1.random.NextDouble() < 0.01)
            {
                return GetOksanaQuest();
            }
            else
            {
                return GetMermaidQuest();
            }

        }
		/*
		static internal Quest GetRandomHandCraftedQuest()
		{
			//var questlist = Game1.content.Load<Dictionary<int,string>>
			//Dictionary<string, string> quests = Helper.ModContent.Load<Dictionary<string, string>>("Data/Quests.json");
			//IDictionary<string, string> quests = (IDictionary<string, string>)Helper.ModContent.Load<Dictionary<int, string>>("assets/Quests.json");
			var quests = sapphireQuestList;  //Game1.content.Load<Dictionary<int, string>>(Helper.ModContent.GetInternalAssetName("Data/Quests").BaseName);


			//Helper.GameContent.Load<Dictionary<int, string>>("Data/Quests.json");
			var candidates = new List<int>();
			foreach (var key in quests.Keys)
			{
				if (key >= 1276400 && key <= 1276900)
				{
					if (!Game1.player.hasQuest(key))
					{
						candidates.Add(key);
					}
				}
			}
			Log.Trace($"{candidates.Count} candidates for daily Quest");

			if (candidates.Count == 0)
			{
				return null;
			}

			int rand = Game1.random.Next(candidates.Count);

			Log.Trace($"chose {candidates[rand]}");
			return QuestFactory.getQuestFromId(candidates[rand]);
		}  */

		static internal Quest GetMermaidQuest()
		{
			MermaidItemDeliveryQuest quest = new MermaidItemDeliveryQuest();
            quest.loadQuestInfo();
            quest.id.Value = "3700000";
            return quest;
        }

        static internal Quest GetMermaidResourceCollectionQuest()
        {
            MermaidResourceCollectionQuest quest = new MermaidResourceCollectionQuest();
            quest.loadQuestInfo();
            quest.id.Value = "3700073";
            return quest;
        }
		/*
        static internal Quest GetFishingQuest()
		{
			FishingQuest quest = new FishingQuest();
			quest.loadQuestInfo();

			int[] possibleFish;
			switch (Game1.currentSeason)
			{
				case "spring":
					{
						 possibleFish = new [8] { 129, 131, 136, 137, 142, 143, 145, 147 };
						//int[] possiblefish2 = new int[8] { 129, 131, 136, 137, 142, 143, 145, 147 };
						//quest.whichFish.Value = possiblefish2[Game1.random.Next(possiblefish2.Length)];
						break;
					}
				case "summer":
					{
						possibleFish = new int[8] {132, 136, 137, 138, 142, 700, 701, 702 };
						break;
					}
				case "fall":
					{
						possibleFish = new int[5] { 137, 139, 140, 143, 699 };
						break;
					}
				case "winter":
					{
						possibleFish = new int[1] { 141 };
						break;
					}
				default:
					{
						possibleFish = new int[1] { 132 };
						break;
					}
			}
			int chosenFish = possibleFish[Game1.random.Next(possibleFish.Length)];
			//quest.whichFish.Value = ExternalAPIs.JA.GetObjectId(chosenFish);
			if (quest.whichFish.Value == -1)
			{
				quest.whichFish.Value = 132; //Bream as fallback

			}

			quest.fish.Value = new SObject(Vector2.Zero, quest.whichFish.Value, 1);
			quest.numberToFish.Value = (int)Math.Ceiling(200.0 / (double)Math.Max(1, quest.fish.Value.Price)) + Game1.player.FishingLevel / 5;
			quest.reward.Value = (int)(quest.numberToFish.Value + 1.5) * quest.fish.Value.Price;
			quest.target.Value = "Qualista";
			quest.parts.Clear();

			//have to patch the CSFiles for this... ugh
			//its in the CP part, data/Quests/StringsForQuests

			quest.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Qualista.FishingQuest.Description", quest.fish.Value, quest.numberToFish.Value)); //actual quest text
			quest.dialogueparts.Clear();
			quest.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Qualista.FishingQuest.HandInDialogue", quest.fish.Value));
			quest.objective.Value = new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13244", 0, quest.numberToFish.Value, quest.fish.Value); // progress

			quest.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13274", quest.reward.Value)); // reward
			quest.parts.Add("Strings\\StringsFromCSFiles:FishingQuest.cs.13275"); //keep fish note
			quest.daysLeft.Value = 7;
			quest.id.Value = 90000000;
			return quest;

		}  */

        static internal Quest GetOksanaQuest()
        {
            NPC npc = Game1.getCharacterFromName("Oksana");
            ItemDeliveryQuest quest = new ItemDeliveryQuest();



            //quest.target.Value.Equals(npc);


            quest.loadQuestInfo();
			//quest.GetValidTargetList().;



            string[] possibleItem;
            switch (Game1.currentSeason)
            {
                case "spring":
                    {
                        possibleItem = new string[8] { "129", "131", "136", "137", "142", "143", "145", "147" };
                        //int[] possiblefish2 = new int[8] { 129, 131, 136, 137, 142, 143, 145, 147 };
                        //quest.whichFish.Value = possiblefish2[Game1.random.Next(possiblefish2.Length)];
                        break;
                    }
                case "summer":
                    {
                        possibleItem = new string[8] { "129", "131", "136", "137", "142", "143", "145", "147" };
                        break;
                    }
                case "fall":
                    {
                        possibleItem = new string[5] { "137", "139", "140", "143", "699" };
                        break;
                    }
                case "winter":
                    {
                        possibleItem = new string[1] { "141" };
                        break;
                    }
                default:
                    {
                        possibleItem = new string[1] { "80" };
                        break;
                    }
            }
            string chosenItem = possibleItem[Game1.random.Next(possibleItem.Length)];

			

            if (quest.ItemId.Value is null)
            {
                quest.ItemId.Value = "(O)80"; //Quartz

            }

            Item item = null;

           
			quest.ItemId.Value = ItemRegistry.QualifyItemId(chosenItem);
            quest.moneyReward.Value = quest.GetGoldRewardPerItem(item);
            quest.target.Set("Oksana");
            //quest.target.Value.Equals(npc);
            quest.parts.Clear();

  

            quest.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Oksana.OksanaQuest.Description", quest.ItemId.Value)); //actual quest text
            quest.dialogueparts.Clear();
            quest.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Oksana.OksanaQuest.HandInDialogue", quest.ItemId.Value));
            quest.objective.Value = new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13614", quest.target.Value, quest.ItemId.Value); // progress

            quest.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13307", quest.moneyReward.Value)); // reward
            quest.parts.Add("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13308"); //will be happy
            quest.daysLeft.Value = 7;
            quest.id.Value = "99000000";
			quest.checkIfComplete(npc,-1,-1, ItemRegistry.Create(chosenItem));

            return quest;

        }

		/*
        static internal Quest getQuestFromId(string id)
		{
			Log.Trace($"Trying to load quest {id}");
			Quest quest = null;
			try
			{
				quest = Quest.getQuestFromId(id);

				if (quest is SlayMonsterQuest monsterQuest)
				{
					Dictionary<int, string> questData = Game1.temporaryContent.Load<Dictionary<int, string>>("Data\\Quests");

					if (questData != null && questData.ContainsKey(id))
					{
						string[] rawData = questData[id].Split('/');
						string questType = rawData[0];
						string[] conditionsSplit = rawData[4].Split(' ');

						monsterQuest.loadQuestInfo();
						monsterQuest.monster.Value.Name = conditionsSplit[0].Replace('_', ' ');
						monsterQuest.monsterName.Value = monsterQuest.monster.Value.Name;
						monsterQuest.numberToKill.Value = Convert.ToInt32(conditionsSplit[1]);
						if (conditionsSplit.Length > 2)
						{
							monsterQuest.target.Value = conditionsSplit[2];
						}
						else
						{
							monsterQuest.target.Value = "null";
						}
						monsterQuest.questType.Value = 4;
						if (rawData.Length > 9)
						{
							monsterQuest.targetMessage = rawData[9];
						}

						monsterQuest.moneyReward.Value = Convert.ToInt32(rawData[6]);
						monsterQuest.reward.Value = monsterQuest.moneyReward.Value;
						monsterQuest.rewardDescription.Value = (rawData[6].Equals("-1") ? null : rawData[7]);
						monsterQuest.parts.Clear();
						monsterQuest.dialogueparts.Clear();
						Helper.Reflection.GetField<bool>(monsterQuest, "_loadedDescription").SetValue(true);
						return monsterQuest;
					}
				}
			}
			catch (Exception e)
			{
				Log.Error($"Failed parsing quest with id {id}");
				Log.Error(e.Message);
				Log.Error(e.StackTrace);
				quest = null;
			}


			return quest;
		}  */
	}

}

