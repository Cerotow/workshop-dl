using DDmod.Content.Dusts;
using DDmod.Content.NPCs.Boss.天雷怒云;
using DDmod.Content.NPCs.Boss.恐惧缝合体;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Content.Particles;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Boss
{
    public class 引雷针Proj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 50;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1;
            Projectile.timeLeft = 400;
        }
        int A;
        public override void AI()
        {
            Projectile.damage = 0;
            Projectile.rotation = 0;

            if (Projectile.velocity.Y < 10)
            {
                Projectile.velocity.Y += 0.15F;
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 120)
            {
                Main.LocalPlayer.Dplayer().Bossperspective(Projectile.Center, 120, false, 0.2F);
                for (int W = 0; W < 3; W++)
                {
                    int A = NewDust(Projectile.Center - new Vector2(0, 21) + new Vector2(Main.rand.Next(40, 60), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(74, 189, 226, 0), Scale: 1F);
                    Main.dust[A].velocity = (Projectile.Center - new Vector2(0, 21) - Main.dust[A].position) / 20;
                    Main.dust[A].noGravity = true;
                    Main.dust[A].customData = -2 - Main.dust[A].DustAI(3);
                }
            }
            else if (Projectile.Player().ownedProjectileCounts[ModContent.ProjectileType<Boss召唤闪电>()] == 0)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center - new Vector2(0, 1060), new Vector2(0, 4), ModContent.ProjectileType<Boss召唤闪电>(), 30, 0, -1, 0, 2, 400);
                }
                if (Projectile.ai[0] > 140)
                {
                    Projectile.ai[0] = 0;
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if(Projectile.velocity.X!=oldVelocity.X)
            {
                Projectile.velocity.X = 0; 
            }
            if(Projectile.velocity.Y!=oldVelocity.Y)
            {
                Projectile.velocity.Y =0; 
            }
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            Main.spriteBatch.Draw(texture, v, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            return false;
        }
    }
    public class Boss召唤闪电 : 闪电
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
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = Length;
            Projectile.extraUpdates = Length / 20;

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
                if (!AnyNPCs(ModContent.NPCType<天雷怒云>()) && Projectile.timeLeft >= 10000)
                {
                    if (Main.netMode != 1)
                        DNPC.NewNPCs(Projectile.GetSource_FromAI(), Projectile.Center-new Vector2(0,1000), ModContent.NPCType<天雷怒云>(), 0);
                }
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
                Projectile projectile = null;
                for (int A = 0; A < 1000; A++)
                {
                    projectile = Main.projectile[A];
                    if (projectile.active && projectile.type == ModContent.ProjectileType<引雷针Proj>())
                    {
                        break;
                    }
                }
                if ((projectile.Center - new Vector2(0, 20) - Projectile.Center).Length() < 16)
                {
                    int Type = ModContent.DustType<光球粒子>();
                    for (int W = 0; W < 30; W++)
                    {
                        Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(2, 254, 201, 0))];
                        dust.noGravity = true;
                        dust.scale = Main.rand.NextFloat(1F, 2.2F);
                        dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3, 6f);
                        dust.rotation = Projectile.rotation;
                        dust.customData = -3;
                    }
                    projectile.Kill();
                    if (!AnyNPCs(ModContent.NPCType<天雷怒云>()))
                    {
                        if (Main.netMode != 1)
                            DNPC.NewNPCs(Projectile.GetSource_FromAI(), Projectile.Center - new Vector2(0, 1000), ModContent.NPCType<天雷怒云>(), 0);
                    }
                    Projectile.timeLeft = 100000;
                }
                if (Projectile.DProj().Times[0] > 20 && Main.rand.NextBool(10) && Projectile.DProj().track > 3)
                {
                    Projectile.DProj().Times[0] = 0;
                    if (projectile != null)
                    {
                        float A = Vector2.Subtract(projectile.Center - new Vector2(0, 20), Projectile.Center).Length() / 300;
                        if (A > 0.6F)
                        {
                            A = 0.6F;
                        }
                        Vector2 vector1 = Utils.RotatedBy(Vector2.Subtract(projectile.Center - new Vector2(0, 20), Projectile.Center).PerfectNormalize() * Projectile.velocity.Length(), Main.rand.NextFloat(-A, A), default);
                        Projectile.velocity = vector1;
                    }
                    else
                    {
                        Vector2 vector1 = Utils.RotatedBy(Projectile.DProj().vector[0].PerfectNormalize() * Projectile.velocity.Length(), Main.rand.NextFloat(-0.6F, 0.6F), default);
                        Projectile.velocity = vector1;
                    }
                    Projectile.netUpdate = true;
                }
            }
        }
    }
}