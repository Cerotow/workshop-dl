
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Players;
using System.Reflection;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.SwordShield
{
    public class 木制剑盾Proj : 剑盾
    {
        public override void Defaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 38;
            Projectile.extraUpdates = 6;
            EffectLength = 10;
            HandheldOffset = 26;
        }
        public override Color color => new Color(127, 92, 69,200);
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if(Projectile.DProj().Bool[0])
            {
                Projectile.damage *= 5;
                Projectile.knockBack *= 2;
                Projectile.DProj().Magnification *= 5;
                Projectile.DProj().Bool[0] = false;
            }
            return true;
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