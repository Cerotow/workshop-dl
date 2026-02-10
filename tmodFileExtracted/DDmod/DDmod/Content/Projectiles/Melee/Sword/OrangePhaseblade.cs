using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class OrangePhaseblade : ALightsaber
    {
        public override Color Endcolor => new Color(55, 55, 55, 255);
        public override Color color => new Color(255, 155, 48, 0);
        public override Color color2 => new Color(0, 100, 207, 0) * 0.5f;
    }
}
