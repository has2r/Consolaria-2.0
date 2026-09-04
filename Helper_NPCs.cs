using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Consolaria;

public static class Helper_NPCs {
    public static void SetDefaultsToEnemy(this NPC npc, ushort lifeMax, 
                                                        ushort damage, 
                                                        ushort defense, 
                                                        float knockBackResist = 0f,
                                                        float spawnSlots = 0f, 
                                                        int aiStyle = -1,
                                                        bool boss = false) {
        npc.aiStyle = aiStyle;

        npc.lifeMax = lifeMax;
        npc.damage = damage;
        npc.defense = defense;
        npc.knockBackResist = knockBackResist;

        npc.friendly = false;

        npc.npcSlots = spawnSlots;

        npc.boss = boss;
    }

    public static void SetHitSounds(this NPC npc, SoundStyle? hitSound,
                                                  SoundStyle? deathSound) {
        npc.HitSound = hitSound;
        npc.DeathSound = deathSound;
    }

    public static void SetMiscellaneousProperties(this NPC npc, float dropCoins,
                                                                bool noGravity = true,
                                                                bool noTileCollide = true,
                                                                bool lavaImmune = false) {
        npc.value = dropCoins;

        npc.noGravity = noGravity;
        npc.noTileCollide = noTileCollide;

        npc.lavaImmune = lavaImmune;
    }

    public static void SetHitboxSizeValues(this NPC npc, ushort width, 
                                                         ushort height = 0) {
        npc.width = width;
        npc.height = height == 0 ? width : height;
    }

    public static void SetMaxFrames(this NPC npc, byte count) => Main.npcFrameCount[npc.type] = count;

    public static Texture2D GetTexture(this NPC npc) => TextureAssets.Npc[npc.type].Value;

    public static void QuickDraw_Vector2Scale(this NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor, Rectangle? frameBox = null, Vector2? scale = null, SpriteEffects? effect = null, float exRot = 0, float yOffset = 0f, float xOffset = 0f, Texture2D? texture = null) {
        Texture2D tex = texture ?? npc.GetTexture();
        Rectangle sourceRectangle = frameBox ?? npc.frame;
        spriteBatch.Draw(tex, npc.Center + Vector2.UnitY * yOffset + Vector2.UnitX * xOffset - screenPos + Vector2.UnitY * npc.gfxOffY, sourceRectangle, lightColor,
            npc.rotation + exRot, sourceRectangle.Centered(), scale ?? Vector2.One * npc.scale, effect ?? npc.spriteDirection.ToSpriteEffects(), 0);
    }

    public static void QuickDraw(this NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor, Rectangle? frameBox = null, float scale = 1f, SpriteEffects? effect = null, float exRot = 0, float yOffset = 0f, float xOffset = 0f, Texture2D? texture = null, Vector2? origin = null,
         Vector2? position = null!, Vector2? scaleVector2 = null, float? rotation = null) {
        Texture2D tex = texture ?? npc.GetTexture();
        Rectangle sourceRectangle = frameBox ?? npc.frame;
        origin ??= sourceRectangle.Centered();
        position ??= npc.Center;
        scaleVector2 ??= Vector2.One;
        rotation ??= npc.rotation + exRot;
        spriteBatch.Draw(tex, position.Value + Vector2.UnitY * yOffset + Vector2.UnitX * xOffset - screenPos + Vector2.UnitY * npc.gfxOffY, sourceRectangle, lightColor,
            rotation.Value, origin.Value, npc.scale * scale * scaleVector2.Value, effect ?? npc.spriteDirection.ToSpriteEffects(), 0);
    }

    public static DrawData QuickDrawAsDrawData(this NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor, Rectangle? frameBox = null, float scale = 1f, SpriteEffects? effect = null, float exRot = 0, float yOffset = 0f, float xOffset = 0f, Texture2D? texture = null) {
        Texture2D tex = texture ?? npc.GetTexture();
        Rectangle sourceRectangle = frameBox ?? npc.frame;
        return new DrawData(tex, npc.Center + Vector2.UnitY * yOffset + Vector2.UnitX * xOffset - screenPos + Vector2.UnitY * npc.gfxOffY, sourceRectangle, lightColor,
            npc.rotation + exRot, sourceRectangle.Centered(), npc.scale * scale, effect ?? npc.spriteDirection.ToSpriteEffects(), 0);
    }

}
