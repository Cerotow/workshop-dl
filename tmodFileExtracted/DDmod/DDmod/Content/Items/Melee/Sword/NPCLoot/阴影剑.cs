using DDmod.Content.Projectiles.Melee;
using System;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.Sword.NPCLoot
{
    public class 阴影剑 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 3;
            Item.scale = 1F;
            Item.shoot = ModContent.ProjectileType<阴影波>();
            Item.shootSpeed = 3;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.稀有;
            Item.DItem().HandheldColor = Color.White;
            Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(200, 0, 0);
            Item.GetGlobalItem<MeleeGlobalItem>().Color2 = new Color(0, 0, 0);

            //Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(255, 121, 3)*0.3F;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}