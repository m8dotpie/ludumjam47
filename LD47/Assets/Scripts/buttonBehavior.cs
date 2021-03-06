﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonBehavior : MonoBehaviour
{
    public GameObject buttonPointer;
    private RectTransform buttonTransform;
    public GameObject hiddenScreen;
    public GameObject showingScreen;
    public GameObject drill;
    public void buttonPushed()
    {
        hiddenScreen.transform.position = new Vector3(hiddenScreen.transform.position.x, hiddenScreen.transform.position.y, 0);
        showingScreen.transform.position = new Vector3(showingScreen.transform.position.x, showingScreen.transform.position.y, 100);
        drill.SetActive(false);
    }
    public void PointerEnter()
    {
        Debug.Log("1");
        buttonTransform = GetComponent<RectTransform>();
        buttonPointer.SetActive(true);
        buttonPointer.transform.position = new Vector3(this.transform.position.x - buttonTransform.rect.width/5, this.transform.position.y - buttonTransform.rect.height/4, buttonPointer.transform.position.z);
    }

    public void PointerOut()
    {
        Debug.Log("1");
        buttonPointer.SetActive(false);
    }
}
