using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class WhitePhaseblade : ALightsaber
    {
        public override Color Endcolor => new Color(55, 55, 55, 255);
        public override Color color => new Color(200, 180, 240, 0);
        public override Color color2 => new Color(55, 75, 15, 0) * 0.5f;
    }
}
