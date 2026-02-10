using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Boss.天地守卫;
using DDmod.Content.Items.Boss.狱火蛇物品;
using DDmod.Content.Items.Boss.鬼牙;
using Terraria.Enums;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.Relic
{
    public class 天地守卫圣物Tile : ModTile
    {
        public const int FrameWidth = 18 * 3;
        public const int FrameHeight = 18 * 4;
        public const int HorizontalFrames = 1;
        public const int VerticalFrames = 1;

        public Asset<Texture2D> RelicTexture;
        public Asset<Texture2D> RelicTextureGlow;

        public virtual string RelicTextureName => "DDmod/Content/Tiles/Relic/天地守卫圣物Tile";
        public virtual string RelicTextureNameGlow => "DDmod/Content/Tiles/Relic/天地守卫圣物Tile_Glow";

        public override string Texture => "DDmod/Content/Tiles/Relic/RelicPedestalHard";

        public override void Load()
        {
            if (!Main.dedServ)
            {
                RelicTexture = ModContent.Request<Texture2D>(RelicTextureName);
                RelicTextureGlow = ModContent.Request<Texture2D>(RelicTextureNameGlow);
            }
        }
        public override void Unload()
        {
            RelicTexture = null;
        }
        public override void SetStaticDefaults()
        {
            Main.tileShine[Type] = 400;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.InteractibleByNPCs[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.StyleHorizontal = false;

            TileObjectData.newTile.StyleWrapLimitVisualOverride = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.styleLineSkipVisualOverride = 0;

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(233, 207, 94), Language.GetText("MapObject.Relic"));
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }
        public override bool RightClick(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int left = i - (int)tile.TileFrameX % (3 * 16) / 16;
            int top = j - (int)tile.TileFrameY % (4 * 16) / 16;
            tile = Main.tile[left, top];
            if (tile.TileFrameX == 0)
            {
                tile.TileFrameX += 1;
            }
            else
            {
                tile.TileFrameX -= 1;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.LocalPlayer.whoAmI, left, top, 1, 1);
            }
            return true;
        }
        bool R;
        float Sc = 0;
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            DDHelper.BackAndForth(0.8F, 1.1F, 0.005F, ref Sc, ref R);
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            tileFrameX %= FrameWidth;
            tileFrameY %= FrameHeight * 2;
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (drawData.tileFrameX % FrameWidth == 0 && drawData.tileFrameY % FrameHeight == 0)
            {
                Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
            }
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = (tile.TileFrameY == 36) ? 18 : 16;
            if (tile.TileFrameX % FrameWidth == 0)
            {
                Main.spriteBatch.Draw(TextureAssets.Tile[Type].Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y + 2)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY, 16, height)), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(TextureAssets.Tile[Type].Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X) - 1, (float)(j * 16 - (int)Main.screenPosition.Y + 2)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY, 16, height)), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
            }
            if ((tile.TileFrameX % FrameWidth == 0|| tile.TileFrameX % FrameWidth == 1) && tile.TileFrameY % FrameHeight == 0)
                TileLoader.SpecialDraw(Type,i,j,spriteBatch );
            return false;
        }
        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D Scanning = DDTextures.Scanning2.Value;
            Vector2 offScreen = new Vector2(Main.offScreenRange);
            if (Main.drawToScreen)
            {
                offScreen = Vector2.Zero;
            }

            Point p = new Point(i, j);
            Tile tile = Main.tile[p.X, p.Y];
            if (tile == null || !tile.HasTile)
            {
                return;
            }

            Texture2D texture = RelicTexture.Value;
            Texture2D textureGlow = RelicTextureGlow.Value;

            int frameY = tile.TileFrameX / FrameWidth;
            Rectangle frame = texture.Frame(HorizontalFrames, VerticalFrames, 0, frameY);

            Vector2 origin = frame.Size() / 2f;
            Vector2 worldPos = p.ToWorldCoordinates(24f, 64f);

            Color color = Color.White;

            bool direction = tile.TileFrameY / FrameHeight != 0;
            SpriteEffects effects = direction ? SpriteEffects.FlipHorizontally : SpriteEffects.None;


            const float TwoPi = (float)Math.PI * 2f;
            float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 5f);
            Vector2 drawPos = worldPos + offScreen - Main.screenPosition + new Vector2(0f, -50f) + new Vector2(0f, offset * 4f);
            if (tile.TileFrameX % FrameWidth == 0)
            {
                spriteBatch.Draw(textureGlow, drawPos, null, new Color(255,255,255,0), 0f, textureGlow.Size()/2, Sc/4, effects, 0f);
            }
            spriteBatch.Draw(texture, drawPos, frame, color, 0f, origin, 1, effects, 0f);


            Lighting.AddLight(new Vector2(i, j) * 16, new Color(204, 181, 72).ToVector3());
            float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 2f) * 0.3f + 1;
            Color effectColor = color;
            effectColor.A = 0;
            effectColor = effectColor * 0.1f * scale;
            for (float num5 = 0f; num5 < 1f; num5 += 355f / (678f * (float)Math.PI))
            {
                spriteBatch.Draw(texture, drawPos + (TwoPi * num5).ToRotationVector2() * (6f + offset * 2f), frame, effectColor, 0f, origin, 1, effects, 0f);
            }
            if (tile.TileFrameX % FrameWidth == 0)
            {
                color = new Color(204, 181, 72, 0);
                Main.spriteBatch.Draw(Scanning, worldPos + offScreen - new Vector2(0, 8) - Main.screenPosition, null, color, 0, new Vector2(Scanning.Width / 2, Scanning.Height / 2), new Vector2(0.8F, 0.4F), 0, 0f);
                Main.spriteBatch.Draw(Scanning, worldPos + offScreen - new Vector2(0, 8) - Main.screenPosition, null, color, 0, new Vector2(Scanning.Width / 2, Scanning.Height / 2), new Vector2(0.8F, 0.4F), 0, 0f);

                Main.spriteBatch.Draw(Scanning, worldPos + offScreen - new Vector2(0, 8) - Main.screenPosition, null, color, 0, new Vector2(Scanning.Width / 2, Scanning.Height / 2 + 10), new Vector2(0.8F, 0.4F - offset / 5), 0, 0f);
                Main.spriteBatch.Draw(Scanning, worldPos + offScreen - new Vector2(0, 8) - Main.screenPosition, null, color, 0, new Vector2(Scanning.Width / 2, Scanning.Height / 2 + 10), new Vector2(0.8F, 0.4F - offset / 5), 0, 0f);
            }
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];

            player.cursorItemIconID = -1;
            string defaultName = TileLoader.DefaultContainerName(tile.TileType,tile.TileFrameX,tile.TileFrameY)/* tModPorter Note: new method takes in FrameX and FrameY */;
            if (player.cursorItemIconText == defaultName)
            {
                player.cursorItemIconID = ModContent.ItemType<天地守卫圣物>();

                player.cursorItemIconText = "";
            }

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }

        public override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
            Player player = Main.LocalPlayer;
            if (player.cursorItemIconText == "")
            {
                player.cursorItemIconEnabled = false;
                player.cursorItemIconID = 0;
            }
        }
    }
}
