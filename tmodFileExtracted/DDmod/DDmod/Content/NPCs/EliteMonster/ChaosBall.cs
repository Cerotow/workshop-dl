using DDmod.Content.Dusts;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Projectiles.Boss.MiniBoss;
using DDmod.Content.Projectiles.Summon;
using static Terraria.GameContent.Bestiary.BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions;
using Terraria.ID;

namespace DDmod.Content.NPCs.EliteMonster
{
    public class ChaosBall : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Chaos Ball");
            //DisplayName.AddTranslation(7, "混沌球");
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                if (!BuffID.Sets.IsATagBuff[k])
                {
                    NPCID.Sets.SpecificDebuffImmunity[Type][k] = true;
                }
            }
            NPCID.Sets.ProjectileNPC[Type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 480;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.knockBackResist = 0f;
            NPC.width = 20;
            NPC.height = 20;
            NPC.scale = 0.1f;
            NPC.dontTakeDamage = true;
            NPC.npcSlots = 0f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath3;
            NPC.netAlways = true;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 50;
            NPC.Dnpc().NoDamage = true;
            NPC.Dnpc().Properties.BossLife = 1.1f;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        public override void AI()
        {
            NPC.ai[3]+=NPC.velocity.Length();
            if((NPC.ai[3]>3600*NPC.scale||NPC.life<=20)&&!NPC.dontTakeDamage)
            {
                NPC.StrikeInstantKill();
            }
            Lighting.AddLight(NPC.Center,new Color(81, 6, 233).ToVector3()/5*NPC.scale);
			NPC.TargetClosest();
			Player player = Main.player[NPC.target];
            if (NPC.ai[1] == 0)
            {
                if (NPC.scale < 12 && !NPC.Dnpc().Bool[0])
                {
                    NPC.scale += 0.05F;
                    NPC.velocity = Vector2.Zero;

                    NPC.position = NPC.Center;
                    NPC.width = (int)(20 * NPC.scale);
                    NPC.height = (int)(20 * NPC.scale);
                    NPC.Center = NPC.position;
                    NPC.position.Y -= 0.5F;
                    NPC.life = (int)(40 * NPC.scale);
                    if (NPC.scale < 11F)
                    {
                        for (int a = 0; a < 10; a++)
                        {
                            Dust dust = Main.dust[NewDust(NPC.Center + new Vector2(Main.rand.NextFloat(30, 40) * NPC.scale/2).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 1, 1, 27, 0, 0, 0, default, 1f)];
                            dust.noGravity = true;
                            dust.scale = 2.2F;
                            dust.velocity = (NPC.Center - dust.position) / Main.rand.NextFloat(6, 10);
                        }
                        if (NPC.soundDelay == 0)
                        {
                            NPC.soundDelay = 20;
                            SoundStyle sound = SoundID.Item88;
                            sound.Pitch = 0.3f;
                            sound.Volume = 4;
                            PlaySound(sound, NPC.position);
                        }
                    }
                    if (NPC.scale > 10.5F && NPC.scale < 11)
                    {
                        NPC.Dnpc().Bool[1] = true;
                        SoundStyle sound = SoundID.Item88;
                        sound.Pitch = -1F;
                        sound.Volume = 4;
                        PlaySound(sound, NPC.position);
                    }
                    Main.LocalPlayer.Dplayer().PlayerShake(5,NPC.scale);
                }
                else
                {
                    if (!NPC.Dnpc().Bool[0])
                    {
                        NPC.Dnpc().Bool[1] = true;
                        NPC.Dnpc().vector[0] = (player.Center - NPC.Center).PerfectNormalize() * 22;
                        NPC.netUpdate = true;
                    }
                    NPC.Dnpc().Bool[0] = true;
                }
            }
            else if (NPC.ai[1] == 1)
            {
                if (NPC.scale < 12 && !NPC.Dnpc().Bool[0])
                {
                    NPC.scale += 0.05F;
                    NPC.velocity = Vector2.Zero;

                    NPC.position = NPC.Center;
                    NPC.width = (int)(20 * NPC.scale);
                    NPC.height = (int)(20 * NPC.scale);
                    NPC.Center = NPC.position;
                    NPC.position.Y -= 0.5F;
                    NPC.life = (int)(40 * NPC.scale);
                    if (NPC.scale < 11F)
                    {
                        if (NPC.soundDelay == 0)
                        {
                            NPC.soundDelay = 20;
                            SoundStyle sound = SoundID.Item88;
                            sound.Pitch = 0.3f;
                            sound.Volume = 4;
                            PlaySound(sound, NPC.position);
                        }
                        for (int a = 0; a < 10; a++)
                        {
                            Dust dust = Main.dust[NewDust(NPC.Center + new Vector2(Main.rand.NextFloat(30, 40) * NPC.scale/2).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 1, 1, 27, 0, 0, 0, default, 1f)];
                            dust.noGravity = true;
                            dust.scale = 2.2F;
                            dust.velocity = (NPC.Center - dust.position) / Main.rand.NextFloat(6, 10);
                        }
                    }
                    if (NPC.scale > 10.5F && NPC.scale < 11)
                    {
                        NPC.Dnpc().Bool[1] = true;
                        SoundStyle sound = SoundID.Item88;
                        sound.Pitch = -1F;
                        sound.Volume = 4;
                        PlaySound(sound, NPC.position);
                    }
                    Main.LocalPlayer.Dplayer().PlayerShake(5, NPC.scale);
                }
                else
                {
                    if (!NPC.Dnpc().Bool[0])
                    {
                        NPC.Dnpc().Bool[1] = true;
                        NPC.Dnpc().vector[0].X = 0;
                        NPC.Dnpc().vector[0].Y = -1;
                        NPC.netUpdate = true;
                    }
                    NPC.ai[2]++;
                    if (NPC.ai[2] > 60 && NPC.ai[2] < 80)
                    {
                        NPC.Dnpc().Bool[2] = true;
                        NPC.Dnpc().Times[0] += 0.4F;
                        NPC.Dnpc().vector[0] = Vector2.Zero;
                    }
                    else
                    if (NPC.ai[2] > 100)
                    {
                        NPC.Dnpc().Times[0] -= 0.4f;
                    }

                    if (NPC.ai[2] > 120)
                    {
                        for (int a = 0; a < 24; a++)
                        {
                            NPC GenkiBomb = Main.npc[NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<ChaosBall>(), 0, 100, 2)];
                            GenkiBomb.Dnpc().vector[0] = Vector2.One.RotatedBy(MathHelper.TwoPi / 24 * a + Main.rand.NextFloat(0, 0.2F)) * 8;
                            GenkiBomb.scale = NPC.scale / 2;
                            GenkiBomb.netUpdate = true;
                        }
                        SoundStyle sound = SoundID.Item14;
                        sound.Pitch = -1f;
                        sound.Volume = 4;
                        PlaySound(sound, NPC.position);

                        NPC.active = false;
                    }
                    NPC.Dnpc().Bool[0] = true;
                }
            }
            else if (NPC.ai[1] == 2)
            {
                if (!NPC.Dnpc().Bool[0])
                {
                    NPC.life = (int)(40 * NPC.scale);
                    NPC.Dnpc().Bool[0] = true;
                }
            }
            else if (NPC.ai[1] == 3)
            {
                if (!NPC.Dnpc().Bool[0])
                {
                    NPC.life = (int)(40 * NPC.scale);
                    NPC.Dnpc().Bool[0] = true;
                }
                NPC.Dnpc().vector[0].Y += 0.2f;
            }
            else if(NPC.ai[1]==4)
            {

                if (!NPC.Dnpc().Bool[0])
                {
                    NPC.life = (int)(40 * NPC.scale);
                    NPC.Dnpc().Bool[0] = true;
                }
                Vector2 vector = (player.Center - NPC.Center).PerfectNormalize();
                NPC.ai[2]++;
                if (NPC.ai[2] > 20 && NPC.ai[2] < 60)
                {
                    NPC.Dnpc().vector[0] = (NPC.Dnpc().vector[0] * 20 + vector * 8) / 21;
                }
            }
            else
            {
                if (NPC.scale < 36 && !NPC.Dnpc().Bool[0])
                {
                    NPC.scale += 0.1F;
                    NPC.velocity = Vector2.Zero;

                    NPC.position = NPC.Center;
                    NPC.width = (int)(20 * NPC.scale);
                    NPC.height = (int)(20 * NPC.scale);
                    NPC.Center = NPC.position;
                    NPC.position.Y -= 1F;
                    NPC.life = (int)(40 * NPC.scale);
                    if (NPC.scale < 34F)
                    {
                        for (int a = 0; a < 10; a++)
                        {
                            Dust dust = Main.dust[NewDust(NPC.Center + new Vector2(Main.rand.NextFloat(30, 40) * NPC.scale/2).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 1, 1, 27, 0, 0, 0, default, 1f)];
                            dust.noGravity = true;
                            dust.scale = 2.2F;
                            dust.velocity = (NPC.Center - dust.position) / Main.rand.NextFloat(6, 10);
                        }
                        if (NPC.soundDelay == 0)
                        {
                            NPC.soundDelay = 20;
                            SoundStyle sound = SoundID.Item88;
                            sound.Pitch = 0.3f;
                            sound.Volume = 4;
                            PlaySound(sound, NPC.position);
                        }
                    }
                    if (NPC.scale > 33F && NPC.scale < 34)
                    {
                        NPC.Dnpc().Bool[1] = true;
                        SoundStyle sound = SoundID.Item88;
                        sound.Pitch = -1F;
                        sound.Volume = 4;
                        PlaySound(sound, NPC.position);
                    }
                    Main.LocalPlayer.Dplayer().PlayerShake(5, NPC.scale/3);
                }
                else
                {
                    if (!NPC.Dnpc().Bool[0])
                    {
                        NPC.Dnpc().Bool[1] = true;
                        NPC.Dnpc().vector[0] = (player.Center - NPC.Center).PerfectNormalize() * 22;
                        NPC.netUpdate = true;
                    }
                    NPC.Dnpc().Bool[0] = true;
                }
                if (!NPC.Dnpc().Bool[2])
                {
                    if (NPC.Dnpc().Bool[1])
                    {
                        NPC.Dnpc().Times[0] += 1.2f;
                        if (NPC.Dnpc().Times[0] > 1)
                        {
                            NPC.Dnpc().Bool[1] = false;
                        }
                    }
                    else if (NPC.Dnpc().Times[0] > 0)
                    {
                        NPC.Dnpc().Times[0] -= 0.6F;
                    }
                }
            }
            if (NPC.Dnpc().Bool[0])
            {
                NPC.scale = ((float)NPC.life / NPC.lifeMax) * 12;

                NPC.position = NPC.Center;
                NPC.width = (int)(20 * NPC.scale);
                NPC.height = (int)(20 * NPC.scale);
                NPC.Center = NPC.position;

                NPC.velocity = NPC.Dnpc().vector[0];

                NPC.dontTakeDamage = false;
            }
            if (!NPC.Dnpc().Bool[2]&& NPC.ai[1] != 5)
            {
                if (NPC.Dnpc().Bool[1])
                {
                    NPC.Dnpc().Times[0] += 0.8f;
                    if (NPC.Dnpc().Times[0] > 1)
                    {
                        NPC.Dnpc().Bool[1] = false;
                    }
                }
                else if (NPC.Dnpc().Times[0] > 0)
                {
                    NPC.Dnpc().Times[0] -= 0.4F;
                }
            }
            NPC.ai[0] = NPC.life/2;
            NPC.damage = (int)NPC.ai[0];
            //NewDustChangeRound((int)NPC.scale, NPC.Center, 5*NPC.scale, 27, 1, 1, true, 2.2f);
            life = NPC.life;

        }
        int life;
		public override void HitEffect(HitInfo hit)
        {
            if(hit.Damage > life)
            {
                hit.Damage = life;
            }
            NewDustChangeRound((int)hit.Damage / 2, NPC.Center, 5 * NPC.scale, 27, 4, 6, true, 2.2f);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            float sc = NPC.Dnpc().Times[0];
            if(sc>0)
            {
                sc = 0;
            }

            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (NPC.direction == 1)
            {
                spriteEffects = 0;
            }
            Color color = new Color(81, 6, 233, 0);
            Texture2D texture = DDTextures.VoidStar.Value;
            Vector2 vector = NPC.Size / 2;
            int L = NPC.oldPos.Length/36;
            L = (int)(L *NPC.scale);
            if(L<5)
            {
                L = 5;
            }
            for (int i = 0; i < L; i++)
            {
                Vector2 vector2 = NPC.oldPos[i] + vector - Main.screenPosition;
                Color Trailcolor = color * ((L - i) / (float)L / 2f);
                Main.spriteBatch.Draw(texture, vector2, null, Trailcolor, NPC.rotation, texture.Size() / 2, NPC.scale / 5f * ((L - i) / (float)L)+ sc, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, vector2, null, Trailcolor, NPC.rotation, texture.Size() / 2, NPC.scale / 5f * ((L - i) / (float)L)+ sc, spriteEffects, 0f);

            }
            Main.spriteBatch.Draw(texture, NPC.position + vector - Main.screenPosition, null, color, NPC.rotation, texture.Size() / 2, NPC.scale / 5f+ NPC.Dnpc().Times[0], spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, NPC.position + vector - Main.screenPosition, null, new Color(132, 166, 21, 0) * 0.75F, NPC.rotation, texture.Size() / 2, NPC.scale / 10 + NPC.Dnpc().Times[0], spriteEffects, 0f);

            return false;
        }
    }
}