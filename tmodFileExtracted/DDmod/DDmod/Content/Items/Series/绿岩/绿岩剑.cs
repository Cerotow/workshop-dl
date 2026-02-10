using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Melee;

namespace DDmod.Content.Items.Series.绿岩
{
    public class 绿岩剑 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.DamageType = DamageClass.Melee;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.crit = 2;
            Item.scale = 1.3F;
            Item.value = Item.buyPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        int r = 0;
        public override bool? UseItem(Player player)
        {
            r = 0;
            return base.UseItem(player);
        }
        public override void OnHitNPC(Player player, NPC target, HitInfo hit, int damageDone)
        {
            if (r <= 0)
            {
                r++;
                NPC npc = NPCdirection.FindClosest(target.Center, 300, false, target);
                if (npc != null)
                {
                    int T = NewProjectile(player.GetSource_FromAI(), target.Center, Vector2.Zero, ModContent.ProjectileType<绿岩能量>(), hit.SourceDamage, 0, -1, npc.whoAmI);
                    Main.projectile[T].DamageType = DamageClass.Melee;
                    Main.projectile[T].DProj().NPCW = new List<byte>() { (byte)target.whoAmI };
                }
            }
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩锭>(), 12).AddTile(16).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.绿岩合成"), () => Main.LocalPlayer.Dplayer().GreenstoneRecipe)).Register();
        }
    }
}