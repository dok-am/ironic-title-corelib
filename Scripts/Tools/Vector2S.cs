using System;
using UnityEngine;

namespace IT.CoreLib.Tools
{
    /// <summary>
    /// Serializable Vector2
    /// </summary>
    [Serializable]
    public struct Vector2S
    {
        public float x;
        public float y;

        public Vector2S(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2S))
            {
                return false;
            }

            var s = (Vector2S)obj;
            return x == s.x &&
                   y == s.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }

        public static bool operator ==(Vector2S a, Vector2S b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Vector2S a, Vector2S b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public static implicit operator Vector2(Vector2S x)
        {
            return new Vector2(x.x, x.y);
        }

        public static implicit operator Vector2S(Vector2 x)
        {
            return new Vector2S(x.x, x.y);
        }
    }
}