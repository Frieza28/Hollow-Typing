#if UNITY_EDITOR
using GIGA.PixelCableRenderer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GIGA.PixelCableRenderer.Editor
{
	public class CableRendererAboutDialog : EditorWindow
	{
		public Texture2D starTexture;
		public Texture2D icon;

		private static GUIStyle starStyle, feedbackButtonStyle,greenButtonStyle;
		private static float starAnimTime;
		private static float animTime;

		public static void Init()
		{
			var window = (CableRendererAboutDialog)EditorWindow.GetWindow(typeof(CableRendererAboutDialog), true, "Soft Pixel Cables");
			window.minSize = new Vector2(400, 590);
			window.maxSize = new Vector2(400, 590);
			window.Show();
			starAnimTime = (float)EditorApplication.timeSinceStartup;
			animTime = 0;

			starStyle = new GUIStyle("label");
			feedbackButtonStyle = new GUIStyle("button");
			feedbackButtonStyle.normal.background = MakeTex(2, 2, new Color32(0x21, 0x96, 0xf3, 255));
			feedbackButtonStyle.hover.background = MakeTex(2, 2, new Color32(0x24, 0x99, 0xf7, 255));

			greenButtonStyle = new GUIStyle("button");
			greenButtonStyle.normal.background = MakeTex(2, 2, new Color32(0x2, 0xd6, 0x73, 255));
			greenButtonStyle.hover.background = MakeTex(2, 2, new Color32(0x4, 0xd9, 0x77, 255));
		}

		void OnGUI()
		{
			string version = CableRenderer.VERSION;

			EditorGUILayout.BeginHorizontal();

			GUILayout.Space(10);
			EditorGUILayout.BeginVertical();

			EditorGUILayout.BeginHorizontal();

			GUI.DrawTexture(new Rect(10, 8, 64, 64), this.icon);
			GUILayout.Space(70);

			EditorGUILayout.BeginVertical();
			GUILayout.Space(10);
			EditorGUILayout.LabelField(string.Format("Soft Pixel Cables LITE"), EditorStyles.boldLabel);
			EditorGUILayout.LabelField(string.Format("Version: {0}", version));
			EditorGUILayout.LabelField(string.Format("Copyright \u00A9 GIGA Softworks, 2024 "));
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndHorizontal();

			GUILayout.Space(30);
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


			GUIStyle linkStyle = new GUIStyle("label");
			linkStyle.normal.textColor = Color.blue;

			EditorGUILayout.LabelField(string.Format("Online Documentation:"), EditorStyles.boldLabel);
			if (GUILayout.Button("Documentation", linkStyle))
				Application.OpenURL("https://www.gigasoftworks.com/products/cablerenderer/docs/overview.html");


			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

			EditorGUILayout.LabelField(string.Format("GIGA Softworks Website:"), EditorStyles.boldLabel);
			if (GUILayout.Button("http://www.gigasoftworks.com", linkStyle))
				Application.OpenURL("http://www.gigasoftworks.com");

			EditorGUILayout.LabelField(string.Format("Contact:"), EditorStyles.boldLabel);
			string address = "contact@gigasoftworks.com";
			if (GUILayout.Button(address, linkStyle))
			{
				string subject = "";
				Application.OpenURL(string.Format("mailto:{0}?subject={1}", address, subject));
			}

			EditorGUILayout.LabelField(string.Format("Bug Report:"), EditorStyles.boldLabel);
			address = "bugs@gigasoftworks.com";

			if (GUILayout.Button(address, linkStyle))
			{
				string subject = string.Format("Soft Pixel Cables Bug Report - Ver. {0} Unity {1}", version, Application.unityVersion);
				Application.OpenURL(string.Format("mailto:{0}?subject={1}", address, subject));
			}

			EditorGUILayout.LabelField(string.Format("Follow me on X for latest updates:"), EditorStyles.boldLabel);
			if (GUILayout.Button("https://x.com/GigaSoftworks", linkStyle))
				Application.OpenURL("https://x.com/GigaSoftworks");


			EditorGUILayout.EndVertical();

			GUILayout.Space(10);
			EditorGUILayout.EndHorizontal();

			float elapsed = (float)EditorApplication.timeSinceStartup - starAnimTime;
			animTime += elapsed;
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			for (int k = 0; k < elapsed % 5; k++)
			{
				if (GUI.Button(new Rect(20 + k * 32 + k * 4, 360, 32, 32), this.starTexture, starStyle))
					Application.OpenURL("https://assetstore.unity.com/packages/slug/235811");
			}
			GUILayout.BeginArea(new Rect(20, 400, 300, 100), "Thank you for downloading this asset!\nAs a small indie developer, your feedback helps me improve this\nasset and continue its development.\nIf you've found it useful, please consider leaving a review or\nsharing your thoughts: ");
			GUILayout.EndArea();

			if (GUI.Button(new Rect(55, 490, 300, 40), "Leave feedback", feedbackButtonStyle))
				Application.OpenURL("https://assetstore.unity.com/packages/slug/235811");

			if (GUI.Button(new Rect(55, 536, 300, 40), "View Full Package", greenButtonStyle))
				Application.OpenURL("https://assetstore.unity.com/packages/slug/305504");

			Repaint();
		}

		private static Texture2D MakeTex(int width, int height, Color col)
		{
			Color[] pix = new Color[width * height];
			for (int i = 0; i < pix.Length; ++i)
			{
				pix[i] = col;
			}
			Texture2D result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();
			return result;
		}

	}
}
#endif
