using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Magic.Book;
using DDmod.Modkey;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Talisman
{
    public class ShadowNecklace : Talismans
    {
        public static Asset<Texture2D> TeleportingEye;
        public static Asset<Texture2D> SoulChains;
        public override void Load()
        {
            TeleportingEye = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Talisman/TeleportingEye");
            SoulChains = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Talisman/SoulChains");
        }
        public override void SetStaticDefaults()
        {
        }
        public override void PreUse()
        {
            Player player = Main.player[Projectile.owner];
            NPC npc = NPCdirection.FindClosest(Projectile.Center, 400, false);
            if (player.TPlayer().TalismanTimes % 40 == 0 && npc != null&& Projectile.owner == Main.myPlayer)
            {
                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 40), (npc.Center - Projectile.Center).PerfectNormalize() * 2, ModContent.ProjectileType<ShadowBall>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            if (ModkeySetup.TalismanKey.JustPressed && Projectile.owner == Main.myPlayer)
            {
                player.TPlayer().TalismanTimes = 0;
                Projectile.DProj().Bool[0] = false;
                Projectile.netUpdate = true;

                for (int A = 0; A < 12; A++)
                {
                    NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 40), Vector2.One.RotatedBy(MathHelper.TwoPi / 12 * A) * 2, ModContent.ProjectileType<ShadowBall>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
                player.Center = Projectile.Center - new Vector2(0, player.height / 2);
                DDmod.SyncData(DDType.PlayerCenter, player.whoAmI, -1, player.whoAmI);
            }
            if (Main.rand.NextBool(3))
            {
                Vector2 projDirection = Utils.RotatedBy(new Vector2(0, -0.7f), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default);
                NewDust(Projectile.position, Projectile.width, Projectile.height, 27, projDirection.X, projDirection.Y, 0, default, 1.2f);
            }
            Projectile.velocity = Vector2.Zero;
            Projectile.spriteDirection = 0;
        }
        public override bool Use()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.spriteDirection = player.direction;
            if (player.TPlayer().TalismanCD >= player.TPlayer().MaxTalismanCD && ModkeySetup.TalismanKey.JustPressed && Projectile.owner == Main.myPlayer && !player.HasBuff(23))
            {
                Projectile.DProj().Bool[0] = true;
                player.TPlayer().TalismanCD = 0;
                player.TPlayer().TalismanTimes = player.TPlayer().MaxTalismanTimes;
                Projectile.netUpdate = true;
            }
            return false;
        }
        public override bool PreEnd()
        {
            Player player = Main.player[Projectile.owner];

            if (player.TPlayer().TalismanTimes <= 0&&Projectile.owner == Main.myPlayer)
            {
                Projectile.Tproj().TalismanTimes = 0;
                Projectile.Tproj().TalismanCD = 0;
                Projectile.DProj().Bool[0] = false;
                Projectile.netUpdate = true;
            }
            return false;
        }
        public override bool MobileAI()
        {
            Player player = Main.player[Projectile.owner];
            //冷却好了粒子
            if (player.TPlayer().TalismanCD >= player.TPlayer().MaxTalismanCD && Main.rand.NextBool(10))
            {
                Vector2 projDirection = Utils.RotatedBy(new Vector2(0, -0.7f), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default);
                NewDust(Projectile.position, Projectile.width, Projectile.height, 27, projDirection.X, projDirection.Y, 0, default, 1.2f);
            }
            return base.MobileAI();
        }
        float[] Sc = new float[] { 0,1F/3F,1F/3F*2};
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
           
            Vector2 vector = Projectile.Size / 2;
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition+new Vector2(0, Projectile.DProj().Times[2]), null, Color.White , Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
            if (Projectile.DProj().Bool[0])
            {
                Projectile.DProj().Times[3]-=0.3f;
                int R = (int)(Projectile.Player().Center - Projectile.Center + new Vector2(0, 40)).Length() / SoulChains.Width();
                for (int W = 0; W < (Projectile.Player().Center - Projectile.Center + new Vector2(0, 40)).Length() / SoulChains.Width(); W++)
                {
                    Color color = new Color(0, 0, 0, 0);
                    int T = R / 4;
                    for (int I = 0; I < T; I++)
                    {
                        int Time = (int)(Projectile.DProj().Times[3] + (R / T * I)) % R;
                        if (Time % R == W)
                        {
                            color = new Color(81, 6, 233, 0) * 1;
                        }
                        if (Time == (W+1))
                        {
                            color = new Color(81, 6, 233, 0)*0.66F;
                        }
                        if (Time == (W-1))
                        {
                            color = new Color(81, 6, 233, 0)*0.66F;
                        }
                        if (Time == (W+2))
                        {
                            color = new Color(81, 6, 233, 0)*0.33F;
                        }
                        if (Time == (W-2))
                        {
                            color = new Color(81, 6, 233, 0)*0.33F;
                        }
                    }
                    if (W < (Projectile.Player().Center - Projectile.Center + new Vector2(0, 40)).Length() / SoulChains.Width() - 1)
                    {
                        Main.spriteBatch.Draw(SoulChains.Value, Projectile.position + (Projectile.Player().Center - Projectile.Center+new Vector2(0,40)).PerfectNormalize() * (8 + SoulChains.Width() * W) + vector - Main.screenPosition - new Vector2(0, 40), new Rectangle?(new Rectangle(0, 0, (int)(SoulChains.Width()), SoulChains.Height())), Color.White*0.15F, (Projectile.Player().Center - Projectile.Center + new Vector2(0, 40)).ToRotation(), SoulChains.Size() / 2, 1, spriteEffects, 0f);
                        Main.spriteBatch.Draw(SoulChains.Value, Projectile.position + (Projectile.Player().Center - Projectile.Center+new Vector2(0,40)).PerfectNormalize() * (8 + SoulChains.Width() * W) + vector - Main.screenPosition - new Vector2(0, 40), new Rectangle?(new Rectangle(0, 0, (int)(SoulChains.Width()), SoulChains.Height())), color, (Projectile.Player().Center - Projectile.Center + new Vector2(0, 40)).ToRotation(), SoulChains.Size() / 2, 1, spriteEffects, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(SoulChains.Value, Projectile.position + (Projectile.Player().Center - Projectile.Center + new Vector2(0, 40)).PerfectNormalize() * (8 + SoulChains.Width() * W) + vector - Main.screenPosition - new Vector2(0, 40), new Rectangle?(new Rectangle(0, 0, (int)(SoulChains.Width() - (SoulChains.Width() - (Projectile.Player().Center - Projectile.Center + new Vector2(0, 40)).Length() % SoulChains.Width())), SoulChains.Height())), Color.White * 0.15F,(Projectile.Player().Center - Projectile.Center + new Vector2(0, 40)).ToRotation(), SoulChains.Size() / 2, 1, spriteEffects, 0f);
                        Main.spriteBatch.Draw(SoulChains.Value, Projectile.position + (Projectile.Player().Center - Projectile.Center + new Vector2(0, 40)).PerfectNormalize() * (8 + SoulChains.Width() * W) + vector - Main.screenPosition - new Vector2(0, 40), new Rectangle?(new Rectangle(0, 0, (int)(SoulChains.Width() - (SoulChains.Width() - (Projectile.Player().Center - Projectile.Center + new Vector2(0, 40)).Length() % SoulChains.Width())), SoulChains.Height())), color, (Projectile.Player().Center - Projectile.Center + new Vector2(0, 40)).ToRotation(), SoulChains.Size() / 2, 1, spriteEffects, 0f);
                    }
                }
                DDHelper.BackAndForth(-10, 10, 0.2f, ref Projectile.DProj().Times[2], ref Projectile.DProj().Bool[2]);
                
                if(Sc==null|| Sc.Length<2)
                {
                    Sc = new float[] { 0,0.5F };
                }
                for(int a = 0;a<2;a++)
                {
                    Sc[a]+=0.02F;
                    if(Sc[a]>1)
                    {
                        Sc[a] = 0;
                    }
                    Main.spriteBatch.Draw(TeleportingEye.Value, Projectile.position + vector - Main.screenPosition - new Vector2(0, 40), null, new Color(81, 6, 233,30)*(1.5f- Sc[a]*1.5f), Projectile.rotation, TeleportingEye.Size() / 2, 1+Sc[a] *1.5f, spriteEffects, 0f);
                }
                Main.spriteBatch.Draw(TeleportingEye.Value, Projectile.position + vector - Main.screenPosition - new Vector2(0, 40), null, new Color(81, 6, 233, 10), Projectile.rotation, TeleportingEye.Size() / 2, 1, spriteEffects, 0f);
            }
            else
            {
                Sc = new float[] { 0, 0.5F};
                Projectile.DProj().Times[2] = 0;
                Projectile.DProj().Times[3] = 99999;
            }
            return false;
        }
    }
}