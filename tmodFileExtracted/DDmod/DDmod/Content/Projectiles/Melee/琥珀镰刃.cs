using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.NoContent.Config;

namespace DDmod.Content.Projectiles.Melee
{
    public class 琥珀镰刃 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide =false;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.alpha = 255;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void AI()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 5;
            }
            if (Projectile.velocity.X > 0)
            {
                Projectile.rotation += Projectile.velocity.X * 0.1F + Math.Abs(Projectile.velocity.Y) * 0.1F;
                Projectile.rotation += 0.2F;
            }
            else
            {
                Projectile.rotation += Projectile.velocity.X * 0.1F - Math.Abs(Projectile.velocity.Y) * 0.1F;
                Projectile.rotation -= 0.2F;
            }
            Projectile.velocity *= 0.96F;
            if (Projectile.velocity.Length() < 0.1F)
            {
                Projectile.velocity = Projectile.velocity.PerfectNormalize() * 0.1F;
            }
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (ModContent.GetInstance<DDConfigClient>().SwordHit)
            {
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/Sword");
                sound.Pitch = 0;
                sound.MaxInstances = 20;
                sound.Volume = 0.2F;
                PlaySound(sound, target.position);
            }
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = new Color(252, 193, 45, 0)*0.33f;
            if (Main.rand.NextBool(30))
            {
                target.AddBuff(ModContent.BuffType<Fossil>(), 180);
                if (target.realLife > 0)
                {
                    Main.npc[target.realLife].AddBuff(ModContent.BuffType<Fossil>(), 180);
                }
            }

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            float R = 1-Projectile.alpha / 255F;
            if (Projectile.timeLeft < 20)
            {
                R = Projectile.timeLeft / 20F;
            }
            Color color = new Color(252, 193, 45, 0) * R;
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length)*0.5F;
                Main.spriteBatch.Draw(texture, vector2, null, oldcolor, Projectile.oldRot[i], texture.Size()/2, Projectile.scale, spriteEffects, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, Color.White * R, Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);

            return false;
        }
    }
}