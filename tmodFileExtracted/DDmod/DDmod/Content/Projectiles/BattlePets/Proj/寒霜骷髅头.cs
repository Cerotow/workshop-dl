using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Boss;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.BattlePets.Proj
{
    public class 寒霜骷髅头 : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] =8;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 500;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.coldDamage = true;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            NPC npc = NPCdirection.FindClosest(Projectile.Center, 1000, true, null);
            if (npc != null && (npc.Center - Projectile.Center).Length() > 50 && npc.active && Projectile.GetGlobalProjectile<DDGlobalProjectile>().track > 30)
            {
                Projectile.Chase(npc, 8, 21);
            }
            Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), 0, 0, 0, default, Main.rand.NextFloat(0.4F, 0.7F))];
            dust.velocity = -Projectile.velocity/10;
            dust.rotation = Projectile.rotation;
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Freeze>(),180);
        }
        public override void OnKill(int timeLeft)
        {
            NewDustChange2(40, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 3, false, 0.4F, 0.8F, 0,new Color(0,150,255,0));
            PlaySound(SoundID.Item27, Projectile.Center);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            SpriteEffects sprite = 0;
            if(Projectile.velocity.X<0)
            {
                sprite = SpriteEffects.FlipVertically;
            }
            for (int a = 0; a < Projectile.oldPos.Length; a++)
            {
                Main.spriteBatch.Draw(texture, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, null, new Color(0,150,255,0)*0.5f * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), Projectile.oldRot[a], texture.Size() / 2, Projectile.scale, sprite, 0);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White*0.5f, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, sprite, 0f);
            return false;
        }
    }
    public class 寒霜矢 : ModProjectile
    {
        public override string Texture => "DDmod/Content/Projectiles/冰弹";
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.coldDamage = true;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void AI()
        {
            NewDustChange2(2, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 3, false, 0.4F, 0.8F, 0, new Color(0, 150, 255, 0));
            Projectile.ai[0]++;
            Projectile.tileCollide = Projectile.ai[0] > 120;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.Track(800,20,16,30);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.netUpdate = true;
            target.AddBuff(ModContent.BuffType<Frozen>(), (int)Main.rand.NextFloat(120, 300));
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            NewDustChange2(20, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 3, false, 0.4F, 0.8F, 0, new Color(0, 150, 255, 0));
            PlaySound(SoundID.Item27, Projectile.Center);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = MeleeProjectile.XiaoiceBombs.Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), Projectile.velocity.ToRotation(), texture.Size() / 2, Projectile.scale / 4, 0, 0f);
            return false;
        }
    }
    public class 极寒领域 : ModProjectile
    {
        public override string Texture => "DDmod/Content/Projectiles/冰弹";
        public override void SetDefaults()
        {
            Projectile.width = 256;
            Projectile.height = 256;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.coldDamage = true;
        }

        public override void SetStaticDefaults()
        {
        }
        public override void AI()
        {
            if (Projectile.ai[0]==0)
            {
                Projectile.ai[0] = 1;
                NewDustChange2(30, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 8, false, 0.4F, 0.8F, 0, new Color(0, 150, 255, 0));
                PlaySound(SoundID.Item27, Projectile.Center);
            }
            if (Projectile.timeLeft > 150)
            {
                if (Projectile.DProj().SpeedScope < 256)
                    Projectile.DProj().SpeedScope += 2;
            }
            else
            {
                if (Projectile.DProj().SpeedScope > 0)
                    Projectile.DProj().SpeedScope -= 2;
                else
                    Projectile.Kill();
            }
            Projectile.Center = Projectile.Player().Center;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }

}