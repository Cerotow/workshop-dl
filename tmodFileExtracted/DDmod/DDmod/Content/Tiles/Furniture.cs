using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Content.Tiles.草;
using DDmod.Players;
using Terraria.GameContent.Drawing;
using Terraria.ObjectData;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Enums;
using DDmod.Content.Items.Tiles.晶凝;
using DDmod.Content.Tiles.晶凝;
using DDmod.Content.Dusts;
using DDmod.Content.Tiles.绿岩;
using DDmod.Content.Items.Series.绿岩;
using Terraria;


namespace DDmod.Content.Tiles
{
    /// <summary> 床 </summary>
    public abstract class DDBed : ModTile
    {
        public const int NextStyleHeight = 38;
        public virtual int Icon => 0; 
        public virtual int Dust => 0;
        public virtual Color Color => new Color(253, 221, 3);
        public virtual Vector3 LightColor => Vector3.Zero;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.CanBeSleptIn[Type] = true;
            TileID.Sets.InteractibleByNPCs[Type] = true;
            TileID.Sets.IsValidSpawnPoint[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);

            DustType = Dust;
            AdjTiles = new int[] { TileID.Beds };

            TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
            TileObjectData.addTile(Type);

            AddMapEntry(Color, Language.GetText("ItemName.Bed"));
            SetDefaults();
        }
        public virtual void SetDefaults()
        {

        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override void ModifySmartInteractCoords(ref int width, ref int height, ref int frameWidth, ref int frameHeight, ref int extraY)
        {

            width = 2;
            height = 2;
        }

        public override void ModifySleepingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
            info.VisualOffset.Y -= 2f;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            int spawnX = (i - (tile.TileFrameX / 18)) + (tile.TileFrameX >= 72 ? 5 : 2);
            int spawnY = j + 2;

            if (tile.TileFrameY % NextStyleHeight != 0)
            {
                spawnY--;
            }

            if (!Player.IsHoveringOverABottomSideOfABed(i, j))
            {
                if (player.IsWithinSnappngRangeToTile(i, j, PlayerSleepingHelper.BedSleepingMaxDistance))
                {
                    player.GamepadEnableGrappleCooldown();
                    player.sleeping.StartSleeping(player, i, j);
                }
            }
            else
            {
                player.FindSpawn();

                if (player.SpawnX == spawnX && player.SpawnY == spawnY)
                {
                    player.RemoveSpawn();
                    Main.NewText(Language.GetTextValue("Game.SpawnPointRemoved"), byte.MaxValue, 240, 20);
                }
                else if (Player.CheckSpawn(spawnX, spawnY))
                {
                    player.ChangeSpawn(spawnX, spawnY);
                    Main.NewText(Language.GetTextValue("Game.SpawnPointSet"), byte.MaxValue, 240, 20);
                }
            }
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;

            if (!Player.IsHoveringOverABottomSideOfABed(i, j))
            {
                if (player.IsWithinSnappngRangeToTile(i, j, PlayerSleepingHelper.BedSleepingMaxDistance))
                {
                    player.noThrow = 2;
                    player.cursorItemIconEnabled = true;
                    player.cursorItemIconID = ItemID.SleepingIcon;
                }
            }
            else
            {
                player.noThrow = 2;
                player.cursorItemIconEnabled = true;
                player.cursorItemIconID = Icon;
            }
        }
    }
    /// <summary> 灯笼 </summary>
    public abstract class DDLantern : ModTile
    {
        public virtual int Dust => 0;
        public virtual Color Color => new Color(253, 221, 3);
        public virtual Vector3 LightColor => Vector3.Zero;
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileWaterDeath[Type] = false;
            TileID.Sets.MultiTileSway[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            SetDefaults();
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            base.AddMapEntry(Color, Language.GetText("MapObject.Lantern"));
            TileID.Sets.DisableSmartCursor[(int)base.Type] = true;
            AdjTiles = new int[]
            {
                42
            };
        }
        public virtual void SetDefaults()
        {

        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2((float)i, (float)j) * 16f, 16, 16, Dust);
            return false;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameX < 18)
            {
                r = LightColor.X;
                g = LightColor.Y;
                b = LightColor.Z;
                return;
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (TileObjectData.IsTopLeft(i,j))
            {
                Main.instance.TilesRenderer.AddSpecialPoint(i, j, TileDrawing.TileCounterType.MultiTileVine);
            }

            return false;
        }
        public override void AdjustMultiTileVineParameters(int i, int j, ref float? overrideWindCycle, ref float windPushPowerX, ref float windPushPowerY, ref bool dontRotateTopTiles, ref float totalWindMultiplier, ref Texture2D glowTexture, ref Color glowColor)
        {

            overrideWindCycle = 1f;
            windPushPowerY = 0;
        }

        public override void GetTileFlameData(int i, int j, ref TileDrawing.TileFlameData tileFlameData)
        {
            /*
            ulong flameSeed = Main.TileFrameSeed ^ (ulong)(((long)i << 32) | (uint)j);

            tileFlameData.flameTexture = flameTexture.Value;
            tileFlameData.flameSeed = flameSeed;

            tileFlameData.flameCount = 7;
            tileFlameData.flameColor = new Color(100, 100, 100, 0);
            tileFlameData.flameRangeXMin = -10;
            tileFlameData.flameRangeXMax = 11;
            tileFlameData.flameRangeYMin = -10;
            tileFlameData.flameRangeYMax = 1;
            tileFlameData.flameRangeMultX = 0.15f;
            tileFlameData.flameRangeMultY = 0.35f;*/
        }
        public override void HitWire(int i, int j)
        {
            TileHelp.Tileswitch(Type, i, j, 1, 2);
        }
    }
    /// <summary> 吊灯 </summary>
    public abstract class DDChandelier : ModTile
    {
        public virtual int Dust => 0;
        public virtual Color Color => new Color(253, 221, 3);
        public virtual Vector3 LightColor => Vector3.Zero;
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileWaterDeath[Type] = false;
            TileID.Sets.MultiTileSway[Type] = true;
            TileID.Sets.IsAMechanism[Type] = true;
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16
            };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Origin = new Point16(1, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
            TileObjectData.newTile.LavaDeath = false;
            SetDefaults();
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            base.AddMapEntry(Color, Language.GetText("MapObject.Chandelier"));
            base.AdjTiles = new int[]
            {
                34
            };
        }
        public virtual void SetDefaults()
        {

        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2((float)i, (float)j) * 16f, 16, 16, Dust);
            return false;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameX < 18)
            {
                r = LightColor.X;
                g = LightColor.Y;
                b = LightColor.Z;
                return;
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (TileObjectData.IsTopLeft(i, j))
            {
                Main.instance.TilesRenderer.AddSpecialPoint(i, j, TileDrawing.TileCounterType.MultiTileVine);
            }
            
            return false;
        }

        public override void AdjustMultiTileVineParameters(int i, int j, ref float? overrideWindCycle, ref float windPushPowerX, ref float windPushPowerY, ref bool dontRotateTopTiles, ref float totalWindMultiplier, ref Texture2D glowTexture, ref Color glowColor)
        {

            overrideWindCycle = 1f;
            windPushPowerY = 0;
        }

        public override void GetTileFlameData(int i, int j, ref TileDrawing.TileFlameData tileFlameData)
        {
            /*
            ulong flameSeed = Main.TileFrameSeed ^ (ulong)(((long)i << 32) | (uint)j);

            tileFlameData.flameTexture = flameTexture.Value;
            tileFlameData.flameSeed = flameSeed;

            tileFlameData.flameCount = 7;
            tileFlameData.flameColor = new Color(100, 100, 100, 0);
            tileFlameData.flameRangeXMin = -10;
            tileFlameData.flameRangeXMax = 11;
            tileFlameData.flameRangeYMin = -10;
            tileFlameData.flameRangeYMax = 1;
            tileFlameData.flameRangeMultX = 0.15f;
            tileFlameData.flameRangeMultY = 0.35f;*/
        }
        public override void HitWire(int i, int j)
        {
            TileHelp.Tileswitch(Type, i, j, 3, 3);
        }
    }
    /// <summary> 钢琴 </summary>
    public abstract class DDPiano : ModTile
    {
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileTable[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;

            DustType = Dust;
            AdjTiles = new int[] { TileID.Tables };

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            SetDefaults();
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

            AddMapEntry(Color, Language.GetText("ItemName.Piano"));
            
        }
        public virtual void SetDefaults()
        {

        }

        public override void NumDust(int x, int y, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
    /// <summary> 工作台 </summary>
    public abstract class DDWorkBench : ModTile
    {
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }

            Main.tileTable[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;

            DustType = Dust;

            AdjTiles = new int[] { TileID.WorkBenches };
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
            TileObjectData.newTile.CoordinateHeights = new[] { 18 };
            SetDefaults();
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

            AddMapEntry(Color, Language.GetText("ItemName.WorkBench"));
        }
        public virtual void SetDefaults()
        {

        }
        public override void NumDust(int x, int y, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

    }
    /// <summary> 蜡烛 </summary>
    public abstract class DDCandle : ModTile
    {
        public virtual int Icon => 0;
        public virtual int Dust => 0;
        public virtual Color Color => new Color(253, 221, 3);
        public virtual Vector3 LightColor => Vector3.Zero;
        public virtual Texture2D flameTexture =>null;
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                20
            };
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.DrawYOffset = -4;
            SetDefaults();
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            AddMapEntry(Color, Language.GetText("ItemName.Candle"));
            TileID.Sets.DisableSmartCursor[(int)base.Type] = true;
            base.AdjTiles = new int[]
            {
                33
            };
        }
        public virtual void SetDefaults()
        {

        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2((float)i, (float)j) * 16f, 16, 16, Dust);
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameX < 18)
            {
                r = LightColor.X;
                g = LightColor.Y;
                b = LightColor.Z;
                return;
            }
        }
        public override void HitWire(int i, int j)
        {
            TileHelp.Tileswitch(Type, i, j, 1, 1);
        }

        public override void MouseOver(int i, int j)
        {
            Player localPlayer = Main.LocalPlayer;
            localPlayer.noThrow = 2;
            localPlayer.cursorItemIconEnabled = true;
            localPlayer.cursorItemIconID = Icon;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (flameTexture == null)
            {
                return;
            }
            SpriteEffects effects = SpriteEffects.None;

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Tile tile = Main.tile[i, j];
            int width = 16;
            int offsetY = 0;
            int height = 16;
            short frameX = tile.TileFrameX;
            short frameY = tile.TileFrameY;

            TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY);

            ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i);

            for (int c = 0; c < 7; c++)
            {
                float shakeX = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
                float shakeY = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;

                spriteBatch.Draw(flameTexture, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + shakeX, j * 16 - (int)Main.screenPosition.Y + offsetY + shakeY) + zero, new Rectangle(frameX, frameY, width, height), new Color(155, 55, 55, 0), 0f, default, 1f, effects, 0f);
            }
        }

        public override bool RightClick(int i, int j)
        {
            TileLoader.HitWire(i, j, Type);
            return true;
        }

    }
    /// <summary> 马桶 </summary>
    public abstract class DDToilet : ModTile
    {

        public const int NextStyleHeight = 40;
        public virtual int Icon => 0;
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.CanBeSatOnForNPCs[Type] = true;
            TileID.Sets.CanBeSatOnForPlayers[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);

            DustType = Dust;
            AdjTiles = new int[] { TileID.Toilets };

            AddMapEntry(Color, Language.GetText("MapObject.Toilet"));

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, 2);
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            SetDefaults();

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);
            SetDefaults2();
            TileObjectData.addTile(Type);
        }
        public virtual void SetDefaults()
        {

        }
        public virtual void SetDefaults2()
        {

        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance);
        }

        public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            info.ExtraInfo.IsAToilet = true;
            info.TargetDirection = -1;

            if (tile.TileFrameX != 0)
            {
                info.TargetDirection = 1;
            }

            info.AnchorTilePosition.X = i;
            info.AnchorTilePosition.Y = j;

            if (tile.TileFrameY % NextStyleHeight == 0)
            {
                info.AnchorTilePosition.Y++;
            }

            if (info.RestingEntity is Player player && player.HasBuff(BuffID.Stinky))
            {
                info.VisualOffset = Main.rand.NextVector2Circular(2, 2);
            }
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;

            if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
            {
                player.GamepadEnableGrappleCooldown();
                player.sitting.SitDown(player, i, j);
            }

            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;

            if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
            {
                return;
            }

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = Icon;

            if (Main.tile[i, j].TileFrameX / 18 < 1)
            {
                player.cursorItemIconReversed = true;
            }
        }

        public override void HitWire(int i, int j)
        {
            Tile tile = Main.tile[i, j];

            int spawnX = i;
            int spawnY = j - (tile.TileFrameY % NextStyleHeight) / 18;

            Wiring.SkipWire(spawnX, spawnY);
            Wiring.SkipWire(spawnX, spawnY + 1);

            if (Wiring.CheckMech(spawnX, spawnY, 60))
            {
                Projectile.NewProjectile(Wiring.GetProjectileSource(spawnX, spawnY), spawnX * 16 + 8, spawnY * 16 + 12, 0f, 0f, ProjectileID.ToiletEffect, 0, 0f, Main.myPlayer);
            }
        }
    }
    /// <summary> 门关 </summary>
    public abstract class DDDoorOff : ModTile
    {

        public virtual int Icon => 0;
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual int DoorID => 0;
        public virtual Vector3 LightColor => Vector3.Zero;
        public virtual int ItemID => Icon;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileFrameImportant[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.NotReallySolid[Type] = true;
            TileID.Sets.DrawsWalls[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
            RegisterItemDrop(ItemID, 0);

            DustType = Dust;
            AdjTiles = new int[] { TileID.ClosedDoor };
            if (DoorID > 0)
            {
                TileID.Sets.OpenDoorID[Type] = DoorID;
            }
            AddMapEntry(Color, Language.GetText("MapObject.Door"));

            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            SetDefaults();
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(0, 1);
            TileObjectData.addAlternate(0);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(0, 2);
            TileObjectData.addAlternate(0);
            TileObjectData.addTile(Type);
        }
        public virtual void SetDefaults()
        {

        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }


        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = Icon;
        }
    }
    /// <summary> 门开 </summary>
    public abstract class DDDoorOn : ModTile
    {
        public virtual int Icon => 0;
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual int DoorID => 0;
        public virtual int ItemID => Icon;
        public virtual Vector3 LightColor => Vector3.Zero;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileFrameImportant[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoSunLight[Type] = true;
            TileID.Sets.HousingWalls[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);

            DustType = Dust;
            AdjTiles = new int[] { TileID.OpenDoor };
            TileID.Sets.CloseDoorID[Type] = DoorID;
            RegisterItemDrop(ItemID, 0);

            AddMapEntry(Color, Language.GetText("MapObject.Door"));

            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(0, 1);
            TileObjectData.addAlternate(0);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(0, 2);
            TileObjectData.addAlternate(0);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(1, 0);
            TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
            TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.addAlternate(1);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(1, 1);
            TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
            TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.addAlternate(1);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(1, 2);
            TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
            TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.addAlternate(1);
            SetDefaults();
            TileObjectData.addTile(Type);
        }
        public virtual void SetDefaults()
        {

        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }


        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = Icon;
        }
    }
    /// <summary> 平台 </summary>
    public abstract class DDPlatforms : ModTile
    {
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.Platforms[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
            AddMapEntry(Color);

            DustType = Dust;
            AdjTiles = new int[] { TileID.Platforms };

            TileObjectData.newTile.CoordinateHeights = new[] { 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleMultiplier = 27;
            TileObjectData.newTile.StyleWrapLimit = 27;
            TileObjectData.newTile.UsesCustomCanPlace = false;
            TileObjectData.newTile.LavaDeath = true;

            SetDefaults();
            TileObjectData.addTile(Type);
        }
        public virtual void SetDefaults()
        {

        }
        public override void PostSetDefaults() => Main.tileNoSunLight[Type] = false;

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
    /// <summary> 沙发 </summary>
    public abstract class DDSofa : ModTile
    {
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileTable[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;

            DustType = Dust;
            AdjTiles = new int[] { TileID.Chairs };

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            SetDefaults();
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);

            AddMapEntry(Color, Language.GetText("ItemName.Sofa"));
        }
        public virtual void SetDefaults()
        {

        }

        public override void NumDust(int x, int y, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

    }
    /// <summary> 书架 </summary>
    public abstract class DDBookcase : ModTile
    {
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileTable[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;

            DustType = Dust;
            AdjTiles = new int[] { TileID.Bookcases };

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.StyleHorizontal = true;
            SetDefaults();
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

            AddMapEntry(Color, Language.GetText("ItemName.Bookcase"));
        }
        public virtual void SetDefaults()
        {

        }
        public override void NumDust(int x, int y, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

    }
    /// <summary> 梳妆台 </summary>
    public abstract class DDDresser : ModTile
    {

        public virtual int Icon => 0;
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileContainer[Type] = true;
            Main.tileWaterDeath[Type] = false;
            Main.tileLavaDeath[Type] = false;
            TileID.Sets.BasicDresser[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16
            };
            TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(new Func<int, int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
            TileObjectData.newTile.AnchorInvalidTiles = new int[]
            {
                127
            };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            SetDefaults();
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

            AddMapEntry(Color, Language.GetText("ItemName.Dresser"));
            TileID.Sets.DisableSmartCursor[(int)base.Type] = true;
            base.AdjTiles = new int[]
            {
                88
            };
        }
        public virtual void SetDefaults()
        {

        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2((float)i, (float)j) * 16f, 16, 16, Dust, 0f, 0f, 1, new Color(100, 130, 150), 1f);
            return false;
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameY != 0)
            {
                Main.playerInventory = false;
                player.chest = -1;
                Recipe.FindRecipes(false);
                Main.interactedDresserTopLeftX = Player.tileTargetX;
                Main.interactedDresserTopLeftY = Player.tileTargetY;
                Main.OpenClothesWindow();
                return true;
            }
            Main.CancelClothesWindow(true);
            int left = (int)(Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameX / 18);
            left %= 3;
            left = Player.tileTargetX - left;
            int top = Player.tileTargetY - (int)(Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameY / 18);
            if (player.sign > -1)
            {
                SoundEngine.PlaySound(SoundID.MenuClose, default(Vector2?));
                player.sign = -1;
                Main.editSign = false;
                Main.npcChatText = string.Empty;
            }
            if (Main.editChest)
            {
                SoundEngine.PlaySound(SoundID.MenuTick, default(Vector2?));
                Main.editChest = false;
                Main.npcChatText = string.Empty;
            }
            if (player.editedChestName)
            {
                NetMessage.SendData(33, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
                player.editedChestName = false;
            }
            if (Main.netMode == 1)
            {
                if (left == player.chestX && top == player.chestY && player.chest != -1)
                {
                    player.chest = -1;
                    Recipe.FindRecipes(false);
                    SoundEngine.PlaySound(SoundID.MenuClose, default(Vector2?));
                }
                else
                {
                    NetMessage.SendData(31, -1, -1, null, left, (float)top, 0f, 0f, 0, 0, 0);
                    Main.stackSplit = 600;
                }
                return true;
            }
            player.piggyBankProjTracker.Clear();
            player.voidLensChest.Clear();
            int num213 = Chest.FindChest(left, top);
            if (num213 != -1)
            {
                Main.stackSplit = 600;
                if (num213 == player.chest)
                {
                    player.chest = -1;
                    Recipe.FindRecipes(false);
                    SoundEngine.PlaySound(SoundID.MenuClose, default(Vector2?));
                }
                else if (num213 != player.chest && player.chest == -1)
                {
                    player.chest = num213;
                    Main.playerInventory = true;
                    Main.recBigList = false;
                    SoundEngine.PlaySound(SoundID.MenuOpen, default(Vector2?));
                    player.chestX = left;
                    player.chestY = top;
                }
                else
                {
                    player.chest = num213;
                    Main.playerInventory = true;
                    Main.recBigList = false;
                    SoundEngine.PlaySound(SoundID.MenuTick, default(Vector2?));
                    player.chestX = left;
                    player.chestY = top;
                }
                Recipe.FindRecipes(false);
                return true;
            }
            return false;
        }

        public void MouseOverNearAndFarSharedLogic(Player player, int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            left -= tile.TileFrameX % 54 / 18;
            if (tile.TileFrameY % 36 != 0)
            {
                top--;
            }
            int chestIndex = Chest.FindChest(left, top);
            player.cursorItemIconID = -1;
            if (chestIndex < 0)
            {
                player.cursorItemIconText = Language.GetTextValue("LegacyDresserType.0");
            }
            else
            {
                string defaultName = TileLoader.DefaultContainerName(tile.TileType, tile.TileFrameX, tile.TileFrameY);
                if (Main.chest[chestIndex].name != "")
                {
                    player.cursorItemIconText = Main.chest[chestIndex].name;
                }
                else
                {
                    player.cursorItemIconText = defaultName;
                }
                if (player.cursorItemIconText == defaultName)
                {
                    player.cursorItemIconID = Icon;
                    player.cursorItemIconText = "";
                }
            }
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }
        public override void MouseOverFar(int i, int j)
        {
            Player player = Main.LocalPlayer;
            MouseOverNearAndFarSharedLogic(player, i, j);
            if (player.cursorItemIconText == "")
            {
                player.cursorItemIconEnabled = false;
                player.cursorItemIconID = 0;
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            MouseOverNearAndFarSharedLogic(player, i, j);
            if (Main.tile[i, j].TileFrameY > 0)
            {
                player.cursorItemIconID = ItemID.FamiliarShirt;
                player.cursorItemIconText = "";
            }
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Chest.DestroyChest(i, j);
        }
    }
    /// <summary> 水槽 </summary>
    public abstract class DDSink : ModTile
    {
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            TileID.Sets.CountsAsWaterSource[Type] = true;
            TileID.Sets.CountsAsHoneySource[Type] = true;
            TileID.Sets.CountsAsLavaSource[Type] = true;

            Main.tileSolid[Type] = false;
            Main.tileLavaDeath[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            SetDefaults();
            TileObjectData.addTile(Type);

            AddMapEntry(Color, Language.GetText("MapObject.Sink"));

            DustType = Dust;
            AdjTiles = new int[] { Type };
        }
        public virtual void SetDefaults()
        {

        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
    /// <summary> 宝箱 </summary>
    public abstract class DDChest : ModTile
    {

        public virtual int Icon => 0;
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;
        public virtual LocalizedText name => null;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileSpelunker[Type] = true;
            Main.tileContainer[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1200;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileOreFinderPriority[Type] = 500;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.BasicChest[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            DustType = Dust;
            AdjTiles = new int[] { TileID.Containers };

            AddMapEntry(Color, name==null? Language.GetText("ItemName.Chest"): name, MapChestName);

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
            TileObjectData.newTile.AnchorInvalidTiles = new int[] { TileID.MagicalIceBlock };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            SetDefaults();
            TileObjectData.addTile(Type);
        }
        public virtual void SetDefaults()
        {

        }

        public override ushort GetMapOption(int i, int j)
        {
            return (ushort)(Main.tile[i, j].TileFrameX / 36);
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override bool IsLockedChest(int i, int j)
        {
            return Main.tile[i, j].TileFrameX / 36 == 1;
        }

        public override bool UnlockChest(int i, int j, ref short frameXAdjustment, ref int dustType, ref bool manual)
        {

            DustType = dustType;
            return true;
        }
        public override LocalizedText DefaultContainerName(int frameX, int frameY)
        {
            return name == null ? Language.GetText("ItemName.Chest") : name;
        }
        public static string MapChestName(string name, int i, int j)
        {
            int left = i;
            int top = j;
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX % 36 != 0)
            {
                left--;
            }

            if (tile.TileFrameY != 0)
            {
                top--;
            }

            int chest = Chest.FindChest(left, top);
            if (chest < 0)
            {
                return Language.GetTextValue("LegacyChestType.0");
            }

            if (Main.chest[chest].name == "" || Main.chest[chest].name == name)
            {
                return name;
            }

            return name + ": " + Main.chest[chest].name;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Chest.DestroyChest(i, j);
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            Main.mouseRightRelease = false;
            int left = i;
            int top = j;
            if (tile.TileFrameX % 36 != 0)
            {
                left--;
            }

            if (tile.TileFrameY != 0)
            {
                top--;
            }

            player.CloseSign();
            player.SetTalkNPC(-1);
            Main.npcChatCornerItem = 0;
            Main.npcChatText = "";
            if (Main.editChest)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Main.editChest = false;
                Main.npcChatText = string.Empty;
            }

            if (player.editedChestName)
            {
                NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f);
                player.editedChestName = false;
            }

            bool isLocked = Chest.IsLocked(left, top);
            if (Main.netMode == NetmodeID.MultiplayerClient && !isLocked)
            {
                if (left == player.chestX && top == player.chestY && player.chest >= 0)
                {
                    player.chest = -1;
                    Recipe.FindRecipes();
                    SoundEngine.PlaySound(SoundID.MenuClose);
                }
                else
                {
                    NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, top);
                    Main.stackSplit = 600;
                }
            }
            else
            {
                if (isLocked)
                {
                    int key = ModContent.ItemType<晶凝箱>();
                    if (player.ConsumeItem(key) && Chest.Unlock(left, top))
                    {
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendData(MessageID.LockAndUnlock, -1, -1, null, player.whoAmI, 1f, left, top);
                        }
                    }
                }
                else
                {
                    int chest = Chest.FindChest(left, top);
                    if (chest >= 0)
                    {
                        Main.stackSplit = 600;
                        if (chest == player.chest)
                        {
                            player.chest = -1;
                            SoundEngine.PlaySound(SoundID.MenuClose);
                        }
                        else
                        {
                            SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
                            player.OpenChest(left, top, chest);
                        }

                        Recipe.FindRecipes();
                    }
                }
            }

            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            if (tile.TileFrameX % 36 != 0)
            {
                left--;
            }

            if (tile.TileFrameY != 0)
            {
                top--;
            }

            int chest = Chest.FindChest(left, top);
            player.cursorItemIconID = -1;
            if (chest < 0)
            {
                player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
            }
            else
            {
                string defaultName = TileLoader.DefaultContainerName(tile.TileType, tile.TileFrameX, tile.TileFrameY)/* tModPorter Note: new method takes in FrameX and FrameY */;
                player.cursorItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : defaultName;
                if (player.cursorItemIconText == defaultName)
                {
                    player.cursorItemIconID = Icon;

                    player.cursorItemIconText = "";
                }
            }

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }

        public override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
            Player player = Main.LocalPlayer;
            if (player.cursorItemIconText == "")
            {
                player.cursorItemIconEnabled = false;
                player.cursorItemIconID = 0;
            }
        }
    }
    /// <summary> 落地灯 </summary>
    public abstract class DDFloorLamp : ModTile
    {
        public virtual Color Color => new Color(253, 221, 3);
        public virtual Vector3 LightColor => Vector3.Zero;
        public virtual Texture2D flameTexture => null;
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
            TileObjectData.newTile.WaterDeath = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            SetDefaults();
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

            AddMapEntry(Color, Language.GetText("MapObject.FloorLamp"));

        }
        public virtual void SetDefaults()
        {

        }

        public override void HitWire(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int topY = j - tile.TileFrameY / 18 % 3;
            short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);

            Main.tile[i, topY].TileFrameX += frameAdjustment;
            Main.tile[i, topY + 1].TileFrameX += frameAdjustment;
            Main.tile[i, topY + 2].TileFrameX += frameAdjustment;

            Wiring.SkipWire(i, topY);
            Wiring.SkipWire(i, topY + 1);
            Wiring.SkipWire(i, topY + 2);

            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendTileSquare(-1, i, topY + 1, 3, TileChangeType.None);
            }
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameX < 18)
            {
                r = LightColor.X;
                g = LightColor.Y;
                b = LightColor.Z;
                return;
            }
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (Main.gamePaused || !Main.instance.IsActive || Lighting.UpdateEveryFrame && !Main.rand.NextBool(4))
            {
                return;
            }

            Tile tile = Main.tile[i, j];

            short frameX = tile.TileFrameX;
            short frameY = tile.TileFrameY;

            if (frameX != 0 || !Main.rand.NextBool(40))
            {
                return;
            }

            int style = frameY / 54;

            if (frameY / 18 % 3 == 0)
            {
                int dustChoice = -1;

                if (style == 0)
                {
                    dustChoice = 21;
                }

                if (dustChoice != -1)
                {
                    var dust = NewDustDirect(new Vector2(i * 16 + 4, j * 16 + 2), 4, 4, dustChoice, 0f, 0f, 100, default, 1f);

                    if (!Main.rand.NextBool(3))
                    {
                        dust.noGravity = true;
                    }

                    dust.velocity *= 0.3f;
                    dust.velocity.Y = dust.velocity.Y - 1.5f;
                }
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (flameTexture == null)
            {
                return;
            }
            SpriteEffects effects = SpriteEffects.None;

            if (i % 2 == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Tile tile = Main.tile[i, j];
            int width = 16;
            int offsetY = 0;
            int height = 16;
            short frameX = tile.TileFrameX;
            short frameY = tile.TileFrameY;

            TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY);

            ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i);

            for (int c = 0; c < 7; c++)
            {
                float shakeX = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
                float shakeY = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;

                spriteBatch.Draw(flameTexture, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + shakeX, j * 16 - (int)Main.screenPosition.Y + offsetY + shakeY) + zero, new Rectangle(frameX, frameY, width, height), new Color(155, 55, 55, 0), 0f, default, 1f, effects, 0f);
            }
        }
    }
    /// <summary> 椅子 </summary>
    public abstract class DDChair : ModTile
    {

        public const int NextStyleHeight = 40;

        public virtual int Icon => 0;
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.CanBeSatOnForNPCs[Type] = true;
            TileID.Sets.CanBeSatOnForPlayers[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);

            DustType = Dust;
            AdjTiles = new int[] { TileID.Chairs };

            AddMapEntry(Color, Language.GetText("MapObject.Chair"));

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, 2);
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleHorizontal = true;

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);
            SetDefaults();
            TileObjectData.addTile(Type);
        }
        public virtual void SetDefaults()
        {

        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance); // Avoid being able to trigger it from long range
        }

        public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
        {
            Tile tile = Framing.GetTileSafely(i, j);


            info.TargetDirection = -1;
            if (tile.TileFrameX != 0)
            {
                info.TargetDirection = 1;
            }

            info.AnchorTilePosition.X = i;
            info.AnchorTilePosition.Y = j;

            if (tile.TileFrameY % NextStyleHeight == 0)
            {
                info.AnchorTilePosition.Y++;
            }
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;

            if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
            {
                player.GamepadEnableGrappleCooldown();
                player.sitting.SitDown(player, i, j);
            }

            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;

            if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
            {
                return;
            }

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = Icon;

            if (Main.tile[i, j].TileFrameX / 18 < 1)
            {
                player.cursorItemIconReversed = true;
            }
        }
    }
    /// <summary> 浴缸 </summary>
    public abstract class DDBathtub : ModTile
    {

        public const int NextStyleHeight = 38;
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileTable[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

            DustType = Dust;
            AdjTiles = new int[] { TileID.Tables };

            TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
            SetDefaults();
            TileObjectData.addTile(Type);

            AddMapEntry(Color, Language.GetText("ItemName.Bathtub"));
        }
        public virtual void SetDefaults()
        {

        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }

    }
    /// <summary> 落地钟 </summary>
    public abstract class DDGrandfatherClock : ModTile
    {
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if(LightColor!= Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.Clock[Type] = true;

            DustType = Dust;
            AdjTiles = new int[] { TileID.GrandfatherClocks };

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };
            SetDefaults();
            TileObjectData.addTile(Type);

            AddMapEntry(Color, Language.GetText("ItemName.GrandfatherClock"));
        }
        public virtual void SetDefaults()
        {

        }
        public override bool RightClick(int x, int y)
        {
            string text = Language.GetTextValue("GameUI.TimeAtMorning");
            double time = Main.time;
            if (!Main.dayTime)
            {
                time += 54000.0;
            }

            time = (time / 86400.0) * 24.0;
            time = time - 7.5 - 12.0;
            if (time < 0.0)
            {
                time += 24.0;
            }

            if (time >= 12.0)
            {
                text = Language.GetTextValue("GameUI.TimePastMorning");
            }

            int intTime = (int)time;
            double deltaTime = time - intTime;
            deltaTime = (int)(deltaTime * 60.0);
            string text2 = string.Concat(deltaTime);
            if (deltaTime < 10.0)
            {
                text2 = "0" + text2;
            }

            if (intTime > 12)
            {
                intTime -= 12;
            }

            if (intTime == 0)
            {
                intTime = 12;
            }

            Main.NewText(Language.GetTextValue("CLI.Time", intTime+":"+text2)+" "+text, 255, 240, 20);
            return true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
    /// <summary> 烛台 </summary>
    public abstract class DDCandelabra : ModTile
    {
        public virtual int Dust => 0;
        public virtual Color Color => new Color(253, 221, 3);
        public virtual Vector3 LightColor => Vector3.Zero;
        public virtual Texture2D flameTexture => null;
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.LavaDeath = false;
            SetDefaults();
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            AddMapEntry(Color, Language.GetText("ItemName.Candelabra"));
            TileID.Sets.DisableSmartCursor[(int)base.Type] = true;
            base.AdjTiles = new int[]
            {
                100
            };
        }
        public virtual void SetDefaults()
        {

        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2((float)i, (float)j) * 16f, 16, 16, Dust, 0f, 0f, 1, new Color(54, 69, 72), 1f);
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].TileFrameX < 18)
            {
                r = LightColor.X;
                g = LightColor.Y;
                b = LightColor.Z;
                return;
            }
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if(flameTexture==null)
            {
                return;
            }
            SpriteEffects effects = SpriteEffects.None;

            if (i % 2 == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Tile tile = Main.tile[i, j];
            int width = 16;
            int offsetY = 0;
            int height = 16;
            short frameX = tile.TileFrameX;
            short frameY = tile.TileFrameY;

            TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY);

            ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i);

            for (int c = 0; c < 7; c++)
            {
                float shakeX = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
                float shakeY = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;

                spriteBatch.Draw(flameTexture, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + shakeX, j * 16 - (int)Main.screenPosition.Y + offsetY + shakeY) + zero, new Rectangle(frameX, frameY, width, height), new Color(155, 55, 55, 0), 0f, default, 1f, effects, 0f);
            }
        }
        public override void HitWire(int i, int j)
        {
            TileHelp.Tileswitch(Type, i, j, 2, 2);
        }
    }
    /// <summary> 桌子 </summary>
    public abstract class DDTable : ModTile
    {
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileTable[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;

            DustType = Dust;
            AdjTiles = new int[] { TileID.Tables };

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            SetDefaults();
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

            AddMapEntry(Color, Language.GetText("MapObject.Table"));
        }
        public virtual void SetDefaults()
        {

        }

        public override void NumDust(int x, int y, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

    }
    /// <summary> 自定义家具 </summary>
    public abstract class DDCustomize : ModTile
    {
        public virtual Vector3 LightColor => Vector3.Zero;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = LightColor.X;
            g = LightColor.Y;
            b = LightColor.Z;
        }
        public override void NumDust(int x, int y, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

    }
    /// <summary> 物块 </summary>
    public abstract class DDBlocks : ModTile
    {
        /// <summary>
        /// 0石头,1草,2土
        /// </summary>
        public virtual int Sound => 0;
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;
        
        public virtual void SetDefaults()
        {

        }
        public virtual void MapEntry()
        {
            this.AddMapEntry(Color);
        }
        public override void SetStaticDefaults()
        {
            if(LightColor!= Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSpelunker[Type] = false;
            DustType = Dust;
            if (Sound == 0)
            {
                HitSound = SoundID.Tink;
            }
            if (Sound == 1)
            {
                HitSound = SoundID.Grass;
            }
            if (Sound == 2)
            {
                HitSound = SoundID.Dig;
            }

            MapEntry();

            SetDefaults();
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Vector3 vector = LightColor;
            if (Main.tile[i, j].TileColor != 0)
            {
                vector = WorldGen.paintColor(Main.tile[i, j].TileColor).ToVector3() * 1.5f;
            }
            r = vector.X;
            g = vector.Y;
            b = vector.Z;
        }


    }
    /// <summary> 简化物块 </summary>
    public abstract class DDSimplifyBlocks : ModTile
    {
        /// <summary>
        /// 0石头,1草,2土
        /// </summary>
        public virtual int Sound => 0;
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;
        public virtual int Glow => -1;

        public virtual void SetDefaults()
        {

        }
        public virtual void MapEntry()
        {
            this.AddMapEntry(Color);
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSpelunker[Type] = true;
            DustType = Dust;
            if (Sound == 0)
            {
                HitSound = SoundID.Tink;
            }
            if (Sound == 1)
            {
                HitSound = SoundID.Grass;
            }
            if (Sound == 2)
            {
                HitSound = SoundID.Dig;
            }

            MapEntry();
            SetDefaults();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Vector3 vector = LightColor;
            if (Main.tile[i, j].TileColor != 0)
            {
                vector = WorldGen.paintColor(Main.tile[i, j].TileColor).ToVector3() * 1.5f;
            }
            r = vector.X;
            g = vector.Y;
            b = vector.Z;
        }
        public override void Load()
        {
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color color = Lighting.GetColor(new Point(i, j));
            if (Glow >= 0)
            {
                TileHelp.TileDraw(i, j, spriteBatch, new Color(255, 255, 255, 255), null, Glow);
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if (Main.tile[i, j].TileColor != 0)
            {
                //color = WorldGen.paintColor(Main.tile[i, j].TileColor);
            }
            return TileHelp.TileDraw(i, j, spriteBatch, Lighting.GetColor(new Point(i, j), color));
        }
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            return TileHelp.TileFrame(i, j, false); ;
        }
        public override bool KillSound(int i, int j, bool fail)
        {
            return base.KillSound(i, j, fail);
        }
    }

    /// <summary> 物块副本(用于RT2d贴图引用) </summary>
    public abstract class DDBlocksCopy : ModTile
    {

    }
    /// <summary> 水晶 </summary>
    public abstract class DDCrystals : ModTile
    {
        /// <summary>
        /// 0石头,1草,2土
        /// </summary>
        public virtual int Sound => 0;
        public virtual int Dust => 0;
        public virtual Color Color => new Color(0, 0, 0, 0);
        public virtual Vector3 LightColor => Vector3.Zero;
        public virtual bool Switch => false;
        public virtual bool SwitchGround => false;
        public virtual int Glow => -1;
        public int Drop = -1;
        public virtual int Style => 9;
        /// <summary>
        /// 你觉得你能杀死我?
        /// </summary>
        public virtual bool WillNoKill => false;

        public virtual void SetDefaults()
        {

        }
        public virtual void MapEntry()
        {
            this.AddMapEntry(Color);
        }
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            Main.tileSolid[(int)Type] = false;
            Main.tileFrameImportant[Type] = true;
            DustType = Dust;
            if (Sound == 0)
            {
                HitSound = SoundID.Tink;
            }
            if (Sound == 1)
            {
                HitSound = SoundID.Grass;
            }
            if (Sound == 2)
            {
                HitSound = SoundID.Dig;
            }
            if (Switch)
            {
                TileObjectData.newTile.UsesCustomCanPlace = true;
                TileObjectData.newTile.StyleHorizontal = true;
                TileObjectData.newTile.CoordinateWidth = 16;
                TileObjectData.newTile.CoordinatePadding = 2;
                TileObjectData.newTile.FlattenAnchors = true;
                TileObjectData.addBaseTile(out TileObjectData.StyleSwitch);
                TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
                TileObjectData.newTile.Style = 0;
                TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
                if(WillNoKill)
                {
                    TileObjectData.newTile.AnchorRight = new AnchorData();
                }
                TileObjectData.newTile.AnchorAlternateTiles = new int[7] {
            124,
            561,
            574,
            575,
            576,
            577,
            578
        };


                TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleSwitch);
                TileObjectData.newAlternate.Style = 1;
                TileObjectData.newAlternate.AnchorWall = true;
                TileObjectData.addAlternate(1);

                TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleSwitch);
                TileObjectData.newAlternate.Style = 2;
                TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0); 
                if (WillNoKill)
                {
                    TileObjectData.newAlternate.AnchorLeft = new AnchorData();
                }
                TileObjectData.newAlternate.AnchorAlternateTiles = new int[7] {
            124,
            561,
            574,
            575,
            576,
            577,
            578
        };

                TileObjectData.newAlternate.DrawXOffset = -2;
                TileObjectData.addAlternate(2);

                if (SwitchGround)
                {
                    TileObjectData.newAlternate.DrawYOffset = 2;
                    TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleSwitch);
                    TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
                    if (WillNoKill)
                    {
                        TileObjectData.newAlternate.AnchorBottom = new AnchorData();
                    }
                    TileObjectData.addAlternate(3);

                }
                TileObjectData.newTile.DrawXOffset = 2;
                TileObjectData.addTile(Type);
            }
            MapEntry();
            SetDefaults();
        }
        public override bool KillSound(int i, int j, bool fail)
        {
            if (!fail)
            {
                PlaySound(SoundID.Shatter, new Vector2(i, j) * 16);
            }
            return base.KillSound(i, j, fail);
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (LightColor != Vector3.Zero)
            {
                Vector3 vector = LightColor;
                if (Main.tile[i, j].TileColor != 0)
                {
                    vector = WorldGen.paintColor(Main.tile[i, j].TileColor).ToVector3() * 0.5f;
                }
                r = vector.X;
                g = vector.Y;
                b = vector.Z;
            }
        }


        public Asset<Texture2D> TileTexture;


        public override void Load()
        {
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            //TileHelp.TileDraw(i, j, spriteBatch, WorldGen.paintColor(Main.tile[i, j].TileColor));
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Main.tile[i, j].TileColor != 0)
            {
                TileHelp.TileDrawCrystals(i, j, spriteBatch, WorldGen.paintColor(Main.tile[i, j].TileColor));
            }
            else
            {
                TileHelp.TileDrawCrystals(i, j, spriteBatch, Color.White);
            }

            return false;
        }
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            if (Switch)
            {
                    Tile tile = Main.tile[i, j];
                    Tile tile4 = Main.tile[i - 1, j];
                    Tile tile5 = Main.tile[i + 1, j];
                    
                    int num25 = -1;
                    int num26 = -1;

                    if (tile4 != null && tile4.HasTile && !tile4.RightSlope && !tile4.IsHalfBlock)
                        num25 = tile4.TileType;

                    if (tile5 != null && tile5.HasTile && !tile5.LeftSlope && !tile5.IsHalfBlock)
                        num26 = tile5.TileType;

                    if (num25 >= 0 && ((Main.tileSolid[num25]&& TileObjectData.GetTileData(num25, 0, 0) == null) || TileID.Sets.IsATreeTrunk[num25]) && !Main.tileSolidTop[num25])
                        tile.TileFrameX = 36;
                    else if (num26 >= 0 && ((Main.tileSolid[num26] && TileObjectData.GetTileData(num26, 0, 0) == null) || TileID.Sets.IsATreeTrunk[num26]) && !Main.tileSolidTop[num26])
                        tile.TileFrameX = 0;
                    else
                        tile.TileFrameX = 18;
                if (WillNoKill)
                {
                    return false;
                }
                return true;
            }
            {
                Tile tile = Main.tile[i, j];
                Tile tile2 = Main.tile[i, j - 1];
                Tile tile3 = Main.tile[i, j + 1];
                Tile tile4 = Main.tile[i - 1, j];
                Tile tile5 = Main.tile[i + 1, j];
                int num23 = -1;
                int num24 = -1;
                int num25 = -1;
                int num26 = -1;
                if (tile2 != null && tile2.HasTile && !tile2.BottomSlope)
                    num24 = tile2.TileType;

                if (tile3 != null && tile3.HasTile && !tile3.IsHalfBlock && !tile3.TopSlope)
                    num23 = tile3.TileType;

                if (tile4 != null && tile4.HasTile && !tile4.RightSlope && !tile4.IsHalfBlock)
                    num25 = tile4.TileType;

                if (tile5 != null && tile5.HasTile && !tile5.LeftSlope && !tile5.IsHalfBlock)
                    num26 = tile5.TileType;

                if (num23 >= 0 && Main.tileSolid[num23] && !Main.tileSolidTop[num23])
                    tile.TileFrameY = 0;
                else if (num25 >= 0 && Main.tileSolid[num25] && !Main.tileSolidTop[num25])
                    tile.TileFrameY = 54;
                else if (num26 >= 0 && Main.tileSolid[num26] && !Main.tileSolidTop[num26])
                    tile.TileFrameY = 36;
                else if (num24 >= 0 && Main.tileSolid[num24] && !Main.tileSolidTop[num24])
                    tile.TileFrameY = 18;
                else
                    WorldGen.KillTile(i, j);
                if (resetFrame)
                {
                    tile.TileFrameX = (short)(WorldGen.genRand.Next(Style) * 18);
                }
            }
            return false;
        }
        public override void PostTileFrame(int i, int j, int up, int down, int left, int right, int upLeft, int upRight, int downLeft, int downRight)
        {
        }
        public override bool Slope(int i, int j)
        {
            return false;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return true;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Tile tile = Main.tile[i, j];
        }

        public override bool CanPlace(int i, int j)
        {
            if (Switch)
            {
                return true;
            }
            Tile tile = Main.tile[i, j];
            Tile tile2 = Main.tile[i, j - 1];
            Tile tile3 = Main.tile[i, j + 1];
            Tile tile4 = Main.tile[i - 1, j];
            Tile tile5 = Main.tile[i + 1, j];
            int num23 = -1;
            int num24 = -1;
            int num25 = -1;
            int num26 = -1;
            if (tile2 != null && tile2.HasTile && !tile2.BottomSlope)
                num24 = tile2.TileType;

            if (tile3 != null && tile3.HasTile && !tile3.IsHalfBlock && !tile3.TopSlope)
                num23 = tile3.TileType;

            if (tile4 != null && tile4.HasTile && !tile4.RightSlope && !tile4.IsHalfBlock)
                num25 = tile4.TileType;

            if (tile5 != null && tile5.HasTile && !tile5.LeftSlope && !tile5.IsHalfBlock)
                num26 = tile5.TileType;
            if ((num24 >= 0 && Main.tileSolid[num24] && !Main.tileSolidTop[num24]) ||
                            (num23 >= 0 && Main.tileSolid[num23] && !Main.tileSolidTop[num23]) ||
                            (num25 >= 0 && Main.tileSolid[num25] && !Main.tileSolidTop[num25]) ||
                            (num26 >= 0 && Main.tileSolid[num26] && !Main.tileSolidTop[num26]))
                return true;
            else
                return false;
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            if (Switch)
            {
                return;
            }
            if (Style > 1)
            {
                Main.tile[i, j].TileFrameX = (short)(WorldGen.genRand.Next(Style) * 18);
            }
        }
    }
}
