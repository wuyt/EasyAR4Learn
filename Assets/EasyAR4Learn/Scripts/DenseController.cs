using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using easyar;

public class DenseController : MonoBehaviour
{
    public GameObject prefab;

    public DenseSpatialMapBuilderFrameFilter dense;

    void Start()
    {
        dense.MeshColor = Color.gray;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            var launchPoint = Camera.main.transform;
            var ball = Instantiate(prefab, launchPoint.position, launchPoint.rotation);
            var rigid = ball.GetComponent<Rigidbody>();
            rigid.velocity = Vector3.zero;
            rigid.AddForce(ray.direction * 15f + Vector3.up * 5f);
        }
    }

    public void RenderMesh(bool show)
    {
        if (!dense)
        {
            return;
        }
        dense.RenderMesh = show;
    }

    public void TransparentMesh(bool trans)
    {
        if (!dense)
        {
            return;
        }
        if (trans)
        {
            dense.MeshColor = Color.gray;
        }
        else
        {
            dense.MeshColor = Color.clear;
        }

    }
}
