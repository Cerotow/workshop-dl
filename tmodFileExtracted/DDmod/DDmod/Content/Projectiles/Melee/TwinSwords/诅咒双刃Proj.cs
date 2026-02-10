
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.FlyingKnife.Make;
using DDmod.Content.Items.Melee.Sword.Make;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Players;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.TwinSwords
{
    public class 诅咒双刃Proj : 双刀
    {
        public override void Defaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 40;
            EffectLength = 10;
            HandheldOffset = 26;
            BladeGlow = true;
        }
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
        }
        public static Asset<Texture2D> Glow;
        public override Texture2D TextureGlow => Glow.Value;
        public override Color color => new Color(81, 6, 233, 100);
        public override string GlowTexture => base.GlowTexture;
        public bool X;
        public Vector2 Vector = Vector2.Zero;
        public override bool PreAI()
        {
            if (Projectile.ai[2] == 2)
            {
                range = MathHelper.TwoPi + 2.5F;
            }
            if(Projectile.ai[2]==3)
            {
                range = MathHelper.TwoPi + 2.5F;
                if (npc!=255)
                {
                    range = MathHelper.TwoPi * 10 + 2.5F;
                }
            }
            Player player = Main.player[Projectile.owner];
            if (Projectile.DProj().track == 40 && Projectile.DProj().Bool[0] && Projectile.DProj().Back == 1)
            {
                player.velocity = Vector2.Zero;
            }
            Projectile.DProj().Times[1]++;

                if (Projectile.ai[2] == 3)
            {
                if (Projectile.DProj().Times[1] < 180 && Projectile.DProj().Bool[0])
                {
                    Projectile.ai[0] = 0;
                }
                else
                {
                    Projectile.DProj().Times[1] = 180;
                    if (!Projectile.DProj().Bool[2])
                    {

                        player.velocity = -Projectile.DProj().vector[2].PerfectNormalize() * 4;
                        player.velocity.Y -= 2;
                        Projectile.DProj().Bool[1] = false;
                        Projectile.DProj().Bool[2] = true;
                    }
                }
                if (Projectile.DProj().Back == 1)
                {
                    Main.projectile[Projectile.DProj().Other].DProj().Times[1] = Projectile.DProj().Times[1];
                }
            }
            if (Projectile.DProj().Bool[1] && Projectile.DProj().Back == 1)
            {
                player.fullRotation = 0;
                if (Math.Abs(Projectile.ai[0]) < range - 20.5F&& npc<=200)
                {
                    NPC npc = Main.npc[this.npc];
                    if(!npc.CanBeChasedBy())
                    {
                        this.npc = 254;
                    }
                    player.dashDelay = 5;
                    player.velocity.Y = -0.001f;
                    player.velocity.X = -0.001f;
                    float SP = 600;
                    Vector = (Vector * SP + (npc.Center - player.Center).PerfectNormalize().RotatedBy(0.1F * player.direction) * 12) / (SP + 1);
                    player.position += Vector + (npc.oldPosition==Vector2.Zero ?Vector2.Zero:(npc.position - npc.oldPosition) / (Projectile.extraUpdates + 1));
                    player.Aplayer().NoGravity = 2;
                    DDPlayer.移动玩家(player, 10, true);
                    if(!X&& Collision.SolidCollision(new Vector2(player.position.X, (player.position.Y)), player.width, player.height,false))
                    {
                        X = true;
                        Projectile.DProj().vector[1] = player.oldPosition;
                    }
                    if (X)
                    {
                        NewDustSector2(3, Projectile.DProj().vector[1], player.Size, new Vector3(-player.velocity.PerfectNormalize(), MathHelper.Pi), 27, 0, 10, true, 1, 2);
                    }
                    NewDustSector2(2, player.position, player.Size, new Vector3(-player.velocity.PerfectNormalize(), MathHelper.Pi), ModContent.DustType<速度粒子>(), 0, 10, true, 1, 2, 100, 1000, color, null);
                }
                else
                {
                    Time++;
                    if (Time > 16)
                    {
                        if (Projectile.owner == Main.myPlayer)
                        {
                            SpecialShoot();
                        }
                        Time = 1;
                        //弹反
                        player.velocity.X = 16*player.direction;
                        player.velocity.Y = -6;
                        NewDustSector2(30, player.position, player.Size, new Vector3(-player.velocity.PerfectNormalize(), 0.6F), ModContent.DustType<速度粒子>(), 0, 30, true, 2, 5, 100, 1000, color, null);
                        Projectile.DProj().Bool[1] = false;
                        Projectile.DProj().Bool[3] = true;
                        if (X)
                        {
                            NewDustSector2(50, Projectile.DProj().vector[1], player.Size, new Vector3(-player.velocity.PerfectNormalize(), MathHelper.Pi), 27, 0, 10, true, 1, 2);
                            player.position = Projectile.DProj().vector[1];
                        }
                        DDPlayer.移动玩家(player, 10, true);
                    }
                    else
                    {
                        DDPlayer.移动玩家(player, 10, false);
                    }
                }
            }
            if (Projectile.DProj().Bool[0] && Projectile.DProj().Times[1] > 30)
            {
                if (Projectile.DProj().Back == 1)
                {
                    if (!player.HasBuff(ModContent.BuffType<SpecialAttackCD>()))
                    {
                        player.AddBuff(ModContent.BuffType<SpecialAttackCD>(), 120);
                    }
                    if (Projectile.owner == Main.myPlayer && Projectile.DProj().vector[2] == Vector2.Zero)
                    {
                        Projectile.DProj().MouseWorld = Main.MouseWorld;
                        Projectile.DProj().vector[2] = Projectile.DProj().MouseWorld - player.Center;
                        Projectile.netUpdate = true;
                    }
                    if (Math.Abs(Projectile.ai[0]) == 0)
                    {
                        player.dashDelay = 5;
                        player.velocity = Projectile.DProj().vector[2].PerfectNormalize() * 26 / (Projectile.extraUpdates + 1);
                        player.position += Collision.TileCollision(player.position, Projectile.DProj().vector[2].PerfectNormalize() * 26 / (Projectile.extraUpdates + 1), player.width, player.height, true, true);
                        player.Aplayer().NoGravity = 2;
                        DDPlayer.移动玩家(player, 30, true);
                    }
                }
            }

            if (Projectile.DProj().Back == -1)
            {
                ExtraLength.Y = -14;
                EffectLength = 4;
                Projectile.MeleeProj().oldVels3 = 0F;
                Projectile.MeleeProj().oldVels2 = 38;
            }
            else
            {
                ExtraLength.Y = -4;
                Projectile.MeleeProj().oldVels3 = 0.15F;
                
            }
            if(Math.Abs(Projectile.ai[0])< range)
            {
                if (Projectile.localAI[2]<1)
                    Projectile.localAI[2] += 0.1F;
            }
            else
            {
                Projectile.localAI[2] -= 0.01F;
            }
            return true;
        }
        public override void SpecialShoot()
        {
        }
        public override void Shoot()
        {
            if (Projectile.ai[2] == 2)
            {
                if (Projectile.DProj().Back == 1)
                {
                    int a = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0] * 12, ModContent.ProjectileType<暗影气刃>(), Projectile.damage, Projectile.knockBack , Projectile.owner);
                    Main.projectile[a].DProj().color = color;
                    Main.projectile[a].DProj().Magnification = Projectile.DProj().Magnification;
                }
            }
        }
        public override void Draw(Texture2D texture, Vector2 Center, Color lightColor)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = color;

            if (Projectile.DProj().Back == -1)
                Projectile.DProj().Bool[1] = true;

            if (Projectile.DProj().Bool[0])
            {
                if (npc == 255)
                {
                    if (Projectile.DProj().Back == 1)
                    {
                        int a = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0] * 20, ModContent.ProjectileType<暗影气刃>(), Projectile.damage, 0.2f, Projectile.owner, 0, 0, 1);
                        Main.projectile[a].DProj().color = color;
                        Main.projectile[a].DProj().Magnification = Projectile.DProj().Magnification;
                    }
                    npc = (byte)target.whoAmI;
                }
                player.AddBuff(ModContent.BuffType<SpecialAttackCD>(), 600);
                Projectile.DProj().Bool[1] = true;
                Projectile.DProj().Bool[0] = false;
            }
            short Jl = (short)(20 - (player.Center - target.Center).Length() / 10);
            if (Jl < 0)
            {
                Jl = 0;
            }
            Time += Jl;
            Projectile.netUpdate = true;
        }


        public override void OnKill(int timeLeft)
        {
        }
    }
}