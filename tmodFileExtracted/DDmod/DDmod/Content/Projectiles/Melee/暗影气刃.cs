using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.NoContent.Config;

namespace DDmod.Content.Projectiles.Melee
{
    public class 暗影气刃 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide =false;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown =5;
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
                    Projectile.rotation += Projectile.velocity.X * 0.05F + Math.Abs(Projectile.velocity.Y) * 0.05F;
                }
                else
                {
                    Projectile.rotation += Projectile.velocity.X * 0.05F - Math.Abs(Projectile.velocity.Y) * 0.05F;
                }
            Player player = Projectile.Player();
            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }
            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()) * 2)
            {
                Projectile.scale += player.GetAdjustedItemScale(player.ActiveItem())/20;
                Projectile.Center = player.Center;
            }
            if (Projectile.ai[2]>0)
            {
                if(Projectile.timeLeft>100)
                {
                    Projectile.timeLeft = 100;
                }
                Projectile.ai[2]++;
                Projectile.Center = player.Center;
            }
            Projectile.frame = 0;
            if (Projectile.localAI[2]==0)
            {
                Projectile.localAI[2] = Projectile.damage;
            }
            if (Projectile.DProj().track > 30)
            {
                Projectile.localAI[2] *= 0.99F;
                Projectile.velocity *= 0.99F;
            }
            Projectile.damage = (int)Projectile.localAI[2];
            Projectile.ProjScaleChange();
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
            Main.projectile[A].DProj().color = new Color(81, 6, 233, 0)*0.33f;
            if (Main.rand.NextBool(5))
            {
                target.AddBuff(153, 180);
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
            Color color = new Color(81, 6, 233, 0)*1.5F * R;
            Vector2 vector = Projectile.Size / 2;
            Rectangle rectangle = new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width,texture.Height/ Main.projFrames[Projectile.type]);
            Vector2 v = rectangle.Size()/2; 
            for (int A = 0; A <3; A++)
            {
                float w = 1;
                if (A == 1)
                {
                    w = 0.4F;
                }
                if (A == 2)
                {
                    w = 0.2F;
                }
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color oldcolor = new Color(27, 2, 78, 155) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length)* R*w*0.25F;
                    Color oldcolor2 = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length)  * w * 0.25F;
                    Main.spriteBatch.Draw(texture, vector2, rectangle, oldcolor, Projectile.oldRot[i]+MathHelper.TwoPi/3* A, v, Projectile.scale, spriteEffects, 0f);
                    Main.spriteBatch.Draw(texture, vector2, rectangle, oldcolor2, Projectile.oldRot[i]+MathHelper.TwoPi/3* A, v, Projectile.scale, spriteEffects, 0f);
                }
            }
            for (int A = 0; A < 3; A++)
            {
                float w = 1;
                if (A == 1)
                {
                    w = 0.4F;
                }
                if (A == 2)
                {
                    w = 0.2F;
                }
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, rectangle, new Color(27, 2, 78, 155) * R * w, Projectile.rotation + MathHelper.TwoPi / 3 * A, v, Projectile.scale, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, rectangle, color * w , Projectile.rotation + MathHelper.TwoPi / 3* A, v, Projectile.scale, spriteEffects, 0f);

            }
            return false;
        }
    }
}