using System.Collections.Generic;
using UnityEngine;

public static class CursorUtility
{
    private static Dictionary<Sprite, Texture2D> cache = new Dictionary<Sprite, Texture2D>();

    public static Texture2D ExtractTexture(Sprite sprite)
    {
        if (cache.TryGetValue(sprite, out var tex))
            return tex;

        var rect = sprite.textureRect;
        tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);

        var data = sprite.texture.GetPixels(
            (int)rect.x, (int)rect.y,
            (int)rect.width, (int)rect.height
        );

        tex.SetPixels(data);
        tex.Apply();

        cache[sprite] = tex;
        return tex;
    }
}
