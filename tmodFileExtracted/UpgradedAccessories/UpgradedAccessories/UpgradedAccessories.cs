using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ModLoader;
using Mono.Cecil;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using UpgradedAccessories.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using UpgradedAccessories.Items.SpaceSuit;

namespace UpgradedAccessories {
    public class UpgradedAccessories : Mod {

        public static bool calamityLoaded;
        public static bool thoriumLoaded;
        public static Mod calamity;
        public static Mod thorium;
        public static ModHotKey gaussHover;
        public static Texture2D stardustShield;
        public static LegacySoundStyle clap;
        // special thanks to ¼ÛÇØ ¼±»ý´Ô
        public static LegacySoundStyle clapSpecial;

        public static UpgradedAccessories Instance;

        public enum NetworkMessageID {
            GAUSS_HOVER,
            CELESTIAL_HEALING
        }

        public UpgradedAccessories() {
        }

        public override void Load() {
            Instance = this;
            calamity = ModLoader.GetMod("CalamityMod");
            calamityLoaded = calamity != null;
            thorium = ModLoader.GetMod("ThoriumMod");
            thoriumLoaded = thorium != null;
            gaussHover = RegisterHotKey("Toggle Gauss Pack Hovering", "OemQuestion");
            stardustShield = ModContent.GetTexture("UpgradedAccessories/UI/StardustShield");
            IL.Terraria.Player.Hurt += Player_Hurt;
            SpaceSuit.LoadTexture();
        }

        // premultiplies alpha so textures with alpha difference gets rendered correctly in BlendState.AlphaBlend
        // without this, the blended pixel will be abnormally bright
        // alternative fix would be calling end and begin in predraw and postdraw with blend state nonpremultiplied hooks but that might strain GPU too much
        // xna uses premultiplied alpha to solve alpha cutouts; filtering with transparent pixel can "bleed" the color values from the it
        // more info on https://shawnhargreaves.com/blog/texture-filtering-alpha-cutouts.html
        // but that means rbg value of the overlaying pixel will not be scaled with alpha and overpower any background colors
        // Color.FromNonPremultiplied basically just multiplies rbg values with alpha, making it darker
        // apparently XNA content pipeline automatically does this
        private static void PreMultiplyAlpha(Texture2D texture) {
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);
            for(int i = 0; i < data.Length; i++) {
                data[i] = Color.FromNonPremultiplied(data[i].ToVector4());
            }
            texture.SetData(data);
        }

        public override void PostSetupContent() {
            clap = GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Custom/Clap");
            clapSpecial = GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Custom/ClapSpecial");
            if(!Main.dedServ) {
                PreMultiplyAlpha(Main.projectileTexture[ModContent.ProjectileType<CelestialYinyang>()]);
            }
        }

        public override void Unload() {
            Instance = null;
            calamityLoaded = false;
            thoriumLoaded = false;
            calamity = null;
            thorium = null;
            gaussHover = null;
            stardustShield = null;
            clap = null;
            clapSpecial = null;
            SpaceSuit.UnloadTexture();
        }

        private void Player_Hurt(ILContext il) {
            var c = new ILCursor(il);
            if(!c.TryGotoNext(MoveType.After,
                inst => inst.OpCode == OpCodes.Ldarg_0, // player
                inst => {
                    if(inst.OpCode == OpCodes.Ldfld) {
                        if(inst.Operand is FieldReference fieldRef) {
                            return fieldRef.Name.Equals(nameof(Player.statLife)); // load player life
                        }
                    }
                    return false;
                },
                inst => {
                    if(inst.OpCode == OpCodes.Ldloc_S) {
                        if(inst.Operand is VariableDefinition variableDef) {
                            return variableDef.Index == 7; // load 7th local variable(damage)
                        }
                    }
                    return false;
                },
                inst => inst.OpCode == OpCodes.Conv_I4)) { // cast the value(the damage variable) into int
                Logger.Error("Can't find ldarg.0, ldfld:Player::statLife, ldloc.s:V_7 then conv_i4. Can't apply patch");
                return;
            }
            // hijack the value
            c.Emit(OpCodes.Ldarg_0);
            c.EmitDelegate<Func<int, Player, int>>((dmg, player) => {
                var mp = player.GetModPlayer<MyPlayer>(); // use the shield value to reduce the damage, the player can take 0 damage
                if(mp.shield > dmg) {
                    mp.shield -= dmg;
                    dmg = 0;
                } else {
                    dmg -= mp.shield;
                    mp.shield = 0;
                }
                return dmg;
            });
            // the stack now has modified damage value that will reduce player health
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI) {
            var messageID = (NetworkMessageID)reader.ReadInt32();
            switch(messageID) {
                case NetworkMessageID.GAUSS_HOVER:
                    MyPlayer.HandleGaussHover(whoAmI, reader);
                    break;
                case NetworkMessageID.CELESTIAL_HEALING:
                    CelestialHealingBolt.HandleCelestialHealing(whoAmI, -1, -1, reader);
                    break;
                default:
                    break;
            }
        }

        public static ModPacket GetNetMessagePacket(NetworkMessageID id) {
            var packet = Instance.GetPacket();
            packet.Write((int)id);
            return packet;
        }

        public static bool IsCalamityRogueProjectile(Projectile proj) {
            if(!calamityLoaded) return false;
            return (bool)calamity.Call("IsRogue", proj);
        }
    }
}
