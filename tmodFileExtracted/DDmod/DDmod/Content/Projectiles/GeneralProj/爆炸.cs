
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Ranged.Ammo;
using ReLogic.Utilities;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public abstract class 爆炸 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 0;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void SetStaticDefaults()
        {
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return null;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return target.whoAmI == Projectile.ai[0] ? false : null;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
    }
    public class 火山爆炸 : 爆炸
    {
        public override void AI()
        {
            if (Projectile.scale == 1)
            {
                SoundStyle sound = SoundID.Item14;
                sound.Volume = 1;
                sound.Pitch = -1;
                sound.MaxInstances = 10;
                
                PlaySound(sound, Projectile.Center);
                NewDustChange4((int)(Projectile.scale * 200), Projectile.position, Projectile.Size, 6, 0, 8 * Projectile.scale, true, 1F * (Projectile.scale), 2F * (Projectile.scale), 100, 1000, new Color(254, 124, 6, 50), Projectile.scale * 5);

                int Type = ModContent.DustType<速度粒子>(); NewDustChange4((int)(Projectile.scale * 50), Projectile.position, Projectile.Size, Type, 5 * Projectile.scale, 10 * Projectile.scale, true, 3F * (Projectile.scale), 4F * (Projectile.scale), 100, 1000, new Color(254, 94, 6, 20),2F);
            }
            if (Projectile.scale < 24)
            {
                Projectile.scale += 4;
                Projectile.ProjScaleChange();
            }
            else if(Projectile.timeLeft>2)
            {
                Projectile.timeLeft = 2;
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            return target.whoAmI == Projectile.ai[0] ? false : null;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);
            Projectile.netUpdate = true;
        }
    }
    public class 钴爆炸 : 爆炸
    {
        public override void AI()
        {
            if (Projectile.scale == 1)
            {
                SoundStyle sound = SoundID.Item14;
                sound.Volume = 1;
                sound.Pitch = -1;
                sound.MaxInstances = 10;
                
                PlaySound(sound, Projectile.Center);
                NewDustChange4((int)(Projectile.scale * 200), Projectile.position, Projectile.Size, ModContent.DustType<光球粒子>(), 0, 8 * Projectile.scale, true, 1F * (Projectile.scale), 2F * (Projectile.scale), 100, 1000, new Color(46, 143, 189, 0), Projectile.scale * 5);

                int Type = ModContent.DustType<速度粒子>();
                NewDustChange4((int)(Projectile.scale * 50), Projectile.position, Projectile.Size, Type, 5 * Projectile.scale, 10 * Projectile.scale, true, 3F * (Projectile.scale), 4F * (Projectile.scale), 100, 1000, new Color(46, 143, 189, 120),2F);
            }
            if (Projectile.scale < 24)
            {
                Projectile.scale += 4;
                Projectile.ProjScaleChange();
            }
            else if(Projectile.timeLeft>2)
            {
                Projectile.timeLeft = 2;
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            return null;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
    }
}
