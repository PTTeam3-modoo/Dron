using UnityEngine;

public class Net : MonoBehaviour
{
    [Header("그물 설정")]
    public float speed = 50f;
    public float maxDistance = 1000f;

    private Vector3 startPosition;


    // 🕸 그물이 생성된 최초 위치 저장
    void Start()
    {
        startPosition = transform.position;
    }


    // 🕸 발사 방향으로 이동하고 최대 1km에서 제거
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        float distance = Vector3.Distance(startPosition, transform.position);

        if (distance >= maxDistance)
        {
            Destroy(gameObject);
        }
    }


    // 🕸 Drone과 충돌하면 그물에 포획
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drone"))
        {
            Debug.Log("Drone 포획");

            Rigidbody droneRb = other.GetComponent<Rigidbody>();

            // Rigidbody가 있는 경우 움직임 정지
            if (droneRb != null)
            {
                droneRb.linearVelocity = Vector3.zero;
                droneRb.angularVelocity = Vector3.zero;
                droneRb.isKinematic = true;
            }

            // Drone을 Net의 자식으로 만들어 같이 움직이게 함
            other.transform.SetParent(transform);
        }
    }
}