using System.IO;
using UnityEngine;

public class Utils
{
	static public Texture2D LoadImage(string path)
	{
		byte[] fileData = System.IO.File.ReadAllBytes(path);

		Texture2D tex = new Texture2D(1, 1);
		tex.LoadImage(fileData);
		tex.filterMode = FilterMode.Bilinear;
		tex.wrapMode = TextureWrapMode.Clamp;

		return tex;
	}

	static public void SaveImage(string path, Texture2D texture)
	{
		System.IO.File.WriteAllBytes(path, texture.EncodeToPNG());
	}

	static public float[,] LoadImage_RawFloat(string path)
	{
		FileStream fs = File.Open(path, FileMode.Open);
		BinaryReader br = new BinaryReader(fs);

		int width = br.ReadInt32();
		int height = br.ReadInt32();
		float[,] data = new float[height, width];

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				data[y, x] = br.ReadSingle();
			}
		}

		br.Close();
		fs.Close();

		return data;
	}
	
	static public Vector4[,] LoadImage_RawFloat4(string path)
	{
		FileStream fs = File.Open(path, FileMode.Open);
		BinaryReader br = new BinaryReader(fs);

		int width = br.ReadInt32();
		int height = br.ReadInt32();
		Vector4[,] data = new Vector4[height, width];

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				Vector4 v = new Vector4();

				v.x = br.ReadSingle();
				v.y = br.ReadSingle();
				v.z = br.ReadSingle();
				v.w = br.ReadSingle();

				data[y, x] = v;
			}
		}

		br.Close();
		fs.Close();

		return data;
	}

	static public Vector3[,] LoadImage_Normals(string path)
	{
		Texture2D texture = LoadImage(path);
		Vector3[,] data = new Vector3[texture.height, texture.width];

		for (int y = 0; y < texture.height; y++)
		{
			for (int x = 0; x < texture.width; x++)
			{
				Color c = texture.GetPixel(x, y);

				Vector3 v = new Vector3(c.r, c.g, c.b);
				v.x = 2.0f * v.x - 1.0f;
				v.y = 2.0f * v.y - 1.0f;
				v.z = 2.0f * v.z - 1.0f;
				v.Normalize();

				data[y, x] = v;
			}
		}

		return data;
	}

	static public T SampleImage_Clamp<T>(T[,] data, int x, int y)
	{
		int height = data.GetLength(0);
		int width = data.GetLength(1);

		if (x < 0)
			x = 0;
		if (y < 0)
			y = 0;

		if (x >= width)
			x = width - 1;
		if (y >= height)
			y = height - 1;

		return data[y, x];
	}
}
