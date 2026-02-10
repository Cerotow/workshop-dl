using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.绿岩之视;
using DDmod.Content.Particles;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged.Gun;
using DDmod.Worlds;
using System;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Ranged
{
    public class RangedArousalHeart : ModProjectile
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/爱心光效2");
        }
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 0;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            Projectile.localAI[2] = -1;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if(Projectile.DProj().track>=30)
            return null;
            return false;
        }
        public override void AI()
        {

            Player player = Projectile.Player();
            if (Projectile.ai[2] == 0)
            {
                Projectile.localAI[0]++;
                Projectile.tileCollide = Projectile.DProj().Bool[0];
                if (!Projectile.DProj().Bool[0])
                {
                    Projectile.timeLeft = 5;
                    if (Projectile.localAI[2] == -1)
                    {
                        for (int A = 0; A < 1000; A++)
                        {
                            Projectile projectile = Main.projectile[A];
                            if (projectile.active && projectile.type == ModContent.ProjectileType<晶凝蓄能炮Proj>() && projectile.owner == Projectile.owner)
                            {
                                Projectile.localAI[2] = projectile.whoAmI;
                            }
                        }
                    }
                    else
                    {
                        if (Main.projectile[(int)Projectile.localAI[2]].active && Main.projectile[(int)Projectile.localAI[2]].type == ModContent.ProjectileType<晶凝蓄能炮Proj>())
                        {
                            Projectile.velocity = Main.projectile[(int)Projectile.localAI[2]].velocity;
                        }
                        float RO = Projectile.velocity.ToRotation();
                        if (Projectile.velocity.X < 0)
                        {
                            RO += MathHelper.Pi;
                        }
                        Projectile.Center = Main.projectile[(int)Projectile.localAI[2]].Center + Projectile.velocity.PerfectNormalize() * (46 + 10 * Projectile.localAI[1]) - new Vector2(0, 4).RotatedBy(RO);
                    }
                    if (Projectile.localAI[1] >= 4)
                    {
                        //蓄力完成
                        if (Projectile.soundDelay == 0)
                        {
                            Projectile.soundDelay = 30;
                            SoundStyle sound = SoundID.Item29;
                            sound.Pitch = -0.3F;
                            PlaySound(sound, Projectile.position);
                            byte[] LifeTextures =
                    [
                        0,0,1,1,1,0,1,1,1,0,0,
            0,1,0,0,0,1,0,0,0,1,0,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            0,1,0,0,0,0,0,0,0,1,0,
            0,0,1,0,0,0,0,0,1,0,0,
            0,0,0,1,0,0,0,1,0,0,0,
            0,0,0,0,1,0,1,0,0,0,0,
            0,0,0,0,0,1,0,0,0,0,0,

        ];
                            for (int a = 0; a < LifeTextures.Length; a++)
                            {
                                if (LifeTextures[a] == 1)
                                {
                                    Vector2 vector = new Vector2(a % 11, a / 11).RotatedBy(Projectile.rotation + MathHelper.Pi);
                                    Vector2 velocity = (vector - (new Vector2(10).RotatedBy(Projectile.rotation + MathHelper.Pi) / 2));
                                    int D = NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<爱心粒子>(), 0, 0, 100);
                                    Main.dust[D].velocity = velocity * 3;
                                    Main.dust[D].customData = 0.75F;
                                    Main.dust[D].scale = 2F;
                                    Main.dust[D].noGravity = false;
                                    Main.dust[D].rotation = velocity.ToRotation() - MathHelper.PiOver2;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int A = 0; A < 2; A++)
                        {
                            Vector2 vector = new Vector2(Main.rand.NextFloat(18, 24)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            int DU = NewDust(Projectile.PreviousCenter() - new Vector2(4) + vector * 10, 0, 0, ModContent.DustType<爱心粒子>(), 0, 0, 300, default, 1.7F);
                            Main.dust[DU].customData = 1001F;
                            Main.dust[DU].noGravity = false;
                            Main.dust[DU].velocity = -vector * 0.3F;
                            Main.dust[DU].scale = 0.5F * Projectile.scale;
                            GlobalDust.DustProjectileOwner[DU] = Projectile.whoAmI;
                        }
                    }
                    Projectile.damage = 0;
                    Projectile.localAI[1] = Projectile.localAI[0] / player.IteUseAnimation2() * 4;
                    if (Projectile.localAI[2]==-1 || !Main.projectile[(int)Projectile.localAI[2]].active || Main.projectile[(int)Projectile.localAI[2]].type != ModContent.ProjectileType<晶凝蓄能炮Proj>()|| Main.projectile[(int)Projectile.localAI[2]].ai[2]!=0)
                    {
                        Projectile.timeLeft = 600;
                        Projectile.velocity = Projectile.velocity.PerfectNormalize() * Projectile.localAI[1] * 10;
                        Projectile.damage = (int)(Projectile.ai[1] * Projectile.localAI[1] / 4);
                        Projectile.DProj().Bool[0] = true;
                        player.velocity -= Projectile.velocity / 4;

                        if (Projectile.scale >= 1F)
                        {
                            byte[] LifeTextures =
                [
                    0,0,1,1,1,0,1,1,1,0,0,
            0,1,0,0,0,1,0,0,0,1,0,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            0,1,0,0,0,0,0,0,0,1,0,
            0,0,1,0,0,0,0,0,1,0,0,
            0,0,0,1,0,0,0,1,0,0,0,
            0,0,0,0,1,0,1,0,0,0,0,
            0,0,0,0,0,1,0,0,0,0,0,

        ];
                            for (int a = 0; a < LifeTextures.Length; a++)
                            {
                                if (LifeTextures[a] == 1)
                                {
                                    Vector2 vector = new Vector2(a % 11, a / 11);
                                    Vector2 velocity = (vector - (new Vector2(10) / 2));
                                    velocity.Y *= 0.25F;
                                    velocity = velocity.RotatedBy(Projectile.rotation + MathHelper.Pi);
                                    velocity -= Projectile.velocity.PerfectNormalize() * 1;
                                    for (int R = 0; R < 10; R++)
                                    {
                                        int D = NewDust(Projectile.Center + Projectile.velocity - new Vector2(4), 0, 0, ModContent.DustType<爱心粒子>(), 0, 0, 100);
                                        Main.dust[D].velocity = velocity * R * Projectile.scale / 4 / 2;
                                        Main.dust[D].customData = 0.75F;
                                        Main.dust[D].scale = 2F * Projectile.scale / 4 * R / 10;
                                        Main.dust[D].noGravity = false;
                                        Main.dust[D].rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
                                    }
                                }
                            }
                        }
                        SoundStyle sound = SoundID.Item14;
                        sound.Pitch = -0.8F;
                        PlaySound(sound, Projectile.Center);
                    }
                }
                else
                {
                    if (Projectile.scale < 1F)
                    {
                        Projectile.Kill();
                    }
                }
                DDHelper.MaxandMinF(ref Projectile.localAI[0], player.IteUseAnimation2(), 0);
                Projectile.scale = Projectile.localAI[1];
            }
            else
            {
                Projectile.extraUpdates = 3;
                Projectile.localAI[1] = 4;
                Projectile.scale = Projectile.ai[0];
                Projectile.Track(600, 21, 25);
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.ProjScaleChange();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnKill(int timeLeft)
        {
            byte[] LifeTextures =
[
    0,0,1,1,1,0,1,1,1,0,0,
            0,1,0,0,0,1,0,0,0,1,0,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            0,1,0,0,0,0,0,0,0,1,0,
            0,0,1,0,0,0,0,0,1,0,0,
            0,0,0,1,0,0,0,1,0,0,0,
            0,0,0,0,1,0,1,0,0,0,0,
            0,0,0,0,0,1,0,0,0,0,0,

        ];
            if (Main.myPlayer == Projectile.owner&& Projectile.ai[2]==0&& Projectile.scale >= 1F)
            {
                for (int a = 0; a < LifeTextures.Length; a++)
                {
                    if (LifeTextures[a] == 1)
                    {
                        Vector2 vector = new Vector2(a % 11, a / 11).RotatedBy(Projectile.rotation + MathHelper.Pi);
                        Vector2 velocity = (vector - (new Vector2(10).RotatedBy(Projectile.rotation + MathHelper.Pi) / 2));
                        Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, velocity*(Projectile.scale/2+0.1F), Type, (int)(Projectile.damage * 0.2f), Projectile.knockBack, Projectile.owner, Projectile.scale/3, 0, 0.2f)];
                        projectile.ai[2] = 1;
                    }
                }
            }
            if (Projectile.ai[2]==1|| Projectile.scale < 1F)
            {
                for (int a = 0; a < LifeTextures.Length; a++)
                {
                    if (LifeTextures[a] == 1)
                    {
                        Vector2 vector = new Vector2(a % 11, a / 11).RotatedBy(Projectile.rotation + MathHelper.Pi);
                        Vector2 velocity = (vector - (new Vector2(10).RotatedBy(Projectile.rotation + MathHelper.Pi) / 2));
                        int D = NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<爱心粒子>(), 0, 0, 100);
                        Main.dust[D].velocity = velocity* Projectile.scale;
                        Main.dust[D].customData = 0.75F;
                        Main.dust[D].scale = Projectile.scale;
                        Main.dust[D].noGravity = false;
                        Main.dust[D].rotation = velocity.ToRotation() - MathHelper.PiOver2;
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            float AL = Projectile.localAI[1]/4;
            if(AL>1)
            {
                AL = 1;
            }

            Color color = new Color(255, 50, 50, 55) * AL;
            Color color2 = new Color(255, 255, 255, 255) * AL;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                
               // if (Projectile.localAI[1] >= 4 || Projectile.DProj().Bool[0])
                if (Projectile.DProj().Bool[0] || Projectile.ai[2]!=0)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                    Main.spriteBatch.Draw(Glow.Value, vector2, null, new Color(100, 100, 100, 255) * AL * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), Projectile.rotation, Glow.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                    Main.spriteBatch.Draw(Glow.Value, vector2, null, color * 0.6f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), Projectile.rotation, Glow.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                }
                else
                {
                    Projectile.oldPos[i] = Vector2.Zero;
                }
            }

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color2, Projectile.rotation, texture.Size()/2, Projectile.scale, spriteEffects, 0f);
            Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, Glow.Size() / 2, Projectile.scale / 4, spriteEffects, 0f);

            return false;
        }
    }
}