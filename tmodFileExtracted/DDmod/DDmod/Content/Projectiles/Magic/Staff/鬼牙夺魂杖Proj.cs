using DDmod.NoContent.Config;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class 鬼牙夺魂杖Proj : ModProjectile
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
            Projectile.HoldProj(player, 34, 0, Vector2.Zero, MathHelper.PiOver4, ValueSpeed: Value);

            if (Projectile.ai[1] >= player.ActiveItem().MagicItem().charging)
            {
                if (Projectile.soundDelay == 0)
                {
                    Projectile.soundDelay = 1145141919;
                    TryGetActiveSound(PlaySound(SoundID.Item29, Projectile.position), out var Sound);
                    Sound.Sound.Pitch = -0.3f;
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
                    Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(100, 50), vector * (player.ActiveItem().shootSpeed + Projectile.ai[1]), ModContent.ProjectileType<鬼牙弹>(), Projectile.damage / 5, Projectile.knockBack, Projectile.owner, 0, 0, Projectile.ai[1])];
                    projectile.scale = (Projectile.ai[1] * 0.75F);
                    projectile.hostile = true;
            }
            else
            {
                Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(100, 50), vector * (player.ActiveItem().shootSpeed + Projectile.ai[1]), ModContent.ProjectileType<鬼牙弹>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, Projectile.ai[1])];
                projectile.scale = (Projectile.ai[1]*0.75F);
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
            Projectile.DProj().Times[0] += 0.02F;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            SpriteEffects sprite = 0;
            float r = 0;
            if(Projectile.velocity.X<0)
            {
                sprite = SpriteEffects.FlipHorizontally;
                r = MathHelper.PiOver2;
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), lightColor, Projectile.rotation+ r, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, sprite, 0f);
            Player player = Main.player[Projectile.owner];
            Color color = new Color(220, 0, 25, 0);
            Vector2 vector2 = Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 26;
            for (int a = 0; a < 5; a++)
            {
                if (Projectile.ai[1] >= player.ActiveItem().MagicItem().charging)
                {
                    texture = DDTextures.Starlight.Value;
                    Main.spriteBatch.Draw(texture, Projectile.Center + Projectile.velocity.PerfectNormalize() * 36 - Main.screenPosition, null, color * (1 - T1), Projectile.rotation + MathHelper.PiOver4, texture.Size() / 2, new Vector2(Projectile.ai[1] / 8, Projectile.ai[1] / 2) * T1, 0, 0f);
                }
            }
            Texture2D texture3 = DDTextures.VoidStar.Value;
            //Main.spriteBatch.Draw(texture3, vector2, null, color * 0.7f, Projectile.rotation - MathHelper.PiOver4, texture3.Size() / 2, new Vector2(Projectile.ai[1] / 8, Projectile.ai[1] / 3.5f), 0, 0f);

            texture = DDTextures.Circle[9].Value;
            DDHelper.Compression(texture, color, Projectile.rotation - MathHelper.PiOver4, Projectile.Opacity, new Vector2(2 + Projectile.ai[1], 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);
            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.ai[1] / 2 / 4, 0, 0);

            if (Projectile.ai[1] >= player.ActiveItem().MagicItem().charging)
            {
                Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity.PerfectNormalize() * -36 * T1, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.ai[1] / 2 * (1-T1)/4, 0, 0);
                Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity.PerfectNormalize() * -36 * T2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.ai[1] / 2 * (1-T2)/4, 0, 0);
                Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity.PerfectNormalize() * -36 * T3, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.ai[1] / 2 * (1-T3)/4, 0, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);


            DDHelper.DrawExpanded(Main.GameViewMatrix.TransformationMatrix, Main.spriteBatch, player.Center - Main.screenPosition + new Vector2(0, -60), 0.5f, Projectile.ai[1] / player.ActiveItem().MagicItem().charging * 0.5f, color * 0.3f, color, 1.1F, 0.6f);
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