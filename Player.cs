using UnityEngine;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    [Header("이동")]
    public float moveSpeed = 3f;
    public Transform head;

    [Header("무기")]
    public Gun gun;

    [Header("애니메이션")]
    public Animator animator;

    [Header("소리")]
    public AudioSource audioSource;
    public AudioClip footstepSound;
    public AudioClip dieSound;

    [Header("상태")]
    public bool isDead = false;

    private bool lastX = false;
    private bool lastY = false;


    void Update()
    {
        if (isDead)
            return;

        Move();
        ChangeWeapon();
        Fire();
    }


    // 🕹️ 왼쪽 조이스틱으로 Player 이동 + 이동 중 발소리
    void Move()
    {
        InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystick);

        // 머리가 바라보는 방향을 이동 기준으로 사용
        Vector3 forward = head.forward;
        Vector3 right = head.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * joystick.y + right * joystick.x;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;


        // 움직이는 동안 발소리
        if (moveDirection.magnitude > 0.1f)
        {
            PlayFootstep();
        }
        else
        {
            StopFootstep();
        }
    }


    // 👣 이동 중 발소리 재생
    void PlayFootstep()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = footstepSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }


    // 👣 멈추면 발소리 정지
    void StopFootstep()
    {
        if (audioSource.clip == footstepSound)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
    }


    // 🕸💣 Y 버튼으로 Bullet ↔ Net 무기 교체
    void ChangeWeapon()
    {
        InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool yButton);

        if (yButton && !lastY)
        {
            gun.ChangeWeapon();
            animator.SetTrigger("ChangeWeapon");
        }

        lastY = yButton;
    }


    // 🕸💣 X 버튼으로 현재 선택된 무기 발사
    void Fire()
    {
        InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        leftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool xButton);

        if (xButton && !lastX)
        {
            gun.Fire();
            animator.SetTrigger("Fire");
        }

        lastX = xButton;
    }


    // 💀 드론 공격에 맞았을 때 Player 사망
    public void Die()
    {
        if (isDead)
            return;

        isDead = true;

        StopFootstep();
        animator.SetTrigger("Die");
        audioSource.PlayOneShot(dieSound);
    }
}