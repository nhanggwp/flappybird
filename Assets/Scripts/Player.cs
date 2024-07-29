using UnityEngine;

public class Player : MonoBehaviour
{
    public float Gravity = -9.8f;
    public float Strength = 5f;
    public Sprite[] _sprites;
    public AudioSource AudioSource;
    public AudioClip Flap;
    public AudioClip Score;
    public AudioClip Crash;

    private int _spriteIndex;
    private Vector3 _direction;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        var position = transform.position;
        position.y = 0f;
        transform.position = position;
        _direction = Vector3.zero;
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.UpArrow) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            AudioSource.PlayOneShot(Flap);
            _direction = Vector3.up * Strength;
        }

        _direction.y += Gravity * Time.deltaTime;
        transform.position += _direction * Time.deltaTime;

        var angle = Mathf.Clamp(_direction.y * 10, -90, 30);
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void AnimateSprite()
    {
        _spriteIndex++;
        if (_spriteIndex >= _sprites.Length)
        {
            _spriteIndex = 0;
        }
        _spriteRenderer.sprite = _sprites[_spriteIndex];
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Obstacle":
                AudioSource.PlayOneShot(Crash);
                FindObjectOfType<GameManager>().GameOver();
                break;
            case "Scoring":
                AudioSource.PlayOneShot(Score);
                FindObjectOfType<GameManager>().IncreaseScore();
                break;
            default:
                break;
        }
    }
}
