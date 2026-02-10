using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using ReLogic.Graphics;
using System.Media;
using Terraria.Audio;
using Terraria.ModLoader.IO;
using System.Collections.ObjectModel;

namespace DDmod.UI.抽奖UI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mouseItem"></param>
    /// <returns></returns>
    public delegate bool CheckPutSlotCondition(Item mouseItem);
    public delegate void ExchangeItemHandler(UIElement target);
    
    public class 抽奖UI信息 : UIElement
    {
        /// <summary>
        /// 是否可以放置物品
        /// </summary>
        public CheckPutSlotCondition CanPutInSlot { get; set; }
        /// <summary>
        /// 是否可以拿去物品
        /// </summary>
        public CheckPutSlotCondition CanTakeOutSlot { get; set; }
        /// <summary>
        /// 框的绘制的拐角尺寸
        /// </summary>
        public Vector2 CornerSize { get; set; }
        /// <summary>
        /// 绘制颜色
        /// </summary>
        public Color DrawColor { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Tooltip { get; set; }
        /// <summary>
        /// 更改物品时调用
        /// </summary>
        public event ExchangeItemHandler PostExchangeItem;
        /// <summary>
        /// 玩家拿取物品时调用
        /// </summary>
        public event ExchangeItemHandler OnPickItem;
        /// <summary>
        /// 大小
        /// </summary>
        public float Scale { get; set; }
        /// <summary>
        /// 透明度
        /// </summary>
        public float Opacity { get; set; }
        /// <summary>
        /// 第几号
        /// </summary>
        public int Type { get; set; }
        public bool R;
        public 抽奖UI信息(int type) : base()
        {
            Scale = 1f;
            Opacity = 1f;
            CanPutInSlot = null;
            DrawColor = new Color(0x3f, 0x41, 0x97) * 0.75f;
            CornerSize = new Vector2(10, 10);
            Type = type;
            Tooltip = "";
            R = false;
        }
        public int XP = 0;
        public Vector2 position;
        public Vector2 velocity;
        public override void Update(GameTime gameTime)
        {
            抽奖UI 抽奖UI = 抽奖UI.instance;
            XP++;
            if (跳B)
            {
                position.Y -= 12F;
                跳 += 1f;
            }
            else
            {
                跳 = 0;
            }
            if(跳>10)
            {
                跳B = false;
            }
            if(position==Vector2.Zero)
            {
                position = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2-120);
            }
            Width.Set(60, 0f);
            Height.Set(60, 0f);
            Vector2 vector = new Vector2(Main.screenWidth / 2 + 60 * 1.05F * (Type % 抽奖UI.Quantity - 抽奖UI.Quantity/2), Main.screenHeight / 2 + 60 * 1.05F * (1 + Type / 10));
            Vector2 SP = (vector - position).PerfectNormalize();
            float Speed = (position - vector).Length()/10;
            if(Speed > 20F)
            {
                Speed = 20F;
            }
            if(Speed < 0.3F)
            {
                Speed = 0.3F;
            }
            float inertia = 10;
            velocity = (velocity * (inertia-1) + SP * Speed) / inertia;
            if (XP / 10 > Type && !跳B&& (翻牌==1|| 翻牌 == -1))
            {
                if ((position - vector).Length() > 0.3F)
                {
                    position += velocity;
                }
                else
                {
                    position = vector;
                }
            }
            Left.Set(position.X, 0f);
            Top.Set(position.Y, 0f);
            if (ContainsPoint(Main.MouseScreen) && Main.LocalPlayer.itemAnimation == 0)
            {
                if ((Main.mouseLeft && !dian) || Main.mouseRight)
                {

                    Click();
                    dian = true;
                }
                if (!Main.mouseLeft)
                {
                    dian = false;
                }
                Main.LocalPlayer.mouseInterface = true;

                base.Update(gameTime);

            }
        }
        bool dian;

        public bool 看到物品;
        public float 翻牌 = -1;

        public float 跳 = 0;
        public bool 跳B;

        public float 翻牌特效;
        public bool 翻牌特效B;

        public float ro;
        public void Click()
        {
            抽奖UI 抽奖UI = 抽奖UI.instance;
            Vector2 vector = new Vector2(Main.screenWidth / 2 + 60 * 1.05F * (Type % 抽奖UI.Quantity - 抽奖UI.Quantity / 2), Main.screenHeight / 2 + 60 * 1.05F * (1 + Type / 10));
            float Speed = (position - vector).Length();
            if (!看到物品)
            {
                if (Speed < 0.01f)
                {
                    看到物品 = true;
                    跳B = true;
                    翻牌特效B = true;
                }
            }
            else
            {
                Item ContainedItem = 抽奖UI.Item[Type];
                if (翻牌 == 1 && Main.mouseItem.type == 0 && ContainedItem.type > 0)
                {
                    //如果可以拿起物品
                    if (CanTakeOutSlot == null || CanTakeOutSlot(ContainedItem))
                    {
                        //触发放物品声音
                        SoundEngine.PlaySound(new SoundStyle("DDmod/NoContent/Sounds/Items/背包"));
                        //拿出物品
                        Main.mouseItem = ContainedItem.Clone();
                        抽奖UI.Item[Type].SetDefaults(0, true);
                        //调用委托
                        OnPickItem?.Invoke(this);

                    }
                }
                //调用委托
                PostExchangeItem?.Invoke(this);
            }
        }
        protected override void DrawSelf(SpriteBatch sb)
        {
            抽奖UI 抽奖UI = 抽奖UI.instance;
            if (Type>= 抽奖UI.Quantity)
            {
                return;
            }
            ro += 0.05f;
            /// <summary>
            /// 框内物品
            /// </summary>
            Item ContainedItem = 抽奖UI.Item[Type];
            Main.instance.LoadItem(ContainedItem.type);
            int r = 0;
            if(翻牌>0)
            {
                r = 抽奖UI.Quality[Type];
                //调用原版的介绍绘制
                if (ContainsPoint(Main.MouseScreen) && ContainedItem.type != 0)
                {
                    Main.hoverItemName = ContainedItem.Name;
                    Main.HoverItem = ContainedItem.Clone();
                }
            }
            Texture2D texture = ModContent.Request<Texture2D>("DDmod/UI/抽奖UI/抽奖UI_"+ r).Value;
            //获取当前UI部件的信息
            var DrawRectangle = GetDimensions();
            //绘制物品框
            DrawAdvBox(sb, (int)DrawRectangle.X, (int)DrawRectangle.Y,
                (int)DrawRectangle.Width, (int)DrawRectangle.Height,
                Color.White, texture, CornerSize, Scale);

            if (ContainedItem.type != 0&& 翻牌>0)
            {
                var frame = Main.itemAnimations[ContainedItem.type] != null ? Main.itemAnimations[ContainedItem.type].GetFrame(TextureAssets.Item[ContainedItem.type].Value) : TextureAssets.Item[ContainedItem.type].Frame(1, 1, 0, 0);
                var size = frame.Size();
                var texScale = 0.8f;
                if (DrawRectangle.Width > DrawRectangle.Height)
                {
                    if (size.X > DrawRectangle.Width * 0.8F)
                    {
                        texScale *= (float)DrawRectangle.Width * 0.8F / size.X;
                    }
                }
                else
                {
                    if (size.Y > DrawRectangle.Height * 0.8F)
                    {
                        texScale *= (float)DrawRectangle.Height * 0.8F / size.Y;
                    }
                }
                //绘制物品贴图
                Vector2 vector = new Vector2(DrawRectangle.X + DrawRectangle.Width / 2, DrawRectangle.Y + DrawRectangle.Height / 2);

                Color color = new Color(112, 111, 170, 0);
                if (抽奖UI.Quality[Type] == 2)
                {
                    color = new Color(89, 97, 202, 0);
                    if (翻牌 == 1 && !R)
                    {
                        R = true;
                    }
                }
                if (抽奖UI.Quality[Type] == 3)
                {
                    color = new Color(221, 78, 198, 0);
                    if (翻牌 == 1 && !R)
                    {
                        R = true;
                    }
                }
                if (抽奖UI.Quality[Type] == 4)
                {
                    color = new Color(208, 104, 72, 0);
                    if (翻牌 == 1 && !R)
                    {
                        R = true;
                    }
                    sb.Draw(DDTextures.VoidStar.Value, vector, null, color, ro, DDTextures.VoidStar.Size() / 2, texScale * Scale/2, 0, 0);
                    sb.Draw(DDTextures.GlowEffect.Value, vector, null, color, ro, DDTextures.GlowEffect.Size() / 2, texScale * Scale /4, 0, 0);
                }
                if (抽奖UI.Quality[Type] == 5)
                {
                    color = new Color(255, 70, 70, 0);
                    if (翻牌 == 1&& !R)
                    {
                        R = true;
                    }
                    sb.Draw(DDTextures.VoidStar.Value, vector, null, color, ro, DDTextures.VoidStar.Size() / 2, texScale * Scale, 0, 0);
                    sb.Draw(DDTextures.GlowEffect.Value, vector, null, color, ro, DDTextures.GlowEffect.Size() / 2, texScale * Scale / 2, 0, 0);
                }

                sb.Draw(TextureAssets.Item[ContainedItem.type].Value, vector, new Rectangle?(frame), Color.White * Opacity, 0, size/2, new Vector2(翻牌, 1) * texScale * Scale, 0, 0);
                //绘制物品左下角那个代表数量的数字
                if (ContainedItem.stack > 1&& 翻牌==1)
                {
                    sb.DrawString(FontAssets.MouseText.Value, ContainedItem.stack.ToString(), new Vector2(DrawRectangle.X + 10, DrawRectangle.Y + DrawRectangle.Height/2), Color.White * Opacity, 0f, Vector2.Zero, new Vector2(翻牌, 1) * Scale *0.75F, SpriteEffects.None, 0f);
                }

                if (翻牌特效B)
                {
                    翻牌特效 += 0.2f;
                }
                else
                {
                    if (翻牌特效 > 0)
                    {
                        翻牌特效 -= 0.1f;
                    }
                    else
                    {
                        翻牌特效 = 0;
                    }
                }
                if(翻牌特效>1)
                {
                    if (!R)
                    {
                        if (抽奖UI.Quality[Type] == 1)
                        {
                            for (int a = 0; a < 20; a++)
                                UIDustDraw.NewDust(vector, 3, color, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)), Main.rand.NextFloat(0.2F, 0.3F));

                        }
                        if (抽奖UI.Quality[Type] == 2)
                        {
                            color = new Color(89, 97, 202, 0);
                                for (int a = 0; a < 40; a++)
                                    UIDustDraw.NewDust(vector, 3, color, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3)), Main.rand.NextFloat(0.2F, 0.4F));
                            
                        }
                        if (抽奖UI.Quality[Type] == 3)
                        {
                            color = new Color(221, 78, 198, 0);
                                for (int a = 0; a < 60; a++)
                                    UIDustDraw.NewDust(vector, 3, color, new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4)), Main.rand.NextFloat(0.2F, 0.5F));
                            
                        }
                        if (抽奖UI.Quality[Type] == 4)
                        {
                            color = new Color(208, 104, 72, 0);
                                for (int a = 0; a < 80; a++)
                                    UIDustDraw.NewDust(vector, 3, color, new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5)), Main.rand.NextFloat(0.2F, 0.6F));
                        }
                        if (抽奖UI.Quality[Type] == 5)
                        {
                            color = new Color(255, 70, 70, 0);
                                for (int a = 0; a < 120; a++)
                                    UIDustDraw.NewDust(vector, 3, color, new Vector2(Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6)), Main.rand.NextFloat(0.2F, 0.7F));
                        }
                        R = true;
                    }
                    翻牌特效B = false;
                }
                else
                {

                    R = false;
                }
                sb.Draw(DDTextures.VoidStar.Value, vector, null, color, ro, DDTextures.VoidStar.Size() / 2, 翻牌特效 * texScale * Scale* (抽奖UI.Quality[Type]/1.5f), 0,0);
                sb.Draw(DDTextures.GlowEffect.Value, vector, null, color, ro, DDTextures.GlowEffect.Size() / 2, 翻牌特效 * texScale * Scale*( 抽奖UI.Quality[Type]/3), 0,0);
            }
        }
        /// <summary>
        /// 绘制物品框
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="c"></param>
        /// <param name="img"></param>
        /// <param name="size4"></param>
        /// <param name="scale"></param>
        public void DrawAdvBox(SpriteBatch sp, int x, int y, int w, int h, Color c, Texture2D img, Vector2 size4, float scale = 1f)
        {
            if (看到物品&& !跳B)
            {
                if (翻牌 < 1)
                {
                    翻牌 += 0.1f;
                }
                else
                {
                    翻牌 = 1;
                }
            }
            var box = img;
            sp.Draw(box, new Vector2(x,y)+ box.Size()/2, null,c,0, box.Size() / 2, new Vector2(Math.Abs(翻牌),scale),0,0);
        }
    }
}
