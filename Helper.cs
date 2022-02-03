using Terraria;
using Microsoft.Xna.Framework;
using System;

namespace Consolaria
{
    public static class Helper
    {
        public static bool Easter;
        public static bool Thanksgiving;

        public static void CheckEaster() {
            DateTime now = DateTime.Now;
            int day = now.Day;
            int month = now.Month;

            if(ServerConfig.Instance.SeasonsEnabled) {
                if((day >= 1 && month == 4) || (day <= 1 && month == 5)) Easter = true;
                else Easter = false;
            }
        }

        public static void CheckThanksgiving() {
            DateTime now = DateTime.Now;
            int day = now.Day;
            int month = now.Month;
            if(ServerConfig.Instance.SeasonsEnabled) {
                if((day > 1 && month == 1) || (day <= 1 && month == 12)) Thanksgiving = true;
                else Thanksgiving = false;
            }
        } 

        public static Vector2 RandomPositon(Vector2 pos1, Vector2 pos2) {
            Random _rand = new Random();
            return new Vector2(_rand.Next((int)pos1.X, (int)pos2.X) + 1, _rand.Next((int)pos1.Y, (int)pos2.Y) + 1);
        }

        public static Vector2 VelocityFPTP(Vector2 pos1, Vector2 pos2, float speed) {
            Vector2 move = pos2 - pos1;
            return move * (speed / (float)Math.Sqrt(move.X * move.X + move.Y * move.Y));
        }

        public static float GradtoRad(float Grad) => Grad * (float)Math.PI / 180.0f;
        public static float RadtoGrad(float Rad) => Rad * 180.0f / (float)Math.PI;
        
        public static int GetNearestNPC(Vector2 Point, bool Friendly = false, bool NoBoss = false) {
            float NearestNPCDist = -1;
            int NearestNPC = -1;
            foreach (NPC npc in Main.npc) {
                if (!npc.active) continue;
                if (NoBoss && npc.boss) continue;
                if (!Friendly && (npc.friendly || npc.lifeMax <= 5)) continue;
                if (NearestNPCDist == -1 || npc.Distance(Point) < NearestNPCDist) {
                    NearestNPCDist = npc.Distance(Point);
                    NearestNPC = npc.whoAmI;
                }
            }
            return NearestNPC;
        }

        public static int GetNearestPlayer(Vector2 Point, bool Alive = false) {
            float NearestPlayerDist = -1;
            int NearestPlayer = -1;
            foreach (Player player in Main.player) {
                if (Alive && (!player.active || player.dead)) continue;
                if (NearestPlayerDist == -1 || player.Distance(Point) < NearestPlayerDist)  {
                    NearestPlayerDist = player.Distance(Point);
                    NearestPlayer = player.whoAmI;
                }
            }
            return NearestPlayer;
        }

        public static Vector2 VelocityToPoint(Vector2 A, Vector2 B, float Speed) {
            Vector2 Move = (B - A);
            return Move * (Speed / (float)Math.Sqrt(Move.X * Move.X + Move.Y * Move.Y));
        }

        public static Vector2 RandomPointInArea(Vector2 A, Vector2 B)
            => new Vector2(Main.rand.Next((int)A.X, (int)B.X) + 1, Main.rand.Next((int)A.Y, (int)B.Y) + 1);
       
        public static Vector2 RandomPointInArea(Rectangle Area)
            => new Vector2(Main.rand.Next(Area.X, Area.X + Area.Width), Main.rand.Next(Area.Y, Area.Y + Area.Height));  

        public static float RotateBetween2Points(Vector2 A, Vector2 B) => (float)Math.Atan2(A.Y - B.Y, A.X - B.X);
        
        public static Vector2 CenterPoint(Vector2 A, Vector2 B) => new Vector2((A.X + B.X) / 2.0f, (A.Y + B.Y) / 2.0f);
        
        public static Vector2 PolarPos(Vector2 Point, float Distance, float Angle, int XOffset = 0, int YOffset = 0) {
            Vector2 ReturnedValue = new Vector2();
            ReturnedValue.X = (Distance * (float)Math.Sin((double)Helper.RadtoGrad(Angle)) + Point.X) + XOffset;
            ReturnedValue.Y = (Distance * (float)Math.Cos((double)Helper.RadtoGrad(Angle)) + Point.Y) + YOffset;
            return ReturnedValue;
        }

        public static bool Chance(float chance) => Main.rand.NextFloat() <= chance;

        public static Vector2 SmoothFromTo(Vector2 From, Vector2 To, float Smooth = 60f) => From + ((To - From) / Smooth);     
    }
}