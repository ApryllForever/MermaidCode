using StardewModdingAPI;
using System.Linq;
using StardewModdingAPI.Events;
using StardewValley.Menus;

namespace RestStopCode
{
    /// <summary>Adds a pre-defined set of custom NPC names to the <see cref="SpecialOrdersBoard"/> list.</summary>
    public static class SpecialOrderNPCIcons
    {

        public static bool Enabled { get; private set; } = false;

        private static IModHelper Helper { get; set; } = null;

        private static IMonitor Monitor { get; set; } = null;

        public static void Enable(IModHelper helper, IMonitor monitor)
        {
            if (!Enabled && helper != null && monitor != null) 
            {
                Helper = helper; 
                Monitor = monitor; 

                Helper.Events.Display.MenuChanged += MenuChanged_UpdateSpecialOrdersBoard;

                Enabled = true;
            }
        }


        /* Mod Settings */
       
        /// <summary>The list of custom NPCs who have emoji icons added to "LooseSprites/emojis".</summary>
        /// <remarks>
        /// Add each NPC's name here, in quotations and separated by commas. This is case-sensitive and based on their "internal" name, not their display name.
        /// Note that this class DOES NOT edit the emoji spritesheet. Do that with Content Patcher or a SMAPI asset editor.
        /// </remarks>
        private static string[] CustomNPCsWithEmoji { get; set; } = new string[]
        {
            "","","","","","","","","","","", "","","Vika", "Oksana", "Colleen", "RangerWren", "NaKeshia"
        };


        /*****************/
        /* Internal Code */
        /*****************/


        /// <summary>The expected, default length of <see cref="SpecialOrdersBoard.emojiIndices"/>.</summary>
        /// <remarks>Based on Stardew Valley v1.5.4. If this number seems incorrect, check that class's decompiled code.</remarks>
        private const int DefaultEmojiLength = 38;

        private static void MenuChanged_UpdateSpecialOrdersBoard(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is SpecialOrdersBoard board) //if the player's current menu is a special orders board
            {
                if (board.emojiIndices?.Length != DefaultEmojiLength) //if the emoji name array is NOT the expected, default size
                    Monitor.LogOnce($"\"SpecialOrdersBoard.emojiIndices\" doesn't match default length: {board.emojiIndices?.Length.ToString() ?? "null"} when it should be {DefaultEmojiLength}. Still loading icons, but they might conflict with a game update or another mod.", LogLevel.Trace);

                board.emojiIndices = board.emojiIndices.Concat(CustomNPCsWithEmoji).ToArray(); //add the custom NPC names to the end of the array
                Monitor.LogOnce($"Added {CustomNPCsWithEmoji.Length} custom NPC names to the special orders board emoji list.", LogLevel.Trace);
            }
        }
    }
}