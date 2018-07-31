using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveIcon : MonoBehaviour {

    public float speed = 0;

    //Time in seconds save icon last;
    public float timer = 2;

    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(ShowSave());
    }

    public IEnumerator ShowSave()
    {
        image.enabled = true;
        transform.localRotation = Quaternion.identity;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.Rotate(-Vector3.forward * speed * Time.deltaTime);

            yield return null;
        }

        timer = 1;
        image.enabled = false;
    }
}
