using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets
{
    public class WormData : ModPlayer {
        public bool IsWormPetActive { get; internal set; }

        public override void ResetEffects()
            => IsWormPetActive = false;
    }

    public class WormSoundManager {
        public enum WormSoundType {
            Boring,
            Bye,
            Grenade,
            Incoming,
            Jump,
            Victory
        }

        private const int IDLE_MAXTIME = 3600;

        public const WormSoundType BORING = WormSoundType.Boring;
        public const WormSoundType BYE = WormSoundType.Bye;
        public const WormSoundType GRENADE = WormSoundType.Grenade;
        public const WormSoundType INCOMING = WormSoundType.Incoming;
        public const WormSoundType JUMP = WormSoundType.Jump;
        public const WormSoundType VICTORY = WormSoundType.Victory;

        private readonly Projectile _itself;

        private int _idleTimer, _cdBetweenQuotesTimer;
        private bool _eventStarted, _incomingSoundPlayed;

        public static WormSoundManager Instance { get; private set; }

        public Player Player
            => Main.player[_itself.owner];

        public int GetBoredd {
            get => _idleTimer;
            set {
                if (_itself.velocity.Length() <= 1f) {
                    _idleTimer = value;

                    const int maxTime = IDLE_MAXTIME;
                    if (_idleTimer >= maxTime) {
                        PlayWormSound(WormSoundType.Boring, () => { _idleTimer = 0; });
                    }
                }
                else {
                    _idleTimer = 0;
                }
            }
        }

        public int CDBetweenQuotes {
            get => _cdBetweenQuotesTimer;
            set {
                if (value > 0) {
                    _cdBetweenQuotesTimer = value;
                }
            }
        }

        public bool CanSpellNextQuote
            => CDBetweenQuotes <= 1;

        public WormSoundManager(Projectile itself) {
            Instance = this;

            _itself = itself;
        }

        public void Update() {
            UpdateValues();

            PlayJumpSound();
            PlayPlayersDeathSound();
            PlayGrenadeSound();
            PlayIncomingSound();
        }

        private void UpdateValues() {
            CDBetweenQuotes--;
            GetBoredd++;
        }

        internal void PlayWormSound(WormSoundType soundType, Action whenPlayed = null, bool noQueue = false) {
            if ((!CanSpellNextQuote && !noQueue) || Main.netMode == NetmodeID.Server) {
                return;
            }

            SoundStyle style = new($"{nameof(Consolaria)}/Assets/Sounds/Worm/{soundType}");
            SoundEngine.PlaySound(style, _itself.Center);

            whenPlayed?.Invoke();

            CDBetweenQuotes = 300;
        }

        private void PlayJumpSound() {
            bool isInFlightState = _itself.ai[0] != 0f,
                 jumped = _itself.velocity.Y < -1f;
            if (isInFlightState || !jumped) {
                return;
            }
            PlayWormSound(JUMP);
        }

        private void PlayPlayersDeathSound() {
            if (!Player.dead) {
                return;
            }
            PlayWormSound(BYE, noQueue: true);
        }

        private void PlayGrenadeSound() {
            Projectile explosivePlayerOwnedProjectile = Main.projectile.FirstOrDefault(projectile => projectile.type == Player.inventory[Player.selectedItem].shoot && projectile.owner == Player.whoAmI && projectile.aiStyle == 16);
            bool arentThereAnyPlayerOwnedExplosiveProjectiles = explosivePlayerOwnedProjectile == null || explosivePlayerOwnedProjectile == new Projectile() || !explosivePlayerOwnedProjectile.active;
            if (arentThereAnyPlayerOwnedExplosiveProjectiles) {
                return;
            }
            if (!Player.ItemAnimationJustStarted) {
                return;
            }
            PlayWormSound(GRENADE);
        }

        private void PlayIncomingSound() {
            bool isThereAnyInvasion = Main.invasionProgress != -1;
            if (Main.bloodMoon || Main.eclipse || isThereAnyInvasion) {
                _eventStarted = true;
            }

            if (!Main.bloodMoon && !Main.eclipse && !isThereAnyInvasion) {
                _eventStarted = false;
            }

            if (!_eventStarted) {
                _incomingSoundPlayed = false;
                return;
            }

            if (_incomingSoundPlayed) {
                return;
            }

            PlayWormSound(INCOMING, () => { _incomingSoundPlayed = true; }, true);
        }
    }

    public class Worm : ConsolariaPet2 {
        private static WormSoundManager _soundManager;

        public override void SetStaticDefaults() {
            Main.projFrames[Type] = 8;

            ProjectileID.Sets.CharacterPreviewAnimations[Type] = ProjectileID.Sets.SimpleLoop(5, 3)
				.WithOffset(-4, 0)
				.WithSpriteDirection(-1)
                .WhenNotSelected(4, 0);

            base.SetStaticDefaults();
        }

        public override void SetDefaults() {
            DrawOriginOffsetY = -5;

            int width = 36, height = 40;
            Projectile.Size = new Vector2(width, height);

            base.SetDefaults();

            _soundManager = new WormSoundManager(Projectile);
        }

        public override bool PreAI() {
            Player player = Main.player[Projectile.owner];
            WormData wormData = player.GetModPlayer<WormData>();
            if (player.dead) {
                wormData.IsWormPetActive = false;
            }
            if (wormData.IsWormPetActive) {
                Projectile.timeLeft = 2;
            }
            else {
                Projectile.Kill();
            }

            return true;
        }

        public override void PostAI()
            => _soundManager.Update();

        private class CheckKilledBossesSystem : GlobalNPC {
            public override void OnKill(NPC npc) {
                if (!npc.boss) {
                    return;
                }
                _soundManager.PlayWormSound(WormSoundManager.VICTORY, noQueue: true);
            }
        }
    }
}