using System.Xml.Serialization;
using Netcode;
using StardewValley.Objects;
using StardewValley.Quests;
using RestStopLocations;
using StardewValley;
using Object = StardewValley.Object;

namespace RestStopCode
{
    [XmlType("Mods_ApryllForever_RestStopCode_MermaidCraftingQuest")]
    public class MermaidCraftingQuest : CraftingQuest
	{
        
        [XmlElement("buttisBigCraftable")]
		public readonly NetBool buttisBigCraftable = new NetBool();

		[XmlElement("buttindexToCraft")]
		public readonly NetInt buttindexToCraft = new NetInt();

		public MermaidCraftingQuest()
		{
		}

		public MermaidCraftingQuest(int indexToCraft, bool bigCraftable)
		{
			this.buttindexToCraft.Value = indexToCraft;
			this.buttisBigCraftable.Value = bigCraftable;
		}

		protected override void initNetFields()
		{
			base.initNetFields();
			base.NetFields.AddField(this.buttisBigCraftable).AddField(this.buttindexToCraft);
		}

		public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -2, Item item = null, string str = null, bool probe = false)
		{
			if (item is Clothing)
			{
				return false;
			}
			if (item != null && item is Object && (item as Object).bigCraftable.Value == this.buttisBigCraftable.Value && (item as Object).parentSheetIndex.Value == this.buttindexToCraft.Value)
			{
				this.questComplete();
				return true;
			}
			return false;
		}
	}
}