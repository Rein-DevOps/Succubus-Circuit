/**
 * File : GateSpa wner.cs
 * Desc : 게이트 생성
 * Functions : SpawGate() - 매개변수의 위치에 게이트 생성
 * 
 */

using UnityEngine;

public class GateSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject gatePrefab;

    public void SpawnGate(Transform tileTransform)
    {
        Instantiate(gatePrefab, tileTransform.position, Quaternion.identity);
    }
}
