using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;

namespace DDmod.Content.Items
{
    public class AdventureGearGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        /// <summary> 稀有度,1稀有,2罕见,3珍宝 </summary>
        public int 稀有度 = -1;
        public const short 稀有 = 1;
        public const short 罕见 = 2;
        public const short 珍宝 = 3;
        public const short 泰拉级 = 4;
        /// <summary> 品质 </summary>
        public int Quality = -1;
        /// <summary> 可以获得生命 </summary>
        public int CanLife;
        public int Life;
        /// <summary> 可以获得魔力 </summary>
        public int CanMana;
        public int Mana;
        /// <summary> 可以获得伤害 </summary>
        public int CanDamage;
        public float Damage;
        /// <summary> 可以获得防御 </summary>
        public int CanDefense;
        public int Defense;
        /// <summary> 可以获得移速 </summary>
        public int CanSpeed;
        public float Speed;
        /// <summary> 可以获得暴击 </summary>
        public int CanCrit;
        public int Crit;
        /// <summary> 生成属性 </summary>
        public bool BuildProperties;
        /// <summary>
        /// 生命等级
        /// 魔法等级
        ///伤害等级
        /// 防御等级
        ///速度等级
        /// 暴击等级
        /// </summary>
        public bool[] Attribute = new bool[6];
        /// <summary>
        /// 饰品品质属性
        /// <param name="item">物品</param>
        /// <param name="Life">生命等级</param>
        /// <param name="Mana">魔法等级</param>
        /// <param name="Damage">伤害等级</param>
        /// <param name="Defense">防御等级</param>
        /// <param name="Speed">速度等级</param>
        /// <param name="Crit">暴击等级</param>
        /// </summary>
        public static void AccessoriesQualityAttribute(Item item, int Life, int Mana, int Damage, int Defense, int Speed, int Crit)
        {
            item.AGItem().CanLife = Life;
            item.AGItem().CanMana = Mana;
            item.AGItem().CanDamage = Damage;
            item.AGItem().CanDefense = Defense;
            item.AGItem().CanSpeed = Speed;
            item.AGItem().CanCrit = Crit;
        }
        public static void WeaponsQualityAttribute(Item item, int Damage, int Crit)
        {
            item.AGItem().CanDamage = Damage;
            item.AGItem().CanCrit = Crit;
        }
        //默认属性
        public override void SetDefaults(Item item)
        {
        }
        //保存和加载
        public override void LoadData(Item item, TagCompound tag)
        {
            Quality = tag.Get<int>("Quality");

            CanLife = tag.Get<int>("CanLife");
            CanMana = tag.Get<int>("CanMana");
            CanDamage = tag.Get<int>("CanDamage");
            CanDefense = tag.Get<int>("CanDefense");
            CanSpeed = tag.Get<int>("CanSpeed");
            CanCrit = tag.Get<int>("CanCrit");
            //BuildProperties = tag.Get<bool>("BuildProperties");

            Attribute = tag.Get<bool[]>("Attribute");
        }

        public override void SaveData(Item item, TagCompound tag)
        {
            tag["Quality"] = Quality;

            tag["CanLife"] = CanLife;
            tag["CanMana"] = CanMana;
            tag["CanDamage"] = CanDamage;
            tag["CanDefense"] = CanDefense;
            tag["CanSpeed"] = CanSpeed;
            tag["CanCrit"] = CanCrit;
            // tag["BuildProperties"] = BuildProperties;

            tag["Attribute"] = Attribute;
        }
        //同步
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(Quality);
            writer.Write(CanLife);
            writer.Write(CanMana);
            writer.Write(CanDamage);
            writer.Write(CanDefense);
            writer.Write(CanSpeed);
            writer.Write(CanCrit);
            writer.Write(BuildProperties);
            for (int a = 0; a < Attribute.Length; a++)
            {
                writer.Write(Attribute[a]);
            }
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            Quality = reader.ReadInt32();

            CanLife = reader.ReadInt32();
            CanMana = reader.ReadInt32();
            CanDamage = reader.ReadInt32();
            CanDefense = reader.ReadInt32();
            CanSpeed = reader.ReadInt32();
            CanCrit = reader.ReadInt32();
            BuildProperties = reader.ReadBoolean();
            for (int a = 0; a < Attribute.Length; a++)
            {
                Attribute[a] = reader.ReadBoolean();
            }
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (Quality >= 0 && !item.AGItem().BuildProperties)
            {
                if (CanDamage > 0)
                {
                    Damage = CanDamage * ((float)Quality / 2);
                }
                if (CanCrit > 0)
                {
                    Crit = (int)(CanCrit * ((float)Quality / 2));
                }
                item.damage += (int)Damage;
                item.crit += Crit;
                item.AGItem().BuildProperties = true;
            }
        }
        public override void PostReforge(Item item)
        {
            if (Quality >= 0)
            {
                if (CanDamage > 0)
                {
                    Damage = CanDamage * ((float)Quality / 2);
                }
                if (CanCrit > 0)
                {
                    Crit = (int)(CanCrit * ((float)Quality / 2));
                }
                item.damage += (int)Damage;
                item.crit += Crit;
            }
        }
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Name == "稀有" && line.Mod == "DDmod")
            {
                EpicTooltipLine(line, ref yOffset, new Color(0, 150, 255, 0));
                return false;
            }
            if (line.Name == "罕见" && line.Mod == "DDmod")
            {
                EpicTooltipLine(line, ref yOffset, new Color(150, 0, 255, 0));
                return false;
            }
            if (line.Name == "珍宝" && line.Mod == "DDmod")
            {
                EpicTooltipLine(line, ref yOffset, new Color(255, 175, 0, 0));
                return false;
            }
            if (line.Name == "泰拉" && line.Mod == "DDmod")
            {
                EpicTooltipLine(line, ref yOffset, new Color(173, 255, 170, 0));
                return false;
            }
            return true;
        }

        private static float Sc;
        private static bool Bool;
        public static void EpicTooltipLine(DrawableTooltipLine line, ref int yOffset, Color color)
        {
            Vector2 size = FontAssets.MouseText.Value.MeasureString(line.Text);
            DDHelper.BackAndForth(0.3F, 0.5F, 0.02F, ref Sc, ref Bool);
            Main.spriteBatch.Draw(DDTextures.VoidStar.Value, new Vector2(line.X, line.Y - 4) + (size / 2f), null, color, 0f, Utils.Size(DDTextures.VoidStar.Value) / 2f, new Vector2(size.X * Sc / 20, Sc), 0, 0f);
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y), color, line.Rotation, line.Origin, line.BaseScale, line.MaxWidth, line.Spread);
        }
        internal Color ColorFunction(float completionRatio)
        {
            if (稀有度 == 1)
            {
                float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
                return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
                {
                new Color(0, 150, 255),
                }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(0, 150, 255), (float)Math.Pow((double)completionRatio, 1.0));
            }
            else if (稀有度 == 2)
            {
                float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
                return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
                {
                new Color(150, 0, 255)*0.4f,
                }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(150, 0, 255), (float)Math.Pow((double)completionRatio, 1.0));
            }
            else if (稀有度 == 3)
            {
                float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
                return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
                {
                new Color(255, 175, 0)*0.4f,
                }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(255, 175, 0), (float)Math.Pow((double)completionRatio, 1.0));
            }
            return new Color(0, 0, 0, 0);
        }
        internal float WidthFunction(float completionRatio)
        {
            if (稀有度 == 1)
            {
                float widthRatio = Utils.GetLerpValue(8f, 0f, completionRatio, false);
                return MathHelper.Lerp(0, 20f, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
            }
            else if (稀有度 == 2)
            {
                float widthRatio = Utils.GetLerpValue(8f, 0f, completionRatio, false);
                return MathHelper.Lerp(0, 60f, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
            }
            else if (稀有度 == 3)
            {
                float widthRatio = Utils.GetLerpValue(8f, 0f, completionRatio, false);
                return MathHelper.Lerp(0, 100f, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
            }
            return 0;
        }
        internal Trailing TrailDrawer;
        float T2 = 1;
        bool TB = false;
        //绘制
        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            int T = 1;
            if (Main.itemAnimations[item.type] != null)
            {
                T = Main.itemAnimations[item.type].FrameCount;
            }
            if (稀有度 == 1)
            {
                Vector2 position = item.Center - Main.screenPosition;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                for (int n = 0; n < 2; n++)
                {
                    spriteBatch.Draw(DDTextures.VoidStar.Value, position, null, new Color(0, 150, 255) * 0.9f, 0, DDTextures.VoidStar.Size() / 2, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(DDTextures.VoidStar.Value, position, null, new Color(255, 105, 0) * 0.9f, 0, DDTextures.VoidStar.Size() / 2, 0.5f, SpriteEffects.None, 0);
                }
                spriteBatch.Draw(DDTextures.VoidLight.Value, position, null, new Color(0, 150, 255), 0, new Vector2(DDTextures.VoidLight.Width() / 2, DDTextures.VoidLight.Height()), 1.5f, SpriteEffects.None, 0);
                spriteBatch.Draw(DDTextures.VoidLight.Value, position, null, new Color(255, 105, 0), 0, new Vector2(DDTextures.VoidLight.Width() / 2, DDTextures.VoidLight.Height()), 0.75f, SpriteEffects.None, 0);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            if (稀有度 == 2)
            {
                Vector2[] RE = new Vector2[2];
                float ro = (float)(-Math.PI / 2);
                for (int a = 0; a < 2; a++)
                {
                    RE[a] = item.position + Vector2.Normalize(ro.ToRotationVector2()) * (a * 200);
                }
                if (TrailDrawer == null)
                {
                    TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), new Trailing.TrailPointRetrievalFunction(Trailing.RigidPointRetreivalFunction), GameShaders.Misc["贴图拖尾"]);
                }
                GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.FireEffect2);
                GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(0.6f);

                TrailDrawer.Draw(RE, item.Size * 0.5f - Main.screenPosition, 300, null);

                Vector2 position = item.Center - Main.screenPosition;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                for (int n = 0; n < 2; n++)
                {
                    spriteBatch.Draw(DDTextures.VoidStar.Value, position, null, new Color(126, 0, 255), 0, DDTextures.VoidStar.Size() / 2, 1.5f, SpriteEffects.None, 0);
                    spriteBatch.Draw(DDTextures.VoidStar.Value, position, null, new Color(129, 255, 0) * 0.8f, 0, DDTextures.VoidStar.Size() / 2, 0.75f, SpriteEffects.None, 0);
                }
                spriteBatch.Draw(DDTextures.VoidLight.Value, position, null, new Color(126, 0, 255), 0, new Vector2(DDTextures.VoidLight.Width() / 2, DDTextures.VoidLight.Height()), 3f, SpriteEffects.None, 0);
                spriteBatch.Draw(DDTextures.VoidLight.Value, position, null, new Color(129, 255, 0), 0, new Vector2(DDTextures.VoidLight.Width() / 2, DDTextures.VoidLight.Height()), 1.5f, SpriteEffects.None, 0);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            if (稀有度 == 3)
            {
                Vector2[] RE = new Vector2[2];
                float ro = (float)(-Math.PI / 2);
                for (int a = 0; a < 2; a++)
                {
                    RE[a] = item.position + Vector2.Normalize(ro.ToRotationVector2()) * (a * 250);
                }
                if (TrailDrawer == null)
                {
                    TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), new Trailing.TrailPointRetrievalFunction(Trailing.RigidPointRetreivalFunction), GameShaders.Misc["贴图拖尾"]);
                }
                GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.FireEffect2);
                GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(0.6f);

                TrailDrawer.Draw(RE, item.Size * 0.5f - Main.screenPosition, 300, null);

                Vector2 position = item.Center - Main.screenPosition;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                DDHelper.BackAndForth(0.75F, 1, 0.01F, ref T2, ref TB, true);
                for (int n = 0; n < 2; n++)
                {
                    spriteBatch.Draw(DDTextures.GlowEffect.Value, position, null, new Color(255, 175, 0) * 0.8f, 0, DDTextures.GlowEffect.Size() / 2, T2, SpriteEffects.None, 0);
                    spriteBatch.Draw(DDTextures.VoidStar.Value, position, null, new Color(255, 175, 0), 0, DDTextures.VoidStar.Size() / 2, 1.5f, SpriteEffects.None, 0);
                    spriteBatch.Draw(DDTextures.VoidStar.Value, position, null, new Color(0, 80, 255) * 0.8f, 0, DDTextures.VoidStar.Size() / 2, 0.75f, SpriteEffects.None, 0);
                }
                spriteBatch.Draw(DDTextures.VoidLight.Value, position, null, new Color(255, 175, 0), 0, new Vector2(DDTextures.VoidLight.Width() / 2, DDTextures.VoidLight.Height()), 3f, SpriteEffects.None, 0);
                spriteBatch.Draw(DDTextures.VoidLight.Value, position, null, new Color(0, 80, 255), 0, new Vector2(DDTextures.VoidLight.Width() / 2, DDTextures.VoidLight.Height()), 1.5f, SpriteEffects.None, 0);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return true;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (稀有度 == 1)
            {
                tooltips.Add(new TooltipLine(Mod, "稀有", Language.GetTextValue("Mods.DDmod.Tooltips.稀有")));
            }
            if (稀有度 == 2)
            {
                tooltips.Add(new TooltipLine(Mod, "罕见", Language.GetTextValue("Mods.DDmod.Tooltips.罕见")));
            }
            if (稀有度 == 3)
            {
                tooltips.Add(new TooltipLine(Mod, "珍宝", Language.GetTextValue("Mods.DDmod.Tooltips.珍宝")));
            }
            if (稀有度 == 4)
            {
                tooltips.Add(new TooltipLine(Mod, "泰拉", Language.GetTextValue("Mods.DDmod.Tooltips.泰拉")));
            }
            if (Quality >= 0)
            {
                switch (Quality)
                {
                    case 0:
                        tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.Tooltips.平平无奇"))
                        {
                            OverrideColor = new Color(255, 255, 255)
                        });
                        break;
                    case 1:
                        tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.Tooltips.优秀"))
                        {
                            OverrideColor = new Color(55, 255, 55)
                        });
                        break;
                    case 2:
                        tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.Tooltips.精良"))
                        {
                            OverrideColor = new Color(0, 75, 255)
                        });
                        break;
                    case 3:
                        tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.Tooltips.史诗"))
                        {
                            OverrideColor = new Color(175, 0, 255)
                        });
                        break;
                    case 4:
                        tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.Tooltips.传说"))
                        {
                            OverrideColor = new Color(255, 100, 0)
                        });
                        break;
                    case 5:
                        tooltips.Add(new TooltipLine(Mod, "", Language.GetTextValue("Mods.DDmod.Tooltips.超凡"))
                        {
                            OverrideColor = new Color(255, 0, 0)
                        });
                        break;
                }
                if (!item.accessory && item.damage > 0)
                {
                    if (CanDamage > 0)
                    {
                        Damage = CanDamage * ((float)Quality / 2);
                    }
                    if (Damage > 0)
                    {
                        tooltips.Add(new TooltipLine(Mod, "", "+" + (int)Damage + "" + Language.GetTextValue("Mods.DDmod.properties.伤害"))
                        {
                            OverrideColor = new Color(0, 255, 175)
                        });
                    }
                    if (CanCrit > 0)
                    {
                        Crit = (int)(CanCrit * ((float)Quality / 2));
                    }
                    if (Crit > 0)
                    {
                        tooltips.Add(new TooltipLine(Mod, "", "+" + Crit + "%" + Language.GetTextValue("Mods.DDmod.properties.暴击"))
                        {
                            OverrideColor = new Color(0, 255, 175)
                        });
                    }
                    return;
                }
                if (Quality == 0)
                {
                    return;
                }
                if (CanLife > 0)
                {
                    Life = (int)(CanLife * ((float)Quality / 2));
                }
                if (CanMana > 0)
                {
                    Mana = (int)(CanMana * ((float)Quality / 2));
                }
                if (CanDamage > 0)
                {
                    Damage = CanDamage * ((float)Quality / 2);
                }
                if (CanDefense > 0)
                {
                    Defense = (int)(CanDefense * ((float)Quality / 2));
                }
                if (CanSpeed > 0)
                {
                    Speed = (CanSpeed * ((float)Quality / 2));
                }
                if (CanCrit > 0)
                {
                    Crit = (int)(CanCrit * ((float)Quality / 2));
                }
                if (Life > 0)
                {
                    tooltips.Add(new TooltipLine(Mod, "", "+" + Life + Language.GetTextValue("Mods.DDmod.properties.生命"))
                    {
                        OverrideColor = new Color(0, 255, 175)
                    });
                }
                if (Mana > 0)
                {
                    tooltips.Add(new TooltipLine(Mod, "", "+" + Mana + Language.GetTextValue("Mods.DDmod.properties.魔力"))
                    {
                        OverrideColor = new Color(0, 255, 175)
                    });
                }
                if (Damage > 0)
                {
                    tooltips.Add(new TooltipLine(Mod, "", "+" + Damage + "%" + Language.GetTextValue("Mods.DDmod.properties.伤害"))
                    {
                        OverrideColor = new Color(0, 255, 175)
                    });

                }
                if (Defense > 0)
                {
                    tooltips.Add(new TooltipLine(Mod, "", "+" + Defense + Language.GetTextValue("Mods.DDmod.properties.防御"))
                    {
                        OverrideColor = new Color(0, 255, 175)
                    });
                }
                if (Speed > 0)
                {
                    tooltips.Add(new TooltipLine(Mod, "", "+" + Speed + "%" + Language.GetTextValue("Mods.DDmod.properties.移动速度"))
                    {
                        OverrideColor = new Color(0, 255, 175)
                    });
                }
                if (Crit > 0)
                {
                    tooltips.Add(new TooltipLine(Mod, "", "+" + Crit + "%" + Language.GetTextValue("Mods.DDmod.properties.暴击"))
                    {
                        OverrideColor = new Color(0, 255, 175)
                    });
                }
            }
        }
        //饰品加成
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (Quality > 0)
            {
                if (CanLife > 0)
                {
                    Life = (int)(CanLife * ((float)Quality / 2));
                    player.statLifeMax2 += Life;
                }
                if (CanMana > 0)
                {
                    Mana = (int)(CanMana * ((float)Quality / 2));
                    player.statManaMax2 += Mana;
                }
                if (CanDamage > 0)
                {
                    Damage = CanDamage * ((float)Quality / 2);
                    player.GetDamage(DamageClass.Generic) += Damage;
                }
                if (CanDefense > 0)
                {
                    Defense = (int)(CanDefense * ((float)Quality / 2));
                    player.statDefense += Defense;
                }
                if (CanSpeed > 0)
                {
                    Speed = (CanSpeed * ((float)Quality / 2));
                    player.moveSpeed += Speed;
                }
                if (CanCrit > 0)
                {
                    Crit = (int)(CanCrit * ((float)Quality / 2));
                    player.GetCritChance(DamageClass.Generic) += Crit;
                }
            }
        }
    }
}