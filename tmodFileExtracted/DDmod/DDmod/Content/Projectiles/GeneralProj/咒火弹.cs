using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.NPCs.Boss.绿岩之视;
using DDmod.Content.Particles;
using DDmod.Worlds;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Transactions;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;
using static Terraria.GameContent.Animations.Actions.Sprites;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 咒火弹 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 0;
            Projectile.scale = 1F;
            Projectile.timeLeft = 360;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.hide = false;
        }
        int A;
        Color color;
        public override void AI()
        {
            Projectile.scale = 0.6F;
            color = new Color(191, 255, 45, 0);
            if (Projectile.ai[0]==0)
            {
                Projectile.penetrate = 1;
                Projectile.Track(800, 20, 12, 30);
                for (int a = 0; a < 2; a++)
                {
                    int A = NewDust(Projectile.position, Projectile.width, Projectile.height, 75, 0, 0, 100, new Color(191, 255, 45, 0), Main.rand.NextFloat(1F, 1.8F)* Projectile.scale);
                    Main.dust[A].velocity = -Projectile.velocity.PerfectNormalize() * 2;
                    Main.dust[A].rotation = Projectile.velocity.ToRotation();
                    Main.dust[A].customData = 1F;
                    Main.dust[A].noGravity = true;
                }
                for (int a = 0; a < 1; a++)
                {
                    int A = NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(191, 255, 45, 0), Main.rand.NextFloat(0.8F, 1.3F)*Projectile.scale);
                    Main.dust[A].velocity = -Projectile.velocity.PerfectNormalize() * 2;
                    Main.dust[A].rotation = Projectile.velocity.ToRotation();
                    Main.dust[A].customData = 1F;
                    Main.dust[A].noGravity = true;
                }
            }
            if (Projectile.ai[0] == 1)
            {
                Projectile.scale = 0.6F;
                if (Projectile.timeLeft > 60)
                    Projectile.timeLeft = 60;
                Projectile.extraUpdates = 5;
                color = new Color(141, 205, 15, 100);
                if (Main.rand.NextBool(2))
                {
                    for (int a = 0; a < 2; a++)
                    {
                        int A = NewDust(Projectile.position - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<光球粒子>(), 0, 0, 100, color, Main.rand.NextFloat(0.3F, 0.6F));
                        Main.dust[A].velocity = Projectile.velocity.PerfectNormalize() * 2;
                        Main.dust[A].rotation = Projectile.velocity.ToRotation();
                        Main.dust[A].customData = Main.dust[A].DustAI(2) + 2F;
                        Main.dust[A].noGravity = true;
                    }
                }
            }
            Projectile.ProjScaleChange();
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(39, 300);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == 0)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    int a = NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<咒火爆炸>(), (int)Projectile.DProj().Times[0], 1, Projectile.owner, Projectile.ai[0], Projectile.scale);
                    Main.projectile[a].DamageType = Projectile.DamageType;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] == 0)
            {
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                color.A = 0;

                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color2 = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    color2.A = 0;
                    float S = ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) + 0.1F;
                    if (S > 1)
                    {
                        S = 1;
                    }
                    Main.spriteBatch.Draw(texture, vector2, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4 * S, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(texture, vector2, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4 * S, SpriteEffects.None, 0);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.velocity.ToRotation(), texture.Size() / 2, Projectile.scale / 4, 0, 0f);
            }
            return false;
        }
    }
    public class 咒火爆炸 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
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

        public override void AI()
        {
            if (!Projectile.DProj().Bool[0])
            {
                Projectile.DProj().Bool[0] = true;
                SoundStyle sound = SoundID.Item14;
                sound.Volume = Projectile.ai[1];
                sound.MaxInstances = 10;
                PlaySound(sound, Projectile.Center);
                PlaySound(sound, Projectile.Center);
                PlaySound(sound, Projectile.Center);
                int Type = ModContent.DustType<速度粒子>(); 
                NewDustChange4((int)(Projectile.ai[1] * 20), Projectile.position, Projectile.Size, Type, 3 * Projectile.ai[1], 6 * Projectile.ai[1], true, 3F * (Projectile.ai[1]), 4F * (Projectile.ai[1]), 100, 1000, new Color(191, 255, 45,0), 2F);
                Type =75;
                NewDustChange4((int)(Projectile.ai[1] * 20), Projectile.position, Projectile.Size, Type, 3 * Projectile.ai[1], 6 * Projectile.ai[1], true, 1F * (Projectile.ai[1]), 3F * (Projectile.ai[1]), 100, 1000, default);

            }
            if (Projectile.scale < 24 * Projectile.ai[1])
            {
                Projectile.scale += 4 * Projectile.ai[1];
                Projectile.ProjScaleChange();
            }
            else if (Projectile.timeLeft > 2)
            {
                Projectile.timeLeft = 2;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(39, 300);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
        }
    }
}