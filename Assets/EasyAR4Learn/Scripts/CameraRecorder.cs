//================================================================================================================================
//
//  Copyright (c) 2015-2019 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using easyar;
using UnityEngine;

namespace VideoRecording
{
    public class CameraRecorder : MonoBehaviour
    {
        private VideoRecorder videoRecorder;
        private Material externalMaterial;
        private RenderTexture rt;

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination);
            if (videoRecorder)
            {
                if (externalMaterial)
                {
                    if (rt && (rt.width != source.width || rt.height != source.height))
                    {
                        Destroy(rt);
                    }
                    if (!rt)
                    {
                        rt = new RenderTexture(source.width, source.height, 0);
                    }
                    Graphics.Blit(source, rt, externalMaterial);
                    videoRecorder.RecordFrame(rt);
                }
                else
                {
                    videoRecorder.RecordFrame(source);
                }
            }
        }

        public void Setup(VideoRecorder recorder, Material material)
        {
            videoRecorder = recorder;
            externalMaterial = material;
        }

        public void Destroy()
        {
            if (rt)
            {
                Destroy(rt);
            }
            Destroy(this);
        }
    }
}
