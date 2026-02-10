
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 火龙波 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = false;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;

        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture +"_Glow");
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 7;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter%4==0)
            {
                Projectile.frame++;
                Projectile.frame%=7;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.velocity.X <= 0)
                Projectile.rotation += MathHelper.Pi;
            if (Main.rand.NextBool(3))
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0, 2.2F);
                    Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, 6)];
                    dust.velocity = projDirection;
                    dust.noGravity = true;
                    dust.alpha = 100;
                    dust.scale = 2.2f;
                }
                for (int i = 0; i < 1; i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(1, 2.2F);
                    Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<拉长粒子>(), newColor: new Color(255, 115, 0, 0))];
                    dust.velocity = projDirection;
                    dust.noGravity = true;
                    dust.alpha = 0;
                    dust.scale = 1.4f;
                }
            }
            NPC npc = Projectile.FindTargetWithinRange(800, false);
            if (npc != null && npc.active&& (npc.velocity.Y != 0 || !npc.collideY) && Projectile.GetGlobalProjectile<DDGlobalProjectile>().track >20)
            {
                if (!Projectile.hostile && Projectile.friendly)
                {
                    Vector2 vector = (npc.Center - Projectile.Center).PerfectNormalize() * 44;
                    Projectile.DProj().vector[0] = (Projectile.DProj().vector[0] * 20 + vector) / (21);
                }
            }
            DDHelper.BackAndForth(-6F * Projectile.velocity.Length() / 10, 6F * Projectile.velocity.Length() / 10, Projectile.velocity.Length() / 10, ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[1]);

            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.ProjScaleChange();
                Projectile.DProj().vector[0] = Projectile.velocity;
                Projectile.DProj().Times[0] = 3 * Projectile.velocity.Length() / 10;
                Projectile.DProj().Times[2] = Projectile.DProj().vector[0].ToRotation();
                Projectile.netUpdate = true;
            }
            Projectile.DProj().Times[1] = Projectile.DProj().vector[0].ToRotation();
            DDHelper.RotateSpeed(ref Projectile.DProj().Times[2], Projectile.DProj().Times[1],0.2F);
            Projectile.velocity = Projectile.DProj().Times[2].ToRotationVector2()*20 + new Vector2(0, Projectile.DProj().Times[0]).RotatedBy(Projectile.DProj().Times[2].ToRotationVector2().ToRotation());

        }
        public override void ModifyHitNPC(NPC target, ref HitModifiers modifiers)
        {
            if(target.velocity.Y!=0||!target.collideY)
            {
                modifiers.SourceDamage += 0.25F;
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.netUpdate = true;
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != 2)
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0, 4.2F);
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, 6)];
                    dust.velocity = projDirection;
                    dust.noGravity = true;
                    dust.alpha = 100;
                    dust.scale = 1.8f;
                }
                for (int i = 0; i < 10; i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(1, 4.2F);
                    Dust dust = Main.dust[NewDust(Projectile.Center-new Vector2(4), 1, 1, ModContent.DustType<拉长粒子>(),newColor:new Color(255,115,0,0))];
                    dust.velocity = projDirection;
                    dust.noGravity = true;
                    dust.alpha = 0;
                    dust.scale = 1.2f;
                }
                SoundStyle sound = SoundID.Item14;
                sound.Pitch = -0.5F;
                PlaySound(sound, Projectile.Center);
            }
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(241, 255, 77, 0),
                new Color(241, 255, 77, 0),
                new Color(255, 144, 0, 0),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(255, 144, 0, 0), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                10,
                20,
                30,
                40,
                50,
                60,
                70,
                80,
                90,
                100,
                110,
            }) * MathHelper.Lerp(0f, 1.4f, widthRatio),30, (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal static Trailing TrailDrawer;
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(Projectile.velocity.Length()/20);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Color color = Color.White;
            if (Projectile.velocity.X > 0)
            {
                color = new Color(255,255,255,0);
                Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, Glow.Value.Height / Main.projFrames[Projectile.type] * Projectile.frame, Glow.Value.Width, Glow.Value.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(Glow.Value.Width, Glow.Value.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale/2, 0, 0f);
              color = Color.White;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                color = new Color(255, 255, 255, 0);
                Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, Glow.Value.Height / Main.projFrames[Projectile.type] * Projectile.frame, Glow.Value.Width, Glow.Value.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(Glow.Value.Width, Glow.Value.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale/2, (SpriteEffects)1, 0f);
                color = Color.White;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, (SpriteEffects)1, 0f);
            }
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition, 204, null);

            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.FireEffect2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(2.2f);
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition, 204, null);
            return false;
        }
    }
}
