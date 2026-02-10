using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Summon.Minions;
using Terraria;

namespace DDmod.Content.Projectiles.Summon
{
    public class SGreenLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 0.01F;
            Projectile.timeLeft =10;
            Projectile.extraUpdates = 0;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            for (int a = 0; a < 1000; a++)
            {
                Projectile Proj = Main.projectile[a];
                if (Proj.active && Proj.owner == Projectile.owner && Proj.type == ModContent.ProjectileType<MeteorYshapedDrone>() && Projectile.ai[0] == Proj.projUUID)
                {
                    Projectile.ai[2] = a;
                    break;
                }
            }
            if (Main.projectile[(int)Projectile.ai[2]].ai[1] == 0 || Main.projectile[(int)Projectile.ai[2]].type != ModContent.ProjectileType<MeteorYshapedDrone>() || !Main.projectile[(int)Projectile.ai[2]].active)
            {
                Projectile.Kill();
            }
            Projectile.width = (int)(12 * Projectile.scale * 4);
            Projectile.height = (int)(12 * Projectile.scale * 4);
            if (Projectile.scale < 0.25F)
            {
                Projectile.scale += 0.0006F;
            }
            else
            {
                Projectile.scale = 0.25f;
            }
            Projectile.ai[1] = Main.projectile[(int)Projectile.ai[2]].ai[1] - 20;
            if (Projectile.ai[1] > 400)
            {
                Projectile.Kill();
            }
            if (Projectile.localAI[1] < 1)
            {
                Projectile.localAI[1] += 0.03F;
            }
            else
            {
                Projectile.localAI[1] = 1;
            }
            if (Projectile.soundDelay <= 0)
            {
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/激光");
                sound.SoundLimitBehavior = SoundLimitBehavior.IgnoreNew;
                sound.MaxInstances = 15;
                PlaySound(sound, Projectile.Center);
                Projectile.soundDelay = 12;
            }
            Main.projectile[(int)Projectile.ai[2]].DProj().Times[0] = Projectile.scale * 4;
            Projectile.Center = Main.projectile[(int)Projectile.ai[2]].Center + new Vector2(0, 14);
            Projectile.rotation = Main.projectile[(int)Projectile.ai[2]].DProj().vector[0].ToRotation() - MathHelper.PiOver2;
            Projectile.velocity = Main.projectile[(int)Projectile.ai[2]].DProj().vector[0];
            Projectile.timeLeft = 10;
        }
        public override void OnKill(int timeLeft)
        {

        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            
            modifiers.SourceDamage *= (Projectile.scale * 4);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            int Type = ModContent.DustType<激光粒子>();
            for (int A = 0; A < 2; A++)
            {
                Dust dust = Main.dust[NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100,new Color(194, 37, 15,0))];
                dust.noGravity = true;
                dust.scale = 0.6F;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.5f, 1f);
                dust.rotation = dust.velocity.ToRotation();
            }
            Projectile.netUpdate = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Projectile.DProj().Times[0]+=3;
            if(Projectile.DProj().Times[0] > Projectile.ai[1] / 4 * Projectile.localAI[1])
            {
                Projectile.DProj().Times[0] = 0;
            }
            for (int i = 0; i < Projectile.ai[1] / 4 * Projectile.localAI[1]-1; i++)
            {
                Vector2 vector2 = Projectile.position + vector + Projectile.velocity.PerfectNormalize() * (4 * i) - Main.screenPosition;
                Color color = new Color(194, 37, 15, 0) * Projectile.scale * 4;
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, (Projectile.scale + 0.2F) * new Vector2(1 - Projectile.localAI[0], 1), spriteEffects, 0f);
                if (Projectile.DProj().Times[0] == i)
                {
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, (Projectile.scale + 0.2F) * new Vector2(1 - Projectile.localAI[0], 1) * 2, spriteEffects, 0f);
                }
                color = new Color(255 - 194, 255 - 37, 255 - 15, 0);

                if (Projectile.scale >= 0.1F)
                {
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, (Projectile.scale + 0.2F) * new Vector2(0.15F - Projectile.localAI[0] / 4, 1F), spriteEffects, 0f); 
                    if (Projectile.DProj().Times[0] == i)
                    {
                        Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, (Projectile.scale + 0.2F) * new Vector2(0.15F - Projectile.localAI[0] / 4, 1F)*2, spriteEffects, 0f);
                    }
                }
            }
            if (Projectile.localAI[1] >= 1)
            {
                Vector2 vector2 = Projectile.position + vector + Projectile.velocity.PerfectNormalize() * (4 * (Projectile.ai[1] / 4)) - Main.screenPosition;
                Color color = new Color(194, 37, 15, 0);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, (Projectile.scale + 0.2F) * 3, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, (Projectile.scale + 0.2F) * 3, spriteEffects, 0f);
                    color = new Color(255- 194, 255-37, 255-15 , 0);
                if (Projectile.scale >= 0.1F)
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, (Projectile.scale + 0.2F) * 3 * new Vector2(0.55F, 0.5F), spriteEffects, 0f);
            }
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            bool Bool = false;
            Vector2 vector;
            for (int i = 0; i < Projectile.ai[1]/4 * Projectile.localAI[1]; i++)
            {
                vector = new Vector2(projHitbox.X, projHitbox.Y) + (Projectile.velocity.PerfectNormalize() * (4 * i));

                if (new Rectangle((int)vector.X, (int)vector.Y, projHitbox.Width, projHitbox.Height).Intersects(targetHitbox))
                {
                    Bool =  true;
                }
            }
            vector = new Vector2(projHitbox.X, projHitbox.Y) + (Projectile.velocity.PerfectNormalize() * (4 * (Projectile.ai[1] / 4 * Projectile.localAI[1])));
        
            projHitbox.Width *= 2;
            projHitbox.Height *= 2;
            if (new Rectangle((int)vector.X+ Projectile.width /2 - projHitbox.Width/4, (int)vector.Y + Projectile.height /2 - projHitbox.Height/4, projHitbox.Width, projHitbox.Height).Intersects(targetHitbox))
            {
                Bool = true;
            }
            return Bool;
        }
    }
}