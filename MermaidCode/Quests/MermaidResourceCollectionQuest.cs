using System.Xml.Serialization;
using Netcode;
using System;
using Microsoft.Xna.Framework;
using StardewValley.Quests;
using StardewValley;
using StardewValley.Extensions;

namespace RestStopCode
{
	[XmlType("Mods_ApryllForever_RestStopCode_MermaidResourceCollectionQuest")]

	public class MermaidResourceCollectionQuest : Quest
	{
		[XmlElement("target")]
		public readonly NetString target = new NetString();

		[XmlElement("targetMessage")]
		public readonly NetString targetMessage = new NetString();

		[XmlElement("numberCollected")]
		public readonly NetInt numberCollected = new NetInt();

		[XmlElement("number")]
		public readonly NetInt number = new NetInt();

		[XmlElement("reward")]
		public readonly NetInt reward = new NetInt();

		[XmlElement("resource")]
		public readonly NetString resource = new NetString();

		//[XmlElement("deliveryItem")]
		//public readonly NetRef<Object> deliveryItem = new NetRef<Object>();

		public readonly NetDescriptionElementList parts = new NetDescriptionElementList();

		public readonly NetDescriptionElementList dialogueparts = new NetDescriptionElementList();

		[XmlElement("objective")]
		public readonly NetDescriptionElementRef objective = new NetDescriptionElementRef();

        [XmlElement("ItemId")]
        public readonly NetString ItemId = new NetString();


        public MermaidResourceCollectionQuest()
		{
			base.questType.Value = 10;
		}

		protected override void initNetFields()
		{
			base.initNetFields();
			base.NetFields.AddField(this.parts).AddField(this.dialogueparts).AddField(this.objective).AddField(this.target).AddField(this.targetMessage).AddField(this.numberCollected).AddField(this.number).AddField(this.reward).AddField(this.resource).AddField(this.ItemId);
		}

		public void loadQuestInfo()
		{
			if (this.target.Value != null || Game1.gameMode == 6)
			{
				return;
			}
			base.questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13640");

            int randomResource;
            randomResource = base.random.Next(6) * 2;


           
			for (int i = 0; i < base.random.Next(1, 100); i++)
			{
				base.random.Next();
			}
			int highest_mining_level = 0;
			int highest_foraging_level = 0;
			foreach (Farmer farmer2 in Game1.getAllFarmers())
			{
				highest_mining_level = Math.Max(highest_mining_level, farmer2.MiningLevel);
			}
			foreach (Farmer farmer in Game1.getAllFarmers())
			{
				highest_foraging_level = Math.Max(highest_foraging_level, farmer.ForagingLevel);
			}
            switch (randomResource)
            {
				case 0:
                    this.ItemId.Value = "(O)378";
					this.number.Value = 20 + highest_mining_level * 2 + base.random.Next(-2, 4) * 2;
					this.reward.Value = (int)this.number * 10;
					this.number.Value = (int)this.number - (int)this.number % 5;
					this.target.Value = "ElfEmerald";
					break;
				case 2:
                    this.ItemId.Value = "(O)380";
					this.number.Value = 15 + highest_mining_level + base.random.Next(-1, 3) * 2;
					this.reward.Value = (int)this.number * 15;
					this.number.Value = (int)((float)(int)this.number * 0.75f);
					this.number.Value = (int)this.number - (int)this.number % 5;
					this.target.Value = "ElfEmerald";
					break;
				case 4:
                    this.ItemId.Value = "(O)382";
					this.number.Value = 10 + highest_mining_level + base.random.Next(-1, 3) * 2;
					this.reward.Value = (int)this.number * 25;
					this.number.Value = (int)((float)(int)this.number * 0.75f);
					this.number.Value = (int)this.number - (int)this.number % 5;
					this.target.Value = "ElfEmerald";
					break;
				case 6:
                    this.ItemId.Value = ((Utility.GetAllPlayerDeepestMineLevel() > 40) ? "(O)384" : "(O)378");
                    this.number.Value = 8 + highest_mining_level / 2 + base.random.Next(-1, 1) * 2;
					this.reward.Value = (int)this.number * 30;
					this.number.Value = (int)((float)(int)this.number * 0.75f);
					this.number.Value = (int)this.number - (int)this.number % 2;
					this.target.Value = "ElfEmerald";
					break;
				case 8:
                    this.ItemId.Value = "(O)388";
                    this.number.Value = 25 + highest_foraging_level + base.random.Next(-3, 3) * 2;
					this.number.Value = (int)this.number - (int)this.number % 5;
					this.reward.Value = (int)this.number * 8;
					this.target.Value =  (Game1.random.NextDouble() <.4) ? "ROTMGrampa" : "SapphireLillia";
					break;
				case 10:
              
                    this.ItemId.Value = "(O)390";
					this.number.Value = 25 + highest_mining_level + base.random.Next(-3, 3) * 2;
					this.number.Value = (int)this.number - (int)this.number % 5;
					this.reward.Value = (int)this.number * 8;
					this.target.Value = "SapphireLillia";
					break;
				case 12:
                    this.ItemId.Value = "(O)770";
                    this.number.Value = (1 + highest_foraging_level);
                    this.reward.Value = 300 * highest_foraging_level;
                    this.target.Value = "Caitlynn";
                    break;
                case 14:
                    this.ItemId.Value = "(O)771";
                    this.number.Value = (5 + highest_foraging_level);
                    this.reward.Value = 300 * highest_foraging_level;
                    this.target.Value = "Caitlynn";
                    break;
                case 16:
                    this.ItemId.Value = "(O)330";
                    this.number.Value = 6;
                    this.reward.Value = 696;
                    this.target.Value = "Ciarra";
                    break;
            }
			if (this.target.Value == null)
			{
				return;
			}


            Item item;
            item = ItemRegistry.Create(this.ItemId.Value);
            if (this.ItemId.Value != "(O)388" && this.ItemId.Value != "(O)390" && this.ItemId.Value != "(O)770" && this.ItemId.Value != "(O)771" && this.ItemId.Value != "(O)330")
			{
				this.parts.Clear();
				int rand = base.random.Next(4);
				//this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Emerald.Ask.13647", this.number.Value, this.deliveryItem.Value, new DescriptionElement[4]{"Strings\\StringsFromCSFiles:Emerald.Ask.13649", "Strings\\StringsFromCSFiles:Emerald.Ask.13650", "Strings\\StringsFromCSFiles:Emerald.Ask.13651", "Strings\\StringsFromCSFiles:Emerald.Ask.13652" }.ElementAt(rand)));

                this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Emerald.Ask.13647", this.number.Value, item, new DescriptionElement("Strings\\StringsFromCSFiles:Emerald.Ask." + (new string[4] { "13649", "13650", "13651", "13652" })[rand])));

                if (rand == 3)
				{
					this.dialogueparts.Clear();
					this.dialogueparts.Add("Strings\\StringsFromCSFiles:Emerald.Ask.13655");
					this.dialogueparts.Add((base.random.NextDouble() < 0.3) ? "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13656" : ((base.random.NextDouble() < 0.5) ? "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13657" : "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13658"));
					this.dialogueparts.Add("Strings\\StringsFromCSFiles:Emerald.Ask.13659");
				}
				else
				{
					this.dialogueparts.Clear();
					this.dialogueparts.Add("Strings\\StringsFromCSFiles:Emerald.Ask.13662");
					this.dialogueparts.Add((base.random.NextDouble() < 0.3) ? "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13656" : ((base.random.NextDouble() < 0.5) ? "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13657" : "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13658"));
					//this.dialogueparts.Add((base.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:Emerald.Ask.13667", (base.random.NextDouble() < 0.3) ? new DescriptionElement("Strings\\StringsFromCSFiles:Emerald.Ask.13668") : ((base.random.NextDouble() < 0.5) ? new DescriptionElement("Strings\\StringsFromCSFiles:Emerald.Ask.13669") : new DescriptionElement("Strings\\StringsFromCSFiles:Emerald.Ask.13670"))) : ((DescriptionElement)"Strings\\StringsFromCSFiles:Emerald.Ask.13672"));

                    this.dialogueparts.Add(base.random.NextBool() ? new DescriptionElement("Strings\\StringsFromCSFiles:Emerald.Ask.13667", new DescriptionElement("Strings\\StringsFromCSFiles:Emerald.Ask." + base.random.Choose("13668", "13669", "13670"))) : new DescriptionElement("Strings\\StringsFromCSFiles:Emerald.Ask.13672"));

                    this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13677", (this.ItemId.Value == "(O)388") ? new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13678") : new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13679")));



                    this.dialogueparts.Add("Strings\\StringsFromCSFiles:Emerald.Ask.13673");
				}
			}
			else if ( this.ItemId.Value != "(O)770" && this.ItemId.Value != "(O)771" && this.ItemId.Value != "(O)330")
                {
				this.parts.Clear();
				this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Wood.13674", this.number.Value, item, this.target.Value));
				this.dialogueparts.Clear();
				this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13677", (this.ItemId.Value != "(O)388" ? new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13678") : new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13679"))));
				this.dialogueparts.Add((base.random.NextDouble() < 0.3) ? "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13681" : ((base.random.NextDouble() < 0.5) ? "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13682" : "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13683"));
			}

            else if (this.ItemId.Value == "(O)770" || this.ItemId.Value == "(O)771")
            {
                this.parts.Clear();
                this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Bio.13674", this.number.Value, item));
                this.dialogueparts.Clear();
                this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13677", (this.ItemId.Value == "(O)388") ? new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13678") : new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13679")));
                this.dialogueparts.Add((base.random.NextDouble() < 0.3) ? "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13681" : ((base.random.NextDouble() < 0.5) ? "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13682" : "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13683"));
            }
            else if (this.ItemId.Value == "(O)330")
            {
                this.parts.Clear();
                this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Clay.1", this.number.Value, item));
                this.dialogueparts.Clear();
                this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Clay.2"));
                this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:Clay.3"));
            }

            this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13607", this.reward.Value));
			this.parts.Add(this.target.Value.Equals("ElfEmerald") ? "Strings\\StringsFromCSFiles:Emerald.Ask.13688" : "");
			this.objective.Value = new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13691", "0", this.number.Value, item);
		}

		public override void reloadDescription()
		{
			if (base._questDescription == "")
			{
				this.loadQuestInfo();
			}
			if (this.parts.Count == 0 || this.parts == null || this.dialogueparts.Count == 0 || this.dialogueparts == null)
			{
				return;
			}
			string descriptionBuilder = "";
			string messageBuilder = "";
			foreach (DescriptionElement a in this.parts)
			{
				descriptionBuilder += a.loadDescriptionElement();
			}
			foreach (DescriptionElement b in this.dialogueparts)
			{
				messageBuilder += b.loadDescriptionElement();
			}
			base.questDescription = descriptionBuilder;
			this.targetMessage.Value = messageBuilder;
		}

		public override void reloadObjective()
		{
            if ((int)this.numberCollected < (int)this.number)
            {
                Item item;
                item = ItemRegistry.Create(this.ItemId.Value);
                this.objective.Value = new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13691", this.numberCollected.Value, this.number.Value, item);
            }
            if (this.objective.Value != null)
            {
                base.currentObjective = this.objective.Value.loadDescriptionElement();
            }
        }

		public override bool checkIfComplete(NPC n = null, int resourceCollected = -1, int amount = -1, Item item = null, string monsterName = null, bool probe = false)
		{
			if ((bool)base.completed)
			{
				return false;
			}
            if (n == null && item?.QualifiedItemId == this.ItemId.Value && amount != -1 && (int)this.numberCollected < (int)this.number)
            {
				this.numberCollected.Value = Math.Min(this.number, (int)this.numberCollected + amount);
                if ((int)this.numberCollected >= (int)this.number)
                {
					NPC actualTarget = Game1.getCharacterFromName(this.target);
					this.objective.Value = new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13277", actualTarget);
					Game1.playSound("jingle1");
				}
				Game1.dayTimeMoneyBox.moneyDial.animations.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Rectangle(387, 497, 3, 8), 800f, 1, 0, Game1.dayTimeMoneyBox.position + new Vector2(228f, 244f), flicker: false, flipped: false, 1f, 0.01f, Color.White, 4f, 0.3f, 0f, 0f)
				{
					scaleChangeChange = -0.012f
				});
			}
			else if (n != null && this.target.Value != null && (int)this.numberCollected >= (int)this.number && n.Name.Equals(this.target.Value) && n.isVillager())
			{
                n.CurrentDialogue.Push(new Dialogue(n, null, this.targetMessage));
                base.moneyReward.Value = this.reward;
				n.Name.Equals("Robin");
				this.questComplete();
				Game1.drawDialogue(n);
				return true;
			}
			return false;
		}
	}

}