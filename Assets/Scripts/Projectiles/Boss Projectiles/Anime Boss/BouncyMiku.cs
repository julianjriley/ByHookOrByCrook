using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Note: Ground needs a CHILD gameobject with a box collider with a Layer set to MikuWall.
/// </summary>
public class BouncyMiku : Projectile
{
    private Vector3 _direction;
    [SerializeField] private GameObject _sprite;
    [SerializeField] private EventReference mikuSound;

    [Header("Miku Bounds")]
    [SerializeField, Tooltip("Horizontal distance from center that counts as bounds.")]
    private float _horBound;
    [SerializeField, Tooltip("Vertical distance up from center that counts as bounds.")]
    private float _topBound;
    [SerializeField, Tooltip("Vertical distance down from center that counts as bounds.")]
    private float _lowBound;

    [Header("Color Swapping!")]
    [SerializeField, Tooltip("Sprite to swap colors on.")]
    SpriteRenderer _colorSprite;
    [SerializeField, Tooltip("List of colors to randomly swap between.")]
    Color[] _colorOptions;

    private Transform _centerScreenPos;

    override protected void Start()
    {
        base.Start();

        // randomize initial direction
        _direction.x = Random.Range(0, 2) == 0 ? -1 : 1;
        _direction.y = Random.Range(0, 2) == 0 ? -1 : 1;

        _rb.velocity = _direction * _speed;

        _centerScreenPos = GameObject.Find("Center Screen").transform;

        // randomly swap to new color
        SwapColor(_colorOptions);
    }

    protected override void FixedUpdate()
    {
        // flip vertical
        if ((transform.position.y > _centerScreenPos.position.y + _topBound && _rb.velocity.y > 0)
            || (transform.position.y < _centerScreenPos.position.y - _lowBound && _rb.velocity.y < 0))
        {
            _direction.y *= -1;
            _rb.velocity = _direction * _speed;

            // bounce sound
            SoundManager.Instance.PlayOneShot(mikuSound, gameObject.transform.position);

            // randomly swap to new color
            SwapColor(_colorOptions);
        }

        // flip horizontal
        if ((transform.position.x > _centerScreenPos.position.x + _horBound && _rb.velocity.x > 0) 
            || (transform.position.x < _centerScreenPos.position.x - _horBound && _rb.velocity.x < 0))
        {
            _direction.x *= -1;
            _rb.velocity = _direction * _speed;

            // bounce sound
            SoundManager.Instance.PlayOneShot(mikuSound, gameObject.transform.position);

            // randomly swap to new color
            SwapColor(_colorOptions);
        }
    }

    /// <summary>
    /// Swaps miku to a random color that is different from miku's current color
    /// </summary>
    private void SwapColor(Color[] colorOptions)
    {
        if (colorOptions.Length == 0)
            throw new System.Exception("Unable to use SwapColor function for Miku with no colors. stop that.");

        int rand = Random.Range(0, colorOptions.Length);
        Color selectedColor = colorOptions[rand];

        // same color detected, try again
        if (ColorsEqual(selectedColor, _colorSprite.color))
        {
            List<Color> newList = new List<Color>();
            for (int i = 0; i < colorOptions.Length; i++)
            {
                // add all but duplicate
                if (i != rand)
                    newList.Add(colorOptions[i]);
            }

            SwapColor(newList.ToArray());
        }
        else // unique color found - so swap it
            _colorSprite.color = selectedColor;
    }

    /// <summary>
    /// Returns true ONLY if all color components (r, g, b, a) are equal.
    /// </summary>
    private bool ColorsEqual(Color color1, Color color2)
    {
        return color1.r == color2.r && color1.g == color2.g && color1.b == color2.b && color1.a == color2.a;
    }
}
