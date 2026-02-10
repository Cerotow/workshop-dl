
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 星旋闪电球 : ModProjectile
    {
        public Asset<Texture2D> Glow;
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 80;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override void AI()
        {
            Projectile.rotation = 0;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Charged2>(), 180);
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            for (int A = 0; A < 30; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<激光粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(29, 255, 187, 0))];
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(2F, 4.2F);
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 12f);
                dust.rotation = dust.velocity.ToRotation();
                dust.customData = -3;
            }
            if (Projectile.owner == Main.myPlayer)
            {
                for (int a = 0; a < 10; a++)
                {
                    NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 8, ModContent.ProjectileType<星旋闪电>(), Projectile.damage / 4, Projectile.knockBack, -1, 0,1F, Main.rand.Next(60, 120));
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Rectangle rectangle = new Rectangle(0,0,texture.Width,texture.Height/4);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color color = new Color(255, 255, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * 0.25f;
                Main.spriteBatch.Draw(texture, vector2, rectangle, color, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, 0, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, Color.White, Projectile.rotation, rectangle.Size() / 2, Projectile.scale, 0, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, new Color(255, 255, 255, 0), Projectile.rotation, rectangle.Size() / 2, Projectile.scale, 0, 0f);
            return false;
        }
    }
    public class 星旋闪电 : 闪电
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
                Projectile.scale -= 0.004F;
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
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Charged2>(), 30);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4, -14), 1, 1, ModContent.DustType<光球粒子>(),0,0,0,new Color(29, 255, 187, 0))];
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(1, 4);
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale = Projectile.ai[1];
            }
            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
            sound.Pitch = -0.1f;
            sound.Volume = .1f;
            PlaySound(sound, Projectile.position);
            Projectile.netUpdate = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
            sound.Volume = 0.3f;
            sound.Pitch = 0.5f;
            if(Projectile.timeLeft>2&& Projectile.timeLeft<1000)
                Projectile.timeLeft = 2;
            PlaySound(sound, Projectile.position);
            int Type = ModContent.DustType<光球粒子>();
            for (int A = 0; A < 20; A++)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(29, 255, 187, 0))];
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(1F, 2.2F);
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 6f);
                dust.rotation = Projectile.rotation;
                dust.customData = -3;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = DDTextures.MiniVoidStar.Value;
            Color color = new Color(29, 255, 187, 0);
            Vector2 vector = Projectile.Size / 2;
            if (V2 == null)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    if (Projectile.oldPos[i] != Projectile.position)
                    {
                        Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                        Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale / 6 + 0.15f + (float)i / 375 / 4, spriteEffects, 0f);
                        Main.spriteBatch.Draw(texture, vector2, null, color.Opposite(), Projectile.rotation, texture.Size() / 2, Projectile.scale / 10 + 0.025F + (float)i / 375 / 4 * 0.6f, spriteEffects, 0f);
                    }
                }
            }
            else
            {
                float ro = 0;
                for (int i = 0; i < V2.Count; i++)
                {
                    float sc = Projectile.scale * (1F - i / Projectile.localAI[2]);
                    bool flag = false;
                    if (i > 0)
                    {
                        float r2 = (V2[i] - V2[i - 1]).ToRotation();
                        if (ro != r2)
                        {
                            ro = r2;
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    if (V2[i] != Projectile.position)
                    {
                        Vector2 vector2 = V2[i] + vector - Main.screenPosition;
                        Main.spriteBatch.Draw(texture, vector2, null, color, ro, texture.Size() / 2, new Vector2(sc >= 0.2F ? sc : 0.2F, Projectile.scale * (1F - i / Projectile.localAI[2])) / 2, spriteEffects, 0f);
                        Main.spriteBatch.Draw(texture, vector2, null, color.Opposite(), ro, texture.Size() / 2, new Vector2(sc >= 0.2F ? sc : 0.2F, Projectile.scale * (1F - i / Projectile.localAI[2]) / 4) / 2, spriteEffects, 0f);
                        if (i == 0)
                        {
                            sc *= 3F;
                            vector2 = V2[i] + vector - Main.screenPosition;
                            Main.spriteBatch.Draw(texture, vector2, null, color, ro, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
                            Main.spriteBatch.Draw(texture, vector2, null, color.Opposite(), ro, texture.Size() / 2, Projectile.scale / 2, spriteEffects, 0f);
                        }
                    }
                }
            }
            return false;
        }
    }
}