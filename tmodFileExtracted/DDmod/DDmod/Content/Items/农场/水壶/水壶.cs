using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Terraria.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI.Chat;

namespace DDmod.Content.Items.农场.水壶
{
    public class 水壶 : GlobalItem
	{
		/// <summary>
		/// 水容量变动
		/// </summary>
        public float WaterVolumeGain = 1;
		/// <summary>
		/// 水类型
		/// </summary>
        public int LiquidType = 0;
		/// <summary>
		/// 当前水量
		/// </summary>
        public int WaterVolume;
		/// <summary>
		/// 最大水量
		/// </summary>
        public int MaxWaterVolume;
		/// <summary>
		/// 判定为水壶
		/// </summary>
		public bool kettle;
		/// <summary>
		/// 重铸次数
		/// </summary>
		public int Recast = 3;
		public override bool InstancePerEntity => true;
        public override void SetDefaults(Item item)
        {
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (kettle && WaterVolumeGain != 1)
			{
				if (WaterVolumeGain > 1)
				{
					int a = (int)((WaterVolumeGain - 1) * 100);
                    TooltipLine line = new TooltipLine(Mod, "KettlePrefix", "+" + a + "% "+ (DDSystem.English? "Water storage volume" : "容量"));
					line.IsModifier = true;
                    tooltips.Add(line);
				}
				else if (WaterVolumeGain < 1)
				{
					int a = (int)((1- WaterVolumeGain)*100);
                    TooltipLine line = new TooltipLine(Mod, "KettlePrefix", "-" + a + "% "+ (DDSystem.English ? "Water storage volume" : "容量"));
					line.IsModifier = true;
					line.OverrideColor = new Color(255, 0, 0);
                    tooltips.Add(line);
				}
			}
			if (kettle)
			{
			    tooltips.Add(new TooltipLine(Mod, "属性加成:", (DDSystem.English? "Remaining water volume" : "剩余水量:") + (int)((float)WaterVolume / MaxWaterVolume * 100) + "%")
				{ OverrideColor = new Color(6, 44, 255) });

			    tooltips.Add(new TooltipLine(Mod, "属性加成:", ((DDSystem.English ? "Max Water storage volume" : "最大储水量:") + ((float)MaxWaterVolume / 1000) + "L"))
				{ OverrideColor = new Color(0, 150, 255) });
			}
		}
		public override bool? PrefixChance(Item item, int pre, UnifiedRandom rand)
        {
			if (kettle&& Main.InReforgeMenu)
			{
				return false;
			}
			return base.PrefixChance(item, pre, rand);
        }
        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
			if (kettle)
			{
				switch (rand.Next(4))
				{
					case 0: return Mod.Find<ModPrefix>("大容量").Type;
					case 1: return Mod.Find<ModPrefix>("超大容量").Type;
					case 2: return Mod.Find<ModPrefix>("大").Type;
					case 3: return Mod.Find<ModPrefix>("巨大").Type;
				}
			}
            return -1;
        }
        public override void SaveData(Item item, TagCompound tag)
        {
			tag["MaxWaterVolume"] = MaxWaterVolume;
			tag["WaterVolume"] = WaterVolume;
			tag["Recast"] = Recast;
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            MaxWaterVolume = tag.Get<int>(nameof(MaxWaterVolume));
            WaterVolume = tag.Get<int>(nameof(WaterVolume));
			Recast = tag.Get<int>(nameof(Recast));
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(MaxWaterVolume);
            writer.Write(WaterVolume);
            writer.Write(Recast);
        }
        public override void NetReceive(Item item, BinaryReader reader)
		{
			MaxWaterVolume = reader.ReadInt32();
			WaterVolume = reader.ReadInt32();
			Recast = reader.ReadInt32();
		}
		public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = TextureAssets.Item[item.type].Value;
			Rectangle value = new Rectangle(0, 0, texture.Width, texture.Height/2);
			if (kettle)
			{
				if (WaterVolume == 0)
				{
					value.Y = texture.Height / 2;
					spriteBatch.Draw(texture, position, new Rectangle?(value), drawColor, 0f, origin, scale, (SpriteEffects)1, 0f);
				}
				else
				{
					value.Y = 0;
					spriteBatch.Draw(texture, position, new Rectangle?(value), drawColor, 0f, origin, scale, (SpriteEffects)1, 0f);
				}
				return false;
			}
			return true;
		}
        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[item.type].Value;
			Rectangle value = new Rectangle(0, 0, texture.Width, texture.Height / 2);
			Vector2 vector = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			if (kettle)
			{
				if (WaterVolume == 0)
				{
					value.Y = texture.Height / 2;
					spriteBatch.Draw(texture, item.Center - Main.screenPosition, new Rectangle?(value), lightColor, 0f, vector, scale, (SpriteEffects)1, 0f);
				}
				else
				{
					value.Y = 0;
					spriteBatch.Draw(texture, item.Center - Main.screenPosition, new Rectangle?(value), lightColor, 0f, vector, scale, (SpriteEffects)1, 0f);
				}
				return false;
			}
			return true;
		}
    }
	public class FillWater : ModPlayer
	{
		public override void PostItemCheck()
		{
			if (!Player.HeldItem.IsAir)
			{
				Item item = Player.HeldItem;
				bool flag18 = Player.position.X / 16f - (float)Player.tileRangeX - (float)item.tileBoost <= (float)Player.tileTargetX && (Player.position.X + (float)Player.width) / 16f + (float)Player.tileRangeX + (float)item.tileBoost - 1f >= (float)Player.tileTargetX && Player.position.Y / 16f - (float)Player.tileRangeY - (float)item.tileBoost <= (float)Player.tileTargetY && (Player.position.Y + (float)Player.height) / 16f + (float)Player.tileRangeY + (float)item.tileBoost - 2f >= (float)Player.tileTargetY;
				if (Player.noBuilding)
				{
					flag18 = false;
				}
				if (flag18 && item.GetGlobalItem<水壶>().kettle)
				{
					if (Player.toolTime == 0 && Player.itemAnimation > 0 && Player.controlUseItem)
					{
						for (int i = 0; i == 0; i++)
						{
							for (int j = 0; j == 0; j++)
							{
								if (!Main.tileAxe[(int)Main.tile[Player.tileTargetX + i, Player.tileTargetY + j].TileType] && !Main.tileHammer[(int)Main.tile[Player.tileTargetX + i, Player.tileTargetY + j].TileType])
								{
									//player.PickTile(Player.tileTargetX + i, Player.tileTargetY + j, 2);
									if (Main.tile[Player.tileTargetX + i, Player.tileTargetY + j].LiquidType == 0 &&Main.tile[Player.tileTargetX + i, Player.tileTargetY + j].LiquidAmount >= 100)
									{
										item.GetGlobalItem<水壶>().WaterVolume = item.GetGlobalItem<水壶>().MaxWaterVolume;
										item.GetGlobalItem<水壶>().LiquidType = Main.tile[Player.tileTargetX + i, Player.tileTargetY + j].LiquidType;
									}
								}
							}
						}
						Player.itemTime = (int)((float)item.useTime * Player.pickSpeed);
						Player.poundRelease = false;
					}
					if (Player.releaseUseItem)
					{
						Player.poundRelease = true;
					}
				}
			}
		}
    }
    public class 水壶Proj : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.coldDamage = true;
            Main.projFrames[Projectile.type] = 2;
        }
        float A;
        int V;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead;
            if (channeling)
            {
                if (!Projectile.DProj().Bool[0])
                {
                    Projectile.DProj().Bool[1] = (player.Dplayer().MouseWorld.X - player.Center.X) < 0;
                    Projectile.DProj().Bool[0] = true;
                }
            }
            else
            {
                Projectile.Kill();
            }
            if(Projectile.DProj().Bool[1])
            {
                Projectile.spriteDirection = 0;
            }
            else
            {
                Projectile.spriteDirection = 1;

            }
            if (Projectile.spriteDirection == 1)
            {
                Projectile.Center = player.MountedCenter + new Vector2(14, 2);
                Projectile.position.Y += Main.player[Projectile.owner].gfxOffY;
                if (A < 0.5f)
                {
                    A += 0.01f;
                }
                Projectile.rotation = A;
            }
            else
            {
                Projectile.Center = player.MountedCenter+new Vector2(-14,2);
                Projectile.position.Y += Main.player[Projectile.owner].gfxOffY;
                if (A > -0.5f)
                {
                    A -= 0.01f;
                }
                Projectile.rotation = A;
            }
            Item item = player.HeldItem;
            if (Main.netMode!=2&&Main.myPlayer == Projectile.owner)
            {
                Texture2D texture = TextureAssets.Item[item.type].Value;
                V++;
                if (Projectile.direction == 1)
                {
                    if (A >= 0.5f)
                    {
                        Vector2 rotation = new Vector2(texture.Width*0.8F * Projectile.scale, -6f).RotatedBy(Projectile.rotation);
                        if (V % 2 == 0 && item.GetGlobalItem<水壶>().WaterVolume > 0)
                        {
                            int A = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + rotation, new Vector2(1F, 3), ModContent.ProjectileType<水滴>(), 0, 0, Projectile.owner);
                            Main.projectile[A].scale = Projectile.scale;
                            item.GetGlobalItem<水壶>().WaterVolume--;
                        }
                    }
                }
                else
                {
                    if (A <= -0.5f)
                    {
                        Vector2 rotation = new Vector2(texture.Width*0.8F*Projectile.scale,6f).RotatedBy(Projectile.rotation);
                        if (V % 2 == 0 && item.GetGlobalItem<水壶>().WaterVolume > 0)
                        {
                            int A = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center - rotation, new Vector2(-1F, 3), ModContent.ProjectileType<水滴>(), 0, 0, 0, Projectile.owner);
                            Main.projectile[A].scale = Projectile.scale;
                            item.GetGlobalItem<水壶>().WaterVolume--;
                        }
                    }
                }
            }
            if (item.GetGlobalItem<水壶>().WaterVolume <= 0)
            {
                Projectile.frame = 1;
            }
            else
            {
                Projectile.frame = 0;
            }
            //projectile.rotation = projectile.velocity.ToRotation();
            Projectile.spriteDirection = Projectile.direction;

            Projectile.timeLeft = 5;
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 5;
            player.itemAnimation = 5;
            player.itemRotation = Projectile.direction;
            Projectile.ProjScale();
            player.fullRotation = 0;
            //projectile.width = (int)(32 * projectile.scale);
            //projectile.height = (int)(24* projectile.scale);
            return false;
        }
        public float AngularDamageFactor
        {
            get
            {
                return Projectile.ai[1];
            }
            set
            {
                Projectile.ai[1] = value;
            }
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            SpriteEffects spriteEffects = (SpriteEffects)((Projectile.direction == -1) ? 1 : 0);
            if (spriteEffects == (SpriteEffects)1)
            {
                Main.spriteBatch.Draw(TextureAssets.Item[player.HeldItem.type].Value, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, TextureAssets.Item[player.HeldItem.type].Value.Height / 2 * Projectile.frame, TextureAssets.Item[player.HeldItem.type].Value.Width, TextureAssets.Item[player.HeldItem.type].Value.Height / 2)), lightColor, Projectile.rotation, new Vector2((float)(TextureAssets.Item[player.HeldItem.type].Value.Width / 1.25F), (float)(TextureAssets.Item[player.HeldItem.type].Value.Height / 4)), Projectile.scale, spriteEffects, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(TextureAssets.Item[player.HeldItem.type].Value, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, TextureAssets.Item[player.HeldItem.type].Value.Height / 2 * Projectile.frame, TextureAssets.Item[player.HeldItem.type].Value.Width, TextureAssets.Item[player.HeldItem.type].Value.Height / 2)), lightColor, Projectile.rotation, new Vector2((float)(TextureAssets.Item[player.HeldItem.type].Value.Width / 6F), (float)(TextureAssets.Item[player.HeldItem.type].Value.Height / 4)), Projectile.scale, spriteEffects, 0f);
            }
            return false;
        }
    }
}
