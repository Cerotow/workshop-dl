using DDmod.Content.Dusts;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class FlameStaff : AMagicStaff
    {
        public override int ProjShoot => ModContent.ProjectileType<BurningFireball>();
        public override float CircleValue => 0.01F;
        public override float MaxCircle => 0.2f;
        public override float Distance => 24;
        public override float ShootDistance => 36;
        public override void Set()
        {
        }
        public override bool PreAI()
        {
            CircleE *= 1.05F;
            CircleA -= 0.04F;
            return true;
        }
        public override SoundStyle Sound()
        {
            SoundStyle sound = SoundID.Item20;
            sound.Volume = 0.5f;
            return sound;
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
        }
        public override void ShootEffect(Player player)
        {
            Vector2 vector = Projectile.Center + Projectile.velocity.PerfectNormalize() * 22 - new Vector2(4);
            for (int a = 0; a < 40; a++)
            {
                int Dust = NewDust(vector + new Vector2(0, Main.rand.NextFloat(-40, 40)).RotatedBy(Projectile.rotation - StaffRot), 0, 0, 6, 0, 0, 0,Color.White, Main.rand.NextFloat(0.4F, 1.2F));
                Main.dust[Dust].velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(1, ProjShootSpeed/2);
            }
            Circle = MaxCircle;
            Circle *= 1.75F;
            CircleA = 0.8F;
            CircleE = 1;
        }
        public override void Shoot(Player player)
        {
            Vector2 vector = Projectile.velocity.PerfectNormalize();
            Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), player.Center + SolidTileDistanceDetection(player, vector * ShootDistance), vector * ProjShootSpeed, ProjShoot, Projectile.damage, Projectile.knockBack, Projectile.owner)];
            //projectile.scale = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DProj().Times[0] += 0.05f;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, 0, 0f);

            Color color = new Color(253, 62, 3, 0);
            color.A = 100;
            Vector2 vector = Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 22;
            //光球渲染
            Main.spriteBatch.Draw(VoidStar, vector, null, color * 0.4f, Projectile.rotation - StaffRot, VoidStar.Size() / 2, new Vector2(0.5f, 1.1f) * Circle, 0, 0f);

            texture = DDTextures.Circle[9].Value;
            color = new Color(55, 55, 55,255);
            //法阵
            DDHelper.Compression(texture, color, Projectile.rotation - StaffRot, Projectile.Opacity, new Vector2(5, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.AlphaBlend);

            Main.spriteBatch.Draw(texture, vector, null, color, 0f, Utils.Size(texture) / 2, Circle, 0, 0);
            //法阵
            DDHelper.Compression(texture, color, Projectile.rotation - StaffRot, Projectile.Opacity, new Vector2(5, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);
            color = new Color(253, 102, 3, 0);
            GameShaders.Misc["压缩"].UseColor(color);
            GameShaders.Misc["压缩"].Apply(default);
            Main.spriteBatch.Draw(texture, vector, null, color, 0f, Utils.Size(texture) / 2, Circle, 0, 0);
            Main.spriteBatch.Draw(texture, vector, null, color, 0f, Utils.Size(texture) / 2, Circle*1.2f, 0, 0);
            if (CircleA > 0)
            {
                GameShaders.Misc["压缩"].UseColor(color * CircleA);
                GameShaders.Misc["压缩"].Apply(default);
                Main.spriteBatch.Draw(texture, vector, null, color * CircleA,0, Utils.Size(texture) / 2,  Circle * CircleE, 0, 0);
                Main.spriteBatch.Draw(texture, vector, null, color * CircleA, 0, Utils.Size(texture) / 2,  Circle * CircleE, 0, 0);
                Main.spriteBatch.Draw(texture, vector, null, color * CircleA, 0, Utils.Size(texture) / 2, Circle * CircleE, 0, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    } 

public class BurningFireball : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override string Texture => "DDmod/Image/VoidStar";
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
        }
        public override void AI()
        {
            for (int A = 0; A < 2; A++)
            {
                int Type = 6;
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1;
                dust.velocity = Vector2.Zero;
            }
            Projectile.ProjScaleChange();
            if (Projectile.velocity.Y < 12&&Projectile.DProj().track>15)
            {
                Projectile.velocity.Y += 0.5F;
            }
            if (Main.tile[(int)Projectile.Center.X / 16, (int)(Projectile.position.Y + Projectile.height) / 16].LiquidAmount > 0)
            {
                Projectile.Kill();
            }
        }
        public override void OnKill(int timeLeft)
        {
            NewProjectile(Projectile.GetSource_FromThis(), Projectile.position + new Vector2(Projectile.width / 2, Projectile.height), Vector2.Zero, ModContent.ProjectileType<PillarOfFire>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
            for (int A = 0; A < 40; A++)
            {
                int Type = 6;
                Dust dust = Main.dust[NewDust(Projectile.Center + new Vector2(0, Projectile.height / 2), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 2;
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(-0.6f, 0.6f)) * Main.rand.NextFloat(1, 15);
            }
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = Projectile.DProj().Bool[0];
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(24, 300);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = Projectile.Size / 2;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color color = new Color(253, 62, 3, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                for(int a=0;a<5;a++)
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale/2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(253, 62, 3, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale/2, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(253, 62, 3, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale/2, spriteEffects, 0f);

            return false;
        }
    }
    public class PillarOfFire : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
        }
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.width = 30;
            Projectile.height = 2;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
        }
        public override void AI()
        {
            for (int A = 0; A < 2 + Projectile.height / 80; A++)
            {
                int Type = 6;
                Dust dust = Main.dust[NewDust(Projectile.Center + new Vector2(0, Projectile.height / 2), Projectile.width, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 2;
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(-0.6f, 0.6f)) * Main.rand.NextFloat(5, 15);
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            bool A = false;
            for (int a = 0; a < 160; a += 6)
            {
                projHitbox = new Rectangle((int)Projectile.position.X - a / 2, (int)(Projectile.position.Y - a), Projectile.width + a, 6);
                if (projHitbox.Intersects(targetHitbox))
                {
                    A = true;
                }
            }
            return new bool?(A);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = Projectile.DProj().Bool[0];
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(24, 300);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}