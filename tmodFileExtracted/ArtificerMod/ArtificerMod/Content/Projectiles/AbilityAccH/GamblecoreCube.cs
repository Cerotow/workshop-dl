using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArtificerMod.Content.Projectiles.AbilityAccH
{
	public class GamblecoreCube : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Generic;
			
			Projectile.friendly = true;
			Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            Projectile.frame = (int)Projectile.ai[0];
            if (Projectile.timeLeft > 120)
            {
                Projectile.Center = owner.Center + new Vector2(0f, -80);

                int animSpd = (int)Utils.Remap(Projectile.timeLeft, 300, 120, 2, 20);
                Projectile.rotation += Utils.Remap(Projectile.timeLeft, 300, 120, 0.6f, 0.15f);

                if (++Projectile.frameCounter >= animSpd)
                {
                    Projectile.frameCounter = 0;
                    SoundEngine.PlaySound(SoundID.Mech, Projectile.Center);

                    if (++Projectile.ai[0] >= 6)
                    {
                        Projectile.ai[0] = 0;
                    }
                }
                return;
            }
            else if(Projectile.timeLeft == 120)
            {
                // Sfx
                if (Projectile.ai[0] == 0) // Fail
                {
                    SoundEngine.PlaySound(SoundID.Item16, Projectile.position);
                }
                else if (Projectile.ai[0] == 4) // Platinum!
                {
                    SoundEngine.PlaySound(SoundID.AchievementComplete, Projectile.position);
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.ResearchComplete, Projectile.position);
                }
                Projectile.tileCollide = true;

                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.velocity = Projectile.Center.DirectionTo(Main.MouseWorld) * 12f;
                    Projectile.netUpdate = true;
                }
                
            }
            
            Projectile.rotation += 0.15f;
            if(Projectile.velocity.Y < 16f)
            {
                Projectile.velocity.Y += 0.2f;
            }
            Projectile.velocity.X *= 0.999f;
        }

		public override void OnKill(int timeLeft)
		{
            if (Projectile.ai[0] == 0) // Fail
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

                for (int i = 0; i < 30; i++)
                {
                    var smoke = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FartInAJar, 0f, 0f, 100, default, 2f);
                    smoke.noGravity = Main.rand.NextBool(3);
                    smoke.velocity *= Main.rand.NextFloat(3f, 5f);
                }

                var smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.FartCloud1, GoreID.FartCloud3 + 1));
                smokeGore.velocity *= 0.5f;
                smokeGore.velocity += Vector2.One;
                smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.FartCloud1, GoreID.FartCloud3 + 1));
                smokeGore.velocity *= 0.5f;
                smokeGore.velocity.X -= 1f;
                smokeGore.velocity.Y += 1f;
                smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.FartCloud1, GoreID.FartCloud3 + 1));
                smokeGore.velocity *= 0.5f;
                smokeGore.velocity.X += 1f;
                smokeGore.velocity.Y -= 1f;
                smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.FartCloud1, GoreID.FartCloud3 + 1));
                smokeGore.velocity *= 0.5f;
                smokeGore.velocity -= Vector2.One;

                Projectile.Resize(80, 80);
                Projectile.maxPenetrate = Projectile.penetrate = -1;
                Projectile.tileCollide = false;
                Projectile.Damage();
                return;
            }
            SoundEngine.PlaySound(SoundID.Coins, Projectile.Center);
            if (Projectile.ai[0] == 5) // Random
            {
                // Dust FX              
                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, Main.rand.Next(DustID.CopperCoin, DustID.PlatinumCoin + 1));
                }

                if (Main.myPlayer == Projectile.owner)
                {
                    for (int i = 0; i < 10; i++) // Shoot coin projectiles
                    {
                        int coinProjTier = 1;
                        if (Main.rand.NextBool(100))
                        {
                            coinProjTier = 4;
                        }
                        else if (Main.rand.NextBool(10))
                        {
                            coinProjTier = 3;
                        }
                        else if (!Main.rand.NextBool(3))
                        {
                            coinProjTier = 2;
                        }

                        int projType = coinProjTier switch
                        {
                            2 => ModContent.ProjectileType<GamblecoreSilver>(),
                            3 => ModContent.ProjectileType<GamblecoreGold>(),
                            4 => ModContent.ProjectileType<GamblecorePlatinum>(),
                            _ => ModContent.ProjectileType<GamblecoreCopper>(),
                        };
                        int coinDmg = coinProjTier switch
                        {
                            2 => Projectile.damage,
                            3 => Projectile.damage * 5,
                            4 => Projectile.damage * 20,
                            _ => Projectile.damage / 2,
                        };

                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center,
                            Main.rand.NextFloat(1f, 2f) * Main.rand.NextVector2CircularEdge(7f, 7f),
                            projType, coinDmg, Projectile.knockBack / 2f, Projectile.owner);
                    }

                    // Drop a few extra coins
                    int coinDropTier = 1;
                    if(Main.rand.NextBool(100))
                    {
                        coinDropTier = 4;
                    }
                    else if (Main.rand.NextBool(10))
                    {
                        coinDropTier = 3;
                    }
                    else if (!Main.rand.NextBool(3))
                    {
                        coinDropTier = 2;
                    }

                    int coinType = coinDropTier switch
                    {
                        2 => ItemID.SilverCoin,
                        3 => ItemID.GoldCoin,
                        4 => ItemID.PlatinumCoin,
                        _ => ItemID.CopperCoin,
                    };
                    int coinCount = coinDropTier switch
                    {
                        2 => Main.rand.Next(1, 15), // 1-15 silver
                        3 => Main.rand.Next(1, 6), // 1-5 gold
                        4 => Main.rand.NextBool(4) ? 2 : 1, // 1 or 2 platinum (25% chance)
                        _ => Main.rand.Next(1, 100), // 1-99 copper
                    };
                    int item = Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.getRect(), coinType, coinCount);
                    if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
                    {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
                    }
                }

                return;
            }

            // Dust FX
            int dustType = (int)Projectile.ai[0] switch
            {               
                2 => DustID.SilverCoin,
                3 => DustID.GoldCoin,
                4 => DustID.PlatinumCoin,
                _ => DustID.CopperCoin,
            };
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType);
            }

            if (Main.myPlayer == Projectile.owner)
            {
                int projType = (int)Projectile.ai[0] switch
                {
                    2 => ModContent.ProjectileType<GamblecoreSilver>(),
                    3 => ModContent.ProjectileType<GamblecoreGold>(),
                    4 => ModContent.ProjectileType<GamblecorePlatinum>(),
                    _ => ModContent.ProjectileType<GamblecoreCopper>(),
                };            
                int coinDmg = (int)Projectile.ai[0] switch
                {
                    2 => Projectile.damage,
                    3 => Projectile.damage * 5,
                    4 => Projectile.damage * 20,
                    _ => Projectile.damage / 2,
                };

                for (int i = 0; i < 10; i++) // Shoot coin projectiles
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center,
                        Main.rand.NextFloat(1f, 2f) * Main.rand.NextVector2CircularEdge(7f, 7f),
                        projType, coinDmg, Projectile.knockBack / 2f, Projectile.owner);
                }

                // Drop a few extra coins
                int coinType = (int)Projectile.ai[0] switch
                {
                    2 => ItemID.SilverCoin,
                    3 => ItemID.GoldCoin,
                    4 => ItemID.PlatinumCoin,
                    _ => ItemID.CopperCoin,
                };
                int coinCount = (int)Projectile.ai[0] switch
                {
                    2 => Main.rand.Next(1, 15), // 1-15 silver
                    3 => Main.rand.Next(1, 6), // 1-5 gold
                    4 => Main.rand.NextBool(4) ? 2 : 1, // 1 or 2 platinum (25% chance)
                    _ => Main.rand.Next(1, 100), // 1-99 copper
                };
                int item = Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.getRect(), coinType, coinCount);
                if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= 2f; // Direct hit with dice deals x2 base dmg
        }

        public override bool? CanDamage()
        {
            if(Projectile.timeLeft >= 120)
            {
                return false;
            }
            return null;
        }

        public override bool ShouldUpdatePosition()
        {
            return Projectile.timeLeft <= 120;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new(0, startY, texture.Width, frameHeight - 2);

            Vector2 origin = sourceRectangle.Size() / 2f;

            Color drawColor = Projectile.GetAlpha(lightColor);

            SpriteEffects spriteFX = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteFX, 0);

            return false;
        }
    }
}