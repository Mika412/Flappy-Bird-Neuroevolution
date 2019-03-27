using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatePipeHeight : MonoBehaviour {

    public GameObject top;
    public GameObject bottom;
    public GameObject medium;

    public float maxHeight = 8f;

    public float heightBottom;

	void Start () {
        heightBottom = Random.Range(1, maxHeight);
        top.transform.localScale = new Vector3(top.transform.localScale.x,heightBottom ,top.transform.localScale.z);
        bottom.transform.localScale = new Vector3(top.transform.localScale.x, maxHeight - heightBottom,top.transform.localScale.z);
        Vector3 vec = medium.transform.localPosition;
        vec.y = bottom.transform.localPosition.y + 2 * bottom.transform.localScale.y + 2.5f;
        medium.transform.localPosition = vec;
    }
}
