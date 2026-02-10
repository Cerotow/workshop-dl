using DDmod.Content.Dusts;
using DDmod.Modkey;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Talisman
{
    public class DeadLeavesSpirit : Talismans
    {
        public override void SetStaticDefaults()
        {
        }
        public override void Set()
        {
            Projectile.hide = true;

        }
        public override void PreUse()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.DProj().Times[0]++;
            if (Projectile.DProj().Times[0] % 60 == 0)
            {
                bool Crit = Main.rand.Next(100) < Projectile.CritChance;
                int r = Crit ? Projectile.damage * 2 : Projectile.damage;
                if (player.whoAmI == Main.myPlayer)
                    player.Heal(r);
                Projectile.netUpdate = true;
            }
            //粒子
            if (Main.rand.NextBool(5))
            {
                int Dust = NewDust(player.position + new Vector2(-10, player.height), player.width + 20, 1, ModContent.DustType<生命粒子>());
                Main.dust[Dust].scale = 0.3f;
                Main.dust[Dust].velocity = new Vector2(0, -3);
                GlobalDust.DustPlayerOwner[Dust] = player.whoAmI;
            }
            Projectile.velocity = Vector2.Zero;
            Projectile.Center = player.MountedCenter - new Vector2(-2 * player.direction, 24).RotatedBy(player.fullRotation);
            Projectile.rotation = (MathHelper.PiOver4 + MathHelper.PiOver4 * -Projectile.Player().direction) + player.fullRotation;
            Projectile.spriteDirection = 0;
        }
        public override bool MobileAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = MathHelper.PiOver4 + MathHelper.PiOver4 / 2 * player.direction;
            Projectile.spriteDirection = 0;
            //冷却好了粒子
            if (player.TPlayer().TalismanCD >= player.TPlayer().MaxTalismanCD)
            {
                //冷却好了粒子
                if (Main.rand.NextBool(10))
                {
                    Vector2 center = Projectile.Center;

                    Vector2 direction = Main.rand.NextVector2CircularEdge(Projectile.width, Projectile.height);
                    float distance = 0.3f + Main.rand.NextFloat() * 0.5f;
                    Vector2 velocity = new Vector2(0f, -Main.rand.NextFloat() * 0.3f - 1.5f);

                    Dust dust = NewDustPerfect(center + direction * distance, ModContent.DustType<生命粒子>(), velocity, 0, default, 0.25f);
                    dust.fadeIn = 1.1f;
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.alpha = 0;
                }
            }
            return base.MobileAI();
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

    }
}