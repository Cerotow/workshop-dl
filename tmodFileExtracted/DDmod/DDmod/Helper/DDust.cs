using Terraria;
using Terraria.Graphics.Shaders;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DDmod.Helper
{
    public static class DDust
    {
        /// <summary>
        /// 数量,位置,尺寸,粒子ID,最小速度,最大速度,是否无视重力,大小,透明度,方向(1000是根据粒子方向改变方向)
        /// </summary>
        public static void NewDustSector2(int Quantity, Vector2 position, Vector2 Size,Vector3 velocity, int type, float MinSpeed, float MaxSpeed, bool noGravity = true, float MinScale = 0, float MaxScale = 1, int Alpha = 0, float Rot = 0, Color color = default,object Data =null,int Shader = 0,Player player =null)
        {
            for (int A = 0; A < Quantity; A++)
            {
                Dust dust = Main.dust[NewDust(position, (int)Size.X, (int)Size.Y, type, 0, 0, Alpha, color)];
                dust.noGravity = noGravity;
                dust.scale = Main.rand.NextFloat(MinScale, MaxScale);
                dust.velocity = new Vector2(velocity.X, velocity.Y).RotatedBy(Main.rand.NextFloat(-velocity.Z, velocity.Z)) * Main.rand.NextFloat(MinSpeed, MaxSpeed);
                if (Rot == 1000)
                {
                    dust.rotation = dust.velocity.ToRotation();
                }
                else
                {
                    dust.rotation = Rot;
                }
                dust.customData = Data;
                if(player!=null)
                dust.shader = GameShaders.Armor.GetSecondaryShader(Shader, player);
            }
        }
        /// <summary>
        /// 数量,位置,尺寸,粒子ID,最小速度,最大速度,是否无视重力,大小,透明度,方向(1000是根据粒子方向改变方向)
        /// </summary>
        public static void NewDustSector(int Quantity, Vector2 position, Vector2 Size,Vector3 velocity, int type, float MinSpeed, float MaxSpeed, bool noGravity = true, float MinScale = 0, float MaxScale = 1, int Alpha = 0, float Rot = 0, Color color = default,object Data =null)
        {
            for (int A = 0; A < Quantity; A++)
            {
                Dust dust = Main.dust[NewDust(position, (int)Size.X, (int)Size.Y, type, 0, 0, Alpha, color)];
                dust.noGravity = noGravity;
                dust.scale = Main.rand.NextFloat(MinScale, MaxScale);
                dust.velocity = new Vector2(velocity.X, velocity.Y).RotatedBy(Main.rand.NextFloat(-velocity.Z, velocity.Z)) * Main.rand.NextFloat(MinSpeed, MaxSpeed);
                if (Rot == 1000)
                {
                    dust.rotation = dust.velocity.ToRotation();
                }
                else
                {
                    dust.rotation = Rot;
                }
                dust.customData = Data;
            }
        }
        /// <summary>
        /// 数量,位置,尺寸,粒子ID,最小速度,最大速度,是否无视重力,大小,透明度,方向(1000是根据粒子方向改变方向)
        /// </summary>
        public static void NewDustSector3(int Quantity, Vector2 position, Vector2 Size,Vector3 velocity, int type, float MinSpeed, float MaxSpeed, bool noGravity = true, float MinScale = 0, float MaxScale = 1, int Alpha = 0, float Rot = 0, Color color = default,object Data =null, int Player = 0)
        {
            for (int A = 0; A < Quantity; A++)
            {
                Dust dust = Main.dust[NewDust(position, (int)Size.X, (int)Size.Y, type, 0, 0, Alpha, color)];
                dust.noGravity = noGravity;
                dust.scale = Main.rand.NextFloat(MinScale, MaxScale);
                dust.velocity = new Vector2(velocity.X, velocity.Y).RotatedBy(Main.rand.NextFloat(-velocity.Z, velocity.Z)) * Main.rand.NextFloat(MinSpeed, MaxSpeed);
                if (Rot == 1000)
                {
                    dust.rotation = dust.velocity.ToRotation();
                }
                else
                {
                    dust.rotation = Rot;
                }
                dust.customData = Data;
                GlobalDust.DustPlayerOwner[dust.dustIndex] = Player;
            }
        }
        /// <summary>
        /// 数量,位置,尺寸,粒子ID,最小速度,最大速度,是否无视重力,大小,透明度,方向(1000是根据粒子方向改变方向)
        /// </summary>
        public static void NewDustChange5(int Quantity, Vector2 position, Vector2 Size, int type, float MinSpeed, float MaxSpeed, bool noGravity = true, float MinScale = 0, float MaxScale = 1, int Alpha = 0, float Rot = 0, Color color = default,object Data =null,int Player = 0)
        {
            for (int A = 0; A < Quantity; A++)
            {
                Dust dust = Main.dust[NewDust(position, (int)Size.X, (int)Size.Y, type, 0, 0, Alpha, color)];
                dust.noGravity = noGravity;
                dust.scale = (MinScale >= MaxScale) ? MaxScale : Main.rand.NextFloat(MinScale, MaxScale);
                dust.velocity = Main.rand.NextVector2Unit() * ((MinSpeed>= MaxSpeed)? MaxSpeed:Main.rand.NextFloat(MinSpeed, MaxSpeed));
                if (Rot == 1000)
                {
                    dust.rotation = dust.velocity.ToRotation();
                }
                else
                {
                    dust.rotation = Rot;
                }
                dust.customData = Data;
                GlobalDust.DustPlayerOwner[dust.dustIndex] = Player;
            }
        }
        /// <summary>
        /// 数量,位置,尺寸,粒子ID,最小速度,最大速度,是否无视重力,大小,透明度,方向(1000是根据粒子方向改变方向)
        /// </summary>
        public static void NewDustChange4(int Quantity, Vector2 position, Vector2 Size, int type, float MinSpeed, float MaxSpeed, bool noGravity = true, float MinScale = 0, float MaxScale = 1, int Alpha = 0, float Rot = 0, Color color = default,object Data =null)
        {
            for (int A = 0; A < Quantity; A++)
            {
                Dust dust = Main.dust[NewDust(position, (int)Size.X, (int)Size.Y, type, 0, 0, Alpha, color)];
                dust.noGravity = noGravity;
                dust.scale = (MinScale >= MaxScale) ? MaxScale : Main.rand.NextFloat(MinScale, MaxScale);
                dust.velocity = Main.rand.NextVector2Unit() * ((MinSpeed>= MaxSpeed)? MaxSpeed:Main.rand.NextFloat(MinSpeed, MaxSpeed));
                if (Rot == 1000)
                {
                    dust.rotation = dust.velocity.ToRotation();
                }
                else
                {
                    dust.rotation = Rot;
                }
                dust.customData = Data;
            }
        }
        /// <summary>
        /// 数量,位置,尺寸,粒子ID,最小速度,最大速度,是否无视重力,大小,透明度,方向(1000是根据粒子方向改变方向)
        /// </summary>
        public static void NewDustChange3(int Quantity, Vector2 position, Vector2 Size, int type, float MinSpeed, float MaxSpeed, bool noGravity = true, float MinScale = 0, float MaxScale = 1, int Alpha = 0, float Rot = 0, Color color = default)
        {
            for (int A = 0; A < Quantity; A++)
            {
                Dust dust = Main.dust[NewDust(position, (int)Size.X, (int)Size.Y, type, 0, 0, Alpha, color)];
                dust.noGravity = noGravity;
                dust.scale = (MinScale >= MaxScale) ? MaxScale : Main.rand.NextFloat(MinScale, MaxScale);
                dust.velocity = Main.rand.NextVector2Unit() * ((MinSpeed >= MaxSpeed) ? MaxSpeed : Main.rand.NextFloat(MinSpeed, MaxSpeed));
                if (Rot == 1000)
                {
                    dust.rotation = dust.velocity.ToRotation();
                }
                else
                {
                    dust.rotation = Rot;
                }
            }
        }
        /// <summary>
        /// 数量,位置,尺寸,粒子ID,最小速度,最大速度,是否无视重力,大小,透明度
        /// </summary>
        public static void NewDustChange2(int Quantity, Vector2 position, Vector2 Size, int type, float MinSpeed, float MaxSpeed, bool noGravity = true, float MinScale = 0,float MaxScale = 1, int Alpha = 0,Color color = default)
        {
            for (int A = 0; A < Quantity; A++)
            {
                Dust dust = Main.dust[NewDust(position, (int)Size.X, (int)Size.Y, type, 0, 0, Alpha, color)];
                dust.noGravity = noGravity;
                dust.scale = (MinScale >= MaxScale) ? MaxScale : Main.rand.NextFloat(MinScale, MaxScale);
                dust.velocity = Main.rand.NextVector2Unit() * ((MinSpeed >= MaxSpeed) ? MaxSpeed : Main.rand.NextFloat(MinSpeed, MaxSpeed));
                dust.rotation = dust.velocity.ToRotation();
            }
        }
        /// <summary>
        /// 数量,位置,尺寸,粒子ID,最小速度,最大速度,是否无视重力,大小,透明度
        /// </summary>
        public static void NewDustChange(int Quantity, Vector2 position, Vector2 Size, int type, float MinSpeed, float MaxSpeed, bool noGravity = true, float Scale = 1, int Alpha = 0,Color color = default)
        {
            for (int A = 0; A < Quantity; A++)
            {
                Dust dust = Main.dust[NewDust(position, (int)Size.X, (int)Size.Y, type, 0, 0, Alpha, color)];
                dust.noGravity = noGravity;
                dust.scale = Scale;
                dust.velocity = Main.rand.NextVector2Unit() * ((MinSpeed >= MaxSpeed) ? MaxSpeed : Main.rand.NextFloat(MinSpeed, MaxSpeed));
            }
        }
        /// <summary>
        /// 数量,位置,尺寸,粒子ID,最小速度,最大速度,是否无视重力,大小,透明度
        /// </summary>
        public static void NewDustChangeRound(int Quantity, Vector2 position, float MaxRange, int type, float MinSpeed, float MaxSpeed, bool noGravity = true, float Scale = 1, int Alpha = 0, Color color = default)
        {
            for (int A = 0; A < Quantity; A++)
            {
                Dust dust = Main.dust[NewDust(position+ Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, MaxRange), 1, 1, type, 0, 0, Alpha, color)];
                dust.noGravity = noGravity;
                dust.scale = Scale;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(MinSpeed, MaxSpeed);
            }
        }
        public static void NewDustChangeRound2(int Quantity, Vector2 position, float MaxRange, int type, float MinSpeed, float MaxSpeed, bool noGravity = true, float Scale = 1, int Alpha = 0, Color color = default, object Data = null)
        {
            for (int A = 0; A < Quantity; A++)
            {
                Dust dust = Main.dust[NewDust(position+ Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, MaxRange), 1, 1, type, 0, 0, Alpha, color)];
                dust.noGravity = noGravity;
                dust.scale = Scale;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(MinSpeed, MaxSpeed);
                dust.customData = Data;
            }
        }
        public static void NewDustChangeRound3(int Quantity, Vector2 position, float MaxRange, int type, float MinSpeed, float MaxSpeed, bool noGravity = true, float MinScale = 0.5f, float MaxScale = 1, int Alpha = 0, Color color = default, object Data = null)
        {
            for (int A = 0; A < Quantity; A++)
            {
                Dust dust = Main.dust[NewDust(position+ Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, MaxRange), 1, 1, type, 0, 0, Alpha, color)];
                dust.noGravity = noGravity;
                dust.scale = Main.rand.NextFloat(MinScale, MaxScale);
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(MinSpeed, MaxSpeed);
                dust.customData = Data;
            }
        }
    }
}