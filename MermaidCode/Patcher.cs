

using HarmonyLib;
using StardewModdingAPI;
using RestStopLocations;

namespace RestStopCode
{
    class Patcher
    {
        static IModHelper Helper;
        static IManifest Manifest;

        public Patcher(IMod mod)
        {
            Helper = mod.Helper;
            Manifest = mod.ModManifest;
        }

        public void PerformPatching()
        {
            var harmony = new Harmony(Manifest.UniqueID);


            QuestPatches.ApplyPatch(harmony, Helper);

        }
    }
}

