/*using StardewModdingAPI;

namespace RestStopCode
{
    internal static class ExternalAPIs
    {
        public static IContentPatcherApi CP;

        private static IMonitor Monitor { get; set; }
        private static IModHelper Helper { get; set; }

        internal static void Initialize(IModHelper helper)
        {
            Helper = helper;


            CP = Helper.ModRegistry.GetApi<IContentPatcherApi>("Pathoschild.ContentPatcher");
            if (CP is null)
            {
                Monitor.Log("Content Patcher is not installed; Rest Stop requires CP to run. Please install CP and restart your game.");
                return;
            }
            
        }
    }
}  */

