using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee.Spear.Proj;

namespace DDmod.Content.Projectiles.Magic
{
    public class Lightning : 闪电
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
            int Length = 375;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = Length;
            Projectile.extraUpdates = Length / 20;
            Projectile.scale = 1;
        }
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
                V2.Add(Projectile.position);
                Projectile proj = null;
                for (int A = 0; A < 1000; A++)
                {
                    proj = Main.projectile[A];
                    if (proj.active && proj.type == 254 && proj.owner == Projectile.owner)
                    {
                        break;
                    }
                }
                Projectile.DProj().Times[0]++;
                if (Projectile.DProj().Times[0] > 20 && Main.rand.NextBool(10) && Projectile.DProj().track > 3)
                {
                    Projectile.DProj().Times[0] = 0;
                    if (proj != null && proj.active && proj.type == 254)
                    {
                        float A = Vector2.Subtract(proj.Center, Projectile.Center).Length() / 300;
                        if (A > 1)
                        {
                            A = 1;
                        }
                        Vector2 vector1 = Utils.RotatedBy(Vector2.Subtract(proj.Center, Projectile.Center).PerfectNormalize() * Projectile.velocity.Length(), Main.rand.NextFloat(-A, A), default);
                        Projectile.velocity = vector1;
                    }
                }
                if (proj != null)
                {
                    if (Vector2.Subtract(proj.Center, Projectile.Center).Length() < 20)
                    {
                        proj.ai[0] = 1;
                        SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                        sound.Volume = 0.3f;
                        sound.Pitch = 0.5f;
                        PlaySound(sound, Projectile.position);
                        int Type = ModContent.DustType<光球粒子>();
                        for (int A = 0; A < 30; A++)
                        {
                            Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(2, 254, 201, 0))];
                            dust.noGravity = true;
                            dust.scale = Main.rand.NextFloat(1F, 2.2F);
                            dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3, 6f);
                            dust.rotation = Projectile.rotation;
                            dust.customData = -3;
                        }
                        Projectile.timeLeft = 2;
                    }
                }
                else
                {
                    Projectile.timeLeft =2;
                }
                Projectile.netUpdate = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            int Type = ModContent.DustType<光球粒子>();
            for (int A = 0; A < 10; A++)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(2, 254, 201, 0))];
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(1F, 2.2F);
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3, 6f);
                dust.rotation = Projectile.rotation;
                dust.customData = -1;
            }
        }
    }
}