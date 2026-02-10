using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.天地守卫;
using DDmod.Content.NPCs.Boss.流星破坏者;
using DDmod.Content.NPCs.Boss.海幽浮王;
using DDmod.Content.NPCs.Boss.绿岩之视;
using DDmod.NoContent.Config;
using Terraria.ID;

namespace DDmod.Helper
{
    public static class DHealthBar
    {
        //双子魔眼血条位置
        public static int Moyan;
        //星心守卫血条位置
        public static int Xing;
        //调整血条位置
        public static void HealthPosition(NPC npc, ref int x, ref int y, ref int width)
        {
            Texture2D barM = NPCHealthBar.Mid[npc.type].Value;
            int Height = 10;
            if (ModContent.GetInstance<DDHealthBar>().ShowTarget || ModContent.GetInstance<DDHealthBar>().DamageandDefense)
            {
                Height = 42;
            }
            //双子魔眼
            if (npc.type == 125)
            {
                width = (int)(Main.screenWidth * 0.2f);
                x += (int)(width * 0.99f);
                if (Moyan!=0)
                {
                    y = Moyan;
                }
                else
                Moyan = y;
            }
            if (npc.type == 126)
            {
                width = (int)(Main.screenWidth * 0.2f);
                x -= (int)(width * 0.33f);
                if (Moyan != 0)
                {
                    y = Moyan;
                }
                else
                Moyan = y;
            }
            if (npc.type == ModContent.NPCType<大地守卫>())
            {
                width = (int)(Main.screenWidth * 0.2f);
                x += (int)(width * 0.99f);
                if (Xing != 0)
                {
                    y = Xing;
                }
                else
                    Xing = y;
            }
            if (npc.type == ModContent.NPCType<苍穹守卫>())
            {
                width = (int)(Main.screenWidth * 0.2f);
                x -= (int)(width * 0.33f);
                if (Xing != 0)
                {
                    y = Xing;
                }
                else
                    Xing = y;
            }
            //石巨人
            if (npc.type == NPCID.Golem)
            {
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && (Main.npc[A].type == 247 || Main.npc[A].type == 248))
                    {
                        y -= barM.Height / npc.NPCHB().Midframe;
                        break;
                    }
                }
                npc.NPCHB().position = y;
            }
            if (npc.type == NPCID.GolemFistLeft)
            {
                width = (int)(Main.screenWidth * 0.2f);
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && Main.npc[A].type == 245)
                    {
                        y = (int)Main.npc[A].NPCHB().position - Height;
                        break;
                    }
                }
            }
            if (npc.type == NPCID.GolemFistRight)
            {
                width = (int)(Main.screenWidth * 0.2f);
                x += (int)(width * 1.5f);

                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && Main.npc[A].type == 245)
                    {
                        y = (int)Main.npc[A].NPCHB().position - Height;
                        break;
                    }
                }
            }
            //骷髅王
            if (npc.type == NPCID.SkeletronHead)
            {
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && (Main.npc[A].type == 36))
                    {
                        y -= barM.Height / npc.NPCHB().Midframe;
                        break;
                    }
                }
                npc.NPCHB().position = y;
            }
            if (npc.type == 36)
            {
                width = (int)(Main.screenWidth * 0.2f);
                if (npc.ai[0] == -1)
                {
                    x += (int)(width * 1.5f);
                }
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && Main.npc[A].type == 35)
                    {
                        y = (int)Main.npc[A].NPCHB().position - Height;
                        break;
                    }
                }
            }
            //流星破坏者
            if (npc.type == ModContent.NPCType<流星破坏者>())
            {
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && (Main.npc[A].type == ModContent.NPCType<流星大炮>()|| Main.npc[A].type == ModContent.NPCType<流星激光枪>()))
                    {
                        y -= barM.Height / npc.NPCHB().Midframe;
                        break;
                    }
                }
                npc.NPCHB().position = y;
            }
            if (npc.type == ModContent.NPCType<流星大炮>())
            {
                width = (int)(Main.screenWidth * 0.2f);
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && Main.npc[A].type == ModContent.NPCType<流星破坏者>())
                    {
                        y = (int)Main.npc[A].NPCHB().position - Height;
                        break;
                    }
                }
            }
            if (npc.type == ModContent.NPCType<流星激光枪>())
            {
                width = (int)(Main.screenWidth * 0.2f);
                x += (int)(width * 1.5f);
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && Main.npc[A].type == ModContent.NPCType<流星破坏者>())
                    {
                        y = (int)Main.npc[A].NPCHB().position - Height;
                        break;
                    }
                }
            }
            //绿岩之视
            if (npc.type == ModContent.NPCType<绿岩之视>())
            {
                width = (int)(Main.screenWidth * 0.35f);
                x = Main.screenWidth / 2 - (width / 2);
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && (Main.npc[A].type == ModContent.NPCType<绿岩炮>()))
                    {
                        y -= barM.Height / npc.NPCHB().Midframe;
                        break;
                    }
                }
                npc.NPCHB().position = y;
            }
            if (npc.type == ModContent.NPCType<绿岩炮>())
            {
                width = (int)(Main.screenWidth * 0.2f);
                x = Main.screenWidth / 2 - (width / 2);
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && Main.npc[A].type == ModContent.NPCType<绿岩之视>())
                    {
                        y = (int)Main.npc[A].NPCHB().position - Height;
                        break;
                    }
                }
            }
            //海幽浮王
            if (npc.type == ModContent.NPCType<海幽浮王>())
            {
                width = (int)(Main.screenWidth * 0.35f);
                x = Main.screenWidth / 2 - (width / 2);
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && (Main.npc[A].type == ModContent.NPCType<海幽浮>()))
                    {
                        y -= barM.Height / npc.NPCHB().Midframe;
                        break;
                    }
                }
                npc.NPCHB().position = y-10;
            }
            if (npc.type == ModContent.NPCType<海幽浮>())
            {
                width = (int)(Main.screenWidth * 0.2f);
                x = Main.screenWidth / 2 - (width / 2);
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && Main.npc[A].type == ModContent.NPCType<海幽浮王>())
                    {
                        y = (int)Main.npc[A].NPCHB().position - Height;
                        break;
                    }
                }
            }
            //机械骷髅王
            if (npc.type == 127)
            {
                width = (int)(Main.screenWidth * 0.3f);
                x = Main.screenWidth / 2 - (width / 2);
                npc.NPCHB().position = y;
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && (Main.npc[A].type == 128 || Main.npc[A].type == 129 || Main.npc[A].type == 130 || Main.npc[A].type == 131))
                    {
                        y -= barM.Height / npc.NPCHB().Midframe;
                        break;
                    }
                }
            }
            if (npc.type == 128 || npc.type == 129 || npc.type == 130 || npc.type == 131)
            {
                width = (int)(Main.screenWidth * 0.15f);
                x = Main.screenWidth / 2 - (int)(width / 1.9f);
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && Main.npc[A].type == 127)
                    {
                        y = (int)Main.npc[A].NPCHB().position-10;
                        break;
                    }
                }
                if (npc.type == 131)
                {
                    x -= (int)(width / 1.3f);
                    y -= barM.Height / npc.NPCHB().Midframe + Height;
                }
                if (npc.type == 128)
                {
                    x += (int)(width / 1.35f);
                    y -= barM.Height / npc.NPCHB().Midframe + Height;
                }
                if (npc.type == 129)
                {
                    x -= (int)(width * 2f);
                }
                if (npc.type == 130)
                {
                    x += (int)(width * 1.95f);
                }
            }
            //月球领主
            if (npc.type == NPCID.MoonLordHead)
            {
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && (Main.npc[A].type == 397))
                    {
                        y -= barM.Height / npc.NPCHB().Midframe;
                        break;
                    }
                }
                npc.NPCHB().position = y;
            }
            if (npc.type == 397)
            {
                width = (int)(Main.screenWidth * 0.2f);
                if (npc.ai[2] == 1)
                {
                    x += (int)(width * 1.5f);
                }
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].active && Main.npc[A].type == 396)
                    {
                        y = (int)Main.npc[A].NPCHB().position - Height;
                        break;
                    }
                }
            }
        }
        //增加血条的高度
        public static void DrawHealthBarY(NPC npc, Texture2D Mid, ref int yTop)
        {
            /*
            if (npc.type == 125 || npc.type == 126)
            {
                if (npc.NPCHB().position == 0)
                {
                    yTop += Mid.Height / npc.NPCHB().Midframe;
                }
            }*/
            for (int He = 0; He < 200; He++)
            {
                if (Main.npc[He].active)
                {
                    if (npc.NPCHB().Multiple)
                    {
                        //石巨人
                        if (npc.type == NPCID.Golem)
                        {
                            if (Main.npc[He].type == NPCID.GolemFistLeft || Main.npc[He].type == NPCID.GolemFistRight)
                            {
                                yTop += Mid.Height / npc.NPCHB().Midframe;
                                break;
                            }
                        }
                        //骷髅王
                        if (npc.type == NPCID.SkeletronHead)
                        {
                            if (Main.npc[He].type == 36)
                            {
                                yTop += Mid.Height / npc.NPCHB().Midframe;
                                break;
                            }
                        }
                        //流星破坏者
                        if (npc.type == ModContent.NPCType<流星破坏者>())
                        {
                            if (Main.npc[He].type == ModContent.NPCType<流星大炮>() || Main.npc[He].type == ModContent.NPCType<流星激光枪>())
                            {
                                yTop += Mid.Height / npc.NPCHB().Midframe;
                                break;
                            }
                        }
                        //绿岩之视
                        if (npc.type == ModContent.NPCType<绿岩之视>())
                        {
                            if (Main.npc[He].type == ModContent.NPCType<绿岩炮>())
                            {
                                yTop += Mid.Height / npc.NPCHB().Midframe;
                                break;
                            }
                        }
                        //海幽浮王
                        if (npc.type == ModContent.NPCType<海幽浮王>())
                        {
                            if (Main.npc[He].type == ModContent.NPCType<海幽浮>())
                            {
                                yTop += Mid.Height / npc.NPCHB().Midframe;
                                break;
                            }
                        }
                        //机械骷髅王
                        if (npc.type == 127)
                        {
                            if (Main.npc[He].type == 128 || Main.npc[He].type == 129 || Main.npc[He].type == 130 || Main.npc[He].type == 131)
                            {
                                yTop += Mid.Height / npc.NPCHB().Midframe;
                                break;
                            }
                        }
                        //月总
                        if (npc.type == NPCID.MoonLordHead)
                        {
                            if (Main.npc[He].type == 397)
                            {
                                yTop += Mid.Height / npc.NPCHB().Midframe;
                                break;
                            }
                        }
                    }
                }
            }
        }
        public static void DrawHealthBarYActive(NPC npc, Texture2D Mid, ref int yTop)
        {
            /*
            if (npc.type == 125 || npc.type == 126)
            {
                if (npc.NPCHB().position == 0)
                {
                    yTop -= Mid.Height / npc.NPCHB().Midframe;
                }
            }*/
            for (int He = 0; He < 200; He++)
            {
                if (Main.npc[He].active)
                {
                    if (npc.NPCHB().Multiple)
                    {
                        //石巨人
                        if (npc.type == NPCID.Golem)
                        {
                            if (Main.npc[He].type == NPCID.GolemFistLeft || Main.npc[He].type == NPCID.GolemFistRight)
                            {
                                yTop -= Mid.Height / npc.NPCHB().Midframe;
                                break;
                            }
                        }
                        //骷髅王
                        if (npc.type == NPCID.SkeletronHead)
                        {
                            if (Main.npc[He].type == 36)
                            {
                                yTop -= Mid.Height / npc.NPCHB().Midframe;
                                break;
                            }
                        }
                        //绿岩之视
                        if (npc.type == ModContent.NPCType<绿岩之视>())
                        {
                            if (Main.npc[He].type == ModContent.NPCType<绿岩炮>())
                            {
                                yTop -= Mid.Height / npc.NPCHB().Midframe;
                                break;
                            }
                        }
                        //流星破坏者
                        if (npc.type == ModContent.NPCType<流星破坏者>())
                        {
                            if (Main.npc[He].type == ModContent.NPCType<流星大炮>() || Main.npc[He].type == ModContent.NPCType<流星激光枪>())
                            {
                                yTop -= Mid.Height / npc.NPCHB().Midframe;
                                break;
                            }
                        }
                        //海幽浮王
                        if (npc.type == ModContent.NPCType<海幽浮王>())
                        {
                            if (Main.npc[He].type == ModContent.NPCType<海幽浮>())
                            {
                                yTop -= Mid.Height / npc.NPCHB().Midframe;
                                break;
                            }
                        }
                        //机械骷髅王
                        if (npc.type == 127)
                        {
                            if (Main.npc[He].type == 128 || Main.npc[He].type == 129 || Main.npc[He].type == 130 || Main.npc[He].type == 131)
                            {
                                yTop -= Mid.Height / npc.NPCHB().Midframe;
                                break;
                            }
                        }
                        //月总
                        if (npc.type == NPCID.MoonLordHead)
                        {
                            if (Main.npc[He].type == 397)
                            {
                                yTop -= Mid.Height / npc.NPCHB().Midframe;
                                break;
                            }
                        }
                    }
                }
            }
        }
        public static void ModifyName(NPC npc, ref string Name)
        {
            if (npc.type == 36)
            {
                if (npc.ai[0] == -1)
                {
                    Name += Language.GetTextValue("Mods.DDmod.HealthBarText.右手"); 
                }
                else
                {
                    Name += Language.GetTextValue("Mods.DDmod.HealthBarText.左手");
                }
            }
            if (npc.type == 397)
            {
                if (npc.ai[2] == 1)
                {
                    Name += Language.GetTextValue("Mods.DDmod.HealthBarText.右");
                }
                else
                {
                    Name += Language.GetTextValue("Mods.DDmod.HealthBarText.左");
                }
            }
        }
        public static bool Master(NPC Boss,NPC npc)
        {
            //飞碟
            if (Boss.type == NPCID.MartianSaucerCore)
            {
                return npc.Dnpc().Master == Boss.whoAmI;
            }
            return true;
        }
    }
}