using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("무기 Prefab")]
    public GameObject bulletPrefab;
    public GameObject netPrefab;

    [Header("발사 위치")]
    public Transform firePoint;

    // false = Bullet
    // true = Net
    public bool isNet = false;


    // 🔄 Bullet ↔ Net 교체
    public void ChangeWeapon()
    {
        isNet = !isNet;

        if (isNet)
        {
            Debug.Log("Net 선택");
        }
        else
        {
            Debug.Log("Bullet 선택");
        }
    }


    // 🎯 현재 선택되어 있는 무기를 생성
    public void Fire()
    {
        if (isNet)
        {
            Instantiate(netPrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}