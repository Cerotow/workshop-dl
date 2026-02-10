using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.BattlePets.Proj;
using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;
using DDmod.Content.Projectiles.Summon;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;
using DDmod.Players;
using DDmod.UI.BattlePetUI.技能;
using MonoMod.Cil;
using System.Diagnostics;
using Terraria.WorldBuilding;

namespace DDmod.Content.Projectiles.BattlePets
{
    public class 牢中灵骷 : BattlePetsProj
    {
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> Glow2;
        public static Asset<Texture2D> Glow3;
        public static Asset<Texture2D> Glow4;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
            Glow2 = ModContent.Request<Texture2D>(Texture + "_盔甲骨骑");
            Glow3 = ModContent.Request<Texture2D>(Texture + "_地狱魔首");
            Glow4 = ModContent.Request<Texture2D>(Texture + "_地狱武士");
            base.Load();
        }
        public override bool? CanDamage()
        {
            if (!Fight)
            {
                return false;
            }
            if (Variation == 3 || Variation == 6 || Variation == 7) return false;
            return null;
        }
        public override void SetDef()
        {
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
        public override void UP()
        {
            if (OldVariation != Variation)
            {
                NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<UPProj>(), 0, 0, -1, 1);
            }
            else
            {

                NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<UPProj>(), 0, 0, -1, 0);
            }
        }
        public override void Hit(Player player, Players.BattlePets battle)
        {
            if (battle.Variant == 3 && Main.rand.NextBool(20))
            {
                battle.SkillCD = 0;
            }
            if (battle.Variant == 5 && Main.rand.NextBool(12))
            {
                battle.SkillCD = 0;
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (Variation == 4 && Skill > 0)
            {
                for (int A = 0; A < 6; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(20) - (Projectile.Center - target.Center).PerfectNormalize() * Projectile.width / 2, 40, 40, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(64, 202, 251, 120), Main.rand.NextFloat(1F, 1.3F))];
                    dust.velocity = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.6F, 0.6F)) * Main.rand.NextFloat(0F, 0.5F) * (Projectile.extraUpdates + 1) / 4;
                }
                target.AddBuff(30, 300);
            }
            if (Variation == 5)
            {
                for (int A = 0; A < 20; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(20) - (Projectile.Center - target.Center).PerfectNormalize() * Projectile.width / 2, 40, 40, 6, 0, 0, 0, default, Main.rand.NextFloat(2F, 2.3F))];
                    dust.velocity = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.6F, 0.6F)) * Main.rand.NextFloat(0F, 0.5F) * (Projectile.extraUpdates + 1);
                    dust.noGravity = true;
                }
                if (Skill > 0)
                {
                    if(Projectile.ai[1]<=0)
                    {
                        NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<狱火爆炸Proj>(), Projectile.damage, 0, -1, 0);
                        Projectile.ai[1] = 180;
                    }
                    target.AddBuff(323, 300);
                }
                else
                {

                    target.AddBuff(24, 300);
                }
            }
            else
            {

                for (int A = 0; A < 20; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(20) - (Projectile.Center - target.Center).PerfectNormalize() * Projectile.width / 2, 40, 40, 26, 0, 0, 0, default, Main.rand.NextFloat(1F, 1.3F))];
                    dust.velocity = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.6F, 0.6F)) * Main.rand.NextFloat(0F, 0.5F) * (Projectile.extraUpdates + 1);
                }
            }
        }
        public override void ModifyHitNPC(NPC target, ref HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            if (player.Dplayer().FightPets >= 0)
            {
                Players.BattlePets battle = player.Dplayer().Bpets[player.Dplayer().FightPets];
            }
        }
        public override void Animate(Players.BattlePets battle)
        {
            //战斗形态
            if (Projectile.DProj().Bool[3])
            {
                Projectile.frame = 0;
                if (Projectile.localAI[2] > 0)
                {
                    Projectile.frame = 1;
                    Projectile.localAI[2]--;
                }
                return;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= battle.projFrames)
                {
                    Projectile.frame = 0;
                }
            }
        }
        public void MoveType(Player player, int Type, Players.BattlePets battle)
        {
            if (Type == 0)
            {
                Vector2 vector = player.Center - new Vector2(40 * player.direction, 80) - Projectile.Center;
                if (battle.Life <= 0)
                {
                    vector = player.Center - Projectile.Center;
                    battle.Life = 1;
                    battle.Wounded = true;
                    Fight = false;
                }
                if (vector.Length() >= 10)
                {
                    Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 18) / 21;
                    Projectile.rotation += (Projectile.velocity.X * 0.03F + Math.Abs(Projectile.velocity.Y) * 0.03F * (Projectile.velocity.X > 0 ? 1 : -1));
                }
                else
                if (vector.Length() >= 3)
                {
                    Projectile.velocity = vector.PerfectNormalize() * 3 + player.velocity;
                    Projectile.rotation += (Projectile.velocity.X * 0.03F + Math.Abs(Projectile.velocity.Y) * 0.03F * (Projectile.velocity.X > 0 ? 1 : -1));
                }
                else
                {
                    if (Projectile.velocity.Length() < 0.02F)
                    {
                        Projectile.RotationSpeed(0, 0.1F);
                        Projectile.velocity = Vector2.Zero;
                    }
                    else
                    {
                        Projectile.rotation += (Projectile.velocity.X * 0.03F + Math.Abs(Projectile.velocity.Y) * 0.03F * (Projectile.velocity.X > 0 ? 1 : -1));
                    }
                    if (battle.Life <= 0 && Main.myPlayer == Projectile.owner)
                    {
                        /*
                        battle.Fight = false;
                        DDmod.SyncData(DDType.Battlepets, Main.myPlayer, -1, Main.myPlayer);
                        Projectile.Kill();
                   */ }
                    if (player.velocity.Length() >= 0.1F)
                    {
                        Projectile.velocity = player.velocity;
                        if (Math.Abs(player.velocity.X) < 0.1F)
                        {
                            Projectile.velocity.X = (float)player.direction / 1000;
                        }
                    }
                    else
                    {
                        Projectile.velocity = new Vector2((float)player.direction / 1000, 0);
                    }
                }
            }
            if (Type == 1)
            {
                if (Projectile.DProj().SpeedScope > 0)
                    Projectile.DProj().SpeedScope -= 2F;
                Vector2 vector = player.Center - new Vector2(40 * player.direction, 80) - Projectile.Center;
                if (battle.Life <= 0)
                {
                    vector = player.Center - Projectile.Center;
                }
                if (vector.Length() >= 10)
                {
                    Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 18) / 21;
                    Projectile.rotation = Projectile.velocity.X * 0.03F;
                }
                else
                if (vector.Length() >= 3)
                {
                    Projectile.velocity = vector.PerfectNormalize() * 3 + player.velocity;
                    Projectile.rotation = Projectile.velocity.X * 0.03F;
                }
                else
                {
                    if (Projectile.velocity.Length() < 0.02F)
                    {
                        Projectile.RotationSpeed(0, 0.1F);
                        Projectile.velocity = Vector2.Zero;
                    }
                    else
                    {
                        Projectile.rotation = Projectile.velocity.X * 0.03F;
                    }
                    if (battle.Life <= 0 && Main.myPlayer == Projectile.owner)
                    {
                        battle.Fight = false;
                        DDmod.SyncData(DDType.Battlepets, Main.myPlayer, -1, Main.myPlayer);
                        Projectile.Kill();
                    }
                    if (player.velocity.Length() >= 0.1F)
                    {
                        Projectile.velocity = player.velocity;
                        if (Math.Abs(player.velocity.X) < 0.1F)
                        {
                            Projectile.velocity.X = (float)player.direction / 1000;
                        }
                    }
                    else
                    {
                        Projectile.velocity = new Vector2((float)player.direction / 1000, 0);
                    }
                }
            }
        }
        public void BattleType(Player player, NPC npc, Players.BattlePets battle)
        {
            
            float Speed = 16;
            Vector2 vector = npc.Center - Projectile.Center;
            //Variation
            //3寒霜法师骷髅
            //4蓝盔甲骷髅
            //5地狱骷髅
            //6地狱武士骷髅
            //7暗咒骷髅
            //开技能
            if (battle.SkillCD == 0 && Variation >= 2)
            {
                if (Variation == 2)
                {
                    battle.SkillCD = battle.MaxSkillCD;
                    Skill = 120;
                }
                else
                if (Variation == 5)
                {
                    if (battle.Life > battle.LifeMax * 0.3F)
                    {
                        battle.SkillCD = battle.MaxSkillCD;
                        Skill = 360;
                    }
                }
                else if (Variation == 6)
                {
                    battle.SkillCD = battle.MaxSkillCD;
                    Skill = 30;
                }
                else if (Variation == 7)
                {
                    battle.SkillCD = battle.MaxSkillCD;
                    Skill = 50;
                }
                else
                {
                    battle.SkillCD = battle.MaxSkillCD;
                    Skill = 360;
                }
            }
            if (Variation == 4 && Skill == 360)
            {
                NewDustChange2(100, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 12, false, 0.4F, 1.5F, 0, new Color(64, 202, 251, 120));
                NewDustChange2(50, Projectile.Center, Vector2.Zero, ModContent.DustType<速度粒子>(), 6, 12, true, 2F, 5, 0, new Color(64, 202, 251, 120));
            }
            if (Variation == 5 && Skill == 360)
            {
                NewDustChange4(100, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 12, false, 1.4F, 2.5F, 0, 0, new Color(253, 62, 3, 0) * 2, 3);
                NewDustChange2(50, Projectile.Center, Vector2.Zero, ModContent.DustType<速度粒子>(), 6, 12, true, 2F, 5, 0, new Color(253, 62, 3, 0) * 2);
            }
            if (battle.Skill2CD == 0 && Variation >= 3)
            {
                battle.Skill2CD = battle.MaxSkill2CD;
                Skill2 = 120;
                if (Variation == 3)
                {
                    Skill2 = 1200;
                    Projectile.NewProjectileChange(player.Center, Vector2.Zero, ModContent.ProjectileType<极寒领域>(), 0, 3, -1);
                }
                if (Variation == 4)
                {
                    Skill2 = 1200;
                    player.AddBuff(ModContent.BuffType<固若金汤Buff>(), DDHelper.Second(20));
                }
                if (Variation == 5)
                {
                    Skill2 = 1200;
                    Projectile.NewProjectileChange(Projectile.Center, Projectile.rotation.ToRotationVector2() * 12, ModContent.ProjectileType<地狱魔首>(), Projectile.damage, Projectile.knockBack, -1);
                    Projectile.NewProjectileChange(Projectile.Center, (Projectile.rotation + MathHelper.Pi).ToRotationVector2() * 12, ModContent.ProjectileType<地狱魔首>(), Projectile.damage, Projectile.knockBack, -1);
                }
                if (Variation == 6)
                {
                    Projectile.NewProjectileChange(Projectile.Center, vector.PerfectNormalize()*2, ModContent.ProjectileType<地狱火刃2>(), Projectile.damage/2, Projectile.knockBack, -1,0,0,0,0.5F);
                }
                if (Variation == 7)
                {
                    Skill2 = 1200;
                    player.AddBuff(ModContent.BuffType<魂怒Buff>(), DDHelper.Second(20));
                }
            }
            //成年体
            if (Variation == 2)
            {
                if (Skill > 0)
                {
                    Projectile.RotationSpeed(Projectile.velocity.X * 0.03F, 0.1F);

                    if (Skill > 80)
                    {
                        if (vector.Length() < npc.Size.Length() * 0.75F + 30)
                        {
                            Projectile.velocity = (Projectile.velocity * 20 + -vector.PerfectNormalize() * Speed) / 21;
                        }
                        else
                        {
                            Projectile.velocity *= 0.96F;
                        }
                    }
                    else
                    {
                        Projectile.velocity *= 0.92F;
                    }
                    if (Skill == 20 || Skill == 40 || Skill == 60)
                    {
                        Projectile.localAI[2] = 10;
                        NewDustChange2(20, Projectile.Center + new Vector2(-4, 16), Vector2.Zero, 26, 0, 3, false, 0.6F, 1F, 0);
                        Projectile.NewProjectileChange(Projectile.Center + new Vector2(0, 20).RotatedBy(Projectile.rotation), new Vector2(Main.rand.NextFloat(4, 8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<骷髅头>(), Projectile.damage, 3, -1);
                    }
                    return;
                }
            }
            //冥思骷髅
            if (Variation == 3)
            {
                if (Projectile.DProj().SpeedScope < 128)
                    Projectile.DProj().SpeedScope += 2;
                Vector2 V = npc.Center - (Projectile.Center + new Vector2(-4, 16));

                if (Skill > 0)
                {
                    if (Skill % 15 == 0)
                    {
                        Projectile.localAI[2] = 6;
                        NewDustChange2(20, Projectile.Center + new Vector2(-4, 16), Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 3, false, 0.4F, 0.8F, 0, new Color(0, 150, 255, 0));
                        Projectile.NewProjectileChange(Projectile.Center + new Vector2(0, 20).RotatedBy(Projectile.rotation), new Vector2(Main.rand.NextFloat(4, 8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<寒霜骷髅头>(), Projectile.damage, 3, -1);
                    }
                    if (Skill == 15)
                    {
                        for (int a = 0; a < 4; a++)
                        {
                            NewDustChange2(20, Projectile.Center + new Vector2(-4, 16), Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 5, false, 0.6F, 1F, 0, new Color(0, 150, 255, 0));
                            Projectile.NewProjectileChange(Projectile.Center + new Vector2(0, 20).RotatedBy(Projectile.rotation), new Vector2(Main.rand.NextFloat(4, 8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<寒霜骷髅头>(), Projectile.damage, 3, -1);
                        }
                    }
                }
                else
                {
                    Projectile.ai[1]++;
                    if (Projectile.ai[1] % 30 == 0)
                    {
                        Projectile.localAI[2] = 10;
                        NewDustChange2(20, Projectile.Center + new Vector2(-4, 16), Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 3, false, 0.4F, 0.8F, 0, new Color(0, 150, 255, 0));
                        Projectile.NewProjectileChange(Projectile.Center + new Vector2(0, 20).RotatedBy(Projectile.rotation), V.PerfectNormalize() * 16, ModContent.ProjectileType<寒霜矢>(), Projectile.damage, 3, -1);

                    }
                    if (Projectile.ai[1] % 90 == 0)
                    {
                        Vector2 vector1 = Projectile.Center + new Vector2(Main.rand.Next(40, 100), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        V = npc.Center - vector1;
                        NewDustChange2(20, vector1, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 3, false, 0.4F, 0.8F, 0, new Color(0, 150, 255, 0));
                        Projectile.NewProjectileChange(vector1, V.PerfectNormalize() * 16, ModContent.ProjectileType<寒霜矢>(), Projectile.damage, 3, -1);

                    }
                }
            }
            //地狱武士
            if (Variation == 6)
            {
                Projectile.DProj().Times[0] = npc.whoAmI;
                if (player.ownedProjectileCounts[ModContent.ProjectileType<地狱火刃>()] == 0)
                {
                    Projectile.NewProjectileChange(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<地狱火刃>(), Projectile.damage, 3, -1);
                }
                if (Projectile.ai[1] < 3)
                {
                    Projectile.ai[1] += 0.03F;
                }
                else
                {
                    Projectile.ai[1] = 3;
                }
                if (Skill > 0)
                {
                    Projectile.localAI[2] = 6;
                    if (Skill % 5 == 0)
                    {
                        NewDustChange2(20, Projectile.Center + new Vector2(-4, 16), Vector2.Zero, 6, 0, 3, false, 0.4F, 0.8F, 0);
                        Projectile.NewProjectileChange(Projectile.Center + new Vector2(0, 20).RotatedBy(Projectile.rotation), new Vector2(Main.rand.NextFloat(4, 8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<地狱骷髅头>(), Projectile.damage, 3, -1);
                    }
                }
                Magnification = 1+Projectile.ai[1]/10;
            }
            //暗咒骷髅
            if (Variation == 7)
            {
                Vector2 V = npc.Center - (Projectile.Center + new Vector2(-4, 16));

                if (Skill > 0)
                {
                    if (Skill == 20)
                    {
                        Projectile.localAI[2] = 6;
                        SoundStyle sound = SoundID.Item14;
                        sound.Pitch = -1.5F;
                        PlaySound(sound, Projectile.Center);
                        for (int A = 0; A < 4; A++)
                        {
                            Dust dust = Main.dust[NewDust(Projectile.Center + new Vector2(-4, 16), 1, 1, ModContent.DustType<光圈粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, new Color(148, 43, 43, 0))];
                            dust.noGravity = true;
                            dust.scale = 0.1f;
                            dust.alpha = -5;
                            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                            dust.velocity = Vector2.Zero;
                            dust.noLightEmittence = false;
                            dust.customData = new Vector4(0.2F, 40 * Projectile.scale, 0, 1F);
                        }
                        Projectile.NewProjectileChange(Projectile.Center + new Vector2(0, 20).RotatedBy(Projectile.rotation), new Vector2(Main.rand.NextFloat(4, 8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<夺命鬼魂>(), Projectile.damage, 3, -1, Main.rand.NextFloat(4, 8));
                        Projectile.NewProjectileChange(Projectile.Center + new Vector2(0, 20).RotatedBy(Projectile.rotation), new Vector2(Main.rand.NextFloat(4, 8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<夺命鬼魂>(), Projectile.damage, 3, -1, Main.rand.NextFloat(4, 8));
                        Projectile.NewProjectileChange(Projectile.Center + new Vector2(0, 20).RotatedBy(Projectile.rotation), new Vector2(Main.rand.NextFloat(4, 8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<夺命鬼魂>(), Projectile.damage, 3, -1, Main.rand.NextFloat(4, 8));
                    }
                }
                else
                {
                    Projectile.ai[1]++;
                    if (Projectile.ai[1] % 80 == 0)
                    {
                        SoundStyle sound = SoundID.Item14;
                        sound.Volume = 0.2F;
                        sound.Pitch = -1;
                        PlaySound(sound, Projectile.Center);
                        for (int A = 0; A < 4; A++)
                        {
                            Dust dust = Main.dust[NewDust(Projectile.Center + new Vector2(-4, 16), 1, 1, ModContent.DustType<光圈粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, new Color(214, 241, 241, 0))];
                            dust.noGravity = true;
                            dust.scale = 0.1f;
                            dust.alpha = -5;
                            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                            dust.velocity = Vector2.Zero;
                            dust.noLightEmittence = false;
                            dust.customData = new Vector4(0.1F, 40 * Projectile.scale, 0, 1F);
                        }
                        Projectile.NewProjectileChange(Projectile.Center + new Vector2(0, 20).RotatedBy(Projectile.rotation), new Vector2(Main.rand.NextFloat(4, 8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<鬼魂>(), Projectile.damage, 3, -1, Main.rand.NextFloat(4, 8));

                    }
                }
            }
            if (Variation == 4 && Skill > 0)
            {
                Magnification *= 1.25F;
            }
            if (Variation == 5 && Skill > 0)
            {

                Magnification = 1.25F;
            }

            if (Variation == 3 || Variation == 7)
            {
                Move(1);
            }
            else
                if (Variation == 6)
            {

                Move(2);
            }
            else
            {
                if (Variation == 4)
                {
                    Speed *= 0.5F;
                    if (Skill > 0)
                    {
                        ExtraUpdates = 1;
                        Speed *= 3F;
                    }
                }
                if (Variation == 5)
                {
                    if (Projectile.ai[1] > 0)
                    {
                        Projectile.ai[1]--;
                    }
                    if (Skill > 0)
                    {
                        ExtraUpdates = 1;
                        battle.Endurance -= 0.2F;
                        Speed *= 2F;
                    }
                }
                Move(0);
            }
            void Move(int Type)
            {
                if (Type == 0)
                {
                    Projectile.rotation += (Projectile.velocity.X * 0.06F + Math.Abs(Projectile.velocity.Y) * 0.06F * (Projectile.velocity.X > 0 ? 1 : -1));

                    if (vector.Length() > 80)
                    {
                        Projectile.velocity = (Projectile.velocity * 50 + vector.PerfectNormalize() * Speed * 2) / 51;
                    }
                    else
                    {
                        if (Projectile.velocity.Length() < Speed * 2)
                        {
                            Projectile.velocity *= 1.02f;
                        }
                        else
                        {
                            Projectile.velocity *= 0.98f;
                        }

                    }
                }
                else if (Type == 1)
                {

                    Projectile.rotation = Projectile.velocity.X * 0.03F;
                    if (vector.Length() < 200)
                    {
                        Projectile.velocity = (Projectile.velocity * 20 + -vector.PerfectNormalize() * Speed) / 21;
                    }
                    else if (vector.Length() > 250)
                    {
                        Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * Speed) / 21;
                    }
                }
                else
                {

                    Vector2 vector = (npc.Center - new Vector2(0, npc.height + 120)) - Projectile.Center;
                    Projectile.rotation = Projectile.velocity.X * 0.03F;
                    float S = vector.Length() / 10;
                    if (S > Speed)
                    {
                        S = Speed;
                    }
                    Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * S) / 21;
                }

                if (Variation == 4)
                {
                    if (Skill > 0)
                        Projectile.velocity = Projectile.velocity.RotatedBy(0.02F);
                }
            }
        }
        float KX;
        public override void Movement(Player player)
        {
            NPC npc = NPCdirection.FindClosest(player.Center, 1000);
            Players.BattlePets battle = player.Dplayer().Bpets[player.Dplayer().FightPets];
            if (battle == null)
            {
                return;
            }
            if (Variation == 5)
            {
                if (Skill <= 0)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 0, default, Main.rand.NextFloat(1.25F, 2F))];
                    dust.noGravity = true;
                }
                else
                {
                    for (int A = 0; A < 3; A++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 0, default, Main.rand.NextFloat(1.25F, 2F))];
                        dust.noGravity = true;
                    }
                    if (battle.Life > battle.LifeMax * 0.2F)
                    {
                        if (KX < 0)
                        {
                            KX = 0;
                        }
                        KX += battle.LifeMax / 10000F;
                        if (KX >= 1)
                        {
                            battle.Life -= (int)KX;
                            KX -= (int)KX;
                        }
                    }
                    else
                    {
                        Skill = 0;
                    }
                }
            }

            if (Variation == 6)
            {
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0, 0, 0, default, Main.rand.NextFloat(1.25F, 2F))];
                dust.noGravity = true;
            }
            if (npc == null || battle.Life <= 0 || !Fight)
            {
                if (Variation == 3 || Variation == 6 || Variation == 7)
                {
                    MoveType(player, 1, battle);
                }
                else
                {

                    MoveType(player, 0, battle);
                }
                Projectile.DProj().Bool[3] = false;

                if (Variation == 6)
                {
                    Projectile.ai[1] = 0;
                }
            }
            else
            {
                //player.Center = Projectile.Center;
                BattleType(player, npc, battle);
                Projectile.DProj().Bool[3] = true;
            }
            //技能CD

            if (Skill > 0)
            {
                Skill -= 1f / (Projectile.extraUpdates + 1);
            }
            if (Skill2 > 0)
            {
                Skill2 -= 1f / (Projectile.extraUpdates + 1);
            }
            //Projectile.RotationSpeed(Projectile.velocity.ToRotation(),0.1F);
            //Projectile.Center = player.Center - new Vector2(100);
            if (Projectile.rotation < 0f)
            {
                Projectile.rotation += MathHelper.TwoPi;
            }
            else if (Projectile.rotation > MathHelper.TwoPi)
            {
                Projectile.rotation -= MathHelper.TwoPi;
            }
        }
        public override void VariationLevel(Player player, int Level)
        {
            Variation = 0;
            if (Level >= 16)
            {
                Variation = 1;
            }
            if (Level >= 30)
            {
                Variation = 2;
            }
            if (Level >= 46)
            {
                Players.BattlePets battle = player.Dplayer().Bpets[player.Dplayer().FightPets];
                if (battle == null)
                {
                    return;
                }
                Variation = 3 + battle.Variant - 1;
            }
        }
        public override int Width()
        {
            if (Variation == 1)
            {
                return 30;
            }
            if (Variation == 2)
            {
                return 52;
            }
            if (Variation == 3 || Variation == 5)
            {
                return 52;
            }
            if (Variation == 4)
            {
                return 58;
            }
            if (Variation == 6)
            {
                return 66;
            }
            if (Variation == 7)
            {
                return 50;
            }
            return 22;
        }
        public override int Height()
        {
            if (Variation == 1)
            {
                return 30;
            }
            if (Variation == 2)
            {
                return 52;
            }
            if (Variation == 3 || Variation == 5)
            {
                return 52;
            }
            if (Variation == 4)
            {
                return 58;
            }
            if (Variation == 6)
            {
                return 66;
            }
            if (Variation == 7)
            {
                return 56;
            }
            return 42;
        }
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D Glow = 牢中灵骷.Glow.Value;
            Rectangle rectangle;
            float RO = Projectile.rotation;
            if (Variation == 4 && Skill > 0)
            {
                rectangle = new Rectangle(0, Glow2.Height() / Main.projFrames[Projectile.type] * Projectile.frame, Glow2.Width(), Glow2.Height() / Main.projFrames[Projectile.type]);
                for (int a = 0; a < Projectile.oldPos.Length; a++)
                {
                    RO = Projectile.oldRot[a];
                    Main.spriteBatch.Draw(Glow2.Value, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, rectangle, new Color(64, 202, 251, 120) * 0.5f * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), RO, rectangle.Size() / 2, Projectile.scale / 2, 0, 0);
                }
            }
            if (Variation == 5)
            {
                rectangle = new Rectangle(0, Glow3.Height() / Main.projFrames[Projectile.type] * Projectile.frame, Glow3.Width(), Glow3.Height() / Main.projFrames[Projectile.type]);
                for (int a = 0; a < Projectile.oldPos.Length; a++)
                {
                    if (a == 0 || Skill > 0)
                    {
                        RO = Projectile.oldRot[a];
                        Main.spriteBatch.Draw(Glow3.Value, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, rectangle, new Color(253, 62, 3, 0) * 0.5f * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), RO, rectangle.Size() / 2, Projectile.scale / 2, 0, 0);
                    }
                }
            }

            rectangle = new Rectangle(texture.Width / 8 * Variation, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width / 8, texture.Height / Main.projFrames[Projectile.type]);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, RO, rectangle.Size() / 2, Projectile.scale, 0, 0);
            Main.spriteBatch.Draw(Glow, Projectile.Center - Main.screenPosition, rectangle, Color.White, RO, rectangle.Size() / 2, Projectile.scale, 0, 0);

            if (Projectile.velocity.Length() > 1)
            {
                int R = (int)Projectile.velocity.Length() * (Projectile.extraUpdates + 1);
                if (R > Projectile.oldPos.Length)
                {
                    R = Projectile.oldPos.Length;
                }
                if (Variation != 5 || Skill == 0)
                {

                    for (int a = 0; a < R; a++)
                    {
                        RO = Projectile.oldRot[a];
                        if (Variation != 5 && Variation != 6)
                        {
                            Main.spriteBatch.Draw(texture, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, rectangle, lightColor * 0.25f * ((R - a) / (float)R), RO, rectangle.Size() / 2, Projectile.scale, 0, 0);
                        }
                        Main.spriteBatch.Draw(Glow, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, rectangle, Color.White * 0.25f * ((R - a) / (float)R), RO, rectangle.Size() / 2, Projectile.scale, 0, 0);
                    }
                }
            }
            else
            {
                Projectile.oldPos[1] = Vector2.Zero;
            }
            return false;
        }
    }
    public class 地狱魔首 : ModProjectile
    {

        public override string Texture => "DDmod/Content/Projectiles/BattlePets/牢中灵骷";
        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 52;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 900;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.extraUpdates = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 24;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 24;
        }
        public override void AI()
        {
            float Speed = 16;
            NPC npc = NPCdirection.FindClosest(Projectile.Center, 800);
            if (npc != null)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                Projectile.rotation += (Projectile.velocity.X * 0.06F + Math.Abs(Projectile.velocity.Y) * 0.06F * (Projectile.velocity.X > 0 ? 1 : -1));

                if (vector.Length() > 80)
                {
                    Projectile.velocity = (Projectile.velocity * 50 + vector.PerfectNormalize() * Speed * 2) / 51;
                }
                else
                {
                    if (Projectile.velocity.Length() < Speed * 2)
                    {
                        Projectile.velocity *= 1.02f;
                    }
                    else
                    {
                        Projectile.velocity *= 0.98f;
                    }

                }
                Projectile.velocity = Projectile.velocity.RotatedBy(0.02F);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.netUpdate = true;
            target.AddBuff(24, (int)Main.rand.NextFloat(120, 300));
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
           Projectile.NewProjectileChange(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<狱火爆炸Proj>(), Projectile.damage, 0, -1, 0);
            //NewDustChange2(70, Projectile.Center, Vector2.Zero, 6, 0, 12, true, 0.4F, 1.5F, 0);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D Glow2 = 牢中灵骷.Glow3.Value;
            Rectangle rectangle;
            float RO = Projectile.rotation;
            rectangle = new Rectangle(0, Glow2.Height / 2 * Projectile.frame, Glow2.Width, Glow2.Height / 2);
            for (int a = 0; a < Projectile.oldPos.Length; a++)
            {
                RO = Projectile.oldRot[a];
                Main.spriteBatch.Draw(Glow2, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, rectangle, new Color(255, 102, 8, 0) * 0.15f * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), RO, rectangle.Size() / 2, Projectile.scale / 2, 0, 0);
            }

            rectangle = new Rectangle(texture.Width / 8 * 5, texture.Height / 2 * Projectile.frame, texture.Width / 8, texture.Height / 2);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, new Color(253, 62, 3, 50), RO, rectangle.Size() / 2, Projectile.scale, 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, new Color(253, 62, 3, 0), RO, rectangle.Size() / 2, Projectile.scale, 0, 0);

            if (Projectile.velocity.Length() > 1)
            {
            }
            else
            {
                Projectile.oldPos[1] = Vector2.Zero;
            }
            return false;
        }
    }
}
