using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using DDmod.Content.Items.Series.仙人掌;
using DDmod.Content.Dusts;
using Terraria.ModLoader.Utilities;
using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.NPCs.IittleMonster.旗子;
using DDmod.Content.Tiles.绿岩;
using DDmod.Content.Biome;

namespace DDmod.Content.NPCs.IittleMonster.绿岩
{
	public class 绿岩监控无人机 : ModNPC
	{
		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
            DGlobalNPC.IgnoreTile[NPC.type] = true;
            NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
        }

		public override void SetDefaults()
        {
            NPC.damage = 0;
			NPC.width = 40;
			NPC.height = 40;
			NPC.aiStyle = -1;
			NPC.defense = 6;
			NPC.scale = 1f;
			NPC.lifeMax = 200;
			NPC.knockBackResist = 0.3f;
			NPC.value = 0;
			NPC.alpha = 255;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
			NPC.noGravity = true;
            NPC.Dnpc().Neutrality = true;
            NPC.Dnpc().Properties.Iron = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<绿岩实验室>().Type };
            NPC.Dnpc().Properties.Level = 2;
            //Banner = NPC.type;
            //BannerItem = ModContent.ItemType<绿岩无人机旗>();
        }
        public override void AI()
        {
            if (NPC.alpha > 0)
            {
                NPC.alpha -= 3;
                NPC.velocity.X *= 0.1F;
                NPC.Dnpc().Bool[0] = NPC.velocity.X > 0;
            }
            if (NPC.Dnpc().vector[0] == Vector2.Zero)
            {
                if (NPC.Dnpc().TETile != Point16.Zero)
                {
                    NPC.Dnpc().vector[0] = NPC.Dnpc().TETile.ToVector2() * 16 + new Vector2(32, 0);
                }
                else
                {
                    NPC.Dnpc().vector[0] = NPC.Center;
                }
            }

            //中立
            if (!NPC.Dnpc().Bool[2] || NPC.target == -1)
            {
                int D = -1;
                NPC.spriteDirection = 0;
                if (NPC.velocity.X > 0)
                {
                    NPC.spriteDirection = 1;
                    D = 1;

                }
                int T = 0;
                for (int A = 1; A < 8; A++)
                {
                    if (Main.tile[(int)NPC.Center.X / 16 + A * D, (int)NPC.Center.Y / 16 + A].HasTile && DDHelper.SolidTile(Main.tile[(int)NPC.Center.X / 16 + A * D, (int)NPC.Center.Y / 16 + A], false, false))
                    {
                        break;
                    }
                    T = A;
                }
                if (NPC.ai[2] < 2 * ((float)T / 8))
                {
                    NPC.ai[2] += 0.02F;
                }
                else
                {

                    NPC.ai[2] -= 0.02F;
                }
                NPC.target = -1;
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && !Main.player[i].dead && !Main.player[i].ghost)
                    {
                        if ((Main.player[i].Center - NPC.Center).Length() > 12 && (Main.player[i].Center - NPC.Center).Length() < 129 * NPC.ai[2])
                        {
                            if (DDHelper.SpecifyDirection(NPC.velocity.X > 0 ? NPC.rotation + (MathHelper.PiOver4) : NPC.rotation - MathHelper.Pi - MathHelper.PiOver4, (Main.player[i].Center - NPC.Center).ToRotation(), 0.175F))
                            {
                                if (Collision.CanHitLine(NPC.Center, 1, 1, Main.player[i].position, Main.player[i].width, Main.player[i].height))
                                {
                                    NPC.target = i;
                                    NPC.Dnpc().Bool[2] = true;

                                    CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 1, 1), new Color(0, 255, 0), "!!!");
                                }
                                break;
                            }
                        }
                    }
                }
                NPC.Dnpc().Bool[1] = false;

                NPC.chaseable = false;
                NPC.Dnpc().Neutrality = true;
                NPC.velocity.Y += NPC.ai[0] / 10;
                NPC.noTileCollide = false;
                if (NPC.Dnpc().vector[0] != Vector2.Zero)
                {
                    if (NPC.Dnpc().vector[0].X - NPC.Center.X > 200)
                    {
                        NPC.Dnpc().Bool[0] = true;
                    }
                    else
                    if (NPC.Dnpc().vector[0].X - NPC.Center.X < -200)
                    {
                        NPC.Dnpc().Bool[0] = false;
                    }
                    bool TileCollision = Collision.SolidCollision(new Vector2(NPC.position.X, (NPC.position.Y)), NPC.width + 16, NPC.height);
                    bool TileCollision2 = Collision.SolidCollision(new Vector2(NPC.position.X - 16, (NPC.position.Y)), NPC.width + 16, NPC.height);
                    if (NPC.Dnpc().Bool[0])
                    {
                        if (NPC.velocity.X < 0.5f)
                        {
                            NPC.velocity.X += 0.01F;
                        }
                        if (TileCollision)
                        {
                            NPC.Dnpc().Bool[0] = false;
                        }
                    }
                    else
                    {

                        if (NPC.velocity.X > -0.5f)
                        {
                            NPC.velocity.X -= 0.01F;
                        }
                        if (TileCollision2)
                        {
                            NPC.Dnpc().Bool[0] = true;
                        }
                    }
                    if ((NPC.Dnpc().vector[0].Y - 50) - (NPC.Center.Y) < 0)
                    {
                        if (NPC.velocity.Y > -1)
                        {
                            NPC.velocity.Y -= 0.1F;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.Y < 1)
                        {
                            NPC.velocity.Y += 0.1F;
                        }
                    }
                }
            }
            else
            {
                if (NPC.ai[2] > 1)
                {
                    NPC.ai[2] -= 0.05F;
                }
                else
                {
                    NPC.ai[2] += 0.05F;

                }
                Player player = Main.player[NPC.target];
                //激怒
                NPC.chaseable = true;
                NPC.Dnpc().Neutrality = false;
                NPC.Dnpc().Bool[1] = true;
                Vector2 vector = player.Center - NPC.Center;
                if (vector.X > 0)
                {
                    vector = player.Center - new Vector2(50, 50) - NPC.Center;
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * vector.Length() / 10) / 21;
                }
                else
                {
                    vector = player.Center + new Vector2(50, -50) - NPC.Center;
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * vector.Length() / 10) / 21;
                }
                vector = player.Center - NPC.Center;
                NPC.spriteDirection = 0;
                if (vector.X > 0)
                {
                    NPC.spriteDirection = 1;
                }
                NPC.noTileCollide = true;
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && (Main.npc[A].type == ModContent.NPCType<绿岩侦察机>() || Main.npc[A].type == NPC.type))
                    {
                        if ((Main.npc[A].Center - NPC.Center).Length() < 1000)
                        {
                            Main.npc[A].target = NPC.target;
                            Main.npc[A].noTileCollide = true;
                            Main.npc[A].Dnpc().Bool[2] = true;
                        }
                    }
                }
                if (Main.rand.NextBool(600))
                    CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 1, 1), new Color(0, 255, 0), "!");
                if (player.dead || (player.Center - NPC.Center).Length() > 1500)
                {
                    NPC.Dnpc().Bool[2] = false;

                    NPC.target = -1;
                }

            }
        }
        public override bool? CanFallThroughPlatforms()
		{
			return true;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<绿岩实验室>().ModBiomeBestiaryInfoElement),
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.绿岩监控无人机")),
            });
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
        }
		//1-5生气
		//6-8转换微笑
		//9-14微笑
		//15-18转换生气
		//19挨打
        public override void FindFrame(int frameHeight)
        {
			NPC.rotation = NPC.velocity.X * 0.03F;
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
            }
            if(NPC.frame.Y>=frameHeight*5)
            {
                NPC.frame.Y = 0;
            }
            if(NPC.Dnpc().Defaults)
            {
                NPC.alpha = 0;
            }
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            DDHelper.BackAndForth(0.9F,1F,0.002F,ref NPC.Dnpc().Times[3], ref NPC.Dnpc().Bool[3]);
            float A = (1F-NPC.alpha / 255);
			Texture2D texture = TextureAssets.Npc[NPC.type].Value;
			SpriteEffects sprite = 0;
			Rectangle rectangle = NPC.frame;
            rectangle.Width = texture.Width/2;

            if (NPC.spriteDirection == 1)
			{
				sprite = SpriteEffects.FlipHorizontally;
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, drawColor * A, NPC.rotation, rectangle.Size() / 2, NPC.scale, sprite, 0);
			rectangle.X += rectangle.Width;
            spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, Color.White * A, NPC.rotation, rectangle.Size() / 2, NPC.scale, sprite, 0);
            texture = DDTextures.Scanning3.Value;
            float SC = NPC.ai[2]/2* NPC.Dnpc().Times[3];
            if (sprite == SpriteEffects.FlipHorizontally)
            {
                spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(6, 6).RotatedBy(NPC.rotation)*NPC.scale, null, new Color(100, 255, 100, 0)* A * NPC.Dnpc().Times[3], NPC.rotation + MathHelper.PiOver2 + MathHelper.PiOver4, texture.Size() / 2, SC * NPC.scale, sprite, 0);
               spriteBatch.Draw(DDTextures.VoidStar.Value, NPC.Center - screenPos + new Vector2(6,6).RotatedBy(NPC.rotation)*NPC.scale, null, new Color(20, 155, 20, 150) * A, NPC.rotation, DDTextures.VoidStar.Size() / 2, NPC.scale/4, sprite, 0);
                spriteBatch.Draw(DDTextures.VoidStar.Value, NPC.Center - screenPos + new Vector2(6,6).RotatedBy(NPC.rotation)*NPC.scale, null, new Color(20, 155, 20, 150) * A, NPC.rotation, DDTextures.VoidStar.Size() / 2, NPC.scale/4, sprite, 0);
            }
            else
            {
                spriteBatch.Draw(texture, NPC.Center - screenPos+ new Vector2(-6,6).RotatedBy(NPC.rotation) * NPC.scale, null, new Color(100, 255, 100, 0) * A * NPC.Dnpc().Times[3], NPC.rotation - MathHelper.PiOver2 - MathHelper.PiOver4, texture.Size() / 2, SC * NPC.scale, sprite, 0);
                spriteBatch.Draw(DDTextures.VoidStar.Value, NPC.Center - screenPos+ new Vector2(-6, 6).RotatedBy(NPC.rotation) * NPC.scale, null, new Color(20, 155, 20, 150) * A, NPC.rotation, DDTextures.VoidStar.Size() / 2, NPC.scale/4, sprite, 0);
               spriteBatch.Draw(DDTextures.VoidStar.Value, NPC.Center - screenPos+ new Vector2(-6, 6).RotatedBy(NPC.rotation) * NPC.scale, null, new Color(20, 155, 20, 150) * A, NPC.rotation, DDTextures.VoidStar.Size() / 2, NPC.scale/4, sprite, 0);
            }
            return false;
            spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.Center - screenPos,null,Color.White, (Main.player[0].Center-NPC.Center ).RotatedBy(-0.1F).ToRotation(),new Vector2(0,DDTextures.WhitePng.Height()/2),new Vector2(120,1),0,0);
            spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.Center - screenPos,null,Color.White, (Main.player[0].Center-NPC.Center).RotatedBy(0.1F).ToRotation(),new Vector2(0,DDTextures.WhitePng.Height()/2),new Vector2(120,1),0,0);

            if (NPC.spriteDirection == 1)
            {
                spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.Center - screenPos, null, Color.Red, NPC.rotation + (MathHelper.PiOver4), new Vector2(0, DDTextures.WhitePng.Height() / 2), new Vector2(129, 1), 0, 0);
                spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.Center - screenPos, null, Color.RoyalBlue, NPC.rotation + (MathHelper.PiOver4)+0.175f, new Vector2(0, DDTextures.WhitePng.Height() / 2), new Vector2(129, 1), 0, 0);
                spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.Center - screenPos, null, Color.RoyalBlue, NPC.rotation + (MathHelper.PiOver4)-0.175f, new Vector2(0, DDTextures.WhitePng.Height() / 2), new Vector2(129, 1), 0, 0);


            }
            else
            {

                spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.Center - screenPos, null, Color.Red, NPC.rotation-MathHelper.Pi-MathHelper.PiOver4, new Vector2(0, DDTextures.WhitePng.Height() / 2), new Vector2(129, 1), 0, 0);
                spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.Center - screenPos, null, Color.RoyalBlue, NPC.rotation - MathHelper.Pi - MathHelper.PiOver4 + 0.175f, new Vector2(0, DDTextures.WhitePng.Height() / 2), new Vector2(129, 1), 0, 0);
                spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.Center - screenPos, null, Color.RoyalBlue, NPC.rotation - MathHelper.Pi - MathHelper.PiOver4 - 0.175f, new Vector2(0, DDTextures.WhitePng.Height() / 2), new Vector2(129, 1), 0, 0);

            }
		}
        public override void OnHitByItem(Player player, Item item, HitInfo hit, int damageDone)
        {
        }
        public override void OnHitByProjectile(Projectile projectile, HitInfo hit, int damageDone)
        {
        }
        public override void HitEffect(HitInfo hit)
        {
            NPC.Dnpc().Times[0] = 10;
            if (!NPC.Dnpc().Bool[2])
            {
                NPC.Dnpc().Bool[2] = true;
                NPC.TargetClosest();
                CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 1, 1), new Color(0, 255, 0), "!!!");
            }
			for (int i = 0; i < 3; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<绿岩粒子>(), hit.HitDirection, -1f, 0,Color.White, 1f);
			}
			for (int i = 0; i < 2; i++)
            {
                int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<绿岩电光粒子>(), 0f, 0f, 10, Scale: Main.rand.NextFloat(0.75F, 1.25F));
                Main.dust[dust].velocity = new Vector2(1,0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))*Main.rand.NextFloat(0,4);
            }

			if (NPC.life <= 0)
			{
				if(Main.netMode!=2)
                {
                    int GoreType = Mod.Find<ModGore>("绿岩监控无人机").Type;
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(0, -2), GoreType, NPC.scale);
				}
				for (int A = 0; A < 30; A++)
				{
					int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<绿岩电光粒子>(), 0f, 0f, 10,Scale: Main.rand.NextFloat(0.75F, 1.25F));
                    Main.dust[dust].velocity = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, 6);
                }
			}
		}
	}
}
