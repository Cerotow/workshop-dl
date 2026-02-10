using DDmod.Content.Dusts;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Magic
{
    public class 魔法泰拉弹 : 魔法神圣弹
    {

        public override string Texture => "DDmod/Content/Projectiles/Magic/魔法神圣弹";
        public override void AI()
        {
            if (Projectile.ai[0] != 0)
            {
                Projectile.scale = Projectile.ai[0];
                Projectile.damage = (int)(Projectile.damage * Projectile.ai[0]);
                Projectile.ai[0]=0;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.tileCollide = false;
            Projectile.ProjScaleChange();
            Projectile.Track(1200, 20, 15, 18);

        }
        public override void OnKill(int timeLeft)
        {
            int Type = ModContent.DustType<光球粒子>();

            for (int A = 0; A < 40; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1f * Projectile.scale;
                dust.velocity = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) / 2 * Main.rand.NextFloat(1);
                dust.position += dust.velocity;
                dust.color = new Color(71, 233, 60, 0);
                dust.customData = 2 * Projectile.scale;
                dust.noLightEmittence = false;
            }
            for (int i = 0; i < 4; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.3F, 2.2F);
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<星光粒子>(), newColor: new Color(141, 233, 130, 0))];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(1F, 1.5F);
            }
            SoundStyle sound = SoundID.Item4;
            sound.Pitch = 0.4f;
            PlaySound(sound, Projectile.position);

        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {

            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade, settings, Projectile.owner);
        }
        public override bool PreDraw(ref Color lightColor)
        {

            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = Projectile.GetAlpha(new Color(121, 233, 110, 0));
            color.A = 0;
            Vector2 vector = Projectile.Size / 2;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length)*0.75f;
                Color oldcolor2 = Projectile.GetAlpha(new Color(100, 100, 100)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture, vector2, null, oldcolor2, Projectile.rotation, texture.Size() / 2, Projectile.scale / 3 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, vector2, null, oldcolor, Projectile.rotation, texture.Size() / 2, Projectile.scale / 3 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
            }

            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale / 3, spriteEffects, 0f);
            return false;
        }
    }
    public class 魔法神圣弹 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
        }
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 25;
            Projectile.scale =1f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.extraUpdates = 1;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return null;
        }
        public override void AI()
        {
            if(Projectile.ai[1]==1)
            {
                Projectile.scale = 0.6f;
                Projectile.Track(600, 20, 12, 5);
            }
            if(Projectile.ai[1]==2)
            {
                Projectile.scale = 0.6f;
                Projectile.extraUpdates = 1;
            }
            //真圣弹
            if(Projectile.ai[1]==3)
            {
                Projectile.scale = 0.9f;
                Projectile.extraUpdates = 2;
                Projectile.Track(600, 20, 12, 5);
            }
            if(Projectile.ai[1]==4)
            {
                Projectile.scale = 0.6f;
                Projectile.Track(600, 20, 12, 5);
            }
            Projectile.localAI[0]++;

                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if(Projectile.DProj().MouseWorld == Vector2.Zero)
            {
                Projectile.DProj().MouseWorld = Projectile.Player().Dplayer().MouseWorld;
            }
            Projectile.tileCollide = Projectile.Center.Y >= Projectile.DProj().MouseWorld.Y;
            Projectile.ProjScaleChange();
        }
        /*
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = (int)(damage * Projectile.scale);
        }*/
        public override void OnKill(int timeLeft)
        {
            int Type = ModContent.DustType<光球粒子>();
            if (Projectile.ai[1] < 3)
            {
                for (int A = 0; A < 30; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale = 1.8f * Projectile.scale;
                    dust.velocity = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) / 2 * Main.rand.NextFloat(1);
                    dust.position += dust.velocity;
                    dust.color = new Color(236, 220, 109, 150);
                    dust.customData = 3 * Projectile.scale;
                    dust.noLightEmittence = false;
                }
            }
            else
            {
                for (int A = 0; A < 30; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale = 1.8f * Projectile.scale;
                    dust.velocity = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) / 2 * Main.rand.NextFloat(1);
                    dust.position += dust.velocity;
                    dust.color = new Color(221, 80, 146, 150);
                    if(A>=15&& Projectile.ai[1]==3)
                    {
                        dust.color = new Color(236, 220, 109, 150);
                    }
                    dust.customData = 3 * Projectile.scale;
                    dust.noLightEmittence = false;
                }
            }
            SoundStyle sound = SoundID.Item4;
            sound.Pitch = 0.4f;
            PlaySound(sound, Projectile.position);
        }
        int npc = -1;
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {

            if (Projectile.ai[1] >= 3)
            {
                Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
                ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
                particleOrchestraSettings.PositionInWorld = positionInWorld;
                ParticleOrchestraSettings settings = particleOrchestraSettings;
                settings.MovementVector = Projectile.velocity;
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TrueExcalibur, settings, Projectile.owner);
            }
            else
            {
                Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
                ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
                particleOrchestraSettings.PositionInWorld = positionInWorld;
                ParticleOrchestraSettings settings = particleOrchestraSettings;
                settings.MovementVector = Projectile.velocity;
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.Excalibur, settings, Projectile.owner);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return base.GetAlpha(lightColor);
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.localAI[0] < 10)
            {
                return false;
            }
                
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = Projectile.GetAlpha(new Color(236, 200, 59));
            if (Projectile.ai[1] > 3)
            {
                color = Projectile.GetAlpha(new Color(221, 80, 146));
            }
            color.A = 0;
            Vector2 vector = Projectile.Size / 2;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Color oldcolor2 = Projectile.GetAlpha(new Color(100, 100, 100)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture, vector2, null, oldcolor2, Projectile.rotation, texture.Size()/2, Projectile.scale/2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, vector2, null, oldcolor, Projectile.rotation, texture.Size()/2, Projectile.scale/2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
            }
            if (Projectile.ai[1] == 3)
            {
                color = Projectile.GetAlpha(new Color(221, 80, 146));
            }
            color.A = 155;

            if (Projectile.ai[1] == 3)
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, Projectile.GetAlpha(new Color(100, 100, 100)), Projectile.rotation, texture.Size() / 2, Projectile.scale / 2, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale / 2, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale / 2, spriteEffects, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, Projectile.GetAlpha(new Color(200, 200, 200)), Projectile.rotation, texture.Size() / 2, Projectile.scale / 2, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale / 2, spriteEffects, 0f);

            }
            return false;
        }
    }
}