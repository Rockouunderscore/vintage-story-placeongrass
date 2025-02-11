using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace placeongrass.Patches;

public class ItemPatch
{
    public static bool OnHeldInteractStart_Prefix(Item __instance, ItemSlot itemslot, EntityAgent byEntity, ref BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
    {
        byEntity.Api.Logger.Debug($"blockSel.Block.FirstCodePart() {blockSel.Block.Code}");
        if (blockSel.Block.FirstCodePart() != "tallgrass")
        {
            return true;
        }
        
        BlockPos blockPosUnder = blockSel.Position.DownCopy();
        Block blockUnder = byEntity.World.BlockAccessor.GetBlock(blockPosUnder);
        if (blockUnder.Fertility == 0)
        {
            return true;
        }

        BlockSelection blockUnderSelection = blockSel.Clone();
        blockUnderSelection.Position = blockPosUnder;
        blockUnderSelection.Block = blockUnder;
        blockUnderSelection.HitPosition.Set(0.5d, 1d, 0.5d); // center top basically
        
        blockSel = blockUnderSelection;
        
        return true;
    }
}