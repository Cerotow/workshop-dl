namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class HeartStaff : ModProjectile
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
            Projectile.HoldProj(player, 24, 0, Vector2.Zero, MathHelper.PiOver4);

            if (player.statMana <= 0 && Main.myPlayer == Projectile.owner)
            {
                Projectile.Kill();
            }
            if (Projectile.ai[1] < (player.HasBuff<BlessingOfTheHeart>() ? 4.5f : 3f))
            {
                Projectile.ai[1] += 0.03f;
            }
            else
            {
                Projectile.ai[1] -= 0.03f;
            }
            if (Projectile.ai[0] > player.ActiveItem().useAnimation)
            {
                int A = player.ItemMana() / 4;
                player.statMana -= A;
                Vector2 vector2 = Projectile.velocity.PerfectNormalize();
                int a = 50;
                float scale = Projectile.ai[1];
                if (scale > 3) scale = 3;
                if (!Collision.CanHitLine(player.Center + Projectile.velocity.PerfectNormalize() * 50, 12, 12, player.Center, player.width / 2, player.height / 2))
                {
                    a = 0;
                    if (scale >= 2.9F&& Main.myPlayer == Projectile.owner)
                    {
                        Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(100, 50), vector2 * (8 + scale), ModContent.ProjectileType<MagicHeart>(), Projectile.damage / 5, Projectile.knockBack, Projectile.owner, 0, 0, scale / 2)];
                        projectile.scale = 1;
                        projectile.hostile = true;
                        TryGetActiveSound(PlaySound(SoundID.Item29, Projectile.position), out var Sound);
                        Sound.Sound.Pitch = -0f;
                    }
                }
                else
                {
                    if (scale >= 2.9F&& Main.myPlayer == Projectile.owner)
                    {
                        Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(100, 50), vector2 * (8 + scale), ModContent.ProjectileType<MagicHeart>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, scale / 2)];
                        projectile.scale = 1;
                        TryGetActiveSound(PlaySound(SoundID.Item29, Projectile.position), out var Sound);
                        Sound.Sound.Pitch = -0f;
                    }
                    scale = Projectile.ai[1] - 3;
                    if (scale >= 1.4f && Main.myPlayer == Projectile.owner)
                    {
                        Vector2 Center = Utils.RotatedBy(new Vector2(-50, 0), Projectile.rotation, default);
                        vector2 = Main.MouseWorld - (Projectile.Center + Center + Projectile.velocity.PerfectNormalize() * a);
                        vector2 = vector2.PerfectNormalize();
                        Projectile proj = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(100, 50), vector2 * (8 + scale), ModContent.ProjectileType<MagicHeart>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, scale / 2)];
                        proj.scale = 0.8F;

                        Vector2 Center2 = Utils.RotatedBy(new Vector2(0, 50), Projectile.rotation, default);
                        vector2 = Main.MouseWorld - (Projectile.Center + Center2 + Projectile.velocity.PerfectNormalize() * a);
                        vector2 = vector2.PerfectNormalize();
                        Projectile proj2 = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Center2 + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(100, 50), vector2 * (8 + scale), ModContent.ProjectileType<MagicHeart>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, scale / 2)];
                        proj2.scale = 0.8F;
                    }
                }
                Projectile.ai[0] -= 20;
            }
            int damageWithChargeAndStats = player.GetWeaponDamage(player.HeldItem);
            Projectile.damage = damageWithChargeAndStats;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
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
            float scale = Projectile.ai[1];
            if (scale > 3) scale = 3;
            for (int a = 0; a < 3; a++)
            {
                Vector2 projDirection = Utils.RotatedBy(Projectile.velocity.PerfectNormalize() * (scale + (30 - T1 * 5 * scale)), 0, default);
                Vector2 projDirection2 = Utils.RotatedBy(Projectile.velocity.PerfectNormalize() * (scale + (30 - T2 * 5 * scale)), 0, default);
                Vector2 projDirection3 = Utils.RotatedBy(Projectile.velocity.PerfectNormalize() * (scale + (30 - T3 * 5 * scale)), 0, default);
                Main.spriteBatch.Draw(DDTextures.Circle[3].Value, Projectile.Center + projDirection - Main.screenPosition, null, new Color(255, 255, 255, 0) * (1 - T1), Projectile.rotation + MathHelper.PiOver4, DDTextures.Circle[3].Size() / 2, new Vector2(scale, scale / 8) * T1, 0, 0f);
                Main.spriteBatch.Draw(DDTextures.Circle[3].Value, Projectile.Center + projDirection2 - Main.screenPosition, null, new Color(255, 255, 255, 0) * (1 - T2), Projectile.rotation + MathHelper.PiOver4, DDTextures.Circle[3].Size() / 2, new Vector2(scale, scale / 8) * T2, 0, 0f);
                Main.spriteBatch.Draw(DDTextures.Circle[3].Value, Projectile.Center + projDirection3 - Main.screenPosition, null, new Color(255, 255, 255, 0) * (1 - T3), Projectile.rotation + MathHelper.PiOver4, DDTextures.Circle[3].Size() / 2, new Vector2(scale, scale / 8) * T3, 0, 0f);
            }
            scale = Projectile.ai[1] - 3;
            if (scale > 2) scale = 2;
            int JL = 50;
            if (scale > 0 && Main.myPlayer == Projectile.owner)
            {
                for (int a = 0; a < 3; a++)
                {
                    Vector2 Center = Utils.RotatedBy(new Vector2(0, 50), Projectile.rotation, default);
                    Vector2 vector = Main.MouseWorld - (Projectile.Center + Center + Projectile.velocity.PerfectNormalize() * JL);
                    Vector2 projDirection = Utils.RotatedBy(Center + Projectile.velocity.PerfectNormalize() * (scale + (JL - T1 * 5 * scale)), 0, default);
                    Vector2 projDirection2 = Utils.RotatedBy(Center + Projectile.velocity.PerfectNormalize() * (scale + (JL - T2 * 5 * scale)), 0, default);
                    Vector2 projDirection3 = Utils.RotatedBy(Center + Projectile.velocity.PerfectNormalize() * (scale + (JL - T3 * 5 * scale)), 0, default);
                    Main.spriteBatch.Draw(DDTextures.Circle[3].Value, Projectile.Center + projDirection - Main.screenPosition, null, new Color(255, 255, 255, 0) * (1 - T1), vector.ToRotation() - MathHelper.PiOver2, DDTextures.Circle[3].Size() / 2, new Vector2(scale, scale / 8) * T1, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.Circle[3].Value, Projectile.Center + projDirection2 - Main.screenPosition, null, new Color(255, 255, 255, 0) * (1 - T2), vector.ToRotation() - MathHelper.PiOver2, DDTextures.Circle[3].Size() / 2, new Vector2(scale, scale / 8) * T2, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.Circle[3].Value, Projectile.Center + projDirection3 - Main.screenPosition, null, new Color(255, 255, 255, 0) * (1 - T3), vector.ToRotation() - MathHelper.PiOver2, DDTextures.Circle[3].Size() / 2, new Vector2(scale, scale / 8) * T3, 0, 0f);

                }
                for (int a = 0; a < 3; a++)
                {
                    Vector2 Center = Utils.RotatedBy(new Vector2(-50, 0), Projectile.rotation, default);
                    Vector2 vector = Main.MouseWorld - (Projectile.Center + Center + Projectile.velocity.PerfectNormalize() * JL);
                    Vector2 projDirection = Utils.RotatedBy(Center + Projectile.velocity.PerfectNormalize() * (scale + (JL - T1 * 5 * scale)), 0, default);
                    Vector2 projDirection2 = Utils.RotatedBy(Center + Projectile.velocity.PerfectNormalize() * (scale + (JL - T2 * 5 * scale)), 0, default);
                    Vector2 projDirection3 = Utils.RotatedBy(Center + Projectile.velocity.PerfectNormalize() * (scale + (JL - T3 * 5 * scale)), 0, default);
                    Main.spriteBatch.Draw(DDTextures.Circle[3].Value, Projectile.Center + projDirection - Main.screenPosition, null, new Color(255, 255, 255, 0) * (1 - T1), vector.ToRotation() + MathHelper.PiOver2, DDTextures.Circle[3].Size() / 2, new Vector2(scale, scale / 8) * T1, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.Circle[3].Value, Projectile.Center + projDirection2 - Main.screenPosition, null, new Color(255, 255, 255, 0) * (1 - T2), vector.ToRotation() + MathHelper.PiOver2, DDTextures.Circle[3].Size() / 2, new Vector2(scale, scale / 8) * T2, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.Circle[3].Value, Projectile.Center + projDirection3 - Main.screenPosition, null, new Color(255, 255, 255, 0) * (1 - T3), vector.ToRotation() + MathHelper.PiOver2, DDTextures.Circle[3].Size() / 2, new Vector2(scale, scale / 8) * T3, 0, 0f);
                }
            }
            return false;
        }
    }
}