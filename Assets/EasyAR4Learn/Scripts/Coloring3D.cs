using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using easyar;

public class Coloring3D : MonoBehaviour
{
    public CameraImageRenderer CameraRenderer;
    private Material material;
    private ImageTargetController imageTarget;
    private RenderTexture renderTexture;
    private Optional<bool> freezed;
    private RenderTexture freezedTexture;

    void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        imageTarget = GetComponentInParent<ImageTargetController>();
        CameraRenderer.RequestTargetTexture((camera, texture) =>
         {
             renderTexture = texture;
         });
        imageTarget.TargetFound += () =>
       {
           if (freezed.OnNone)
           {
               freezed = false;
           }
       };
    }

    public void Freeze()
    {
        freezed = true;
        if (freezedTexture)
        {
            Destroy(freezedTexture);
        }
        if (renderTexture)
        {
            freezedTexture = new RenderTexture(renderTexture.width, renderTexture.height, 0);
            Graphics.Blit(renderTexture, freezedTexture);
        }
        material.SetTexture("_MainTex", freezedTexture);
    }

    public void Thaw()
    {
        if (freezedTexture)
        {
            Destroy(freezedTexture);
        }
    }


    void Update()
    {
        if (freezed.OnNone || freezed.Value || imageTarget.Target == null)
        {
            return;
        }
        var halfWidth = 0.5f;
        var halfHeight = 0.5f / imageTarget.Target.aspectRatio();
        Matrix4x4 points = Matrix4x4.identity;
        Vector3 targetAnglePoint1 = imageTarget.transform.TransformPoint(new Vector3(-halfWidth, halfHeight, 0));
        Vector3 targetAnglePoint2 = imageTarget.transform.TransformPoint(new Vector3(-halfWidth, -halfHeight, 0));
        Vector3 targetAnglePoint3 = imageTarget.transform.TransformPoint(new Vector3(halfWidth, halfHeight, 0));
        Vector3 targetAnglePoint4 = imageTarget.transform.TransformPoint(new Vector3(halfWidth, -halfHeight, 0));
        points.SetRow(0, new Vector4(targetAnglePoint1.x, targetAnglePoint1.y, targetAnglePoint1.z, 1f));
        points.SetRow(1, new Vector4(targetAnglePoint2.x, targetAnglePoint2.y, targetAnglePoint2.z, 1f));
        points.SetRow(2, new Vector4(targetAnglePoint3.x, targetAnglePoint3.y, targetAnglePoint3.z, 1f));
        points.SetRow(3, new Vector4(targetAnglePoint4.x, targetAnglePoint4.y, targetAnglePoint4.z, 1f));
        material.SetMatrix("_UvPints", points);
        material.SetMatrix("_RenderingViewMatrix", Camera.main.worldToCameraMatrix);
        material.SetMatrix("_RenderingProjectMatrix", GL.GetGPUProjectionMatrix(Camera.main.projectionMatrix, false));
        material.SetTexture("_MainTex", renderTexture);
    }

    void OnDestroy()
    {
        if (freezedTexture) { Destroy(freezedTexture); }
    }
}
