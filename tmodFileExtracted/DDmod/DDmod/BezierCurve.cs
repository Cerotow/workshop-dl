namespace DDmod
{
    public class BezierCurve
    {
        public BezierCurve(params Vector2[] controls)
        {
            this.ControlPoints = controls;
        }
        public Vector2 Evaluate(float interpolant)
        {
            return this.PrivateEvaluate(this.ControlPoints, MathHelper.Clamp(interpolant, 0f, 1f));
        }
        public List<Vector2> GetPoints(int totalPoints)
        {
            float perStep = 1f / totalPoints;
            List<Vector2> points = new List<Vector2>();
            for (float step = 0f; step <= 1f; step += perStep)
            {
                points.Add(this.Evaluate(step));
            }
            return points;
        }
        private Vector2 PrivateEvaluate(Vector2[] points, float T)
        {
            while (points.Length > 2)
            {
                Vector2[] nextPoints = new Vector2[points.Length - 1];
                for (int i = 0; i < points.Length - 1; i++)
                {
                    nextPoints[i] = Vector2.Lerp(points[i], points[i + 1], T);
                }
                points = nextPoints;
            }
            return Vector2.Lerp(points[0], points[1], T);
        }
        public Vector2[] ControlPoints;
    }
}
