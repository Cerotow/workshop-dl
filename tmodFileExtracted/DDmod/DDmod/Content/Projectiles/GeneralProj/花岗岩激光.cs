
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 花岗岩激光 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 0;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override void SetStaticDefaults()
        {
        }
        public override void AI()
        {
            if(Projectile.timeLeft>10)
            {
                if (Projectile.DProj().Times[1]<3)
                {
                    Projectile.DProj().Times[1]+=0.2F;
                }
                else
                {
                    Projectile.DProj().Times[1] = 3;
                }
            }
            else
            {
                Projectile.DProj().Times[1]-=0.3f;
            }
            Projectile.scale = Projectile.DProj().Times[1] + Projectile.DProj().Times[0];
            DDHelper.BackAndForth(0, 0.5F, 0.1F, ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0]);
            Projectile.ProjScaleChange();
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            /*
            int R =NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<速度粒子>(), Projectile.velocity.X/2, Projectile.velocity.Y/2, 100, new Color(50, 155, 255, 0), 2);
            Main.dust[R].velocity = Projectile.velocity/2;
            Main.dust[R].rotation = Projectile.rotation;*/
            Projectile.ai[0] = 0;
            int A = 150*16;
            for (int a = 0; a < A/Projectile.height; a++)
            {
                Projectile.ai[0]++;
                bool TileCollision = Collision.SolidCollision(Projectile.position + Projectile.velocity.PerfectNormalize() * (Projectile.height * Projectile.ai[0]), Projectile.width, Projectile.height);
                if (TileCollision)
                {
                    Projectile.ai[1] = Projectile.height;
                    for (int R = Projectile.height; R > 0; R--)
                    {
                        TileCollision = Collision.SolidCollision(Projectile.position + Projectile.velocity.PerfectNormalize() * (Projectile.height * Projectile.ai[0] - R), Projectile.width, Projectile.height); if (TileCollision)
                        {
                            Projectile.ai[1] = R;
                            break;
                        }
                    }
                    break;
                }
            }
            Projectile.velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(0.02F));
        }
        int L;
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
         //   NewDustChange2(30,Projectile.Center-new Vector2(4),Vector2.Zero,ModContent.DustType<速度粒子>(),1,2,true,1,3,100,new Color(50,155,255,0));
            Projectile.netUpdate = true;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            bool B = false;
            for (int A = 0; A <= Projectile.ai[0]; A++)
            {
                Vector2 vector = Projectile.position + Projectile.velocity.PerfectNormalize()*(Projectile.height*A);
                if (A == Projectile.ai[0])
                {
                    vector -= Projectile.velocity.PerfectNormalize() * Projectile.ai[1];
                }
                projHitbox.X = (int)vector.X;
                projHitbox.Y = (int)vector.Y;
                if (projHitbox.Intersects(targetHitbox))
                {
                    B = true;
                    break;
                }
            }
            return new bool?(B);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DDTextures.GlowEffect.Value;
            texture = DDTextures.VoidStar.Value;
            for (int A = 0; A <= Projectile.ai[0]; A++)
            {
                Vector2 Size = Projectile.Size / 2;
                Vector2 vector = Projectile.position + Projectile.velocity.PerfectNormalize() * (Projectile.height * A);
                Vector2 Scale = new Vector2(1, 3);
                if (A == 0)
                {
                    Scale = new Vector2(1.3F);
                }
                if (A == Projectile.ai[0])
                {
                    vector -= Projectile.velocity.PerfectNormalize() * Projectile.ai[1];
                    Scale = new Vector2(1);
                }
                Main.spriteBatch.Draw(texture , vector+Size - Main.screenPosition, null, new Color(0, 0, 0,255), Projectile.rotation, texture.Size()/2, Projectile.scale/6* Scale, 0, 0);
                Main.spriteBatch.Draw(texture , vector+Size - Main.screenPosition, null, new Color(50,155,255,0)*0.75f, Projectile.rotation , texture.Size()/2, Projectile.scale/6 * Scale, 0, 0);
                Main.spriteBatch.Draw(texture , vector+Size - Main.screenPosition, null, new Color(50,155,255,0) * 0.75f, Projectile.rotation , texture.Size()/2, Projectile.scale/6 * Scale, 0, 0);
                //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, vector - Main.screenPosition, null, Color.White * 0.8F, 0, Vector2.Zero, new Vector2(Projectile.width, Projectile.height) / 2, 0, 0);

            }
            texture = DDTextures.GlowEffect.Value;
            for (int A = 0; A < Projectile.ai[0]; A++)
            {
                Vector2 Size = Projectile.Size / 2;
                Vector2 vector = Projectile.position + Projectile.velocity.PerfectNormalize() * (Projectile.height * A + Projectile.ai[2] % Projectile.height);
                Vector2 Scale = new Vector2(1, 1);
                if (A == 0)
                {
                    Scale = new Vector2(1.3F);
                    vector = Projectile.position + Projectile.velocity.PerfectNormalize() * (Projectile.height * A);
                    Main.spriteBatch.Draw(texture, vector + Size - Main.screenPosition, null, new Color(0, 155, 255, 0) * 0.5f, Projectile.rotation + Projectile.ai[2] / 10, texture.Size() / 2, Projectile.scale / 10 * Scale, 0, 0);
                    Main.spriteBatch.Draw(texture, vector + Size - Main.screenPosition, null, new Color(0, 155, 255, 0) * 0.5f, Projectile.rotation + Projectile.ai[2] / 10, texture.Size() / 2, Projectile.scale / 10 * Scale, 0, 0);

                }
                else
                {
                    if (A == Projectile.ai[0])
                    {
                        vector -= Projectile.velocity.PerfectNormalize() * Projectile.ai[1];
                        Scale = new Vector2(1);
                    }
                    Main.spriteBatch.Draw(texture, vector + Size - Main.screenPosition, null, new Color(0, 155, 255, 0) * 0.5f, Projectile.rotation, texture.Size() / 2, Projectile.scale / 10 * Scale, 0, 0);
                }
            }
            Projectile.ai[2] +=4F;
            return false;
        }
    }
}
