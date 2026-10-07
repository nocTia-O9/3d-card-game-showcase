using UnityEngine;
using UnityEngine.AddressableAssets;

public class Boot : MonoBehaviour
{
    public AssetReference persistent;
    private void Awake()
    {
        Addressables.LoadSceneAsync(persistent);
    }
}
