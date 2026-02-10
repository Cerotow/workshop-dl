using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Pet.MasterPet
{
    public class 微型流星破坏者 : ModProjectile
    {
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> Pao;
        public static Asset<Texture2D> Pao_Glow;
        public static Asset<Texture2D> Qiang;
        public static Asset<Texture2D> Qiang_Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Pet/MasterPet/微型流星破坏者_Glow");
            Pao = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Pet/MasterPet/微型流星炮");
            Pao_Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Pet/MasterPet/微型流星炮_Glow");
            Qiang = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Pet/MasterPet/微型流星激光枪");
            Qiang_Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Pet/MasterPet/微型流星激光枪_Glow");
        }

        public override void SetStaticDefaults()
        {

            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.LightPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
            Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet);

            Projectile.aiStyle = -1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor;
        }
        internal static Trailing TrailDrawer;
        internal  Trailing TrailDrawer2;
        internal Color ColorFunction(float completionRatio)
        {
            return new Color(252, 128, 48);
        }
        internal float WidthFunction(float completionRatio)
        {
            return 10;
        }
        internal Color ColorFunction2(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(252, 160,28 ),
                new Color(252, 160,28 ),
                new Color(252, 160,28 ),
                new Color(248, 66,5 ),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(248, 66, 5), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction2(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                10,
                20,
                10,
                5,
            }), 2, (float)Math.Pow((double)completionRatio, 1.0));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            if (TrailDrawer2 == null)
            {
                TrailDrawer2 = new Trailing(new Trailing.VertexWidthFunction(WidthFunction2), new Trailing.VertexColorFunction(ColorFunction2), null, GameShaders.Misc["贴图拖尾"]);
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 14, 88, null, Projectile.scale);

            Vector2[] vectors = new Vector2[4];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D VoidStar = DDTextures.MiniVoidStar.Value;
            Rectangle rectangle = new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]);
            SpriteEffects sprite = 0;
            if (Projectile.Player().Center.X - Projectile.Center.X > 0)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation, rectangle.Size() / 2, 1f, sprite, 0);
            Main.EntitySpriteDraw(Glow.Value, Projectile.Center - Main.screenPosition, rectangle, Color.White, Projectile.rotation, rectangle.Size() / 2, 1f, sprite, 0);

            vectors[0] = Projectile.Center + new Vector2(-16, 16).RotatedBy(Projectile.rotation);
            vectors[1] = vectors[0]+ new Vector2(-32, 32).RotatedBy(Projectile.rotation);

            TrailDrawer2.Draw(vectors, -Main.screenPosition, 88, null, Projectile.scale);

            vectors[0] = Projectile.Center + new Vector2(16, 16).RotatedBy(Projectile.rotation);
            vectors[1] = vectors[0] + new Vector2(32, 32).RotatedBy(Projectile.rotation);
            TrailDrawer2.Draw(vectors, -Main.screenPosition, 88, null, Projectile.scale);


            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            vectors[0] = Projectile.Center - new Vector2(26, 0).RotatedBy(Projectile.rotation);
            vectors[1] = vectors[2] = vectors[3] = Projectile.DProj().vector[0];
            float rotation = (Projectile.DProj().vector[0] - vectors[0]).ToRotation();
            texture = Pao.Value;
            rectangle = new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]);

            sprite = 0;
            if (Projectile.DProj().vector[0].X - Projectile.Center.X <= 0)
            {
                sprite = SpriteEffects.FlipHorizontally;
                rotation += MathHelper.Pi;
            }
            TrailDrawer.Draw(vectors, -Main.screenPosition, 88, null, Projectile.scale);

            Main.EntitySpriteDraw(VoidStar, vectors[0] - Main.screenPosition, null, new Color(252, 128, 48, 150), rotation, VoidStar.Size() / 2, 0.33f, sprite, 0);
            Main.EntitySpriteDraw(VoidStar, vectors[0] - Main.screenPosition, null, new Color(252, 128, 48, 150), rotation, VoidStar.Size() / 2, 0.33f, sprite, 0);
            Main.EntitySpriteDraw(texture, Projectile.DProj().vector[0] - Main.screenPosition, rectangle, lightColor, rotation, rectangle.Size() / 2, 1f, sprite, 0);
            Main.EntitySpriteDraw(Pao_Glow.Value, Projectile.DProj().vector[0] - Main.screenPosition, rectangle, Color.White, rotation, rectangle.Size() / 2, 1f, sprite, 0);



            vectors[0] = Projectile.Center + new Vector2(26, 0).RotatedBy(Projectile.rotation);
            vectors[1] = vectors[2] = vectors[3] = Projectile.DProj().vector[1];
            rotation = (Projectile.DProj().vector[1] - vectors[0]).ToRotation();
            texture = Qiang.Value;
            rectangle = new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]);

            sprite = 0;
            if (Projectile.DProj().vector[1].X - Projectile.Center.X <= 0)
            {
                sprite = SpriteEffects.FlipHorizontally;
                rotation += MathHelper.Pi;
            }
            TrailDrawer.Draw(vectors, -Main.screenPosition, 88, null, Projectile.scale);

            Main.EntitySpriteDraw(VoidStar, vectors[0] - Main.screenPosition, null, new Color(252, 128, 48,150), rotation, VoidStar.Size()/2, 0.33f, sprite, 0);
            Main.EntitySpriteDraw(VoidStar, vectors[0] - Main.screenPosition, null, new Color(252, 128, 48,150), rotation, VoidStar.Size()/2, 0.33f, sprite, 0);
            Main.EntitySpriteDraw(texture, Projectile.DProj().vector[1] - Main.screenPosition, rectangle, lightColor, rotation, rectangle.Size() / 2, 1f, sprite, 0);
            Main.EntitySpriteDraw(Qiang_Glow.Value, Projectile.DProj().vector[1] - Main.screenPosition, rectangle, Color.White, rotation, rectangle.Size() / 2, 1f, sprite, 0);
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Lighting.AddLight(Projectile.Center, new Color(252, 128, 48).ToVector3()*2F);
            CheckActive(player);

            bool movesFast = Movement(player);
            Animate(movesFast);
        }

        private void CheckActive(Player player)
        {
            if (!player.dead && player.HasBuff(ModContent.BuffType<微型流星破坏者Buff>()))
            {
                Projectile.timeLeft = 2;
            }
        }

        private bool Movement(Player player)
        {
            float velDistanceChange = 2f;

            int dir = -player.direction;
            Projectile.direction = Projectile.spriteDirection = dir;

            Vector2 desiredCenterRelative = new Vector2(0, -150f);

            desiredCenterRelative.Y += (float)Math.Sin(Main.GameUpdateCount / 120f * MathHelper.TwoPi) * 5;

            Vector2 desiredCenter = player.ArmCenter() + desiredCenterRelative;
            Vector2 betweenDirection = desiredCenter - Projectile.Center;
            float betweenSQ = betweenDirection.LengthSquared();

            if (betweenSQ > 1000f * 1000f || betweenSQ < velDistanceChange * velDistanceChange)
            {
                Projectile.Center = desiredCenter;
                Projectile.velocity = Vector2.Zero;
            }

            if (betweenDirection != Vector2.Zero)
            {
                Projectile.velocity = betweenDirection * 0.1f * 2;
            }

            bool movesFast = Projectile.velocity.LengthSquared() > 6f * 6f;
            Projectile.rotation = Projectile.velocity.X * 0.03F;

            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.Center;
            }
            if (Projectile.DProj().vector[1] == Vector2.Zero)
            {
                Projectile.DProj().vector[1] = Projectile.Center;
            }
            
            Vector2 vector = Projectile.Center + new Vector2(-50,30);
            Projectile.DProj().vector[0] -= (Projectile.DProj().vector[0] - vector) / 8*(Projectile.velocity.Length()/10+1);
            vector = Projectile.Center + new Vector2(50,30);
            Projectile.DProj().vector[1] -= (Projectile.DProj().vector[1] - vector) / 8 * (Projectile.velocity.Length()/10+1);

            return movesFast;
        }
        private void Animate(bool movesFast)
        {

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}
