using Luna.Biz.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Luna.Biz.Locations
{
    class LocationHelper
    {
        private const int RectSize = 100;
        private const int MinDistanceBetweenLocations = 20;
        private const int MaxConnectionCount = 3;
        private static readonly int[] XOffset = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };
        private static readonly int[] YOffset = new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };

        private static Vector2 ScenePosToRectPos(Vector2 pos)
        {
            pos /= RectSize;
            pos.X = MathF.Floor(pos.X);
            pos.Y = MathF.Floor(pos.Y);
            return pos;
        }

        private static bool IsSceneInRect(Vector2 scenePos, Vector2 rectPos)
        {
            return scenePos.X > rectPos.X && scenePos.Y > rectPos.Y && scenePos.X <= rectPos.X + RectSize && scenePos.Y <= rectPos.Y + RectSize;
        }

        private static Vector2 ApplyOffsetToRectPos(Vector2 pos, int offsetIndex)
        {
            return new Vector2(pos.X + XOffset[offsetIndex] * RectSize, pos.Y + YOffset[offsetIndex] * RectSize);
        }

        public static Vector2 GetRandomPointInRect(Vector2 rectPos, Random random)
        {
            float offsetX = MinDistanceBetweenLocations * 0.5f + random.Next(0, (RectSize - MinDistanceBetweenLocations) * 1000) / 1000f;
            float offsetY = MinDistanceBetweenLocations * 0.5f + random.Next(0, (RectSize - MinDistanceBetweenLocations) * 1000) / 1000f;
            
            return rectPos + new Vector2(offsetX, offsetY);
        }

        Vector2 ownRectPos;
        Random random = new Random();
        Vector2[] otherPos;
        bool[] isNeighbourOccupied;

        public LocationHelper(Vector2 pos, Vector2[] otherPos)
        {
            ownRectPos = ScenePosToRectPos(pos);
            this.otherPos = otherPos;

            CheckNeighbourhood();
        }

        /// <summary>
        /// Tries to generate new locations around ownPos. 
        /// </summary>
        /// <returns></returns>
        public Vector2[] GeneratePossibleNewLocationsAroundOwnPos(int amount)
        {
            List<Vector2> newLocations = new List<Vector2>(amount);
            while (amount > 0)
            {
                Vector2? newLoc = GenerateNewLocation();

                if (newLoc == null)
                    break;

                newLocations.Add(newLoc.Value);
                amount--;
            }

            return newLocations.ToArray();
        }

        public Vector2[] ForceGenerateNewLocationsAnywhere(int amount)
        {
            List<Vector2> newLocations = new List<Vector2>(amount);
            for (int iPos = otherPos.Length - 1; iPos >= 0; iPos--)
            {
                for (int iNeigh = 0; iNeigh < 8; iNeigh++)
                {
                    Vector2 rectPos = ApplyOffsetToRectPos(otherPos[iPos], iNeigh);
                    bool isFree = true;
                    for (int iScene = 0; iScene < otherPos.Length; iScene++)
                    {
                        if (IsSceneInRect(otherPos[iScene], rectPos))
                        {
                            isFree = false;
                            break;
                        }
                    }

                    if (isFree)
                    {
                        Vector2 newLoc = GetRandomPointInRect(rectPos, random);
                        newLocations.Add(newLoc);
                        amount--;
                        
                        if (amount <= 0)
                            break;
                    }
                }

                if (amount <= 0)
                    break;
            }

            // it is impossible for all stars to have neighbours, so newLocations should always contain at least 1 element
            return newLocations.ToArray();
        }

        private void CheckNeighbourhood()
        {
            isNeighbourOccupied = new bool[9];
            for (int iNeigh = 0; iNeigh < 8; iNeigh++)
            {
                Vector2 rectPos = ApplyOffsetToRectPos(ownRectPos, iNeigh);
                for (int iScene = 0; iScene < otherPos.Length; iScene++)
                {
                    if (IsSceneInRect(otherPos[iScene], rectPos))
                    {
                        isNeighbourOccupied[iNeigh] = true;
                        break;
                    }
                }
            }
        }

        private Vector2? GenerateNewLocation()
        {
            int startNumber = random.Next(0, 8);
            for (int i = 0; i < 8; i++)
            {
                int offsetIndex = (startNumber + i) % 8;

                if (isNeighbourOccupied[offsetIndex])
                    continue;

                // free neighbourhood detected
                Vector2 newRectPos = ApplyOffsetToRectPos(ownRectPos, offsetIndex);
                Vector2 newPos = GetRandomPointInRect(newRectPos, random);
                isNeighbourOccupied[offsetIndex] = true;

                return newPos;
            }
            return null;
        }
    }
}
