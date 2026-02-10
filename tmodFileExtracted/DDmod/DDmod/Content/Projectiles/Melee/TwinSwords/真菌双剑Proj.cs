using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Players;
using System.Reflection;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.TwinSwords
{
    public class 真菌双剑Proj : 双刀
    {
        public override void Defaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 38;
            EffectLength = 14;
            HandheldOffset = 30;
        }
        public override Color color => new Color(133, 131, 195,150);
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            SpecialAttack(210);
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = color;

            if (Projectile.DProj().Back == -1)
                Projectile.DProj().Bool[1] = true;

            if (Projectile.DProj().Bool[0] && Projectile.DProj().Back == 1)
            {
                Projectile.DProj().Bool[1] = true;
                Projectile.DProj().Bool[0] = false;
            }
            short Jl = (short)(20 - (player.Center - target.Center).Length() / 10);
            if (Jl < 0)
            {
                Jl = 0;
            }
            Time += Jl;

            Projectile.netUpdate = true;
        }

        public override void OnKill(int timeLeft)
        {
        }
        public override void SpecialShoot()
        {
            for (int a = 0; a < 6; a++)
            {
                int A = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0].RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f)) * 6, ModContent.ProjectileType<夜光蘑菇>(), Projectile.damage / 2, 0.2f, Projectile.owner);
                Main.projectile[A].DamageType = DamageClass.Melee;
            }
        }
        public override void Shoot()
        {
            if (Projectile.ai[2] == 2)
            {
                for (int a = 0; a < 3; a++)
                {
                    int A = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0].RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f)) * 6, ModContent.ProjectileType<夜光蘑菇>(), Projectile.damage/2, 0.2f, Projectile.owner);
                    Main.projectile[A].DamageType = DamageClass.Melee;
                }
            }
            else if (Projectile.ai[2] < 2)
            {
                int A = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0].RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f)) * 6, ModContent.ProjectileType<夜光蘑菇>(), Projectile.damage, 0.2f, Projectile.owner);
                Main.projectile[A].DamageType = DamageClass.Melee;
            }
        }
    }
}