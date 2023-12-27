using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] string _androidAdUnitId;
    [SerializeField] string _iOSAdUnitId;
    string _adUnitId = null; // �������� �ʴ� �÷����� ��� ���� null�� ���� �ֽ��ϴ�.

    void Awake()
    {
        // ���� �÷����� ���� ���� ID�� �����ɴϴ�.
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        //���� ǥ�õ� �غ� �� ������ ��ư�� ��Ȱ��ȭ�մϴ�.
        _showAdButton.interactable = false;
    }

    void Start()
    {
        LoadAd();
    }

    // ���� ���ֿ� �������� �ε��մϴ�.
    public void LoadAd()
    {
        // �߿�! �ʱ�ȭ �Ŀ��� �������� �ε��մϴ�(�� �������� �ʱ�ȭ�� �ٸ� ��ũ��Ʈ���� ó����).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);

        OnUnityAdsAdLoaded(_adUnitId);
    }

    // ���� ���������� �ε�Ǹ� ��ư�� �����ʸ� �߰��ϰ� Ȱ��ȭ�մϴ�.
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))
        {
            // Ŭ�� �� ShowAd() �޼��带 ȣ���ϵ��� ��ư�� �����մϴ�.
            _showAdButton.onClick.AddListener(ShowAd);
            // ������ Ŭ���� �� �ֵ��� ��ư�� Ȱ��ȭ�մϴ�.
            _showAdButton.interactable = true;
        }
    }

    // ������ ��ư�� Ŭ���� �� ������ �޼��带 �����մϴ�.
    public void ShowAd()
    {
        AudioManager.instance.StopBGM();
        // ��ư�� ��Ȱ��ȭ�մϴ�.
        _showAdButton.interactable = false;
        // �׷� ���� ���� ǥ���մϴ�.
        Advertisement.Show(_adUnitId, this);
    }

    // Show Listener�� OnUnityAdsShowComplete �ݹ� �޼��带 �����Ͽ� ������ ������ ������ �����մϴ�.
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // ������ �ݴϴ�.
            GameManager.I.retryGame();
        }
    }

    // Load �� Show ������ ���� �ݹ��� �����մϴ�.
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // ���� ���� ������ ����Ͽ� �� �ٸ� ���� �ε����� ���θ� �����մϴ�.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // ���� ���� ������ ����Ͽ� �� �ٸ� ���� �ε����� ���θ� �����մϴ�.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        // ��ư �����ʸ� �����մϴ�.
        _showAdButton.onClick.RemoveAllListeners();
    }
}