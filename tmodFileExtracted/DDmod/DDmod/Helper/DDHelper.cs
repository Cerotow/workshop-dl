using DDmod.AccessorySlot;
using DDmod.Content.Items;
using DDmod.Content.Items.Melee.SwordShield;
using DDmod.Content.Projectiles;
using DDmod.Content.Projectiles.Melee.SwordShield;
using DDmod.Content.Projectiles.Melee.TwinSwords;
using DDmod.Players;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria.Chat;
using Terraria.Graphics.Shaders;

namespace DDmod.Helper
{
    public static class DDHelper
    {
        public static void newText(string Text,Color color)
        {
            if (Main.netMode == 0)
                Main.NewText(Text, color);
            else if (Main.netMode == 2 && Text != "")
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Text), color);
        }
        public static string Deep(float height)
        {
            int num22 = (int)((double)((height) * 2f) - Main.worldSurface * 2.0);
            int num24 = 1200;
            float num23 = (float)Main.maxTilesX / 4200f;
            float num25 = (float)((double)(height - (65f + 10f * num23)) / (Main.worldSurface / 5.0));
            string text6 = ((height * 16 > (float)((Main.maxTilesY - 204) * 16)) ? Language.GetTextValue("GameUI.LayerUnderworld") : (((double)height * 16 > Main.rockLayer * 16.0 + (double)(num24 / 2) + 16.0) ? Language.GetTextValue("GameUI.LayerCaverns") : ((num22 > 0) ? Language.GetTextValue("GameUI.LayerUnderground") : ((!(num25 >= 0f)) ? Language.GetTextValue("GameUI.LayerSpace") : Language.GetTextValue("GameUI.LayerSurface")))));
            return text6 + Math.Abs(num22);
        }
        public static bool SolidTile(float i, float j, bool Platforms = true, bool tileSolidTop = true)
        {
            Tile tile = Main.tile[(int)i, (int)j];
            return SolidTile(tile,Platforms,tileSolidTop);
        }
        public static bool SolidTile(Tile tile, bool Platforms = true, bool tileSolidTop = true)
        {
            return WorldGen.SolidTile(tile) || (Platforms && TileID.Sets.Platforms[tile.TileType]) || (tileSolidTop && Main.tileSolidTop[tile.TileType]);
        }
        public static float AngleDifference(float a, float b)
        {
            float diff = (a - b + (float)Math.PI) % (float)(2 * Math.PI) - (float)Math.PI;
            return diff < -(float)Math.PI ? diff + (float)(2 * Math.PI) : diff;
        }
        /// <summary>
        /// 0NPC,1Item
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SoundStyle SoundStyle(int type,string Text = "")
        {
            return new SoundStyle(Sound(type,Text));
        }
        public static string Sound(int type,string Text = "")
        {
            string A = "DDmod/NoContent/Sounds/";
            if(type ==0)
            {
                A += "NPC/";
            }
            if(type ==1)
            {
                A += "Items/";
            }
            return A+ Text;
        }
        public static Color[] GetColors(this Texture2D texture)
        {
            if (Main.netMode == 2 || Main.gameMenu)
            {
                return [];
            }
            int x = texture.Width;
            int y = texture.Height;
            Color[] xy = new Color[x * y];
            texture.GetData(xy);
            return xy;
        }
        public static byte[] Getbytes(this Texture2D texture)
        {
            if(Main.netMode==2|| Main.gameMenu)
            {
                return [];
            }
            Color[] xy = GetColors(texture);
            byte[] b = new byte[xy.Length];
            for(int a= 0;a < xy.Length; a++)
            {
                if (xy[a] != Color.Transparent)
                {
                    b[a] = 1;
                }
            }
            return b;
        }
        public static Color Opposite(this Color color)
        {
            color.R = (byte)(255 - color.R);
            color.G = (byte)(255 - color.G);
            color.B = (byte)(255 - color.B);
            return color;
        }
        public static Rectangle GetClippingRectangle(this SpriteBatch spriteBatch, Rectangle rect)
        {
            Vector2 vector = new Vector2(rect.X, rect.Y);
            Vector2 position = new Vector2(rect.Width, rect.Height) + vector;
            vector = Vector2.Transform(vector, Main.UIScaleMatrix);
            position = Vector2.Transform(position, Main.UIScaleMatrix);
            Rectangle rectangle = new Rectangle((int)vector.X, (int)vector.Y, (int)(position.X - vector.X), (int)(position.Y - vector.Y));
            int num = (int)((float)Main.screenWidth * Main.UIScale);
            int num2 = (int)((float)Main.screenHeight * Main.UIScale);
            rectangle.X = Utils.Clamp(rectangle.X, 0, num);
            rectangle.Y = Utils.Clamp(rectangle.Y, 0, num2);
            rectangle.Width = Utils.Clamp(rectangle.Width, 0, num - rectangle.X);
            rectangle.Height = Utils.Clamp(rectangle.Height, 0, num2 - rectangle.Y);
            Rectangle scissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
            int num3 = Utils.Clamp(rectangle.Left, scissorRectangle.Left, scissorRectangle.Right);
            int num4 = Utils.Clamp(rectangle.Top, scissorRectangle.Top, scissorRectangle.Bottom);
            int num5 = Utils.Clamp(rectangle.Right, scissorRectangle.Left, scissorRectangle.Right);
            int num6 = Utils.Clamp(rectangle.Bottom, scissorRectangle.Top, scissorRectangle.Bottom);
            return new Rectangle(num3, num4, num5 - num3, num6 - num4);
        }
        public static void DrawrectBegin(this SpriteBatch sb,Rectangle rect, BlendState blendState, Matrix matrix, out SamplerState anisotropicClamp, out RasterizerState rasterizerState, out Rectangle scissorRectangle)
        {
            anisotropicClamp = SamplerState.AnisotropicClamp;
            //anisotropicClamp = Main.DefaultSamplerState;
            rasterizerState = sb.GraphicsDevice.RasterizerState;
            scissorRectangle = sb.GraphicsDevice.ScissorRectangle;

            RasterizerState rasterizer = new RasterizerState
            {
                CullMode = CullMode.None,
                ScissorTestEnable = true
            };
            sb.End();
            Rectangle rectangle1 = Rectangle.Intersect(DDHelper.GetClippingRectangle(sb, rect), sb.GraphicsDevice.ScissorRectangle);
            sb.GraphicsDevice.ScissorRectangle = rectangle1;
            sb.GraphicsDevice.RasterizerState = rasterizer;
            sb.Begin(SpriteSortMode.Deferred, blendState, anisotropicClamp, DepthStencilState.None, rasterizer, null, matrix);
        }
        public static void DrawrectEnd(this SpriteBatch sb, BlendState blendState, Matrix matrix, SamplerState anisotropicClamp, RasterizerState rasterizerState, Rectangle scissorRectangle)
        {
            sb.End();
            sb.GraphicsDevice.ScissorRectangle = scissorRectangle;
            sb.GraphicsDevice.RasterizerState = rasterizerState;
            sb.Begin(SpriteSortMode.Deferred, blendState, anisotropicClamp, DepthStencilState.None, rasterizerState, null, matrix);
        }
        //反射
        public static FieldInfo FieldReflection(Type T, string name, BindingFlags binding)
        {
            FieldInfo field = T.GetField(name, binding);
            return field;
        }
        public static MethodInfo MethodReflection(Type T, string name, BindingFlags binding)
        {
            MethodInfo field = T.GetMethod(name, binding);
            return field;
        }
        public static Vector2 OffsetCenter(this Texture2D texture,int W,int H)
        {
            Vector2 vector;
            if (W == 0)
            {
                vector.X = 0;
            }
            else
            {
                vector.X = texture.Width / W;
            }
            if (W == 0)
            {
                vector.Y = 0;
            }
            else
            {
                vector.Y = texture.Height / H;
            }
            return vector/2;
        }
        /// <summary> 冒险装备 </summary>
        public static AdventureGearGlobalItem AGItem(this Item item)
        {
            return item.GetGlobalItem<AdventureGearGlobalItem>();
        }
        /// <summary> 全局饰品 </summary>
        public static AccessoryGlobalItem AccItem(this Item item)
        {
            return item.GetGlobalItem<AccessoryGlobalItem>();
        }
        /// <summary> 饰品栏玩家 </summary>
        public static AccessoryPlayer AccPlayer(this Player player)
        {
            return player.GetModPlayer<AccessoryPlayer>();
        }
        /// <summary> 全局饰品 </summary>
        public static AccessoryGlobalItem AccessoryItem(this Item item)
        {
            return item.GetGlobalItem<AccessoryGlobalItem>();
        }
        public static DGlobalItem DItem(this Item item)
        {
            return item.GetGlobalItem<DGlobalItem>();
        }
        public static DDPlayer Dplayer(this Player player)
        {
            return player.GetModPlayer<DDPlayer>();
        }
        public static AttributesPlayer Aplayer(this Player player)
        {
            return player.GetModPlayer<AttributesPlayer>();
        }
        public static EntrustPlayer Eplayer(this Player player)
        {
            return player.GetModPlayer<EntrustPlayer>();
        }
        public static MagicGlobalItem MagicItem(this Item item)
        {
            return item.GetGlobalItem<MagicGlobalItem>();
        }
        public static bool 活着(this Player player)
        {
            return player.active && !player.dead;
        }
        /// <summary>
        /// 圆形碰撞箱
        /// </summary>
        /// <param name="Rectangle">方形碰撞箱</param>
        /// <param name="CircleCenter">圆碰撞中心</param>
        /// <param name="CircleRange">圆碰撞大小</param>
        /// <returns></returns>
        public static bool CircleInsertRectangle(Rectangle rectangle, Vector2 circleCenter, float circleRadius)
        {
            // 找到矩形上距离圆心最近的点
            float closestX = MathHelper.Clamp(circleCenter.X, rectangle.Left, rectangle.Right);
            float closestY = MathHelper.Clamp(circleCenter.Y, rectangle.Top, rectangle.Bottom);

            // 计算圆心到最近点的距离
            float distanceX = circleCenter.X - closestX;
            float distanceY = circleCenter.Y - closestY;
            float distanceSquared = distanceX * distanceX + distanceY * distanceY;

            // 判断距离是否小于等于半径
            return distanceSquared <= circleRadius * circleRadius;
        }
        public static void FixedDamage(this Player player,int damage,PlayerDeathReason playerDeathReason)
        {
            player.statLife -= damage;
            if (player.statLife <= 0)
                player.KillMe(playerDeathReason, (int)damage, 0);
            CombatText.NewText(player.getRect(), new Color(255, 100, 100), damage);
        }
        public static void 绘制偏移头部(Texture2D texture,Projectile projectile, Color color, float Rotation2 = MathHelper.Pi, Texture2D Glow =null,int GlowOr = 20,float GlowScale = 4)
        {
            if(GlowOr!=0)
            GlowOr = 10;
            if (Glow==null)
            {
                Glow = DDTextures.Nullpng.Value;
            }
            float RO = projectile.rotation;
            SpriteEffects sprite = 0;
            Vector2 Origia = new Vector2(texture.Width * 0.5f, projectile.height / 2);
            Vector2 Origia2 = new Vector2(Glow.Width * 0.5f, (GlowOr+ projectile.height / 2) * GlowScale);
            if (projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipVertically;
                RO -= Rotation2;
                Origia = new Vector2(texture.Width * 0.5f, texture.Height - projectile.height/2);
                Origia2 = new Vector2(Glow.Width * 0.5f, Glow.Height - (GlowOr + projectile.height / 2) * GlowScale);
            }

            Main.spriteBatch.Draw(Glow, projectile.Center - Main.screenPosition, null, DDGlobalProjectile.GlowColor[projectile.type], RO, Origia2, projectile.scale/GlowScale, sprite, 0f);
            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, color, RO, Origia, projectile.scale, sprite, 0f);
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, projectile.position - Main.screenPosition, null, Color.White*0.5F, 0, Vector2.Zero, projectile.Size/2, 0, 0f);
        }
        public static void DrawCentre(this Texture2D texture, Projectile projectile,Rectangle? rectangle, Color color, Vector2 Scale, float Rotation2 = MathHelper.Pi, Texture2D Glow = null, float GlowScale = 4)
        {
            if (Glow == null)
            {
                Glow = DDTextures.Nullpng.Value;
            }
            float RO = projectile.rotation+ Rotation2;
            SpriteEffects sprite = 0;
            Vector2 Origia = texture.Size() / 2;
            Vector2 Origia2 = Glow.Size() / 2;
            if (projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }

            Main.spriteBatch.Draw(Glow, projectile.Center - Main.screenPosition, rectangle, DDGlobalProjectile.GlowColor[projectile.type], RO, Origia2, Scale / GlowScale, sprite, 0f);
            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, rectangle, color, RO, Origia, Scale, sprite, 0f);
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, projectile.position - Main.screenPosition, null, Color.White*0.5F, 0, Vector2.Zero, projectile.Size/2, 0, 0f);
        }
        public static void DrawCentre(this Texture2D texture, Projectile projectile,Rectangle? rectangle, Color color, float Scale = 1, float Rotation2 = MathHelper.Pi, Texture2D Glow = null, float GlowScale = 4)
        {
            if (Glow == null)
            {
                Glow = DDTextures.Nullpng.Value;
            }
            float RO = projectile.rotation+ Rotation2;
            SpriteEffects sprite = 0;
            if(rectangle==null)
            {
                rectangle = new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height));
            }
            Vector2 Origia = rectangle.Value.Size() / 2;
            Vector2 Origia2 = rectangle.Value.Size() / 2;
            if (projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }

            Main.spriteBatch.Draw(Glow, projectile.Center - Main.screenPosition, rectangle, DDGlobalProjectile.GlowColor[projectile.type], RO, Origia2, Scale / GlowScale, sprite, 0f);
            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, rectangle, color, RO, Origia, Scale, sprite, 0f);
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, projectile.position - Main.screenPosition, null, Color.White*0.5F, 0, Vector2.Zero, projectile.Size/2, 0, 0f);
        }
        public static void 绘制偏移头部拖尾(Texture2D texture,Vector2 Center, Projectile projectile, Color color, float Rotation2 = MathHelper.Pi, Texture2D Glow =null,int GlowOr = 20,float GlowScale = 4)
        {
            if(GlowOr!=0)
            GlowOr = 10;
            if (Glow==null)
            {
                Glow = DDTextures.Nullpng.Value;
            }
            float RO = projectile.rotation;
            SpriteEffects sprite = 0;
            Vector2 Origia = new Vector2(texture.Width * 0.5f, projectile.height / 2);
            Vector2 Origia2 = new Vector2(Glow.Width * 0.5f, (GlowOr+ projectile.height / 2) * GlowScale);
            if (projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipVertically;
                RO -= Rotation2;
                Origia = new Vector2(texture.Width * 0.5f, texture.Height - projectile.height/2);
                Origia2 = new Vector2(Glow.Width * 0.5f, Glow.Height - (GlowOr + projectile.height / 2) * GlowScale);
            }

            Main.spriteBatch.Draw(Glow, Center - Main.screenPosition, null, DDGlobalProjectile.GlowColor[projectile.type], RO, Origia2, projectile.scale/GlowScale, sprite, 0f);
            Main.spriteBatch.Draw(texture, Center - Main.screenPosition, null, color, RO, Origia, projectile.scale, sprite, 0f);
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, projectile.position - Main.screenPosition, null, Color.White*0.5F, 0, Vector2.Zero, projectile.Size/2, 0, 0f);
        }
        public static Vector2 MicroStop(this Vector2 vector) => new Vector2(0,0.01F);
        /// <summary>
        /// 可见度
        /// </summary>
        public static float Visible(this Projectile projectile) => (1-(float)projectile.alpha / 255);
        public static Item ActiveItem(this Player player)
        {
            return player.inventory[player.selectedItem];
        }
        public static float IteUseAnimation(this Player player)
        {
            float A = player.ActiveItem().useAnimation;
            if (A<1)
            {
                A = 1;
            }
            return A;
        }
        public static float IteUseAnimation2(this Player player)
        {
            float A = player.ActiveItem().useAnimation / player.GetTotalAttackSpeed(player.ActiveItem().DamageType);
            if (A<1)
            {
                A = 1;
            }
            return A;
        }
        public static Vector2 ArmCenter(this Player player)
        {
            Vector2 cen = new Vector2(6 * player.direction, 0);
            if (player.itemRotation.ToRotationVector2().Y > 0)
            {
                cen = new Vector2(6 * player.direction, 0);
            }
            return player.MountedCenter-cen;
        }
        public static int Second(float v) => (int)(v * 60); 
        public static int Second(int v) => v * 60; 
        public static int MaxandMin(ref int Value, int Max, int Min)
        {
            if(Value>Max)
            {
                Value = Max;
            }
            if(Value<Min)
            {
                Value = Min;
            }
            return Value;
        }
        public static float MaxandMinF(ref float Value, float Max, float Min)
        {
            if (Value > Max)
            {
                Value = Max;
            }
            if (Value < Min)
            {
                Value = Min;
            }
            return Value;
        }
        public static int ItemManaV(this Player player)
        {
            if (!Main.mouseItem.IsAir)
            {
                return player.GetManaCost(Main.mouseItem);
            }
            return player.GetManaCost(player.HeldItem);
        }
        public static int ItemMana(this Player player)
        {
            if (ItemManaV(player)*2 >= player.statMana && player.statLifeMax2 >= ItemManaV(player) && player.manaFlower)
            {
                player.QuickMana();
            }
            return ItemManaV(player);
        }
        public static int ItemMana(this Player player,float Magnification)
        {
            if (ItemManaV(player)* Magnification * 2 >= player.statMana && player.statLifeMax2 >= ItemManaV(player)* Magnification && player.manaFlower)
            {
                player.QuickMana();
            }
            return ItemManaV(player);
        }
        /// <summary>
        /// 0:正常均速 1:速度越来越慢 2:变大变淡 3:从淡变小 4:速度越来越快 5:速度越来越慢加变大变淡 6:随机移动,7向上，8速度越来越慢并且变黑,9向下,10从淡变小但是消失会变大
        /// </summary>]
        public static int DustAI(this Dust dust, int i = 0)
        {
            return 1000 * i;
        }
        public static float ItemUseTime(this Player player)
        {
            if (!Main.mouseItem.IsAir)
            {
                return player.GetWeaponAttackSpeed(Main.mouseItem);
            }
            return player.GetWeaponAttackSpeed(player.HeldItem);
        }
        /// <summary> 使用时将向量归一化 </summary>
        public static Vector2 PerfectNormalize(this Vector2 vector) => vector == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(vector);
        public static void DirectPerfectNormalize(this ref Vector2 vector) => vector = vector.PerfectNormalize();

        /// <summary> 缓慢看向某个地方,Rotation要达到的方向,Speed转向速度 </summary>
        public static void RotationSpeed(this Player player, float targetRotation, float speed)
        {
            // 规范化角度到 [0, 2π)
            targetRotation = NormalizeAngle(targetRotation);
            float currentRotation = NormalizeAngle(player.fullRotation);

            // 计算最短路径
            float difference = targetRotation - currentRotation;

            // 处理跨越0度的情况，选择最短旋转方向
            if (Math.Abs(difference) > MathHelper.Pi)
            {
                difference = difference > 0 ? difference - MathHelper.TwoPi : difference + MathHelper.TwoPi;
            }

            // 应用旋转
            if (Math.Abs(difference) <= speed)
            {
                // 如果距离很小，直接到达目标
                player.fullRotation = targetRotation;
            }
            else
            {
                // 否则按速度旋转
                player.fullRotation = NormalizeAngle(currentRotation + Math.Sign(difference) * speed);
            }
        }

        private static float NormalizeAngle(float angle)
        {
            angle %= MathHelper.TwoPi;
            if (angle < 0f)
                angle += MathHelper.TwoPi;
            return angle;
        }
        /// <summary> 当看向某方向时候会返回ture,OwnRotation初始方向, SpecifyRotation到达方向,Sector扇形范围扩散 </summary>
        public static bool SpecifyDirection(float OwnRotation, float SpecifyRotation, float Sector)
        {
            float diff = Math.Abs(NormalizeAngle(OwnRotation) - NormalizeAngle(SpecifyRotation));
            return Math.Min(diff, MathHelper.TwoPi - diff) <= Sector;
        }
        /// <summary> 让一个值来回浮动 </summary>
        public static bool BackAndForthInt(int Min, int Max, int Speed, ref int Value, ref bool Bool)
        {
            if (Bool)
            {
                Value += Speed;
            }
            else
            {
                Value -= Speed;
            }
            bool r = false;
            if (Value >= Max)
            {
                Bool = false;
                r = true;
            }
            if (Value <= Min)
            {
                Bool = true;
                r = true;
            }
            if (Value < Min)
            {
                Value = Min;
            }
            if (Value > Max)
            {
                Value = Max;
            }
            return r;
        }
        public static bool BackAndForthInt(byte Min, byte Max, byte Speed, ref byte Value, ref bool Bool)
        {
            if (Bool)
            {
                Value += Speed;
            }
            else
            {
                Value -= Speed;
            }
            bool r = false;
            if (Value >= Max)
            {
                Bool = false;
                r = true;
            }
            if (Value <= Min)
            {
                Bool = true;
                r = true;
            }
            if (Value < Min)
            {
                Value = Min;
            }
            if (Value > Max)
            {
                Value = Max;
            }
            return r;
        }
        /// <summary> 让一个值来回浮动 </summary>
        public static bool BackAndForth(float Min, float Max, float Speed, ref float Value, ref bool Bool,bool LockValue = true)
        {
            if (Bool)
            {
                Value += Speed;
            }
            else
            {
                Value -= Speed;
            }
            bool r = false;
            if (Value >= Max)
            {
                Bool = false;
                r = true;
            }
            if (Value <= Min)
            {
                Bool = true;
                r = true;
            }
            if (LockValue)
            {
                if (Value < Min)
                {
                    Value = Min;
                }
                if (Value > Max)
                {
                    Value = Max;
                }
            }
            return r;
        }
        /// <summary> 旋转,OwnRotation初始方向, SpecifyRotation到达方向,Sector扇形范围扩散 </summary>
        public static void RotateSpeed(ref float OwnRotation, float SpecifyRotation, float Speed)
        {
            SpecifyRotation = NormalizeAngle(SpecifyRotation);
            OwnRotation = NormalizeAngle(OwnRotation);

            // 计算最短角度差并选择旋转方向
            float diff = SpecifyRotation - OwnRotation;
            if (Math.Abs(diff) > MathHelper.Pi)
            {
                diff = diff > 0 ? diff - MathHelper.TwoPi : diff + MathHelper.TwoPi;
            }

            // 应用旋转或直接到达目标
            if (Math.Abs(diff) <= Speed)
            {
                OwnRotation = SpecifyRotation;
            }
            else
            {
                OwnRotation = NormalizeAngle(OwnRotation + Math.Sign(diff) * Speed);
            }
        }
        /// <summary>
        /// 属于装备
        /// </summary>
        public static bool IsArmor(this Item item)
        {
            return (item.headSlot != -1 || item.bodySlot != -1 || item.legSlot != -1) && !item.vanity;
        }
        /// <summary> 绘制发光用,type==0绘制在手上,1绘制在弹幕上 </summary>

        public static void drawGlow(int type, Item item, Player player, Vector2 poisoned, Rectangle? sourceRect, float rotation, Vector2 origin, float scale, SpriteEffects Effects, PlayerDrawSet drawinfo)
        {
            if (item.DItem().TwinGlow == 0)
            {
                if (item.glowMask != -1 && type == 0)
                {
                    Color glowLighting = new Color(255, 255, 255, item.alpha);
                    glowLighting = player.GetImmuneAlpha(item.GetAlpha(glowLighting) * player.stealth, 0f);
                    DrawData glowData = new(TextureAssets.GlowMask[(int)item.glowMask].Value, poisoned, sourceRect, glowLighting, rotation, origin, scale, Effects, 0);
                    drawinfo.DrawDataCache.Add(glowData);
                }
            }
            if(item.DItem().TwinGlow==1)
            {
                Color glowLighting = new Color(255, 255, 255, item.alpha);
                glowLighting = player.GetImmuneAlpha(item.GetAlpha(glowLighting) * player.stealth, 0f);
                DrawData glowData = new(机械魔眼剑盾Proj.Glow.Value, poisoned, sourceRect, glowLighting, rotation, origin, scale, Effects, 0);
                drawinfo.DrawDataCache.Add(glowData);
            }
            if(item.DItem().TwinGlow==2)
            {
                Color glowLighting = new Color(255, 255, 255, item.alpha);
                glowLighting = player.GetImmuneAlpha(item.GetAlpha(glowLighting) * player.stealth, 0f);
                DrawData glowData = new(流星剑盾Proj.Glow.Value, poisoned, sourceRect, glowLighting, rotation, origin, scale, Effects, 0);
                drawinfo.DrawDataCache.Add(glowData);
            }
            if(item.DItem().TwinGlow==3)
            {
                Color glowLighting = new Color(255, 255, 255, item.alpha);
                glowLighting = player.GetImmuneAlpha(item.GetAlpha(glowLighting) * player.stealth, 0f);
                DrawData glowData = new(绿岩剑盾Proj.Glow.Value, poisoned, sourceRect, glowLighting, rotation, origin, scale, Effects, 0);
                drawinfo.DrawDataCache.Add(glowData);
            }
            if(item.DItem().TwinGlow==4)
            {
                Color glowLighting = new Color(255, 255, 255, item.alpha);
                glowLighting = player.GetImmuneAlpha(item.GetAlpha(glowLighting) * player.stealth, 0f);
                DrawData glowData = new(诅咒双刃Proj.Glow.Value, poisoned, sourceRect, glowLighting, rotation, origin, scale, Effects, 0);
                drawinfo.DrawDataCache.Add(glowData);
            }
        }
        /// <summary> 贴图压缩使用,texture获取压缩贴图的部分信息,color颜色,rotation方向,opacity不透明度,Scale压缩程度(越大越扁),Direction方向,CircularRotation旋转,blendMode选择混合模式 </summary>
        public static void Compression(Texture2D texture, Color color, float rotation, float opacity, Vector2 Scale, float Direction, float CircularRotation, BlendState blendMode)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, blendMode, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Matrix viewMatrix;
            Matrix projectionMatrix;
            playerHelper.CalculatePerspectiveMatricies(out viewMatrix, out projectionMatrix,0);
            var shader = GameShaders.Misc["压缩"];
            shader.UseColor(color);
            shader.UseSaturation(rotation);
            if (blendMode == BlendState.Additive)
            {
                shader.UseOpacity(opacity);
            }
            else
            {
                shader.UseOpacity((float)color.A/255);
            }
            shader.Shader.Parameters["usc"].SetValue(Scale);
            shader.Shader.Parameters["uDirection"].SetValue((float)Direction);
            shader.Shader.Parameters["uCircularRotation"].SetValue(CircularRotation);
            shader.Shader.Parameters["uImageSize0"].SetValue(Utils.Size(texture));
            shader.Shader.Parameters["overallImageSize"].SetValue(Utils.Size(texture));
            shader.Shader.Parameters["uWorldViewProjection"].SetValue(viewMatrix * projectionMatrix);
            shader.Apply(default);
        }
        /// <summary> 刀光使用,texture获取额外绘制贴图,color颜色,opacity不透明度,Flipped翻转,blendMode选择混合模式 </summary>
        public static void BladeTrail(Asset<Texture2D> texture, Color color, float opacity, bool Flipped, float Width )
        {
            var shader = GameShaders.Misc["刀光"];
            shader.SetShaderTexture(texture);
            shader.UseColor(color);
            shader.UseSecondaryColor(color);
            shader.Shader.Parameters["FlameColor"].SetValue(color.ToVector3());
            shader.Shader.Parameters["uWidth"].SetValue(Width);
            shader.Shader.Parameters["flipped"].SetValue(Flipped);
            shader.Shader.Parameters["uDarkshade"].SetValue((float)color.A/255*opacity);
            shader.Shader.Parameters["uAlpha"].SetValue(1F);
            shader.Apply(default(DrawData?));
        }
        /// <summary> 刀光使用,texture获取额外绘制贴图,color颜色,opacity不透明度,Flipped翻转,blendMode选择混合模式 </summary>
        public static void BladeTrail(Asset<Texture2D> texture, Color color, float opacity, bool Flipped)
        {
            var shader = GameShaders.Misc["刀光"];
            shader.SetShaderTexture(texture);
            shader.UseColor(color);
            shader.UseSecondaryColor(color);
            shader.Shader.Parameters["FlameColor"].SetValue(color.ToVector3());
            shader.Shader.Parameters["uWidth"].SetValue(1f);
            shader.Shader.Parameters["flipped"].SetValue(Flipped);
            shader.Shader.Parameters["uDarkshade"].SetValue((float)color.A/255*opacity);
            shader.Shader.Parameters["uAlpha"].SetValue(1F);
            shader.Apply(default(DrawData?));
        }
        /// <summary> 刀光使用,texture获取额外绘制贴图,color颜色,opacity不透明度,Flipped翻转,blendMode选择混合模式 </summary>
        public static void BladeTrail(Asset<Texture2D> texture, Color[] color, float opacity, bool Flipped)
        {
            var shader = GameShaders.Misc["刀光"];
            shader.SetShaderTexture(texture);
            shader.UseColor(color[0]);
            shader.UseSecondaryColor(color[1]);
            shader.Shader.Parameters["FlameColor"].SetValue(color[2].ToVector3());
            shader.Shader.Parameters["uWidth"].SetValue(1f);
            shader.Shader.Parameters["flipped"].SetValue(Flipped);
            shader.Shader.Parameters["uDarkshade"].SetValue(opacity);
            shader.Shader.Parameters["uAlpha"].SetValue(1F);
            shader.Apply(default(DrawData?));
        }
        /// <summary> 环形进度条,矩阵,绘制,位置,最大值,当前值,两半的颜色,透明度,大小 </summary>
        public static void DrawExpanded(Matrix matrix,SpriteBatch spriteBatch, Vector2 position, float MaxValue, float Value, Color color, Color color2, float opacity = 1, float scale = 1)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, matrix);
            Color End = color*0.2f;
            Color End2 = color2 * 0.2f;
            End.A = 255;
            End2.A = 255;            
            ApplyBarShaders(opacity, End, End2, MaxValue);
            spriteBatch.Draw(DDTextures.Circle[3].Value, position, default(Rectangle?), Color.White * opacity, -MathHelper.PiOver2, DDTextures.Circle[3].Size() / 2, scale, (SpriteEffects)1, 0f);
            ApplyBarShaders(opacity, color, color2, Value);
            spriteBatch.Draw(DDTextures.Circle[3].Value, position, default(Rectangle?), Color.White * opacity, -MathHelper.PiOver2, DDTextures.Circle[3].Size() / 2, scale, (SpriteEffects)1, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, matrix);
        }
        public static void ApplyBarShaders(float opacity, Color color, Color SecondaryColor, float T)
        {
            var shader = GameShaders.Misc["环性进度条"];
            shader.UseOpacity(opacity);
            shader.UseSaturation(T);
            shader.UseColor(color);
            shader.UseSecondaryColor(SecondaryColor);
            shader.Apply();
        }
        /// <summary> 猿形shader,opacity透明度,color颜色,Time条纹移动速度</summary>
        public static void RotundityShaders(float opacity, Color color, float Time, float umax = 1)
        {
            if (umax > 1) umax = 1;

            var shader = GameShaders.Misc["圆形"];
            shader.UseOpacity(opacity);
            shader.UseColor(color);
            shader.Shader.Parameters["uTime2"].SetValue(Time);
            shader.Shader.Parameters["umax"].SetValue(umax);
            shader.Apply();
        }
        /// <summary> 环形shader,opacity透明度,color颜色,Time条纹移动速度</summary>
        public static void AnnularShaders(float opacity, Color color, float Time)
        {
            var shader = GameShaders.Misc["环形"];
            shader.UseOpacity(opacity);
            shader.UseColor(color);
            shader.Shader.Parameters["uTime2"].SetValue(Time);
            shader.Apply();
        }
        /// <summary> 盾shader,opacity透明度,color颜色,Time条纹移动速度</summary>
        public static void shieldShaders(float opacity, Color color, float Time)
        {
            var shader = GameShaders.Misc["盾"];
            shader.UseOpacity(opacity);
            shader.UseColor(color);
            shader.Shader.Parameters["uTime2"].SetValue(Time);
            shader.Apply();
        }
        /// <summary> 测试盾shader,opacity透明度,color颜色,Time条纹移动速度</summary>
        public static void 测试shieldShaders(float opacity, Color color, float Time, Vector2 vector, bool uBool, bool uBool2 = true, float Alpha = 0)
        {
            var shader = GameShaders.Misc["测试盾2"];
            shader.UseOpacity(opacity);
            shader.UseColor(color);
            if (uBool2)
            {
                shader.Shader.Parameters["uBool"].SetValue(false);
                shader.Shader.Parameters["uTime5"].SetValue(0.65f);
            }
            else
            {
                shader.Shader.Parameters["uBool"].SetValue(true);
                shader.Shader.Parameters["uTime4"].SetValue(0.35f);
            }
            shader.Shader.Parameters["uTime2"].SetValue(Time);
            shader.Shader.Parameters["uTime3"].SetValue(Time);
            shader.Shader.Parameters["uImageSize0"].SetValue(vector);
            shader.Shader.Parameters["Alpha"].SetValue(Alpha);
            shader.Apply();
        }
        /// <summary> 测试盾shader,opacity透明度,color颜色,Time条纹移动速度</summary>
        public static void 测试shieldShaders2(float opacity, Color color, float Time, Vector2 vector, bool uBool, bool uBool2 = true, float Alpha = 0)
        {
            Time %= 1.5F;
            Time -= 1F;

            var shader = GameShaders.Misc["测试盾2"];
            shader.UseOpacity(opacity);
            shader.UseColor(color);
            shader.Shader.Parameters["uTime2"].SetValue(Time);
            shader.Shader.Parameters["uTime3"].SetValue(Time);
            if (uBool2)
            {
                shader.Shader.Parameters["uTime4"].SetValue(0f);
                shader.Shader.Parameters["uTime5"].SetValue(0.35f);
            }
            else
            {
                shader.Shader.Parameters["uTime4"].SetValue(0.35f);
                shader.Shader.Parameters["uTime5"].SetValue(0.585f);
            }
            shader.Shader.Parameters["uBool"].SetValue(uBool);
            shader.Shader.Parameters["uBool2"].SetValue(uBool2);
            shader.Shader.Parameters["uImageSize0"].SetValue(vector);
            shader.Shader.Parameters["Alpha"].SetValue(Alpha);
            shader.Apply();
        }
        /// <summary> 测试</summary>
        public static void 测试NPC滤镜(float opacity, Color color, float Time)
        {
            var shader = GameShaders.Misc["渲染滤镜"];
            shader.UseOpacity(opacity);
            shader.SetShaderTexture(DDTextures.远古背景);
            shader.UseColor(color);
            shader.Shader.Parameters["uImageSize1"].SetValue(DDTextures.远古背景.Size());
            shader.Shader.Parameters["renderTargetArea"].SetValue(new Vector2(300, 2160));
            shader.Shader.Parameters["uWorldPosition"].SetValue(Main.screenPosition + new Vector2(0, Time));
            shader.Shader.Parameters["upscaleFactor"].SetValue(new Vector2(-0.7F));
            shader.Apply();
        }
        public static void DrawTextBubbles(SpriteBatch spriteBatch,Texture2D texture ,Texture2D texture2, Vector2 position,string Text,float Textscale, Color Textcolor, Color color, DynamicSpriteFont font, int L, SpriteEffects spriteEffects = 0)
        {
            DrawTextBubblesV(spriteBatch, texture, texture2, position-new Vector2(0,10), Text, new Vector2(Textscale), Textcolor,color, font, L, spriteEffects);
        }
        public static void DrawTextBubblesV(SpriteBatch spriteBatch,Texture2D texture ,Texture2D texture2, Vector2 position,string Text,Vector2 Textscale, Color Textcolor, Color color, DynamicSpriteFont font,int L, SpriteEffects spriteEffects = 0)
        {
            if(font==null)
            {
                font = FontAssets.MouseText.Value;
            }
            TextDisplayCache textDisplay = new TextDisplayCache();
            textDisplay.PrepareCache(Text, font,L);
            string[] textLines = textDisplay.TextLines;
            int amountOfLines = textDisplay.AmountOfLines + 1;
            for (int A = 0; A < amountOfLines; A++)
            {
                if (A != amountOfLines - 1)
                {
                    textLines[A] = textLines[A].Remove(textLines[A].Length - 1).Replace("-", "");
                    //C -= ChatManager.GetStringSize(font, "-", Vector2.One, 0).X;
                }
            }
            float C = ChatManager.GetStringSize(font, textLines[0], Vector2.One, 0).X / 2;
            for (int A = 0; A < amountOfLines; A++)
            {
                if (C < ChatManager.GetStringSize(font, textLines[A], Vector2.One, 0).X / 2)
                {
                    C = ChatManager.GetStringSize(font, textLines[A], Vector2.One, 0).X / 2;
                }
            }

            C *= Textscale.X / 2;
            

            for (int A = 0; A < amountOfLines; A++)
            {
                Vector2 vector = new Vector2(0, -50 - 20 * (amountOfLines - 1 - A));
                if (A == 0)
                {
                    spriteBatch.Draw(texture2, position + vector - Main.screenPosition - new Vector2(0, 12), new Rectangle?(new Rectangle(6, 0, 4, 10)), color, 0, new Vector2(4, 10) / 2, new Vector2(C+2, 1), 0, 0f);
                }
                if (A == amountOfLines - 1)
                {
                    spriteBatch.Draw(texture2, position + vector - Main.screenPosition + new Vector2(0, 12), new Rectangle?(new Rectangle(6, 10, 4, 10)), color, 0, new Vector2(4, 10) / 2, new Vector2(C + 2, 1), 0, 0f);
                }
                spriteBatch.Draw(texture2, position + vector - Main.screenPosition, new Rectangle?(new Rectangle(8, 8, 4, 4)), color, 0, new Vector2(4, 4) / 2, new Vector2(C + 2, 6), 0, 0f);
                spriteBatch.Draw(texture2, position + vector - Main.screenPosition - new Vector2(4 * C / 2 + 4, 0), new Rectangle?(new Rectangle(0, 8, 4, 4)), color, 0, new Vector2(4, 4) / 2, new Vector2(1, 6+1.5F), 0, 0f);
                spriteBatch.Draw(texture2, position + vector - Main.screenPosition + new Vector2(4 * C / 2 + 4, 0), new Rectangle?(new Rectangle(16, 8, 4, 4)), color, 0, new Vector2(4, 4) / 2, new Vector2(1, 6+ 1.5F), 0, 0f);

            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
            for (int A = 0; A < amountOfLines; A++)
            {
                Vector2 vector = new Vector2(0, -50 - 20 * (amountOfLines - 1 - A));
                Vector2 origin = ChatManager.GetStringSize(font, textLines[A], Vector2.One, 0) / 2;

                ChatManager.DrawColorCodedStringWithShadow(
                    spriteBatch,
                    font, textLines[A], position + vector - Main.screenPosition + new Vector2(0, 6 - origin.Y) - new Vector2(4 * C / 2, 0), Textcolor, 0, new Vector2(0, 0), Textscale);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
            spriteBatch.Draw(texture, position + new Vector2(0, -38) - Main.screenPosition, null, color, 0, new Vector2(texture.Width, texture.Height) / 2, 1, spriteEffects, 0f);
        }
        public class TextDisplayCache
        {
            private string _originalText;
            private int _lastScreenWidth;
            private int _lastScreenHeight;

            public string[] TextLines { get; private set; }
            public int AmountOfLines { get; private set; }

            public void PrepareCache(string text, DynamicSpriteFont font, int maxWidth)
            {
                if (Main.screenWidth != _lastScreenWidth ||
                    Main.screenHeight != _lastScreenHeight ||
                    _originalText != text)
                {
                    _lastScreenWidth = Main.screenWidth;
                    _lastScreenHeight = Main.screenHeight;
                    _originalText = text;

                    if (font == FontAssets.DeathText.Value)
                    {
                        TextLines = Utils.WordwrapString(text, font, 800, 10, out int lineAmount);
                        AmountOfLines = lineAmount;
                    }
                    else
                    {
                        TextLines = Utils.WordwrapString(text, font, maxWidth, 10, out int lineAmount);
                        AmountOfLines = lineAmount;
                    }
                }
            }
        }

        public struct CustomVertexInfo : IVertexType
        {
            private static readonly VertexDeclaration _vertexDeclaration = new(new[]
            {
        new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
        new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
        new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
    });

            public Vector2 Position;
            public Color Color;
            public Vector3 TexCoord;

            public CustomVertexInfo(Vector2 position, Color color, Vector3 texCoord)
            {
                Position = position;
                Color = color;
                TexCoord = texCoord;
            }

            public CustomVertexInfo(Vector2 position, Vector3 texCoord) : this(position, Color.White, texCoord)
            {
            }

            public VertexDeclaration VertexDeclaration => _vertexDeclaration;
        }
        public static float ReadFloat(this BinaryReader w) { return w.ReadSingle(); }
    }
}