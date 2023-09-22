﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Quests;
using StardewModdingAPI;
using UtilitiesStuff;

namespace RestStopLocations.Quests
{
	public class OksanaBoard : Billboard
	{
		static IModHelper Helper { get; set; }
		int timestampOpened;
		static int safetyTimer = 500;
		internal Quest dailyQuest;
		private QuestData questData;
		private string boardType;
		Texture2D billboardTexture;
		Color fontColor = Game1.textColor;
		string description;

		internal OksanaBoard(QuestData questData, string boardType = "") : base(dailyQuest: true)
		{
			this.questData = questData;
			this.boardType = boardType;
			timestampOpened = (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds;
			
			{
				this.dailyQuest = questData.dailyOksanaQuest;
				this.acceptQuestButton.visible = !questData.acceptedDailyOksanaQuest;
			}
			if (dailyQuest != null)
			{
				this.description = dailyQuest.questDescription;
			}
			else
			{
				this.description = "";
			}


			Texture2D billboardTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\RangerBoard");
			Log.Debug($"{boardType}, {boardType.Equals("")}");
			
			this.billboardTexture = billboardTexture;

			


		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (this.timestampOpened + safetyTimer < Game1.currentGameTime.TotalGameTime.TotalMilliseconds)
			{
				base.receiveRightClick(x, y, playSound);
			}
			return;
		}

		public override void draw(SpriteBatch b)
		{

			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
			
				b.Draw(this.billboardTexture, new Vector2(base.xPositionOnScreen, base.yPositionOnScreen), null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);


			if (this.dailyQuest == null || this.dailyQuest.currentObjective == null || this.dailyQuest.currentObjective.Length == 0)
			{
				b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\UI:Billboard_NothingPosted"), new Vector2(base.xPositionOnScreen + 384, base.yPositionOnScreen + 320), this.fontColor);
			}
			else
			{
				SpriteFont font = ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ko) ? Game1.smallFont : Game1.dialogueFont);
				string description = Game1.parseText(this.description, font, 640);
				Utility.drawTextWithShadow(b, description, font, new Vector2(base.xPositionOnScreen + 320 + 32, base.yPositionOnScreen + 256), this.fontColor, 1f, -1f, -1, -1, 0.5f);
				if (this.acceptQuestButton.visible)
				{

					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.acceptQuestButton.bounds.X, this.acceptQuestButton.bounds.Y, this.acceptQuestButton.bounds.Width, this.acceptQuestButton.bounds.Height, (this.acceptQuestButton.scale > 1f) ? Color.LightPink : Color.White, 4f * this.acceptQuestButton.scale);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:AcceptQuest"), Game1.dialogueFont, new Vector2(this.acceptQuestButton.bounds.X + 12, this.acceptQuestButton.bounds.Y + (LocalizedContentManager.CurrentLanguageLatin ? 16 : 12)), this.fontColor);
				}
			}

			if (this.upperRightCloseButton != null && this.shouldDrawCloseButton())
			{
				this.upperRightCloseButton.draw(b);
			}


			{

				base.drawMouse(b);
			}
		}

		public override void performHoverAction(int x, int y)
		{
			if (dailyQuest != null && this.acceptQuestButton.visible)
			{
				float oldScale = this.acceptQuestButton.scale;
				this.acceptQuestButton.scale = (this.acceptQuestButton.bounds.Contains(x, y) ? 1.5f : 1f);
				if (this.acceptQuestButton.scale > oldScale)
				{
					Game1.playSound("Cowboy_gunshot");
				}

				if (this.upperRightCloseButton != null)
				{
					this.upperRightCloseButton.tryHover(x, y, 0.5f);
				}
			}

		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			return;
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.timestampOpened + safetyTimer > Game1.currentGameTime.TotalGameTime.TotalMilliseconds)
			{
				return;
			}
			if (this.acceptQuestButton.visible && this.acceptQuestButton.containsPoint(x, y) && this.dailyQuest != null)
			{
				Game1.playSound("newArtifact");

					this.questData.acceptedDailyOksanaQuest = true;
				
				Game1.player.questLog.Add(this.dailyQuest);

				this.acceptQuestButton.visible = false;
			}
			else if (this.upperRightCloseButton != null && this.upperRightCloseButton.containsPoint(x, y))
			{
				if (playSound)
				{
					Game1.playSound("bigDeSelect");
				}
				this.exitThisMenu();
			}
		}
	}
}




