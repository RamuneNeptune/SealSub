﻿using SealSubMod.Interfaces;
using SealSubMod.MonoBehaviours.Abstract;
using SealSubMod.Utility;

namespace SealSubMod.MonoBehaviours.Prefab;

// the class that spawns the power cell model
internal class SpawnPowerCellModel : PrefabModifierAsync
{
    private static GameObject[] _cachedModels = null;

    // serialize field is my favorite access modifier (I love serialize field <3)
    [SerializeField] Transform parent;
    [SerializeField] Vector3 localPos;
    [SerializeField] Vector3 localAngles;
    [SerializeField] Vector3 localScale;
    [SerializeField] BatterySource batterySource;

    private void OnValidate()
    {
        batterySource = GetComponent<BatterySource>();
    }

    // isn't this such a useful interface??!?!??!?!? ikr?!?
    public override IEnumerator SetupPrefabAsync()
    {
        if (_cachedModels == null) yield return LoadModels();

        for(int i = 0; i < _cachedModels.Length; i++)
            batterySource.batteryModels[i].model = SpawnModel(_cachedModels[i]);

        // um what do we do for custom batteries...? eldritch, any ideas? is that even a concern?

        //Not sure there, could try get the batteries from the seamoth/cyclops directly though?
        //or if custom batteries is integrated into nautilus that would help with compat here
    }

    public IEnumerator LoadModels()
    {
        // if it doesn't exist then what's the point of using it
        yield return CyclopsReferenceManager.EnsureCyclopsReferenceExists();

        var powerCellModelReference = CyclopsReferenceManager.CyclopsReference.transform.Find("cyclopspower/generator/SubPowerSocket1/SubPowerCell1/model").gameObject;
        var ionPowerCellModelReference = CyclopsReferenceManager.CyclopsReference.transform.Find("cyclopspower/generator/SubPowerSocket1/SubPowerCell1/engine_power_cell_ion").gameObject;

        _cachedModels = new[]
        {
            powerCellModelReference, 
            ionPowerCellModelReference,
        };
    }

    // method name can't be same as class name, I wanted it to be called "SpawnPowerCellModel" as well :(
    private GameObject SpawnModel(GameObject modelReference)
    {
        var spawnedModel = Instantiate(modelReference, parent);

        spawnedModel.transform.localPosition = localPos;
        spawnedModel.transform.localEulerAngles = localAngles;
        spawnedModel.transform.localScale = localScale;

        return spawnedModel;
    }
}