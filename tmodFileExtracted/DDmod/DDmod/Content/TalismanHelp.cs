using DDmod.AccessorySlot;
using DDmod.Content.Prefixes;
using DDmod.DrawPlayer;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace DDmod.Content
{
    public class TalismanDamage : DamageClass
    {
        public override void Load()
        {
            Instance = this;
        }
        public override void Unload()
        {
            Instance = null;
        }
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            return StatInheritanceData.None;
        }
        public static DamageClass Instance;
    }
    public class TalismanGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        /// <summary>我是一个一个一个,法宝啊啊啊啊啊啊</summary>
        public bool Talisman;

        /// <summary>法宝冷却</summary>
        public int TalismanCD;

        /// <summary>法宝时间</summary>
        public int TalismanTimes;

        /// <summary>最大法宝冷却</summary>
        public int MaxTalismanCD;

        /// <summary>最大法宝时间</summary>
        public int MaxTalismanTimes;
        /// <summary>法宝生命值</summary>
        public int TalismanLife;

        /// <summary>最大法宝生命值</summary>
        public int MaxTalismanLife;

        /// <summary>法宝类型</summary>
        public int TalismanType;
        public override bool PreAI(Projectile projectile)
        {
            return base.PreAI(projectile);
        }
        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            if (Talisman)
            {
                TalismanType = binaryReader.ReadInt32();
                TalismanCD = binaryReader.ReadInt32();
                TalismanTimes = binaryReader.ReadInt32();
                MaxTalismanCD = binaryReader.ReadInt32();
                MaxTalismanTimes = binaryReader.ReadInt32();
                TalismanLife = binaryReader.ReadInt32();
                MaxTalismanLife = binaryReader.ReadInt32();
            }
        }
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            if (Talisman)
            {
                binaryWriter.Write(TalismanType);
                binaryWriter.Write(TalismanCD);
                binaryWriter.Write(TalismanTimes);
                binaryWriter.Write(MaxTalismanCD);
                binaryWriter.Write(MaxTalismanTimes);
                binaryWriter.Write(TalismanLife);
                binaryWriter.Write(MaxTalismanLife);
            }
        }
    }
    public class TalismanGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        /// <summary>我是一个一个一个,法宝啊啊啊啊啊啊</summary>
        public bool Talisman;
        /// <summary>法宝类型</summary>
        public TalismanTypes TalismanType;

        public int Level;
        /// <summary>法宝冷却</summary>
        public int TalismanCD;

        /// <summary>法宝时间</summary>
        public int TalismanTimes;
        //默认属性
        public override void SetDefaults(Item item)
        {
        }
        //物品,类型,冷却,持续时间
        public void SetTalismanDefaults(Item item, TalismanTypes types, int CD, int Times)
        {
            item.accessory = true;
            Talisman = true;
            TalismanType = types;
            TalismanCD = CD;
            TalismanTimes = Times;
            item.DamageType = ModContent.GetInstance<TalismanDamage>();
        }
        public void TalismanSpawning(Item item, Player player, int type)
        {
            player.TPlayer().type = type;
            player.TPlayer().Talisman = true;
            player.TPlayer().MaxTalismanCD = TalismanCD;
            player.TPlayer().MaxTalismanTimes = TalismanTimes;

            int Proj = 0;
            for (int a = 0; a < 1000; a++)
            {
                Projectile projectile = Main.projectile[a];
                if (projectile.type > 0 && projectile.active && projectile.owner == player.whoAmI && projectile.Tproj().Talisman)
                {
                    Proj++;
                }
            }
            if (Proj < 1 && player.ownedProjectileCounts[item.shoot] == 0)
            {
                int A = NewProjectile(player.GetSource_FromAI(), player.Center + new Vector2(1000 * player.direction, -1000), Vector2.Zero, item.shoot, item.damage, item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[A].CritChance = item.crit;
                Main.projectile[A].originalDamage = item.damage;
                Main.projectile[A].Tproj().TalismanType = type;
            }
        }
        //重铸
        public override bool? PrefixChance(Item item, int pre, UnifiedRandom rand)
        {
            if (Talisman && Main.InReforgeMenu)
            {
                return false;
            }
            return base.PrefixChance(item, pre, rand);
        }
        public override bool AllowPrefix(Item item, int pre)
        {
            if (Talisman)
            {
                return false;
            }
            return base.AllowPrefix(item, pre);
        }
        public override bool CanReforge(Item item)
        {
            return base.CanReforge(item);
        }
        //可以获得的词条
        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            return base.ChoosePrefix(item, rand);
        }
        //禁止装备饰品
        public override bool CanEquipAccessory(Item item, Player player, int slot, bool modded)
        {
            if (Talisman)
            {
                return modded && slot == AccessorySystem.TalismanSlots;
            }
            return base.CanEquipAccessory(item, player, slot, modded);
        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            
            if (player.TPlayer().UseTalisman)
            {
                return true;
            }
            if (equippedItem.type > 0)
            {
                if (equippedItem.Titem().Talisman && incomingItem.Titem().Talisman)
                {
                    return false;
                }
            }
            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }
        public override GlobalItem NewInstance(Item target)
        {
            return base.NewInstance(target);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            Level = tag.Get<int>("Level");
            item.damage = item.OriginalDamage * (Level + 1);
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag["Level"] = Level;
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(Level);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            Level = reader.ReadInt32();
            item.damage = item.OriginalDamage * (Level + 1);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Talisman)
            {
                tooltips.Add(new TooltipLine(Mod, "CD", Language.GetTextValue("Mods.DDmod.Tooltips.TalismanCD", ((float)TalismanCD / 60).ToString("F1") + Language.GetTextValue("Mods.DDmod.Tooltips.Second"))));
                tooltips.Add(new TooltipLine(Mod, "Times", Language.GetTextValue("Mods.DDmod.Tooltips.TalismanTimes", ((float)TalismanTimes / 60).ToString("F1") + Language.GetTextValue("Mods.DDmod.Tooltips.Second"))));

                foreach (TooltipLine line in tooltips)
                {
                    if (line.Mod == "Terraria" && line.Name == "Equipable")
                    {
                        line.Text = Language.GetTextValue("Mods.DDmod.Tooltips.Talisman");
                    }
                    if (line.Mod == "Terraria" && line.Name == "Damage")
                    {
                        if (TalismanType == TalismanTypes.Damage)
                        {
                            line.Text = item.damage + " " + Language.GetTextValue("Mods.DDmod.Tooltips.Damage");
                        }
                        if (TalismanType == TalismanTypes.Restoration)
                        {
                            line.Text = item.damage + " " + Language.GetTextValue("Mods.DDmod.Tooltips.Restoration");
                        }
                        if (TalismanType == TalismanTypes.RestorationMana)
                        {
                            line.Text = item.damage + " " + Language.GetTextValue("Mods.DDmod.Tooltips.RestorationMana");
                        }
                        if (TalismanType == TalismanTypes.Resist)
                        {
                            line.Text = item.damage + " " + Language.GetTextValue("Mods.DDmod.Tooltips.Resist");
                        }
                    }
                    if (line.Mod == "Terraria" && line.Name == "CritChance")
                    {
                        if (TalismanType == TalismanTypes.Restoration)
                        {
                            line.Text = item.crit + "% " + Language.GetTextValue("Mods.DDmod.Tooltips.RestorationCrit");
                        }
                        if (TalismanType == TalismanTypes.RestorationMana)
                        {
                            line.Text = item.crit + "% " + Language.GetTextValue("Mods.DDmod.Tooltips.RestorationManaCrit");
                        }
                        if (TalismanType == TalismanTypes.Resist)
                        {
                            line.Text = item.crit + "% " + Language.GetTextValue("Mods.DDmod.Tooltips.ResistCrit");
                        }
                    }
                }
                TalismanLevel level = (TalismanLevel)Level;

                if (level == TalismanLevel.Ordinary)
                {
                    tooltips.Add(new TooltipLine(Mod, "法宝品质", "普通"));
                }
                if (level == TalismanLevel.Excellent)
                {
                    tooltips.Add(new TooltipLine(Mod, "法宝品质", "优秀"));
                }
                if (level == TalismanLevel.Excellent2)
                {
                    tooltips.Add(new TooltipLine(Mod, "法宝品质", "优秀+"));
                }
                if (level == TalismanLevel.Rare)
                {
                    tooltips.Add(new TooltipLine(Mod, "法宝品质", "稀有"));
                }
                if (level == TalismanLevel.Rare2)
                {
                    tooltips.Add(new TooltipLine(Mod, "法宝品质", "稀有+"));
                }
                if (level == TalismanLevel.Epic)
                {
                    tooltips.Add(new TooltipLine(Mod, "法宝品质", "史诗"));
                }
                if (level == TalismanLevel.Epic2)
                {
                    tooltips.Add(new TooltipLine(Mod, "法宝品质", "史诗+"));
                }
                if (level == TalismanLevel.Epic3)
                {
                    tooltips.Add(new TooltipLine(Mod, "法宝品质", "史诗++"));
                }
                if (level == TalismanLevel.Legend)
                {
                    tooltips.Add(new TooltipLine(Mod, "法宝品质", "传说"));
                }
                if (level == TalismanLevel.Legend2)
                {
                    tooltips.Add(new TooltipLine(Mod, "法宝品质", "传说+"));
                }
                if (level == TalismanLevel.Legend3)
                {
                    tooltips.Add(new TooltipLine(Mod, "法宝品质", "传说++"));
                }
            }
        }
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Name == "普通" && line.Mod == "DDmod")
            {
                TextShader(1, ModContent.Request<Texture2D>("DDmod/Content/Items/Talisman/Ordinary"), Color.White, new Vector2(Main.GlobalTimeWrappedHourly*20, 0));
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y), Color.White, line.Rotation, line.Origin, line.BaseScale, line.MaxWidth, line.Spread);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y), Color.White*0.75f, line.Rotation, line.Origin, line.BaseScale, line.MaxWidth, line.Spread);

                return false;
            }
            if (line.Name == "优秀" && line.Mod == "DDmod")
            {
                TextShader(1, ModContent.Request<Texture2D>("DDmod/Content/Items/Talisman/Excellent"), Color.White, new Vector2(Main.GlobalTimeWrappedHourly*20, 0));
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y), Color.White, line.Rotation, line.Origin, line.BaseScale, line.MaxWidth, line.Spread);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y), Color.White*0.75f, line.Rotation, line.Origin, line.BaseScale, line.MaxWidth, line.Spread);

                return false;
            }
            return true;
        }
        public void TextShader(float Opacity,Asset<Texture2D> asset,Color color,Vector2 vector)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            GameShaders.Misc["渲染滤镜"].UseOpacity(Opacity);
            GameShaders.Misc["渲染滤镜"].SetShaderTexture(asset);
            GameShaders.Misc["渲染滤镜"].UseColor(color);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["uColor2"].SetValue(color.ToVector3() * 1.25f);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["uWorldPosition"].SetValue(Vector2.Zero);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["uImageSize1"].SetValue(asset.Size()/10);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["renderTargetArea"].SetValue(new Vector2(asset.Width(), asset.Height())/5);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["position"].SetValue(vector);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["ImageSize"].SetValue(asset.Size()/10);
            GameShaders.Misc["渲染滤镜"].Shader.Parameters["upscaleFactor"].SetValue(new Vector2(-0.7F));
            GameShaders.Misc["渲染滤镜"].Apply();
        }
    }
    public enum TalismanLevel : int
    {
        /// <summary>普通</summary>
        Ordinary,
        /// <summary>优秀</summary>
        Excellent,
        /// <summary>优秀+</summary>
        Excellent2,
        /// <summary>稀有</summary>
        Rare,
        /// <summary>稀有+</summary>
        Rare2,
        /// <summary>史诗</summary>
        Epic,
        /// <summary>史诗+</summary>
        Epic2,
        /// <summary>史诗++</summary>
        Epic3,
        /// <summary>传说</summary>
        Legend,
        /// <summary>传说+</summary>
        Legend2,
        /// <summary>传说++</summary>
        Legend3,
    }
    public class TalismanPlayer : ModPlayer
    {
        public const int 鬼火灯笼 = 1;
        public const int 暗影项链 = 2;
        public const int 迷你史莱姆皇冠 = 3;
        public const int 眼牙 = 4;
        public const int 木灵剑 = 5;
        public const int 巫毒娃娃 = 6;
        public const int 传奇凝胶 = 7;
        public const int 枯叶灵 = 8;
        public const int 混元伞 = 9;
        public const int 永恒冰晶 = 10;
        public const int 奇异的云 = 11;
        public const int 地狱葫芦 = 12;
        public const int 玉净瓶 = 13;
        public const int 鬼牙 = 14;
        public const int 魔力菇 = 15;
        public const int 绿岩导弹发射器 = 16;
        public const int 宣花葫芦 = 17;
        public const int 药王葫芦 = 18;
        /// <summary>
        /// 法宝类型
        /// </summary>
        public int type;

        /// <summary> 护盾值 </summary>
        public int Shield;
        public int oldShield;

        /// <summary> 最大护盾值 </summary>
        public int MaxShield;
        /// <summary> 使用中的法宝 </summary>
        public bool UseTalisman;

        /// <summary>法宝冷却</summary>
        public int MaxTalismanCD;

        public int TalismanCD;
        public int TalismanCD2;

        /// <summary>法宝使用时间</summary>
        public int TalismanTimes;
        public int TalismanTimes2;
        public int MaxTalismanTimes;

        /// <summary>法宝</summary>
        public bool Talisman;
        ///<summary>法宝UI位置</summary>
        public Vector2 TalismanPo = new Vector2(660, 40);
        public bool mouseX;
        public bool mouse;
        public override void SaveData(TagCompound tag)
        {
            tag.Add("TalismanPo", TalismanPo);
        }
        public override void LoadData(TagCompound tag)
        {
            TalismanPo = tag.Get<Vector2>("TalismanPo");
        }
        public override void ResetEffects()
        {
            type = 0;
            if (!Talisman)
            {
                TalismanCD = 0;
            }
            Talisman = false;
        }
        public override void PreUpdate()
        {
            if (!UseTalisman)
            {
                if (TalismanCD2 > TalismanCD)
                {
                    TalismanCD2 -= 1 + (TalismanCD2 - TalismanCD) / 30;
                    float A = (float)Main.LocalPlayer.TPlayer().TalismanCD2 / Main.LocalPlayer.TPlayer().MaxTalismanCD;
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        for (int a = 0; a < 1 + (TalismanCD2 - TalismanCD) / 100; a++)
                        {
                            UIDustDraw dust = DustSystem.UIDustDraws[UIDustDraw.NewDust(TalismanPo + new Vector2(-25 + 50 * A, Main.rand.NextFloat(-4, 4)), 1, new Color(46, 157, 255, 0), new Vector2(Main.rand.NextFloat(1, 2), 0), Main.rand.NextFloat(1F, 1.5F))];
                            dust.Gravity = false;
                            dust.ScaleSpeed = 0.03F;
                        }
                    }
                    if (TalismanCD2 - TalismanCD < 1)
                    {
                        TalismanCD2 = TalismanCD;
                    }
                }
                if (TalismanCD2 < TalismanCD)
                {
                    TalismanCD2 += 1 + (TalismanCD - TalismanCD2) / 200;
                    if (TalismanCD - TalismanCD2 < 1)
                    {
                        TalismanCD2 = TalismanCD;
                    }
                }
                if (MaxTalismanCD > TalismanCD)
                {
                    TalismanCD++;
                }
                TalismanTimes2 = TalismanTimes;
            }
            else
            {
                if (Player.whoAmI == Main.myPlayer && Main.netMode == 1 && TalismanTimes % 60 == 0)
                {
                    DDmod.SyncData(DDType.Talisman, Player.whoAmI, -1, Player.whoAmI);
                }
                TalismanCD2 = TalismanCD;
                if (TalismanTimes2 > TalismanTimes)
                {
                    TalismanTimes2 -= 1 + (TalismanTimes2 - TalismanTimes) / 30;
                    float A = (float)Main.LocalPlayer.TPlayer().TalismanTimes2 / Main.LocalPlayer.TPlayer().MaxTalismanTimes;
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        for (int a = 0; a < 1 + (TalismanTimes2 - TalismanTimes) / 100; a++)
                        {
                            UIDustDraw dust = DustSystem.UIDustDraws[UIDustDraw.NewDust(TalismanPo + new Vector2(-25 + 50 * A, Main.rand.NextFloat(-4, 4)), 1, new Color(46, 157, 255, 0), new Vector2(Main.rand.NextFloat(1, 2), 0), Main.rand.NextFloat(1F, 1.5F))];
                            dust.Gravity = false;
                            dust.ScaleSpeed = 0.03F;
                        }
                    }
                    if (TalismanTimes2 - TalismanTimes < 1)
                    {
                        TalismanTimes2 = TalismanTimes;
                    }
                }
                if (TalismanTimes2 < TalismanTimes)
                {
                    TalismanTimes2 += 1 + (TalismanTimes - TalismanTimes2) / 200;
                    if (TalismanTimes - TalismanTimes2 < 1)
                    {
                        TalismanTimes2 = TalismanTimes;
                    }
                }
                if (TalismanTimes > 0)
                {
                    TalismanTimes--;
                }
                TalismanCD = 0;
                bool B = false;
                for (int a = 0; a < 1000; a++)
                {
                    if (Main.projectile[a].active && Main.projectile[a].owner == Player.whoAmI && Main.projectile[a].Tproj().Talisman)
                    {
                        B = true;
                        break;
                    }
                }

                UseTalisman = B;
            }
            if (Shield > 0)
            {
                Player.GetModPlayer<DrawDDPlayer>().HunyuanShieldSc = 0;
                if (Player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit > 0) Player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit -= 3;
            }
            else
            {
                if (Player.GetModPlayer<DrawDDPlayer>().HunyuanShieldSc < 0.5F)
                {
                    Player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit = 0.1F;
                    Player.GetModPlayer<DrawDDPlayer>().HunyuanShieldSc += 0.03f;
                }
            }
        }
        public override bool ConsumableDodge(Player.HurtInfo info)
        {
            if (Shield > 0)
            {
                Shield -= info.Damage;
                Player.immune = true;
                Player.hurtCooldowns[1] = 20;
                Player.immuneTime = 20;
                Player.immuneNoBlink = true;
                Player.GetModPlayer<DrawDDPlayer>().HunyuanShieldHit = 60;
                if (Shield > 0)
                {
                    SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/格挡");
                    sound.Pitch = 0.5f;
                    PlaySound(sound, Player.position);
                }
                else
                {
                    SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/格挡");
                    sound.Pitch = -1;
                    PlaySound(sound, Player.position);
                }
                return true;
            }
            return base.ConsumableDodge(info);
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
        }
        //同步延迟
        public static void PlayerTalisman(Mod mod, BinaryReader reader)
        {
            byte playerByte = reader.ReadByte();

            // 简单直接，按顺序读取所有字段
            bool useTalisman = reader.ReadBoolean();
            int shield = reader.ReadInt32();
            int oldShield = reader.ReadInt32();
            int maxShield = reader.ReadInt32();
            int maxTalismanCD = reader.ReadInt32();
            int talismanCD = reader.ReadInt32();
            int talismanCD2 = reader.ReadInt32();
            int talismanTimes = reader.ReadInt32();
            int talismanTimes2 = reader.ReadInt32();
            int maxTalismanTimes = reader.ReadInt32();

            Player player = Main.player[playerByte];
            TalismanPlayer DDPlayer = player.TPlayer();

            // 一次性赋值
            DDPlayer.UseTalisman = useTalisman;
            DDPlayer.Shield = shield;
            DDPlayer.oldShield = oldShield;
            DDPlayer.MaxShield = maxShield;
            DDPlayer.MaxTalismanCD = maxTalismanCD;
            DDPlayer.TalismanCD = talismanCD;
            DDPlayer.TalismanCD2 = talismanCD2;
            DDPlayer.TalismanTimes = talismanTimes;
            DDPlayer.TalismanTimes2 = talismanTimes2;
            DDPlayer.MaxTalismanTimes = maxTalismanTimes;

            // 服务器转发
            if (Main.netMode == NetmodeID.Server)
            {
                DDmod.SyncData(DDType.Talisman, playerByte, -1, playerByte);
            }
        }
    }

    public static class TalismanHelp
    {
        public static TalismanGlobalProjectile Tproj(this Projectile projectile)
        {
            return projectile.GetGlobalProjectile<TalismanGlobalProjectile>();
        }
        public static TalismanGlobalItem Titem(this Item item)
        {
            return item.GetGlobalItem<TalismanGlobalItem>();
        }
        public static TalismanPlayer TPlayer(this Player player)
        {
            return player.GetModPlayer<TalismanPlayer>();
        }
    }
    public enum TalismanTypes : int
    {
        None,
        /// <summary>伤害型法宝</summary>
        Damage,
        /// <summary>治愈型法宝</summary>
        Restoration,
        /// <summary>魔力恢复法宝</summary>
        RestorationMana,
        /// <summary>抵挡型法宝</summary>
        Resist,
    } 
}