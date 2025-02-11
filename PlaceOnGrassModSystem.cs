using System.Reflection;
using HarmonyLib;
using placeongrass.Patches;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.Config;
using Vintagestory.API.Common;
using Vintagestory.Common;
using Vintagestory.GameContent;

namespace placeongrass;

public class PlaceOnGrassModSystem : ModSystem
{
    public const string ModID = "placeongrass";
    private const string ConfigFileName = ModID + "/config.json";
    
    private Harmony harmony;

    private void Init(ICoreAPI api)
    {
        Global.Api = api;
        try
        {
            Global.Config = Global.Api.LoadModConfig<Config>(ConfigFileName);
            Global.Api.StoreModConfig(Global.Config, ConfigFileName); // update the config file
        }
        finally
        {
            if (Global.Config == null)
            {
                Global.Config = new Config();
                Global.Api.StoreModConfig(Global.Config, ConfigFileName);
            }
        }
        
        // Harmony.DEBUG = true;
        if (!Harmony.HasAnyPatches(ModID))
        {
            harmony = new Harmony(ModID);

            MethodInfo treeSeed = typeof(ItemTreeSeed).GetMethod(nameof(ItemTreeSeed.OnHeldInteractStart));
            MethodInfo reedRoot = typeof(ItemCattailRoot).GetMethod(nameof(ItemCattailRoot.OnHeldInteractStart));
            HarmonyMethod prefix = typeof(ItemPatch).GetMethod(nameof(ItemPatch.OnHeldInteractStart_Prefix));
            harmony.Patch(treeSeed, prefix: prefix);
            harmony.Patch(reedRoot, prefix: prefix);
        }
    }
    
    public override void StartClientSide(ICoreClientAPI api)
    {
        Mod.Logger.Notification($"{nameof(PlaceOnGrassModSystem)}.{nameof(StartClientSide)} {ModID}");
        
        Init(api);
    }
    
    public override void Dispose()
    {
        Mod.Logger.Notification($"{nameof(PlaceOnGrassModSystem)}.{nameof(Dispose)} {ModID}");
        harmony?.UnpatchAll(ModID);
    }
    
}