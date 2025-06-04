#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace GIGA.PixelCableRenderer.Editor
{
	[InitializeOnLoad]
	public class StartupDialog : EditorWindow
	{
		private const string EDITORPREF_STARTUP_DONT_SHOW_AGAIN = "gigasoftworks_soft_cables_lite_startup_dontshow";
		private const string EDITORPREF_STARTUP_EVER_SHOWN = "gigasoftworks_soft_cables_lite_startup_evershown";
		private const int DEFAULT_WIDTH = 800;
		private const int DEFAULT_HEIGHT = 590;
		private const int WHATS_NEW_SECTION_HEIGHT = 200;

		private static bool alreadyShown;
		private static float delay;
		private static bool defaultKeysSelection = true;
		private static bool dontShowAgain = false;

		// References
		public Texture2D selectorImage1;
		public Texture2D selectorImage2;
		public Texture2D selectorImage3;
		public Texture2D whatsNewImage1;
		public Texture2D whatsNewImage2;

		private static GUISkin imgSkin;
		private static bool showWhatsNewSection = true;

		static StartupDialog()
		{
			if (!alreadyShown)
			{
				EditorApplication.update += Update;
				alreadyShown = true;


			}
		}

		static void Update()
		{
			delay += Time.deltaTime;

			if (delay >= 1.2f)
			{
				if (!Application.isPlaying)
				{
					bool show = true;

					if (EditorPrefs.HasKey(GetVersionDependantSettingKey(EDITORPREF_STARTUP_DONT_SHOW_AGAIN)) && EditorPrefs.GetBool(GetVersionDependantSettingKey(EDITORPREF_STARTUP_DONT_SHOW_AGAIN)))
						show = false;
					else
					{
						bool everShown = EditorPrefs.HasKey(GetVersionDependantSettingKey(EDITORPREF_STARTUP_EVER_SHOWN)) && EditorPrefs.GetBool(GetVersionDependantSettingKey(EDITORPREF_STARTUP_EVER_SHOWN));
						if (everShown && EditorApplication.timeSinceStartup > 30)
							show = false;
					}

					if (show)
					{
						StartupDialog.Init();
						EditorPrefs.SetBool(GetVersionDependantSettingKey(EDITORPREF_STARTUP_EVER_SHOWN), true);
					}
				}

				EditorApplication.update -= Update;
			}
		}

		public static void Init()
		{
			var window = (StartupDialog)EditorWindow.GetWindow(typeof(StartupDialog), true, "Soft Pixel Cables LITE");
			if (!showWhatsNewSection)
			{
				window.minSize = new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT);
				window.maxSize = new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT);
			}
			else
			{
				window.minSize = new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT + WHATS_NEW_SECTION_HEIGHT);
				window.maxSize = new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT + WHATS_NEW_SECTION_HEIGHT);
			}

			dontShowAgain = EditorPrefs.HasKey(GetVersionDependantSettingKey(EDITORPREF_STARTUP_DONT_SHOW_AGAIN)) && EditorPrefs.GetBool(GetVersionDependantSettingKey(EDITORPREF_STARTUP_DONT_SHOW_AGAIN)) ? true : false;
			window.Show();
		}

		void OnGUI()
		{
			GUIStyle wrappedText = new GUIStyle("label");
			wrappedText.alignment = TextAnchor.UpperCenter;
			wrappedText.wordWrap = true;

			GUIStyle centeredTitle = new GUIStyle("label");
			centeredTitle.alignment = TextAnchor.MiddleCenter;
			centeredTitle.fontStyle = FontStyle.Bold;

			GUIStyle greenButtonStyle = new GUIStyle("button");
			greenButtonStyle.normal.background = MakeTex(2, 2, new Color32(0x2, 0xd6, 0x73, 255));
			greenButtonStyle.hover.background = MakeTex(2, 2, new Color32(0x4, 0xd9, 0x77, 255));


			EditorGUILayout.BeginVertical(); // Main container
			GUILayout.Space(10);
			EditorGUILayout.LabelField("Thank you for downloading Soft Pixel Cables!", centeredTitle);
			GUILayout.Space(10);
			EditorGUILayout.LabelField("This package is the demo version of the full Soft Pixel Cables package, which includes more customization options and 3 additional renderers:",wrappedText);

			// Selectors
			GUILayout.Space(10);
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			Color exColor = GUI.backgroundColor;

			// Img 1
			EditorGUILayout.BeginVertical();
			DrawImage(selectorImage1,200);
			EditorGUILayout.LabelField("Elastic String", centeredTitle);
			EditorGUILayout.LabelField("Stretches, snaps back, and delivers a bouncy, interactive experience. Experiment with exotic color combinations to simulate electric sparks or energy fluxes.\r\n", wrappedText, GUILayout.Width(200), GUILayout.Height(80));
			EditorGUILayout.EndVertical();

			// Img 2
			EditorGUILayout.BeginVertical();
			DrawImage(selectorImage2,200);
			EditorGUILayout.LabelField("Tentacle", centeredTitle);
			EditorGUILayout.LabelField("Versatile and dynamic. Ideal for creatures, smoke, grass, energy trails, and more. It also features a collider on the tip to detect when it interacts with other objects.\r\n", wrappedText, GUILayout.Width(200), GUILayout.Height(80));
			EditorGUILayout.EndVertical();

			// Img 3
			EditorGUILayout.BeginVertical();
			DrawImage(selectorImage3,200);
			EditorGUILayout.LabelField("Rope", centeredTitle);
			EditorGUILayout.LabelField("Naturally sways, swings, and simulates physics when dragged around. You can attach objects to its anchor point to create swinging traps and other dynamic elements.\r\n\r\n", wrappedText, GUILayout.Width(200), GUILayout.Height(80));
			EditorGUILayout.EndVertical();

			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			GUI.backgroundColor = exColor;

			// Run button
			GUILayout.Space(15);
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			GUI.backgroundColor = new Color32(0x2, 0xd6, 0x73, 255);
			if (GUILayout.Button("View Full Package", GUILayout.MinHeight(60),GUILayout.MaxWidth(400)))
			{
				Application.OpenURL("https://assetstore.unity.com/packages/slug/305504");
			}
			GUI.backgroundColor = exColor;

			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(10);
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

			// What's new section
			if (showWhatsNewSection)
				DrawWhatsNewSection();
			else
				GUILayout.Space(10);

			EditorGUILayout.BeginHorizontal();

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("About Soft Pixel Cables...",GUILayout.MinHeight(40), GUILayout.MinWidth(200)))
				CableRendererAboutDialog.Init();

			if (GUILayout.Button("Close",GUILayout.MinHeight(40), GUILayout.MinWidth(200)))
				this.CloseDialog();

			GUILayout.FlexibleSpace();

			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			dontShowAgain = EditorGUILayout.Toggle("Don't show again", dontShowAgain);
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndVertical(); // Main container
			
		}

		private void DrawWhatsNewSection()
		{
			GUIStyle title = new GUIStyle("label");
			title.fontStyle = FontStyle.Bold;
			GUIStyle wrappedText = new GUIStyle("label");
			wrappedText.wordWrap = true;

			EditorGUILayout.BeginHorizontal(); // Main container
			
			EditorGUILayout.BeginVertical(); // Left Column

			// Title
			EditorGUILayout.LabelField("3D Support added! (Full package only)", title);
			// Text
			EditorGUILayout.LabelField("Version 1.1 takes Soft Pixel Cables to the next dimension!\r\nYou can now place cables seamlessly in 3D space with dedicated editor handles. Full rendering support is included, featuring billboard rendering for optimal visual feedback when needed.",wrappedText,GUILayout.MinHeight(WHATS_NEW_SECTION_HEIGHT / 2),GUILayout.MaxWidth(DEFAULT_WIDTH / 2));

			EditorGUILayout.EndVertical(); // Left Column

			EditorGUILayout.BeginVertical(); // Right Column

			EditorGUILayout.BeginHorizontal(); 
			EditorGUILayout.EndHorizontal(); 


			EditorGUILayout.EndVertical(); // Right Column
			DrawImage(whatsNewImage1,160);
			DrawImage(whatsNewImage2,160);
			EditorGUILayout.EndHorizontal(); // Main container

			GUILayout.Space(10);
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			GUILayout.Space(10);
		}

		private void CloseDialog()
		{
			EditorPrefs.SetBool(GetVersionDependantSettingKey(EDITORPREF_STARTUP_DONT_SHOW_AGAIN), dontShowAgain);
			this.Close();
		}

		private void DrawImage(Texture2D img,float maxSize)
		{
			EditorGUILayout.BeginVertical("box");
			GUILayout.Label(img, GUILayout.MaxWidth(maxSize), GUILayout.MaxHeight(maxSize));
			EditorGUILayout.EndVertical();
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

		private static string GetVersionDependantSettingKey(string key)
		{
			return key + "_" + CableRenderer.VERSION;
		}
	}
}

#endif
