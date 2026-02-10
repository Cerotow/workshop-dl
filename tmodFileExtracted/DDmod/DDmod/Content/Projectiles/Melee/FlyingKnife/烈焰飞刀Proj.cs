using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 烈焰飞刀Proj : 飞刀Proj
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI1;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            Projectile.timeLeft = 330;
            Projectile.width = 20;
            Projectile.height = 20;
        }
        public override void PostAI()
        {
            //if (Main.rand.NextBool(5))
            {
                NewDustChange(2, Projectile.Center - new Vector2(4), Vector2.Zero, 6, 0, 4, true, 1f);
            }
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            NewDustChange(30, Projectile.Center, Vector2.Zero, 6, 0, 5, true, 2.5f);
            if (Projectile.owner == Main.myPlayer)
            {
                NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<火焰>(), Projectile.damage / 5, 0, player.whoAmI, 0);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            float RO = Projectile.rotation;
            SpriteEffects sprite = 0;
            Vector2 Origia = new Vector2(texture.Width * 0.5f, Projectile.height / 2);
            Vector2 Origia2 = new Vector2(Glow.Width() * 0.5f, (10 + Projectile.height / 2) * 4);
            if (Projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipVertically;
                RO -= Rotation2;
                Origia = new Vector2(texture.Width * 0.5f, texture.Height- Projectile.height / 2);
                Origia2 = new Vector2(Glow.Width() * 0.5f, Glow.Height() - (10 + Projectile.height / 2) * 4);
            }
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float RO2 = Projectile.oldRot[i];
                if (Projectile.velocity.X < 0)
                {
                    RO2 -= Rotation2;
                }
                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                Color color = new Color(255, 255, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);

                Main.spriteBatch.Draw(Glow.Value, vector2, null, color, RO2, Origia2, Projectile.scale / 4 * 0.75F, sprite, 0f);
            }

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, RO, Origia, Projectile.scale, sprite, 0f);
            Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), RO, Origia2, Projectile.scale/4, sprite, 0f);


           // Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White * 0.5F, 0, Vector2.Zero, Projectile.Size / 2, 0, 0f);
            return false;
        }
    }
    public class 火焰 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Main.projFrames[Type] = 6;
            Projectile.timeLeft = 120;
            Projectile.scale = 1.3F;
        }
        public override void PostAI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 6 == 0)
            {
                Projectile.frame++;
            }
            Projectile.frame %= 6;
            if (Main.rand.NextBool(5))
            {
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 100, default)];
                dust.noGravity = false;
                dust.scale = 1F;
                dust.velocity = new Vector2(0, -Main.rand.NextFloat(0.2F, 1));
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);

        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            NewDustChange(50, Projectile.Center, Vector2.Zero, 6, 0, 5, true, 2.5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[34].Value;
            Main.instance.LoadProjectile(34);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0,texture.Height/6*Projectile.frame, texture.Width, texture.Height/6)), Color.White, Projectile.rotation, texture.OffsetCenter(1,6), Projectile.scale, 0, 0f);
            return false;
        }
    }
}