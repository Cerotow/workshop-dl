using DDmod.Content.Projectiles.Melee.Sword;
using System.Reflection;
using Terraria.Graphics.Light;

namespace DDmod.Content.Items.Melee.Sword
{
    public class TachiItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 23;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 660;
            Item.useAnimation = Item.useTime/3;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 4, 80, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shootSpeed = 1;
            Item.shoot = ModContent.ProjectileType<Tachi>();
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.scale = 1;
            Item.hasVanityEffects = true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            NewProjectile(source, position, velocity, type, damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}