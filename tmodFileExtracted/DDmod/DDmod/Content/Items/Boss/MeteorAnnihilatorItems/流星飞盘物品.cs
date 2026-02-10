using DDmod.Content.Projectiles.Melee.Boomerang;
using DDmod.Content.Projectiles.Melee.Boomerang.Chakram;
using DDmod.Players;
using Terraria;

namespace DDmod.Content.Items.Boss.MeteorAnnihilatorItems
{
    public class 流星飞盘物品 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
            Item.height = 30;
            Item.noUseGraphic = true;
            Item.consumable = false;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<流星飞盘>();
            Item.shootSpeed = 10f;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 2, 20, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.channel = true;
            //Item.GetGlobalItem<MeleeGlobalItem>().Chakram = true;
            Item.DItem().DrawDistance = -12;
            Item.DItem().DrawDefaults = true;
            Item.DItem().Boomerang = 1;
        }
        public override bool CanUseItem(Player player)
        {
            return DDPlayer.UseBoomerang(Item, player);
        }
    }
}
