using DDmod.Content.NPCs.TownNPC;
using DDmod.Content.Projectiles.Magic;
using DDmod.Content.Projectiles.Magic.Book;
using DDmod.Content.Projectiles.Magic.Staff;
using DDmod.NoContent.Config;
using StructureHelper;
using System.Collections.ObjectModel;
using System.Linq;
using Terraria.ID;

namespace DDmod.Content.Items
{
    public class MagicGlobalItem : GlobalItem
    {
        public bool Handheld;
        public bool HandStaff;

        /// <summary>
        /// 充能最高倍率
        /// </summary>
        public float charging = 1;
        public float ExtraMana = 1;
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Item item)
        {
            
            if (!ModContent.GetInstance<DDConfigServer>().MagicRework)
            {
                return;
            }
            //火花法杖
            if (item.type == ItemID.WandofSparking)
            {
                Staff(item, ModContent.ProjectileType<WandofSparking>());
                item.useTime = 60;
                item.useAnimation = item.useTime;
                charging = 2;
                ExtraMana = 5;
                Handheld = true;
            }
            //结霜法杖
            if (item.type == ItemID.WandofFrosting)
            {
                Staff(item, ModContent.ProjectileType<结霜法杖>());
                item.useTime = 60;
                item.useAnimation = item.useTime;
                charging = 2;
                ExtraMana = 5;
                Handheld = true;
            }
            //紫晶法杖
            if (item.type == ItemID.AmethystStaff)
            {
                Staff(item, ModContent.ProjectileType<AmethystStaff>());
                item.damage = 20;
                item.useTime = 60;
                item.useAnimation = item.useTime;
                charging = 3;
                ExtraMana = 6;
                Handheld = true;
            }
            //黄玉法杖
            if (item.type == ItemID.TopazStaff)
            {
                Staff(item, ModContent.ProjectileType<TopazStaff>());
                item.damage = 22;
                item.useTime = 60;
                item.useAnimation = item.useTime;
                charging = 3;
                ExtraMana = 6;
                Handheld = true;
            }
            //蓝宝石法杖
            if (item.type == ItemID.SapphireStaff)
            {
                Staff(item, ModContent.ProjectileType<SapphireStaff>());
                item.damage = 22;
                item.useTime = 180;
                item.useAnimation = item.useTime;
                charging = 2;
                ExtraMana = 8;
                Handheld = true;
            }
            //翡翠法杖
            if (item.type == ItemID.EmeraldStaff)
            {
                Staff(item, ModContent.ProjectileType<EmeraldStaff>());
                item.damage = 23;
                item.useTime = 180;
                item.useAnimation = item.useTime;
                charging = 2;
                ExtraMana = 8;
                Handheld = true;
            }
            //红玉法杖
            if (item.type == ItemID.RubyStaff)
            {
                Staff(item, ModContent.ProjectileType<RubyStaff>());
                item.damage = 18;
                item.useTime = 180;
                item.useAnimation = item.useTime;
                charging = 6;
                ExtraMana = 12;
                Handheld = true;
            }
            //钻石法杖
            if (item.type == ItemID.DiamondStaff)
            {
                Staff(item, ModContent.ProjectileType<DiamondStaff>());
                item.damage = 19;
                item.useTime = 180;
                item.useAnimation = item.useTime;
                charging = 6;
                ExtraMana = 12;
                Handheld = true;
            }
            //琥珀法杖
            if (item.type == ItemID.AmberStaff)
            {
                Staff(item, ModContent.ProjectileType<AmberStaff>());
                item.damage = 20;
                item.useTime = 120;
                item.useAnimation = item.useTime;
                charging = 4;
                ExtraMana = 8;
                Handheld = true;
            }
            //荆棘法杖
            if (item.type == ItemID.Vilethorn)
            {
                Staff(item, ModContent.ProjectileType<Vilethorn>());
                item.useTime = 120;
                item.useAnimation = item.useTime;
                item.shootSpeed = 8;
                charging = 3;
                ExtraMana = 10;
                Handheld = true;
                item.mana = 5;
            }
            //夜光
            if (item.type == 4952)
            {
                ItemID.Sets.BonusAttackSpeedMultiplier[4952] = 0f;
                item.mana = 14;
                item.damage = 42;
                item.useAnimation = 28;
                //item.useLimitPerAnimation = 10;
            }
            //水矢
            if (item.type == ItemID.WaterBolt)
            {
                item.damage = 30;
                Staff(item, ModContent.ProjectileType<WaterBolt>());
            }
            //恶魔锄刀
            if (item.type == ItemID.DemonScythe)
            {
                Staff(item, ModContent.ProjectileType<DemonScythe>());
            }
            //骷髅头
            if (item.type == ItemID.BookofSkulls)
            {
                Staff(item, ModContent.ProjectileType<BookofSkulls>());
            }
            //魔焰
            if (item.type == ItemID.CursedFlames)
            {
                Staff(item, ModContent.ProjectileType<CursedFlames>());
            }
            //水晶魔法书
            if (item.type == ItemID.CrystalStorm)
            {
                item.damage -= 15;
                Staff(item, ModContent.ProjectileType<CrystalStorm>());
            }
            //灵液
            if (item.type == ItemID.GoldenShower)
            {
                Staff(item, ModContent.ProjectileType<GoldenShower>());
            }
            //磁球
            if (item.type == ItemID.MagnetSphere)
            {
                Staff(item, ModContent.ProjectileType<MagnetSphere>());
            }
            //利刃台风
            if (item.type == ItemID.RazorbladeTyphoon)
            {
                item.damage = 32;
                item.useAnimation = item.useTime= 18;
                Staff(item, ModContent.ProjectileType<RazorbladeTyphoon>());
            }
            //月曜
            if (item.type == ItemID.LunarFlareBook)
            {
                Staff(item, ModContent.ProjectileType<LunarFlareBook>());
            }
            //火之花
            if (item.type == ItemID.FlowerofFire)
            {
                Staff(item, ModContent.ProjectileType<FlowerofFire>());
                Handheld = true;
            }
            //魔法飞刀
            if (item.type == ItemID.MagicDagger)
            {
                item.damage = (int)(item.damage * 2.25f);
                Staff(item, ModContent.ProjectileType<MagicDagger>());
                Handheld = true;
            }
            //太空枪
            if (item.type == 127)
            {
                item.shoot = ModContent.ProjectileType<GreenLaser>();
            }
            //激光步枪
            if (item.type == 514)
            {
                item.shoot = ModContent.ProjectileType<VioletLaser>();
            }
            //裂天剑
            if (item.type == ItemID.SkyFracture)
            {
                Staff(item, ModContent.ProjectileType<SkyFracture>());
                item.damage = 25;
                item.mana = 7;
                Handheld = true;
            }
            //海蓝权杖
            if (item.type == ItemID.AquaScepter)
            {
                item.DItem().DrawRot = -MathHelper.PiOver4;
                item.DItem().DrawDistance = 8;
                Staff(item, ModContent.ProjectileType<AquaScepter>());
                item.useAnimation = item.useTime = 10;
                item.shootSpeed = 8;
                item.damage = 18;
                item.mana = 3;
                Handheld = true;
            }
            //邪恶三叉戟
            if (item.type == ItemID.UnholyTrident)
            {
                item.useAnimation += 10;
                item.useTime += 10;
                Staff(item, ModContent.ProjectileType<UnholyTrident>());
                Handheld = true;
            }
            //寒霜法杖
            if (item.type == ItemID.FrostStaff)
            {
                Staff(item, ModContent.ProjectileType<FrostStaff>());
                item.useTime = 40;
                item.useAnimation = item.useTime;
                charging = 2;
                ExtraMana = 3;
                Handheld = true;
            }
            //荆棘法杖
            if (item.type == ItemID.NettleBurst)
            {
                item.damage += 25;
                item.useAnimation += 25;
                item.useTime += 15;
                Staff(item, ModContent.ProjectileType<NettleBurst>());
                Handheld = true;
            }
            // 魔法导弹
            if (item.type == ItemID.MagicMissile)
            {
                Staff(item, ModContent.ProjectileType<MagicMissile>());
                Handheld = true;
            }
            //烈焰火鞭
            if (item.type == ItemID.Flamelash)
            {
                Staff(item, ModContent.ProjectileType<Flamelash>());
                Handheld = true;
            }
            //彩虹法杖
            if (item.type == ItemID.RainbowRod)
            {
                Staff(item, ModContent.ProjectileType<RainbowRod>());
                Handheld = true;
            }
            //暗影束法杖
            if (item.type == ItemID.ShadowbeamStaff)
            {
                item.damage += 20;
                Staff(item, ModContent.ProjectileType<ShadowbeamStaff>());
                Handheld = true;
            }
            //欲火叉
            if (item.type == ItemID.InfernoFork)
            {
                Staff(item, ModContent.ProjectileType<InfernoFork>());
                Handheld = true;
            }
            //幽灵法杖
            if (item.type == ItemID.SpectreStaff)
            {
                item.damage += 20;
                Staff(item, ModContent.ProjectileType<SpectreStaff>());
                Handheld = true;
            }
            //蝙蝠法杖
            if (item.type == ItemID.BatScepter)
            {
                item.mana = 2;
                item.useAnimation = 8;
                item.useTime = 8;
                Staff(item, ModContent.ProjectileType<BatScepter>());
                Handheld = true;
            }
            //冰雪法杖
            if (item.type == ItemID.BlizzardStaff)
            {
                Staff(item, ModContent.ProjectileType<BlizzardStaff>());
                Handheld = true;
            }
            //剧毒法杖
            if (item.type == ItemID.PoisonStaff)
            {
                item.mana = 4;
                item.useAnimation = 5;
                item.useTime = 5;
                Staff(item, ModContent.ProjectileType<PoisonStaff>());
                Handheld = true;
            }
            //毒液法杖
            if (item.type == ItemID.VenomStaff)
            {
                item.mana = 3;
                item.useAnimation = 5;
                item.useTime = 5;
                Staff(item, ModContent.ProjectileType<VenomStaff>());
                Handheld = true;
            }
            //蜜蜂枪
            if (item.type == ItemID.BeeGun)
            {
                item.useAnimation += 7;
                item.useTime += 7;
            }
            if (item.type == ItemID.WaspGun)
            {
                item.useAnimation += 7;
                item.useTime += 7;
            }
            //陨石法杖
            if (item.type == ItemID.MeteorStaff)
            {
                Staff(item, ModContent.ProjectileType<MeteorStaff>());
                Handheld = true;
            }
            //爬藤怪法杖
            if (item.type == ItemID.ClingerStaff)
            {
                Staff(item, ModContent.ProjectileType<ClingerStaff>());
                item.mana = 4;
                Handheld = true;
            }
            //碎魔晶法杖
            if (item.type == ItemID.CrystalVileShard)
            {
                Staff(item, ModContent.ProjectileType<CrystalVileShard>());
                item.shootSpeed = 8;
                item.useTime = 120;
                item.useAnimation = item.useTime;
                charging = 5;
                ExtraMana = 20;
                item.mana = 10;
                Handheld = true;
            }
            //巫毒娃娃
            if (item.type == ItemID.ShadowFlameHexDoll)
            {
                item.useTime = 100;
                item.useAnimation = item.useTime;
                Staff(item, ModContent.ProjectileType<ShadowFlameHexDoll>());
                Handheld = true;
            }
            //水晶蛇
            if (item.type == ItemID.CrystalSerpent)
            {
                
                item.useTime = item.useAnimation= 20;
                Staff(item, ModContent.ProjectileType<CrystalSerpent>());
                Handheld = true;
            }
            //霹雳法杖
            if (item.type == ItemID.ThunderStaff)
            {
                item.useAnimation = item.useTime = 10;
                item.damage = 12;
                item.mana = 4;
                Staff(item, ModContent.ProjectileType<ThunderStaff>());
                Handheld = true;
            }
            //夺命杖
            if (item.type == ItemID.SoulDrain)
            {
                Staff(item, ModContent.ProjectileType<SoulDrain>());
                Handheld = true;
            }
        }
        public void Staff(Item item,int shoot)
        {
            item.useStyle = ItemUseStyleID.Rapier;

            item.useTime=item.useAnimation;
            item.shoot = shoot;
            item.noUseGraphic = true;
            item.channel = true;
            item.autoReuse = true;
            item.UseSound = null;
            HandStaff = true;
        }
        public override bool CanShoot(Item item, Player player)
        {
            return true;
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (HandStaff)
            {
                NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
        public override void HoldItem(Item item, Player player)
        {
            if (item.type == ItemID.SkyFracture&&player.ownedProjectileCounts[ModContent.ProjectileType<SkyFracture>()]==0)
            {
                int A = NewProjectile(item.GetSource_FromThis(), player.Center, Vector2.One, ModContent.ProjectileType<SkyFracture>(), (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(item.damage), (int)(player.GetTotalKnockback(DamageClass.Magic).ApplyTo(item.knockBack)), player.whoAmI);
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                {
                    //荆棘法杖
                    if (item.type == ItemID.NettleBurst)
                    {
                        line.Text = Language.GetTextValue("Mods.DDmod.Tooltips.NettleBurst");
                    }
                    //魔法导弹
                    if (item.type == ItemID.MagicMissile)
                    {
                        line.Text = Language.GetTextValue("Mods.DDmod.Tooltips.MagicMissile");
                    }
                    //魔法导弹
                    if (item.type == ItemID.Flamelash)
                    {
                        line.Text = Language.GetTextValue("Mods.DDmod.Tooltips.Flamelash");
                    }
                    //彩虹法杖
                    if (item.type == ItemID.RainbowRod)
                    {
                        line.Text = Language.GetTextValue("Mods.DDmod.Tooltips.RainbowRod");
                    }
                    //水晶蛇
                    if (item.type == ItemID.CrystalSerpent)
                    {
                        line.Text = Language.GetTextValue("Mods.DDmod.Tooltips.CrystalSerpent");
                    }
                    //爬藤怪法杖
                    if (item.type == ItemID.ClingerStaff)
                    {
                        line.Text = Language.GetTextValue("Mods.DDmod.Tooltips.ClingerStaff");
                    }
                }
                if (ExtraMana > 1)
                {
                    if (line.Mod == "Terraria" && line.Name == "UseMana")
                    {
                        line.Text += "(Max:" + (int)((item.mana * Main.LocalPlayer.manaCost) * ExtraMana) + ")";
                    }
                }
            }
            if (item.DItem().SummonLamp)
            {
                tooltips.Add(new TooltipLine(Mod, "充能", Language.GetTextValue("Mods.DDmod.Tooltips.Charging2", charging))
                {
                    OverrideColor = new Color(0, 155, 255)
                });
            }
            else
            {

                if (charging > 1)
                {
                    tooltips.Add(new TooltipLine(Mod, "充能", Language.GetTextValue("Mods.DDmod.Tooltips.Charging") + (charging * 100) + "%")
                    {
                        OverrideColor = new Color(0, 155, 255)
                    });
                }
            }
        }
            
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ItemID.SkyFracture)
            {
                return false;
            }
                return base.CanUseItem(item, player);
        }
    }
}