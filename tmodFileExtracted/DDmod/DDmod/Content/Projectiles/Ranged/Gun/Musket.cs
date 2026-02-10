namespace DDmod.Content.Projectiles.Ranged.Gun
{
    public class Musket : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 2;
            Projectile.coldDamage = true;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            bool canShoot = !Projectile.DProj().Bool[0] && player.HasAmmo(player.inventory[player.selectedItem]) && !player.noItems && !player.CCed && player.Dplayer().ForbiddenToAttack == 0;
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead && player.Dplayer().ForbiddenToAttack == 0;

            if (channeling)
            {
                Projectile.HoldProj(player, 10, 0, Vector2.Zero, 0, 0, true, -1);
            }
            else
            {
                Projectile.HoldProj(player, 10, 0, Projectile.velocity, 0, 0, true, -1);
            }
            if (canShoot)
            {
                if (Projectile.ai[0] <= 0)
                {
                    Projectile.ai[0] = 0;
                }
                if (!channeling)
                {
                    Projectile.CritChance += (int)(50 - Projectile.ai[0]*1.5f);
                    int A = NewProjectile(Projectile.GetSource_FromThis(), player.Center+ Projectile.velocity.PerfectNormalize()*26, Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-Projectile.ai[0] / 50, Projectile.ai[0] / 50)) * player.ActiveItem().shootSpeed, Projectile.GetGlobalProjectile<RangedProjectile>().Bullet, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    int Sp = (int)(4 - Projectile.ai[0] / 10);
                    DDHelper.MaxandMin(ref Sp, 4, 0);

                    int Type = 6;
                    for (int D = 0; D < 10; D++)
                    {
                        Dust dust = Main.dust[NewDust(player.Center + Projectile.velocity.PerfectNormalize() * 26 - new Vector2(4), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.scale = 1.8F;
                        dust.velocity = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-1f, 1f)) * Main.rand.NextFloat(1f,2.5f) * Sp;
                        dust.rotation = Projectile.rotation;
                    }
                    for (int a = 0; a < 12; a++)
                    {
                        int dust = NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * 26-new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(253, 122, 3, 0), 1);
                        Main.dust[dust].velocity = Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(3, 12);
                        Main.dust[dust].noGravity = true;
                    }
                    player.velocity -= Projectile.velocity * Sp;

                    Main.projectile[A].extraUpdates = Sp;
                    Main.projectile[A].CritChance = Projectile.CritChance;
                    PlaySound(SoundID.Item11, Projectile.position);
                    int GoreType = Mod.Find<ModGore>("弹药壳").Type;
                    Gore.NewGore(player.GetSource_Death(), player.Center + (player.itemRotation).ToRotationVector2(), (player.itemRotation).ToRotationVector2() * -4 * player.direction, GoreType, 1.3f);
                    Projectile.ai[0] = player.HeldItem.useAnimation/3;
                    Projectile.netUpdate = true;
                    Projectile.DProj().Bool[0] = true;
                }
            }
            else
            {
                if (Projectile.ai[0] <= 0)
                {
                    Projectile.Kill();
                }
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
            Player player = Main.player[Projectile.owner];
            Vector2 Center = Projectile.velocity.PerfectNormalize() * 26;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
                if (!Projectile.DProj().Bool[0])
                {
                    Vector2 Aim = new Vector2(Projectile.scale * (2 - Projectile.ai[0] / 50), Projectile.scale * 1.2F);
                    Main.spriteBatch.Draw(DDTextures.Wire.Value, Projectile.Center - Main.screenPosition + Center-new Vector2(0,2).RotatedBy(Projectile.rotation), null, new Color(255, 0, 0) * (1 - Projectile.ai[0] / 30 + 0.1F), Projectile.rotation + Projectile.ai[0] / 50, Vector2.Zero, Aim, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.Wire.Value, Projectile.Center - Main.screenPosition + Center - new Vector2(0, 2).RotatedBy(Projectile.rotation), null, new Color(255, 0, 0) * (1 - Projectile.ai[0] / 30 + 0.1F), Projectile.rotation - Projectile.ai[0] / 50, Vector2.Zero, Aim, 0, 0f);
                }
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.Pi, texture.Size() / 2, Projectile.scale, (SpriteEffects)1, 0f);
                if (!Projectile.DProj().Bool[0])
                {
                    Vector2 Aim = new Vector2(Projectile.scale * (2 - Projectile.ai[0] / 50), Projectile.scale * 1.2F);
                    Main.spriteBatch.Draw(DDTextures.Wire.Value, Projectile.Center - Main.screenPosition + Center, null, new Color(255, 0, 0) * (1 - Projectile.ai[0] / 30 + 0.1F), Projectile.rotation + Projectile.ai[0] / 50, Vector2.Zero, Aim, 0, 0f);
                    Main.spriteBatch.Draw(DDTextures.Wire.Value, Projectile.Center - Main.screenPosition + Center, null, new Color(255, 0, 0) * (1 - Projectile.ai[0] / 30 + 0.1F), Projectile.rotation - Projectile.ai[0] / 50, Vector2.Zero, Aim, 0, 0f);
                }
            }
            return false;
        }
    }
}