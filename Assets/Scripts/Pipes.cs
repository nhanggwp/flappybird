using UnityEngine;

public class Pipes : MonoBehaviour
{
    public float Speed = 5f;
    private float _leftEdge;

    private void Start()
    {
        _leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
    }

    private void Update()
    {
        transform.position += Vector3.left * Speed * Time.deltaTime;

        if (transform.position.x < _leftEdge)
        {
            FindObjectOfType<Spawner>().ReturnBackObjectToPool(gameObject);
        }
    }
}
