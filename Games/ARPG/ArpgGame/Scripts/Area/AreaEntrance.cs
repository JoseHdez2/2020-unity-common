﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaEntrance : MonoBehaviour
{
    public string sceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == _Constants.TagPlayer)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
