using DDmod.Content.Tiles;
using System.Reflection;
using Terraria.Graphics.Shaders;

namespace DDmod
{
    internal static class playerHelper
    {
        public static void CalculatePerspectiveMatricies(out Matrix viewMatrix, out Matrix projectionMatrix, int MatrixType)
        {
            if (MatrixType == 0)
            {
                Vector2 zoom = Main.GameViewMatrix.Zoom;
                Matrix zoomScaleMatrix = Matrix.CreateScale(zoom.X, zoom.Y, 1f);
                int width = Main.instance.GraphicsDevice.Viewport.Width;
                int height = Main.instance.GraphicsDevice.Viewport.Height;
                viewMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up);
                viewMatrix *= Matrix.CreateTranslation(0f, (float)(-(float)height), 0f);
                viewMatrix *= Matrix.CreateRotationZ(3.1415927f);
                if (Main.LocalPlayer.gravDir == -1f)
                {
                    viewMatrix *= Matrix.CreateScale(1f, -1f, 1f) * Matrix.CreateTranslation(0f, height, 0f);
                }
                viewMatrix *= zoomScaleMatrix;
                projectionMatrix = Matrix.CreateOrthographicOffCenter(0f, width * zoom.X, 0f, height * zoom.Y, 0f, 1f) * zoomScaleMatrix;
            }
            else
            {
                Vector2 zoom = new Vector2(Main.UIScale, Main.UIScale);
                Matrix zoomScaleMatrix = Main.UIScaleMatrix;
                int width = Main.screenWidth;
                int height = Main.screenHeight;
                viewMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up);
                viewMatrix *= Matrix.CreateTranslation(0f, (float)(-(float)height), 0f);
                viewMatrix *= Matrix.CreateRotationZ(3.1415927f);
                viewMatrix *= zoomScaleMatrix;
                projectionMatrix = Matrix.CreateOrthographicOffCenter(0f, width * zoom.X, 0f, height * zoom.Y, 0f, 1f);
            }
        }
        public static MiscShaderData SetShaderTexture(this MiscShaderData shader, Asset<Texture2D> texture)
        {
            UImageFieldMisc.SetValue(shader, texture);
            return shader;
        }
        public static Color MulticolorLerp(float increment, params Color[] colors)
        {
            increment %= 0.999f;
            int currentColorIndex = (int)(increment * colors.Length);
            Color color = colors[currentColorIndex];
            Color nextColor = colors[(currentColorIndex + 1) % colors.Length];
            return Color.Lerp(color, nextColor, increment * colors.Length % 1f);
        }
        public static float Lerp(float value1, float value2, float amount)
        {
            amount = MathHelper.Clamp(amount, 0f, 1f);
            return MathHelper.Lerp((int)value1, (int)value2, amount);
        }
        public static float FMulticolorLerp(float increment, params float[] v)
        {
            increment %= 0.999f;
            int currentColorIndex = (int)(increment * v.Length);
            float color = v[currentColorIndex];
            float nextColor = v[(currentColorIndex + 1) % v.Length];
            return MathHelper.Lerp(color, nextColor, increment * v.Length % 1f);
        }
        public static TileEntity FindTileEntity(int i, int j, int width, int height, int sheetSquare = 16)
        {
            Tile t = Main.tile[i, j];
            int left = i - (int)t.TileFrameX % (width * sheetSquare) / sheetSquare;
            int top = j - (int)t.TileFrameY % (height * sheetSquare) / sheetSquare;
            TileEntity te;
            if (TileEntity.ByPosition.TryGetValue(new Point16(left, top), out te))
            {
                return te;
            }
            else
            {
                return null;
            }
        }
        public static T FindTileEntity2<T>(int i, int j, int width, int height, int sheetSquare = 16) where T : ModTileEntity
        {
            Tile t = Main.tile[i, j];

            int left = i - (t.TileFrameX) % (width * sheetSquare) / sheetSquare;
            int top = j - (t.TileFrameY) % (height * sheetSquare) / sheetSquare;

            byte chargerType = ModContent.GetInstance<T>().type;
            TileEntity te;
            if (TileEntity.ByPosition.TryGetValue(new Point16(left, top), out te))
            {
                return (T)((object)te);
            }
            else
            {
                return default(T);
            }
        }
        public static T FindTileEntity3<T>(int i, int j, int width, int height, int WidthsheetSquare = 16, int HeightsheetSquare = 16) where T : ModTileEntity
        {
            Tile t = Main.tile[i, j];

            int left = i - (t.TileFrameX) % (width * WidthsheetSquare) / WidthsheetSquare;
            int top = j - (t.TileFrameY) % (height * HeightsheetSquare) / HeightsheetSquare;

            byte chargerType = ModContent.GetInstance<T>().type;
            TileEntity te;
            if (TileEntity.ByPosition.TryGetValue(new Point16(left, top), out te))
            {
                return (T)((object)te);
            }
            else
            {
                return default(T);
            }
        }

        internal static readonly FieldInfo UImageFieldMisc = typeof(MiscShaderData).GetField("_uImage1", (BindingFlags)36);
    }
}
