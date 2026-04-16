using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets;

public sealed class AlbinoRunt : ConsolariaFlyingPet {
    public override int maxFrames => 6;
    public override int PreviewOffsetX => -4;
    public override int PreviewOffsetY => -30;
    public override int PreviewSpriteDirection => -1;
    public override bool isLightPet => false;

    public override void SetDefaults() {
        int width = 34; int height = 34;
        Projectile.Size = new Vector2(width, height);

        base.SetDefaults();
    }

    public override void AI() {
        Player player = Main.player[Projectile.owner];
        if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.AlbinoRuntBuff>()))
            Projectile.timeLeft = 2;

        FloatingAI(tilt: 0.025f);
        int frameTime = 2;
        Animation(frameTime);
    }
}