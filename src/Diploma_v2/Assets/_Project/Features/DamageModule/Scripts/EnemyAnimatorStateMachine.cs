using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Project.Features.DamageModule {
    public class EnemyAnimatorStateMachine : MonoBehaviour {
        [SerializeField] private Animator _animator;

        [SerializeField] private SerializedDictionary<State, string> _animations;

        private const float CROSSFADE_DURATION = 0.1f;

        //
        // private readonly Dictionary<State, List<int>> _animationVariants = new() {
        //     { State.Idle, new() { ToHash(ANIM_IDLE_1_INPLACE), ToHash(ANIM_IDLE_2_INPLACE) } },
        //     { State.Walk, new() { ToHash(ANIM_WALK_1_INPLACE), ToHash(ANIM_WALK_2_INPLACE) } },
        //     { State.WalkFast, new() { ToHash(ANIM_WALK_FAST_INPLACE) } },
        //     { State.Run, new() { ToHash(ANIM_RUN_1_INPLACE), ToHash(ANIM_RUN_2_INPLACE) } },
        //     { State.WalkBack, new() { ToHash(ANIM_WALK_BACK_INPLACE) } },
        //     { State.LightAttack1, new() { ToHash(ANIM_ATTACK_1_INPLACE) } },
        //     { State.LightAttack2, new() { ToHash(ANIM_ATTACK_2_INPLACE) } },
        //     { State.LightAttack3, new() { ToHash(ANIM_ATTACK_3_INPLACE) } },
        //     { State.HeavyAttack, new() { ToHash(ANIM_ATTACK_4_INPLACE) } },
        //     { State.Flying, new() { ToHash(ANIM_FLY_INPLACE) } },
        //     { State.Death, new() { ToHash(ANIM_DEATH_1_INPLACE), ToHash(ANIM_DEATH_2_INPLACE) } }
        // };
        

        private State _currentState;

        public void SwitchToState(State state) {
            if (_currentState == state)
                return;

            _currentState = state;
            int randomAnimation = GetRandomAnimation(state);
            _animator.CrossFade(randomAnimation, CROSSFADE_DURATION);
        }

        private int GetRandomAnimation(State state) =>
            ToHash(_animations[state]);

        private static int ToHash(string animName) =>
            Animator.StringToHash(animName);
    }
}