using System.Collections.Generic;
using UnityEngine;

namespace WordVista.Environment
{
    [ExecuteAlways]
    public class WorldEnvironmentSetup : MonoBehaviour
    {
        [Header("Active World")]
        [SerializeField] private int worldId = 1;

        [Header("Stylized Materials / Colors")]
        [SerializeField] private Material terrainMaterial;
        [SerializeField] private Material waterMaterial;
        [SerializeField] private Material foliageMaterial;
        [SerializeField] private Material stoneMaterial;
        [SerializeField] private Material crystalMaterial;

        [Header("Lighting")]
        [SerializeField] private Light directionalSun;

        private GameObject _environmentRoot;

        private void Start()
        {
            BuildEnvironment(worldId);
        }

        public void BuildEnvironment(int targetWorldId)
        {
            worldId = targetWorldId;

            if (_environmentRoot != null)
            {
                if (Application.isPlaying) Destroy(_environmentRoot);
                else DestroyImmediate(_environmentRoot);
            }

            _environmentRoot = new GameObject($"World_{worldId}_Environment");
            _environmentRoot.transform.SetParent(transform, false);

            switch (worldId)
            {
                case 1: BuildGreenValley(); break;
                case 2: BuildMysticForest(); break;
                case 3: BuildCrystalLake(); break;
                case 4: BuildDesertKingdom(); break;
                case 5: BuildSnowValley(); break;
                case 6: BuildTropicalIsland(); break;
                case 7: BuildSkyKingdom(); break;
                case 8: BuildAuroraWorld(); break;
                default: BuildGreenValley(); break;
            }
        }

        private void BuildGreenValley()
        {
            SetLighting(new Color(1f, 0.96f, 0.85f), 1.2f, new Color(0.45f, 0.72f, 0.48f), new Color(0.6f, 0.82f, 0.95f));

            // Main rolling ground
            CreatePlane(Vector3.zero, new Vector3(80, 1, 80), new Color(0.32f, 0.65f, 0.28f));

            // Low-poly rolling hills
            CreateMound(new Vector3(-18, 0, 15), new Vector3(14, 8, 14), new Color(0.28f, 0.58f, 0.24f));
            CreateMound(new Vector3(20, 0, 18), new Vector3(16, 10, 16), new Color(0.28f, 0.58f, 0.24f));

            // Winding river brook
            CreatePlane(new Vector3(0, 0.05f, 5), new Vector3(8, 1, 60), new Color(0.25f, 0.65f, 0.85f, 0.8f));

            // Stone arch bridge across the river
            CreateBridge(new Vector3(0, 0.8f, 4), new Vector3(9, 0.6f, 3.5f), new Color(0.65f, 0.62f, 0.58f));

            // Stylized trees
            CreateTree(new Vector3(-8, 0, 8), new Color(0.22f, 0.55f, 0.25f));
            CreateTree(new Vector3(-12, 0, 12), new Color(0.18f, 0.48f, 0.22f));
            CreateTree(new Vector3(10, 0, 9), new Color(0.24f, 0.58f, 0.26f));
            CreateTree(new Vector3(14, 0, 15), new Color(0.2f, 0.5f, 0.22f));

            // Wildflower clusters
            CreateFlowerCluster(new Vector3(-4, 0.1f, 2), new Color(0.95f, 0.35f, 0.45f));
            CreateFlowerCluster(new Vector3(5, 0.1f, 3), new Color(0.98f, 0.85f, 0.2f));
            CreateFlowerCluster(new Vector3(2, 0.1f, -2), new Color(0.85f, 0.45f, 0.95f));
        }

        private void BuildMysticForest()
        {
            SetLighting(new Color(0.7f, 0.85f, 1f), 0.8f, new Color(0.15f, 0.35f, 0.25f), new Color(0.1f, 0.15f, 0.25f));
            CreatePlane(Vector3.zero, new Vector3(80, 1, 80), new Color(0.12f, 0.28f, 0.16f));

            // Ancient glowing mossy trees
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f * Mathf.Deg2Rad;
                float dist = 12f + (i % 3) * 4f;
                Vector3 pos = new Vector3(Mathf.Cos(angle) * dist, 0, Mathf.Sin(angle) * dist + 10f);
                CreateTree(pos, new Color(0.08f, 0.32f, 0.22f), 1.5f);
            }

            // Glowing magical mushrooms & firefly lights
            CreateGlowOrb(new Vector3(-4, 1.2f, 6), new Color(0.3f, 0.95f, 0.6f));
            CreateGlowOrb(new Vector3(6, 1.5f, 8), new Color(0.4f, 0.7f, 1f));
        }

        private void BuildCrystalLake()
        {
            SetLighting(new Color(0.9f, 0.95f, 1f), 1.3f, new Color(0.2f, 0.5f, 0.65f), new Color(0.7f, 0.88f, 0.98f));
            CreatePlane(Vector3.zero, new Vector3(80, 1, 80), new Color(0.85f, 0.82f, 0.72f)); // sandy shore

            // Lake water plane
            CreatePlane(new Vector3(0, 0.1f, 10), new Vector3(70, 1, 50), new Color(0.18f, 0.65f, 0.82f, 0.85f));

            // Floating crystalline clusters
            CreateCrystal(new Vector3(-6, 0.5f, 8), new Color(0.4f, 0.85f, 1f));
            CreateCrystal(new Vector3(7, 0.8f, 11), new Color(0.6f, 0.95f, 0.85f));
            CreateCrystal(new Vector3(0, 0.3f, 14), new Color(0.75f, 0.55f, 1f));
        }

        private void BuildDesertKingdom()
        {
            SetLighting(new Color(1f, 0.88f, 0.65f), 1.4f, new Color(0.65f, 0.45f, 0.25f), new Color(0.95f, 0.85f, 0.7f));
            CreatePlane(Vector3.zero, new Vector3(80, 1, 80), new Color(0.88f, 0.72f, 0.42f)); // golden sand

            // Sand dunes
            CreateMound(new Vector3(-15, 0, 15), new Vector3(18, 6, 22), new Color(0.84f, 0.68f, 0.38f));
            CreateMound(new Vector3(18, 0, 18), new Vector3(20, 8, 25), new Color(0.84f, 0.68f, 0.38f));

            // Sandstone ancient columns/ruins
            CreateColumn(new Vector3(-5, 0, 8), 4f, new Color(0.75f, 0.6f, 0.4f));
            CreateColumn(new Vector3(6, 0, 9), 5f, new Color(0.75f, 0.6f, 0.4f));
            CreateColumn(new Vector3(9, 0, 12), 3f, new Color(0.75f, 0.6f, 0.4f));
        }

        private void BuildSnowValley()
        {
            SetLighting(new Color(0.85f, 0.92f, 1f), 1.1f, new Color(0.5f, 0.65f, 0.8f), new Color(0.8f, 0.88f, 0.95f));
            CreatePlane(Vector3.zero, new Vector3(80, 1, 80), new Color(0.92f, 0.95f, 0.98f)); // crisp snow

            // Snow peaks
            CreateMound(new Vector3(-20, 0, 20), new Vector3(16, 12, 16), new Color(0.88f, 0.92f, 0.96f));
            CreateMound(new Vector3(22, 0, 22), new Vector3(18, 14, 18), new Color(0.88f, 0.92f, 0.96f));

            // Frozen pine trees
            CreateTree(new Vector3(-8, 0, 8), new Color(0.25f, 0.45f, 0.45f));
            CreateTree(new Vector3(10, 0, 10), new Color(0.28f, 0.48f, 0.48f));
        }

        private void BuildTropicalIsland()
        {
            SetLighting(new Color(1f, 0.92f, 0.8f), 1.3f, new Color(0.2f, 0.65f, 0.55f), new Color(0.45f, 0.85f, 0.95f));
            CreatePlane(Vector3.zero, new Vector3(80, 1, 80), new Color(0.9f, 0.85f, 0.68f)); // warm sand
            CreatePlane(new Vector3(0, 0.05f, 8), new Vector3(80, 1, 60), new Color(0.12f, 0.72f, 0.82f, 0.85f)); // turquoise ocean

            // Tropical palm trees
            CreateTree(new Vector3(-7, 0, 6), new Color(0.2f, 0.65f, 0.28f));
            CreateTree(new Vector3(8, 0, 7), new Color(0.18f, 0.62f, 0.25f));
        }

        private void BuildSkyKingdom()
        {
            SetLighting(new Color(1f, 0.95f, 0.9f), 1.4f, new Color(0.55f, 0.65f, 0.85f), new Color(0.75f, 0.85f, 1f));

            // Floating island platform
            CreatePlane(new Vector3(0, 0, 5), new Vector3(40, 4, 35), new Color(0.42f, 0.68f, 0.38f));

            // Ancient marble sky temple pillars
            CreateColumn(new Vector3(-7, 0, 8), 6f, new Color(0.95f, 0.95f, 0.92f));
            CreateColumn(new Vector3(7, 0, 8), 6f, new Color(0.95f, 0.95f, 0.92f));
            CreateColumn(new Vector3(-7, 0, 15), 6f, new Color(0.95f, 0.95f, 0.92f));
            CreateColumn(new Vector3(7, 0, 15), 6f, new Color(0.95f, 0.95f, 0.92f));
        }

        private void BuildAuroraWorld()
        {
            SetLighting(new Color(0.55f, 0.4f, 0.85f), 0.7f, new Color(0.15f, 0.1f, 0.35f), new Color(0.08f, 0.05f, 0.18f));
            CreatePlane(Vector3.zero, new Vector3(80, 1, 80), new Color(0.12f, 0.08f, 0.22f)); // dark celestial stone

            // Glowing iridescent crystals
            CreateCrystal(new Vector3(-6, 0.8f, 8), new Color(0.35f, 0.95f, 0.85f));
            CreateCrystal(new Vector3(7, 1.2f, 10), new Color(0.95f, 0.35f, 0.85f));
            CreateCrystal(new Vector3(0, 1.5f, 13), new Color(0.45f, 0.55f, 1f));
        }

        private void SetLighting(Color lightColor, float intensity, Color ambientColor, Color skyColor)
        {
            if (directionalSun != null)
            {
                directionalSun.color = lightColor;
                directionalSun.intensity = intensity;
            }
            RenderSettings.ambientLight = ambientColor;
            Camera.main.backgroundColor = skyColor;
        }

        private GameObject CreatePlane(Vector3 pos, Vector3 scale, Color color)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = "Ground";
            obj.transform.SetParent(_environmentRoot.transform, false);
            obj.transform.position = pos;
            obj.transform.localScale = scale;
            SetColor(obj, color);
            return obj;
        }

        private GameObject CreateMound(Vector3 pos, Vector3 scale, Color color)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.name = "Mound";
            obj.transform.SetParent(_environmentRoot.transform, false);
            obj.transform.position = pos;
            obj.transform.localScale = scale;
            SetColor(obj, color);
            return obj;
        }

        private GameObject CreateTree(Vector3 pos, Color foliageColor, float scaleMultiplier = 1f)
        {
            var tree = new GameObject("StylizedTree");
            tree.transform.SetParent(_environmentRoot.transform, false);
            tree.transform.position = pos;

            // Trunk
            var trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            trunk.transform.SetParent(tree.transform, false);
            trunk.transform.localPosition = new Vector3(0, 1.5f * scaleMultiplier, 0);
            trunk.transform.localScale = new Vector3(0.5f * scaleMultiplier, 1.5f * scaleMultiplier, 0.5f * scaleMultiplier);
            SetColor(trunk, new Color(0.42f, 0.28f, 0.16f));

            // Foliage cone/sphere
            var canopy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            canopy.transform.SetParent(tree.transform, false);
            canopy.transform.localPosition = new Vector3(0, 3.5f * scaleMultiplier, 0);
            canopy.transform.localScale = new Vector3(3f * scaleMultiplier, 2.5f * scaleMultiplier, 3f * scaleMultiplier);
            SetColor(canopy, foliageColor);

            return tree;
        }

        private GameObject CreateBridge(Vector3 pos, Vector3 scale, Color color)
        {
            var bridge = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bridge.name = "StoneBridge";
            bridge.transform.SetParent(_environmentRoot.transform, false);
            bridge.transform.position = pos;
            bridge.transform.localScale = scale;
            SetColor(bridge, color);
            return bridge;
        }

        private GameObject CreateColumn(Vector3 pos, float height, Color color)
        {
            var col = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            col.name = "Column";
            col.transform.SetParent(_environmentRoot.transform, false);
            col.transform.position = pos + new Vector3(0, height * 0.5f, 0);
            col.transform.localScale = new Vector3(0.8f, height * 0.5f, 0.8f);
            SetColor(col, color);
            return col;
        }

        private GameObject CreateCrystal(Vector3 pos, Color glowColor)
        {
            var crystal = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            crystal.name = "Crystal";
            crystal.transform.SetParent(_environmentRoot.transform, false);
            crystal.transform.position = pos;
            crystal.transform.rotation = Quaternion.Euler(15, 25, -10);
            crystal.transform.localScale = new Vector3(0.6f, 2f, 0.6f);
            SetColor(crystal, glowColor);

            var light = crystal.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = glowColor;
            light.range = 5f;
            light.intensity = 1.5f;

            return crystal;
        }

        private GameObject CreateGlowOrb(Vector3 pos, Color color)
        {
            var orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            orb.name = "GlowOrb";
            orb.transform.SetParent(_environmentRoot.transform, false);
            orb.transform.position = pos;
            orb.transform.localScale = Vector3.one * 0.4f;
            SetColor(orb, color);

            var light = orb.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = color;
            light.range = 4f;
            light.intensity = 1.2f;

            return orb;
        }

        private void CreateFlowerCluster(Vector3 center, Color petalColor)
        {
            for (int i = 0; i < 5; i++)
            {
                var f = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                f.name = "Flower";
                f.transform.SetParent(_environmentRoot.transform, false);
                f.transform.position = center + new Vector3((i - 2) * 0.35f, 0, (i % 2) * 0.3f);
                f.transform.localScale = Vector3.one * 0.25f;
                SetColor(f, petalColor);
            }
        }

        private void SetColor(GameObject obj, Color col)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                var mat = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
                mat.color = col;
                renderer.sharedMaterial = mat;
            }
        }
    }
}
