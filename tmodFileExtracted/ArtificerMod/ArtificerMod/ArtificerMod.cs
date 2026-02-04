using ArtificerMod.Common;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ArtificerMod
{
	public class ArtificerMod : Mod
	{
        // Credit and thanks to Chem's Vanity - and in turn the Clicker Class mod - for this glowmask code!
        // TODO: Currently unused => Add more glowmasks!
        /*
        public static void BasicInWorldGlowmask(Item item, SpriteBatch spriteBatch, Texture2D glowTexture, Color color, float rotation, float scale)
        {
            spriteBatch.Draw(
                glowTexture,
                new Vector2(
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - glowTexture.Height * 0.5f
                ),
                new Rectangle(0, 0, glowTexture.Width, glowTexture.Height),
                color,
                rotation,
                glowTexture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f);
        }
        */

        // The below code is adapted from Example Mod and handles netcode, namely dodge effects
        internal enum MessageType : byte
        {
            MotherboardDodge,
            HeroShieldDodge,
            XenoShieldBlock
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();

            switch (msgType)
            {
                case MessageType.MotherboardDodge:
                case MessageType.HeroShieldDodge:
                case MessageType.XenoShieldBlock:
                    int dodgeType;
                    switch (msgType)
                    {
                        case MessageType.MotherboardDodge:
                            dodgeType = 0;
                            break;
                        case MessageType.HeroShieldDodge:
                            dodgeType = 1;
                            break;
                        default:
                            dodgeType = 2;
                            break;
                    }
                    ArtificerPlayer.HandleDodgeNetcode(reader, whoAmI, dodgeType);
                    break;
                default:
                    Logger.WarnFormat("ArtificerMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }

        public override object Call(params object[] args)
        {
            // Make sure the call doesn't include anything that could potentially cause exceptions.
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args), "Arguments cannot be null!");
            }

            if (args.Length == 0)
            {
                throw new ArgumentException("Arguments cannot be empty!");
            }

            if (args[0] is string callName)
            {
                switch (callName)
                {
                    case "AddArtificerAbility": // Returns if the player corresponding to the index of the given int has pet perks on
                        
                        if (args[1] is not int accID)
                        {
                            throw new Exception($"Expected an argument of type int for accessory's item.type, but got type {args[1].GetType().Name} instead.");
                        }
                        if (args[2] is not int cooldownTime)
                        {
                            throw new Exception($"Expected an argument of type int for cooldown time, but got type {args[2].GetType().Name} instead.");
                        }
                        if (args[3] is not string abilityDesc)
                        {
                            throw new Exception($"Expected an argument of type string for ability description, but got type {args[3].GetType().Name} instead.");
                        }
                        if (args[4] is not string placeholderTooltip)
                        {
                            throw new Exception($"Expected an argument of type strng for name of placeholder tooltip line, but got type {args[4].GetType().Name} instead.");
                        }
                        if (args[5] is not Func<Player, bool> canActivate)
                        {
                            throw new Exception($"Expected an argument of type Func<Player, bool> for checking ability availability, but got type {args[5].GetType().Name} instead.");
                        }
                        if (args[6] is not Func<Player, bool> useAbility)
                        {
                            throw new Exception($"Expected an argument of type Func<Player, bool> for ability effects, but got type {args[6].GetType().Name} instead.");
                        }

                        CallIntegration.AddNewAbilityData(accID, cooldownTime, abilityDesc, placeholderTooltip, canActivate, useAbility);
                        return true;

                    case "AddAbilityCooldownMod":
                        if (args[1] is not float coolMult)
                        {
                            throw new Exception($"Expected an argument of type float for multipler on cooldown time, but got type {args[1].GetType().Name} instead.");
                        }
                        if(coolMult <= 0)
                        {
                            throw new Exception($"Expected given argument of type float to be greater than 0, but it was not.");
                        }

                        if (args[2] is not Func<Player, bool> applyMult)
                        {
                            throw new Exception($"Expected an argument of type Func<Player, bool> for determining when multiplier is active, but got type {args[2].GetType().Name} instead.");
                        }
                        if (args[3] is not Func<Player, bool> activityChange)
                        {
                            throw new Exception($"Expected an argument of type Func<Player, bool> for checking when multiplier activates/deactivates, but got type {args[3].GetType().Name} instead.");
                        }



                        CallIntegration.AddNewCooldownData(coolMult, applyMult, activityChange);

                        return true;

                    case "GetTotalCooldownMod":
                        if (args[1] is not int playerIndex0)
                        {
                            throw new Exception($"Expected an argument of type int for player index, but got type {args[1].GetType().Name} instead.");
                        }

                        return AbilityAccessories.GetCoolMods(Main.player[playerIndex0]);

                    case "IsAbilityAccEquipped":
                        if (args[1] is not int playerIndex1)
                        {
                            throw new Exception($"Expected an argument of type int for player index, but got type {args[1].GetType().Name} instead.");
                        }

                        if (!Main.player[playerIndex1].TryGetModPlayer(out ArtificerPlayer artificer0))
                        {
                            return false;
                        }

                        return artificer0.abilityAcc;

                    case "IsCooldownAccEquipped":
                        if (args[1] is not int playerIndex2)
                        {
                            throw new Exception($"Expected an argument of type int for player index, but got type {args[1].GetType().Name} instead.");
                        }

                        if (!Main.player[playerIndex2].TryGetModPlayer(out ArtificerPlayer artificer1))
                        {
                            return false;
                        }

                        return artificer1.cooldownAcc;

                    case "IsAbilityCooldownActive":
                        if (args[1] is not int playerIndex3)
                        {
                            throw new Exception($"Expected an argument of type int for player index, but got type {args[1].GetType().Name} instead.");
                        }

                        if (!Main.player[playerIndex3].TryGetModPlayer(out ArtificerPlayer artificer2))
                        {
                            return false;
                        }

                        return artificer2.abilityCooldown;

                    case "CountAsArtificerSetBonus":
                        if (args[1] is not int playerIndex4)
                        {
                            throw new Exception($"Expected an argument of type int for player index, but got type {args[1].GetType().Name} instead.");
                        }

                        if (!Main.player[playerIndex4].TryGetModPlayer(out ArtificerPlayer artificer3))
                        {
                            return false;
                        }
                        artificer3.callCoolBonus = true;

                        return true;

                    case "AddCooldownAccessory":
                        if (args[1] is not int itemID)
                        {
                            throw new Exception($"Expected an argument of type int for accessory's item.type, but got type {args[1].GetType().Name} instead.");
                        }

                        CallIntegration.AddNewCooldownAcc(itemID);

                        return true;

                    default:
                        throw new Exception($"Given call name is invalid! Valid calls include: AddArtificerAbility, AddAbilityCooldownMod, GetTotalCooldownMod, IsAbilityAccEquipped, IsCooldownAccEquipped, IsAbilityCooldownActive, CountAsArtificerSetBonus, AddCooldownAccessory");
                }
            }

            return false;
        }
    }
}