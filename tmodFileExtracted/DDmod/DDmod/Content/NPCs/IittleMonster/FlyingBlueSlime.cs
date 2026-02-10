namespace DDmod.Content.NPCs.IittleMonster
{
    public class FlyingBlueSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Flying Blue Slime");
           //DisplayName.AddTranslation(7, "飞翔蓝史莱姆");
            Main.npcFrameCount[NPC.type] = 8;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 20;
            NPC.damage = 15;
            NPC.defense = 2;
            NPC.knockBackResist = 1f;
            NPC.width = 40;
            NPC.height = 40;
            NPC.alpha = 50;
            NPC.value = Item.buyPrice(0, 0, 50, 0);
            NPC.npcSlots = 0f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.Item1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.buffImmune[24] = true;
            NPC.netAlways = true;
            NPC.Dnpc().LifeUP = false;
            NPC.Dnpc().Properties.Gel=true;
            if (Main.hardMode)
            {
                NPC.damage = 50;
                NPC.lifeMax = 300;
                NPC.defense = 10;
            }
        }
        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

                new FlavorTextBestiaryInfoElement(DDSystem.English?"Elite slimes obedient to King Slime will fly to the enemy's head for a ranged attack":"听命于史莱姆王的精锐史莱姆,会飞到敌人的头部进行远程攻击。")
            });
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 2)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y = NPC.frame.Y + 58;
            }
            if (NPC.frame.Y >= 464)
            {
                NPC.frame.Y = 0;
            }
        }
        public override void AI()
        {
            float spacing = NPC.width * NPC.scale * 1.5f;
            float idleAccel = 0.2f;
            for (int k = 0; k < 200; k++)
            {
                NPC npc2 = Main.npc[k];
                if (k != NPC.whoAmI && npc2.active && npc2.type == NPC.type && Math.Abs(NPC.position.X - npc2.position.X) + Math.Abs(NPC.position.Y - npc2.position.Y) < spacing)
                {
                    if (NPC.position.X < Main.npc[k].position.X)
                    {
                        NPC.velocity.X -= idleAccel;
                    }
                    else
                    {
                        NPC.velocity.X += idleAccel;
                    }
                    if (NPC.position.Y < Main.npc[k].position.Y)
                    {
                        NPC.velocity.Y -= idleAccel;
                    }
                    else
                    {
                        NPC.velocity.Y += idleAccel;
                    }
                }
            }
            NPC.TargetClosest(true);
            if (Main.player[NPC.target].position.X > NPC.position.X)
            {
                NPC.spriteDirection = 1;
            }
            else
            {
                NPC.spriteDirection = 0;
            }
            if (NPC.spriteDirection == 0 && NPC.velocity.X > -8f)
            {
                NPC.velocity.X = NPC.velocity.X - 0.4f;
                if (NPC.velocity.X > 4f)
                {
                    NPC.velocity.X = NPC.velocity.X - 0.4f;
                }
                else if (NPC.velocity.X > 0f)
                {
                    NPC.velocity.X = NPC.velocity.X + 0.16f;
                }
                if (NPC.velocity.X < -8f)
                {
                    NPC.velocity.X = -8f;
                }
            }
            else if (NPC.spriteDirection == 1 && NPC.velocity.X < 8f)
            {
                NPC.velocity.X = NPC.velocity.X + 0.4f;
                if (NPC.velocity.X < -4f)
                {
                    NPC.velocity.X = NPC.velocity.X + 0.4f;
                }
                else if (NPC.velocity.X < 0f)
                {
                    NPC.velocity.X = NPC.velocity.X - 0.16f;
                }
                if (NPC.velocity.X > 8f)
                {
                    NPC.velocity.X = 8f;
                }
            }
            if (NPC.Center.Y + 150 > Main.player[NPC.target].Center.Y && (double)NPC.velocity.Y > -4)
            {
                NPC.velocity.Y = NPC.velocity.Y - 0.16f;
                if ((double)NPC.velocity.Y < -3)
                {
                    NPC.velocity.Y = -3f;
                }
            }
            else if (NPC.Center.Y + 150 < Main.player[NPC.target].Center.Y && (double)NPC.velocity.Y < 3)
            {
                NPC.velocity.Y = NPC.velocity.Y + 0.08f;
                if (NPC.velocity.Y > 1.5)
                {
                    NPC.velocity.Y = 1.5f;
                }
            }
            NPC.ai[1]++;
            if (NPC.ai[1] % 30 == 0)
            {
                Vector2 vector = new Vector2(NPC.Center.X, NPC.Center.Y);
                float num = (float)Math.Atan2(vector.Y - (Main.player[NPC.target].position.Y + Main.player[NPC.target].height * 0.5f) + 250, vector.X - (Main.player[NPC.target].position.X + Main.player[NPC.target].width * 0.5f));
                //npc.velocity.X = (float)(Math.Cos(num) * 5.0) * -1f;
                //npc.velocity.Y = (float)(Math.Sin(num) * 5.0) * -1f;
            }

            if (Main.rand.Next(300) == 2)
            {
                Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
                direction.Normalize();
                direction.X *= 15f;
                direction.Y *= 15f;
                int amountOfProjectiles = Main.rand.Next(3, 5);
                for (int i = 0; i < amountOfProjectiles; i++)
                {
                    float A = Main.rand.Next(-150, 150) * 0.01f;
                    float B = Main.rand.Next(-150, 150) * 0.01f;
                    int damage = 10;
                    if (Main.hardMode) damage = 60;
                    NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ProjectileID.SpikedSlimeSpike, damage, 1f, Main.myPlayer, 0f, 0f);
                }
            }

        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            if (Main.rand.Next(500) == 0)
            {
                //Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("传奇凝胶").Type);
            }
        }
        public override bool CheckDead()
        {
            int targetX = (int)NPC.Center.X;
            int targetY = (int)NPC.Center.Y;
            NewNPC(NPC.GetSource_FromAI(), targetX, targetY - 10, 535, 0);
            return true;
        }
    }
}