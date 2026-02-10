using DDmod.Content.Dusts;
using DDmod.Content.NPCs.Boss.绿岩之视;
using DDmod.Content.Particles;
using DDmod.Worlds;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Boss
{
    public class Boss绿岩导弹 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 0;
            Projectile.scale = 1F;
            Projectile.timeLeft = 180;
        }
        int A;
        public override void AI()
        {
            for (int a = 0; a < 2; a++)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(119, 237, 130, 50))];
                dust.noGravity = true;
                dust.scale = Projectile.scale;
                dust.velocity *= 0.1f;
                Vector2 vector = (dust.position - Projectile.Center).PerfectNormalize();
                dust.velocity += -vector * Projectile.scale;
                dust.rotation = Projectile.velocity.ToRotation();
            }
            NPC Master = Main.npc[(int)Projectile.ai[0]];
            if (!Master.active || Master.type != ModContent.NPCType<绿岩之视>())
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.DProj().track == 1)
            {
                PlaySound(SoundID.Item11, Projectile.Center);
            }
            if (Projectile.DProj().track < 120 || Projectile.ai[1] == 1)
            {
                Projectile.DProj().vector[0] = Main.player[Master.target].Center;
                if (Projectile.ai[0] == 0)
                {
                    Projectile.DProj().vector[0] = Main.player[Master.target].Center + Main.player[Master.target].velocity * 60;
                }
            }
            if (Projectile.DProj().track > 30&& (Projectile.DProj().track<90|| Projectile.ai[1]==0))
            {
                Projectile.velocity = (Projectile.velocity * 20 + (Projectile.DProj().vector[0] - Projectile.Center).PerfectNormalize() * 18) / 21;
            }
            if ((Projectile.DProj().vector[0] - Projectile.Center).Length() < 10)
            {
                Projectile.Kill();
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != 1)
            {
                NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Boss绿岩导弹爆炸>(), Projectile.damage, 1, Main.myPlayer);
            }
            PlaySound(SoundID.Item14, Projectile.Center);
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(119, 237, 130, 50),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(119, 237, 130, 50), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                20,
                15,
                10,
            }), 5, (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal static Trailing TrailDrawer;
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f- Projectile.velocity.PerfectNormalize()*20 - Main.screenPosition, 88, null);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            texture.DrawCentre(Projectile, new Rectangle(0, 0, texture.Width / 2, texture.Height), lightColor, Projectile.scale);
            texture.DrawCentre(Projectile, new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height), Color.White, Projectile.scale);

            return false;
        }
    }
    public class Boss绿岩导弹爆炸 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 0;
            Projectile.scale = 1F;
            Projectile.timeLeft = 12;
        }
        int A;
        public override void AI()
        {
            if (!Projectile.DProj().Bool[0])
            {
                for (int a = 0; a < 60; a++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(2), 1, 1, ModContent.DustType<速度粒子>(), 0f, 0f, 0, new Color(119, 237, 130, 50), 4f)];
                    Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(1, 4), Main.rand.NextFloat(1, 4)), (Math.PI * 2 / a) + a, default);
                    dust.velocity = vector * 2;
                    dust.customData =2; ;
                    dust.rotation = dust.velocity.ToRotation();
                    dust.noGravity = true;
                }
                for (int a = 0; a < 30; a++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<冰雾>(), 0f, 0f, -Main.rand.Next(800,3000), new Color(119, 237, 130, 50), Main.rand.NextFloat(0.5F,1.5F))];
                    Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) / 4, (Math.PI * 2 / a) + a, default);
                    dust.velocity = vector;
                    dust.noGravity = false;
                }
                Projectile.DProj().Bool[0] = true;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
        }
    }
}