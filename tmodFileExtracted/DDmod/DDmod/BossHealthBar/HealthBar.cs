using DDmod.NoContent.Config;
using Terraria.Graphics;
using Terraria.ID;
using static System.Net.Mime.MediaTypeNames;

namespace DDmod.BossHealthBar
{
    /// <summary>
    /// Class handles the drawing and effects of the health bar, which is in turn manipulated elsewhere.
    /// One per type of NPC, see BossDisplayInfo
    /// </summary>
    public class HealthBar
    {
        public enum DisplayType
        {
            Standard, // Uses npc.life and npc.realLife
            Multiple, // Counts all NPCs of a typelist and uses collective life vs lifeMax
            Phase, // Checks for NPCs in the typelist in descending order, using lifeMax as a total.
            Disabled, // Just don't show
        }

        /// <summary>
        /// Global variable for checking if mouse is over bars, in which case fade out
        /// </summary>
        public static int MouseOver = 0;

        /// <summary> Always call the small bar textures when drawing. Typically reserved for minibosses </summary>
        public bool ForceSmall = false;
        /// <summary> Never show the chip bar graphics (numbers are still shown) </summary>
        public bool ForceNoChip = false;
        public bool LoopMidBar = false;
        /*
        /// <summary> Only allow one of these bars to show regardless of how many are active </summary>
        public bool ForceUnique = false;
        */

        /// <summary> Check if the provided bar fill texture has some kind of transparency 
        /// on its right edge, this determines how the damage display bar is drawn. </summary>
        protected bool IsSlanted
        {
            get
            {
                try
                {
                    Texture2D barFill = (Texture2D)ModContent.Request<Texture2D>(DDSystem.HealthBar + "默认血条Tail");
                    Color[] barColour1D = new Color[barFill.Width * barFill.Height];
                    barFill.GetData(barColour1D);

                    int x = 0;
                    int yStart = -1;
                    int yEnd = barFill.Height - 1;
                    Color c;
                    // Check along left side of texture to see where the bar is
                    for (int y = 0; y < barFill.Height; y++)
                    {
                        c = barColour1D[x + y * barFill.Width];
                        // Look for first left side pixel
                        if (yStart == -1)
                        {
                            if (c.A > 0) // Has a colour, this is the start
                            {
                                yStart = y;
                            }
                        }
                        // Look for last bottom left pixel
                        else
                        {
                            if (c.A == 0) // Has no colour, go back 1 row
                            {
                                yEnd = y - 1;
                                break;
                            }
                        }
                    }

                    x = barFill.Width - 1;
                    // Check endpoints on right side of texture for any transparency
                    c = barColour1D[x + yStart * barFill.Width];
                    if (c.A < 255) { return true; }
                    c = barColour1D[x + yEnd * barFill.Width];
                    if (c.A < 255) { return true; }
                }
                catch // Something went wrong? go to default (false)
                { }
                return false;
            }
        }

        public DisplayType DisplayMode = DisplayType.Standard;

        /// <summary>
        /// All NPC types collected in this health bar, see DisplayType.Multiple
        /// </summary>
        internal int[] multiNPCType = null;
        internal long multiNPCLifeMax = 0;
        internal bool multiNPCLIfeMaxRecordedOnExpert = false;
        /// <summary>
        /// 多NPC计数
        /// </summary>
        internal ushort multiShowCount = 0;

        internal static void ResetStaticVars()
        {
            if (MouseOver > 0) MouseOver--;
            // Turn off multishow again for this frame
            foreach (KeyValuePair<int, HealthBar> kvp in BossDisplayInfo.NPCHealthBars)
            {
                kvp.Value.multiShowCount = 0;
                //kvp.Value.drawnUnique = false;
            }

        }

        #region Default Textures


        /// <summary>
        /// The NPC in Main.npc to use as the source of the head icon. 
        /// Most likely Main.npc[NPC.FindFirstNPC( some_npc_type )];
        /// </summary>
        /// <returns></returns>
        protected virtual NPC GetBossHeadSource(NPC npc)
        {
            return npc;
        }
        protected virtual string GetBossDisplayNameNPC(NPC npc)
        {
            return npc.GivenOrTypeName;
        }

        /// <summary>
        /// Just in case you REALLY want to override standard behaviour and draw this health bar.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="TooFarAway"></param>
        /// <returns>true to show when normally it might not</returns>
        public virtual bool ShowHealthBarOverride(NPC npc, bool TooFarAway)
        {
            return true;
        }

        /// <summary>
        /// Just in case you REALLY want to hide the health bar no matter what. This overrides EVERYTHING else.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="TooFarAway"></param>
        /// <returns>true to hide always</returns>
        public virtual bool HideHealthBarOverride(NPC npc, bool TooFarAway)
        {
            return false;
        }

        /// <summary>
        /// Just in case you want to change up the values displayed for some reason.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="life"></param>
        /// <param name="lifeMax"></param>
        protected virtual void ShowHealthBarLifeOverride(NPC npc, ref long life, ref long lifeMax) { }

        public virtual bool DisableFadeInFor(int type) { return false; }
        public virtual bool DisableFadeOutFor(int type) { return false; }

        /// <summary> Called after a healthbar has been registered, in case you need to initialise some final things. </summary>
        public virtual void OnRegister() { }
        #endregion

        public virtual Texture2D GetBossHeadTextureOrNull(NPC npc)
        {
            if (npc.NPCHB().MiniBoss)
            {
                return null;
            }
            if(NPCHealthBar.Headoffset[npc.type].Length()>1000)
            {
                return null;
            }
            if (NPCHealthBar.BossHead[npc.type] != null)
            {
                return NPCHealthBar.BossHead[npc.type].Value;
            }
            int headSlot = GetBossHeadSource(npc).GetBossHeadTextureIndex();
            // No slot, but this is a multi bar?
            if (headSlot < 0 && DisplayMode == DisplayType.Multiple)
            {
                // Search through npcTypes for a head that may match
                foreach (NPC n in Main.npc)
                {
                    foreach (int type in multiNPCType)
                    {
                        if (n.type == type)
                        {
                            headSlot = n.GetBossHeadTextureIndex();
                            if (headSlot > -1) break;
                        }
                    }
                    if (headSlot > -1) break;
                }
            }

            if (headSlot > -1)
            {
                try
                {
                    return (Texture2D)TextureAssets.NpcHeadBoss[headSlot];
                }
                catch { return null; }
            }
            return null;
        }
        //绘制条
        public int DrawHealthBarDefault(SpriteBatch spriteBatch, float Alpha, int stackY, int maxStackY, long life, long lifeMax, NPC npc, bool Death)
        {
            if (NPCHealthBar.Mid[npc.type] == null) return 0;
            Texture2D barM = NPCHealthBar.Mid[npc.type].Value;
            int x, y, width;

            //长度
            width = (int)(Main.screenWidth * 0.35f);
            if (npc.NPCHB().Multiple)
            {
                width = (int)(Main.screenWidth * 0.5f);
            }
            x = Main.screenWidth / 2 - (width / 2);
            y = stackY;
            DHealthBar.HealthPosition(npc, ref x, ref y, ref width);
            if (multiShowCount > 0 && (DisplayMode == DisplayType.Multiple || DisplayMode == DisplayType.Phase))
            {
                multiShowCount++;
                return y; // 阻止多个npc共用一个血条时绘制多个血条
            }
            //会小幅度修改
            y -= barM.Height / npc.NPCHB().Midframe;
            if (ModContent.GetInstance<DDHealthBar>().ShowTarget || ModContent.GetInstance<DDHealthBar>().DamageandDefense)
            {
                //y -= 32;
                y -= (int)(32*npc.NPCHB().TextScale);
                y += (int)(20 * (1 - npc.NPCHB().TextScale));
            }
            return DrawHealthBar(spriteBatch, x, y, width, Alpha, life, lifeMax, npc, Death);
        }
        public void DrawHealthBarDefault2(SpriteBatch spriteBatch, float Alpha, int stackY, int maxStackY, long life, long lifeMax, NPC npc, bool Death)
        {
            Texture2D barM = NPCHealthBar.Mid[npc.type].Value;
            int x, y, width;

            //长度
            width = (int)(Main.screenWidth * 0.5f);

            x = Main.screenWidth / 2 - (width / 2);
            y = stackY;
            DHealthBar.HealthPosition(npc, ref x, ref y, ref width);
            //会小幅度修改
            y -= barM.Height / npc.NPCHB().Midframe;
            if (ModContent.GetInstance<DDHealthBar>().ShowTarget || ModContent.GetInstance<DDHealthBar>().DamageandDefense)
            {
                y -= (int)(32 * npc.NPCHB().TextScale);
                y += (int)(20 * (1 - npc.NPCHB().TextScale));
            }
            DrawHealthBar(spriteBatch, x, y, width, Alpha, life, lifeMax, npc, Death);
        }
        public int DrawHealthBar(SpriteBatch spriteBatch, int XLeft, int yTop, int BarLength, float Alpha, long life, long lifeMax, NPC npc, bool Death)
        {

            string displayName = "";
            ManageMultipleNPCVars(ref life, ref lifeMax, ref displayName);
            ShowHealthBarLifeOverride(npc, ref life, ref lifeMax);

            Color frameColour = new Color(1f, 1f, 1f);
            Color blackColour = new Color(0f, 0f, 0f);
            Color barColour = Color.White;
            frameColour *= Alpha;
            blackColour *= Alpha * Alpha;
            barColour *= Alpha;
            Texture2D bossHead = GetBossHeadTextureOrNull(npc);

            //头,身,内,尾,底,盾,无敌
            Texture2D Head, Mid, Fill, Tail, End, Shield, Lock;

            Head = NPCHealthBar.Head[npc.type].Value;
            Mid = NPCHealthBar.Mid[npc.type].Value;
            Fill = NPCHealthBar.Fill[npc.type].Value;
            Tail = NPCHealthBar.Tail[npc.type].Value;
            End = NPCHealthBar.End[npc.type].Value;
            Shield = NPCHealthBar.Shield[npc.type].Value;
            Lock = NPCHealthBar.Lock[npc.type].Value;

            if (npc.NPCHB().Head2 != null) Head = npc.NPCHB().Head2.Value;

            if (npc.NPCHB().Mid2 != null) Mid = npc.NPCHB().Mid2.Value;

            if (npc.NPCHB().Fill2 != null) Fill = npc.NPCHB().Fill2.Value;

            if (npc.NPCHB().Tail2 != null) Tail = npc.NPCHB().Tail2.Value;

            if (npc.NPCHB().End2 != null) End = npc.NPCHB().End2.Value;

            if (npc.NPCHB().Shield2 != null) Shield = npc.NPCHB().Shield2.Value;

            if (npc.NPCHB().Lock2 != null) Lock = npc.NPCHB().Lock2.Value;

            DHealthBar.DrawHealthBarY(npc, Mid, ref yTop);
            //显示血量
            bool dontTake = npc.dontTakeDamage;
            string dontTakeDamage = dontTake ? Language.GetTextValue("Mods.DDmod.HealthBarText.dontTakeDamage") : (life + "/" + lifeMax);
            int[] NPCT = npc.NPCHB().HealthNPCType;
            if (NPCT != null)
            {
                for (int a = 0; a < NPCT.Length; a++)
                {
                    for (int He = 0; He < 200; He++)
                    {
                        if (Main.npc[He].active && Main.npc[He].type == NPCT[a] && DHealthBar.Master(npc, Main.npc[He]))
                        {
                            life = Main.npc[He].life;
                            lifeMax = Main.npc[He].lifeMax;
                            ManageMultipleNPCVars2(npc, ref life, ref lifeMax, ref displayName);
                            dontTake = Main.npc[He].dontTakeDamage;
                            dontTakeDamage = dontTake ? Language.GetTextValue("Mods.DDmod.HealthBarText.dontTakeDamage") : (life + "/" + lifeMax);
                        }
                    }
                }
            }
            //四柱
            bool Tower = false;
            if (npc.type == NPCID.LunarTowerVortex && ShieldStrengthTowerVortex > 0)
            {
                life = ShieldStrengthTowerVortex;
                Tower = true;
            }
            if (npc.type == NPCID.LunarTowerSolar && ShieldStrengthTowerSolar > 0)
            {
                life = ShieldStrengthTowerSolar;
                Tower = true;
            }
            if (npc.type == NPCID.LunarTowerNebula && ShieldStrengthTowerNebula > 0)
            {
                life = ShieldStrengthTowerNebula;
                Tower = true;
            }
            if (npc.type == NPCID.LunarTowerStardust && ShieldStrengthTowerStardust > 0)
            {
                life = ShieldStrengthTowerStardust;
                Tower = true;
            }
            if ((npc.type == 422 || npc.type == 517 || npc.type == 507 || npc.type == 493) && Tower)
            {
                dontTake = false;
                lifeMax = ShieldStrengthTowerMax;
                dontTakeDamage = dontTake ? Language.GetTextValue("Mods.DDmod.HealthBarText.dontTakeDamage") : (life + "/" + lifeMax);
            }

            // Length of bar is set relative to the screen
            // Centre, - bar length, eg. 500 * (1f - 0.4f or 0.6f)



            // The very far left where the side frames start
            Vector2 FrameTopLeft = new Vector2(XLeft - Head.Width, yTop);



            //填充条
            drawHealthBarFill(spriteBatch, life, lifeMax, Fill, End, Shield, Lock, BarLength, XLeft, yTop, Head.Width, Mid.Width, barColour, npc, dontTake);

            //绘制填充物
            drawHealthBarFrame(spriteBatch, blackColour, frameColour, Head, Mid, Tail, BarLength, XLeft, yTop, FrameTopLeft, npc, Death);

            //绘制npc大头贴
            if (bossHead != null)
            {
                int headOffsetX = (bossHead.Width % 4 != 0) ? 1 : 0;
                int headOffsetY = (bossHead.Height % 4 != 0) ? 1 : 0;
                if (bossHead.Width % 4 < 2)
                {
                    spriteBatch.Draw(bossHead, FrameTopLeft + new Vector2(66 + headOffsetX, 22 + headOffsetY) + NPCHealthBar.Headoffset[npc.type], null, frameColour, 0, bossHead.Size() / 2, 1, 0, 0);
                }
                else
                {
                    spriteBatch.Draw(bossHead, FrameTopLeft + new Vector2(65 + headOffsetX, 22 + headOffsetY) + NPCHealthBar.Headoffset[npc.type], null, frameColour, 0, bossHead.Size() / 2, 1, 0, 0);

                }
            }

            if (DisplayMode != DisplayType.Multiple)
            {
                displayName = GetBossDisplayNameNPC(npc);
            }
            #region Draw text
            //显示百分比
            float A = ((float)life / lifeMax) * 100;
            string H = A.ToString("F2");
            string Percentage = " (" + H + "%)";
            if (!ModContent.GetInstance<DDHealthBar>().ShowPercentage)
            {
                Percentage = "";
            }
            //显示目标
            string player = Language.GetTextValue("Mods.DDmod.HealthBarText.Player");
            string player2 = "";
            if (npc.target >= 0 && npc.target <= 255)
            {
                player = Main.player[npc.target].name;
                if (player.Length > 5)
                {
                    player = player.Remove(5, player.Length - 5);
                    player2 = "...";
                }
            }

            if (!ModContent.GetInstance<DDHealthBar>().ShowLife)
            {
                dontTakeDamage = "";
            }
            DHealthBar.ModifyName(npc, ref displayName);
            string text = string.Concat(displayName + ": " + dontTakeDamage + Percentage);

            if (!ModContent.GetInstance<DDHealthBar>().ShowTarget && !ModContent.GetInstance<DDHealthBar>().ShowLife && !ModContent.GetInstance<DDHealthBar>().ShowPercentage)
            {
                text = string.Concat(displayName);
            }
            if (!ModContent.GetInstance<DDHealthBar>().ShowName)
            {
                text = string.Concat(dontTakeDamage + Percentage);
            }
            Vector2 position = new Vector2(XLeft + BarLength / 2, 4 + yTop + Mid.Height / npc.NPCHB().Midframe / 2) + npc.NPCHB().TextPosition;
            Vector2 origin = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One, 0) / 2;
            float scale = npc.NPCHB().TextScale;
            // Draw border
            DrawText(spriteBatch, text, position, blackColour, frameColour, origin, scale);


            position = new Vector2(XLeft + BarLength / 2, 4 + yTop - 16 * scale) + npc.NPCHB().TextPosition;
            origin = ChatManager.GetStringSize(FontAssets.MouseText.Value, player + player2, Vector2.One, 0) / 2;
            if (ModContent.GetInstance<DDHealthBar>().ShowTarget)
            {
                if (npc.target >= 0 && npc.target <= 255)
                {
                    Main.MapPlayerRenderer.DrawPlayerHead(Main.Camera, Main.player[npc.target], position-new Vector2(origin.X+18, 0), 0, scale, Color.White);
                }
                DrawText(spriteBatch, player + player2, position, blackColour, frameColour, new Vector2(origin.X, 0), scale);
            }
            #endregion

            // 淡化
            if (MouseOver < 2)
            {
                if (Main.mouseY > yTop - 46 && Main.mouseY < yTop + Mid.Height + 46)
                {
                    if (Main.mouseX > XLeft - 100 && Main.mouseX < XLeft + BarLength + 100)
                    {
                        MouseOver = 2;
                    }
                }
            }
            DHealthBar.DrawHealthBarYActive(npc, Mid, ref yTop);
            return yTop;
        }

        /// <summary>
        /// 替换生命值和生命最大值以尝试在一个生命条下收集多个NPC
        /// </summary>
        /// <param name="life"></param>
        /// <param name="lifeMax"></param>
        public void DrawText(SpriteBatch spriteBatch, string text, Vector2 position, Color blackColour, Color frameColour, Vector2 origin, float scale)
        {
            for (int y = -2; y <= 2; y++)
            {
                for (int x = -2; x <= 2; x++)
                {
                    DynamicSpriteFontExtensionMethods.DrawString(
                        spriteBatch,
                        FontAssets.MouseText.Value,
                        text,
                        position + new Vector2(x, y),
                        blackColour, 0f,
                        origin,
                        scale, SpriteEffects.None, 0f);
                }
            }

            for (int y = -2; y <= 2; y++)
            {
                for (int x = -2; x <= 2; x++)
                {
                    DynamicSpriteFontExtensionMethods.DrawString(
                        spriteBatch,
                        FontAssets.MouseText.Value,
                        text,
                        position + new Vector2(x, y - 0),
                        blackColour, 0f,
                        origin,
                        scale, SpriteEffects.None, 0f);
                }
            }
            DynamicSpriteFontExtensionMethods.DrawString(
                spriteBatch,
                FontAssets.MouseText.Value,
                text,
                position - new Vector2(0, 0),
                frameColour, 0f,
                origin,
                scale, SpriteEffects.None, 0f);
        }
        private void ManageMultipleNPCVars(ref long life, ref long lifeMax, ref string displayName)
        {
            if (DisplayMode == DisplayType.Multiple && multiNPCType != null)
            {
                life = 0; lifeMax = 0;

                // Reset when the life max would change (pretty much only during switching to expert mode)
                if (multiNPCLIfeMaxRecordedOnExpert != Main.expertMode)
                {
                    multiNPCLIfeMaxRecordedOnExpert = Main.expertMode;
                    multiNPCLifeMax = 0;
                }

                // First run, only include active
                foreach (int type in multiNPCType)
                {
                    foreach (NPC n in Main.npc)
                    {
                        if (n.type == type)
                        {
                            // Get the names in order of priority
                            if (displayName == "")
                            {
                                displayName = GetBossDisplayNameNPC(n);
                            }
                            if (!n.active) continue;
                            life += n.life;
                            lifeMax += n.lifeMax;
                        }
                    }
                }
                // Get the highest recorded value
                if (multiNPCLifeMax < lifeMax) multiNPCLifeMax = lifeMax;
                lifeMax = multiNPCLifeMax;

                // Set to true to prevent further draws of the same thing this frame (see BossDisplayInfo)
                multiShowCount++;
            }
        }
        private void ManageMultipleNPCVars2(NPC npc, ref long life, ref long lifeMax, ref string displayName)
        {
            if (npc.NPCHB().HealthNPCType != null)
            {
                life = 0; lifeMax = 0;

                int[] NPCT = npc.NPCHB().HealthNPCType;

                foreach (NPC n in Main.npc)
                {
                    for (int A = 0; A < NPCT.Length; A++)
                    {
                        if (n.active && DHealthBar.Master(npc, n) && n.type == NPCT[A])
                        {
                            if (displayName == "")
                            {
                                displayName = GetBossDisplayNameNPC(n);
                            }
                            life += n.life;
                            lifeMax += n.lifeMax;

                        }
                    }
                }
                if (npc.NPCHB().multiNPCLifeMax < lifeMax) npc.NPCHB().multiNPCLifeMax = lifeMax;
                lifeMax = npc.NPCHB().multiNPCLifeMax;

                multiShowCount++;
            }
        }
        //绘制,血量,最大血量,条,低条,盾条,无敌锁,长度,x位置,y位置,头长度,尾长度,颜色,npc,无敌
        private int drawHealthBarFill(SpriteBatch spriteBatch, long life, long lifeMax, Texture2D fill, Texture2D end, Texture2D Shield, Texture2D Lock, int barLength, int X, int Y, int Head, int Mid, Color barColour, NPC npc, bool dontTake)
        {
            NPCHealthBar gnpc = npc.NPCHB();
            float F = (barLength + Mid) / (Mid * npc.NPCHB().Scale) % 1;
            F = (F == 0 ? 1 : F);
            float Length = (barLength + Mid * (1 + (1 - F)) + gnpc.RightLength + gnpc.LeftLength) + 4;
            float ActualLength = Length * ((float)life / lifeMax);
            float ActualLength2 = Length * ((float)npc.life / npc.lifeMax);
            if (npc.NPCHB().FakeLength == 0)
            {
                npc.NPCHB().FakeLength = (barLength + Mid + gnpc.RightLength + gnpc.LeftLength);
            }
            if (npc.NPCHB().FakeLength > ActualLength)
            {
                if (npc.NPCHB().FakeLengthTime < 0)
                {
                    npc.NPCHB().FakeLength -= Length / 100;
                }
                npc.NPCHB().FakeLengthTime--;
            }
            else
            {
                npc.NPCHB().FakeLengthTime = 30;
                npc.NPCHB().FakeLength = ActualLength;
            }

            spriteBatch.Draw(end, new Vector2(X - gnpc.LeftLength - Mid, Y), new Rectangle(0, 0, 2, end.Height), barColour, 0f, Vector2.Zero, new Vector2(Length / 2, npc.NPCHB().Scale), SpriteEffects.None, 0f);

            spriteBatch.Draw(end, new Vector2(X + Length - Mid - gnpc.LeftLength, Y), new Rectangle(2, 0, end.Width - 2, end.Height), barColour, 0f, Vector2.Zero, new Vector2(1, npc.NPCHB().Scale), SpriteEffects.None, 0f);
            bool Repeat = false;

            int[] NPCT = npc.NPCHB().HealthNPCType;
            if (NPCT != null)
            {
                for (int He = 0; He < 200; He++)
                {
                    for (int A = 0; A < NPCT.Length; A++)
                    {
                        if (Main.npc[He].active && Main.npc[He].type == npc.NPCHB().HealthNPCType[A] && DHealthBar.Master(npc, Main.npc[He]))
                        {
                            Repeat = true;
                        }
                    }
                }
            }
            if (npc.type == NPCID.LunarTowerVortex && ShieldStrengthTowerVortex > 0)
            {
                Repeat = true;
            }
            if (npc.type == NPCID.LunarTowerSolar && ShieldStrengthTowerSolar > 0)
            {
                Repeat = true;
            }
            if (npc.type == NPCID.LunarTowerNebula && ShieldStrengthTowerNebula > 0)
            {
                Repeat = true;
            }
            if (npc.type == NPCID.LunarTowerStardust && ShieldStrengthTowerStardust > 0)
            {
                Repeat = true;
            }
            if (Repeat)
            {
                //if (npc.dontTakeDamage) barColour = new Color(155, 155, 155, 255);

                spriteBatch.Draw(fill, new Vector2(X - 1 - gnpc.LeftLength - Mid, Y), new Rectangle(0, 0, 6, fill.Height), barColour, 0f, Vector2.Zero, new Vector2(ActualLength2 / 6, npc.NPCHB().Scale), SpriteEffects.None, 0f);

                spriteBatch.Draw(fill, new Vector2(X - 1 + ActualLength2 - Mid - gnpc.LeftLength, Y), new Rectangle(6, 0, fill.Width - 6, fill.Height), barColour, 0f, Vector2.Zero, new Vector2(1, npc.NPCHB().Scale), SpriteEffects.None, 0f);


                spriteBatch.Draw(fill, new Vector2(X - 1 - gnpc.LeftLength - Mid, Y), new Rectangle(0, 0, 6, fill.Height), barColour * 0.3F, 0f, Vector2.Zero, new Vector2(npc.NPCHB().FakeLength / 6, npc.NPCHB().Scale), SpriteEffects.None, 0f);

                spriteBatch.Draw(fill, new Vector2(X - 1 + npc.NPCHB().FakeLength - Mid - gnpc.LeftLength, Y), new Rectangle(6, 0, fill.Width - 6, fill.Height), barColour * 0.3F, 0f, Vector2.Zero, new Vector2(1, 1) * npc.NPCHB().Scale, SpriteEffects.None, 0f);

                //barColour = new Color(255, 255, 255, 255);

                spriteBatch.Draw(Shield, new Vector2(X - 1 - gnpc.LeftLength - Mid, Y), new Rectangle(0, 0, 1, Shield.Height), barColour, 0f, Vector2.Zero, new Vector2(ActualLength, npc.NPCHB().Scale), SpriteEffects.None, 0f);

                spriteBatch.Draw(Shield, new Vector2(X - 1 + ActualLength - Mid - gnpc.LeftLength, Y), new Rectangle(0, 0, Shield.Width, Shield.Height), barColour, 0f, Vector2.Zero, new Vector2(1, npc.NPCHB().Scale), SpriteEffects.None, 0f);

                spriteBatch.Draw(Shield, new Vector2(X - 1 - gnpc.LeftLength - Mid, Y), new Rectangle(0, 0, 1, Shield.Height), barColour * 0.3F, 0f, Vector2.Zero, new Vector2(npc.NPCHB().FakeLength, npc.NPCHB().Scale), SpriteEffects.None, 0f);

                spriteBatch.Draw(Shield, new Vector2(X - 1 + npc.NPCHB().FakeLength - Mid - gnpc.LeftLength, Y), new Rectangle(0, 0, Shield.Width, Shield.Height), barColour * 0.3F, 0f, Vector2.Zero, new Vector2(1, 1) * npc.NPCHB().Scale, SpriteEffects.None, 0f);

                if (npc.NPCHB().damageTime >= 6 && ModContent.GetInstance<DDHealthBar>().Text && npc.NPCHB().HealthNPCType != null)
                {
                    int damage = npc.NPCHB().damage;
                    for (int a = 0; a < 200; a++)
                    {
                        for (int b = 0; b < npc.NPCHB().HealthNPCType.Length; b++)
                        {
                            if (Main.npc[a].active && Main.npc[a].Dnpc().Master == npc.whoAmI && npc.NPCHB().HealthNPCType[b] == Main.npc[a].type)
                            {
                                damage += Main.npc[a].NPCHB().damage;
                            }
                        }
                    }
                    if (damage > 0)
                        UITextDraw.NewText(new Vector2(X + ActualLength - Mid - gnpc.LeftLength, Y), damage.ToString(), 40, scale: 0.4F, color: new Color(0, 150, 255), vector: new Vector2(Main.rand.NextFloat(0.2F, 1), Main.rand.NextFloat(-2, -4)), type: 2);
                    for (int a = 0; a < 200; a++)
                    {
                        for (int b = 0; b < npc.NPCHB().HealthNPCType.Length; b++)
                        {
                            if (Main.npc[a].active && Main.npc[a].Dnpc().Master == npc.whoAmI && npc.NPCHB().HealthNPCType[b] == Main.npc[a].type)
                            {
                                Main.npc[a].NPCHB().damage = 0;
                            }
                        }
                    }
                    npc.NPCHB().damageTime = 0;
                    npc.NPCHB().damage = 0;
                }
            }
            else
            {
                //if (npc.dontTakeDamage) barColour = new Color(155, 155, 155, 255);

                spriteBatch.Draw(fill, new Vector2(X - 1 - gnpc.LeftLength - Mid, Y), new Rectangle(0, 0, 2, fill.Height), barColour, 0f, Vector2.Zero, new Vector2(ActualLength / 2, npc.NPCHB().Scale), SpriteEffects.None, 0f);

                spriteBatch.Draw(fill, new Vector2(X - 1 + ActualLength - Mid - gnpc.LeftLength, Y), new Rectangle(2, 0, fill.Width - 2, fill.Height), barColour, 0f, Vector2.Zero, new Vector2(1, npc.NPCHB().Scale), SpriteEffects.None, 0f);

                spriteBatch.Draw(fill, new Vector2(X - 1 - gnpc.LeftLength - Mid, Y), new Rectangle(0, 0, 2, fill.Height), barColour * 0.3F, 0f, Vector2.Zero, new Vector2(npc.NPCHB().FakeLength / 2, npc.NPCHB().Scale), SpriteEffects.None, 0f);

                spriteBatch.Draw(fill, new Vector2(X - 1 + npc.NPCHB().FakeLength - Mid - gnpc.LeftLength, Y), new Rectangle(2, 0, fill.Width - 2, fill.Height), barColour * 0.3F, 0f, Vector2.Zero, new Vector2(1, 1) * npc.NPCHB().Scale, SpriteEffects.None, 0f);
                if (npc.NPCHB().damageTime >= 6 && ModContent.GetInstance<DDHealthBar>().Text)
                {
                    if (npc.NPCHB().damage > 0)
                        UITextDraw.NewText(new Vector2(X + ActualLength - Mid - gnpc.LeftLength, Y), npc.NPCHB().damage.ToString(), 40, scale: 0.4F, color: new Color(255, 100, 100), vector: new Vector2(Main.rand.NextFloat(0.2F, 1), Main.rand.NextFloat(-2, -4)), type: 2);
                    npc.NPCHB().damageTime = 0;
                    npc.NPCHB().damage = 0;
                }
            }
            if (NPCHealthBar.FillE[npc.type] != null)
            {
                Texture2D texture = NPCHealthBar.FillE[npc.type].Value;
                Color barColour2 = barColour;
                spriteBatch.Draw(texture, new Vector2(X - gnpc.LeftLength - Mid, Y), new Rectangle(0, 0, 2, texture.Height), barColour, 0f, Vector2.Zero, new Vector2(Length / 2, npc.NPCHB().Scale), SpriteEffects.None, 0f);

                spriteBatch.Draw(texture, new Vector2(X + Length - Mid - gnpc.LeftLength, Y), new Rectangle(2, 0, texture.Width - 2, texture.Height), barColour, 0f, Vector2.Zero, new Vector2(1, npc.NPCHB().Scale), SpriteEffects.None, 0f);
            }
            if (dontTake)
            {
                int XRight = X + barLength;
                for (int i = 1; i <= (barLength + Lock.Width) / Lock.Width; i++)
                {
                    spriteBatch.Draw(Lock, new Vector2(XRight - Lock.Width * i, Y), new Rectangle?(new Rectangle(0, Lock.Height / npc.NPCHB().Lockframe * gnpc.LockframeCurrent, Lock.Width, Lock.Height / npc.NPCHB().Lockframe)), barColour, 0f, Vector2.Zero, npc.NPCHB().Scale, SpriteEffects.None, 0f);
                }
            }
            return (int)ActualLength;
        }
        private static Dictionary<Texture2D, Color[]> Ctexture = new Dictionary<Texture2D, Color[]>();
        public void UDust(Texture2D texture, Vector2 Position, int Frame, int jump, float Scals)
        {
            if (Ctexture == null)
            {
                Ctexture = new Dictionary<Texture2D, Color[]>();
            }
            Color[] colors;
            if (!Ctexture.ContainsKey(texture))
            {
                colors = DDHelper.GetColors(texture);
                Ctexture.Add(texture, colors);
            }
            colors = Ctexture[texture];
            for (int i = 0; i < colors.Length / Frame; i += jump)
            {
                float x = i % texture.Width;
                float y = i / texture.Width;
                for (int r = 0; r < jump; r++)
                {
                    if (colors[i] == new Color(0, 0, 0, 0))
                    {
                        i++;
                    }
                    if (i >= colors.Length / Frame - 1 || colors[i] != new Color(0, 0, 0, 0))
                    {
                        break;
                    }
                }
                if (colors[i] != new Color(0, 0, 0, 0))
                {
                    Dust dust = Main.dust[NewDust(Position + (new Vector2(x, y) - texture.Size() / 2), 1, 1, 267, 0, 0, 0, colors[i], 2.5f)];
                    //dust.customData = true;
                    dust.noGravity = true;
                    dust.velocity = ((new Vector2(x, y) - texture.Size() / 2)) * 0.6f;
                    UIDustDraw.NewDust(Position + (new Vector2(x, y)), 1, colors[i], new Vector2(Main.rand.NextFloat(-3, 3), -3), Scals);
                }

            }
        }
        public void Extra(SpriteBatch spriteBatch, Color frameColour, Texture2D Head, Texture2D Mid, float barLength, Vector2 FrameTopLeft, NPC npc)
        {
            //地牢守护者
            {
                bool DD2 = npc.type == 551
                || npc.type == 564
                || npc.type == 565
                || npc.type == 576
                || npc.type == 577;
                if (DD2)
                {
                    Vector2 Midpo = FrameTopLeft + new Vector2(Head.Width, 0);
                    for (float i = 1; i < barLength; i++)
                    {
                        if (i == (int)(barLength * 0.3F) || i == (int)(barLength * 0.7F))
                        {
                            spriteBatch.Draw(NPCHealthBar.DD2E.Value, Midpo, null, frameColour, 0f, Vector2.Zero, npc.NPCHB().Scale, SpriteEffects.None, 0f);
                        }
                        if (i == (int)(barLength * 0.9F))
                        {
                            spriteBatch.Draw(NPCHealthBar.DD2E.Value, Midpo + new Vector2(0, 40), null, frameColour, 0f, Vector2.Zero, npc.NPCHB().Scale, SpriteEffects.FlipVertically, 0f);
                        }
                        Midpo.X += Mid.Width;
                    }
                }
            }
            //月球领主
            {

            }
        }
        private void drawHealthBarFrame(SpriteBatch spriteBatch, Color blackColour, Color frameColour, Texture2D Head, Texture2D Mid, Texture2D Tail, int barLength, int XLeft, int yTop, Vector2 FrameTopLeft, NPC npc, bool Death)
        {
            Color frameColour2 = frameColour * 0.5f;
            frameColour2.A = 0;
            NPCHealthBar gnpc = npc.NPCHB();
            //int XRight = XLeft + barLength;

            // loop draws from the right bar, to the left
            Vector2 Midpo = FrameTopLeft + new Vector2(Head.Width, 0);

            float F = (barLength + Mid.Width) / (Mid.Width * npc.NPCHB().Scale);
            for (float i = 1; i < F; i++)
            {
                spriteBatch.Draw(Mid, Midpo, new Rectangle?(new Rectangle(0, Mid.Height / npc.NPCHB().Midframe * gnpc.MidframeCurrent, Mid.Width, Mid.Height / npc.NPCHB().Midframe)), frameColour, 0f, Vector2.Zero, npc.NPCHB().Scale, SpriteEffects.None, 0f);
                Midpo.X += Mid.Width;
            }
            if (Death)
            {
                Midpo = FrameTopLeft + new Vector2(Head.Width, 0);
                for (float i = 1; i < F; i += (int)F / 40 + 1)
                {
                    UDust(Mid, Midpo, npc.NPCHB().Midframe, 80, Main.rand.NextFloat(3, 4));
                    UDust(NPCHealthBar.End[npc.type].Value, Midpo, 1, 80, Main.rand.NextFloat(3, 4));
                    Midpo.X += Mid.Width * ((int)F / 40 + 1);
                }
            }
            Extra(spriteBatch, frameColour,Head,Mid, F, FrameTopLeft,npc);

            Texture2D Damage = DDmodBossBar.Damage.Value;
            Texture2D Defense = DDmodBossBar.Defense.Value;
            float scale = npc.NPCHB().TextScale;
            //Draw side frames
            Vector2 Headpo = FrameTopLeft;
            spriteBatch.Draw(Head, Headpo, new Rectangle?(new Rectangle(0, Head.Height / npc.NPCHB().Headframe * gnpc.HeadframeCurrent, Head.Width, Head.Height / npc.NPCHB().Headframe)), frameColour, 0, Vector2.Zero, npc.NPCHB().Scale, 0, 0);
            if (Death)
                UDust(Head, Headpo, npc.NPCHB().Headframe, 80, Main.rand.NextFloat(3, 4));
            if (ModContent.GetInstance<DDHealthBar>().DamageandDefense)
            {
                spriteBatch.Draw(Damage, FrameTopLeft + new Vector2(Head.Width - 16, -16 * scale), null, frameColour, 0, Vector2.Zero, scale, 0, 0);

                Vector2 position = FrameTopLeft + new Vector2(Head.Width - 16, -16 * scale);
                DrawText(spriteBatch, "" + npc.damage, position + new Vector2(16 * scale, -4) + npc.NPCHB().TextPosition, blackColour, new Color(frameColour.R, 0, 0, frameColour.A), Vector2.Zero, scale);
            }
            Vector2 Tailpo = FrameTopLeft + new Vector2(Head.Width, 0) + new Vector2(Mid.Width * F - (Mid.Width * ((F % 1) == 0 ? 1 : (F % 1))), 0);
            spriteBatch.Draw(Tail, Tailpo, new Rectangle?(new Rectangle(0, Tail.Height / npc.NPCHB().Tailframe * gnpc.TailframeCurrent, Tail.Width, Tail.Height / npc.NPCHB().Tailframe)), frameColour, 0, Vector2.Zero, npc.NPCHB().Scale, 0, 0);
            if (Death)
                UDust(Tail, Tailpo, npc.NPCHB().Tailframe, 80, Main.rand.NextFloat(3, 4));
            if (ModContent.GetInstance<DDHealthBar>().DamageandDefense)
            {
                Vector2 position = Tailpo - new Vector2(16, 16 * scale);
                spriteBatch.Draw(Defense, position, null, frameColour, 0, Vector2.Zero, scale, 0, 0);

                int NPCDefense = npc.defense;
                if (npc.defense > 0)
                {
                    int Pen = npc.Dnpc().Penetrate;
                    if (npc.HasBuff(69))
                    {
                        Pen += 15;
                    }
                    if (npc.HasBuff(203))
                    {
                        Pen += 40;
                    }
                    NPCDefense -= Pen;
                    if (NPCDefense < 0)
                    {
                        NPCDefense = 0;
                    }
                }
                string T = "" + NPCDefense /*+ "(" + npc.defense + "-" + Pen + ")"*/;
                Vector2 origin = ChatManager.GetStringSize(FontAssets.MouseText.Value, T, Vector2.One, 0);
                origin.Y = 0;
                DrawText(spriteBatch, T, position - new Vector2(0 * scale, 4) + npc.NPCHB().TextPosition, blackColour, new Color(0, frameColour.R, frameColour.G, frameColour.A), origin, scale);
            }
        }
    }
}
