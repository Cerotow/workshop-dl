using DDmod.Content.Projectiles.Melee;
using Terraria;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.Sword.Make
{
    public class MeteorSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 36;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.scale += 0.2f;
            Item.alpha = 255;
            Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(248, 66, 5, 100);
            Item.GetGlobalItem<MeleeGlobalItem>().ColorL = true;
            DDSystem.Instance.DDEquipGlow.TryGetValue("流星剑", out int GG);
            Item.glowMask = (short)GG;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void OnHitNPC(Player player, NPC target, HitInfo hit, int damageDone)
        {
            int A = NewProjectile(Item.GetSource_FromAI(), target.Center, new Vector2(player.direction * 0.01F, 0), ModContent.ProjectileType<FlameExplosion>(), hit.SourceDamage, hit.Knockback * 2, player.whoAmI);
        }
        public override Color? GetAlpha(Color color)
        {
            return color;
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
            CreateRecipe(1).AddIngredient(ItemID.MeteoriteBar, 18).AddTile(TileID.Anvils).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.流星合成"), () => Main.LocalPlayer.Dplayer().MeteorRecipe)).Register();
        }
    }
}