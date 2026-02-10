using DDmod.NoContent.Config;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class TopazStaff : ModProjectile
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
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            float Value = 0;
            if (player.statMana >= player.ItemMana() && Main.myPlayer == Projectile.owner)
            {
                Projectile.ai[2]++;
                Value = 1;
            }
            Projectile.HoldProj(player, 24, 0, Vector2.Zero, MathHelper.PiOver4, ValueSpeed: Value);

            if (Projectile.ai[1] >= player.ActiveItem().MagicItem().charging)
            {
                if (Projectile.soundDelay == 0)
                {
                    Projectile.soundDelay = 1145141919;
                    TryGetActiveSound(PlaySound(SoundID.Item29, Projectile.position), out var Sound);
                    Sound.Sound.Pitch = -0.3f;
                    Projectile.localAI[0]++;
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
            Projectile.ai[1] = Projectile.ai[0] / player.IteUseAnimation() * player.ActiveItem().MagicItem().charging;

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
                Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(100, 50), vector * (player.ActiveItem().shootSpeed + Projectile.ai[1]), ModContent.ProjectileType<TopazBullet>(), Projectile.damage / 5, Projectile.knockBack, Projectile.owner, 0, 0, Projectile.ai[1])];
                projectile.scale = Projectile.ai[1];
                projectile.hostile = true;

            }
            else
            {
                Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(100, 50), vector * (player.ActiveItem().shootSpeed + Projectile.ai[1]), ModContent.ProjectileType<TopazBullet>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, Projectile.ai[1])];
                projectile.scale = Projectile.ai[1];
            }
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
            T1 += 0.033f;
            T2 += 0.033f;
            T3 += 0.033f;
            if (T1 > 0.99f) T1 = 0;
            if (T2 > 0.99f) T2 = 0;
            if (T3 > 0.99f) T3 = 0;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, 0, 0f);
            Player player = Main.player[Projectile.owner];
            Color color = new Color(255, 105, 0, 0);
            for (int a = 0; a < 3; a++)
            {
                Vector2 projDirection = Utils.RotatedBy(Projectile.velocity.PerfectNormalize() * (Projectile.ai[1] + (30 - T1 * 5 * Projectile.ai[1])), 0, default);
                Vector2 projDirection2 = Utils.RotatedBy(Projectile.velocity.PerfectNormalize() * (Projectile.ai[1] + (30 - T2 * 5 * Projectile.ai[1])), 0, default);
                Vector2 projDirection3 = Utils.RotatedBy(Projectile.velocity.PerfectNormalize() * (Projectile.ai[1] + (30 - T3 * 5 * Projectile.ai[1])), 0, default);
                Main.spriteBatch.Draw(DDTextures.Circle[2].Value, Projectile.Center + projDirection - Main.screenPosition, null, color * (1 - T1), Projectile.rotation + MathHelper.PiOver4, DDTextures.Circle[2].Size() / 2, new Vector2(Projectile.ai[1] / 2, Projectile.ai[1] / 8) * T1, 0, 0f);
                Main.spriteBatch.Draw(DDTextures.Circle[2].Value, Projectile.Center + projDirection2 - Main.screenPosition, null, color * (1 - T2), Projectile.rotation + MathHelper.PiOver4, DDTextures.Circle[2].Size() / 2, new Vector2(Projectile.ai[1] / 2, Projectile.ai[1] / 8) * T2, 0, 0f);
                Main.spriteBatch.Draw(DDTextures.Circle[2].Value, Projectile.Center + projDirection3 - Main.screenPosition, null, color * (1 - T3), Projectile.rotation + MathHelper.PiOver4, DDTextures.Circle[2].Size() / 2, new Vector2(Projectile.ai[1] / 2, Projectile.ai[1] / 8) * T3, 0, 0f);
            }
            for (int a = 0; a < 5; a++)
            {
                if (Projectile.ai[1] >= player.ActiveItem().MagicItem().charging)
                {
                    texture = DDTextures.Starlight.Value;
                    Main.spriteBatch.Draw(texture, Projectile.Center + Projectile.velocity.PerfectNormalize() * 36 - Main.screenPosition, null, color * (1 - T1), Projectile.rotation + MathHelper.PiOver4, texture.Size() / 2, new Vector2(Projectile.ai[1] / 5, Projectile.ai[1] / 2) * T1, 0, 0f);
                }
            }
            DDHelper.DrawExpanded(Main.GameViewMatrix.TransformationMatrix, Main.spriteBatch, player.Center - Main.screenPosition + new Vector2(0, -50), 0.5f, Projectile.ai[1] / player.ActiveItem().MagicItem().charging * 0.5f, color * 0.3f, color, 1.1F, 0.6f);
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