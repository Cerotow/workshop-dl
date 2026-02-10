using DDmod.Content.Items.Accessory;
using DDmod.Content.Items.Boss.克苏鲁之眼;
using DDmod.Content.Items.Pet;
using DDmod.Content.Items.Summon;
using DDmod.Content.NPCs.Boss.恐惧缝合体;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Tiles.绿岩;
using DDmod.NoContent.Config;
using DDmod.UI;
using DDmod.Worlds;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace DDmod.Content.Items
{
    public class DGlobalItem : GlobalItem
    {
        public int ItemTimes;
        public override bool InstancePerEntity => true;
        public bool DrawItems = true;
        public bool DrawMelee = false;
        public bool DrawRanged = false;
        public bool DrawDefaults = false;
        /// <summary>
        /// 不绘制
        /// </summary>
        public bool NoDraw = false;
       /// <summary>
       /// 召唤灯武器
       /// </summary>
        public bool SummonLamp = false;
        public float DrawRot = 0;
        /// <summary>
        /// 手持武器绘制距离
        /// </summary>
        public int DrawDistance = 0;
        /// <summary>
        /// 绘制位置偏移
        /// </summary>
        public Vector2 DrawVec = Vector2.Zero;
        public bool UpdatesRequired;
        public Color HandheldColor = new Color(0,0,0,0);
        public static bool[] FlyingKnife = ItemID.Sets.Factory.CreateBoolSet(false);
        public static bool[] Frisbee = ItemID.Sets.Factory.CreateBoolSet(false);
        public int Boomerang = 0;
        public int PotionCD = 0;
        /// <summary>
        /// 狙击枪
        /// </summary>
        public bool Sniper;
        public static bool[] ChainHammer = ItemID.Sets.Factory.CreateBoolSet(false);

        /// <summary>
        /// 双持武器
        /// </summary>
        public bool Twin;
        public int TwinGlow =0;
            public bool TryGetValue(int type,  out Item value)
        {
            if(type<=0)
            {
                value = DDmod.NewItem.Clone();
                return false;
            }
            Item item = new Item(type);
            value = item;
            return true;
        }
        //ContentSamples.ItemsByType.TryGetValue
        public float OriginalScale(Item item2)
        {
            return TryGetValue(item2.type, out Item item) ? item.scale : 0;
        }
        public int OriginalCrit(Item item2)
        {
            return TryGetValue(item2.type, out Item item) ? item.crit : 0;
        }
        public int OriginaluseAnimation(Item item2)
        {
            return TryGetValue(item2.type, out Item item) ? item.useAnimation : 0;
        }
        public int OriginaluseTime(Item item2)
        {
            return TryGetValue(item2.type, out Item item) ? item.useTime : 0;
        }
        public int OriginalMana(Item item2)
        {
            return TryGetValue(item2.type, out Item item) ? item.mana : 0;
        }
        public float OriginalShootSpeed(Item item2)
        {
            return TryGetValue(item2.type, out Item item) ? item.shootSpeed : 0;
        }
        public float OriginalKnockBack(Item item2)
        {
            return TryGetValue(item2.type, out Item item) ? item.knockBack : 0;
        }
        public int OriginalValue(Item item2)
        {
            return TryGetValue(item2.type, out Item item) ? item.value : 0;
        }
        public int OriginalRare(Item item2)
        {
            return TryGetValue(item2.type, out Item item) ? item.rare : 0;
        }
        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            ItemTimes++;
            if(ModContent.GetInstance<DDConfigServer>().ItemTime!=0&& ItemTimes>ModContent.GetInstance<DDConfigServer>().ItemTime*60)
            {
                item.active = false;
            }
        }
        public override void UpdateInventory(Item item, Player player)
        {                      
            ItemTimes = 0;
        }
        public override bool? UseItem(Item item, Player player)
        {
            return base.UseItem(item, player);
        }
        public override void SetStaticDefaults()
        {
            for (int A = 0; A < ItemID.Sets.Deprecated.Length; A++)
            {
                ItemID.Sets.Deprecated[A] = false;
            }
            FlyingKnife[9] = true;
            FlyingKnife[287] = true;
            FlyingKnife[3054] = true;
            FlyingKnife[3030] = true;

            FlyingKnife[42] = true;
            FlyingKnife[168] = true;
            FlyingKnife[279] = true;
            FlyingKnife[287] = true;
            FlyingKnife[517] = true;
            FlyingKnife[1913] = true;
            FlyingKnife[2586] = true;
            FlyingKnife[3116] = true;
            FlyingKnife[3379] = true;
            FlyingKnife[3548] = true;
            ItemID.Sets.ShimmerTransformToItem[151] = 0;
            ItemID.Sets.ShimmerTransformToItem[959] = 0;

            /*
            Frisbee[191] = true;
            Frisbee[561] = true;
            Frisbee[1918] = true;
            ChainHammer[162] = true;
            ChainHammer[163] = true;
            ChainHammer[220] = true;
            ChainHammer[389] = true;
            ChainHammer[801] = true;
            ChainHammer[1259] = true;
            ChainHammer[1325] = true;
            ChainHammer[2611] = true;
            ChainHammer[3012] = true;
            */
        }
        public override void SetDefaults(Item item)
        {

            if (item.shoot is 6 or 19 or  33 or  52 or  113 or  320 or  333 or  383 or  491 or  867 or  902 or  866)
            {
                Boomerang = 1;
            }

            if (item.shoot == 106)
            {
                Boomerang = 6;
            }

            if (item.shoot == 272)
            {
                Boomerang = 10;
            }

            if (item.shoot == 332 || item.shoot == 1000)
            {
                Boomerang = 3;
            }
            if (item.type == 168)
            {
                item.useStyle = 1;
            }
            if (item.type == 1922)
            {
                item.maxStack = Item.CommonMaxStack;
            }
            //史莱姆王
            if (item.type == ItemID.SlimeCrown)
            {
                item.consumable = false;
            }
            //克苏鲁之眼
            if (item.type == ItemID.SuspiciousLookingEye)
            {
                item.consumable = false;
            }
            //伞
            if (item.holdStyle > 0)
            {
                DrawItems = false;
            }
            //星怒
            if (item.type == 65)
            {
                HandheldColor = new Color(255, 255, 255, 160);
            }
            //史莱姆法杖
            if (item.type == 1309)
            {
                item.damage = 12;
                item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.珍宝;
            }
            //史莱姆法杖
            if (item.type == 1297)
            {
                DrawDistance = -12;
            }
            //史莱姆法杖
            if (item.type == 1257)
            {
                DrawDistance = -12;
            }
            if (item.type is 3835 or 3858 or 4722)
            {
                DrawMelee = true;
            }
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == 4271&&Main.bloodMoon&&!NPC.AnyNPCs(ModContent.NPCType<恐惧缝合体>()))
            {
                player.itemAnimationMax = 120;
                player.itemTimeMax = 120;
                player.itemAnimation = 120;
                player.itemTime = 120;
                if(Main.myPlayer==player.whoAmI)
                    NewProjectile(player.GetSource_FromAI(), player.Center, new Vector2(0, -20), ModContent.ProjectileType<血珠>(), 10, 0, -1);
                return true;
            }
            return base.CanUseItem(item, player);
        }
        public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue)
        {
            if(player.Dplayer().MushroomsLife)
            {
                healValue = (int)(healValue * 1.0501F);
            }
        }
        public override void GetHealMana(Item item, Player player, bool quickHeal, ref int healValue)
        {
            if (player.Dplayer().MushroomsMana)
            {
                healValue = (int)(healValue * 1.1501F);
            }
        }
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == 3329)
            {
                itemLoot.NormalLoot(4, 1, 1, 1, ModContent.ItemType<石像机器控制器>());
            }
            if (item.type == 3319)
            {
                itemLoot.NormalLoot(4, 1, 1, 1, ModContent.ItemType<眼球魔杖>());
                itemLoot.Add(ItemDropRule.FewFromOptionsWithNumerator(2, 1, 1,ModContent.ItemType<眼球头>(),ModContent.ItemType<血嘴衣>(),ModContent.ItemType<眼肉裤>()));
            }
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (item.ModItem == null || item.ModItem.Mod.Name == "DDmod")
            {
            player.RotationSpeed(player.fullRotation, 0);
            float ro = player.fullRotation - MathHelper.Pi;
            position = player.MountedCenter;
            float SP = velocity.Length();
            velocity = (player.Dplayer().MouseWorld - position).PerfectNormalize() * SP;
            }
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        /// <summary> 
        ///鞭子： item 物品,projectileId 弹幕,dmg 伤害,shootspeed 速度,animationTotalTime 攻速
        /// </summary>
        public static void DefaultToWhip(Item item, int projectileId, int dmg, float kb, float shootspeed, int animationTotalTime = 30)
        {
            item.autoReuse = true;
            item.useStyle = 1;
            item.useAnimation = animationTotalTime;
            item.useTime = animationTotalTime;
            item.width = 18;
            item.height = 18;
            item.shoot = projectileId;
            item.UseSound = SoundID.Item152;
            item.noMelee = true;
            item.DamageType = DamageClass.SummonMeleeSpeed;
            item.noUseGraphic = true;
            item.damage = dmg;
            item.knockBack = kb;
            item.shootSpeed = shootspeed;
        }
        public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
        {
            return base.PreDrawTooltip(item, lines, ref x, ref y);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!ModContent.GetInstance<DDConfigServer>().ForceMechanism)
            {
                if (item.type == ItemID.LifeCrystal)
                {
                    int A = 0;
                    if (NPCDowned.downedLifeGuard)
                    {
                        A += 5;
                    }
                    if ( NPCDowned.downedLifeGuard2)
                    {
                        A += 5;
                    }
                    if ( NPCDowned.downedLifeGuard3)
                    {
                        A += 5;
                    }
                    A -= (Main.LocalPlayer.statLifeMax-100) / 20;
                    if (A < 0)
                    {
                        A = 0;
                    }
                    tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.Tooltips.LifeCrystal", A))
                    {
                        OverrideColor = new Color(255, 0, 0)
                    });
                }
                if (item.type == ItemID.ManaCrystal)
                {
                    int A = 0;
                    if ( NPCDowned.downedStarGuard)
                    {
                        A += 4;
                    }
                    if ( NPCDowned.downedStarGuard2)
                    {
                        A += 5;
                    }
                    if (NPCDowned.downedStarGuard3)
                    {
                        A += 5;
                    }
                    A -= (Main.LocalPlayer.statManaMax+Main.LocalPlayer.Dplayer().statManaMax - 20) / 20;
                    if (A < 0)
                    {
                        A = 0;
                    }
                    tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.Tooltips.ManaCrystal", A))
                    {
                        OverrideColor = new Color(0, 155, 255)
                    });
                }
            }
            if (item.type == ItemID.HermesBoots)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.Tooltips.HermesBoots"))
                {
                    OverrideColor = new Color(100, 255, 100)
                });
            }
            if (item.type == ItemID.FlurryBoots)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.Tooltips.FlurryBoots"))
                {
                    OverrideColor = new Color(0, 155, 255)
                });
            }
            if (item.type == ItemID.Boomstick)
            {
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.Tooltips.Boomstick"))
                {
                    OverrideColor = new Color(255, 130, 0)
                });
            }
            if (ModContent.GetInstance<DDConfigClient>().ItemID)
            {
                if (item.type == ModContent.ItemType<咬人的书>())
                {
                    tooltips.Add(new TooltipLine(Mod, "Damage", Language.GetTextValue("Mods.DDmod.Tooltips.ItemsID") + 520+"(?)"));
                }
                else
                { 
                    tooltips.Add(new TooltipLine(Mod, "Damage", Language.GetTextValue("Mods.DDmod.Tooltips.ItemsID") + item.type));
                }
            }

            for (int A = 0; A < tooltips.Count; A++)
            {
                TooltipLine line = tooltips[A];
                if (line.Mod == "Terraria")
                {
                    if (item.type == 4722)
                    {
                        if (line.Name == "Knockback")
                        {
                            tooltips.Insert(A+1, new TooltipLine(Mod, "Tooltip0", Language.GetTextValue("Mods.DDmod.Items.第一分型.Tooltip")));
                            break;
                        }
                    }
                }
            }
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria")
                {
                    if (item.ammo > 0 && line.Name == "CritChance")
                    {
                        line.Text = "";
                    }
                    if (line.Name == "Ammo")
                    {
                        if (item.ammo > 0)
                        {
                            line.Text += "[i:" + item.ammo + "]";
                        }
                    }
                    if (line.Name == "Price")
                    {
                        //line.Text = ""+114514;
                    }
                    if (item.type == 4722)
                    {
                        if (line.Name == "ItemName")
                        {
                            line.Text = Language.GetTextValue("Mods.DDmod.Items.第一分型.DisplayName");
                        }
                    }
                    if (Sniper)
                    {
                        if (line.Name == "Tooltip0")
                        {
                            if (line.Text != "")
                            {
                                line.Text += "\n";
                            }
                            line.Text += Language.GetTextValue("Mods.DDmod.Tooltips.狙击枪");
                        }
                    }
                }
            }
            if (item.useAmmo > 0)
                tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.Tooltips.UseAmmo") + "[i: " + item.useAmmo + "]"));
            if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
            {
                tooltips.Add(new TooltipLine(Mod, "", "品质:"+item.rare));
            }
            //武器强化,暂时不需要
            /*tooltips.Add(new TooltipLine(Mod, "Damage", "强化:" + 强化));

            if (item.prefix > 0)
            {
                Item item2 = Main.tooltipPrefixComparisonItem;
                if (item2 == null || item2.netID != item.netID)
                {
                    item2 = new Item();
                    item2.netDefaults(item.netID);
                }

                if (item2.damage != item.damage)
                {
                    double num7 = (float)(item.damage - ((int)(item2.damage * 强化) / 10)) - item2.damage;

                    num7 = num7 / item2.damage * 100.0;

                    num7 = Math.Round(num7);

                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "PrefixDamage")
                        {
                            if (num7 > 0.0)
                            {
                                line.Text = "+" + num7 + Lang.tip[39].Value;
                            }
                            else if (num7 < 0.0)
                            {
                                line.Text = num7 + Lang.tip[39].Value;
                                line.IsModifierBad = true;
                            }
                            else
                            {
                                line.Text = "";
                            }
                        }
                    }
                }
            }*/
        }
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Name == "物品" && line.Mod == "DDmod")
            {
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y), line.Color, line.Rotation, line.Origin, line.BaseScale, line.MaxWidth,0);
                return false;
            }
            if (line.Name == "Damage" && line.Mod == "DDmod")
            {
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y), new Color(255, 255, 255), line.Rotation, line.Origin, line.BaseScale, line.MaxWidth, line.Spread);
                Vector2 size = FontAssets.MouseText.Value.MeasureString(line.Text);
                Main.spriteBatch.Draw(DDTextures.VoidStar.Value, new Vector2(line.X, line.Y - 4) + (size / 2f), null, new Color(0, 255, 255, 0) * 0.45f, 0f, Utils.Size(DDTextures.VoidStar.Value) / 2f, new Vector2(size.X / 50, 0.4f), 0, 0f);
                return false;
            }
            return base.PreDrawTooltipLine(item, line, ref yOffset);
        }
        public override bool CanStackInWorld(Item destination, Item source)
        {
            if (destination.type == 521|| destination.type == 520|| destination.type == 1332)
            {
                return false;
            }
            return base.CanStackInWorld(destination, source);
        }
        public override bool CanStack(Item destination, Item source)
        {
            return base.CanStack(destination, source);
        }
        //武器强化,暂时不需要
        /*
        public int 强化 = 10;
        public int damage;
        public override GlobalItem NewInstance(Item target)
        {
            return base.NewInstance(target);
        }
        public override void PostReforge(Item item)
        {
            强化++;
            item.damage += item.OriginalDamage * 强化 / 10;
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            强化 = tag.Get<int>("强化");
            item.damage += item.OriginalDamage * 强化 / 10;
        }

        public override void SaveData(Item item, TagCompound tag)
        {
            tag["强化"] = 强化;
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(强化);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            强化 = reader.ReadInt32();
            item.damage += item.OriginalDamage * 强化 / 10;
        }

        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
        }

        public void GainExperience(Item item, int xp)
        {

        }*/
    }
}