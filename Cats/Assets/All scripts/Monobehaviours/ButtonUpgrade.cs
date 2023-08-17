using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUpgrade : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _selectedImage;
    [SerializeField] private Button _button;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private InfoUpgrade _infoUpgrade;
    [SerializeField] private float _tapDurationButton;

    private int _numberUpgrade;
    private EventManager _eventManager;
    private void Awake()
    {
        if (_image == null)
            _image = transform.Find("Image").GetComponent<Image>();

        if (_selectedImage == null)
            _selectedImage = transform.Find("Selected Image").gameObject;

        _selectedImage.SetActive(false);

        if (_button == null)
            _button = GetComponent<Button>();

        if(_upgradeManager == null)
            _upgradeManager = GameObject.FindGameObjectWithTag("GameData").GetComponentInChildren<UpgradeManager>();
    }
    private void Start()
    {
        _eventManager = _upgradeManager.eventManager;
        CheckValueCoins();

        _eventManager.SelectUpdateEvent += SelectThisUpdate;
        _eventManager.CheckActiveButtonButEvent += CheckValueCoins;
        _eventManager.UpgradeTextInfoTypesEvent += CheckValueCoins;

        _numberUpgrade = transform.GetSiblingIndex();
    }
    private void SelectThisUpdate(int number)
    {
        _numberUpgrade = transform.GetSiblingIndex();

        if (_numberUpgrade == number)
            _selectedImage.SetActive(true);
        else
            _selectedImage.SetActive(false);
    }
    public void GetSprite(Sprite spriteImage)
    {
        _image.sprite = spriteImage;
    }
    public void GetInfo(InfoUpgrade infoUpgrade)
    {
        _infoUpgrade = infoUpgrade;

        if(_infoUpgrade != null)
            GetSprite(_infoUpgrade.Sprite);
    }
    private void CheckValueCoins()
    {
        if (_infoUpgrade.Cost <= _upgradeManager.Coins)
            _button.interactable = true;
        else
            _button.interactable = false;
    }

    public void OnMouseDown()
    {
        _buttonIsPressed = true;

        StartCoroutine(TapTimer());
    }
    public void OnMouseUp()
    {
        _buttonIsPressed = false;
    }
    private bool _buttonIsPressed;
    private IEnumerator TapTimer()
    {
        float timer = 0;

        while (_buttonIsPressed)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);
            yield return null;
        }

        if (!_buttonIsPressed)
        {
            if (timer < _tapDurationButton)
            {
                _upgradeManager.GetInfoType(_infoUpgrade);
                _eventManager.UpgradeTextInfoTypes();
                _eventManager.SelectUpdate(_numberUpgrade);
            }

            StopCoroutine(TapTimer());
        }
    }
    private void OnDestroy()
    {
        _eventManager.SelectUpdateEvent -= SelectThisUpdate;
        _eventManager.CheckActiveButtonButEvent -= CheckValueCoins;
        _eventManager.UpgradeTextInfoTypesEvent -= CheckValueCoins;
    }
}
