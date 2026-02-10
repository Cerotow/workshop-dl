using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class RedPhasesaber : ALightsaber
    {
        public override void Set()
        {
            Projectile.height = 250;
            Width = 84;
            oldVels = 108;

        }
        public override Color color => new Color(255, 0, 0, 0);
        public override Color color2 => new Color(0, 205, 205, 0);
    }
}
