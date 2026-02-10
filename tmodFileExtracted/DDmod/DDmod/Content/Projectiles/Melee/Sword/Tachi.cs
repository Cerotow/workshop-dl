using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class Tachi : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 960;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.ownerHitCheck = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            Projectile.scale = 0.1f;
            Projectile.MeleeProj().SwordHitbox = true;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 64;
            Projectile.extraUpdates = 6;
        }
        int proj = 0;
        public override bool PreAI()
        {
            //大小加成
            Player player = Main.player[Projectile.owner];
            if (Projectile.scale< player.ActiveItem().scale)
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * Projectile.scale));

            Projectile.HoldProj(player, 52 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
            Projectile.HoldSword2(player, -player.HeldItem.useAnimation, 100, 2.2f, false);
            int useTime = (int)(player.HeldItem.useTime / player.GetTotalAttackSpeed(DamageClass.Melee));
            if (Projectile.localAI[1] >= 0 && Projectile.ai[1] == 2 && Projectile.DProj().Times[2] < useTime)
            {
                Projectile.ai[1] = 3;

                PlaySound(SoundID.Item1, Projectile.position);
            }
            if (Projectile.localAI[1] <= 0|| Projectile.MeleeProj().DelayedKill>0)
            {
                proj = 0;
            }
            else
            {
            }

            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }

        Color color = new Color(78, 100, 233, 0);
        Color color2 = new Color(201, 249, 255, 0);
        Color color3 = new Color(155, 38, 173, 0);
        public Color TrailColor(float completionRatio)
        {
            float trailOpacity = Utils.GetLerpValue(0f, 0.1f, completionRatio, true) * Utils.GetLerpValue(0.7f, 0.58f, completionRatio, true);
            Color startingColor = Color.Lerp(color, color, 0.07f);
            return playerHelper.MulticolorLerp(completionRatio, new Color[]
            {
                startingColor,
            }) * trailOpacity;
        }
        public float TrailWidth(float completionRatio)
        {
            return 44 * Projectile.scale;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                return false;
            }
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(TrailWidth), new Trailing.VertexColorFunction(TrailColor), null, GameShaders.Misc["刀光"]);
            }
            Vector2 vector = Projectile.Player().Center;
            if (Projectile.MeleeProj().oldPlayer != Vector2.Zero)
            {
                vector = Projectile.MeleeProj().oldPlayer;
            }
            GameShaders.Misc["刀光"].SetShaderTexture(DDTextures.WhitePng);
            GameShaders.Misc["刀光"].UseColor(Projectile.GetAlpha(color * 0.2f));
            GameShaders.Misc["刀光"].UseSecondaryColor(Projectile.GetAlpha(color3 * 0.2f));
            GameShaders.Misc["刀光"].Shader.Parameters["FlameColor"].SetValue(Projectile.GetAlpha(color).ToVector3());
            GameShaders.Misc["刀光"].Shader.Parameters["flipped"].SetValue(Projectile.DProj().Times[0] > 0);
            GameShaders.Misc["刀光"].Apply(default(DrawData?));
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null);

            GameShaders.Misc["刀光"].SetShaderTexture(DDTextures.Wave);
            GameShaders.Misc["刀光"].UseColor(Projectile.GetAlpha(color * 0.2f));
            GameShaders.Misc["刀光"].UseSecondaryColor(Projectile.GetAlpha(color3 * 0.2f));
            GameShaders.Misc["刀光"].Shader.Parameters["FlameColor"].SetValue(Projectile.GetAlpha(color).ToVector3());
            GameShaders.Misc["刀光"].Shader.Parameters["flipped"].SetValue(Projectile.DProj().Times[0] > 0);
            GameShaders.Misc["刀光"].Apply(default(DrawData?));
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Vector2 Center = Projectile.Center - Main.screenPosition;
                if (Projectile.Player().direction == -1)
                {
                    Center += new Vector2(10 * Projectile.Player().direction, 0).RotatedBy(Projectile.rotation + MathHelper.PiOver4 * 0.5f) * Projectile.DProj().Times[0];
                }
                else
                {
                    Center -= new Vector2(10 * Projectile.Player().direction, 0).RotatedBy(Projectile.rotation + MathHelper.PiOver4 * 0.5f) * Projectile.DProj().Times[0];
                }
                if (Projectile.DProj().Times[0] == -1)
                {
                    Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Color.White, Projectile.rotation+ MathHelper.PiOver4 * 0.5f, texture.Size() / 2, Projectile.scale, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Color.White, Projectile.rotation + MathHelper.PiOver4 * 1.5f, texture.Size() / 2, Projectile.scale, (SpriteEffects)(1), 0f);
                }
            }
            else
            {
                Projectile.alpha += 10;
            }
            return false;
        }
        internal Trailing TrailDrawer;
    }
}