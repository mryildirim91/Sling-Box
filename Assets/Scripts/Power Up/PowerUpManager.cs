using System.Collections;
using DG.Tweening;
using MyUtils;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }
    
    private bool _canLaunchMissile;
    private string _powerupDef;
    [SerializeField] private Transform _hook;
    [SerializeField] private GameObject _missile, _crossHair;
    [SerializeField] private Image _powerUpImg;
    [SerializeField] private Button _powerUpButton;
    [SerializeField] private Text _powerUpText;
    [SerializeField] private Sprite[] _powerUpImgSprites;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        LaunchMissile();
    }

    public void SpawnPowerUp()
    {
        int random = Random.Range(1, 5);
        
        _powerUpImg.gameObject.SetActive(true);
        _powerUpImg.sprite = _powerUpImgSprites[random - 1];
        
        switch (random)
        {
            case 1:
                _powerupDef = "Launch a box eraser!";
                break;
            case 2:
                _powerupDef = "Launch a x2 multiplier!";
                break;
            case 3:
                _powerupDef = "Launch a x4 multiplier!";
                break;
        }
        
        if (random == 4)
        {
            Invoke(nameof(LetLaunchMissile),0.5f);
            _powerupDef = "Pick s spot to launch a missile!";
        }
        else
        {
            StartCoroutine(DelayGetHookChild(random));
        }

        _powerUpText.text = _powerupDef;
    }

    private IEnumerator DelayGetHookChild(int random)
    {
        yield return BetterWaitForSeconds.Wait(0.5f);
        _hook.GetComponentInChildren<BoxPiece>().ActivatePowerUp(random);
        _hook.GetComponentInChildren<PowerUp>().enabled = true;
    }

    private void LaunchMissile()
    {
        if (!_canLaunchMissile)
            return;
        
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
        if (Input.GetMouseButtonDown(0) && _canLaunchMissile)
        {
            _crossHair.transform.position = touchPos;
            SpriteRenderer spriteRenderer = _crossHair.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = true;
        }

        if (Input.GetMouseButton(0))
        {
            _crossHair.transform.DOMove(touchPos, 0.1f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _canLaunchMissile = false;
            GameObject powerUp = ObjectPool.Instance.GetObject(_missile);
            powerUp.name = "Missile";
            
            powerUp.transform.position = ScreenBoundaries.GetScreenBoundaries(0,touchPos.x,1,5);
            StartCoroutine(ShowCrossHair());
        }
    }

    private void LetLaunchMissile()
    {
        _canLaunchMissile = true;
    }

    private IEnumerator ShowCrossHair()
    {
        yield return BetterWaitForSeconds.Wait(3);
        _crossHair.GetComponent<SpriteRenderer>().enabled = false;
    }
    
    public void ActivatePowerUp()
    {
        Invoke(nameof(DelayActivation), 1);
    }

    private void DelayActivation()
    {
        _powerUpImg.gameObject.SetActive(false);
        _powerUpButton.interactable = true;
        _powerUpButton.image.sprite = _powerUpImgSprites[_powerUpImgSprites.Length - 1];
        _powerUpText.text = "";
    }
}
