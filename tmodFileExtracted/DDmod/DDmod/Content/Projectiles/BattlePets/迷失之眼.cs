using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;
using DDmod.Players;
using System.Diagnostics;

namespace DDmod.Content.Projectiles.BattlePets
{
    public class 迷失之眼 : BattlePetsProj
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
            base.Load();
        }
        public override void SetDef()
        {
            Main.projFrames[Projectile.type] = 5;

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
            if (player.Dplayer().FightPets >= 0)
            {
                Players.BattlePets battle = player.Dplayer().Bpets[player.Dplayer().FightPets];

                if (Variation == 3)
                {
                        if (target.HasBuff(30))
                        {
                            int L = damageDone*3;
                            battle.Life += L;
                            CombatText.NewText(Projectile.getRect(), new Color(100, 255, 100, 255), L, false, true);
                            if (battle.Life > battle.LifeMax)
                            {
                                battle.Life = battle.LifeMax;
                            }
                        }
                }
            }
            if (Variation >= 1)
            {
                for (int a = -1; a <= 1; a++)
                {
                    if (a != 0 || Variation > 1)
                    {
                        if (Skill2 <= 0)
                        {
                            int D = NewDust(target.Center, 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(205, 20, 10, 220), 4);
                            Main.dust[D].position -= Projectile.velocity.PerfectNormalize() * 24;
                            Main.dust[D].position += Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2 * a) * 8;
                            Main.dust[D].velocity = Projectile.velocity.PerfectNormalize() * 7;
                            Main.dust[D].customData = 2;
                            Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
                        }
                        else
                        {
                            int D = NewDust(target.Center, 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(205, 20, 10, 220), 8);
                            Main.dust[D].position -= Projectile.velocity.PerfectNormalize() * 24;
                            Main.dust[D].position += Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2 * a) * 16;
                            Main.dust[D].velocity = Projectile.velocity.PerfectNormalize() * 7;
                            Main.dust[D].customData = 4;
                            Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
                        }
                    }
                }
            }
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
        }
        public override void Movement(Player player)
        {
            NPC npc = NPCdirection.FindClosest(player.Center, 1000);
            Players.BattlePets battle = player.Dplayer().Bpets[player.Dplayer().FightPets];
            if (battle == null)
            {
                return;
            }
            if (npc == null || battle.Life <= 0|| !Fight)
            {
                Vector2 vector = player.Center - new Vector2(40 * player.direction, 80) - Projectile.Center;
                if (battle.Life <= 0)
                {
                    battle.Life = 1;
                    battle.Wounded = true;
                    vector = player.Center - Projectile.Center;
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
                    /*
                    if (battle.Life <= 0&& Main.myPlayer == Projectile.owner)
                    {
                        battle.Fight = false;
                        DDmod.SyncData(DDType.Battlepets, Main.myPlayer, -1, Main.myPlayer);
                        Projectile.Kill();
                    }*/
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
            else
            {
                //player.Center = Projectile.Center;
                if (battle.SkillCD == 0&&Variation >= 2)
                {
                    battle.SkillCD = battle.MaxSkillCD;
                    Skill = 120;
                }
                if(battle.Skill2CD == 0&&Variation >= 3)
                {
                    battle.Skill2CD = battle.MaxSkill2CD;
                    Skill2 = 300;
                }
                if(Skill==300)
                {
                    Color color = new Color(205, 20, 10, 220);
                    Color color2 = new Color(205, 20, 10, 220);
                    NewDustChange2(100, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 4, false, 0.2F, 1, 0, color);
                    NewDustChange2(50, Projectile.Center, Vector2.Zero, ModContent.DustType<速度粒子>(), 4, 6, true, 2F, 4, 0, color2);

                }
                
                Vector2 vector = npc.Center - Projectile.Center;
                if (Skill > 0)
                {
                    ExtraUpdates = 1;
                    if (Projectile.localAI[0] <= 0)
                    {
                        if (Skill2 > 0)
                        {
                            Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 18) / 21;
                            if (Time < 15)
                            {

                                Projectile.velocity *= 0.96F;
                            }
                            if (Time-- <= 0)
                            {
                                Projectile.velocity = vector.PerfectNormalize() * 40;
                                Time = 60;
                                NewDustChange2(50, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 8, false, 0.2F, 1.5F, 0, new Color(205, 20, 10, 220));
                                NewDustChange2(20, Projectile.Center, Vector2.Zero, ModContent.DustType<速度粒子>(), 8, 12, true, 3F, 6, 0, new Color(205, 20, 10, 220));
                            }
                        }
                        else
                        {

                            Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 12) / 21;
                            if (Time < 15)
                            {

                                Projectile.velocity *= 0.96F;
                            }
                            if (Time-- <= 0)
                            {
                                Projectile.velocity = vector.PerfectNormalize() * 28;
                                Time = 60;
                                NewDustChange2(50, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 4, false, 0.2F, 1, 0, new Color(205, 20, 10, 220));
                                NewDustChange2(20, Projectile.Center, Vector2.Zero, ModContent.DustType<速度粒子>(), 4, 6, true, 2F, 4, 0, new Color(205, 20, 10, 220));
                            }
                        }
                        Projectile.DProj().Times[3] = 0;
                    }
                    else
                    {
                        if (Projectile.DProj().Times[3] == 0)
                        {
                            Projectile.DProj().Times[3] = Main.rand.NextBool(2) ? Main.rand.NextFloat(0.5F, 1) : -Main.rand.NextFloat(0.5F, 1);
                        }
                        if (Projectile.velocity.Length() < 8)
                            Projectile.velocity += Projectile.rotation.ToRotationVector2() * 1F;
                        else
                            Projectile.velocity *= 0.96f;
                        Projectile.velocity = Projectile.velocity.RotatedBy(0.02F * Projectile.DProj().Times[3]);
                    }
                }
                else
                {
                    if (Projectile.localAI[0] <= 0)
                    {
                        Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 12) / 21;
                        if (Time<15)
                        {

                            Projectile.velocity *= 0.96F;
                        }
                        if (Time--<=0)
                        {
                            Projectile.velocity = vector.PerfectNormalize() * 16;
                            Time = 120;
                        }
                        Projectile.DProj().Times[3] = 0;
                    }
                    else
                    {
                        if (Projectile.DProj().Times[3] == 0)
                        {
                            Projectile.DProj().Times[3] = Main.rand.NextBool(2) ? Main.rand.NextFloat(0.5F,1) : -Main.rand.NextFloat(0.5F, 1);
                        }
                        if (Projectile.velocity.Length() < 8)
                            Projectile.velocity += Projectile.rotation.ToRotationVector2() * 1F;
                        else
                            Projectile.velocity *= 0.96f;
                        Projectile.velocity = Projectile.velocity.RotatedBy(0.02F * Projectile.DProj().Times[3]);
                    }
                }
            }
            if(Skill>0)
            {
                Magnification = 1.5f;
                Skill -= 1f/ (Projectile.extraUpdates + 1);
            }
            if (Skill2 > 0)
            {
                battle.SkillCD = battle.MaxSkillCD;
                Skill = 2;
                Skill2 -= 1f / (Projectile.extraUpdates+1);
                if (Projectile.DProj().Times[2] < 1.5F)
                    Projectile.DProj().Times[2] += 0.01F;
                else
                    Projectile.DProj().Times[2] = 1.75f;
            }
            else
            {
                if (Projectile.DProj().Times[2] > 1F)
                {
                    Projectile.DProj().Times[2] -= 0.1F;
                }
                else
                {
                    Projectile.DProj().Times[2] = 1f;
                }
            }
            Projectile.localAI[0]--;
            Projectile.rotation = Projectile.velocity.ToRotation();
            //Projectile.RotationSpeed(Projectile.velocity.ToRotation(),0.1F);
            //Projectile.Center = player.Center - new Vector2(100);
        }
        public override int Width()
        {
            if (Variation == 1)
            {
                return 30;
            }
            if (Variation == 2)
            {
                return 38;
            }
            if (Variation == 3)
            {
                return (int)(56 * Projectile.DProj().Times[2]);
            }
            return 22;
        }
        public override int Height()
        {
            if(Variation==1)
            {
                return 30;
            }
            if(Variation==2)
            {
                return 38;
            }
            if (Variation == 3)
            {
                return (int)(56 * Projectile.DProj().Times[2]);
            }
            return 22;
        }
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D Glow = 迷失之眼.Glow.Value;
            Rectangle rectangle = new Rectangle(texture.Width /4*Variation, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width / 4, texture.Height / Main.projFrames[Projectile.type]);
            SpriteEffects sprite = 0;
            float RO = Projectile.rotation;
            if (Projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipHorizontally;
                RO += MathHelper.Pi;
            }

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, lightColor, RO, rectangle.Size() / 2, Projectile.scale, sprite, 0);

            if (Variation == 3)
            { 
                if (Projectile.DProj().Times[2] > 1)
                {
                    /*
                    texture = Glow;
                    rectangle = new Rectangle(texture.Width / 4 * Variation, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width / 4, texture.Height / Main.projFrames[Projectile.type]);

                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, new Color(155, 0, 0, 0), RO, rectangle.Size() / 2, Projectile.scale/4* Projectile.DProj().Times[2], sprite, 0);
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, new Color(155, 0, 0, 0), RO, rectangle.Size() / 2, Projectile.scale/4* Projectile.DProj().Times[2], sprite, 0);
                    for (int a = 0; a < Projectile.oldPos.Length; a++)
                    {
                        RO = Projectile.oldRot[a];
                        if (Projectile.velocity.X < 0)
                        {
                            sprite = SpriteEffects.FlipHorizontally;
                            RO += MathHelper.Pi;
                        }
                        Main.spriteBatch.Draw(texture, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, rectangle, new Color(155, 0, 0, 0) * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), RO, rectangle.Size() / 2, Projectile.scale/4* Projectile.DProj().Times[2], sprite, 0);
                    }*/
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, new Color(155, 0, 0, 100), RO, rectangle.Size() / 2, Projectile.scale * Projectile.DProj().Times[2], sprite, 0);
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, new Color(155, 0, 0, 0), RO, rectangle.Size() / 2, Projectile.scale * Projectile.DProj().Times[2], sprite, 0);
                    for (int a = 0; a < Projectile.oldPos.Length; a++)
                    {
                        RO = Projectile.oldRot[a];
                        if (Projectile.velocity.X < 0)
                        {
                            sprite = SpriteEffects.FlipHorizontally;
                            RO += MathHelper.Pi;
                        }
                        Main.spriteBatch.Draw(texture, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, rectangle, new Color(120, 0, 0, 0) * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), RO, rectangle.Size() / 2, Projectile.scale * Projectile.DProj().Times[2], sprite, 0);
                    }
                }
                else
                if (Skill > 0)
                {
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, new Color(155, 0, 0, 0), RO, rectangle.Size() / 2, Projectile.scale, sprite, 0);
                    for (int a = 0; a < Projectile.oldPos.Length; a++)
                    {
                        RO = Projectile.oldRot[a];
                        if (Projectile.velocity.X < 0)
                        {
                            sprite = SpriteEffects.FlipHorizontally;
                            RO += MathHelper.Pi;
                        }
                        Main.spriteBatch.Draw(texture, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, rectangle, new Color(120, 0, 0, 0) * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), RO, rectangle.Size() / 2, Projectile.scale, sprite, 0);
                    }
                }
                else
                {
                    for (int a = 0; a < Projectile.oldPos.Length; a++)
                    {
                        RO = Projectile.oldRot[a];
                        if (Projectile.velocity.X < 0)
                        {
                            sprite = SpriteEffects.FlipHorizontally;
                            RO += MathHelper.Pi;
                        }
                        Main.spriteBatch.Draw(texture, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, rectangle, lightColor*0.15f * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), RO, rectangle.Size() / 2, Projectile.scale * Projectile.DProj().Times[2], sprite, 0);
                    }
                }
            }
            else
            if (Skill>0)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, new Color(155, 0, 0, 0), RO, rectangle.Size() / 2, Projectile.scale, sprite, 0);
                for (int a =0;a< Projectile.oldPos.Length;a++)
                {
                    RO = Projectile.oldRot[a];
                    if (Projectile.velocity.X < 0)
                    {
                        sprite = SpriteEffects.FlipHorizontally;
                        RO += MathHelper.Pi;
                    }
                    Main.spriteBatch.Draw(texture, Projectile.oldPos[a]+Projectile.Size/2 - Main.screenPosition, rectangle, new Color(120,0,0,0) * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), RO, rectangle.Size() / 2, Projectile.scale, sprite, 0);
                }
            }
            else
            {

                for (int a = 0; a < Projectile.oldPos.Length; a++)
                {
                    RO = Projectile.oldRot[a];
                    if (Projectile.velocity.X < 0)
                    {
                        sprite = SpriteEffects.FlipHorizontally;
                        RO += MathHelper.Pi;
                    }
                    Main.spriteBatch.Draw(texture, Projectile.oldPos[a] + Projectile.Size / 2 - Main.screenPosition, rectangle, lightColor*0.15f * ((Projectile.oldPos.Length - a) / (float)Projectile.oldPos.Length), RO, rectangle.Size() / 2, Projectile.scale, sprite, 0);
                }

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
