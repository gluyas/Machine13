using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    public int Health = 3;

    private static Color _damageFlashColor = Color.yellow;
    private static float _damageFlashTime = 0.2f;
    
    private Renderer[] _renderers;
    private Color[] _colors;
    
    private void Awake()
    {
        _renderers = transform.root.gameObject.GetComponentsInChildren<Renderer>();
        _colors = new Color[_renderers.Length];
        for (var i = 0; i < _renderers.Length; i++)
        {
            _colors[i] = _renderers[i].material.color;
        }
    }

    public void Damage(int dmg)
    {
        Health -= dmg;
        StartCoroutine(this.Flash());
    }

    private IEnumerator Flash()
    {
        for (var i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.color = _damageFlashColor;
        }
        yield return new WaitForSeconds(_damageFlashTime);
        for (var i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.color = _colors[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            if (transform.parent == null)
                Destroy(gameObject, 0.0f);
            if (transform.parent != null) { 
                if (transform.parent.parent != null)
                    Destroy(transform.parent.parent.gameObject, 0.0f);

                if (transform.parent != null)
                    Destroy(transform.parent.gameObject, 0.0f);
            }
        }
    }
}
