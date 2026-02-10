using DDmod.Worlds;
using Terraria.ID;
using static Terraria.ModLoader.NPCShop;

namespace DDmod.Helper
{
    public class Showonlydrops : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return false;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return null;
        }
    }
    public static class DNPC
    {
        public static int NewNPCs(IEntitySource spawnSource, Vector2 position, int Type, int Start, float ai0 = 0, float ai1 = 0, float ai2 = 0, float ai3 = 0, int Target = 255)
        {
            if (Main.netMode == 1)
            {
                return -1;
            }
            return NewNPCs(spawnSource, position.X, position.Y, Type, Start, ai0, ai1, ai2, ai3, Target);
        }
        public static int NewNPCs(IEntitySource spawnSource, float x, float y, int Type, int Start, float ai0 = 0, float ai1 = 0, float ai2 = 0, float ai3 = 0, int Target = 255)
        {
            if (Main.netMode == 1)
            {
                return 0;
            }
            return NewNPC(spawnSource, (int)x, (int)y, Type, Start, ai0, ai1, ai2, ai3, Target);
        }
        /// <summary> 引用DGlobalNPCnpc的变量 </summary>
        public static DGlobalNPC Dnpc(this NPC npc)
        {
            return npc.GetGlobalNPC<DGlobalNPC>();
        }
        public static DGlobalNPC2 Dnpc2(this NPC npc)
        {
            return npc.GetGlobalNPC<DGlobalNPC2>();
        }
        public static DGlobalNPCExp Exp(this NPC npc)
        {

            return npc.GetGlobalNPC<DGlobalNPCExp>();
        }
        public static DGlobalNPCBuff Bnpc(this NPC npc)
        {
            return npc.GetGlobalNPC<DGlobalNPCBuff>();
        }
        public static NPCHealthBar NPCHB(this NPC npc)
        {
            return npc.GetGlobalNPC<NPCHealthBar>();
        }
        /// <summary>
        /// 检查一定范围内有多少这种npc
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Location">位置</param>
        /// <param name="Range">范围</param>
        /// <returns></returns>
        public static int CountNPCS(int Type, Vector2 Location = default, int Range = 0)
        {
            int num = 0;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == Type)
                {
                    if (Location == default || (Location - Main.npc[i].Center).Length() < Range)
                        num++;
                }
            }

            return num;
        }
        /// <summary>
        /// 检查是哪个TE生成的npc
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Location">位置</param>
        /// <param name="Range">范围</param>
        /// <param name="Max">不看距离的最大数量</param>
        /// <returns></returns>
        public static int TileCountNPCS(int Type, Point16 point)
        {
            int num = 0;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].active && Main.npc[i].Dnpc().TETile==point)
                {
                    num++;
                }
            }
            return num;
        }
        /// <summary>
        /// 检查一定范围内有多少这种npc
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Location">位置</param>
        /// <param name="Range">范围</param>
        /// <param name="Max">不看距离的最大数量</param>
        /// <returns></returns>
        public static int CountNPCS(int Type, Vector2 Location, int Range, out int Max)
        {
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == Type)
                {
                    num2++;
                    if ((Location - Main.npc[i].Center).Length() < Range)
                        num++;
                }
            }
            Max = num2;
            return num;
        }
        public static bool IceCombustion(this NPC npc) => npc.Dnpc().Properties.Ice;
        public static bool NoIceCombustion(this NPC npc) => npc.Dnpc().Properties.Gel || npc.Dnpc().Properties.Grass || npc.HasBuff(137);
        public static bool Combustion(this NPC npc) => npc.Dnpc().Properties.Ice || npc.Dnpc().Properties.Gel || npc.Dnpc().Properties.Grass || npc.HasBuff(137);
        /// <summary>
        /// 商品
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="ItemType">物品</param>
        /// <param name="Price">价格</param>
        /// <param name="shopSpecialCurrency">货币类型</param>
        public static void Commodity(this NPCShop shop, int ItemType, int Price, int shopSpecialCurrency = -1)
        {
            Item item = new Item(ItemType);
            item.shopCustomPrice = new int?(Price);
            item.shopSpecialCurrency = shopSpecialCurrency;
            shop.Add(new Entry(item));
        }
        public static void Commodity(this NPCShop shop, int ItemType, int Price, int shopSpecialCurrency = -1, params Condition[] condition)
        {
            Item item = new Item(ItemType);
            item.shopCustomPrice = new int?(Price);
            item.shopSpecialCurrency = shopSpecialCurrency;
            shop.Add(new Entry(item, condition));
        }
        public static void NScale(this NPC npc,float Scale,bool Damage)
        {
            npc.position = npc.Center;
            npc.width = (int)(npc.width * Scale);
            npc.height = (int)(npc.height * Scale);
            npc.Center = npc.position;
            //DGlobalNPC.SendScale(npc);
            if (Damage)
            {
                npc.damage = (int)(npc.damage * Scale);
                npc.lifeMax = (int)(npc.lifeMax * Scale);
                npc.life = npc.lifeMax;
            }
        }
        public const byte 默认 = 0;
        public const byte 生命 = 1;
        public const byte 魔力 = 2;
        public static void NPCText(this NPC npc,string Text, byte texture = 0,Color BoxColor=default,Color TextColor=default)
        {
            if (Text == "") return;
            npc.Dnpc().Text = Text;
            npc.Dnpc().TextTime = 5;
            npc.Dnpc().texture = texture;
            npc.Dnpc().BoxColor = BoxColor;
            npc.Dnpc().TextColor = TextColor;
        }
        public static int NewNPCProj(this NPC npc, Vector2 position, Vector2 velocity, int Type, int Damage, float KnockBack, int Owner = -1, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f)
        {
            if (Main.netMode != 1)
                return NewProjectile(npc.GetSource_FromAI(), position, velocity, Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
            return -1;
        }
        public static void SmoothVelocity(this NPC npc, Vector2 Speed, float Smooth = 20) => npc.velocity = (npc.velocity * Smooth + Speed) / (Smooth+1);
        public static void Kill(this NPC npc,bool Loot = true)
        {
            npc.Dnpc().Deathrattle = false;
            npc.life = -1;
            npc.HitEffect(0, 10000);
            npc.life = -1;
            if (npc.active)
            {
                if (Loot)
                {
                    if (NPCLoader.CheckDead(npc))
                    {
                        npc.checkDead();
                    }
                    else
                    {
                        npc.NPCLoot();
                    }
                }
                npc.active = false;
            }
        }
        /// <summary> 缓慢看向某个地方,Rotation要达到的方向,Speed转向速度 </summary>
        public static void RotationSpeed(this NPC npc, float Rotation, float Speed)
        {
            if (Rotation < 0f)
            {
                Rotation += MathHelper.TwoPi;
            }
            else if (Rotation > 6.283)
            {
                Rotation -= MathHelper.TwoPi;
            }
            if (npc.rotation < Rotation)
            {
                if ((Rotation - npc.rotation) > MathHelper.Pi)
                {
                    npc.rotation -= Speed;
                }
                else
                {
                    npc.rotation += Speed;
                }
            }
            else if (npc.rotation > Rotation)
            {
                if ((npc.rotation - Rotation) > MathHelper.Pi)
                {
                    npc.rotation += Speed;
                }
                else
                {
                    npc.rotation -= Speed;
                }
            }
            if (npc.rotation < 0f)
            {
                npc.rotation += MathHelper.TwoPi;
            }
            else if (npc.rotation > MathHelper.TwoPi)
            {
                npc.rotation -= MathHelper.TwoPi;
            }
            if (npc.rotation > Rotation - Speed && npc.rotation < Rotation + Speed)
            {
                npc.rotation = Rotation;
                return;
            }
            if (npc.rotation > Rotation + MathHelper.TwoPi - Speed && npc.rotation < Rotation + MathHelper.TwoPi + Speed)
            {
                npc.rotation = Rotation;
                return;
            }
            if (npc.rotation + MathHelper.TwoPi > Rotation - Speed && npc.rotation + MathHelper.TwoPi < Rotation + Speed)
            {
                npc.rotation = Rotation;
                return;
            }
        }
    }
}