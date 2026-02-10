using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Magic.Book
{
    public class 苍穹原典Proj : ModProjectile
    {
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Main.projFrames[Projectile.type] = 8;
        }
        public override bool PreAI()
        {
            Player player = Projectile.Player();
            Projectile.HoldProj(player, Projectile.DProj().Times[1], 0, new Vector2(player.direction, 0), 0, 0, true, Projectile.DProj().Times[4] >= 1f ? 1 : 0, false);

            Projectile.HoldBook(player.ItemMana(), 50, 15, 0.1F, 3);

            if (Projectile.ai[0] >= player.HeldItem.useAnimation && Projectile.DProj().Bool[1])
            {
                Vector2 vector2 = Projectile.velocity.PerfectNormalize();

                if (Main.myPlayer == Projectile.owner)
                {
                    //消耗魔力
                    int A = player.ItemMana();
                    player.statMana -= A;

                    Vector2 vector13 = Main.MouseWorld - Projectile.Center;
                    //如果玩家需要翻转
                    if (vector13.X > 0)
                    {
                        player.ChangeDir(1);
                    }
                    else
                    {
                        player.ChangeDir(-1);
                    }
                    Projectile.netUpdate = true;
                    vector2 = vector13.PerfectNormalize();
                }
                int Type = ModContent.DustType<星星粒子>();
                if (Main.rand.NextBool(5))
                {
                    for (float A = 0; A < 10; A++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, default)];
                        dust.noGravity = false;
                        dust.scale = 0.5F;
                        dust.customData = 1;
                        Vector2 vector = Main.rand.NextVector2Unit(-MathHelper.PiOver2 - 0.6F, 1.2F);
                        dust.position += vector.PerfectNormalize() * 8F;
                        dust.velocity = vector * Main.rand.NextFloat(2.5f, 3.5f);
                    }
                    if (Projectile.owner == Main.myPlayer)
                    {
                        for (float A = 0; A < 3; A++)
                        {
                            float R = Main.rand.NextFloat(1.5F, 2F);
                            Projectile proj = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, -Main.rand.NextFloat(6, 9)).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)), ModContent.ProjectileType<ArousalStar>(), (int)(Projectile.damage * R), Projectile.knockBack, Projectile.owner, 0, R, R)];
                        }
                    }
                }
                else
                {
                    for (float A = 0; A < 3; A++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, default)];
                        dust.noGravity = false;
                        dust.scale = 0.2F;
                        dust.customData = 1;
                        Vector2 vector = Main.rand.NextVector2Unit(-MathHelper.PiOver2 - 0.6F, 1.2F);
                        dust.position += vector.PerfectNormalize() * 8F;
                        dust.velocity = vector * Main.rand.NextFloat(2.5f, 3.5f);
                    }
                    if (Projectile.owner == Main.myPlayer)
                    {
                        float R = Main.rand.NextFloat(0.75F, 1.25F);
                        Projectile proj = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, -Main.rand.NextFloat(6, 9)).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)), ModContent.ProjectileType<ArousalStar>(), (int)(Projectile.damage * R), Projectile.knockBack, Projectile.owner, 0, R, R)];
                    }
                }
                PlaySound(SoundID.Item8, Projectile.position);
                Projectile.ai[0] -= player.HeldItem.useAnimation;
                Projectile.netUpdate = true;
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            Projectile.DProj().Times[0] += 0.1f;
            Texture2D texture = DDTextures.Circle[4].Value;
            for (int A = 0; A < 1; A++)
            {
                Color color = new Color(0, 120, 233, 0);
                Vector2 vector2 = Projectile.Center - Main.screenPosition + new Vector2(-2, 7);
                Main.spriteBatch.Draw(VoidStar, vector2, null, color * 0.05f, 0, VoidStar.Size() / 2, new Vector2(0.5f, 0.2f) * Projectile.DProj().Times[4], 0, 0f);
                DDHelper.Compression(texture, color, 0, Projectile.Opacity, new Vector2(2, 6), 1, Projectile.DProj().Times[0], BlendState.Additive);

                Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.DProj().Times[4], 0, 0);
                Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.DProj().Times[4], 0, 0);
                Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.DProj().Times[4], 0, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            }
            texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            SpriteEffects sprite = (SpriteEffects)((Main.player[Projectile.owner].direction == 1) ? 0 : 1);
            Rectangle? rectangle = new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]));
            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2);
            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation, vector, new Vector2(Projectile.scale, Projectile.scale / 1.5f) / 1.3f, sprite, 0f);

            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, Projectile.rotation + MathHelper.Pi, vector, new Vector2(Projectile.scale, Projectile.scale / 1.5f) / 1.3f, sprite, 0f);
 }

            return false;
        }
    }
}