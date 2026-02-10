
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class MeteorHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 22;
            Item.value = Item.buyPrice(0, 1, 80, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 2;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.MeteorSuit && legs.type == ItemID.MeteorLeggings;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.05f;
            player.maxMinions += 1;
        }
        public override void UpdateVanitySet(Player player)
        {
            if (player.velocity.Length() > 0.5F)
            {
                for (int a = 0; a < 2; a++)
                {
                    Dust dust = Main.dust[NewDust(player.position, player.width, player.height, 6)];
                    dust.velocity = -player.velocity;
                    dust.scale = 1.5f;
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
                }
            }
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.Meteor");
            player.maxMinions += 1;
            player.GetDamage(DamageClass.Summon) += 0.05F;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.MeteoriteBar, 12).AddTile(TileID.Anvils).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.流星合成"), () => Main.LocalPlayer.Dplayer().MeteorRecipe)).Register(); ;
        }
    }
}