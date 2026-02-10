using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;
using DDmod.Modkey;
using DDmod.Players;
using System.Diagnostics;
using Terraria;

namespace DDmod.Content.Projectiles.BattlePets
{
    public abstract class BattlePetsProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }
        public float Time;
        public float Skill;
        public float Skill2;
        public bool Fight;
        public override void SendExtraAI(BinaryWriter writer)
        {
            
            Player player = Projectile.Player();
            writer.Write(player.whoAmI);
            writer.Write(Skill);
            writer.Write(Skill2);
            writer.Write(Fight);
            writer.Write(Time);
            writer.Write(player.Dplayer().FightPets);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Player player = Main.player[reader.ReadInt32()];
            Skill = reader.ReadFloat();
            Skill2 = reader.ReadFloat();
            Fight = reader.ReadBoolean();
            Time = reader.ReadFloat();
            player.Dplayer().FightPets = reader.ReadInt32();
        }

        public virtual void SetDef()
        {

        }
        public float Magnification;
        public int ExtraUpdates;
        public int Comeback = 600;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Default;
            //Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            SetDef();
        }
        public virtual int Width() => 2;
        public virtual int Height() => 2;
        public int OldVariation;
        //进化等级F
        public int Variation
        {
            get
            {
                return (int)Projectile.DProj().Times[4];
            }
            set
            {
                Projectile.DProj().Times[4] = value;
            }
        }
        public virtual void UP()
        {
            NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<UPProj>(), 0, 0, -1);
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.localAI[0] = 20;
            Player player = Main.player[Projectile.owner];
            if (player.Dplayer().FightPets >= 0)
            {
                Players.BattlePets battle = player.Dplayer().Bpets[player.Dplayer().FightPets];

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
        public virtual void Hit(Player player, Players.BattlePets battle)
        {

        }
        public override bool? CanDamage()
        {
            if (!Fight)
            {
                return false;
            }
            return base.CanDamage();
        }
        int EUP = 0;
        float XL = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CheckActive(player);
            player.Dplayer().ownedFightPets = Projectile.whoAmI;
            if (player.Dplayer().FightPets >= 0)
            {
                Players.BattlePets battle = player.Dplayer().Bpets[player.Dplayer().FightPets];
                if (battle == null)
                {
                    return;
                }
                if (Main.myPlayer == Projectile.owner && ModkeySetup.BattlePetsKey.JustPressed && EUP <= 0)
                {
                    EUP = 60;
                    Fight = !Fight;
                    if (!Fight)
                    {
                        int A = CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 1, 1), Color.White, Language.GetTextValue("Mods.DDmod.BattlePetText.待命"));
                    }
                    else if (!battle.Wounded)
                    {
                       int A = CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 1, 1), Color.White, Language.GetTextValue("Mods.DDmod.BattlePetText.战斗"));
                    } 
                    else
                    {
                        Fight = false;
                        UITextDraw.NewText(new Vector2(Main.screenWidth/2, Main.screenHeight/2), Language.GetTextValue("Mods.DDmod.BattlePetUI.重伤"), 60, 0.5F);
                    }
                    Projectile.netUpdate = true;
                }
                Movement(player);
                if((player.Center-Projectile.Center).Length()>1200)
                {
                    Comeback--;
                }
                else
                {
                    Comeback = 600;
                }
                if (Comeback<=0)
                {
                    Projectile.Center = player.Center;
                }
                if (battle.Life > 0)
                {
                    if(XL<0)
                    {
                        XL = 0;
                    }
                    XL += battle.LifeMax / 50000F;
                    if (XL >= 1)
                    {
                        battle.Life += (int)XL;
                        XL -= (int)XL;
                        if (battle.Life > battle.LifeMax)
                        {
                            battle.Life = battle.LifeMax;
                        }
                    }
                }
                Animate(battle);
                VariationLevel(player, battle.Level);
                Projectile.Resize((int)(Width() * Projectile.scale), (int)(Height() * Projectile.scale));
                if (player == Main.LocalPlayer)
                {
                    if (Projectile.DProj().Bool[4])
                    {
                        UP();
                        Projectile.DProj().Bool[4] = false;
                    }
                    Projectile.damage = (int)(battle.Damage * Magnification);
                    Projectile.DProj().Magnification = Magnification;
                    Magnification = 1;
                    if (Immunity > 0)
                    {
                        Immunity--;
                    }
                    else if(Fight)
                    {
                        for (int a = 0; a < 1000; a++)
                        {
                            Projectile projectile = Main.projectile[a];
                            if (projectile.active && projectile.hostile && projectile.damage > 0)
                            {
                                if (Projectile.Colliding(projectile.getRect(), Projectile.getRect()))
                                {
                                    int Damage = projectile.damage*2;
                                    if (Main.masterMode)
                                    {
                                        Damage *= 6;
                                    }
                                    else if (Main.expertMode)
                                    {
                                        Damage *= 2;
                                    }
                                    Damage -=  battle.Defense;
                                    Damage = (int)(Damage *(1F-battle.Endurance));
                                    if(Damage<1)
                                    {
                                        Damage = 1;
                                    }
                                    Hit(player,battle);
                                    battle.Life -= Damage;

                                    CombatText.NewText(Projectile.getRect(), new Color(255, 255, 0, 255), Damage, false, true);
                                    DDmod.SyncData(DDType.Battlepets, Main.myPlayer, -1, Main.myPlayer);
                                    Immunity = 30;
                                    break;
                                }
                            }
                        }
                        for (int a = 0; a < 200; a++)
                        {
                            NPC npc = Main.npc[a];
                            int S = -1;
                            if (npc.active && npc.damage > 0 && !npc.friendly && NPCLoader.CanHitPlayer(npc,Main.LocalPlayer,ref S))
                            {
                                if (npc.getRect().Intersects(Projectile.getRect()))
                                {
                                    int Damage = npc.damage - battle.Defense;
                                    Damage = (int)(Damage * (1F - battle.Endurance));
                                    if (Damage < 1)
                                    {
                                        Damage = 1;
                                    }
                                    Hit(player, battle);
                                    battle.Life -= Damage;
                                    CombatText.NewText(npc.getRect(), new Color(255, 255, 0, 255), Damage, false, true);
                                    DDmod.SyncData(DDType.Battlepets, Main.myPlayer, -1, Main.myPlayer);
                                    Immunity = 30;
                                    break;
                                }
                            }
                        }
                    }
                    if (battle.Life <= 0)
                    {
                        Projectile.damage = 0;
                    }

                }
            }
            if (EUP>0)
            {
                EUP--;
            }
            OldVariation = Variation;
            Projectile.extraUpdates = ExtraUpdates;
            ExtraUpdates = 0;
        }
        public int Immunity;
        public virtual void CheckActive(Player player)
        {
            if (Projectile.ai[0] == player.Dplayer().FightPets)
            {
                Projectile.timeLeft = 2;
            }
        }

        public virtual void VariationLevel(Player player, int Level)
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
                Variation = 3;
            } 
        }

        public virtual void Movement(Player player)
        {
            NPC npc = NPCdirection.FindClosest(Projectile.Center,1000);
            Players.BattlePets battle = player.Dplayer().Bpets[player.Dplayer().FightPets];
            if (npc == null || battle.Life <= 0|| !Fight)
            {
                Vector2 vector = player.Center - new Vector2(40 * player.direction, 50) - Projectile.Center;
                if (battle.Life <= 0)
                {
                    vector = player.Center - Projectile.Center;
                    battle.Wounded = true;
                    battle.Life = 1;
                    Fight = false;
                }
                if (vector.Length() >= 10)
                {
                    Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 18) / 21;
                }
                else
                if (vector.Length() >= 3)
                {
                    Projectile.velocity = vector.PerfectNormalize()*3+player.velocity;
                }
                else
                {
                    /*
                    if (battle.Life <= 0)
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

                Vector2 vector = npc.Center - Projectile.Center;
                if (Projectile.localAI[0] <= 0)
                {
                    Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 8) / 21;
                }
                else
                {
                    if (Projectile.velocity.Length() < 8)
                        Projectile.velocity += Projectile.rotation.ToRotationVector2() *1;
                    else
                        Projectile.velocity *= 0.96f;
                }
            }
            Projectile.localAI[0]--;
            Projectile.rotation = Projectile.velocity.ToRotation();
            //Projectile.RotationSpeed(Projectile.velocity.ToRotation(),0.1F);
            //Projectile.Center = player.Center - new Vector2(100);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return null;
        }
        public override bool MinionContactDamage()
        {
            return true;
        }
        public virtual void Animate(Players.BattlePets battle)
        {
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
    }
}
