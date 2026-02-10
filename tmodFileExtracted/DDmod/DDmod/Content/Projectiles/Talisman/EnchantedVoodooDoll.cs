using DDmod.Content.Dusts;
using DDmod.Modkey;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Talisman
{
    public class EnchantedVoodooDoll : Talismans
    { 
        public static Asset<Texture2D> SoulChains;
        public override void Load()
        {
            SoulChains = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Talisman/SoulChains");
        }
        public override void SetStaticDefaults()
        {
        }

        public override void PreUse()
        {
            Player player = Main.player[Projectile.owner];
            //目标
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            //巫毒娃娃
            NPC VoodooDoll = Main.npc[(int)Projectile.ai[1]];
            if ((!VoodooDoll.active || VoodooDoll.type != ModContent.NPCType<EnchantedVoodooDollNPC>()) && NewNPC)
            {
                Projectile.ai[1] = NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, ModContent.NPCType<EnchantedVoodooDollNPC>(), 0, Projectile.owner);
                NewNPC = true;
            }
            /*
            if (Main.netMode == 1)
                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, (int)Projectile.ai[1]);*/
            Projectile.velocity = (npc.Center - Projectile.Center).PerfectNormalize() * 0.2F;
            Projectile.netUpdate = true;
        }
        public override bool PreEnd()
        {
            Player player = Main.player[Projectile.owner];
            //目标
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            //巫毒娃娃
            NPC VoodooDoll = Main.npc[(int)Projectile.ai[1]];
            if (Projectile.owner == Main.myPlayer)
            {
                if (player.TPlayer().TalismanTimes <= 0 || Projectile.Tproj().TalismanLife <= 0 || !npc.active || !VoodooDoll.active)
                {
                    player.TPlayer().TalismanTimes = 0;
                    Projectile.DProj().Bool[0] = false;
                    Projectile.Tproj().TalismanLife = 0;
                    Projectile.Tproj().MaxTalismanLife = 0;
                    NewNPC = false;
                    Projectile.netUpdate = true;
                }
            }
                return false;
        }
        public override bool Use()
        {
            Player player = Main.player[Projectile.owner];
            if (player.TPlayer().TalismanCD >= player.TPlayer().MaxTalismanCD && ModkeySetup.TalismanKey.JustPressed && Projectile.owner == Main.myPlayer && NPCdirection.CustomHaveGoal(Main.MouseWorld, 100, -1, true)&& !player.HasBuff(23))
            {
                NPC npc = NPCdirection.FindClosest(Main.MouseWorld, 100, true);
                NewNPC = true;
                Projectile.ai[0] = npc.whoAmI;
                Projectile.DProj().Bool[0] = true;
                bool Crit = Main.rand.Next(100) < Projectile.CritChance;
                Projectile.Tproj().TalismanLife = Crit ? Projectile.damage * 2 : Projectile.damage;
                Projectile.Tproj().MaxTalismanLife = Crit ? Projectile.damage * 2 : Projectile.damage;
                player.TPlayer().TalismanTimes = player.TPlayer().MaxTalismanTimes;
                player.TPlayer().TalismanCD = 0;
            }
            return false;
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = Projectile.Size / 2;
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            Color color = new Color(81, 6, 233, 0) * Projectile.DProj().Times[3];
            if(Projectile.DProj().Times[3]>0)
            {
                Projectile.DProj().Times[3] -= 0.1F;
            }
            if (Projectile.DProj().Bool[0])
            {
                for (int W = 0; W < (npc.Center - Projectile.Center).Length() / SoulChains.Width(); W++)
                {
                    if (W < (npc.Center - Projectile.Center).Length() / SoulChains.Width()-1)
                    {
                        Main.spriteBatch.Draw(SoulChains.Value, Projectile.position+ (npc.Center - Projectile.Center).PerfectNormalize() * (8 + SoulChains.Width() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(SoulChains.Width()), SoulChains.Height())), Color.White*0.2f, (npc.Center - Projectile.Center).ToRotation(), SoulChains.Size() / 2, 1, spriteEffects, 0f);
                        Main.spriteBatch.Draw(SoulChains.Value, Projectile.position+ (npc.Center - Projectile.Center).PerfectNormalize() * (8 + SoulChains.Width() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(SoulChains.Width()), SoulChains.Height())), color, (npc.Center - Projectile.Center).ToRotation(), SoulChains.Size() / 2, 1, spriteEffects, 0f);
                        Main.spriteBatch.Draw(SoulChains.Value, Projectile.position+ (npc.Center - Projectile.Center).PerfectNormalize() * (8 + SoulChains.Width() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(SoulChains.Width()), SoulChains.Height())), color, (npc.Center - Projectile.Center).ToRotation(), SoulChains.Size() / 2, 1, spriteEffects, 0f);
                        Main.spriteBatch.Draw(SoulChains.Value, Projectile.position+ (npc.Center - Projectile.Center).PerfectNormalize() * (8 + SoulChains.Width() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, (int)(SoulChains.Width()), SoulChains.Height())), color, (npc.Center - Projectile.Center).ToRotation(), SoulChains.Size() / 2, 1, spriteEffects, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(SoulChains.Value, Projectile.position + (npc.Center - Projectile.Center).PerfectNormalize() * (8 + SoulChains.Width() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0,0, (int)(SoulChains.Width()- (SoulChains.Width()-(npc.Center - Projectile.Center).Length()% SoulChains.Width())), SoulChains.Height())), Color.White * 0.2f, (npc.Center - Projectile.Center).ToRotation(), SoulChains.Size() / 2, 1, spriteEffects, 0f);
                        Main.spriteBatch.Draw(SoulChains.Value, Projectile.position + (npc.Center - Projectile.Center).PerfectNormalize() * (8 + SoulChains.Width() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0,0, (int)(SoulChains.Width()- (SoulChains.Width()-(npc.Center - Projectile.Center).Length()% SoulChains.Width())), SoulChains.Height())), color, (npc.Center - Projectile.Center).ToRotation(), SoulChains.Size() / 2, 1, spriteEffects, 0f);
                        Main.spriteBatch.Draw(SoulChains.Value, Projectile.position + (npc.Center - Projectile.Center).PerfectNormalize() * (8 + SoulChains.Width() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0,0, (int)(SoulChains.Width()- (SoulChains.Width()-(npc.Center - Projectile.Center).Length()% SoulChains.Width())), SoulChains.Height())), color, (npc.Center - Projectile.Center).ToRotation(), SoulChains.Size() / 2, 1, spriteEffects, 0f);
                        Main.spriteBatch.Draw(SoulChains.Value, Projectile.position + (npc.Center - Projectile.Center).PerfectNormalize() * (8 + SoulChains.Width() * W) + vector - Main.screenPosition, new Rectangle?(new Rectangle(0,0, (int)(SoulChains.Width()- (SoulChains.Width()-(npc.Center - Projectile.Center).Length()% SoulChains.Width())), SoulChains.Height())), color, (npc.Center - Projectile.Center).ToRotation(), SoulChains.Size() / 2, 1, spriteEffects, 0f);
                    }
                }
                return false;
            }
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
            return false;
        }
    }
}