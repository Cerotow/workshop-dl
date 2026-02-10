using DDmod.NoContent.Config;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class ForestWoodenMagicwand : ModProjectile
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
            if (Projectile.ai[0] > player.IteUseAnimation())
            {
                Vector2 vector2 = Projectile.velocity.PerfectNormalize();
                int A = player.ItemMana();
                player.statMana -= A;
                if (Main.myPlayer == Projectile.owner)
                {
                    NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(80, 50), vector2 * 5, ModContent.ProjectileType<ForestBullets>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0, 1);
                }
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/法杖");
                sound.Pitch = 1.5F;
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

            Main.spriteBatch.Draw(DDTextures.VoidStar.Value, Projectile.Center - Main.screenPosition+Projectile.velocity.PerfectNormalize()*10, null, new Color(125, 213, 18, 0)* (Projectile.ai[0] / player.IteUseAnimation()), Projectile.rotation-MathHelper.PiOver4, DDTextures.VoidStar.Size() / 2, new Vector2(Projectile.scale/4,Projectile.scale/6), 0, 0f);
            Main.spriteBatch.Draw(DDTextures.VoidStar.Value, Projectile.Center - Main.screenPosition+Projectile.velocity.PerfectNormalize()*10, null, new Color(125, 213, 18, 0)* (Projectile.ai[0] / player.IteUseAnimation())*0.2f, Projectile.rotation-MathHelper.PiOver4, DDTextures.VoidStar.Size() / 2, new Vector2(Projectile.scale/4,Projectile.scale/6), 0, 0f);
            return false;
        }
    }
}