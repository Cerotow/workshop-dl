using DDmod.Content.Projectiles.Melee;
using Terraria;

namespace DDmod.Content.Items.Boss.鬼牙
{
    public class 鬼牙夺命棒 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 92;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useTurn = true;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.crit = 0;
            Item.value = Item.buyPrice(0, 36, 60, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = false;
            Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(222,0, 22, 0)*0.6F;
            Item.GetGlobalItem<MeleeGlobalItem>().EffectLength = 28;
            //Item.DItem().HandheldColor = new Color(255,255,255,150);
            Item.shoot = ModContent.ProjectileType<鬼齿>();
            Item.shootSpeed = 8f;
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type <= 0)
            {
                return false;
            }
            NewProjectile(source, position, velocity.RotatedBy(Main.rand.NextFloat(0, 0.4f)), type, damage, knockback);
            NewProjectile(source, position, velocity, type, damage, knockback);
            NewProjectile(source, position, velocity.RotatedBy(-Main.rand.NextFloat(0, 0.4f)), type, damage, knockback);
            A++;
            if (A >= 3)
            {
                NewProjectile(source, position, velocity, ModContent.ProjectileType<鬼斩>(), damage*3, knockback);
                A = 0;
            }
            R = true;
            return false;
        }
        int A;
        bool R;
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(30, Main.rand.Next(100, 300));
            if (!R)
            {
                return;
            }
            R = false;
            float lifeStoled = damageDone * 0.1f;
            if (lifeStoled < 1)
            {
                lifeStoled = 1;
            }
            if (target.HasBuff(30)) lifeStoled *= 2;
            if ((int)lifeStoled > 0 && !player.moonLeech && target.type != NPCID.TargetDummy && target.life > 2)
            {
                NewProjectile(player.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileID.VampireHeal, 0, 0f, player.whoAmI, player.whoAmI, lifeStoled);
            }
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}