using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee.FlyingKnife.Proj;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using DDmod.Players;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 泰拉之锋Proj : 飞刀Proj
    {

        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void Defaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            AIStyle = 飞刀AI.AI3;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 0;
            Projectile.penetrate = -1;
            Projectile.width = 20;
            Projectile.height = 20;
        }
        public int 手;
        public float 手旋转;
        public float 旋转;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            //右键
            if (player.controlUseTile&& !Projectile.DProj().Bool[0]&& Projectile.ai[0] == 0)
            {
                Projectile.DProj().Bool[1] = true;
            }
            if (Projectile.DProj().Bool[1])
            {
                if (player.controlUseTile)
                {
                    if (手 < 200)
                    {
                        手++;
                        if(Main.rand.NextBool(3))
                        { 
                            Vector2 vector = Projectile.position + new Vector2(Main.rand.NextFloat(-13, 13), Main.rand.NextFloat(0, 66));
                            Dust dust = Main.dust[NewDust(vector, 0, 0, ModContent.DustType<星光粒子>(), newColor: new Color(141, 233, 130, 0))];
                            dust.velocity = vector - dust.position;
                            dust.velocity /= 40;
                            dust.customData = 1;
                            dust.noGravity = false;
                            dust.alpha = 100;
                            dust.scale =1.3f;
                        }
                    }
                    else if(!Projectile.DProj().Bool[2])
                    {
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center - new Vector2(0, 800), new Vector2(0, 10), ModContent.ProjectileType<泰拉闪电>(), Projectile.damage * 2, 3, Projectile.owner);
                        Projectile.DProj().Bool[2] = true;
                    }
                    if (player.dead)
                    {
                        Projectile.Kill();
                    }
                    player.heldProj = Projectile.whoAmI;

                    Vector2 Pvelocity = (player.Dplayer().MouseWorld - player.Center).PerfectNormalize();
                    player.ChangeDir(Pvelocity.X >= 0 ? 1 : -1);
                    Projectile.velocity.X = player.direction;
                    Projectile.velocity.Y = 0;
                    if (player.mount.Active)
                    {
                        Projectile.Center = player.RotatedRelativePoint(player.Center - new Vector2(0, 80), reverseRotation: false, addGfxOffY: false) - new Vector2(4 * player.direction, 0).RotatedBy(player.fullRotation) + new Vector2(0, player.gfxOffY);

                    }
                    else
                    {
                        Projectile.Center = player.Center - new Vector2(4 * player.direction, 0).RotatedBy(player.fullRotation)-new Vector2(0, 70) + new Vector2(0, player.gfxOffY);

                    }
                    Projectile.position.X -= player.direction;
                    Projectile.rotation = 0;
                    player.itemRotation = new Vector2(0, -1).ToRotation();
                    player.PlayerAction().PlayerArmRotation(new Vector2(0, -1).ToRotation() - MathHelper.PiOver2-player.fullRotation, 0);


                    Projectile.timeLeft = 300;

                    player.itemTime = 2;
                    player.itemAnimation = 2;
                    if (player.velocity.X == 0)
                    {
                        player.PlayerAction().PlayerArmRotationBack(-0.1F, 0);
                    }
                }
                else
                {
                    Projectile.Kill();
                }
                return false;
            }
            if(player.HasBuff(ModContent.BuffType<TerraPower2>()))
            {
                Projectile.extraUpdates = 1;
            }
            Projectile.DProj().Bool[0] = true;
            if (Projectile.ai[0] == 1)
            {
                Projectile.alpha -= 30;
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
                if (Projectile.timeLeft > 50)
                {
                    Projectile.timeLeft = 50;
                }
                if (Projectile.timeLeft > 25 && Projectile.damage == 0)
                {
                    Projectile.timeLeft = 25;
                }
                if (Projectile.timeLeft < 25)
                {
                    Projectile.velocity *= 0.85f;
                    Projectile.extraUpdates = 0;
                }
                if (Projectile.velocity.Length() < 1)
                {
                    Projectile.velocity = Projectile.velocity.PerfectNormalize();
                }
                float v = Projectile.velocity.Length() / 20;
                if (v > 1)
                {
                    v = 1;
                }
                Projectile.alpha = (int)(255 - 255 * v);
                Projectile.tileCollide = false;
            }
            else
            {
                if (Projectile.timeLeft < 25)
                {
                    Projectile.damage = 0;
                    Projectile.velocity *= 0.9f;
                    Projectile.extraUpdates = 0;
                    if (Projectile.velocity.Length() < 1)
                    {
                        Projectile.velocity = Projectile.velocity.PerfectNormalize();
                    }
                    float v = Projectile.velocity.Length() / 20;
                    if (v > 1)
                    {
                        v = 1;
                    }
                    Projectile.alpha = (int)(255 - 255 * v);
                    Projectile.tileCollide = false;
                }
                else
                {
                    if (Main.rand.NextBool(5))
                    {
                        int A = NewDust(Projectile.position + Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity, 1, 1, ModContent.DustType<星光粒子>(), newColor: new Color(141, 233, 130, 0));
                        Main.dust[A].noGravity = true;
                    }
                    else
                    {
                        int B = NewDust(Projectile.position + Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity, 1, 1, 107, 0, 0, 0, default, 1.4f);
                        Main.dust[B].noGravity = true;
                    }
                    Projectile.alpha = 0;
                }
            }
            return true;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.DProj().Bool[1])
            {
                return;
            }
            Projectile.damage = (int)(Projectile.damage * 0.8F);
            int R = 1;
            if (Projectile.extraUpdates == 1)
            {
                R = 3;
            }
            if (Projectile.timeLeft > 25 && Projectile.ai[0] == 0)
            {
                Projectile.timeLeft = 25;
            }
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade, settings, Projectile.owner);
            if (Projectile.DProj().Bool[3])
            {
                return;
            }
            Projectile.DProj().Bool[3] = true;
            if (Projectile.ai[0] == 0)
            {
                for (int A = -R; A <= R; A++)
                {
                    Vector2 vector = Projectile.Center - Projectile.velocity.RotatedBy(A * 0.7f - Main.rand.NextFloat(0, MathHelper.TwoPi)).PerfectNormalize() * 240;
                    NewProjectile(Projectile.GetSource_FromAI(), vector, (Projectile.Center - vector).PerfectNormalize() * 16, Type, Projectile.damage / 2, 0, Projectile.owner, 1);

                    NewDustChange(20, vector - new Vector2(4), Vector2.Zero, 107, 0, 6, true, 2f);
                    NewDustChange(6, vector - new Vector2(4), Vector2.Zero, ModContent.DustType<星光粒子>(), 0, 6, true, 0.8f,0, new Color(141, 233, 130, 0));
                }
            }
            if (Projectile.extraUpdates == 1 && Projectile.ai[0] == 0)
            {
                int A= NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center - new Vector2(0, 800), new Vector2(0, 10), ModContent.ProjectileType<泰拉闪电>(), (int)(Projectile.damage *1.5f), 3, Projectile.owner);
                Main.projectile[A].hostile = false;
                Main.projectile[A].localAI[0] = Projectile.position.Y;
            }
            Projectile.netUpdate = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity;
            if (Projectile.timeLeft > 25 && Projectile.ai[0] == 0)
            {
                Projectile.timeLeft = 25;
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 0&& Projectile.timeLeft>25)
            {
                //NewDustChange(150, Projectile.Center - new Vector2(4), Vector2.Zero, 107, 0, 8, true, 2f);
            }
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[0] == 0|| Projectile.timeLeft>=25;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D glow = Glow.Value;
            float RO = Projectile.rotation;
            SpriteEffects sprite = 0;
            Vector2 Origia = new Vector2(texture.Width / 2,20);
            Vector2 Origia2 = new Vector2(glow.Width / 2, 80);
            if (Projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipVertically;
                RO -= Rotation2;
                Origia = new Vector2(texture.Width / 2, texture.Height - 20);
                Origia2 = new Vector2(glow.Width / 2, glow.Height - 80);
            }
            Vector2 Po = Projectile.Center;
            if (!Projectile.DProj().Bool[1])
            {
                Player player = Main.player[Projectile.owner];
                if (Projectile.extraUpdates == 1 || Projectile.ai[0] == 1)
                {
                    for (int i = 0; i < Projectile.oldPos.Length; i++)
                    {
                        float A = Projectile.oldRot[i];
                        if (Projectile.velocity.X < 0)
                        {
                            A -= Rotation2;
                        }
                        Vector2 vector2 = Projectile.oldPos[i] +Projectile.Size/2- Main.screenPosition;
                        Color color = new Color(71, 233, 60, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * 1.2f;
                        Main.spriteBatch.Draw(glow, vector2, null, Projectile.GetAlpha(color), A, Origia2, Projectile.scale / 4, sprite, 0f);
                    }
                }
                if (Projectile.ai[0] == 0)
                {
                    if (Projectile.extraUpdates == 1 || Projectile.ai[0] == 1)
                    {
                        Main.spriteBatch.Draw(glow, Po - Main.screenPosition, null, Projectile.GetAlpha(new Color(255, 255, 255, 0)), RO, Origia2, Projectile.scale / 4, sprite, 0f);
                    }
                    Main.spriteBatch.Draw(texture, Po - Main.screenPosition, null, Projectile.GetAlpha(Color.White), RO, Origia, Projectile.scale, sprite, 0f);
                }
            }
            else
            {
                if (Projectile.ai[0] == 0)
                {
                    if (手 < 200)
                    {
                        float L = ((float)手 / 210);
                        Main.spriteBatch.Draw(texture, Po - Main.screenPosition, null, new Color(80, 80, 80, 255), RO, Origia, Projectile.scale, sprite, 0f);
                        if (sprite == SpriteEffects.FlipVertically)
                        {
                            Rectangle? rectangle = new Rectangle?(new Rectangle(0, (int)(texture.Height - texture.Height * L), texture.Width, (int)(texture.Height * L)));

                            Main.spriteBatch.Draw(texture, Po - Main.screenPosition - new Vector2(0, 1), rectangle, Color.White, RO, Origia, Projectile.scale, sprite, 0f);
                            rectangle = new Rectangle?(new Rectangle(0, (int)(texture.Height - texture.Height * L), texture.Width, 4));
                            Main.spriteBatch.Draw(texture, Po - Main.screenPosition - new Vector2(0, 1) + new Vector2(0, (int)(-texture.Height * L)), rectangle, new Color(255, 255, 255, 0), RO, Origia, Projectile.scale, sprite, 0f);
                            Main.spriteBatch.Draw(texture, Po - Main.screenPosition - new Vector2(0, 1) + new Vector2(0, (int)(-texture.Height * L)), rectangle, new Color(255, 255, 255, 0), RO, Origia, Projectile.scale, sprite, 0f);
                            Main.spriteBatch.Draw(texture, Po - Main.screenPosition - new Vector2(0, 1) + new Vector2(0, (int)(-texture.Height * L)), rectangle, new Color(255, 255, 255, 0), RO, Origia, Projectile.scale, sprite, 0f);
                        }
                        if (sprite == 0)
                        {
                            Rectangle? rectangle = new Rectangle?(new Rectangle(0, (int)(texture.Height - texture.Height * L), texture.Width, (int)(texture.Height * L)));
                            Main.spriteBatch.Draw(texture, Po - Main.screenPosition + new Vector2(0, (int)(texture.Height - texture.Height * L)), rectangle, Color.White, RO, Origia, Projectile.scale, sprite, 0f);
                            rectangle = new Rectangle?(new Rectangle(0, (int)(texture.Height - texture.Height * L), texture.Width, 4));
                            Main.spriteBatch.Draw(texture, Po - Main.screenPosition + new Vector2(0, (int)(texture.Height - texture.Height * L)), rectangle, new Color(255, 255, 255, 0), RO, Origia, Projectile.scale, sprite, 0f);
                            Main.spriteBatch.Draw(texture, Po - Main.screenPosition + new Vector2(0, (int)(texture.Height - texture.Height * L)), rectangle, new Color(255, 255, 255, 0), RO, Origia, Projectile.scale, sprite, 0f);
                            Main.spriteBatch.Draw(texture, Po - Main.screenPosition + new Vector2(0, (int)(texture.Height - texture.Height * L)), rectangle, new Color(255, 255, 255, 0), RO, Origia, Projectile.scale, sprite, 0f);
                        }
                    }
                    else
                    {
                        Main.spriteBatch.Draw(glow, Po - Main.screenPosition - new Vector2(0, 5), null, new Color(255, 255, 255, 0), RO, Origia2, Projectile.scale / 4, sprite, 0f);
                        Main.spriteBatch.Draw(texture, Po - Main.screenPosition, null, Color.White, RO, Origia, Projectile.scale, sprite, 0f);
                    }
                }
            }
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White * 0.5F, 0, Vector2.Zero, Projectile.Size / 2, sprite, 0f);
            return false;
        }
        internal Trailing TrailDrawer;
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(36, 208, 2),
                new Color(36, 208, 2),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(81, 6, 233), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                10,
                20,
                30,
                40,
                50,
                60,
                50,
                40,
                30,
                20,
                10,
            }) * Projectile.scale, 10 * Projectile.scale, (float)Math.Pow((double)completionRatio, 1.0));
        }
    }
}