using DDmod.Content;
using DDmod.Content.Items;
using DDmod.Content.Items.Boss.MeteorAnnihilatorItems;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Series.ShadowFlame;
using DDmod.Content.Items.农场.水壶;
using DDmod.Players;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace DDmod.AccessorySlot
{
    public class AccessorySystem : ModSystem
    {
        //手表栏
        public static int WatchSlots;
        //法宝栏
        public static int TalismanSlots;
    }

    public class AccessoryPlayer : ModPlayer
    {
        public int wing;
        public int wingslot;
        public int wingFrameCounter;
        public int wingFrame;
        public override void ResetEffects()
        {
            wingslot = 0;
        }
        public override void PreUpdate()
        {
            if ((Player.wingsLogic > 0 && Player.controlJump && Player.wingTime > 0f && Player.jump == 0 && Player.velocity.Y != 0f) || Player.jump > 0)
            {
                wingFrameCounter++;
                if (wingFrameCounter > 4)
                {
                    wingFrame++;
                    wingFrameCounter = 0;

                    if (wingFrame >= 5)
                        wingFrame = 0;
                }
            }
            else if (!Player.controlJump || Player.velocity.Y == 0f)
            {
                wingFrame = 5;
            }
            else
            {
                wingFrame = 1;
            }
        }
        public void GrappleMovement()
        {
        }
        public override void Load()
        {
        }
    }
    //手表栏
    public class WatchSlot : ModAccessorySlot
    {
        public override bool HasEquipmentLoadoutSupport => false;
        public override Vector2? CustomLocation => new Vector2(564, 20);
        public override string FunctionalTexture => "Terraria/Images/Item_" + 15;
        public override string FunctionalBackgroundTexture => "DDmod/UI/InterfaceUI/表UI";
        public override bool DrawVanitySlot => false;
        public override bool DrawDyeSlot => false;
        public override void OnMouseHover(AccessorySlotType context)
        {
            switch (context)
            {
                case AccessorySlotType.FunctionalSlot:
                    Main.hoverItemName = DDSystem.English ? "Watch" : "表";
                    break;
            }
        }
        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
        {
            if (AccessorySystem.WatchSlots != Type)
            {
                AccessorySystem.WatchSlots = Type;
            }
            if (checkItem.AccessoryItem().Watch)
            {
                return true;
            }
            return false;
        }
        public override void Load()
        {
            AccessorySystem.WatchSlots = Type;
        }
    }
    //法宝栏
    public class TalismanSlot : ModAccessorySlot
    {
        public override bool HasEquipmentLoadoutSupport => false;
        public override Vector2? CustomLocation
        {
            get
            {
                int A = 172;
                if (Main.mapStyle == 1)
                {
                    A += Main.miniMapHeight + 16;
                }
                return new Vector2?(new Vector2(Main.screenWidth - 246 - (Main.netMode == 1 ?40 : 0), A));
            }
        }

        public override string FunctionalTexture => "Terraria/Images/Item_" + 0;
        public override string FunctionalBackgroundTexture => "DDmod/UI/InterfaceUI/法宝UI";
        public override void OnMouseHover(AccessorySlotType context)
        {
            switch (context)
            {
                case AccessorySlotType.FunctionalSlot:
                    Main.hoverItemName = DDSystem.English ? "Talisman" : "法宝";
                    break;
            }
        }
        public override bool DrawFunctionalSlot => (Main.EquipPage != 1 && (!UILinkPointNavigator.Shortcuts.NPCS_IconsDisplay || !PlayerInput.UsingGamepad));
        public override bool DrawVanitySlot =>false;
        public override bool DrawDyeSlot => false;
        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
        {
            if (AccessorySystem.TalismanSlots != Type)
            {
                AccessorySystem.TalismanSlots = Type;
            }
            if (checkItem.Titem().Talisman)
            {
                return true;
            }
            return false;
        }
        public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo)
        {
            return false;
        }

        public override bool IsHidden()
        {
            return false;
        }
        public override bool IsEnabled()
        {
            return true;
        }
        public override bool IsVisibleWhenNotEnabled()
        {
            return !IsEmpty;
        }

        public override void Load()
        {
            AccessorySystem.TalismanSlots = Type;
        }
    }
    //脖子绘制
    public class WingSlot2 : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new BeforeParent(PlayerDrawLayers.HeadBack);
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            return drawInfo.shadow == 0f && !drawPlayer.dead&&false;
        }
        protected override void Draw(ref PlayerDrawSet drawinfo)
        {
            DrawData item;
            Color color = drawinfo.drawPlayer.skinColor;
            Color LightColor = Lighting.GetColor((int)(drawinfo.drawPlayer.Center.X / 16), (int)(drawinfo.drawPlayer.Center.Y / 16), color);
            Texture2D texture = ModContent.Request<Texture2D>("DDmod/Textures/脖子").Value;
            Vector2 vec = drawinfo.drawPlayer.MountedCenter - Main.screenPosition;
            vec -= new Vector2(-1* drawinfo.drawPlayer.direction, 7);
            item = new DrawData(texture, vec.Floor(), null, LightColor, drawinfo.drawPlayer.bodyRotation, texture.Size() / 2f,1, drawinfo.playerEffect, 0);
            item.shader = drawinfo.cHead;
            drawinfo.DrawDataCache.Add(item);

            return;
        }
    }
    //翅膀绘制
    public class WingSlot : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new BeforeParent(PlayerDrawLayers.Wings);
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            AccessoryPlayer modPlayer = drawPlayer.AccPlayer();
            return drawInfo.shadow == 0f && !drawPlayer.dead && modPlayer.wingslot > 0;
        }
        protected override void Draw(ref PlayerDrawSet drawinfo)
        {
            DrawData item;
            AccessoryPlayer modPlayer = drawinfo.drawPlayer.AccPlayer();
            if (modPlayer.wingslot== ModContent.ItemType<流星冲刺翼>())
            {
                Color color = Color.White;
                Vector2 vector = new Vector2(12f, 2f);
                Texture2D texture = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
                Vector2 vec = drawinfo.drawPlayer.MountedCenter - Main.screenPosition + vector * drawinfo.drawPlayer.Directions - Vector2.UnitX * drawinfo.drawPlayer.direction * 14f;
                vec.Y += drawinfo.drawPlayer.gfxOffY;
                Rectangle rectangle = texture.Frame(1, 2, 0, 0);
                if(drawinfo.drawPlayer.wingTimeMax>0&&drawinfo.drawPlayer.wingTime<=0&& drawinfo.drawPlayer.controlJump)
                {
                    rectangle = texture.Frame(1, 2, 0, 1);
                }
                rectangle.Width -= 2;
                rectangle.Height -= 2;
                item = new DrawData(texture, vec.Floor(), rectangle, color, drawinfo.drawPlayer.bodyRotation, rectangle.Size() / 2f, 1f, drawinfo.playerEffect, 0);
                item.shader = drawinfo.cWings;
                drawinfo.DrawDataCache.Add(item);

                return;
            }
        }
    }
}