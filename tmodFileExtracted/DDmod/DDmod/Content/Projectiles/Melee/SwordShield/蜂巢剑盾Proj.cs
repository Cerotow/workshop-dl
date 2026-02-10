
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Players;
using System.Reflection;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.SwordShield
{
    public class 蜂巢剑盾Proj : 剑盾
    {
        public override void Defaults()
        {
            Projectile.width = 52;
            Projectile.height = 52;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 36;
            Projectile.extraUpdates = 6;
            EffectLength = 24;
            HandheldOffset = 26;
        }
        public override Color color => new Color(34, 23, 60,150);
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if(Projectile.DProj().Bool[0])
            {
                Projectile.damage *= 3;
                Projectile.knockBack *= 1.5f;
                Projectile.DProj().Magnification *= 3;
                Projectile.DProj().Bool[1] = true;
                Projectile.DProj().Bool[0] = false;
            }
            return true;
        }
        public override void Shoot()
        {
            if (Projectile.DProj().Bool[1])
            {
                for (int a = 0; a < 8; a++)
                {
                    int t = 181;
                    if (Main.player[Projectile.owner].strongBees)
                    {
                        if (Main.rand.NextBool(3))
                        {
                            t = 556;
                        }
                    }

                    NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0].RotatedBy(Main.rand.NextFloat(-0.2F, 0.2F)) * Main.rand.NextFloat(6, 10), t, Projectile.damage / 12, 0, Projectile.owner);
                }
            }
            else
            {

                for (int a = 0; a < Main.rand.Next(1,3); a++)
                {
                    int t = 181;
                    if (Main.player[Projectile.owner].strongBees)
                    {
                        if (Main.rand.NextBool(3))
                        {
                            t = 556;
                        }
                    }

                    NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0].RotatedBy(Main.rand.NextFloat(-0.2F, 0.2F)) * Main.rand.NextFloat(6, 10), t, Projectile.damage / 4, 0, Projectile.owner);
                }
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = color;


            Projectile.netUpdate = true;
        }


        public override void OnKill(int timeLeft)
        {
        }
    }
}