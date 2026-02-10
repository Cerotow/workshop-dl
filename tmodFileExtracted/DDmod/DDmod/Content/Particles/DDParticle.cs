
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Particles
{
    public class DDParticle
    {
        /// <summary> 引用模组颗粒 </summary>
        public static void RequestParticleSpawn(ParticleType type, ParticleOrchestraSettings settings, int? overrideInvokingPlayerIndex = null)
        {
            settings.IndexOfPlayerWhoInvokedThis = (byte)Main.myPlayer;
            if (overrideInvokingPlayerIndex.HasValue)
            {
                settings.IndexOfPlayerWhoInvokedThis = (byte)overrideInvokingPlayerIndex.Value;
            }
            SpawnParticlesDirect(type, settings);
        }
        public static void SpawnParticlesDirect(ParticleType type, ParticleOrchestraSettings settings)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                switch (type)
                {
                    case ParticleType.Heart:
                        HeartParticles.Heart(settings);
                        break;
                    case ParticleType.Star:
                        StarParticles.Star(settings);
                        break;
                    case ParticleType.blackHole:
                        blackHoleParticles.BlackHole(settings);
                        break;
                }
            }
        }
    }

    /// <summary> 选择颗粒 </summary>
    public enum ParticleType : byte
    {
        /// <summary> 爱心颗粒</summary>
        Heart,
        /// <summary> 星星颗粒</summary>
        Star,
        /// <summary> 黑洞颗粒</summary>
        blackHole
    }

}