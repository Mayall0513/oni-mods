using System.IO;
using UnityEngine;

namespace Pliers {
    public static class Utilities {
        public static Sprite CreateSpriteDxt5(Stream inputStream, int width, int height) {
            byte[] buffer = new byte[inputStream.Length - 128];
            inputStream.Seek(128, SeekOrigin.Current);
            inputStream.Read(buffer, 0, buffer.Length);

            Texture2D texture = new Texture2D(width, height, TextureFormat.DXT5, false);
            texture.LoadRawTextureData(buffer);
            texture.Apply(false, true);
            return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5F, 0.5F));
        }

        public static CellOffset ConnectionsToOffset(UtilityConnections utilityConnections) {
            return utilityConnections switch {
                UtilityConnections.Left => new CellOffset(-1, 0),
                UtilityConnections.Right => new CellOffset(1, 0),
                UtilityConnections.Up => new CellOffset(0, 1),
                _ => new CellOffset(0, -1)
            };
        }

        public static UtilityConnections OppositeDirection(UtilityConnections utilityConnections) {
            return utilityConnections switch {
                UtilityConnections.Left => UtilityConnections.Right,
                UtilityConnections.Right => UtilityConnections.Left,
                UtilityConnections.Up => UtilityConnections.Down,
                _ => UtilityConnections.Up,
            };
        }
    }
}
