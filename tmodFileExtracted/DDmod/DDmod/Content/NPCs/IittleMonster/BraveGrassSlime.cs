
using DDmod.Content.Dusts;
using System.Linq;
using DDmod.BossHealthBar;
using DDmod.SubworldLibraryWorld;
using Terraria;

namespace DDmod.Content.NPCs.IittleMonster
{
	public class BraveGrassSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 2;

		}

		public override void SetDefaults()
		{
			NPC.damage = 8;
			NPC.width = 32;
			NPC.height = 26;
			NPC.aiStyle = 1;
			NPC.defense = 8;
			NPC.scale = 1f;
			NPC.lifeMax = 300;
			NPC.knockBackResist = 0f;
			NPC.value = Item.buyPrice(0, 1, 0, 0);
			NPC.alpha = 50;
			NPC.DeathSound = SoundID.NPCDeath1;
			AnimationType = 1;
			NPC.HitSound = SoundID.Grass;
			NPC.NPCHB().MiniBoss = true;
			NPC.dontTakeDamage = true;
            NPC.Dnpc().Properties.Level = 2;
            //Music = MusicLoader.GetMusicSlot(Mod, "NoContent/Music/凝胶之邦");
        }
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
			});
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(23, 1, 2, 4));
			npcLoot.Add(ItemDropRule.Common(62, 1, 3, 6));
		}
		int R = 0;
		
        public override bool PreAI()
		{
			if (!NPC.Dnpc().Bool[0])
			{
				Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 2, true, 0.3f);
				if (NPC.velocity.Y == 0)
				{
					NPC.direction = (Main.player[NPC.target].Center.X - NPC.Center.X) > 0 ? 1 : -1;
				}
				else
				{
					NPC.direction = NPC.velocity.X > 0 ? 1 : -1;
				}
				NPC.Dnpc().Times[2] = NPC.direction;
				NPC.aiStyle = 0;
				NPC.ai[0] = -100;
				NPC.velocity.X = 0;
				if ((int)NPC.Dnpc().Times[4]== 199 || (int)NPC.Dnpc().Times[4]== 299)
				{
					TL = 0;
					TLTime = 0;
				}
				if ((int)NPC.Dnpc().Times[4]< 100)
				{
					NPC.Dnpc().Times[4]++;
				}
				else if ((int)(int)NPC.Dnpc().Times[4]== 100)
				{
					if (Text == "")
						NPC.velocity.Y = -5;
					NPC.Dnpc().Times[4]++;
				}
				else if ((int)NPC.Dnpc().Times[4]< 200)
                {
                    Text = DDSystem.English ? "Who are you?" : "你是谁?";
                    NPC.Dnpc().Times[4]++;
				}
				else if ((int)NPC.Dnpc().Times[4]< 300)
                {
                    Text = DDSystem.English ? "Do you want to break into my territory?" : "难道你想闯入我的地盘?";
                    NPC.Dnpc().Times[4]++;
				}
				else if ((int)NPC.Dnpc().Times[4]< 400)
                {
                    Text = DDSystem.English ? "Don't even think about it!" : "你想都别想!";
                    NPC.Dnpc().Times[4]++;
				}
				else if ((int)NPC.Dnpc().Times[4]== 400)
				{
					TL = 0;
					TLTime = 0;
					Text = "";
					NPC.Dnpc().Bool[0] = true;
					NPC.Dnpc().Times[4]= 0;
				}
			}
			else
            {
				NPC.aiStyle = 1;
            }
            if (Text != "")
            {
                NPC.NPCText(Text);
            }
            return NPC.Dnpc().Bool[0];

		}
        public override void AI()
        {
			if (NPC.Dnpc().Bool[1])
			{
				NPC.direction = (Main.player[NPC.target].Center.X - NPC.Center.X) > 0 ? -1 : 1;
				NPC.Dnpc().Times[2] = NPC.direction;
				if (NPC.velocity.Y != 0)
				{
					if (NPC.velocity.X == 0)
					{
						R = -2;
					}
                    else
                    {
						R = 0;
                    }
				}
				if (NPC.velocity.Y == 0)
				{
					NPC.Dnpc().Times[1]++;
					NPC.velocity = new Vector2(NPC.Dnpc().Times[2] * 7, -5-R);
				}
				else
				{
					NPC.velocity.X = NPC.Dnpc().Times[2] * 7;
				}
				if (NPC.position.X + NPC.width > 7000)
				{
					NPC.active = false;
				}
				if (NPC.position.X < 5000)
				{
					NPC.active = false;
				}
				NPC.ai[2]++;
				if(NPC.ai[2]>180)
                {
					NPC.alpha += 20;
                }
				if(NPC.alpha>255)
				{
					NPC.active = false;
				}
				Text = DDSystem.English ? "You bully me, woo" : "你欺负史,呜呜";
				if (Text != "")
				{
					if (Text.Length > TL)
					{
						TLTime++;
						if (TLTime % 5 == 0)
						{
							TL += ((Text.Length / 10) == 0) ? 1 : (Text.Length / 10);
						}
					}
                    else if (Text.Length < TL)
					{
						TL = Text.Length;
					}
                }
                if (Text != "")
                {
                    NPC.NPCText(Text);
                }
                return;
			}

			NPC.dontTakeDamage = false;
			NPC.Dnpc().Times[0]++;
			if (NPC.Dnpc().Times[0] > 500)
            {
				if (NPC.velocity.Y == 0)
				{
					NPC.Dnpc().Times[1]++;
					NPC.velocity = new Vector2(NPC.Dnpc().Times[2] * 7, -5);
				}
                else
                {
					NPC.velocity.X = NPC.Dnpc().Times[2] * 7;
				}
				NPC.ai[0] = -100;
				NPC.direction = (int)NPC.Dnpc().Times[2];
				Text = DDSystem.English? "Slime Shock!" : "史莱姆冲击!";
				TL = Text.Length;
                if (Text != "")
                {
                    NPC.NPCText(Text);
                }
            }
            else
			{
				Text = "";
				if (NPC.velocity.Y == 0)
				{
					NPC.direction = (Main.player[NPC.target].Center.X - NPC.Center.X) > 0 ? 1 : -1;
				}
				else
				{
					NPC.direction = NPC.velocity.X > 0 ? 1 : -1;
				}
				NPC.Dnpc().Times[2] = NPC.direction;
			}
			if (NPC.Dnpc().Times[0] > 600)
            {
				NPC.Dnpc().Times[0] = 0;
			}
			if (NPC.position.X + NPC.width > 7000)
			{
				NPC.position.X = 7000 - NPC.width;
			}
			if (NPC.position.X < 5000)
			{
				NPC.position.X = 5000;
			}
            base.AI();
		}
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			if (NPC.Dnpc().Times[0] > 500)
            {
				modifiers.SourceDamage *= 2;
            }
		}
		string Text = "";
		int TL = 0;
		int TLTime = 0;
        public override bool CheckDead()
        {
			NPC.Dnpc().Bool[1] = true;
			NPC.life = 10;
			NPC.dontTakeDamage = true;
			TL = 0;
			TLTime = 0;

			return false;
        }
		float CTextAlpha = 0;

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = TextureAssets.Npc[NPC.type].Value;
			SpriteEffects spriteEffects = 0;
			if(NPC.direction==1)
            {
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			spriteBatch.Draw(texture, NPC.Center-screenPos-new Vector2(0,4), new Rectangle?(NPC.frame), drawColor, 0, new Vector2(texture.Width,texture.Height/2)/2, NPC.scale, spriteEffects, 0);

			return false;
        }
        public override void HitEffect(HitInfo hit)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<草粒子>(), hit.HitDirection, -1f, 0, default(Color), 1f);
			}
			if (NPC.life <= 0)
			{
				for (int A = 0; A < 50; A++)
				{
					int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<草粒子>(), 0f, 0f, 10, default(Color), 2f);
					Main.dust[dust].velocity *= 0.5f;
					if (Main.rand.NextBool(2))
					{
						Main.dust[dust].scale = 0.1f;
						Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.02f;
					}
				}
			}
		}
	}
}
