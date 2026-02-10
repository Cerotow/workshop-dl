using DDmod.NoContent.Config;
using System.Linq;
using DDmod.Content.NPCs.EliteMonster.四柱护卫;


namespace DDmod.BossHealthBar
{
    public static class NPCTracker
    {
        private const bool DEBUG_TRACKER = false;
        private static Dictionary<NPC, bool> deathNPCs;
        internal static Dictionary<NPC, bool> DeathNPCs
        {
            get
            {
                if (deathNPCs == null)
                {
                    deathNPCs = new Dictionary<NPC, bool>(255);
                }
                return deathNPCs;
            }
        }

        private static Dictionary<NPC, int> trackedNpcs;
        internal static Dictionary<NPC, int> TrackedNPCs
        {
            get
            {
                if (trackedNpcs == null)
                {
                    trackedNpcs = new Dictionary<NPC, int>(255);
                }
                return trackedNpcs;
            }
        }

        private static Dictionary<NPC, int> trackedNPCOldLife;
        internal static Dictionary<NPC, int> TrackedNPCOldLife
        {
            get
            {
                if (trackedNPCOldLife == null)
                {
                    trackedNPCOldLife = new Dictionary<NPC, int>(255);
                }
                return trackedNPCOldLife;
            }
        }
        private static Dictionary<NPC, float> trackedNPCChipLife;
        internal static Dictionary<NPC, float> TrackedNPCChipLife
        {
            get
            {
                if (trackedNPCChipLife == null)
                {
                    trackedNPCChipLife = new Dictionary<NPC, float>(255);
                }
                return trackedNPCChipLife;
            }
        }
        private static Dictionary<NPC, int> trackedNPCChipTime;
        internal static Dictionary<NPC, int> TrackedNPCChipTime
        {
            get
            {
                if (trackedNPCChipTime == null)
                {
                    trackedNPCChipTime = new Dictionary<NPC, int>(255);
                }
                return trackedNPCChipTime;
            }
        }

        public static void ResetTracker()
        {
            trackedNpcs = null;
        }
        public static bool CanTrackNPCHealth(NPC npc)
        {
            if (!npc.active) return false;

            if (npc.timeLeft <= 0) return false;

            if (npc.life <= 0) return false;

            if (npc.realLife >= 0 && npc.realLife != npc.whoAmI) return false;

            bool tooFar = (npc.position - Main.screenPosition).Length() > 5000;


            if (npc.immortal || npc.dontTakeDamageFromHostiles || tooFar)
            {
                return false;
            }
            /*if (npc.dontTakeDamage)
            {
                return false;
            }*/

            if (npc.ModNPC != null)
            {
                float scale = 1f;
                Vector2 position = new Vector2(npc.position.X + npc.width / 2, npc.position.Y + npc.gfxOffY);
                bool? result = npc.ModNPC.DrawHealthBar(Main.HealthBarDrawSettings, ref scale, ref position);
                if (result == false)
                {
                    return false;
                }
            }
            if (npc.NPCHB().NeedBlood)
            {
                return true;
            }

            return false;
        }

        public static void UpdateNPCTracker()
        {
            if (DEBUG_TRACKER && Main.time % 60 == 0)
            {
                string tracked = "list: ";
                foreach (NPC npc in TrackedNPCs.Keys)
                {
                    tracked += string.Concat("[", npc.whoAmI, "]");
                }
            }

            foreach (NPC npc in Main.npc)
            {
                if (CanTrackNPCHealth(npc))
                {
                    if (!TrackedNPCs.ContainsKey(npc))
                    {
                        TrackedNPCs.Add(npc, 0);
                    }
                }
                else
                {
                    foreach (NPC tracked in TrackedNPCs.Keys)
                    {
                        if (npc == tracked)
                        {
                            RemoveTrackedNPC(npc);
                            break;
                        }
                        else if (npc.whoAmI == tracked.whoAmI)
                        {
                            RemoveTrackedNPC(tracked);
                            break;
                        }
                    }
                }

                if (TrackedNPCs.ContainsKey(npc) && !TrackedNPCChipLife.ContainsKey(npc))
                {
                    TrackedNPCChipLife.Add(npc, 0);
                    TrackedNPCChipTime.Add(npc, 0);
                }
                else if (!TrackedNPCs.ContainsKey(npc) && TrackedNPCChipLife.ContainsKey(npc))
                {
                    TrackedNPCChipLife.Remove(npc);
                    TrackedNPCChipTime.Remove(npc);
                }
            }

            foreach (NPC tracked in TrackedNPCs.Keys.ToList())
            {
                bool disableFadeIn = false, disableFadeOut = false;
                HealthBar hb;
                if (BossDisplayInfo.NPCHealthBars.TryGetValue(tracked.type, out hb))
                {
                    disableFadeIn = hb.DisableFadeInFor(tracked.type);
                    disableFadeOut = hb.DisableFadeOutFor(tracked.type);
                }

                if (TrackedNPCs[tracked] > 0)
                {
                    TrackedNPCs[tracked]--;

                    if (disableFadeIn)
                    { TrackedNPCs[tracked] = 0; }
                }
                else if (TrackedNPCs[tracked] <= -1)
                {
                    if (TrackedNPCs[tracked] == -1 ||
                        disableFadeOut)
                    {
                        TrackedNPCs.Remove(tracked);
                    }
                    else
                    {
                        TrackedNPCs[tracked]++;
                    }
                }
            }
        }

        private static void RemoveTrackedNPC(NPC npc)
        {
            if (!DeathNPCs.ContainsKey(npc))
            {
                if (npc.life <= 0)
                {
                    DeathNPCs.Add(npc, true);
                }
                else
                {
                    DeathNPCs.Add(npc, false);
                }
                return;
            }
            HealthBar hb = BossDisplayInfo.GetHealthBarForNPCOrNull(npc.type);
            int fadeTime = 8 - TrackedNPCs[npc];
            if (hb != null)
            {
                if (hb.DisplayMode == HealthBar.DisplayType.Multiple && hb.multiShowCount > 1)
                {
                    TrackedNPCs.Remove(npc);
                    DeathNPCs.Remove(npc);
                    return;
                }
            }

            if (fadeTime > 1)
            {
                NPC temp = (NPC)npc.Clone();
                temp.whoAmI = -1 - npc.whoAmI;
                if (temp.life <= 0 || (temp.dontTakeDamage && npc.life == npc.lifeMax)) temp.life = 0;
                TrackedNPCs.Add(temp, -fadeTime - 1);
            }

            TrackedNPCs.Remove(npc);
            DeathNPCs.Remove(npc);
        }

        private static float GetAlpha(NPC npc)
        {
            float Alpha;
            try
            {
                float time = TrackedNPCs[npc];
                if (time < 0) time += 1;
                Alpha = 1f - (time / 5);
                if (HealthBar.MouseOver > 0 || Main.playerInventory || Main.InReforgeMenu || Main.InGuideCraftMenu)
                {
                    Alpha = MathHelper.Min(Alpha, 0.25F);
                }
                return Alpha;
            }
            catch
            {
                return 0f;
            }
        }

        //删除
        internal static double GetLifeFillNormal(NPC npc)
        {
            return 1d;
        }
        public static void DrawHealthBars(SpriteBatch spriteBatch)
        {
            HealthBar.ResetStaticVars();

            int maxYStack = (int)(Main.screenHeight * 0.9);
            int stackY = (int)(Main.screenHeight * 0.98);
            if (ModContent.GetInstance<DDHealthBar>().ShowTarget || ModContent.GetInstance<DDHealthBar>().DamageandDefense)
            {
                stackY += 16;
            }
            foreach (NPC npc in TrackedNPCs.Keys)
            {
                if (stackY > maxYStack * 0.9f || npc.NPCHB().Child)
                {
                    long L = 0;
                    if (npc.type >= 0 && npc.active)
                    {
                        for (int a = 0; a < npc.Dnpc().Lifes.Length; a++)
                        {
                            L += npc.Dnpc().Lifes[a];
                        }
                    }
                    long ML = 0;
                    if (npc.type >= 0 && npc.active)
                    {
                        for (int a = 0; a < npc.Dnpc().MaxLifes.Length; a++)
                        {
                            ML += npc.Dnpc().MaxLifes[a];
                        }
                    }
                    long lifes = npc.life + L;
                    long Maxlifes = npc.lifeMax + ML;

                    HealthBar hb = BossDisplayInfo.GetHealthBarForNPCOrNull(npc.type);
                    if (hb == null) hb = new HealthBar();

                    bool r = true;
                    for (int a = 0; a < 200; a++)
                    {
                        if (Main.npc[a].active)
                        {
                            if ((Main.npc[a].type == 13 || Main.npc[a].type == 14 || Main.npc[a].type == 15))
                            {
                                r = false;
                            }
                            if ((Main.npc[a].type == ModContent.NPCType<星尘护卫头>() || Main.npc[a].type == ModContent.NPCType<星尘护卫身>() || Main.npc[a].type == ModContent.NPCType<星尘护卫尾>()))
                            {
                                r = false;
                            }
                        }
                    }
                    if (DeathNPCs.ContainsKey(npc) && r && DeathNPCs[npc] && ModContent.GetInstance<DDHealthBar>().Kill)
                    {
                        if (npc.active && npc.type >= 0 && npc.NPCHB().Child)
                        {
                            hb.DrawHealthBarDefault2(spriteBatch, GetAlpha(npc), stackY, maxYStack, lifes, Maxlifes, npc, true);
                        }
                        else
                        {
                            stackY = hb.DrawHealthBarDefault(spriteBatch, GetAlpha(npc), stackY, maxYStack, lifes, Maxlifes, npc, true);
                        }
                    }
                    else
                    {
                        if (npc.active&&npc.type >= 0&& npc.NPCHB().Child)
                        {
                            hb.DrawHealthBarDefault2(spriteBatch, GetAlpha(npc), stackY, maxYStack, lifes, Maxlifes, npc, false);
                        }
                        else
                        {
                            stackY = hb.DrawHealthBarDefault(spriteBatch, GetAlpha(npc), stackY, maxYStack, lifes, Maxlifes, npc, false);
                        }
                    }
                }
            }
            DHealthBar.Moyan = 0;
            DHealthBar.Xing = 0;
        }
    }
}
