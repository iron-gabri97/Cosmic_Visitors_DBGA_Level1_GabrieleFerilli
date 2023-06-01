using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private List<GameObject> backgroundImages = new List<GameObject>();

    Camera camera;

    private float BackgroundSpeed = 1.0f;

    private float backgroundImageHeight;
    private float boundTop;
    private float boundBottom;

    private void Awake()
    {
        camera = Camera.main;

        SpriteRenderer backgroundSR = backgroundImages[0].GetComponent<SpriteRenderer>();
        backgroundImageHeight = backgroundSR.sprite.bounds.size.y;

        boundTop = Helper.GetScreenBoundTop(camera);
        boundBottom = Helper.GetScreenBoundBottom(camera);
    }

    private void Update()
    {
        for (int i = 0; i < backgroundImages.Count; i++)
        {
            backgroundImages[i].transform.position += BackgroundSpeed * Time.deltaTime * Vector3.down;

            if (backgroundImages[i].transform.position.y < boundBottom - backgroundImageHeight / 2)
            {
                backgroundImages[i].transform.position = new Vector3(backgroundImages[i].transform.position.x,
                    boundTop + backgroundImageHeight / 2,
                    backgroundImages[i].transform.position.z);
            }
        }
    }
}
