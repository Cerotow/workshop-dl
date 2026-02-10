
using Newtonsoft.Json.Linq;
using SteelSeries.GameSense.DeviceZone;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class 远古血腥头盔 : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 6;
            Item.value = 50000;
            Item.rare = 1;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (body.type == ModContent.ItemType<远古血腥胸甲>()|| body.type == 793) &&(legs.type == ModContent.ItemType<远古血腥护腿>() || body.type == 794);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.03F;
        }
        public override void UpdateVanitySet(Player player)
        {
            int maxValue = 10;
            if (Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y) > 1f)
                maxValue = 2;

            if (Main.rand.NextBool(maxValue))
            {
                int num7 = Dust.NewDust(player.position, player.width, player.height, 115, 0f, 0f, 140, default(Color), 0.75f);
                Main.dust[num7].noGravity = true;
                Main.dust[num7].fadeIn = 1.5f;
                Main.dust[num7].velocity *= 0.3f;
                Main.dust[num7].velocity += player.velocity * 0.2f;
                Main.dust[num7].shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
            }
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.Crimson");
            player.Aplayer().Crimson = true;
            player.crimsonRegen = true;
        }
    }
}