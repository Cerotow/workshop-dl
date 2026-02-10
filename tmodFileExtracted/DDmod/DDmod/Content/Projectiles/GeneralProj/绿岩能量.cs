
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 绿岩能量 : ModProjectile
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
            Projectile.timeLeft = 8000;
            Projectile.extraUpdates = 50;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.scale = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.alpha = 255;
        }

        public override void SetStaticDefaults()
        {
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.ai[2] == 0)
            {
                NPC npc = Main.npc[(int)Projectile.ai[0]];
                if (npc.active)
                {
                    Projectile.timeLeft = 2;
                }
                Vector2 vector = npc.Center - Projectile.Center;
                Projectile.velocity = vector.PerfectNormalize() * 4;
                Projectile.rotation = Projectile.velocity.ToRotation();
                int R = NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<速度粒子>(), Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 100, new Color(119, 237, 130, 0), 2);
                Main.dust[R].velocity = Projectile.velocity / 2;
                Main.dust[R].rotation = Projectile.rotation;
            }
            else
            if (Projectile.ai[2] == 1)
            {
                int R = NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<速度粒子>(), Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 100, new Color(119, 237, 130, 0), 2);
                Main.dust[R].velocity = Projectile.velocity / 2;
                Main.dust[R].rotation = Projectile.rotation;
                if (Projectile.timeLeft > 80)
                {
                    Projectile.timeLeft = 80;
                }
            }
            else
            if (Projectile.ai[2] == 2)
            {
                Projectile.extraUpdates = 5;
                Projectile.ProjScale();
                Projectile.scale = 2.5F;
                if (Projectile.alpha < 60)
                {
                    int R = NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(119, 237, 130, 150), Projectile.scale);
                    Main.dust[R].velocity = Vector2.Zero;
                    Main.dust[R].customData = 6;
                }
                if (Projectile.timeLeft > 300)
                {
                    Projectile.timeLeft = 300;
                }
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 10;
                }
            }
            else
            if (Projectile.ai[2] == 3)
            {
                Projectile.extraUpdates = 3;
                Projectile.ProjScale();
                Projectile.scale = 1F; Projectile.alpha = 0;
                int R = NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(119, 237, 130, 150), Projectile.scale*0.75f);
                Main.dust[R].velocity = Vector2.Zero;
                Main.dust[R].customData = 3;

                if (Projectile.timeLeft > 300)
                {
                    Projectile.timeLeft = 300;
                }
                Projectile.Track(400, 20, 2,100);
            }
            else
            if (Projectile.ai[2] == 4)
            {
                Projectile.tileCollide =false;
                Projectile.penetrate = -1;
                if (Projectile.timeLeft > 70)
                {
                    Projectile.timeLeft = 70;
                }
                if (Projectile.timeLeft % 2 == 0)
                {
                    Lighting.AddLight(Projectile.Center, new Color(119, 237, 130).ToVector3() * (1F+Projectile.timeLeft / 140F));
                }
                Projectile.damage = (int)(Projectile.Player().GetWeaponDamage(Projectile.Player().ActiveItem())* (Projectile.timeLeft/70F));
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            if(Projectile.ai[2] >= 1)
            {
                return base.CanHitNPC(target);
            }
            if (target.whoAmI == Projectile.ai[0])
            {
                return base.CanHitNPC(target);
            }
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.ai[2] == 4)
            {
                NewDustChange2(30, Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 2 * (Projectile.timeLeft / 70F), 4 * (Projectile.timeLeft / 70F), true, 1 * (Projectile.timeLeft / 70F), 3 * (Projectile.timeLeft / 70F), 100, new Color(119, 237, 130, 0));
            }
            if (Projectile.ai[2] < 2)
            {
                NewDustChange2(30, Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 1, 2, true, 1, 3, 100, new Color(119, 237, 130, 0));
                if (Projectile.ai[1] < 3)
                {
                    List<NPC> n = [target];
                    if (Projectile.DProj().NPCW == null)
                    {
                        Projectile.DProj().NPCW = new List<byte>();
                    }
                    for (int R = 0; R < Projectile.DProj().NPCW.Count; R++)
                    {
                        n.Add(Main.npc[Projectile.DProj().NPCW[R]]);
                    }
                    Projectile.ai[1]++;
                    NPC npc = NPCdirection.FindClosest2(target.Center, 300, false, n);
                    if (npc != null)
                    {
                        Projectile.DProj().NPCW.Add((byte)target.whoAmI);
                        int r = NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<绿岩能量>(), hit.SourceDamage, 0, -1, npc.whoAmI, Projectile.ai[1]);
                        Main.projectile[r].DamageType = Projectile.DamageType;
                        Main.projectile[r].DProj().NPCW = Projectile.DProj().NPCW;
                    }
                }
            }
            Projectile.netUpdate = true;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[2] == 2)
            {
                for (int a = 0; a < 3; a++)
                {
                    int r = NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, -Projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))/2, ModContent.ProjectileType<绿岩能量>(), (int)(Projectile.damage*0.4F), 0, -1, -1, 0, 3);
                    Main.projectile[r].DamageType = Projectile.DamageType;
                }
                NewDustChange4(40, Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 6, 8, true, 2, 4, 100, 1000, new Color(119, 237, 130, 0), 2);
            }
            if (Projectile.ai[2] == 3)
            {
                NewDustChange4(20, Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 4, 6, true, 1, 3, 100,1000, new Color(119, 237, 130, 0),2);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {

            if (Projectile.ai[2] == 2|| Projectile.ai[2]==3)
                DDTextures.MiniVoidStar.Value.DrawCentre(Projectile, null, new Color(119, 237, 130, 0),Projectile.scale/3);

                return base.PreDraw(ref lightColor);
        }
    }
}
