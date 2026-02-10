using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee;
using Terraria;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.Sword.Make
{
    public class FossilGreatsword : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 10f;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.FossilOre, 28).AddTile(TileID.Anvils).Register();
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<Fossil>(), 300);
            if (target.realLife > 0)
            {
                Main.npc[target.realLife].AddBuff(ModContent.BuffType<Fossil>(), 300);
            }
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}