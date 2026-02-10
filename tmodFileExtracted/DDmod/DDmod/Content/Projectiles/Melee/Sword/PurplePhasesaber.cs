using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class PurplePhasesaber : ALightsaber
    {
        public override void Set()
        {
            Projectile.height = 250;
            Width = 84;
            oldVels = 108;

        }
        public override Color color => new Color(233, 50, 233, 0);
        public override Color color2 => new Color(22, 205, 22, 0);
    }
}
