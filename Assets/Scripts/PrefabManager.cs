using System;
using UnityEngine;
using UnityEngine.Animations;

public class PrefabManager : MonoBehaviour
{

    [SerializeField]
    [Tooltip("화면에 prefab on/off를 제어하기 위한 manager")]
    GameObject m_PrefabObject;

    public GameObject prefabObject
    {
        get => m_PrefabObject;
        set => m_PrefabObject = value;
    }


    void OnEnable()
    {
        ConstraintSource m_Source = new ConstraintSource();
        m_Source.sourceTransform = Camera.main.transform;
        m_Source.weight = 1.0f;
    }

    public void EnablePrefab(bool enable)
    {
        m_PrefabObject.SetActive(enable);
    }

}