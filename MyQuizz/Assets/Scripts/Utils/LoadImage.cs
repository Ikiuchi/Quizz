using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public static class LoadImage
{
	public delegate void MyDelegate();
	public static MyDelegate OnLoadImage;

	public static IEnumerator LoadImageFromFile(string path, RawImage rawImage, Vector2 gameObjectSize)
	{
		WWW www = new WWW(path);
		while (!www.isDone)
			yield return null;

		KeepGameObjectSize(rawImage, gameObjectSize);
		rawImage.texture = www.texture;
		rawImage.SizeToParent();

		OnLoadImage?.Invoke();
	}

	public static FileInfo[] LoadFilesFromFolder(string path)
	{
		var info = new DirectoryInfo(path);
		var fileInfo = info.GetFiles();

		return fileInfo;
	}

	/// <summary>
	/// Garde la taille du game object parce que si on met une image plus petite,
	///	SizeToParent fera diminuer la taille au fur et a mesure
	/// </summary>
	public static void KeepGameObjectSize(RawImage image, Vector2 gameObjectSize)
	{
	
		var rectTransform = image.GetComponent<RectTransform>();
		if (rectTransform != null)
			rectTransform.sizeDelta = gameObjectSize;
	}

	public static Vector2 SizeToParent(this RawImage image, float padding = 0)
	{
		float w = 0, h = 0;
		var parent = image.GetComponentInParent<RectTransform>();
		var imageTransform = image.GetComponent<RectTransform>();

		// check if there is something to do
		if (image.texture != null)
		{
			if (!parent) { return imageTransform.sizeDelta; } //if we don't have a parent, just return our current width;
			padding = 1 - padding;
			float ratio = image.texture.width / (float)image.texture.height;
			var bounds = new Rect(0, 0, parent.rect.width, parent.rect.height);
			if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90)
			{
				//Invert the bounds if the image is rotated
				bounds.size = new Vector2(bounds.height, bounds.width);
			}
			//Size by height first
			h = bounds.height * padding;
			w = h * ratio;
			if (w > bounds.width * padding)
			{ //If it doesn't fit, fallback to width;
				w = bounds.width * padding;
				h = w / ratio;
			}
		}
		imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
		imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
		return imageTransform.sizeDelta;
	}

	public static Texture2D makeTextureReadable(this Texture2D source)
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

	public static Texture2D ToTexture2D(this Texture texture)
	{
		return Texture2D.CreateExternalTexture(
			texture.width,
			texture.height,
			TextureFormat.RGB24,
			false, false,
			texture.GetNativeTexturePtr());
	}
}
