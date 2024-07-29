using UnityEngine;

public class Spawner : MonoBehaviour
{
    public ObjectPool _pool;
    public float SpawnRate = 1.5f;
    public float MinHeight = -1f;
    public float MaxHeight = 1f;

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), 2, SpawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }

    private void Spawn()
    {
        var pipes = _pool.GetObject();
        Vector3 spawnPosition = transform.position + Vector3.up * Random.Range(MinHeight, MaxHeight);
        pipes.transform.position = spawnPosition;
    }

    public void ReturnBackObjectToPool(GameObject obj)
    {
        _pool.ReturnBack(obj);
    }
}
