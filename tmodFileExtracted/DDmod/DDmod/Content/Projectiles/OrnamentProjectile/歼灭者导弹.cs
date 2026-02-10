using DDmod.Content.Projectiles.Boss;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.OrnamentProjectile
{
    public class 歼灭者导弹 : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] =10;
            ProjectileID.Sets.TrailingMode[Type] = 0;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 500;
        }
        public int moveSpeed;
        public int moveSpeedY;
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation()+MathHelper.PiOver2;
            Projectile.Track(800, 21, 18,30,true);

            for (int A = 0; A < 200; A++)
            {
                NPC npc = Main.npc[A];
                Rectangle npcRectangle = npc.getRect();
                Rectangle playerRectangle = Projectile.getRect();
                if (npc.CanBeChasedBy(Projectile))
                {
                    //让玩家可以踩npc
                    if (playerRectangle.Intersects(npcRectangle))
                    {
                        Projectile.Kill();
                    }
                }
            }
            Projectile.netUpdate = true;
        }
        internal Color ColorFunction(float completionRatio)
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
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                10,
                20,
                30,
                20,
            }) * Projectile.scale, 10 * Projectile.scale, (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal Trailing TrailDrawer;
        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != 2)
            {
                int A = NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<歼灭者导弹爆炸>(), Projectile.damage, 1, Main.myPlayer);
                Main.projectile[A].friendly = true;
                Main.projectile[A].hostile = false;
            }
            PlaySound(SoundID.Item14, Projectile.Center);
            Projectile.active = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition-Projectile.velocity.PerfectNormalize()*8, 104, null, Projectile.scale);

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, 1, 0, 0f);
            return false;
        }
    }
}