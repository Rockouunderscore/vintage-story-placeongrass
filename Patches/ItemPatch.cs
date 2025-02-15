using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace placeongrass.Patches;

public class ItemPatch
{
    public static bool OnHeldInteractStart_Prefix(ItemSlot itemslot, EntityAgent byEntity, ref BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
    {
        if (blockSel == null || byEntity?.World == null || !byEntity.Controls.ShiftKey)
        {
            return true;
        }

        if (blockSel?.Block?.FirstCodePart() != "tallgrass")
        {
            return true;
        }
        
        BlockPos blockPosUnder = blockSel.Position.DownCopy();
        Block blockUnder = byEntity.World.BlockAccessor.GetBlock(blockPosUnder);
        
        blockSel.Position = blockPosUnder;
        blockSel.Block = blockUnder;
        
        return true;
    }
}