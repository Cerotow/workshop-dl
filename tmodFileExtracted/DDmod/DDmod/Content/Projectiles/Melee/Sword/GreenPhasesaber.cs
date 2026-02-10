using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class GreenPhasesaber : ALightsaber
    {
        public override void Set()
        {
            Projectile.height = 250;
            Width = 84;
            oldVels = 108;

        }
        public override Color color => new Color(0, 255, 0, 0);
        public override Color color2 => new Color(255, 0, 255, 0);
    }
}
