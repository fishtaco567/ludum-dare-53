using System.Collections.Generic;
using System.Text;
using Progfish.Utils;
using Unity.Mathematics;
using UnityEngine;

namespace Progfish.Boat.Graphics {

    public class WaterSimulation : Singleton<WaterSimulation> {

        private struct Splash {
            public float2 position;
            public float2 size;
            public float angle;
            public float initialIntensity;
            public SplashKind kind;
        }

        const int MAX_ZONES = 128;

        public Material rtMat;
        public CustomRenderTexture rt;
        public Renderer renderObject;

        private List<Splash> splashList;

        private List<CustomRenderTextureUpdateZone> zones;
        private CustomRenderTextureUpdateZone[] zonesArray;

        private List<float4> intensities;

        private CustomRenderTextureUpdateZone simulate;

        private float scaleFactor;
        private float2 offset;

        private GraphicsBuffer curBuffer;

        public float size;
        public float intensity;

        private void Awake() {
            splashList = new List<Splash>();
            zones = new List<CustomRenderTextureUpdateZone>();
            zonesArray = new CustomRenderTextureUpdateZone[1];

            curBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 1, 16);

            intensities = new List<float4>();

            simulate = new CustomRenderTextureUpdateZone() {
                updateZoneCenter = new float3(0.5f, 0.5f, 0f),
                updateZoneSize = new float3(1f, 1f, 0f),
                rotation = 0,
                passIndex = 0,
                needSwap = true,
            };

            rt = new CustomRenderTexture(2048, 2048, RenderTextureFormat.ARGBHalf);
            rt.initializationColor = Color.black;
            rt.initializationMode = CustomRenderTextureUpdateMode.OnDemand;
            rt.updateMode = CustomRenderTextureUpdateMode.OnDemand;
            rt.material = rtMat;
            rt.doubleBuffered = true;

            renderObject.material.mainTexture = rt;

            rt.Initialize();
        }

        private void Start() {
            SetupScaleAndOffset();
        }

        private void Update() {
            DebugInteraction();

            CopySplashesToUpdateZones();

            CopyToArray();

            CopyToSimulator();
            UpdateTexture();
        }

        public override void OnDestroy() {
            base.OnDestroy();

            zones.Clear();
            for(int i = 0; i < MAX_ZONES; i++) {
                zones.Add(new CustomRenderTextureUpdateZone() {
                    updateZoneCenter = new float3(i, 0, 0),
                    updateZoneSize = new float3(i + MAX_ZONES, 0, 0),
                    passIndex = i,
                    rotation = i + MAX_ZONES,
                    needSwap = i % 2 == 0 ? true : false
                });
            }
            CopyToSimulator();

            curBuffer.Dispose();
        }

        public void StartSplash(SplashKind kind, float2 position, float2 size, float angle, float intensity) {
            splashList.Add(new Splash {
                kind = kind,
                position = position,
                size = size,
                angle = angle,
                initialIntensity = intensity,
            });
        }

        private void DebugInteraction() {
            var mainCam = Camera.main;

            if(UnityEngine.Input.GetMouseButton(0)) {
                var point = mainCam.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                StartSplash(SplashKind.GeneralSplash, new float2(point.x, point.z), new float2(size, size) * UnityEngine.Random.Range(.6f, 3.2f), 0, intensity / 5f);
            }
        }

        private void CopyToSimulator() {
            rt.SetUpdateZones(zones.ToArray());

            if(intensities.Count != 0) {
                if(!curBuffer.IsValid() || curBuffer.count != intensities.Count) {
                    curBuffer.Dispose();
                    curBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, intensities.Count, 16);
                }
                curBuffer.SetData(intensities);
                rt.material.SetBuffer("splashIntensity", curBuffer);
            }

            splashList.Clear();
        }

        private void SetupScaleAndOffset() {
            var renderObjectSize = renderObject.bounds.size;
            var max = math.max(renderObjectSize.x, math.max(renderObjectSize.y, renderObjectSize.z));
            scaleFactor = 1 / max;

            var renderObjectPos = renderObject.bounds.min;
            offset = new float2(-renderObjectPos.x, -renderObjectPos.y);
        }

        private void CopySplashesToUpdateZones() {
            zones.Clear();
            intensities.Clear();

            foreach(var splash in splashList) {
                CopySplashToUpdateZone(in splash);
            }

            zones.Add(simulate);
            zones.Add(simulate);
            zones.Add(simulate);
            zones.Add(simulate);
            zones.Add(simulate);
            zones.Add(simulate);
            zones.Add(simulate);
            zones.Add(simulate);
        }

        private void CopySplashToUpdateZone(in Splash splash) {
            var pos = (new float2(-splash.position.x, splash.position.y) + offset) * scaleFactor;
            var size = splash.size * scaleFactor;

            zones.Add(new CustomRenderTextureUpdateZone() {
                updateZoneCenter = new float3(pos.x, pos.y, 0), 
                updateZoneSize = new float3(size.x, size.y, 0),
                rotation = splash.angle,
                passIndex = (int) splash.kind,
                needSwap = false,
            });

            intensities.Add(new float4(splash.initialIntensity));
        }

        private void CopyToArray() {
            if(zonesArray.Length != zones.Count) {
                zonesArray = new CustomRenderTextureUpdateZone[zones.Count];
            }

            zones.CopyTo(zonesArray);
        }

        private void UpdateTexture() {
            rt.Update();
        }

        private void ResetSim() {
            rt.Initialize();
        }

    }

    public enum SplashKind {
        BoatSplash = 1,
        GeneralSplash = 2,
    }

}