using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WordVista.Save;

namespace WordVista.Environment
{
    [Serializable]
    public class TransformationStage
    {
        public int milestoneLevel;
        public string stageName;
        public GameObject[] objectsToActivate;
        public ParticleSystem[] particlesToPlay;
        public Light[] lightsToEnrich;
        public float lightTargetIntensity = 1.2f;
        public bool isUnlocked;
    }

    public class EnvironmentalProgressController : MonoBehaviour
    {
        [Header("World Configuration")]
        [SerializeField] private int worldId = 1;
        [SerializeField] private string worldName = "Green Valley";

        [Header("Transformation Stages")]
        [SerializeField] private List<TransformationStage> stages = new List<TransformationStage>();

        [Header("Dynamic Micro-Reactions")]
        [SerializeField] private ParticleSystem wordSolveBurst;
        [SerializeField] private Transform focalPoint;

        public event Action<TransformationStage> OnStageUnlocked;

        private void Start()
        {
            ApplyCurrentWorldProgress();
        }

        public void InitializeForWorld(int world, int highestLevel)
        {
            this.worldId = world;
            ApplyProgress(highestLevel);
        }

        public void OnWordSolvedReaction()
        {
            if (wordSolveBurst != null)
            {
                wordSolveBurst.Play();
            }
        }

        public void CheckLevelProgress(int completedLevelId)
        {
            foreach (var stage in stages)
            {
                if (!stage.isUnlocked && completedLevelId >= stage.milestoneLevel)
                {
                    UnlockStage(stage, true);
                }
            }
        }

        private void ApplyCurrentWorldProgress()
        {
            var wp = SaveManager.Instance.GetWorldProgress(worldId);
            ApplyProgress(wp.highestLevelCompleted);
        }

        private void ApplyProgress(int highestLevel)
        {
            foreach (var stage in stages)
            {
                bool shouldBeActive = highestLevel >= stage.milestoneLevel;
                UnlockStage(stage, false);
                SetStageActive(stage, shouldBeActive);
            }
        }

        private void UnlockStage(TransformationStage stage, bool playAnimation)
        {
            stage.isUnlocked = true;
            SetStageActive(stage, true);

            if (playAnimation)
            {
                if (stage.particlesToPlay != null)
                {
                    foreach (var ps in stage.particlesToPlay)
                    {
                        if (ps != null) ps.Play();
                    }
                }
                OnStageUnlocked?.Invoke(stage);
            }
        }

        private void SetStageActive(TransformationStage stage, bool active)
        {
            if (stage.objectsToActivate != null)
            {
                foreach (var obj in stage.objectsToActivate)
                {
                    if (obj != null) obj.SetActive(active);
                }
            }
        }
    }
}
