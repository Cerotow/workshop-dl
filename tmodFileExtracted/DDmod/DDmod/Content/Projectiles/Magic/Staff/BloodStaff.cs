using DDmod.NoContent.Config;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class BloodStaff : ModProjectile
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
                    SoundStyle sound = SoundID.Item29;
                    sound.Pitch = 0.2F;
                    sound.Volume = 0.1f;
                    PlaySound(sound, Projectile.position);
                    Projectile.soundDelay = 1145141919;
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

            Projectile.ai[1] = (Projectile.ai[0] / player.IteUseAnimation()+Projectile.localAI[0]) * player.ActiveItem().MagicItem().charging;

            Projectile.damage = (int)(player.GetWeaponDamage(player.HeldItem) * Projectile.ai[1]);

            if (Projectile.ai[1] / player.ActiveItem().MagicItem().charging >= 1 && Projectile.ai[1] / player.ActiveItem().MagicItem().charging < 2)
            {
                Vector2 Pvelocity = Utils.RotatedBy(Projectile.velocity.PerfectNormalize(), Main.rand.NextFloat(0, MathHelper.TwoPi), default);
                if (Projectile.owner == Main.myPlayer && Main.rand.NextBool(3) && player.controlUseTile && player.ownedProjectileCounts[ModContent.ProjectileType<SuckBloodPlayer>()] < 8)
                {
                    NewProjectile(Projectile.GetSource_FromThis(), player.Center, Pvelocity * 5, ModContent.ProjectileType<SuckBloodPlayer>(), 10, 0, Projectile.owner, Projectile.whoAmI, Projectile.type);
                    player.statLife -= 3;
                    if (player.statLife <= 0)
                    {
                        player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.DDmod.PlayerKill.Kill3", player.name)), 0, 0);
                    }
                    CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 1, 1), new Color(255, 47, 37), "-" + 3, false, false);
                }
            }
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
                Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(80, 30), vector * (player.ActiveItem().shootSpeed + Projectile.ai[1]), ModContent.ProjectileType<BloodBomb>(), Projectile.damage / 5, Projectile.knockBack, Projectile.owner, 0, 0, Projectile.ai[1])];
                projectile.scale = Projectile.ai[1]*0.75F;
                projectile.hostile = true;
            }
            else
            {
                Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(80, 30), vector * (player.ActiveItem().shootSpeed + Projectile.ai[1]), ModContent.ProjectileType<BloodBomb>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, Projectile.ai[1])];
                projectile.scale = Projectile.ai[1] * 0.75F;
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
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, 0, 0f);
            Player player = Main.player[Projectile.owner];
            Color color = new Color(255, 47, 37, 0);
            Vector2 vector2 = Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 26;
            for (int a = 0; a < 5; a++)
            {
                if (Projectile.ai[1] / player.ActiveItem().MagicItem().charging>=2)
                {
                    texture = DDTextures.Starlight.Value;
                    Main.spriteBatch.Draw(texture, Projectile.Center + Projectile.velocity.PerfectNormalize() * 46 - Main.screenPosition, null, color * (1 - T1), Projectile.rotation + MathHelper.PiOver4, texture.Size() / 2, new Vector2(Projectile.ai[1] / 5, Projectile.ai[1] / 2) * T1, 0, 0f);
                }
            }
            Texture2D texture3 = DDTextures.VoidStar.Value;
            Main.spriteBatch.Draw(texture3, vector2, null, color * 0.7f, Projectile.rotation - MathHelper.PiOver4, texture3.Size() / 2, new Vector2(Projectile.ai[1] / 6, Projectile.ai[1] / 2.5f), 0, 0f);

            texture = DDTextures.Circle[7].Value;
            DDHelper.Compression(texture, color, Projectile.rotation - MathHelper.PiOver4, Projectile.Opacity, new Vector2(2 + Projectile.ai[1], 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);
            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.ai[1] / 3, 0, 0);
            if (Projectile.ai[0] > 5)
            {
                Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity.PerfectNormalize() * -30 * T1, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.ai[1] / 3 * (1 - T1), 0, 0);
                Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity.PerfectNormalize() * -30 * T2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.ai[1] / 3 * (1 - T2), 0, 0);
                Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity.PerfectNormalize() * -30 * T3, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.ai[1] / 3 * (1 - T3), 0, 0);
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