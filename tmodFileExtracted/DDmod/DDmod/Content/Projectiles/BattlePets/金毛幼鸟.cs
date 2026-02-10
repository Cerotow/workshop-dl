using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.BattlePets.Proj;
using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;
using DDmod.Players;
using System.Diagnostics;

namespace DDmod.Content.Projectiles.BattlePets
{
    public class 金毛幼鸟 : BattlePetsProj
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            //Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
            base.Load();
        }
        public override void SetDef()
        {
            Main.projFrames[Projectile.type] = 6;

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
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.localAI[0] = 20;
            Player player = Main.player[Projectile.owner];
            /*
            if (player.Dplayer().FightPets >= 0)
            {
                Players.BattlePets battle = player.Dplayer().Bpets[player.Dplayer().FightPets];

                if (Variation == 3)
                {
                        if (target.HasBuff(30))
                        {
                            int L = damageDone / 10;
                            battle.Life += L;
                            CombatText.NewText(Projectile.getRect(), new Color(100, 255, 100, 255), L, false, true);
                            if (battle.Life > battle.LifeMax)
                            {
                                battle.Life = battle.LifeMax;
                            }
                        }
                }
            }*/
            if (Variation == 0)
            {
                for (int a = -1; a <= 1; a++)
                {
                    int D = NewDust(target.Center, 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(205, 205, 205, 220), 2.5F);
                    
                    Main.dust[D].position -= Projectile.velocity.PerfectNormalize() * 24;
                    Main.dust[D].position += Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2 * a) * 5;
                    Main.dust[D].velocity = Projectile.velocity.PerfectNormalize() * 3;
                    Main.dust[D].customData = 2;
                    Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
                }
            }
            if (Variation == 1)
            {
                for (int a = -1; a <= 1; a++)
                {
                    int D = NewDust(target.Center, 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(205, 205, 205, 220), 3.5F);
                    
                    Main.dust[D].position -= Projectile.velocity.PerfectNormalize() * 24;
                    Main.dust[D].position += Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2 * a) * 5;
                    Main.dust[D].velocity = Projectile.velocity.PerfectNormalize() * 3;
                    Main.dust[D].customData = 2;
                    Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
                }
            }
            if (Variation == 2)
            {
                for (int a = -1; a <= 1; a++)
                {
                    int D = NewDust(target.Center, 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(205, 205, 205, 220), 4F);
                    
                    Main.dust[D].position -= Projectile.velocity.PerfectNormalize() * 24;
                    Main.dust[D].position += Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2 * a) * 5;
                    Main.dust[D].velocity = Projectile.velocity.PerfectNormalize() * 3;
                    Main.dust[D].customData = 2;
                    Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
                }
            }
            if (Variation == 3)
            {
                for (int a = -1; a <= 1; a++)
                {
                    int D = NewDust(target.Center, 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(205, 205, 205, 220), 5F);
                    
                    Main.dust[D].position -= Projectile.velocity.PerfectNormalize() * 44;
                    Main.dust[D].position += Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2 * a) * 8;
                    Main.dust[D].velocity = Projectile.velocity.PerfectNormalize() * 10;
                    Main.dust[D].customData = 3;
                    Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
                }
                target.AddBuff(30, 300);
            }
            Projectile.velocity.Y = -Projectile.velocity.Y/2;
            Time = 20;
        }
        public override void ModifyHitNPC(NPC target, ref HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            if (player.Dplayer().FightPets >= 0)
            {
                Players.BattlePets battle = player.Dplayer().Bpets[player.Dplayer().FightPets];
                if (Variation == 3)
                {
                    if (Skill2 > 0)
                    {
                        target.AddBuff(30, 300);
                        if (target.HasBuff(30))
                        {
                            modifiers.FinalDamage *= 1.2F;
                        }
                    }
                }
            }
            Projectile.netUpdate = true;
        }
        public override void Movement(Player player)
        {
            NPC npc = NPCdirection.FindClosest(player.Center, 1000);
            Players.BattlePets battle = player.Dplayer().Bpets[player.Dplayer().FightPets];
            if (npc == null || battle.Life <= 0|| !Fight)
            {
                Vector2 vector = player.Center - new Vector2(40 * player.direction, 80) - Projectile.Center;
                if (Skill2 > 0)
                {
                    Projectile.velocity *= 0.92F;

                    if (Skill2 > 30)
                    {
                        for (int a = 0; a < 4; a++)
                        {
                            int A = NewDust(Projectile.Center - new Vector2(4) + new Vector2(Main.rand.Next(200, 240), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(255, 238, 53, 100), Scale: 1.8F);
                            Main.dust[A].velocity = (Projectile.Center - new Vector2(4, -6) - Main.dust[A].position).PerfectNormalize() * Main.rand.NextFloat(10, 14);
                            Main.dust[A].noGravity = true;
                            Main.dust[A].customData = 3.5f + Main.dust[A].DustAI(3);
                        }
                    }
                    if (Skill2 == 30)
                    {
                        NewDustChange2(100, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 8, false, 0.2F, 1, 0, new Color(255, 238, 53, 100));
                        NewDustChange2(50, Projectile.Center, Vector2.Zero, ModContent.DustType<速度粒子>(), 8, 12, true, 2F, 4, 0, new Color(255, 238, 53, 100));
                        if (Main.myPlayer == Projectile.owner)
                        {
                            for (int a = 0; a < 12; a++)
                                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * 12, ModContent.ProjectileType<治愈飞羽>(), 1, 2);
                        }
                    }
                }
                else
                {
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
                    }
                    else
                    if (vector.Length() >= 3)
                    {
                        Projectile.velocity = vector.PerfectNormalize() * 3 + player.velocity;
                    }
                    else
                    {
                        if (battle.Life <= 0 && Main.myPlayer == Projectile.owner)
                        {
                            /*
                            if (battle.Life <= 0)
                            {
                                battle.Fight = false;
                                DDmod.SyncData(DDType.Battlepets, Main.myPlayer, -1, Main.myPlayer);
                                Projectile.Kill();
                            }*/
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
                if(Fight)
                {
                    if(Projectile.Player().statLife!= Projectile.Player().statLifeMax2)
                    {
                        if (battle.Skill2CD == 0 && Variation >= 3)
                        {
                            battle.Skill2CD = battle.MaxSkill2CD;
                            Skill2 = 90;
                        }
                    }
                }
            }
            else
            {
                
                Vector2 vector = npc.Center - Projectile.Center;
                Vector2 Govector = npc.position+new Vector2(npc.width/2,0) - Projectile.Center;
                
                if (Projectile.localAI[0] <= 0&& Skill2<=0)
                {
                    Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 12) / 21;
                    if (Govector.Length() < 200)
                    {
                        if (Time < 15)
                        {

                            Projectile.velocity *= 0.86F;
                        }
                        if (Time-- <= 0)
                        {
                            Projectile.velocity = vector.PerfectNormalize() * 24;
                            Time = 120;
                        }
                    }
                    else
                    {
                        Time = 15;
                    }
                    
                    Projectile.DProj().Times[3] = 0;

                    if (battle.SkillCD == 0 && Variation >= 2&& Projectile.damage>0)
                    {
                        battle.SkillCD = battle.MaxSkillCD;

                        Time = 15;
                        if (Main.myPlayer == Projectile.owner)
                        {
                            for (int a = 0; a < 12; a++)
                            {
                                Vector2 vector1 = vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.4F, 0.4F)) * Main.rand.NextFloat(8F, 16F);
                                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center+ vector1*5, vector1, ModContent.ProjectileType<女妖飞羽>(), Projectile.damage, 2);
                            }
                        }
                        Projectile.velocity = -vector.PerfectNormalize() * 24;
                    }
                }
                else
                {
                    if (battle.Skill2CD == 0 && Variation >= 3)
                    {
                        battle.Skill2CD = battle.MaxSkill2CD;
                        Skill2 = 90;
                        Projectile.netUpdate = true;
                    }

                    if (Govector.X < 0)
                    {
                        Projectile.DProj().Times[0] = 1;
                    }
                    else
                    {
                        Projectile.DProj().Times[0] = -1;
                    }
                    if (Skill2 <= 0)
                    {
                        if (Govector.Y < 20)
                        {
                            Govector = npc.position + new Vector2(npc.width / 2 + 100 * Projectile.DProj().Times[0], -120) - Projectile.Center;
                            Projectile.velocity = (Projectile.velocity * 20 + Govector.PerfectNormalize() * 12) / 21;
                        }
                        else
                        {
                            Govector = npc.position + new Vector2(npc.width / 2 + 100 * Projectile.DProj().Times[0], -120) - Projectile.Center;
                            Projectile.velocity = (Projectile.velocity * 20 + Govector.PerfectNormalize() * 12) / 21;
                        }
                    }
                    else
                    {
                        Projectile.velocity *= 0.92F;

                        if (Skill2 > 30)
                        {
                            for (int a = 0; a < 4; a++)
                            {
                                int A = NewDust(Projectile.Center - new Vector2(4) + new Vector2(Main.rand.Next(200, 240), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, newColor: new Color(255, 238, 53, 100), Scale: 1.8F);
                                Main.dust[A].velocity = (Projectile.Center - new Vector2(4, -6) - Main.dust[A].position).PerfectNormalize() * Main.rand.NextFloat(10, 14);
                                Main.dust[A].noGravity = true;
                                Main.dust[A].customData = 3.5f + Main.dust[A].DustAI(3);
                            }
                        }
                        if (Skill2==30)
                        {
                            NewDustChange2(100, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 8, false, 0.2F, 1, 0, new Color(255,238,53,100));
                            NewDustChange2(50, Projectile.Center, Vector2.Zero, ModContent.DustType<速度粒子>(), 8, 12, true, 2F, 4, 0, new Color(255, 238, 53, 100));
                            if (Main.myPlayer == Projectile.owner)
                            {
                                for(int a= 0;a<12;a++)
                                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * 12, ModContent.ProjectileType<治愈飞羽>(), 1, 2);
                            }
                        }
                    }
                }
            }
            if(Skill>0)
            {
                Skill--;
            }
            if(Skill2>0)
            {
                Skill2--;
            }
            Projectile.localAI[0]--;
            Projectile.rotation = Projectile.velocity.X* 0.03f;
            //Projectile.RotationSpeed(Projectile.velocity.ToRotation(),0.1F);
            //Projectile.Center = player.Center - new Vector2(100);
        }
        public override int Width()
        {
            if (Variation == 1)
            {
                return 16;
            }
            if (Variation == 2)
            {
                return 24;
            }
            if (Variation == 3)
            {
                return 38;
            }
            return 12;
        }
        public override int Height()
        {
            if(Variation==1)
            {
                return 12;
            }
            if(Variation==2)
            {
                return 24;
            }
            if (Variation == 3)
            {
                return 52;
            }
            return 12;
        }
        public override void Animate(Players.BattlePets battle)
        {
            Projectile.frameCounter++;
            if (Variation == 0)
            {
                if (Projectile.frameCounter > 4)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                }
            }
            if (Variation == 1)
            {
                if (Projectile.frameCounter > 3)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                }
            }
            if (Variation == 2)
            {
                if (Projectile.frameCounter > 3)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                }
            }
            if (Variation == 3)
            {
                if (Projectile.frameCounter > 2)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                }
            }
            if (Projectile.frame >= battle.projFrames)
            {
                Projectile.frame = 0;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle rectangle = new Rectangle(texture.Width /4*Variation, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width / 4, texture.Height / Main.projFrames[Projectile.type]);
            SpriteEffects sprite = 0;
            float RO = Projectile.rotation;
            if (Projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, RO, rectangle.Size() / 2, Projectile.scale, sprite, 0);

                for (int a = 0; a < Projectile.oldPos.Length; a++)
                {
                    RO = Projectile.oldRot[a];
                    if (Projectile.velocity.X < 0)
                    {
                        sprite = SpriteEffects.FlipHorizontally;
                    }
                    Main.spriteBatch.Draw(texture, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, rectangle, lightColor*0.15f * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), RO, rectangle.Size() / 2, Projectile.scale, sprite, 0);
                }
            /*
            if (Projectile.Player().Dplayer().FightPets >= 0)
            {
                Vector2 vector = Projectile.Center - Main.screenPosition;
                Projectile.damage = Projectile.Player().Dplayer().Bpets[Projectile.Player().Dplayer().FightPets].Damage;
                string text = ("Lv." + Projectile.Player().Dplayer().Bpets[Projectile.Player().Dplayer().FightPets].Level)+"  生命:" + Projectile.Player().Dplayer().Bpets[Projectile.Player().Dplayer().FightPets].Life + "/" + Projectile.Player().Dplayer().Bpets[Projectile.Player().Dplayer().FightPets].LifeMax;
                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, text, vector.X, vector.Y -20, Color.White, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, text, new Vector2(1)) / 2, 0.75F);
            }*/
            return false;
        }
    }
}
