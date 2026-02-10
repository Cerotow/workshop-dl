using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Magic.Gun
{
    public class 绿岩聚能炮Proj : ModProjectile
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
            Projectile.HoldProj(player, 16, 0, Projectile.localAI[2] == 0 ? Vector2.Zero : Projectile.velocity, 0, 0, Projectile.localAI[2] < 15&& Value==1, ValueSpeed: Value);

            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead && player.Dplayer().ForbiddenToAttack == 0;
            if (channeling|| Projectile.ai[0] / player.IteUseAnimation()<0.5F)
            {
                if (Projectile.ai[1] >= player.ActiveItem().MagicItem().charging)
                {
                    if (Projectile.soundDelay <= 10)
                    {
                        Projectile.soundDelay = 1145141919;
                        SoundStyle sound = SoundID.Item29;
                        sound.Pitch = -0.3f;
                        PlaySound(sound, Projectile.position);
                        Projectile.localAI[0]++;
                    }
                }
                else
                {
                    if (Projectile.soundDelay == 0&& Value==1)
                    {
                        Projectile.soundDelay = 5;
                        SoundStyle sound = SoundID.Item157;
                        sound.Pitch = Projectile.ai[1];
                        sound.MaxInstances = 3;
                        sound.Volume = Projectile.ai[1];
                        PlaySound(sound, Projectile.position);
                    }
                    if (Projectile.ai[2] >= player.IteUseAnimation() / player.ActiveItem().MagicItem().ExtraMana)
                    {
                        int A = player.ItemMana();
                        player.statMana -= A;
                        Projectile.ai[2] -= player.IteUseAnimation() / player.ActiveItem().MagicItem().ExtraMana;
                    }
                    if (Value == 1)
                    {
                        Vector2 vector = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.3F, 0.3F)) * 6;
                        int DU = NewDust(Projectile.PreviousCenter()-player.velocity - new Vector2(4) - new Vector2(0, 2 * player.direction).RotatedBy(Projectile.rotation) + vector * 10, 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 255, new Color(100, 255, 100, 0), 1.2F);
                        Main.dust[DU].customData = 1F;
                        Main.dust[DU].noGravity = false;
                        Main.dust[DU].velocity = -vector * 0.6F;
                        Main.dust[DU].rotation = Main.dust[DU].velocity.ToRotation();
                        GlobalDust.DustProjectileOwner[DU] = Projectile.whoAmI;
                    }
                }

                DDHelper.MaxandMinF(ref Projectile.ai[0], player.IteUseAnimation(), 0);
                Projectile.ai[1] = Projectile.ai[0] / player.IteUseAnimation() * player.ActiveItem().MagicItem().charging;

            }
            else
            {
                Projectile.damage = (int)(player.GetWeaponDamage(player.HeldItem) * (Projectile.ai[1]));
                if (Projectile.localAI[2]==0)
                {
                    if(Main.myPlayer == Projectile.owner)
                    {
                        NewProjectile(Projectile.GetSource_FromAI(),Projectile.Center,Projectile.velocity*8,ModContent.ProjectileType<绿岩激光束>(),Projectile.damage,Projectile.knockBack,-1, Projectile.ai[1]);
                    }
                    SoundStyle sound = SoundID.Item158;
                    sound.Volume = Projectile.ai[1];
                    sound.Pitch = -1;
                    PlaySound(sound, Projectile.position);
                    Projectile.ai[1] *= 2F;
                }
                if (Projectile.ai[1] > 0)
                {
                    Projectile.ai[1] -= 0.25F;
                }
                Projectile.localAI[2]++;

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
            Vector2 Center = Utils.RotatedBy(new Vector2(0, 2), Projectile.rotation, default);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            if (Main.player[Projectile.owner].direction == 1)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Center = Utils.RotatedBy(new Vector2(0, -2), Projectile.rotation, default);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.Pi, texture.Size() / 2, Projectile.scale, (SpriteEffects)1, 0f);
            }
                Texture2D texture2 = DDTextures.Starlight3.Value;
                Main.spriteBatch.Draw(texture2, Projectile.Center - Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 20, null, new Color(100, 255, 100, 00), 0, texture2.Size() / 2, Projectile.scale * new Vector2(1, 2) / 4* Projectile.ai[1], 0, 0);
                Main.spriteBatch.Draw(texture2, Projectile.Center - Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 20, null, new Color(100, 255, 100, 00), MathHelper.PiOver2, texture2.Size() / 2, Projectile.scale * new Vector2(1, 3) / 4* Projectile.ai[1], 0, 0);

            return false;
        }
    }
    public class 绿岩激光束 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.extraUpdates = 15;

        }
        public override void AI()
        {
            if (Projectile.ai[1]++ > 2)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(100, 255, 100, 0), Projectile.ai[0]*2)];
                dust.velocity = Vector2.Zero;
                dust.rotation = Projectile.velocity.ToRotation();
                dust.customData = Projectile.ai[0]/2;
            }
            Projectile.DProj().Magnification = Projectile.ai[0];
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * 0.8F);
            float A = Projectile.ai[0]*2;
            for (int a = 0; a < 15; a++)
            {
                int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(100, 255, 100, 0), A);
                Main.dust[dust].velocity = Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(2, 4) * A;
                Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                Main.dust[dust].noGravity = true;
                Main.dust[dust].customData = 1 + Main.dust[dust].DustAI(0);
            }
        }
    }
}