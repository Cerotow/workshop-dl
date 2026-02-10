using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;

namespace DDmod.Content.Projectiles.Melee.Boomerang.Chakram
{
    public class 流星飞盘 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.coldDamage = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.localAI[0] = 2;
        }
        public override void AI()
        {
            Projectile.HoldChakram(12, new Vector2(50)*1.5f);
            if (Projectile.DProj().Times[1]>=90)
            {
                if (Projectile.ai[0]==0&&Main.myPlayer==Projectile.owner)
                {
                    for (int a = 0; a < 10; a++)
                    {
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, -7).RotatedBy(a/10F*MathHelper.TwoPi), ModContent.ProjectileType<流星飞盘2>(), Projectile.damage / 4, 0);
                    }
                    Projectile.ai[0] = 1;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Rectangle value = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 vector = Projectile.Size/2;
            if (Projectile.ai[0] == 2)
            {
                lightColor *= 0.4F;
            }
            SpriteEffects sprite = 0;
            if(Projectile.Player().direction==1)
            {
                sprite = SpriteEffects.FlipHorizontally;

            }
            float r = Projectile.DProj().Times[2];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                Color color = new Color(248, 66, 5, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                Main.spriteBatch.Draw(texture, vector2, new Rectangle?(value), color*0.1f, Projectile.oldRot[i], texture.Size() / 2, Projectile.scale * r*2, sprite, 0f);
            }
            Main.spriteBatch.Draw(texture,Projectile.Center- Main.screenPosition, new Rectangle?(value), new Color(248, 66, 5, 0), Projectile.rotation, texture.Size()/2, Projectile.scale * r * 2, sprite, 0f);
            Main.spriteBatch.Draw(texture,Projectile.Center- Main.screenPosition, new Rectangle?(value), new Color(248, 66, 5, 0), Projectile.rotation, texture.Size()/2, Projectile.scale * r * 2, sprite, 0f);
            Main.spriteBatch.Draw(texture,Projectile.Center- Main.screenPosition, new Rectangle?(value), new Color(248, 66, 5, 0), Projectile.rotation, texture.Size()/2, Projectile.scale * r * 2, sprite, 0f);
            Main.spriteBatch.Draw(texture,Projectile.Center- Main.screenPosition, new Rectangle?(value), lightColor, Projectile.rotation, texture.Size()/2, Projectile.scale, sprite, 0f);
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 20;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            Projectile.localAI[0] = (Projectile.velocity).ToRotation();
            PlaySound(SoundID.Dig, Projectile.Center);
            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = new Color(238, 69, 72, 0) * 0.3F;
            Main.projectile[A].localAI[0] = Projectile.DProj().Times[2];
            Main.projectile[A].localAI[1] = Projectile.DProj().Times[2];
            Main.projectile[A].scale = Projectile.DProj().Times[2] / 4;
        }
    }
    public class 流星飞盘2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 4;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.coldDamage = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override void AI()
        {
            Player P = Main.player[Projectile.owner];
            Projectile.ai[1]++;
            //回来
            if (Projectile.ai[1] > 60)
            {
                if (Projectile.localAI[0] > 0)
                {
                    Projectile.localAI[0] -= 0.1F;
                }
                Projectile.tileCollide = false;
                Vector2 vector4 = Vector2.Subtract(P.Center, Projectile.Center);
                Vector2 perturbedSpeed = Utils.RotatedBy(vector4.PerfectNormalize(), Projectile.localAI[0], default);

                Projectile.velocity = (Projectile.velocity * 10 + perturbedSpeed * 10) / 11;
                
                Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                Rectangle value2 = new Rectangle((int)P.position.X, (int)P.position.Y, P.width, P.height);
                if (rectangle.Intersects(value2))
                {
                    Projectile.Kill();
                }
            }
            Projectile.rotation += 0.15f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Rectangle value = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 vector = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
            if (Projectile.ai[0] == 2)
            {
                lightColor *= 0.4F;
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(value), new Color(238,69,72,0), Projectile.rotation, vector, Projectile.scale, 0, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(value), new Color(238,69,72,0), Projectile.rotation, vector, Projectile.scale, 0, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(value), new Color(238,69,72,0), Projectile.rotation, vector, Projectile.scale, 0, 0f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                Color color = new Color(238, 69, 72, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i]+vector - Main.screenPosition, new Rectangle?(value), color, Projectile.rotation, vector, Projectile.scale, 0, 0f);
            }
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            PlaySound(SoundID.Dig, Projectile.Center);
            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] == 0)
                //Projectile.ai[1] += 60;

            if (Projectile.ai[0] == 2)
            {

                }
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = new Color(238, 69, 72, 0) * 0.3F;
            Main.projectile[A].localAI[0] = 1F;
            Main.projectile[A].scale = 0.5f;
        }
    }
}