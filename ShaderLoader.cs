using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using static Consolaria.Helper;
using static Terraria.ModLoader.Core.TmodFile;

namespace Consolaria;

sealed class ShaderLoader : ModSystem {
    // shoutout to NoelFB https://github.com/NoelFB/CelesteEffects/blob/main/Distort.fx
    public static class DistortShader {
        public static Effect Effect => Distort.Value;

        private static float _anxiety;
        public static float Anxiety {
            get => _anxiety;
            set => Effect?.Parameters["anxiety"].SetValue(_anxiety = value);
        }

        private static Vector2 _anxietyOrigin;
        public static Vector2 AnxietyOrigin {
            get => _anxietyOrigin;
            set => Effect?.Parameters["anxietyOrigin"].SetValue(_anxietyOrigin = value);
        }

        private static float _gamerate;
        public static float Gamerate {
            get => _gamerate;
            set => Effect?.Parameters["gamerate"].SetValue(_gamerate = value);
        }

        private static float _waterSine;
        public static float WaterSine {
            get => _waterSine;
            set => Effect?.Parameters["waterSine"].SetValue(_waterSine = value);
        }

        private static float _waterCameraY;
        public static float WaterCameraY {
            get => _waterCameraY;
            set => Effect?.Parameters["waterCameraY"].SetValue(_waterCameraY = value);
        }

        private static float _waterAlpha;
        public static float WaterAlpha {
            get => _waterAlpha;
            set => Effect?.Parameters["waterAlpha"].SetValue(_waterAlpha = value);
        }

        private static float _strength;
        public static float Strength {
            get => _strength;
            set => Effect?.Parameters["glitch"].SetValue(_strength = value);
        }

        private static float _seed;
        public static float Seed {
            get => _seed;
            set => Effect?.Parameters["seed"].SetValue(_seed = value);
        }

        private static float _amplitude;
        public static float Amplitude {
            get => _amplitude;
            set => Effect?.Parameters["amplitude"].SetValue(_amplitude = value);
        }

        private static float _minimum;
        public static float Minimum {
            get => _minimum;
            set => Effect?.Parameters["minimum"].SetValue(_minimum = value);
        }

        public static void SetFrameSize(float width, float height) {
            Vector4 sourceRectangle = new(-width / 2f, -height / 2f, width, height);
            Vector2 size = new(width, height);
            Effect.Parameters["uSourceRect"].SetValue(sourceRectangle);
            Effect.Parameters["uLegacyArmorSourceRect"].SetValue(sourceRectangle);
            Effect.Parameters["uImageSize0"].SetValue(size);
            Effect.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
        }
        
        public static void SetDefault(float width, float height) {
            SetFrameSize(width, height);
            WaterAlpha = 0f;
            Gamerate = 1f;
            Anxiety = 1f;
            Minimum = -1f;
            Amplitude = MathHelper.TwoPi;
            Seed = Main.rand.NextFloat() * 100f;
            Strength = 1.5f;
        }
    }

    public static void ApplyEffect(Effect Effect, SpriteBatch batch, Action draw) {
        SpriteBatchSnapshot snapshot = batch.CaptureSnapshot();
        batch.End();
        batch.Begin(SpriteSortMode.Immediate, snapshot.blendState, snapshot.samplerState, snapshot.depthStencilState, snapshot.rasterizerState, snapshot.effect, snapshot.transformationMatrix);
        Effect?.CurrentTechnique.Passes[0].Apply();
        draw();
        batch.End();
        batch.Begin(in snapshot);
    }

    public static Asset<Effect> Distort => _loadedShaders["Distort"];

    // shoutout to Spirit Reforged https://github.com/GabeHasWon/SpiritReforged/blob/3e15095767b31ca7c616282d240d225a6c6147d8/AssetLoader.cs
    private static Dictionary<string, Asset<Effect>> _loadedShaders = [];

    public override void OnModLoad() {
        if (Main.dedServ) {
            return;
        }

        var tmodfile = (TmodFile)typeof(Consolaria).GetProperty("File", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Consolaria.Instance);
        var files = (IDictionary<string, FileEntry>)typeof(TmodFile).GetField("files", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(tmodfile);
        string assetsDirectory = $"Assets/";
        foreach (KeyValuePair<string, FileEntry> kvp in files.Where(x => x.Key.Contains(assetsDirectory))) {
            string shaderDirectory = assetsDirectory + "Effects/";
            bool flag = kvp.Key.Contains(".xnb");
            if (kvp.Key.Contains(shaderDirectory) && (flag || kvp.Key.Contains(".fxc"))) {
                string shaderPath = RemoveExtension(kvp.Key, flag ? ".xnb" : ".fxc");
                string shaderKey = RemoveDirectory(shaderPath, shaderDirectory);
                Asset<Effect> shaderAssetToLoad = Mod.Assets.Request<Effect>(shaderPath, AssetRequestMode.ImmediateLoad);
                _loadedShaders.Add(shaderKey, shaderAssetToLoad);
            }
        }
    }

    public override void OnModUnload() {
        if (Main.dedServ) {
            return;
        }

        _loadedShaders.Clear();
        _loadedShaders = new Dictionary<string, Asset<Effect>>();
    }

    private static string RemoveExtension(string input, string extensionType) => input[..^extensionType.Length];
    private static string RemoveDirectory(string input, string directory) => input[directory.Length..];
}
