using DDmod.Content.Projectiles;

namespace DDmod.Content.NPCs
{
    internal static class NPCdirection
    {

        public static bool Incident(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Invasion || Main.eclipse || Main.pumpkinMoon || Main.snowMoon|| Main.invasionType != 0
                || NPC.AnyNPCs(NPCID.LunarTowerSolar)
                || NPC.AnyNPCs(NPCID.LunarTowerVortex)
                || NPC.AnyNPCs(NPCID.LunarTowerStardust)
               || NPC.AnyNPCs(NPCID.LunarTowerNebula))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary> 检测最近的NPC,Position位置,maxRange最大距离,checkCanHit穿墙,npc不追踪的npc </summary>
        public static NPC FindClosest2(Vector2 Position, float maxRange, bool checkCanHit = true, List<NPC> NONPC = null, int NPCMaster = -1)
        {
            NPC result = null;
            float num = maxRange;
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (NONPC != null)
                {
                    bool r = false;
                    for (int a = 0; a < NONPC.Count; a++)
                    {
                        if (npc.whoAmI == NONPC[a].whoAmI)
                        {
                            r = true;
                            break;
                        }
                    }
                    if (r)
                    {
                        continue;
                    }
                }
                if (npc.CanBeChasedBy())
                {
                    if (NPCMaster < 0)
                    {
                        float num2 = (Position - npc.Center).Length();
                        if (!(num <= num2))
                        {
                            if (checkCanHit || Collision.CanHitLine(Position, 1, 1, npc.position, npc.width, npc.height))
                            {
                                num = num2;
                                result = npc;
                            }
                        }
                    }
                    else
                    {
                        if (i == NPCMaster || npc.realLife == Main.npc[NPCMaster].realLife || npc.realLife == Main.npc[NPCMaster].type)
                        {
                            float num2 = (Position - npc.Center).Length();
                            if (!(num <= num2))
                            {
                                if (checkCanHit || Collision.CanHitLine(Position, 1, 1, npc.position, npc.width, npc.height))
                                {
                                    num = num2;
                                    result = npc;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        /// <summary> 检测最近的NPC,Position位置,maxRange最大距离,checkCanHit穿墙,npc不追踪的npc </summary>
        public static NPC FindClosest2(Vector2 Position, float maxRange, bool checkCanHit = true, NPC[] NONPC = null, int NPCMaster = -1)
        {
            NPC result = null;
            float num = maxRange;
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.CanBeChasedBy())
                {
                    continue;
                }
                if (NONPC != null)
                {
                    bool r = false;
                    for (int a = 0; a < NONPC.Length; a++)
                    {
                        if (npc == NONPC[a])
                        {
                            r = true;
                            break;
                        }
                    }
                    if (r)
                    {
                        continue;
                    }
                }
                if (NPCMaster < 0)
                {
                    float num2 = (Position - npc.Center).Length();
                    if (!(num <= num2))
                    {
                        if (checkCanHit || Collision.CanHitLine(Position, 1, 1, npc.position, npc.width, npc.height))
                        {
                            num = num2;
                            result = npc;
                        }
                    }
                }
                else
                {
                    if (i == NPCMaster || npc.realLife == Main.npc[NPCMaster].realLife || npc.realLife == Main.npc[NPCMaster].type)
                    {
                        float num2 = (Position - npc.Center).Length();
                        if (!(num <= num2))
                        {
                            if (checkCanHit || Collision.CanHitLine(Position, 1, 1, npc.position, npc.width, npc.height))
                            {
                                num = num2;
                                result = npc;
                            }
                        }
                    }
                }
            }
            return result;
        }
        /// <summary> 检测最近的NPC,Position位置,maxRange最大距离,checkCanHit穿墙,npc不追踪的npc </summary>
        public static NPC FindClosest(Vector2 Position, float maxRange, bool checkCanHit = true, NPC NONPC = null, int NPCMaster = -1)
        {
            NPC result = null;
            float num = maxRange;
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.CanBeChasedBy())
                {
                    continue;
                }
                if (NONPC == null || NONPC != npc)
                {
                    if (NPCMaster < 0)
                    {
                        float num2 = (Position - npc.Center).Length();
                        if (maxRange >=num2&& num >= num2)
                        {
                            if (checkCanHit || Collision.CanHitLine(Position, 1, 1, npc.position, npc.width, npc.height))
                            {
                                num = num2;
                                result = npc;
                            }
                        }
                    }
                    else
                    {
                        if (i == NPCMaster || npc.realLife == Main.npc[NPCMaster].realLife || npc.realLife == Main.npc[NPCMaster].type)
                        {
                            float num2 = (Position - npc.Center).Length();
                            if (maxRange >= num2 && num >= num2)
                            {
                                if (checkCanHit || Collision.CanHitLine(Position, 1, 1, npc.position, npc.width, npc.height))
                                {
                                    num = num2;
                                    result = npc;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        /// <summary> 有追踪目标 </summary>
        public static bool HaveGoal(this Projectile projectile, int Distance, int Time = 30, bool IgnoreTile = false, int NPCID = -1)
        {
            if (NPCID == -2)
            {
                return false;
            }
            NPC npc = projectile.FindTargetWithinRange(Distance, !IgnoreTile);
            if (NPCID >= 0)
            {
                npc = Main.npc[NPCID];
                if (npc == null)
                {
                    npc = FindClosest(projectile.Center, Distance, IgnoreTile, null, NPCID);
                }
                if ((npc.Center - projectile.Center).Length() > Distance)
                {
                    npc = null;
                }
            }
            if (npc != null && npc.active && (Time>=0 ?projectile.GetGlobalProjectile<DDGlobalProjectile>().track > Time:true))
            {
                return true;
            }
            return false;
        }
        /// <summary> 有追踪目标 </summary>
        public static bool CustomHaveGoal(Vector2 vector, int Distance, int Time = 30, bool IgnoreTile = false, int NPCID = -1)
        {
            if (NPCID == -2)
            {
                return false;
            }
            NPC npc = FindClosest(vector, Distance, IgnoreTile);
            if (NPCID >= 0)
            {
                npc = Main.npc[NPCID];
                if (npc == null)
                {
                    npc = FindClosest(vector, Distance, IgnoreTile, null, NPCID);
                }
                if ((npc.Center - vector).Length() > Distance)
                {
                    npc = null;
                }
            }
            if (npc != null && npc.active)
            {
                return true;
            }
            return false;
        }
        /// <summary> 追踪 </summary>
        public static void Track(this Projectile projectile, int Distance, float Inertia = 10, float Speed = 8, int Time = 30, bool IgnoreTile = false, int NPCID = -1)
        {
            if(NPCID==-2)
            {
                return;
            }
            NPC npc;
            if (NPCID >= 0)
            {
                npc = Main.npc[NPCID];
                if (npc == null)
                {
                    npc = FindClosest(projectile.Center, Distance, IgnoreTile, null, NPCID);
                }
                if ((npc.Center - projectile.Center).Length() > Distance)
                {
                    npc = null;
                }
            }
            else
            {
                npc = FindClosest(projectile.Center, Distance, IgnoreTile, null, NPCID);
            }
            if (npc != null && npc.active && projectile.GetGlobalProjectile<DDGlobalProjectile>().track > Time)
            {
                projectile.Chase(npc, Speed, Inertia);
            }
        }
        /// <summary> 不建议使用,追踪目标 </summary>
        public static void Chase(this Projectile projectile, NPC npc, float Speed, float Inertia)
        {
            if (!projectile.hostile && projectile.friendly)
            {
                Vector2 vector = (npc.Center - projectile.Center).PerfectNormalize() * Speed;
                projectile.velocity = (projectile.velocity * Inertia + vector) / (Inertia + 1);
            }
        }
    }
}