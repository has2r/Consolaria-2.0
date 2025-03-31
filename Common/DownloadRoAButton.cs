using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Consolaria.Common;

sealed class DownloadRoAButton : ILoadable {
    private static bool _shouldBeShown = true;
    private static string _url;
    private static bool _isNew = true;
    private static bool _requested;

    private static readonly HttpClient client = new HttpClient();

    private static float _lerpColorProgress;
    private static Color _lerpColor;
    private static Color? _currentColor = null, _nextColor = null;

    private sealed class VariableStorage : ModSystem {
    }

    void ILoadable.Load(Mod mod) {
        LoadFile();
        On_Main.HandleNews += On_Main_HandleNews;
    }

    private void LoadFile() {
        if (ModLoader.HasMod("RoA")) {
            return;
        }

        string dir = Path.Join(Main.SavePath, "Consolaria");
        if (!File.Exists(dir)) {
            File.WriteAllText(dir, null);
        }
        string[] lines = File.ReadAllLines(dir);
        if (lines.Contains(nameof(_shouldBeShown))) {
            _shouldBeShown = false;
        }
        if (lines.Contains(nameof(_isNew))) {
            _isNew = false;
        }
    }

    private void On_Main_HandleNews(On_Main.orig_HandleNews orig, Microsoft.Xna.Framework.Color menuColor) {
        orig(menuColor);

        if (ModLoader.HasMod("RoA")) {
            return;
        }

        if (Main.menuMode != 0) {
            return;
        }

        if (!_shouldBeShown) {
            return;
        }

        string url = "https://steamcommunity.com/sharedfiles/filedetails/?id=2864843929";
        if (!_requested) {
            client.GetStringAsync(url).ContinueWith(response => {
                if (!response.IsCompletedSuccessfully || response.Exception != null) {
                    _shouldBeShown = false;
                    return;
                }
                _url = url;
            });
            _requested = true;
        }

        string latestNewsText = Language.GetTextValue("Mods.Consolaria.DownloadRoA");
        string latestNewsText2 = Language.GetTextValue("Mods.Consolaria.DownloadRoA2");
        bool shouldDrawHideButton = false;
        var newsScale = 1.2f;
        var newsScales = new Vector2(newsScale);
        var newsPosition = new Vector2(Main.screenWidth - 10f, Main.screenHeight - 69f);
        var newsSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, latestNewsText, newsScales);
        var newsRect = new Rectangle((int)(newsPosition.X - newsSize.X), (int)(newsPosition.Y - newsSize.Y), (int)newsSize.X, (int)newsSize.Y);
        bool newsMouseOver = newsRect.Contains(Main.mouseX, Main.mouseY);
        var previousSize = newsSize;
        if (newsMouseOver) {
            latestNewsText = latestNewsText.PadRight(latestNewsText.Length + latestNewsText2.Length + 2, ' ');
            shouldDrawHideButton = true;
        }
        if (_isNew) {
            menuColor = GetLerpColor([new Color(77, 210, 89), new Color(138, 241, 95)]);
        }
        newsPosition = new Vector2(Main.screenWidth - 10f, Main.screenHeight - 69f);
        newsSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, latestNewsText, newsScales);
        newsRect = new Rectangle((int)(newsPosition.X - newsSize.X), (int)(newsPosition.Y - newsSize.Y),
            (int)(newsSize.X - (shouldDrawHideButton ? (newsSize.X - previousSize.X) : 0)), (int)newsSize.Y);
        newsMouseOver = newsRect.Contains(Main.mouseX, Main.mouseY);
        var newsColor = newsMouseOver /*&& newsURL != null */? Main.highVersionColor : menuColor;
        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, latestNewsText, newsPosition - newsSize, newsColor, 0f, Vector2.Zero, newsScales);

        string dir = Path.Join(Main.SavePath, "Consolaria");
        if (newsMouseOver && Main.hasFocus/* && newsURL != null*/) {
            if (_isNew) {
                _isNew = false;
                File.AppendAllText(dir, nameof(_isNew));
            }
            if (Main.mouseLeftRelease && Main.mouseLeft) {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                Utils.OpenToURL(_url);
            }
            //newsIsNew = false;
        }

        if (!shouldDrawHideButton) {
            return;
        }

        newsScale = 1.2f;
        newsScales = new Vector2(newsScale);
        newsPosition = new Vector2(Main.screenWidth - 10f, Main.screenHeight - 70f);
        newsSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, latestNewsText2, newsScales);
        newsRect = new Rectangle((int)(newsPosition.X - newsSize.X), (int)(newsPosition.Y - newsSize.Y), (int)newsSize.X, (int)newsSize.Y);
        newsMouseOver = newsRect.Contains(Main.mouseX, Main.mouseY);
        newsColor = newsMouseOver /*&& newsURL != null */? Main.highVersionColor : menuColor;
        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, latestNewsText2, newsPosition - newsSize, newsColor, 0f, Vector2.Zero, newsScales);

        if (newsMouseOver && Main.hasFocus/* && newsURL != null*/) {
            if (Main.mouseLeftRelease && Main.mouseLeft) {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                File.AppendAllText(dir, nameof(_shouldBeShown));
                _shouldBeShown = false;
            }
            //Utils.OpenToURL(newsURL);
            //newsIsNew = false;
        }
    }

    private Color GetLerpColor(List<Color> from) {
        _lerpColorProgress += 0.005f;
        int colorCount = from.Count;
        for (int i = 0; i < colorCount; i++) {
            float part = 1f / colorCount;
            float min = part * i;
            float max = part * (i + 1);
            if (_lerpColorProgress >= min && _lerpColorProgress <= max) {
                _lerpColor = Color.Lerp(from[i], from[i == colorCount - 1 ? 0 : (i + 1)], Utils.Remap(_lerpColorProgress, min, max, 0f, 1f, true));
            }
        }
        if (_lerpColorProgress > 1f) {
            _lerpColorProgress = 0f;
        }
        return _lerpColor;
    }

    void ILoadable.Unload() {
    }
}
