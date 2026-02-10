using DDmod.NoContent.Config;
using System.Collections.Generic;
using System.Linq;
using Terraria.Graphics.Shaders;

namespace DDmod
{
    public class Trailing
    {
        public int MType = 0;
        public int DegreeOfBezierCurveCornerSmoothening;
        public VertexWidthFunction WidthFunction;
        public VertexColorFunction ColorFunction;
        public BasicEffect BaseEffect;
        public MiscShaderData SpecialShader;
        public TrailPointRetrievalFunction TrailPointFunction;
        public Vector2 OverridingStickPointStart = Vector2.Zero;
        public Vector2 OverridingStickPointEnd = Vector2.Zero;

        public Trailing(VertexWidthFunction widthFunction, VertexColorFunction colorFunction,
            TrailPointRetrievalFunction pointFunction = null, MiscShaderData specialShader = null, int matrixType = 0)
        {
            WidthFunction = widthFunction;
            ColorFunction = colorFunction;
            TrailPointFunction = pointFunction ?? SmoothBezierPointRetreivalFunction;
            SpecialShader = specialShader;
            MType = matrixType;

            BaseEffect = new BasicEffect(Main.instance.GraphicsDevice)
            {
                VertexColorEnabled = true,
                TextureEnabled = false
            };

            UpdateBaseEffect(out _, out _, matrixType);
        }

        public void UpdateBaseEffect(out Matrix effectProjection, out Matrix effectView, int matrix)
        {
            playerHelper.CalculatePerspectiveMatricies(out effectView, out effectProjection, matrix);
            BaseEffect.View = effectView;
            BaseEffect.Projection = effectProjection;
        }

        public static List<Vector2> RigidPointRetreivalFunction(IEnumerable<Vector2> originalPositions, Vector2 generalOffset, int totalTrailPoints, IEnumerable<float> _ = null)
        {
            var basePoints = originalPositions.Where(p => p != Vector2.Zero).ToList();

            if (basePoints.Count < 2)
            {
                for (int i = 0; i < basePoints.Count; i++)
                    basePoints[i] += generalOffset;
                return basePoints;
            }

            var endPoints = new List<Vector2>();
            for (int j = 0; j < totalTrailPoints; j++)
            {
                float completionRatio = j / (totalTrailPoints - 1f);
                int currentIndex = (int)(completionRatio * (basePoints.Count - 1));
                Vector2 currentPoint = basePoints[currentIndex];
                Vector2 nextPoint = basePoints[(currentIndex + 1) % basePoints.Count];
                endPoints.Add(Vector2.Lerp(currentPoint, nextPoint, completionRatio * (basePoints.Count - 1) % 0.99999f) + generalOffset);
            }

            endPoints.Add(basePoints.Last() + generalOffset);
            return endPoints;
        }

        public List<Vector2> SmoothBezierPointRetreivalFunction(IEnumerable<Vector2> originalPositions, Vector2 generalOffset, int totalTrailPoints, IEnumerable<float> originalRotations = null)
        {
            var controlPoints = new List<Vector2>();

            // 保持原有的遍历方式，只简化语法
            foreach (var position in originalPositions)
            {
                if (position != Vector2.Zero)
                    controlPoints.Add(position + generalOffset);
            }

            if (controlPoints.Count <= 1)
                return controlPoints;

            bool pointCountCondition = DegreeOfBezierCurveCornerSmoothening <= 0 || controlPoints.Count < DegreeOfBezierCurveCornerSmoothening * 3;
            int effectivePointCount = DegreeOfBezierCurveCornerSmoothening > 0 ? totalTrailPoints * DegreeOfBezierCurveCornerSmoothening : totalTrailPoints;

            // 保持原有的位运算逻辑
            var filteredPoints = controlPoints.Where((_, i) =>
                (DegreeOfBezierCurveCornerSmoothening <= 0 ||
                 i % DegreeOfBezierCurveCornerSmoothening == 0 ||
                 i == 0 || i == controlPoints.Count - 1) | pointCountCondition).ToArray();

            var bezierCurve = new BezierCurve(filteredPoints);
            return bezierCurve.GetPoints(effectivePointCount);
        }

        public static List<Vector2> SmoothCatmullRomPointRetreivalFunction(IEnumerable<Vector2> originalPositions, Vector2 generalOffset, int _, IEnumerable<float> originalRotations)
        {
            var positions = originalPositions.ToArray();
            var rotations = originalRotations.ToArray();
            var smoothenedPoints = new List<Vector2>();

            for (int i = 0; i < positions.Length - 1; i++)
            {
                Vector2 current = positions[i];
                Vector2 ahead = positions[i + 1];

                if (current == Vector2.Zero || ahead == Vector2.Zero)
                    continue;

                float currentRotation = MathHelper.WrapAngle(rotations[i]);
                float aheadRotation = MathHelper.WrapAngle(rotations[i + 1]);
                int pointsToAdd = (int)Math.Round(Math.Abs(MathHelper.WrapAngle(aheadRotation - currentRotation)) * 8f / 3.1415927f);

                smoothenedPoints.Add(current + generalOffset);

                if (pointsToAdd != 0)
                {
                    float segmentLength = Vector2.Distance(current, ahead);
                    float increment = 1f / (pointsToAdd + 2);
                    Vector2 backEnd = current + Utils.ToRotationVector2(currentRotation) * segmentLength;
                    Vector2 frontEnd = ahead + Utils.ToRotationVector2(aheadRotation) * -segmentLength;

                    for (float j = increment; j < 1f; j += increment)
                    {
                        smoothenedPoints.Add(Vector2.CatmullRom(backEnd, current, ahead, frontEnd, j) + generalOffset);
                    }
                }
            }
            return smoothenedPoints;
        }

        public VertexPosition2DColor[] GetVerticesFromTrailPoints(List<Vector2> trailPoints, float scale, float alpha)
        {
            var vertices = new VertexPosition2DColor[trailPoints.Count * 2 - 2];

            for (int i = 0; i < trailPoints.Count - 1; i++)
            {
                float completionRatio = i / (float)trailPoints.Count;
                float widthAtVertex = WidthFunction(completionRatio) * scale;
                Color vertexColor = ColorFunction(completionRatio) * alpha;

                Vector2 current = trailPoints[i];
                Vector2 directionToAhead = Utils.SafeNormalize(trailPoints[i + 1] - current, Vector2.Zero);
                Vector2 sideDirection = new Vector2(-directionToAhead.Y, directionToAhead.X);

                Vector2 left = current - sideDirection * widthAtVertex;
                Vector2 right = current + sideDirection * widthAtVertex;

                if (i == 0 && OverridingStickPointStart != Vector2.Zero)
                {
                    left = OverridingStickPointStart;
                    right = OverridingStickPointEnd;
                }

                vertices[i * 2] = new VertexPosition2DColor(left, vertexColor, new Vector2(completionRatio, 0f));
                vertices[i * 2 + 1] = new VertexPosition2DColor(right, vertexColor, new Vector2(completionRatio, 1f));
            }

            return vertices;
        }

        public short[] GetIndicesFromTrailPoints(int pointCount)
        {
            var indices = new short[(pointCount - 1) * 6];

            for (int i = 0; i < pointCount - 2; i++)  // 保持原有的循环条件
            {
                int startIndex = i * 6;
                int vertexIndex = i * 2;

                indices[startIndex] = (short)vertexIndex;
                indices[startIndex + 1] = (short)(vertexIndex + 1);
                indices[startIndex + 2] = (short)(vertexIndex + 2);
                indices[startIndex + 3] = (short)(vertexIndex + 2);
                indices[startIndex + 4] = (short)(vertexIndex + 1);
                indices[startIndex + 5] = (short)(vertexIndex + 3);
            }

            return indices;
        }

        public void Draw(IEnumerable<Vector2> originalPositions, Vector2 generalOffset, int totalTrailPoints,
            IEnumerable<float> originalRotations = null, float scale = 1, float alpha = 1, SpriteBatch spriteBatch = null)
        {
            spriteBatch ??= Main.spriteBatch;

            var rasterizerState = new RasterizerState
            {
                CullMode = CullMode.None,
                ScissorTestEnable = true
            };

            Main.instance.GraphicsDevice.RasterizerState = rasterizerState;

            int actualTrailPoints = (int)(totalTrailPoints * ModContent.GetInstance<DDConfigClient>().SlashEffect);
            var trailPoints = TrailPointFunction(originalPositions, generalOffset, actualTrailPoints, originalRotations);

            if (trailPoints.Count < 2 || trailPoints.Any(Utils.HasNaNs) || trailPoints.All(p => p == trailPoints[0]))
                return;

            UpdateBaseEffect(out Matrix projection, out Matrix view, MType);

            var vertices = GetVerticesFromTrailPoints(trailPoints, scale, alpha);
            var triangleIndices = GetIndicesFromTrailPoints(trailPoints.Count);

            if (triangleIndices.Length % 6 != 0 || vertices.Length % 2 != 0)
                return;

            if (SpecialShader != null)
            {
                SpecialShader.Shader.Parameters["uWorldViewProjection"].SetValue(view * projection);
                SpecialShader.Apply();
            }
            else
            {
                BaseEffect.CurrentTechnique.Passes[0].Apply();
            }

            spriteBatch.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, triangleIndices, 0, triangleIndices.Length / 3);
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }

        public struct VertexPosition2DColor : IVertexType
        {
            public Vector2 Position;
            public Color Color;
            public Vector2 TextureCoordinates;

            private static readonly VertexDeclaration _vertexDeclaration = new(new[]
            {
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
            });

            public VertexPosition2DColor(Vector2 position, Color color, Vector2 textureCoordinates)
            {
                Position = position;
                Color = color;
                TextureCoordinates = textureCoordinates;
            }

            public VertexDeclaration VertexDeclaration => _vertexDeclaration;
        }

        public delegate float VertexWidthFunction(float completionRatio);
        public delegate Color VertexColorFunction(float completionRatio);
        public delegate List<Vector2> TrailPointRetrievalFunction(IEnumerable<Vector2> originalPositions, Vector2 generalOffset, int totalTrailPoints, IEnumerable<float> originalRotations = null);
    }
}