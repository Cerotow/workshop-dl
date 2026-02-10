using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class PurplePhaseblade : ALightsaber
    {
        public override Color Endcolor => new Color(55, 55, 55, 255);
        public override Color color => new Color(233, 50, 233, 0);
        public override Color color2 => new Color(22, 205, 22, 0) * 0.5f;
    }
}
