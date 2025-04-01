using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static VFX;
using System.Collections;


public class VFXPool : MonoBehaviour
{


    public List<VFX> VFXs;

    public int PoolSize = 20;

    private List<VFX> _vfxs;

    public static VFXPool Instance;

    private void Awake()
    {
        //하나임을 보장하는 코드로 변경
        Instance = this;

        // 풀 크기를 정하고
        int VFXPrefabsCount = VFXs.Count;
        _vfxs = new List<VFX>(PoolSize * VFXPrefabsCount);

        foreach (VFX vfxPrefab in VFXs)
        {
            for (int i = 0; i < PoolSize; i++)
            {
                VFX vfx = Instantiate(vfxPrefab);

                _vfxs.Add(vfx);

                vfx.transform.SetParent(this.transform);

                vfx.gameObject.SetActive(false);
            }
        }
    }

    public VFX Create(VFXType vfxtype, Vector3 position)
    {
        foreach (VFX vfx in _vfxs)
        {
            if (vfx.vfxtype == vfxtype && vfx.gameObject.activeInHierarchy == false)
            {
                vfx.transform.position = position;

                vfx.Initialize();

                vfx.gameObject.SetActive(true);

                StartCoroutine(unActiveVFX(vfx, 3f));
                return vfx;
            }
        }

        return null;
    }

    IEnumerator unActiveVFX(VFX vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        vfx.gameObject.SetActive(false);
    }
}
