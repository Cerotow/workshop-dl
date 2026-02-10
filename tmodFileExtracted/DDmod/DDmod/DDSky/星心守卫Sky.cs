using DDmod.Content.NPCs.Boss.MeteorAnnihilator;
using DDmod.Content.NPCs.Boss.星心守卫;
using DDmod.Content.NPCs.Boss.狱火蛇;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.DDOn;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace DDmod.Worlds
{
    public class 星心守卫背景 : ModSceneEffect
    {
        public static bool Open;
        public override SceneEffectPriority Priority
        {
            get
            {
                return SceneEffectPriority.Event;
            }
        }
        public override bool IsSceneEffectActive(Player player)
        {
            Open = NPC.AnyNPCs(ModContent.NPCType<星心守卫>());
            if (Open)
            {
                Open = false;
                return true;
            }
            return false;
        } 

        public override void SpecialVisuals(Player player, bool isActive)
        {
            SkyDowned.Sky("星心守卫Sky", isActive);
        }
    }
    public class 星心守卫Sky : CustomSky
    {
        public override void OnLoad()
        {
        }
        public override void Update(GameTime gameTime)
        {
            if (Main.rand.NextBool(10))
            {
                杂物Sky.NewDirectionalProj2(6, new Vector2(Main.rand.NextFloat(-8, 8), Main.rand.NextFloat(10, 16)), 5, 1, true, false, false, false);
            }
            if (Main.rand.NextBool(10))
            {
                杂物Sky.NewDirectionalProj2(7, new Vector2(0, -Main.rand.NextFloat(10, 16)), 5, 1, false, true, false, false);
            }
            if (isActive && intensity < 1f)
            {
                intensity += 0.01f;
                return;
            }
            if (!isActive && intensity > 0f)
            {
                intensity -= 0.01f;
            }
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (minDepth >6)
            {
                Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Vector2.Zero, null, new Color(0, 0, 0, 255) * intensity, 0, Vector2.Zero, new Vector2(Main.screenWidth, Main.screenHeight) / 2, 0, 0);
                Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Vector2.Zero, null, new Color(255, 0, 255, 0) * 0.3f * intensity, 0, Vector2.Zero, new Vector2(Main.screenWidth, Main.screenHeight) / 2, 0, 0);

            }
            Main.spriteBatch.Draw(DDTextures.Wire.Value, new Vector2(Main.screenWidth / 2, Main.screenHeight), null, new Color(255, 50, 50, 255)*0.7f * intensity, -MathHelper.PiOver2, new Vector2(DDTextures.Wire.Width() / 2, DDTextures.Wire.Height() / 2), new Vector2(1.7F, Main.screenHeight), 0, 0);
          
            Main.spriteBatch.Draw(DDTextures.Wire.Value, new Vector2(Main.screenWidth / 2, 0), null, new Color(0, 100, 255, 255) * 0.7f * intensity, MathHelper.PiOver2, new Vector2(DDTextures.Wire.Width() / 2, DDTextures.Wire.Height() / 2), new Vector2(1.7F, Main.screenHeight), 0, 0);
         
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
            return this.isActive || this.intensity > 0f|| 星心守卫背景.Open;
        }
        private bool isActive;
        private float intensity;
    }

}
