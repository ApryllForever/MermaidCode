using StardewValley;
using System.Collections.Generic;

namespace RestStopLocations.Quests
{
    public static class QuestDictionaries

    {

        public static List<string> OksanaSmuggleItems()
        {
            List<string> list = null;

            list = new List<string> { "288", "432", "367", "421", "787", }; //MegaBomb, truffle oil, poppy, sunflower, battery, 

           if (Game1.player.hasOrWillReceiveMail("Island_UpgradeHouse"))
            {
                list.Add("857"); //tigerslime egg
            };


            //explosive ammo 441, radioactive ore 909, fertilizer 368, poppy seeds 453, 



            return list;
        }

        public static List<string> OksanaCookItems()
        {
            List<string> list = null;

            list = new List<string> { "419", "423", "192", "245", "246", "247", "270", "276", "280" }; //vinegar, rice, potate, sugar, flour, oil, corn, pumpkin, yam

            if (Game1.player.hasOrWillReceiveMail("Island_UpgradeHouse"))
            {
                //list.Add(857); //tigerslime egg
            };


            //explosive ammo 441, radioactive ore 909, fertilizer 368, poppy seeds 453, Torch 93, corn 270, 



            return list;
        }


        public static List<string> VikaItems()
        {
            List<string> list = null;

            list = new List<string> { "684", "690", "206", "253", }; //bug meat, warp totem beach, pizza, expresso

            if (Game1.player.hasOrWillReceiveMail("Island_UpgradeHouse"))
            {
                list.Add("834"); //mango
            };


            //explosive ammo 441, radioactive ore 909, fertilizer 368, poppy seeds 453



            return list;
        }


        public static List<string> ColleenItems()
        {
            List<string> list = null;

            list = new List<string> { "773", "166", "233", }; //life elixer, treasure chest, ice cream

            if (Game1.player.hasOrWillReceiveMail("Island_UpgradeHouse"))
            {
                //list.Add(857); //tigerslime egg
            };


            //explosive ammo 441, radioactive ore 909, fertilizer 368, poppy seeds 453



            return list;
        }




        //static Dictionary<int, string> sapphireQuestList = new Dictionary<int, string>();






    }


}




