using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHide : MonoBehaviour
{
    public GameObject show;
    public bool hideWhenControlsOnline;
    public bool showWhenControlsOnline;
    public bool startShowing;

    // Start is called before the first frame update
    void Start()
    {
        if (!startShowing)
            show.SetActive(false);
    }

    private void Update()
    {
        if (PlayerMovement.controlsReady)
        {
            if (hideWhenControlsOnline)
                show.SetActive(false);
            else if (showWhenControlsOnline)
                show.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (!hideWhenControlsOnline || !PlayerMovement.controlsReady))
        {
            show.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            show.SetActive(false);
        }
    }
}
