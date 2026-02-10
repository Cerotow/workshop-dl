using DDmod.Content.Items.Boss.LifeGuardItems;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Boss.恐惧缝合体;
using DDmod.Content.NPCs.Boss.天地守卫;
using DDmod.Content.NPCs.Boss.星心守卫;
using DDmod.Worlds;
using StructureHelper;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DDmod.Content.Tiles.流星;
using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.Items.Melee.Sword.Make;
using DDmod.Content.Items.Melee.FlyingKnife.Make;
using DDmod.Content.Items.Summon;
using StructureHelper.API;
using DDmod.Content.Items.Melee.SwordShield;
using DDmod.Content.Items.Melee.Sword;
using Terraria.WorldBuilding;
using DDmod.Content.Tiles.绿岩;

namespace DDmod.Content.Items.Boss.先祖咒魂
{
    public class 诅咒之火 : ModItem
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 3200;
            Item.rare = 7;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
            Item.expert = true;
        }

        public override bool? UseItem(Player player)
        {
            if (!DDWorld.诅咒之火 && player.itemAnimation > 0 && player.ItemTimeIsZero)
            {
                if (Main.netMode == 0)
                {
                    DDWorld.诅咒之火 = true;
                    Main.NewText(Language.GetTextValue("Mods.DDmod.ItemTips.诅咒之火"), 50, byte.MaxValue, 130);
                }
                else
                {
                    DDmod.SyncData(DDType.PlayersWorld, player.whoAmI, -1, player.whoAmI, (byte)1);
                }
            }
            return true;
        }
    }
}