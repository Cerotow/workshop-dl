using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Boss
{
    public class Boss鬼牙 : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
            Projectile.width = 66;
            Projectile.height = 66;
            Projectile.scale = 1.3f;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.timeLeft =1000;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
        }
        int[] Body = new int[5];
        Vector2[] Center = new Vector2[5];
        float[] Rotation = new float[5];
        float Time;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = Projectile.GetAlpha(new Color(255,0,0));
            Color color2 = Projectile.GetAlpha(new Color(255, 0, 0));
            if (Projectile.ai[0] == -4)
            {
                color *= 0.2F;
                color.A = 255;
                color2 *= 0.2F;
                color2.A = 255;
            }
            for (int B = Body.Length - 1; B >= 0; B--)
            {
                if (B == 0)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 3)), color2, Rotation[0]+ MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else if (B < Body.Length - 1)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color2, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * 2, texture.Width, texture.Height / 3)), color2, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
            }
            for (int B = Body.Length - 1; B >= 0; B--)
            {
                if (B == 0)
                { Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 3)), color, Rotation[0]+ MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else if (B < Body.Length - 1)
                { Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else
                {Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * 2, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
            }
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return null;
        }
        public void Visual()
        {
            Center[0] = Projectile.Center += Projectile.velocity;
            Rotation[0] = Projectile.rotation;
            for (int B = 0; B < Body.Length; B++)
            {
                if (B > 0)
                {
                    Vector2 vector = Center[B - 1] - Center[B];
                    Rotation[B] = vector.ToRotation() + MathHelper.PiOver2;
                    Center[B] = Center[B - 1] - vector.PerfectNormalize() * (42 * Projectile.scale);
                }
            }
            int Length = 7;
            if (Projectile.ai[0] == -2)
            {
                Length = 13;
            }
            if (Projectile.ai[0] == -3)
            {
                Length = 16;
            }
            if (Projectile.ai[0] == -4)
            {
                Length = 36;
            }
            if (Body.Length != Length)
            {
                Body = new int[Length];
                Center = new Vector2[Length];
                Rotation = new float[Length];
                for (int B = 0; B < Length; B++)
                {
                    Center[B] = Projectile.Center - new Vector2(0.1f);
                }
            }
        }

        public override void AI()
        {
            Player player = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
            if (!NPC.AnyNPCs(ModContent.NPCType<鬼牙头>())||!Main.npc[(int)Projectile.ai[1]].active|| Main.npc[(int)Projectile.ai[1]].type!= ModContent.NPCType<鬼牙头>())
            {
                Projectile.Kill();
            }
            if (Projectile.ai[0] >= 0)
            {
                Projectile.timeLeft = 5;
                if (Main.npc[(int)Projectile.ai[1]].Dnpc().Stage != 1)
                {
                    Projectile.Kill();
                }
                Projectile.scale = Projectile.ai[0];
                float Speed = 10 * Projectile.ai[0];
                //移动代码
                if (Projectile.ai[2] < Speed)
                {
                    Projectile.ai[2] += Speed / 60;
                }
                else
                {
                    Projectile.ai[2] = Speed;
                }
                if (Projectile.DProj().Times[0] <= 0)
                {
                    DDHelper.RotateSpeed(ref Projectile.rotation, (player.Center - Projectile.Center).ToRotation(), Projectile.DProj().Times[1]);
                }
                if ((Projectile.Center - player.Center).Length() < 200)
                {
                    Projectile.DProj().Times[0] = 5 * Projectile.ai[0];
                    Projectile.DProj().Times[1] = 0;
                }
                if (Projectile.DProj().Times[0] > 0)
                {
                    Projectile.DProj().Times[0]--;
                }
                else
                {
                    if (Projectile.DProj().Times[1] < 0.1F * Projectile.ai[0])
                    {
                        Projectile.DProj().Times[1] += 0.001f * Projectile.ai[0];
                    }
                }
                if(Projectile.alpha>0)
                {
                    Projectile.alpha -= 5;
                }
                Projectile.ProjScaleChange();
                Projectile.velocity = Projectile.rotation.ToRotationVector2().PerfectNormalize() * Projectile.ai[2];
            }
            if (Projectile.ai[0]==-1)
            {
                if(!Projectile.DProj().Bool[0])
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    Projectile.DProj().Bool[0] = true;
                }
                Projectile.scale = 0.6F;
                float Speed = 14;
                //移动代码
                if (Projectile.ai[2] < Speed)
                {
                    Projectile.ai[2] += Speed / 60;
                }
                else
                {
                    Projectile.ai[2] = Speed;
                }
                Projectile.DProj().Times[0]++;
                if((player.Center - Projectile.Center).Length()<200)
                {
                    Projectile.DProj().Times[0] = 200;
                }
                if (Projectile.DProj().Times[0] <= 100)
                {
                    DDHelper.RotateSpeed(ref Projectile.rotation, (player.Center - Projectile.Center).ToRotation(), 0.2F);
                }
                Projectile.ProjScaleChange();
                Projectile.velocity = Projectile.rotation.ToRotationVector2().PerfectNormalize() * Projectile.ai[2];
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 5;
                }
            }
            if (Projectile.ai[0]==-2)
            {
                if(!Projectile.DProj().Bool[0])
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    Projectile.DProj().Bool[0] = true;
                }
                Projectile.scale = 1F;
                float Speed = 10;
                //移动代码
                if (Projectile.ai[2] < Speed)
                {
                    Projectile.ai[2] += Speed / 60;
                }
                else
                {
                    Projectile.ai[2] = Speed;
                }
                Projectile.DProj().Times[0]++;
                if((player.Center - Projectile.Center).Length()<200)
                {
                    Projectile.DProj().Times[0] = 300;
                }
                if (Projectile.DProj().Times[0] <= 200)
                {
                    DDHelper.RotateSpeed(ref Projectile.rotation, (player.Center - Projectile.Center).ToRotation(), 0.2F);
                }
                Projectile.ProjScaleChange();
                Projectile.velocity = Projectile.rotation.ToRotationVector2().PerfectNormalize() * Projectile.ai[2];
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 5;
                }
            }
            if (Projectile.ai[0]==-3)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.ProjScaleChange();
                Projectile.velocity *= 1.03F;
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 5;
                }
            }
            if (Projectile.ai[0]==-4)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.ProjScaleChange();
                Projectile.extraUpdates = 10;
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 5;
                }
            }
            if (Projectile.ai[0]==-5)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.ProjScaleChange();
                Projectile.extraUpdates = 10;
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 5;
                }
            }
            Visual();
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {

            if (Projectile.ai[0] == -4)
            {
                return new bool?(false);
            }
            Rectangle[] vectors = new Rectangle[Body.Length];
            bool B = false;
            for (int A = 0; A < Body.Length; A++)
            {
                Vector2 Size = Projectile.Size / 2;
                vectors[A] = new Rectangle((int)(Center[A].X - Size.X), (int)(Center[A].Y - Size.Y), Projectile.width, Projectile.height);
                if (vectors[A].Intersects(targetHitbox))
                {
                    B = true;
                }
            }
            return new bool?(B);
        }
    }
}