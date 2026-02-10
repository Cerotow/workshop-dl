using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class 发光蘑菇崽 : Summons
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefault()
        {
            Projectile.width = 24;
            Projectile.height = 28;
            Projectile.minionSlots = 1;
            inertia = 10f;
            SearchRange = 1000;
            IgnoreTile = false;
            Minibuff = ModContent.BuffType<发光蘑菇崽Buff>();
            Speed = 16;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.extraUpdates = 0;
            Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 0.5F;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Summon/Minions/发光蘑菇崽_Glow");
        }
        public override void Visual()
        {

            if (!target)
                Projectile.rotation = Projectile.velocity.X * 0.03F;

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 11)
            {
                Projectile.frame = 0;
            }

            if (Projectile.DProj().Times[0] <= 0 && target)
            {
                Projectile.frame = 1;
            }
            Lighting.AddLight(Projectile.Center, new Vector3(0, 0.8F, 2) * (1 - Projectile.DProj().Times[0] / 60));
        }
        public override bool MobileAI()
        {
            if (Projectile.DProj().Times[0] > 0)
            {
                Projectile.DProj().Times[0]-=2;
            }
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                if (Projectile.DProj().Times[0] <= 0)
                {
                    Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * Speed) / 21;

                    if (vector.X > 0)
                        Projectile.RotationSpeed(Projectile.velocity.ToRotation(), 0.1F);
                    else

                        Projectile.RotationSpeed(Projectile.velocity.ToRotation()+MathHelper.Pi, 0.1F);
                }
                else
                {

                    if (Projectile.DProj().Times[0] <= 20)
                    {
                        Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * Speed/8) / 21;
                        if (vector.X > 0)
                            Projectile.RotationSpeed(Projectile.velocity.ToRotation(), 0.02F);
                        else
                            Projectile.RotationSpeed(Projectile.velocity.ToRotation() + MathHelper.Pi, 0.02F);
                    }
                    else if (vector.Length() < 160)
                    {
                        Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * -Speed) / 21;
                        Projectile.RotationSpeed(Projectile.velocity.X * 0.03F, 0.1F);
                    }
                    else if (vector.Length() > 200)
                    {
                        Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * Speed) / 21;
                        Projectile.RotationSpeed(Projectile.velocity.X * 0.03F, 0.1F);
                    }
                    else
                    {
                        Projectile.velocity = Projectile.velocity.PerfectNormalize()/16;
                        Projectile.RotationSpeed(Projectile.velocity.X * 0.03F, 0.1F);
                    }
                }
                /*
                if ((npc.position-npc.oldPosition).Length() > 0&& (npc.position - npc.oldPosition).Length()<2000)
                {
                    Projectile.position += (npc.position-npc.oldPosition) / (Projectile.extraUpdates + 1);
                }*/
                Projectile.netUpdate = true;
                return false;
            }
            return true;
        }
        public override bool AttackAI()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            AttackFrame = 60;
            Projectile.DProj().Times[0] = 60;
            Projectile.velocity = -Projectile.velocity*0.8F;
            NewDustChange4(42,Projectile.Center+(target.Center-Projectile.Center).PerfectNormalize()*12,Vector2.Zero,ModContent.DustType<光球粒子>(),1,6,color: new Color(0, 120, 255, 0),Data:3);
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Glow.Value;
            Vector2 Center;
            SpriteEffects sprite = 0;
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                if (vector.X < 0)
                {
                    sprite = SpriteEffects.FlipHorizontally;
                }
            }
            else
            {
                if (Projectile.velocity.X < 0)
                {
                    sprite = SpriteEffects.FlipHorizontally;
                }
            }
            Rectangle rectangle = new Rectangle(0, texture.Height / 11 * Projectile.frame, texture.Width, texture.Height / 11);
            Color color = lightColor;
            for (int a = 0; a < Projectile.oldPos.Length; a++)
            {
                color = Lighting.GetColor((int)(Projectile.Center.X/16), (int)(Projectile.Center.Y/16), new Color(0,120,255,0)) * (1 - (float)a / Projectile.oldPos.Length)*0.5F;
                color.A = 0;
                color = new Color(0, 120, 255, 0) *(1 - (float)a / Projectile.oldPos.Length) * 0.5F*(1- Projectile.DProj().Times[0]/60);
                Center = Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition;
                Main.EntitySpriteDraw(texture, Center, rectangle, color, Projectile.rotation, rectangle.Size() / 2f, Projectile.scale*1, sprite, 0f);
            }
            Center = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(texture, Center, rectangle, new Color(0, 120, 255, 0) * (1 - Projectile.DProj().Times[0] / 60), Projectile.rotation, rectangle.Size() / 2f, Projectile.scale * 1, sprite, 0f);
            texture = TextureAssets.Projectile[Projectile.type].Value;
            rectangle = new Rectangle(0, texture.Height / 11 * Projectile.frame, texture.Width, texture.Height / 11);
            Center = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(texture, Center, rectangle, lightColor, Projectile.rotation, rectangle.Size() / 2f, Projectile.scale, sprite, 0f);


            return false;
        }
        internal Trailing TrailDrawer;
    }
}