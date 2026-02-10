using DDmod.Content.Projectiles.Melee;
using System.Reflection.Emit;
using System.Reflection;
using Terraria;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.Sword.Make
{
    public class 寒冰巨剑 : ModItem
    {
        public override string Texture => "DDmod/Content/Items/Melee/Sword/Make/寒冰巨剑";
        public override void SetDefaults()
        {
            Item.damage = 56;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = 6;
            Item.UseSound = SoundID.Item1;
            Item.scale = 1.1F;
            Item.shoot = ModContent.ProjectileType<寒冰斩>();
            Item.shootSpeed = 10;
            Item.autoReuse = true;
            Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(100, 155, 255, 0) * 0.3f;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(724).AddIngredient(676).AddIngredient(520, 25).AddIngredient(521, 25).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}