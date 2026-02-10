using DDmod.Content.Dusts;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 真圣刃Proj : 飞刀Proj
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI1;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            Projectile.timeLeft = 330;
            Projectile.width = 20;
            Projectile.height = 20;
        }
        public override void PostAI()
        {
            //if (Main.rand.NextBool(5))
            {
                //NewDustChange(2, Projectile.Center - new Vector2(4), Vector2.Zero, 6, 0, 4, true, 1f);
            }
            Player player = Main.player[Projectile.owner];
            if (player.Aplayer().TrueHolyEnergy)
            {

                AIStyle = 飞刀AI.AI3;
            }
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            NewDustChange(10, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 3, true, 1,0,new Color(90,182,255,0));
            NewDustChange(10, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 3, true, 1,0,new Color(217,68,200, 0));
            NewDustChange(10, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 3, true, 1,0,new Color(255, 255, 104, 0));
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.owner == Main.myPlayer)
            {
                if (player.Aplayer().TrueHolyEnergy)
                    NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(Main.rand.NextFloat(-10,10),-10), ModContent.ProjectileType<真圣刃2Proj>(), Projectile.damage, 0, player.whoAmI,Main.rand.Next(3));
            }
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TrueExcalibur, settings, Projectile.owner);

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            float RO = Projectile.rotation;
            SpriteEffects sprite = 0;
            Vector2 Origia = new Vector2(texture.Width * 0.5f, Projectile.height / 2);
            Vector2 Origia2 = new Vector2(Glow.Width() * 0.5f, 20 * 4);
            if (Projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipVertically;
                RO -= Rotation2;
                Origia = new Vector2(texture.Width * 0.5f, texture.Height - Projectile.height / 2);
                Origia2 = new Vector2(Glow.Width() * 0.5f, Glow.Height() - 20 * 4);
            }
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float RO2 = Projectile.oldRot[i];
                if (Projectile.velocity.X < 0)
                {
                    RO2 -= Rotation2;
                }
                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                Color color = new Color(255, 255, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                if (!player.Aplayer().TrueHolyEnergy)
                {
                    color = lightColor * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                }

                    if (player.Aplayer().TrueHolyEnergy)
                    Main.spriteBatch.Draw(Glow.Value, vector2, null, color, RO2, Origia2, Projectile.scale / 4, sprite, 0f);
                else
                    Main.spriteBatch.Draw(texture, vector2, null, color, RO2, Origia, Projectile.scale, sprite, 0f);

            }
            if (player.Aplayer().TrueHolyEnergy)
            {
                Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0), RO, Origia2, Projectile.scale / 4, sprite, 0f);
                lightColor = Color.White;
            }

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, RO, Origia, Projectile.scale, sprite, 0f);


            return false;
        }
    }
    public class 真圣刃2Proj : 飞刀Proj
    {
        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI0;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            Projectile.timeLeft = 330;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.penetrate = -1;
        }
        public override void PostAI()
        {
            if (Projectile.ai[0] == 0)
            {
                NewDustChange(2, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 6, true, 0.4f, 0, new Color(255, 255, 104,0));
            }
            if (Projectile.ai[0] == 1)
            {
                NewDustChange(2, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 6, true, 0.4f, 0, new Color(90, 182, 255, 0));
            }
            if (Projectile.ai[0] == 2)
            {
                NewDustChange(2, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 6, true, 0.4f, 0, new Color(217, 68, 200, 0));
            }
            if (Projectile.ai[1] < 20)
            {
                Projectile.ai[1]++;
                Projectile.rotation += Projectile.velocity.X * 0.1F;
                if (Projectile.velocity.Y < 20)
                {
                    Projectile.velocity.Y += 0.2f;
                }
            }
            else if (Projectile.ai[1] == 20)
            {
                Projectile.ai[1]++;
                Projectile.tileCollide = false;
                NPC npc = NPCdirection.FindClosest(Projectile.Center, 1400, true);
                if (npc != null)
                {
                    Projectile.velocity = (npc.Center - Projectile.Center).PerfectNormalize() * 20;
                    Projectile.rotation = Projectile.velocity.ToRotation() + Rotation;
                }
            }
            else
            {
                Projectile.ai[1]++; if (Projectile.ai[1] >= 40)
                    Projectile.Kill();
            }
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 0)
            {
                NewDustChange(10, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 2, true, 1, 0, new Color(255, 255, 104, 0));
            }
            if (Projectile.ai[0] == 1)
            {
                NewDustChange(10, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 2, true, 1, 0, new Color(90, 182, 255, 0));
            }
            if (Projectile.ai[0] == 2)
            {
                NewDustChange(10, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 2, true, 1, 0, new Color(217, 68, 200, 0));
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TrueExcalibur, settings, Projectile.owner);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            float RO = Projectile.rotation;
            SpriteEffects sprite = 0;
            Vector2 Origia = new Vector2(texture.Width * 0.5f, 20 * 4);
            if (Projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipVertically;
                RO -= Rotation2;
                Origia = new Vector2(texture.Width * 0.5f, texture.Height - 20 * 4);
            }
            if (Projectile.ai[1] < 20)
            {

                Origia = new Vector2(texture.Width * 0.5f, texture.Height * 0.5F);
            }
            Texture2D texture2D = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Melee/FlyingKnife/圣刃Proj_D").Value;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.ai[1] < 20)
                {
                    Projectile.oldPos[i] = Vector2.Zero;
                }
                float RO2 = Projectile.oldRot[i];
                if (Projectile.velocity.X < 0)
                {
                    RO2 -= Rotation2;
                }
                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 10;
                Color color = new Color(255, 255, 104, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                if(Projectile.ai[0]==1)
                {
                    color = new Color(90, 182, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                }
                if(Projectile.ai[0]==2)
                {
                    color = new Color(217, 68, 200, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                }
                Main.spriteBatch.Draw(texture, vector2, null, color, RO2, Origia, Projectile.scale / 4 * 0.75F, sprite, 0f);
                Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 10, null, color , RO, Origia, Projectile.scale / 4, sprite, 0f);
            }

            lightColor = new Color(255, 255, 104);
            if (Projectile.ai[0] == 1)
            {
                lightColor = new Color(90, 182, 255);
            }
            if (Projectile.ai[0] == 2)
            {
                lightColor = new Color(217, 68, 200);
            }
            Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 10, null, lightColor, RO, Origia, Projectile.scale / 4, sprite, 0f);
            lightColor.A = 0;
            Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 10, null, lightColor, RO, Origia, Projectile.scale / 4, sprite, 0f);


            return false;
        }
    }
}