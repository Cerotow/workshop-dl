namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class 仙人掌杖Proj : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 2;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.ArmCenter(), reverseRotation: false, addGfxOffY: false);

            Projectile.DProj().vector[0] = (player.Dplayer().MouseWorld - vector).PerfectNormalize();
            Projectile.netUpdate = true;
            Projectile.velocity = Projectile.DProj().vector[0].PerfectNormalize();

            if (!Projectile.DProj().Bool[0])
            {
                Projectile.DProj().Times[1] = Projectile.DProj().vector[0].ToRotation();
                Projectile.DProj().Bool[0] = true;
            }
            DDHelper.RotateSpeed(ref Projectile.DProj().Times[1], Projectile.DProj().vector[0].ToRotation(), 0.08F);
            Projectile.HoldProj(player, 24, Projectile.DProj().Times[1], new Vector2(1, 0), MathHelper.PiOver4);
            if (player.statMana <= 0 && Main.myPlayer == Projectile.owner)
            {
                Projectile.Kill();
            }

            if (Projectile.ai[1] < 2)
            {
                Projectile.ai[1] += 0.025f;
                Projectile.ai[0] = 0;
            }
            else if (Projectile.ai[0] > player.ActiveItem().useAnimation && Main.myPlayer == Projectile.owner)
            {
                int A = player.ItemMana();
                player.statMana -= A;
                Projectile.ai[0] -= player.ActiveItem().useAnimation;
                for(int a = 0;a<Main.rand.Next(1,4); a++)
                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(64, 50) + new Vector2(0, Main.rand.NextFloat(-20, 20)).RotatedBy(Projectile.rotation - MathHelper.PiOver4), Projectile.velocity * Main.rand.NextFloat(4,8), ModContent.ProjectileType<魔法仙人掌刺>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.whoAmI);
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public float T1 = 0;
        public float T2 = 0.33f;
        public float T3 = 0.66f;
        public float H2 = 1;
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DProj().Times[0] += 0.02F;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, 0, 0f);
            Player player = Main.player[Projectile.owner];

            Color color = new Color(80, 132, 38, 0)*0.5f;
            Vector2 vector2 = Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 22;

            Texture2D texture3 = DDTextures.VoidStar.Value;
            Main.spriteBatch.Draw(texture3, vector2, null, color * 0.7f, Projectile.rotation - MathHelper.PiOver4, texture3.Size() / 2, new Vector2(Projectile.ai[1] / 6, Projectile.ai[1] / 2.5f), 0, 0f);

            texture = DDTextures.Circle[7].Value;
            color = new Color(80, 132, 38, 75);
            DDHelper.Compression(texture, color, Projectile.rotation - MathHelper.PiOver4, Projectile.Opacity, new Vector2(1.5f+Projectile.ai[1]*2, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.AlphaBlend);
            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.ai[1] / 3, 0, 0);
            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.ai[1] / 3, 0, 0);
            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.ai[1] / 3, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}