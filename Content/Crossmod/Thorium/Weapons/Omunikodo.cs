using Consolaria.Content.Crossmod.Thorium.Dusts;

using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Sounds;

namespace Consolaria.Content.Crossmod.Thorium.Weapons;

public sealed class Omunikodo : ThoriumItem_BardBase {
    public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;

    public override void SetStaticDefaults() {
        Empowerments.AddInfo<EmpowermentProlongation>(3);
    }

    public override void SetBardDefaults() {
        Item.SetSizeValues(62, 32);

        Item.SetShopValues(Terraria.Enums.ItemRarityColor.White0, Item.sellPrice());
        Item.SetShootableValues<Omunikodo_Shot>(12f);

        Item.SetWeaponValues(60, 5f);
        Item.SetDefaultsToUsable(ItemUseStyleID.Shoot, 18, autoReuse: true, useSound: null);

        Item.holdStyle = ItemHoldStyleID.HoldHeavy;

        Item.useTime /= 3;
        Item.reuseDelay = 2;

        InspirationCost = 1;
    }

    public override void BardUseAnimation(Player player) {
        SoundStyle[] prePlanteraSounds = [
            ThoriumSounds.Accordion,
            ThoriumSounds.Bagpipe_Sound,
            ThoriumSounds.Banjo_Sound,
            ThoriumSounds.Baritone_Sound,
            ThoriumSounds.BassDrum_Sound,
            ThoriumSounds.Bassoon_Sound,
            ThoriumSounds.Bongo,
            ThoriumSounds.Clarinet_Sound,
            ThoriumSounds.Conch_Sound,
            ThoriumSounds.Cowbell_Sound,
            ThoriumSounds.Didgeridoo,
            ThoriumSounds.Flute_Sound,
            ThoriumSounds.FrenchHorn_Sound,
            ThoriumSounds.Gong_Sound,
            ThoriumSounds.Harmonica_Sound,
            ThoriumSounds.Horn,
            ThoriumSounds.Kazoo_Sound,
            ThoriumSounds.Maraca_Sound,
            ThoriumSounds.Melodica_Sound,
            ThoriumSounds.Nocturne_Sound,
            ThoriumSounds.Panflute_Sound,
            ThoriumSounds.Rackett_Sound,
            ThoriumSounds.Saxophone_Sound,
            ThoriumSounds.SlideWhistle_Sound,
            ThoriumSounds.SteelDrum_Sound,
            ThoriumSounds.String_Sound,
            ThoriumSounds.TambourineSound,
            ThoriumSounds.Trombone_Sound,
            ThoriumSounds.Trumpet_Sound,
            ThoriumSounds.Tuba_Sound,
            ThoriumSounds.WoodenClaves_Sound,
            ThoriumSounds.Whistle_Sound,
            ThoriumSounds.WindChimes_Sound,
            ThoriumSounds.Xylophone_Sound,
            ThoriumSounds.Zunpet_Sound
        ];
        Item.UseSound = Main.rand.NextFromList(prePlanteraSounds) with { PitchVariance = 0.25f, MaxInstances = 2 };
        if (Main.rand.NextBool(10)) {
            Item.UseSound = new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/OmunikodoUseSound1") with { PitchVariance = 0.25f, MaxInstances = 2 };
        }
    }

    public override bool? BardUseItem(Player player) {
        return base.BardUseItem(player);
    }

    public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
        Vector2 vector = Vector2.Normalize(velocity) * 68f;
        //if (ThoriumUtils.CanHitLine(position, position + vector)) {
        //    position += vector;
        //}
        position += vector;

        float speedX, speedY;
        speedX = velocity.X; speedY = velocity.Y;
        float num0 = 0.25f;
        float num1 = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
        double num2 = Math.Atan2(speedX, speedY);
        double num3 = num2 + 0.4f * num0;
        double num4 = num2 + 0f * num0;
        double num5 = num2 - 0.4f * num0;
        float num6 = Main.rand.NextFloat() * 0.2f + 0.95f;

        position += velocity * 1.375f;

        Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, ai2: 2);

        position -= velocity * 2.5f;
        Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, ai2: 1);

        position += velocity * 5f;
        Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, ai2: 0);

        return false;
    }

    public sealed class Omunikodo_Shot : ThoriumProjectile_BardBase {
        private float _wave;
        private byte _option = 255;
        private float _bounceCooldown;
        private bool _collided;
        private float _moveSpeed = 1f;

        public override string Texture => "Consolaria/Assets/Textures/Empty";

        public override void SetBardDefaults() {
            int width = 16; int height = width;
            Projectile.Size = new Vector2(width, height);

            //Projectile.aiStyle = -1;
            //AIType = ProjectileID.Bullet;

            Projectile.friendly = true;
            Projectile.tileCollide = false;

            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;

            Projectile.alpha = 255;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
            width = height = 2;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            return false;
        }

        public override void AI() {
            if (_bounceCooldown > 0) {
                _bounceCooldown--;
            }

            if (_option == 255) {
                _option = (byte)Projectile.ai[2];
                Projectile.ai[2] = Projectile.GetOwnerAsPlayer().direction;
            }

            if (_wave == 0f) {
                _wave = 1f;
            }
            if (_wave < 20f) {
                _wave += 0.2f;
            }
            if (_collided) {
                _moveSpeed = Helper.Approach(_moveSpeed, 0f, 0.02f);
                if (_moveSpeed <= 0.2f) {
                    Projectile.Kill();
                }
            }
            Projectile.velocity *= Utils.Remap(_moveSpeed, 0f, 1f, 0.985f, 1f, true);
            Projectile.position += new Vector2(0f, Projectile.ai[0] * Projectile.ai[2]).RotatedBy(Projectile.velocity.ToRotation()) * _wave * _moveSpeed;
            if (Projectile.ai[1] != 1f) Projectile.ai[0] -= 1f;
            else Projectile.ai[0] += 1;
            if (Projectile.ai[0] <= -6 * _moveSpeed) Projectile.ai[1] = 1f;
            if (Projectile.ai[0] >= 6 * _moveSpeed) Projectile.ai[1] = 0f;
            //if (Collision.SolidTiles(Projectile.Center - Vector2.One * 6, 3, 3)) {
            //    Projectile.Kill();
            //}

            if (Projectile.timeLeft > 120 - 1) {
                return;
            }

            void bounce() {
                _collided = true;
                //if (_bounceCooldown <= 0) {
                //    Projectile.velocity = -Projectile.velocity;
                //    _bounceCooldown = 30;
                //}
            }

            int size = 3;
            if (Collision.SolidTiles(Projectile.Center - Vector2.One * size * 2, size, size)) {
                bounce();
            }

            float num3 = 0f;
            Vector2 vector6 = Projectile.position;
            Vector2 vector7 = Projectile.oldPosition;
            vector7.Y -= num3 / 2f;
            vector6.Y -= num3 / 2f;
            int num5 = (int)Vector2.Distance(vector6, vector7) / 3 + 1;
            if (Vector2.Distance(vector6, vector7) % 3f != 0f)
                num5++;

            Color color = Color.Lerp(new Color(198, 17, 185), new Color(255, 81, 206), Main.rand.NextFloat());
            switch (_option) {
                case 0:
                    break;
                case 1:
                    color = Color.Lerp(new Color(115, 17, 196), new Color(176, 81, 255), Main.rand.NextFloat());
                    break;
                case 2:
                    color = Color.Lerp(new Color(196, 145, 17), new Color(255, 194, 81), Main.rand.NextFloat());
                    break;
            }

            bool bounced = false;
            for (float num6 = 1f; num6 <= (float)num5; num6 += 1f) {
                Dust obj = Main.dust[Dust.NewDust(Projectile.position, 0, 0, ModContent.DustType<OmunikodoDust>())];

                Vector2 position = Vector2.Lerp(vector7, vector6, num6 / (float)num5) + Projectile.Size / 2;

                obj.position = position;

                if (!bounced && Collision.SolidTiles(position + Projectile.Size / 2 - Vector2.One * size * 2, size, size)) {
                    bounce();
                    bounced = true;
                }

                obj.noGravity = true;
                obj.velocity.Y *= 0.5f;
                obj.scale *= Main.rand.NextFromList(0.9f, 1.3f);
                obj.color = color;
                obj.alpha = Main.rand.Next(150);

                obj.scale *= Ease.QuintOut(_moveSpeed);
            }
        }
    }
}
