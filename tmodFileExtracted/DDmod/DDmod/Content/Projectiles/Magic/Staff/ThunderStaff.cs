using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using DDmod.NoContent.Config;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class ThunderStaff : ModProjectile
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
            Projectile.HoldProj(player, 32, 0, Vector2.Zero, MathHelper.PiOver4);

            if (player.statMana < player.ItemMana() && Main.myPlayer == Projectile.owner)
            {
                Projectile.Kill();
                return false;
            }
            if (Projectile.ai[0] > player.IteUseAnimation())
            {
                Vector2 vector2 = Projectile.velocity.PerfectNormalize();
                int A = player.ItemMana();
                player.statMana -= A;
                if (Main.myPlayer == Projectile.owner)
                {
                    int proj = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(80, 50), vector2 * player.ActiveItem().shootSpeed, ModContent.ProjectileType<MagicElectric3>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0.75F,200);
                    Main.projectile[proj].DamageType = DamageClass.Magic;
                }
                SoundStyle sound = SoundID.Item72;
                sound.Volume = 0.5F;
                PlaySound(sound, Projectile.position);
                Projectile.ai[0] -= player.IteUseAnimation();
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
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            if (Projectile.ai[0] / player.IteUseAnimation() < 0.5F)
            {
                return false;
            }
            Main.spriteBatch.Draw(DDTextures.GlowEffect.Value, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 24, null, new Color(0, 186, 242, 0) * (Projectile.ai[0] / player.IteUseAnimation()), Projectile.rotation - MathHelper.PiOver4, DDTextures.GlowEffect.Size() / 2, new Vector2(Projectile.scale / 2, Projectile.scale / 6)/2, 0, 0f);
            Main.spriteBatch.Draw(DDTextures.GlowEffect.Value, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 24, null, new Color(255, 70, 15, 0) * (Projectile.ai[0] / player.IteUseAnimation()) * 0.2f, Projectile.rotation - MathHelper.PiOver4, DDTextures.GlowEffect.Size() / 2, new Vector2(Projectile.scale / 2, Projectile.scale / 6)/2, 0, 0f);

            Main.spriteBatch.Draw(DDTextures.Starlight.Value, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 24, null, new Color(0, 186, 242, 0) * (Projectile.ai[0] / player.IteUseAnimation()), Projectile.rotation - MathHelper.PiOver4, DDTextures.Starlight.Size() / 2, new Vector2(Projectile.scale/3, Projectile.scale), 0, 0f);
            Main.spriteBatch.Draw(DDTextures.Starlight.Value, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 24, null, new Color(255, 70, 15, 0) * (Projectile.ai[0] / player.IteUseAnimation()) * 0.2f, Projectile.rotation - MathHelper.PiOver4, DDTextures.Starlight.Size() / 2, new Vector2(Projectile.scale/3, Projectile.scale), 0, 0f);
            return false;
        }
    }
    public class MagicElectric3 : 闪电
    {
        public override void AI()
        {
            if (V2 == null)
            {
                V2 = new List<Vector2>();
            }
            if (Projectile.ai[2] != 0)
            {
                Projectile.timeLeft = (int)Projectile.ai[2];
                Projectile.localAI[2] = Projectile.ai[2];
                Projectile.ai[2] = 0;
            }
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity.PerfectNormalize() * 3;
                Projectile.velocity = Projectile.DProj().vector[0];
            }
            if (Projectile.timeLeft < 2)
            {
                if (Vector == null)
                {
                    Vector = new Vector2[Projectile.oldPos.Length];
                    for (int i = 0; i < Projectile.oldPos.Length; i++)
                    {
                        Vector[i] = Projectile.oldPos[i];
                    }
                }
                Projectile.timeLeft = 10000;
            }
            else
            if (Projectile.timeLeft > 1000)
            {
                Projectile.extraUpdates = 15;
                Projectile.damage = 0;
                Projectile.timeLeft = 10000;
                Projectile.velocity = Vector2.Zero;
                Projectile.scale -= 0.002F;
                if (Projectile.ai[0] == 0)
                {
                    Projectile.scale -= 0.002F;
                }
                if (Projectile.scale <= 0)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.scale = Projectile.ai[1];
                V2.Add(Projectile.position);
                Projectile.DProj().Times[0]++;
                if (Projectile.DProj().Times[0] > 20 && Main.rand.NextBool(10) && Projectile.DProj().track > 3)
                {
                    Projectile.DProj().Times[0] = 0;
                    Vector2 vector1 = Utils.RotatedBy(Projectile.DProj().vector[0].PerfectNormalize() * Projectile.velocity.Length(), Main.rand.NextFloat(-0.6F, 0.6F), default);
                    Projectile.velocity = vector1;
                    Projectile.netUpdate = true;
                }
            }
        }
    }
}