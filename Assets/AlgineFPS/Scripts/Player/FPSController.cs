using Algine.FPS.MobileInput;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Algine
{
    public class FPSController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float MoveSpeed = 1f;
        public float CrouchSpeed = 0.4f;

        public float RunSpeedMultiplier = 2f;
        public float JumpForce = 4f;
        public float CrouchSpeedMultiplier = 10f;
        public float CrouchHeight = 0.5f;
        public LayerMask GroundLayerMask;
        public Transform CamHolder;

        //dependencies Component
        [SerializeField]
        private Animator weaponHolderAnimator;
        private Rigidbody m_controllerRigidbody;
        private CapsuleCollider m_controllerCollider;
        private AudioSource m_audioSource;

        [Header("Damage settings")]
        [Tooltip("Player's health")]
        public int m_health = 100;
        [Tooltip("UI element to draw health as number")]
        public TextMeshProUGUI HealthUIText;
        [Tooltip("UI element to draw health as slider")]
        public Slider HealthUISlider;

        [Header("Damage effect")]
        [Tooltip("UI Image with fullscreen hit fx")]
        public Image DamageScreenFX;
        [Tooltip("UI Image color to change on hit")]
        public Color DamageScreenColor;
        [Tooltip("UI Image fade speed after hit")]
        public float DamageScreenFadeSpeed = 1.4f;

        [Header("MouseLook Settings")]
        [SerializeField]
        private Vector2 MaximumSensitivity = new Vector2 { x = .5f, y = .5f };
        [SerializeField]
        private float LimitForXAngle = 100f;

        private Vector2 _sensitivity = new Vector2 { x = .08f, y = .08f };
        public Vector2 Sensitivity
        {
            get
            {
                return _sensitivity;
            }
            set
            {
                if (value.x > MaximumSensitivity.x)
                {
                    _sensitivity.x = MaximumSensitivity.x;
                }
                if (value.y > MaximumSensitivity.y)
                {
                    _sensitivity.x = MaximumSensitivity.y;
                }
                _sensitivity = value;
            }
        }

        //Exposed Properties
        [HideInInspector]
        public Vector3 Climbdir = Vector3.up;
        [HideInInspector]
        public bool IsClimbing { get; private set; }
        [HideInInspector]
        public bool IsCrouching { get; private set; }
        [HideInInspector]
        public bool IsRunning { get; private set; }

        [SerializeField]
        private AudioClip landingSound;
        [SerializeField]
        private AudioClip jumpingSound;

        //hold States 
        private Vector3 m_playerStartPos;
        private float moveSpeedLocal;
        private Vector3 m_initial_holder_pos;
        private float m_normalHeight = 1.6f;
        private Vector2 _mouseAbsolute;
        private Vector2 _smoothMouse;
        private float distanceToGround;
        private float inAirTime;
        private Vector3 dirVector;
        private Color m_damageScreenColor_temp;
        private bool m_addHealth;
        private bool m_isPlayerDead;

        private Vector2 m_touchDir;
        private Vector2 m_joyStictDir;

        private void Start()
        {
            m_controllerRigidbody = GetComponent<Rigidbody>();
            m_controllerCollider = GetComponent<CapsuleCollider>();

            m_playerStartPos = transform.position;

            m_audioSource = GetComponentInChildren<AudioSource>();

            distanceToGround = GetComponent<CapsuleCollider>().bounds.extents.y;

            if (weaponHolderAnimator == null)
            {
                Debug.LogError("WEapon Holder Animator Component Required");
            }
            InputEvents.Current.OnJump += OnJump;
            InputEvents.Current.OnCrouch += OnCrouch;
            InputEvents.Current.OnJoyStickDrag += onJoyStictDrag;
            InputEvents.Current.OnTouchLook += onTouchLook;

            m_normalHeight = m_controllerCollider.height;
            m_initial_holder_pos = CamHolder.localPosition;

            StartCoroutine(IncreaseHealthOnRest(4.0f));

        }
        private void OnDestroy()
        {
            InputEvents.Current.OnJump -= OnJump;
            InputEvents.Current.OnCrouch -= OnCrouch;
            InputEvents.Current.OnJoyStickDrag -= onJoyStictDrag;
            InputEvents.Current.OnTouchLook -= onTouchLook;
        }
        private void onJoyStictDrag(Vector2 dir)
        {
            m_joyStictDir = dir;
        }
        private void onTouchLook(Vector2 dir)
        {
            m_touchDir = dir;
        }

        private void PlayLandingSound()
        {
            m_audioSource.PlayOneShot(landingSound);
        }
        private void PlayJumpSound()
        {
            m_audioSource.PlayOneShot(jumpingSound);
        }

        private void Update()
        {
            if (m_health <= 0 && !m_isPlayerDead)
            {
                PlayerDeath();
            }
            handlePlayerRotation();
            handleMovementAnimation();

            crouch();
            landing();
        }

        IEnumerator IncreaseHealthOnRest(float checkInterval)
        {
            while (true)
            {
                if (m_addHealth == true)
                {
                    m_health += 10;
                }
                if (m_health > 100)
                {
                    m_health = 100;
                }
                m_addHealth = true;
                yield return new WaitForSeconds(checkInterval);
                DrawHealthStats();
            }
        }

        public void Damage(float damage)
        {
            if (damage > 0)
            {
                m_health -= (int) damage;
                DamageScreenFX.color = DamageScreenColor;
                m_damageScreenColor_temp = DamageScreenColor;
                StartCoroutine(HitFX());
            }

            if (m_health < 0)
            {
                m_health = 0;
            }

            if (m_health <= 0 && !m_isPlayerDead)
            {
                PlayerDeath();
            }

            m_addHealth = false;

            DrawHealthStats();
        }

        IEnumerator HitFX()
        {
            while (DamageScreenFX.color.a > 0)
            {
                m_damageScreenColor_temp = new Color(m_damageScreenColor_temp.r,
                    m_damageScreenColor_temp.g, m_damageScreenColor_temp.b,
                    m_damageScreenColor_temp.a -= DamageScreenFadeSpeed * Time.deltaTime);

                DamageScreenFX.color = m_damageScreenColor_temp;

                yield return new WaitForEndOfFrame();
            }
        }

        private void PlayerDeath()
        {
            if (!m_isPlayerDead)
            {
                CamHolder.LeanMoveLocalY(0,.5f);

                enabled = false;
                m_controllerCollider.enabled = false;
                m_controllerRigidbody.isKinematic = true;

                GameEvents.Current.PlayerDeath();

                m_isPlayerDead = true;

                StartCoroutine(RespawnAfer(5f));
            }
            else
                return;
        }

        private IEnumerator RespawnAfer(float time)
        {
            yield return new WaitForSeconds(time);
            Respawn();
        }

        private void Respawn()
        {
            m_health = 100;

            m_isPlayerDead = false;
            enabled = true;
            m_controllerCollider.enabled = true;
            m_controllerRigidbody.isKinematic = false;

            ///reset player pos and rotation
            /// also collider
            transform.position = m_playerStartPos;
            transform.rotation = Quaternion.identity;

            CamHolder.localPosition = m_initial_holder_pos;
            CamHolder.rotation = Quaternion.identity;

            GameEvents.Current.OnPlayerResPawn();
        }
        private void DrawHealthStats()
        {
            if (HealthUIText != null)
                HealthUIText.text = m_health.ToString();

            if (HealthUISlider != null)
                HealthUISlider.value = m_health;
        }




        private void handleMovementAnimation()
        {
            if (isGrounded())
            {
                if (hasMovement())
                {
                    weaponHolderAnimator.SetBool(AnimatorHash.hash_Walk, true);
                    moveSpeedLocal = MoveSpeed;
                }
                else
                    weaponHolderAnimator.SetBool(AnimatorHash.hash_Walk, false);

                if (m_joyStictDir.y > 0.5f
                    && !IsClimbing &&
                    !IsCrouching &&
                    weaponHolderAnimator.GetBool(AnimatorHash.hash_Walk) == true)
                {
                    IsRunning = true;
                    moveSpeedLocal = RunSpeedMultiplier * MoveSpeed;
                    weaponHolderAnimator.SetBool(AnimatorHash.hash_Run, true);
                }
                else
                {
                    IsRunning = false;
                    weaponHolderAnimator.SetBool(AnimatorHash.hash_Run, false);
                }

            }
            else
            {
                weaponHolderAnimator.SetBool(AnimatorHash.hash_Walk, false);
                weaponHolderAnimator.SetBool(AnimatorHash.hash_Run, false);
            }

            if (IsCrouching)
            {
                moveSpeedLocal = CrouchSpeed;
                weaponHolderAnimator.SetBool(AnimatorHash.hash_Walk, false);
                weaponHolderAnimator.SetBool(AnimatorHash.hash_Run, false);
                if (hasMovement())
                {
                    weaponHolderAnimator.SetBool(AnimatorHash.hash_Crouch, true);
                }
                else
                    weaponHolderAnimator.SetBool(AnimatorHash.hash_Crouch, false);
            }
            else
                weaponHolderAnimator.SetBool(AnimatorHash.hash_Crouch, false);

        }

        void FixedUpdate()
        {
            CharacterMovement();
        }

        private void CharacterMovement()
        {
            var camForward = CamHolder.transform.forward;
            var camRight = CamHolder.transform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            if (IsClimbing)
            {
                IsCrouching = false;
                IsRunning = false;
                m_controllerRigidbody.useGravity = false;

                dirVector = new Vector3(m_joyStictDir.x, 0, m_joyStictDir.y);
                Vector3 moveDirection = (Climbdir) * dirVector.z + camRight * dirVector.x;

                m_controllerRigidbody.MovePosition(transform.position + moveDirection * moveSpeedLocal * Time.deltaTime);
            }
            else
            {
                m_controllerRigidbody.useGravity = true;

                dirVector = new Vector3(m_joyStictDir.x,
                    0, m_joyStictDir.y);

                Vector3 moveDirection = camForward * dirVector.z + camRight * dirVector.x;

                m_controllerRigidbody.MovePosition(transform.position + moveDirection
                    * moveSpeedLocal * Time.deltaTime);
            }
        }

        bool hasMovement()
        {
            if (m_joyStictDir != Vector2.zero)
            {
                return true;
            }
            return false;
        }

        private void handlePlayerRotation()
        {
            Vector2 mouseDelta = new Vector2(
                    m_touchDir.x
                    * _sensitivity.x,
                    m_touchDir.y
                    * _sensitivity.y);

            _smoothMouse = Vector3.Slerp(_smoothMouse,
                        mouseDelta, Time.deltaTime *
                        9f);

            _mouseAbsolute += _smoothMouse;

            _mouseAbsolute.x %= 360;

            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -LimitForXAngle * .5f,
                LimitForXAngle * .5f);

            CamHolder.transform.localRotation =
                Quaternion.Euler(-_mouseAbsolute.y, _mouseAbsolute.x, 0.0f);
        }

        private void crouch()
        {
            if (IsCrouching == true)
            {
                m_controllerCollider.height = Mathf.Lerp(m_controllerCollider.height, CrouchHeight,
                    Time.deltaTime * CrouchSpeedMultiplier);
            }

            else
            {
                Ray ray = new Ray();
                RaycastHit hit;
                ray.origin = transform.position;
                ray.direction = transform.up;
                if (!Physics.Raycast(ray, out hit, 1))
                {
                    m_controllerCollider.height = Mathf.Lerp(m_controllerCollider.height,
                        m_normalHeight,
                    Time.deltaTime * CrouchSpeedMultiplier);

                    IsCrouching = false;
                }
                else
                {
                    IsCrouching = true;
                }

            }

        }

        public float GetVelocityMagnitude()
        {
            return m_controllerRigidbody.velocity.magnitude;
        }

        private void OnJump(bool state)
        {

            if (state && isGrounded() && !IsCrouching)
            {
                PlayJumpSound();
                m_controllerRigidbody.AddForce(transform.up * JumpForce, ForceMode.VelocityChange);
            }
            if (state && IsCrouching)
            {
                IsCrouching = !IsCrouching;
            }

        }

        private void OnCrouch(bool state)
        {
            if (state)
            {
                IsCrouching = !IsCrouching;
            }
        }

        private bool isGrounded()
        {
            RaycastHit hit;
            return Physics.SphereCast(transform.position,m_controllerCollider.radius ,-transform.up,out hit ,distanceToGround + 0.1f, GroundLayerMask);
        }

        private void landing()
        {
            if (!isGrounded())
            {
                inAirTime += Time.deltaTime;
            }
            else
            {
                if (inAirTime > 0.5f)
                {
                    weaponHolderAnimator.Play(AnimatorHash.hash_Landing);
                    PlayLandingSound();
                }
                inAirTime = 0;
            }
        }
    }
}
