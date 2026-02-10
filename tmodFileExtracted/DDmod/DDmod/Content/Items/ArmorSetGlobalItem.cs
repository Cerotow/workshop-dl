using DDmod.Content.Dusts;
using DDmod.Content.Items.Armor;
using DDmod.Content.Projectiles.Talisman;
using DDmod.Modkey;
using StructureHelper.Content.GUI;
using Terraria;
using Terraria.GameInput;

namespace DDmod.Content.Items
{
    public class ArmorSetGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Item item)
        {
            if(item.type==959)
            {
                item.defense = 2;
                item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.罕见;
            }
            if(item.type== 3773)
            {
                item.defense = 7;
                item.vanity = false;
            }
            if(item.type== 3774)
            {
                item.defense = 12;
                item.vanity = false;
            }
            if(item.type== 3775)
            {
                item.defense = 4;
                item.vanity = false;
            }
        }
        //套装属性
        public override void UpdateEquip(Item item, Player player)
        {
            //流星套
            if (item.type == ItemID.MeteorSuit || item.type == ItemID.MeteorLeggings)
            {
                if (Main.LocalPlayer.armor[0].type == ModContent.ItemType<MeteorHood>())
                {
                    player.GetDamage(DamageClass.Summon) += 0.09F;
                    player.GetDamage(DamageClass.Magic) -= 0.09F;
                }
                else if (Main.LocalPlayer.armor[0].type != ItemID.MeteorHelmet)
                {
                    player.GetDamage(DamageClass.Magic) -= 0.09F;
                }
            }
            //远古套
            if (item.type == 3773)
            {
                player.GetDamage(DamageClass.Generic) += 0.12F;
                player.GetCritChance(DamageClass.Generic) += 5F;
            }
            if (item.type == 3774)
            {
                player.endurance += 0.08F;
            }
            if (item.type == 3775)
            {
                player.moveSpeed += 0.1F;
                Player.jumpHeight += 2;
            }
            //暗夜套
            if (item.type == ItemID.ShadowGreaves || item.type == ItemID.ShadowScalemail || item.type == ItemID.ShadowHelmet ||
                item.type == 956 || item.type == 957 || item.type == 958)
            {
                player.GetCritChance(DamageClass.Generic) -= 5f;
                player.GetAttackSpeed(DamageClass.Generic) += 0.07f;
            }
        }
        public override void UpdateVanitySet(Player player, string set)
        {
            if (set == "忍者套")
            {
                player.Aplayer().Ninja = true;
            }
        }
        //套装属性描述
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                {
                    if (item.type == ItemID.MeteorSuit || item.type == ItemID.MeteorLeggings)
                    {
                        if (Main.LocalPlayer.armor[0].type == ModContent.ItemType<MeteorHood>())
                        {
                            line.Text = Language.GetTextValue("Mods.DDmod.Tooltips.MeteorArmor2");
                        }
                        else if (Main.LocalPlayer.armor[0].type != ItemID.MeteorHelmet)
                        {
                            line.Text = Language.GetTextValue("Mods.DDmod.Tooltips.MeteorArmor");
                        }

                    }
                    //暗夜套
                    if (item.type == ItemID.ShadowGreaves || item.type == ItemID.ShadowScalemail || item.type == ItemID.ShadowHelmet ||
                item.type == 956 || item.type == 957 || item.type == 958)
                    {
                        line.Text = Language.GetTextValue("Mods.DDmod.Tooltips.Shadow2");
                    }
                }
            }
            if (!item.social)
            {
                for (int A = 0; A < tooltips.Count; A++)
                {
                    if (tooltips[A].Name == "Defense")
                    {
                        //远古套
                        if (item.type == 3773)
                        {
                            tooltips.Insert(A+1,new TooltipLine(Mod, "远古套", Language.GetTextValue("Mods.DDmod.Tooltips.远古套1")));
                        }
                        if (item.type == 3774)
                        {
                            tooltips.Insert(A+1, new TooltipLine(Mod, "远古套", Language.GetTextValue("Mods.DDmod.Tooltips.远古套2")));
                        }
                        if (item.type == 3775)
                        {
                            tooltips.Insert(A+1, new TooltipLine(Mod, "远古套", Language.GetTextValue("Mods.DDmod.Tooltips.远古套3")));
                        }
                        break;
                    }
                }
            }
        }
        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            //忍者
            if (head.type == 256 && body.type == 257 && legs.type == 258)
            {
                return "忍者套";
            }
            if (head.type == 3880 && body.type == 3881 && legs.type == 3882)
            {
                return "忍者套";
            }
            if (head.type == 4982 && body.type == 4983 && legs.type == 4984)
            {
                return "忍者套";
            }
            //木套
            if (head.type == ItemID.WoodHelmet && body.type == ItemID.WoodBreastplate && legs.type == ItemID.WoodGreaves)
            {
                return "木套";
            }
            //红木套
            if (head.type == ItemID.RichMahoganyHelmet && body.type == ItemID.RichMahoganyBreastplate && legs.type == ItemID.RichMahoganyGreaves)
            {
                return "红木套";
            }
            //乌木套
            if (head.type == ItemID.EbonwoodHelmet && body.type == ItemID.EbonwoodBreastplate && legs.type == ItemID.EbonwoodGreaves)
            {
                return "乌木套";
            }
            //暗影木套
            if (head.type == ItemID.ShadewoodHelmet && body.type == ItemID.ShadewoodBreastplate && legs.type == ItemID.ShadewoodGreaves)
            {
                return "暗影木套";
            }
            //针叶木套
            if (head.type == ItemID.BorealWoodHelmet && body.type == ItemID.BorealWoodBreastplate && legs.type == ItemID.BorealWoodGreaves)
            {
                return "针叶木套";
            }
            //棕榈木套
            if (head.type == ItemID.PalmWoodHelmet && body.type == ItemID.PalmWoodBreastplate && legs.type == ItemID.PalmWoodGreaves)
            {
                return "棕榈木套";
            }
            //珍珠木套
            if (head.type == ItemID.PearlwoodHelmet && body.type == ItemID.PearlwoodBreastplate && legs.type == ItemID.PearlwoodGreaves && Main.hardMode)
            {
                return "珍珠木套";
            }
            //猩红套
            if (head.type == ItemID.CrimsonHelmet && (body.type == ModContent.ItemType<远古血腥胸甲>() || body.type == 793) && (legs.type == ModContent.ItemType<远古血腥护腿>() || body.type == 794))
            {
                return "猩红套";
            }
            //暗影套
            if ((head.type == ItemID.ShadowHelmet || head.type == 956) && (body.type == ItemID.ShadowScalemail || body.type == 957) && (legs.type == ItemID.ShadowGreaves || legs.type == 958))
            {
                return "暗影套";
            }
            //紫晶套
            if (head.type == ItemID.WizardHat && body.type == ItemID.AmethystRobe)
            {
                return "紫晶套";
            }
            //黄玉套
            if (head.type == ItemID.WizardHat && body.type == ItemID.TopazRobe)
            {
                return "黄玉套";
            }
            //蓝玉套
            if (head.type == ItemID.WizardHat && body.type == ItemID.SapphireRobe)
            {
                return "蓝玉套";
            }
            //翡翠套
            if (head.type == ItemID.WizardHat && body.type == ItemID.EmeraldRobe)
            {
                return "翡翠套";
            }
            //红玉套
            if (head.type == ItemID.WizardHat && body.type == ItemID.RubyRobe)
            {
                return "红玉套";
            }
            //钻石套
            if (head.type == ItemID.WizardHat && body.type == ItemID.DiamondRobe)
            {
                return "钻石套";
            }
            //琥珀套
            if (head.type == ItemID.WizardHat && body.type == ItemID.AmberRobe)
            {
                return "琥珀套";
            }
            //琥珀套
            if (head.type == 123 && body.type == 124 && legs.type == 125)
            {
                return "流星套";
            }
            //钴套
            if ((head.type == 371 || head.type == 372 || head.type == 373) && body.type == 374 && legs.type == 375)
            {
                return "钴套";
            }
            //钯金套
            if ((head.type == 1205 || head.type == 1206 || head.type == 1207) && body.type == 1208 && legs.type == 1209)
            {
                return "钯金套";
            }
            if ((head.type == 376 || head.type == 377 || head.type == 378) && body.type == 379 && legs.type == 380)
            {
                return "秘银套";
            }
            if ((head.type == 1210 || head.type == 1211 || head.type == 1212) && body.type == 1213 && legs.type == 1214)
            {
                return "山铜套";
            }
            if ((head.type == 400 || head.type == 401 || head.type == 402) && body.type == 403 && legs.type == 404)
            {
                return "精金套";
            }
            if ((head.type == 1215 || head.type == 1216 || head.type == 1217) && body.type == 1218 && legs.type == 1219)
            {
                return "钛金套";
            }
            if ((head.type == 1001 || head.type == 1002 || head.type == 1003) && body.type == 1004 && legs.type == 1005)
            {
                return "叶绿套";
            }
            if ((head.type == 3374) && body.type == 3375 && legs.type == 3376)
            {
                return "化石套";
            }
            if ((head.type == 1135) && body.type == 1136)
            {
                return "雨衣";
            }
            if ((head.type == 2361) && body.type == 2362 && legs.type == 2363)
            {
                return "蜜蜂套";
            }
            if ((head.type == 3187) && body.type == 3188 && legs.type == 3189)
            {
                return "角斗套";
            }
            if ((head.type == 151) && body.type == 152 && legs.type == 153)
            {
                return "死灵套";
            }
            if ((head.type == 959) && body.type == 152 && legs.type == 153)
            {
                return "死灵套2";
            }
            if ((head.type == 3773) && body.type == 3774 && legs.type == 3775)
            {
                return "远古套";
            }
            return "";
        }
        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "木套")
            {
                player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.Wood");
                player.GetDamage(DamageClass.Generic) += 0.03f;
                player.moveSpeed += 0.03f;
            }

            if (set == "红木套")
            {
                player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.RichMahogany");
                player.statDefense += 2;
                if (player.ZoneJungle)
                {
                    player.GetDamage(DamageClass.Generic) += 0.05f;
                    player.moveSpeed += 0.05f;
                    player.statDefense += 3;
                }
            }

            if (set == "乌木套")
            {
                player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.Ebonwood");
                player.statDefense++;
                if (player.ZoneCorrupt)
                {
                    player.GetDamage(DamageClass.Generic) += 0.04f;
                    player.endurance += 0.04f;
                    player.statDefense += 2;
                }
            }

            if (set == "暗影木套")
            {
                player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.Shadewood");
                player.statDefense++;
                if (player.ZoneCrimson)
                {
                    player.Dplayer().DodgeChance += 2;
                    player.GetDamage(DamageClass.Generic) += 0.06f;
                    player.statDefense += 2;
                }
            }

            if (set == "针叶木套")
            {
                player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.BorealWood");
                player.statDefense++;
                if (player.ZoneSnow)
                {
                    player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
                    player.statDefense += 2;
                }
            }

            if (set == "棕榈木套")
            {
                player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.PalmWood");
                player.statDefense++;
                if (player.ZoneBeach)
                {
                    player.fishingSkill += 15;
                }
                else
                if (player.ZoneDesert)
                {
                    player.moveSpeed += 0.1f;
                    player.statDefense += 2;
                }
            }

            if (set == "珍珠木套")
            {
                player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.Pearlwood");
                player.statDefense += 11;
                if (player.ZoneHallow)
                {
                    player.GetAttackSpeed(DamageClass.Generic) += 0.1f;
                    player.GetDamage(DamageClass.Generic) += 0.12f;
                    player.statDefense += 10;
                }
                else
                if (player.ZoneCorrupt || player.ZoneCrimson)
                {
                    player.endurance += 0.1f;
                }
            }

            if (set == "猩红套")
            {
                player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.Crimson");
                player.Aplayer().Crimson = true;
            }
            if (set == "暗影套")
            {
                player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.Shadow");
                player.moveSpeed += 0.5F;
                if (player.velocity.Length() > 1)
                {
                    player.Dplayer().DodgeChance += 15;
                }
                if (!Main.dayTime)
                {
                    player.moveSpeed += 0.5F;
                }
                player.Aplayer().ShadowSet = true;
            }
            if (set == "紫晶套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.Amethyst");
                if (player.ActiveItem().type == ItemID.AmethystStaff)
                {
                    player.GetAttackSpeed(DamageClass.Magic) += 0.4f;
                    player.manaCost *= 0.5f;
                }
            }
            if (set == "黄玉套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.Topaz");
                if (player.ActiveItem().type == ItemID.TopazStaff)
                {
                    player.GetAttackSpeed(DamageClass.Magic) += 0.4f;
                    player.manaCost *= 0.5f;
                }
            }
            if (set == "蓝玉套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.Sapphire");
                if (player.ActiveItem().type == ItemID.SapphireStaff)
                {
                    player.GetAttackSpeed(DamageClass.Magic) += 0.4f;
                    player.manaCost *= 0.5f;
                }
            }
            if (set == "翡翠套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.Emerald");
                if (player.ActiveItem().type == ItemID.EmeraldStaff)
                {
                    player.GetAttackSpeed(DamageClass.Magic) += 0.4f;
                    player.manaCost *= 0.5f;
                }
            }
            if (set == "红玉套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.Ruby");
                if (player.ActiveItem().type == ItemID.RubyStaff)
                {
                    player.GetAttackSpeed(DamageClass.Magic) += 0.4f;
                    player.manaCost *= 0.5f;
                }
            }
            if (set == "钻石套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.Diamond");
                if (player.ActiveItem().type == ItemID.DiamondStaff)
                {
                    player.GetAttackSpeed(DamageClass.Magic) += 0.4f;
                    player.manaCost *= 0.5f;
                }
            }
            if (set == "琥珀套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.Amber");
                if (player.ActiveItem().type == ItemID.AmberStaff)
                {
                    player.GetAttackSpeed(DamageClass.Magic) += 0.4f;
                    player.manaCost *= 0.5f;
                }
            }
            if (set == "流星套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.Meteor2");
                player.GetDamage(DamageClass.Magic) += 0.1F;
                player.GetCritChance(DamageClass.Magic) += 5;
            }
            if (set == "钴套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.钴套");
                player.Aplayer().CobaltSet = true;
            }

            if (set == "钯金套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.钯金套");
                player.Aplayer().PalladiumSet = true;
            }

            if (set == "秘银套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.秘银套");
                player.Dplayer().wingTimeMax += 0.5F;
            }

            if (set == "山铜套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.山铜套");
                player.Dplayer().CritDamage += 0.15F;
            }

            if (set == "精金套")
            {
                string Text = "";
                if (Main.myPlayer == player.whoAmI && ModkeySetup.SetBonus.GetAssignedKeys(InputMode.Keyboard).Count > 0)
                {
                    Text = ModkeySetup.SetBonus.GetAssignedKeys(InputMode.Keyboard)[0];
                }
                if (!DDSystem.English && Text == "Space")
                {
                    Text = "空格";
                }
                if (Text == "")
                {
                    Text = Lang.menu[195].Value;
                }
                else
                {
                    Text = "<" + Text + ">";
                }
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.精金套", Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN"), Text);
                player.jumpSpeedBoost += 2f;
                player.Aplayer().RefinedGoldSet = true;
            }

            if (set == "钛金套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.钛金套");
                int s = player.ownedProjectileCounts[908];
                player.GetDamage(DamageClass.Generic) += 0.02F * s;
                player.GetCritChance(DamageClass.Generic) += s;
            }
            if (set == "叶绿套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.叶绿套");
            }

            if (set == "化石套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.化石套");
                player.Aplayer().FossilSet = true;
            }

            if (set == "雨衣")
            {
                player.setBonus += Language.GetTextValue("Mods.DDmod.ItemArmorSet.雨衣");
                if (Main.raining)
                {
                    player.GetDamage(DamageClass.Generic) += 0.1f;
                    player.moveSpeed += 0.1f;
                    player.GetAttackSpeed(DamageClass.Generic) += 0.1f;
                    player.statDefense += 5;
                }
            }
            if (set == "蜜蜂套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.蜜蜂套");
                if (Main.myPlayer == player.whoAmI && Main.rand.NextBool(100))
                {
                    for (int A = 0; A < Main.rand.Next(2, 6); A++)
                    {
                        int T = player.beeType();
                        NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.Next(0, 900) / 300, T, T == 181 ? 12 : 18, player.beeKB(2), -1);
                    }
                }
            }
            if (set == "角斗套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.角斗套");
                player.Aplayer().GladiatorSet = true;
            }
            if (set == "死灵套")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.死灵套");
                player.Aplayer().UndeadSet = true;
            }
            if (set == "死灵套2")
            {
                player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.死灵套2");
                player.Aplayer().UndeadSet2 = true;
            }
            if (set == "远古套")
            {
                //player.setBonus += "\n" + Language.GetTextValue("Mods.DDmod.ItemArmorSet.远古套");
                player.GetDamage(DamageClass.Generic)+=0.1F;
                player.GetCritChance(DamageClass.Generic)+=10;
                player.GetAttackSpeed(DamageClass.Generic) += 0.15F;
                player.moveSpeed += 0.25F;
                player.jumpSpeedBoost += 2;
                player.setBonus += Language.GetTextValue("Mods.DDmod.ItemArmorSet.远古套");
            }
        }
    }
}