using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Linq;

public class SaveLoad : MonoBehaviour
{
    SaveManager saver = new SaveManager();
    SaveManager loader = new SaveManager();

    // Save the sprite into files
    public void Save ()
    {
        BinaryFormatter binary = new BinaryFormatter();
        Texture2D tex = DuplicateTexture(PicturesScrolling.vrPicture.texture);
        saver.x = tex.width;
        saver.y = tex.height;
        saver.bytes = ImageConversion.EncodeToPNG(tex);
        string text = JsonConvert.SerializeObject(saver);
        File.WriteAllText(Application.persistentDataPath + "/" + PicturesScrolling.vrPicture.name + ".json", text);
    }

    // Load all sprites from memory
    public void Load ()
    {
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/");
        var files = di.GetFiles().Where(o => o.Name.EndsWith(".json")).ToArray();
        for (int i = 0; i < files.Length; i++)
        {
            string text = File.ReadAllText(Application.persistentDataPath + "/" + files[i].Name);
            loader = JsonConvert.DeserializeObject<SaveManager>(text);
            Texture2D tex = new Texture2D(loader.x, loader.y);
            ImageConversion.LoadImage(tex, loader.bytes);
            Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), Vector2.one);
            mySprite.name = files[i].Name.Remove(files[i].Name.IndexOf('.'));
            Store.spritesList.Add(mySprite);
        }
    }

    // Duplicate texture to enable read/write option
    Texture2D DuplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}

[Serializable]
class SaveManager
{
    [SerializeField]
    public int x;
    [SerializeField]
    public int y;
    [SerializeField]
    public byte[] bytes;
}
