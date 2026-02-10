
using DDmod.Content.Projectiles.OrnamentProjectile;

namespace DDmod.Content.Items.Boss.MeteorAnnihilatorItems
{
    [AutoloadEquip(EquipType.Wings)]
    public class 流星冲刺翼 : ModItem
    {

        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(5, 9f, 2.5f);
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.expert = true;
        }
        public override void UpdateVanity(Player player)
        {
            if (player.AccPlayer().wingslot == 0 && player.AccPlayer().wing == Item.wingSlot)
            {
                player.AccPlayer().wingslot = Item.type;
            }
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.AccPlayer().wingslot == 0 && player.AccPlayer().wing == Item.wingSlot)
            {
                player.AccPlayer().wingslot = Item.type;
            }
            /*
            if (ProjTime > 0)
            {
                ProjTime--;
            }
            else if(ProjTime>-10)
            {
                if (ProjTime ==-5)
                {
                    for (int a = 0; a < 8; a++)
                    {
                        NewProjectile(player.GetSource_FromAI(), player.Center, new Vector2(0, 10).RotatedBy(MathHelper.TwoPi/8*a), ModContent.ProjectileType<歼灭者导弹>(), 40, 1, player.whoAmI);
                    }
                }
                ProjTime--;
            }
            else
            {
                ProjTime = 500;
            }*/
            player.dashType = -1;
            player.Aplayer().MeteorDash = true;
        }
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f; // Falling glide speed
            ascentWhenRising = 0.15f; // Rising speed
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 3f;
            constantAscend = 0.135f;
        }
    }
}
