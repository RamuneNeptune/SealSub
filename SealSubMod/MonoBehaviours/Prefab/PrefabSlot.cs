﻿using SealSubMod.Interfaces;
using UWE;

namespace SealSubMod.MonoBehaviours.Prefab;

internal class PrefabSlot : MonoBehaviour, IAsyncPrefabSetupOperation
{
    [Header("Whether the Class ID or TechType is used:")]
    [SerializeField] Mode mode;
    [Header("The Class ID or TechType to use:")]
    [SerializeField] string classId;
    [SerializeField] TechType techType;

    public IEnumerator SetupPrefabAsync(GameObject prefabRoot)
    {
        GameObject prefab;
        if (mode == Mode.ClassId)
        {
            var task = PrefabDatabase.GetPrefabAsync(classId);
            yield return task;
            task.TryGetPrefab(out prefab);
        }
        else
        {
            var task = CraftData.GetPrefabForTechTypeAsync(techType);
            yield return task;
            prefab = task.GetResult();
        }
        Instantiate(prefab, transform, false);
    }

    public enum Mode
    {
        ClassId,
        TechType
    }
}
