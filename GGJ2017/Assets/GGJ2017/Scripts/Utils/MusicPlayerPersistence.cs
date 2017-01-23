using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerPersistence : MonoBehaviour {

    public static GameObject instance;
    static string myObjName;

	// Use this for initialization
	void Start()
	{
        if (instance != null)
        {
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            instance = this.gameObject;
            instance.GetComponent<AudioSource>().Play();
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
	}
}
