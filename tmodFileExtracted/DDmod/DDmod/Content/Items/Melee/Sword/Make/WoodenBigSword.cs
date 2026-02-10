using DDmod.Content.Projectiles.Melee;
using Terraria;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.Sword.Make
{
    public class WoodenBigSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8;
            Item.value = Item.buyPrice(0, 0, 2, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.scale = 1.3f;
            Item.autoReuse = true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
            CreateRecipe(1).AddIngredient(9, 200).AddTile(TileID.Anvils).Register();
        }
    }
}