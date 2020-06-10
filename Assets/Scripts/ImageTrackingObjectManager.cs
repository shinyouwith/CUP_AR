using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackingObjectManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Image manager on the AR Session Origin")]
    ARTrackedImageManager m_ImageManager;

    /// <summary>
    /// Get the <c>ARTrackedImageManager</c>
    /// </summary>
    public ARTrackedImageManager ImageManager
    {
        get => m_ImageManager;
        set => m_ImageManager = value;
    }

    [SerializeField]
    [Tooltip("Reference Image Library")]
    XRReferenceImageLibrary m_ImageLibrary;

    /// <summary>
    /// Get the <c>XRReferenceImageLibrary</c>
    /// </summary>
    public XRReferenceImageLibrary ImageLibrary
    {
        get => m_ImageLibrary;
        set => m_ImageLibrary = value;
    }

    [SerializeField]
    [Tooltip("chamomile stage prefab")]
    GameObject m_ChamomileStagePrefab;

    public GameObject stagePrefab
    {
        get => m_ChamomileStagePrefab;
        set => m_ChamomileStagePrefab = value;
    }

    GameObject m_SpawnedChamomileStagePrefab;

    public GameObject spawnedChamomileStagePrefab
    {
        get => m_SpawnedChamomileStagePrefab;
        set => m_SpawnedChamomileStagePrefab = value;
    }

    [SerializeField]
    [Tooltip("earlgrey stage prefab")]
    GameObject m_EarlGreyStagePrefab;

    /// <summary>
    /// Get the Earl Grey prefab
    /// </summary>
    public GameObject onePrefab
    {
        get => m_EarlGreyStagePrefab;
        set => m_EarlGreyStagePrefab = value;
    }

    GameObject m_SpawnedEarlGreyStagePrefab;

    /// <summary>
    /// get the spawned Earl Grey prefab
    /// </summary>
    public GameObject spawnedEarlGreyStagePrefab
    {
        get => m_SpawnedEarlGreyStagePrefab;
        set => m_SpawnedEarlGreyStagePrefab = value;
    }

    [SerializeField]
    [Tooltip("peppermint stage prefab")]
    GameObject m_PeppermintStagePrefab;

    /// <summary>
    /// get the Peppermint prefab
    /// </summary>
    public GameObject peppermintStagePrefab
    {
        get => m_PeppermintStagePrefab;
        set => m_PeppermintStagePrefab = value;
    }

    GameObject m_SpawnedPeppermintStagePrefab;

    /// <summary>
    /// get the spawned Peppermint prefab
    /// </summary>
    public GameObject spawnedPeppermintStagePrefab
    {
        get => m_SpawnedPeppermintStagePrefab;
        set => m_SpawnedPeppermintStagePrefab = value;
    }

    int m_NumberOfTrackedImages;

    PrefabManager m_ChamomileStagePrefabManager;
    PrefabManager m_EarlGreyStagePrefabManager;
    PrefabManager m_PeppermintStagePrefabManager;

    static Guid s_ChamomileMarkerGUID;
    static Guid s_EarlGreyMarkerGUID;
    static Guid s_PeppermintMarkerGUID;

    void OnEnable()
    {
        s_ChamomileMarkerGUID = m_ImageLibrary[0].guid;
        s_EarlGreyMarkerGUID = m_ImageLibrary[1].guid;
        s_PeppermintMarkerGUID = m_ImageLibrary[2].guid;

        m_ImageManager.trackedImagesChanged += ImageManagerOnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_ImageManager.trackedImagesChanged -= ImageManagerOnTrackedImagesChanged;
    }

    void ImageManagerOnTrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        // added, spawn prefab
        foreach(ARTrackedImage image in obj.added)
        {
            if (image.referenceImage.guid == s_ChamomileMarkerGUID)
            {
                // 해당 이미지 위치에 stage prefab을 instantiate
                m_SpawnedChamomileStagePrefab = Instantiate(m_ChamomileStagePrefab, image.transform.position, image.transform.rotation);

                // 만든 prefab을 관리하기 위한 매니저 같이 생성
                m_ChamomileStagePrefabManager = m_SpawnedChamomileStagePrefab.GetComponent<PrefabManager>();
            }
            else if (image.referenceImage.guid == s_EarlGreyMarkerGUID)
            {
                // 해당 이미지 위치에 stage prefab을 instantiate
                m_SpawnedEarlGreyStagePrefab = Instantiate(m_EarlGreyStagePrefab, image.transform.position, image.transform.rotation);

                // 만든 prefab을 관리하기 위한 매니저 같이 생성
                m_EarlGreyStagePrefabManager = m_SpawnedEarlGreyStagePrefab.GetComponent<PrefabManager>();

            } else if (image.referenceImage.guid == s_PeppermintMarkerGUID)
            {
                // 해당 이미지 위치에 stage prefab을 instantiate
                m_SpawnedPeppermintStagePrefab = Instantiate(m_PeppermintStagePrefab, image.transform.position, image.transform.rotation);

                // 만든 prefab을 관리하기 위한 매니저 같이 생성
                m_PeppermintStagePrefabManager = m_SpawnedPeppermintStagePrefab.GetComponent<PrefabManager>();
            }
        }
        
        // updated, set prefab position and rotation
        foreach(ARTrackedImage image in obj.updated)
        {
            // image is tracking or tracking with limited state, show visuals and update it's position and rotation
            // 이미지를 계속 'Tracking'하는 상태이면,
            if (image.trackingState == TrackingState.Tracking)
            {
                if (image.referenceImage.guid == s_ChamomileMarkerGUID)
                {
                    Debug.Log("Tracking State Test >>>>> Enable Chamomile");
                    // 그래픽 그리기
                    m_ChamomileStagePrefabManager.EnablePrefab(true);

                    // 이미지 위치에 항상 stage가 그려지도록 위치, 각도 업데이트
                    Vector3 markerPosition = image.transform.position;
                    Vector3 targetPosition = new Vector3(markerPosition.x + 0.02f, markerPosition.y, markerPosition.z - 0.012f);
                    m_SpawnedChamomileStagePrefab.transform.SetPositionAndRotation(targetPosition, image.transform.rotation);
                }
                else if (image.referenceImage.guid == s_EarlGreyMarkerGUID)
                {
                    // 그래픽 그리기
                    m_EarlGreyStagePrefabManager.EnablePrefab(true);

                    // 이미지 위치에 항상 stage가 그려지도록 위치, 각도 업데이트
                    m_SpawnedEarlGreyStagePrefab.transform.SetPositionAndRotation(image.transform.position, image.transform.rotation);
                }
                else if (image.referenceImage.guid == s_PeppermintMarkerGUID)
                {
                    // 그래픽 그리기
                    m_PeppermintStagePrefabManager.EnablePrefab(true);

                    // 이미지 위치에 항상 stage가 그려지도록 위치, 각도 업데이트
                    m_SpawnedPeppermintStagePrefab.transform.SetPositionAndRotation(image.transform.position, image.transform.rotation);
                }
            }
            // image is no longer tracking, disable visuals TrackingState.Limited TrackingState.None
            // 이미지 트래킹 상태가 아니면,
            else
            {
                if (image.referenceImage.guid == s_ChamomileMarkerGUID)
                {
                    Debug.Log("Tracking State Test >>>>> Disable Chamomile");
                    m_ChamomileStagePrefabManager.EnablePrefab(false);
                }
                else if (image.referenceImage.guid == s_EarlGreyMarkerGUID)
                {
                    m_EarlGreyStagePrefabManager.EnablePrefab(false);
                }
                else if (image.referenceImage.guid == s_PeppermintMarkerGUID)
                {
                    m_PeppermintStagePrefabManager.EnablePrefab(false);
                }

            }
        }
        
        // removed, destroy spawned instance
        // 아예 종료된 상황이면
        foreach(ARTrackedImage image in obj.removed)
        {
            if (image.referenceImage.guid == s_ChamomileMarkerGUID)
            {
                Debug.Log("Tracking State Test >>>>> Destroy Chamomile");
                Destroy(m_SpawnedChamomileStagePrefab);
            }
            else if (image.referenceImage.guid == s_EarlGreyMarkerGUID)
            {
                Destroy(m_SpawnedEarlGreyStagePrefab);
            }
            else if (image.referenceImage.guid == s_PeppermintMarkerGUID)
            {
                Destroy(m_SpawnedPeppermintStagePrefab);
            }
        }
    }

    public int NumberOfTrackedImages()
    {
        m_NumberOfTrackedImages = 0;
        foreach (ARTrackedImage image in m_ImageManager.trackables)
        {
            if (image.trackingState == TrackingState.Tracking)
            {
                m_NumberOfTrackedImages++;
            }
        }
        return m_NumberOfTrackedImages;
    }
}
