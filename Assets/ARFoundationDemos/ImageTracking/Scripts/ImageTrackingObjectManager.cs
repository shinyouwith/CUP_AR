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
    [Tooltip("스테이지 prefab")]
    GameObject m_StagePrefab;

    public GameObject stagePrefab
    {
        get => m_StagePrefab;
        set => m_StagePrefab = value;
    }

    GameObject m_SpawnedStagePrefab;

    public GameObject spawnedStagePrefab
    {
        get => m_SpawnedStagePrefab;
        set => m_SpawnedStagePrefab = value;
    }

    [SerializeField]
    [Tooltip("Prefab for tracked 1 image")]
    GameObject m_OnePrefab;

    /// <summary>
    /// Get the one prefab
    /// </summary>
    public GameObject onePrefab
    {
        get => m_OnePrefab;
        set => m_OnePrefab = value;
    }

    GameObject m_SpawnedOnePrefab;
    
    /// <summary>
    /// get the spawned one prefab
    /// </summary>
    public GameObject spawnedOnePrefab
    {
        get => m_SpawnedOnePrefab;
        set => m_SpawnedOnePrefab = value;
    }

    [SerializeField]
    [Tooltip("Prefab for tracked 2 image")]
    GameObject m_TwoPrefab;

    /// <summary>
    /// get the two prefab
    /// </summary>
    public GameObject twoPrefab
    {
        get => m_TwoPrefab;
        set => m_TwoPrefab = value;
    }

    GameObject m_SpawnedTwoPrefab;
    
    /// <summary>
    /// get the spawned two prefab
    /// </summary>
    public GameObject spawnedTwoPrefab
    {
        get => m_SpawnedTwoPrefab;
        set => m_SpawnedTwoPrefab = value;
    }

    int m_NumberOfTrackedImages;

    PrefabManager m_StagePrefabManager;
    NumberManager m_OneNumberManager;
    NumberManager m_TwoNumberManager;

    static Guid s_FirstImageGUID;
    static Guid s_SecondImageGUID;
    static Guid s_CupArMarkerImageGUID;

    void OnEnable()
    {
        // 등록된 이미지 라이브러리 중 첫번째 이미지 -> Cup AR marker 이미지로 사용할 수 있도록 고유한 아이디 할당
        s_CupArMarkerImageGUID = m_ImageLibrary[0].guid;
        //s_FirstImageGUID = m_ImageLibrary[0].guid;
        //s_SecondImageGUID = m_ImageLibrary[1].guid;
        
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
            // CUP 마커 이미지가 감지되었다면
            if (image.referenceImage.guid == s_CupArMarkerImageGUID) {

                // 해당 이미지 위치에 stage prefab을 instantiate
                m_SpawnedStagePrefab = Instantiate(m_StagePrefab, image.transform.position, image.transform.rotation);

                // 만든 prefab을 관리하기 위한 매니저 같이 생성
                m_StagePrefabManager = m_SpawnedStagePrefab.GetComponent<PrefabManager>();
            }

            //if (image.referenceImage.guid == s_FirstImageGUID)
            //{
            //    m_SpawnedOnePrefab = Instantiate(m_OnePrefab, image.transform.position, image.transform.rotation);
            //    m_OneNumberManager = m_SpawnedOnePrefab.GetComponent<NumberManager>();
            //}
            //else if (image.referenceImage.guid == s_SecondImageGUID)
            //{
            //    m_SpawnedTwoPrefab = Instantiate(m_TwoPrefab, image.transform.position, image.transform.rotation);
            //    m_TwoNumberManager = m_SpawnedTwoPrefab.GetComponent<NumberManager>();
            //}
        }
        
        // updated, set prefab position and rotation
        foreach(ARTrackedImage image in obj.updated)
        {
            // image is tracking or tracking with limited state, show visuals and update it's position and rotation
            // 이미지를 계속 'Tracking'하는 상태이면,
            if (image.trackingState == TrackingState.Tracking)
            {
                if (image.referenceImage.guid == s_CupArMarkerImageGUID) {

                    // 그래픽 그리기
                    m_StagePrefabManager.EnablePrefab(true);

                    // 이미지 위치에 항상 stage가 그려지도록 위치, 각도 업데이트
                    m_SpawnedStagePrefab.transform.SetPositionAndRotation(image.transform.position, image.transform.rotation);
                }

                //if (image.referenceImage.guid == s_FirstImageGUID)
                //{
                //    m_OneNumberManager.Enable3DNumber(true);
                //    m_SpawnedOnePrefab.transform.SetPositionAndRotation(image.transform.position, image.transform.rotation);
                //}
                //else if (image.referenceImage.guid == s_SecondImageGUID)
                //{
                //    m_TwoNumberManager.Enable3DNumber(true);
                //    m_SpawnedTwoPrefab.transform.SetPositionAndRotation(image.transform.position, image.transform.rotation);
                //}
            }
            // image is no longer tracking, disable visuals TrackingState.Limited TrackingState.None
            // 이미지 트래킹 상태가 아니면,
            else
            {
                if (image.referenceImage.guid == s_CupArMarkerImageGUID) {
                    // stage prefab이 보이지 않도록 변경 
                    m_StagePrefabManager.EnablePrefab(false);
                }

                //if (image.referenceImage.guid == s_FirstImageGUID)
                //{
                //    m_OneNumberManager.Enable3DNumber(false);
                //}
                //else if (image.referenceImage.guid == s_SecondImageGUID)
                //{
                //    m_TwoNumberManager.Enable3DNumber(false);
                //}
            }
        }
        
        // removed, destroy spawned instance
        // 아예 종료된 상황이면
        foreach(ARTrackedImage image in obj.removed)
        {
            if (image.referenceImage.guid == s_CupArMarkerImageGUID) {

                // stage prefab 리소스 아예 제거
                Destroy(m_SpawnedStagePrefab);
            }

            //if (image.referenceImage.guid == s_FirstImageGUID)
            //{
            //    Destroy(m_SpawnedOnePrefab);
            //}
            //else if (image.referenceImage.guid == s_FirstImageGUID)
            //{
            //    Destroy(m_SpawnedTwoPrefab);
            //}
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
