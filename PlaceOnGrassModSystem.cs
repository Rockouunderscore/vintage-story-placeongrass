
using System.Reflection;
using HarmonyLib;
using placeongrass.Patches;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace placeongrass;

public class PlaceOnGrassModSystem : ModSystem
{
    public const string ModID = "placeongrass";
    private const string ConfigFileName = ModID + "/config.json";
    
    private Harmony harmony;

    private void Init(ICoreAPI api)
    {
        Mod.Logger.Notification($"{nameof(PlaceOnGrassModSystem)}.{nameof(Init)} {api.Side} {ModID}");
        
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

    public new double ExecuteOrder() => 0.2d;

    public override void Start(ICoreAPI api)
    {
        base.Start(api);
        
        // Mod.Logger.Notification($"{nameof(PlaceOnGrassModSystem)}.{nameof(Start)} {api.Side} {ModID}");

        // Init(api);
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        base.StartServerSide(api);

        // Mod.Logger.Notification($"{nameof(PlaceOnGrassModSystem)}.{nameof(StartServerSide)} {ModID}");

        // Init(api);
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);
        
        Mod.Logger.Notification($"{nameof(PlaceOnGrassModSystem)}.{nameof(StartClientSide)} {ModID}");

        Init(api);
    }
    
    public override void Dispose()
    {
        Mod.Logger.Notification($"{nameof(PlaceOnGrassModSystem)}.{nameof(Dispose)} {ModID}");
        harmony?.UnpatchAll(ModID);
    }
    
}