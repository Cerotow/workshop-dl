using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using Terraria;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.Sword.Make
{
    public class 烈阳刃 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 120;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(0, 42, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.scale += 0.2f;
            Item.alpha = 255;
            Item.GetGlobalItem<MeleeGlobalItem>().ColorL = true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 8, true));
        }
        int a = 0;
        public override void OnHitNPC(Player player, NPC target, HitInfo hit, int damageDone)
        {
            NPC npc = target;
            if (a <= 0)
            {
                for (int A = 0; A < 2; A++)
                {
                    Vector2 vector = npc.Center;
                    Vector2 v = new Vector2(vector.X, player.Center.Y) - new Vector2(Main.rand.Next(-3000, 3000) /5, 800);

                    int D = NewProjectile(player.GetSource_FromAI(), v, (vector - v).PerfectNormalize() * 10, ModContent.ProjectileType<烈阳光束>(), player.GetWeaponDamage(Item), player.GetWeaponKnockback(Item), -1, Main.rand.NextFloat(0.75F,1.75F));
                    Main.projectile[D].DamageType = DamageClass.Melee;
                }
                a++;
            }
            //NewProjectile(player.GetSource_FromAI(), target.Center,Vector2.Zero,953,damageDone,0,-1,0, 1.35f);
        }
        public override bool? UseItem(Player player)
        {
            a = 0;
            if (Main.myPlayer == player.whoAmI)
            {
                NPC npc = NPCdirection.FindClosest(Main.MouseWorld, 500, true);
                Vector2 vector = Main.MouseWorld;
                Vector2 v = new Vector2(vector.X, player.Center.Y) - new Vector2(Main.rand.Next(-3000, 3000) / 30, 800);
                if (npc != null)
                {
                    vector = npc.Center;
                    v = new Vector2(vector.X, player.Center.Y) - new Vector2(Main.rand.Next(-3000, 3000) / 30, 800);
                }
                int D = NewProjectile(player.GetSource_FromAI(), v, (vector-v).PerfectNormalize() * 10, ModContent.ProjectileType<烈阳光束>(), player.GetWeaponDamage(Item), player.GetWeaponKnockback(Item), -1, Main.rand.NextFloat(0.75F, 1.75F));
                Main.projectile[D].DamageType = DamageClass.Melee;
            }
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override Color? GetAlpha(Color color)
        {
            return Color.White;
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
            CreateRecipe(1).AddIngredient(3458, 18).AddTile(TileID.LunarCraftingStation).Register();
        }
    }
}