using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("총알 설정")]
    public float speed = 100f;
    public float maxDistance = 500f;

    private Vector3 startPosition;


    // 🎯 총알이 생성된 최초 위치 저장
    void Start()
    {
        startPosition = transform.position;
    }


    // 🎯 발사 방향으로 이동하고 최대 500m에서 제거
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        float distance = Vector3.Distance(startPosition, transform.position);

        if (distance >= maxDistance)
        {
            Destroy(gameObject);
        }
    }


    // 💥 Drone과 충돌하면 Drone과 Bullet 제거
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drone"))
        {
            Debug.Log("Drone 명중");
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}