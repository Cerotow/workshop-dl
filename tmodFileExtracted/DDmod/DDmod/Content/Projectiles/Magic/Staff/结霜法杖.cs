using DDmod.NoContent.Config;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class 结霜法杖 : ModProjectile
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
            Projectile.coldDamage = true;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.DProj().Times[0] += 0.1f;
            float Value = 0;
            if (player.statMana >= player.ItemMana() && Main.myPlayer == Projectile.owner)
            {
                Projectile.ai[2]++;
                Value = 1;
            }
            Projectile.HoldProj(player, 24, 0, Vector2.Zero, MathHelper.PiOver4, ValueSpeed: Value);
            if (Projectile.ai[0] / player.IteUseAnimation() >= 1)
            {
                if (Projectile.soundDelay == 0)
                {
                    Projectile.soundDelay = 2;
                    Dust dust = Main.dust[NewDust(player.MountedCenter + Projectile.velocity.PerfectNormalize() * 50 - new Vector2(4), 8, 8, 135, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale *= 2;
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2.5f, 4.5f);
                }
            }
            else
            {
                if (Projectile.ai[2] >= player.IteUseAnimation() / player.ActiveItem().MagicItem().ExtraMana)
                {
                    int A = player.ItemMana();
                    player.statMana -= A;
                    Projectile.ai[2] -= player.IteUseAnimation() / player.ActiveItem().MagicItem().ExtraMana;
                }
            }
            DDHelper.MaxandMinF(ref Projectile.ai[0], player.IteUseAnimation(), 0);
            Projectile.ai[1] = Projectile.ai[0] / player.IteUseAnimation()* player.ActiveItem().MagicItem().charging;

            Projectile.damage = (int)(player.GetWeaponDamage(player.HeldItem) * (Projectile.ai[1]));

            return false;
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer != Projectile.owner || Projectile.ai[0] < player.IteUseAnimation() / player.ActiveItem().MagicItem().ExtraMana)
            {
                return;
            }
            Vector2 vector = Projectile.velocity.PerfectNormalize();
            player.velocity -= vector * Projectile.ai[1];

            if (!Collision.CanHitLine(player.Center + Projectile.velocity.PerfectNormalize() * 50, 12, 12, player.Center, player.width / 2, player.height / 2) && ModContent.GetInstance<DDConfigServer>().Staffdamage)
            {
                Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(80, 30), vector * (player.ActiveItem().shootSpeed + Projectile.ai[1]), ModContent.ProjectileType<Frostspark>(), Projectile.damage / 5, Projectile.knockBack, Projectile.owner, 0, 0, Projectile.ai[1])];
                projectile.scale = Projectile.ai[1];
                projectile.hostile = true;
            }
            else
            {
                Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(80, 30), vector * (player.ActiveItem().shootSpeed + Projectile.ai[1]), ModContent.ProjectileType<Frostspark>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, Projectile.ai[1])];
                projectile.scale = Projectile.ai[1];
            }
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, 0, 0f);
            Player player = Main.player[Projectile.owner];
            Color color = new Color(0, 186, 242, 0);
            Vector2 vector2 = Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 26;
            Texture2D texture3 = DDTextures.VoidStar.Value;
            Main.spriteBatch.Draw(texture3, vector2, null, color*0.25f, Projectile.rotation - MathHelper.PiOver4, texture3.Size() / 2, new Vector2(Projectile.ai[1] / 6, Projectile.ai[1] / 2.5f), 0, 0f);

            texture = DDTextures.Circle[9].Value;
            DDHelper.Compression(texture, color, Projectile.rotation - MathHelper.PiOver4, Projectile.Opacity, new Vector2(2 + Projectile.ai[1], 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);

            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.ai[1] / 12, 0, 0);
            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.ai[1] / 12, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            DDHelper.DrawExpanded(Main.GameViewMatrix.TransformationMatrix, Main.spriteBatch, player.Center - Main.screenPosition + new Vector2(0, -50), 0.5f, Projectile.ai[1]/ player.ActiveItem().MagicItem().charging * 0.5f, color * 0.3f, color, 1.1F, 0.6f);
            DynamicSpriteFontExtensionMethods.DrawString(
                Main.spriteBatch,
                FontAssets.MouseText.Value,
                (int)(Projectile.ai[1] / player.ActiveItem().MagicItem().charging * 100) + "%",
                player.Center - Main.screenPosition - new Vector2(0, 38),
                color, 0f,
                ChatManager.GetStringSize(FontAssets.MouseText.Value, (int)(Projectile.ai[1] / player.ActiveItem().MagicItem().charging * 100) + "%", Vector2.One) / 2,
                0.75F, SpriteEffects.None, 0f);
            return false;
        }
    }
}