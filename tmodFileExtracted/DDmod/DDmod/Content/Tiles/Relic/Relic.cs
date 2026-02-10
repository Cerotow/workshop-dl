using DDmod.Content.Items.Boss.LifeGuardItems;
using DDmod.Content.Items.Boss.MeteorAnnihilatorItems;
using Terraria.Enums;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.Relic
{
    public enum RelicType
    {
        PreHardmode,
        Hardmode,
        PreHardmodeMini,
        HardmodeMini,
    }
    public abstract class Relic : ModTile
    {
        public const int FrameWidth = 18 * 3;
        public const int FrameHeight = 18 * 4;
        public const int HorizontalFrames = 1;
        public const int VerticalFrames = 1;

        public Asset<Texture2D> RelicTexture;
        public Asset<Texture2D> RelicTexture2;
        public virtual RelicType RelicType =>0;
        public override string Texture
        {
            get
            {
                if(RelicType==RelicType.Hardmode)
                {
                    return "DDmod/Content/Tiles/Relic/RelicPedestalHard";
                }
                if(RelicType==RelicType.PreHardmodeMini)
                {

                    return "DDmod/Content/Tiles/Relic/RelicPedestalMini";
                }
                return "DDmod/Content/Tiles/Relic/RelicPedestal";
            }
        }

        public virtual string RelicTextureName => (GetType().Namespace + "." + Name).Replace('.', '/');
        public virtual string RelicTextureName2 => null;
        public virtual Vector2 OffsetPosition => new Vector2(0f, -50f);

        public override void Load()
        {
            if (!Main.dedServ)
            {
                RelicTexture = ModContent.Request<Texture2D>(RelicTextureName);
                if(RelicTextureName2!=null)
                {

                    RelicTexture2 = ModContent.Request<Texture2D>(RelicTextureName2);
                }
            }
        }
        public override void Unload()
        {
            RelicTexture = null;
            RelicTexture2 = null;
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
        public float R = 0;
        public float Sc = 0;
        public float ro = 0;
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            R += 0.006f;
            if (R > MathHelper.TwoPi * 6)
            {
                R = 0;
            }
            if (R > MathHelper.TwoPi * 3)
            {
                if (Sc < 20)
                {
                    Sc += 0.3f;
                }
            }
            else
            {
                if (Sc < 30)
                {
                    Sc += 0.3f;
                }
                else
                {
                    Sc -= 0.3f;
                }
            }
            ro += 0.05f;
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
            Vector2 offScreen = new Vector2(Main.offScreenRange);
        }

        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
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

            int frameY = tile.TileFrameX / FrameWidth;
            Rectangle frame = texture.Frame(HorizontalFrames, VerticalFrames, 0, frameY);

            Vector2 origin = frame.Size() / 2f;
            Vector2 worldPos = p.ToWorldCoordinates(24f, 64f);

            Color color = Lighting.GetColor(p.X, p.Y);

            bool direction = tile.TileFrameY / FrameHeight != 0;
            SpriteEffects effects = direction ? SpriteEffects.FlipHorizontally : SpriteEffects.None;


            const float TwoPi = (float)Math.PI * 2f;
            float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 5f);
            Vector2 drawPos = worldPos + offScreen - Main.screenPosition + OffsetPosition + new Vector2(0f, offset * 4f);

            //for (int A = 0; A < 100; A++)
            {
                spriteBatch.Draw(texture, drawPos, frame, color, 0f, origin, 1, effects, 0f);
            }


            float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 2f) * 0.3f + 1;
            Color effectColor = color;
            effectColor.A = 0;
            effectColor = effectColor * 0.1f * scale;
            for (float num5 = 0f; num5 < 1f; num5 += 355f / (678f * (float)Math.PI))
            {
                spriteBatch.Draw(texture, drawPos + (TwoPi * num5).ToRotationVector2() * (6f + offset * 2f), frame, effectColor, 0f, origin, 1, effects, 0f);
            }
        }
    }
}
