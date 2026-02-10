using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Sword.Axe;
using Terraria;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.Sword.Make
{
    public class 熔岩战斧 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack =9;
            Item.value = Item.buyPrice(0, 0, 66, 0);
            Item.rare = 4;
            Item.axe = 40;
            Item.scale = 1.2F;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.GetGlobalItem<MeleeGlobalItem>().RightSwitch = true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override bool AltFunctionUse(Player player)
        {
            Item.GetGlobalItem<MeleeGlobalItem>().Switch(Item, player, ModContent.ProjectileType<熔岩战斧Proj>(), 10);
            return true;
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
            //CreateRecipe(1).AddIngredient(9, 50).AddTile(TileID.Anvils).Register();
        }
    }
}