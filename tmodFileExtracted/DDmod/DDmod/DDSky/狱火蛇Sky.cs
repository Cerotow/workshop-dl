using DDmod.Content.NPCs.Boss.MeteorAnnihilator;
using DDmod.Content.NPCs.Boss.狱火蛇;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.DDOn;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Filters = Terraria.Graphics.Effects.Filters;

namespace DDmod.Worlds
{
    public class 狱火蛇背景 : ModSceneEffect
    {
        public static bool 狱火;
        public override SceneEffectPriority Priority
        {
            get
            {
                return SceneEffectPriority.Event;
            }
        }
        public override bool IsSceneEffectActive(Player player)
        {
            if(狱火)
            {
                狱火 = false;
                return true;
            }
            return false;
        } 

        public override void SpecialVisuals(Player player, bool isActive)
        {
            SkyDowned.Sky("狱火蛇Sky", isActive);
        }
    }
    public class 狱火蛇Sky : CustomSky
    {
        public override void OnLoad()
        {
        }
        public override void Update(GameTime gameTime)
        {
            if (Main.rand.NextBool(5))
                杂物Sky.NewDirectionalProj(4, 10, 6, 2, false, true, false, false);
            if (isActive && intensity < 0.3f)
            {
                intensity += 0.001f;
                return;
            }
            if (!isActive && intensity > 0f)
            {
                intensity -= 0.01f;
            }
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Vector2.Zero, null, new Color(253, 62, 3, 0)*0.4f * intensity, 0, Vector2.Zero, new Vector2(Main.screenWidth, Main.screenHeight)/2, 0, 0);
        }

        public override float GetCloudAlpha()
        {
            return 0f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
        }
        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }
        public override void Reset()
        {
            isActive = false;
        }
        public override bool IsActive()
        {
            return this.isActive || this.intensity > 0f|| 狱火蛇背景.狱火;
        }
        private bool isActive;
        private float intensity;
    }

}
