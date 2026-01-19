using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{

    public characterType tipoPersonagem;
    public int HP;
    public GameObject AIManager;

    public Image hpBar;
    public Text lifeCounter;

    private int maxHP;

    [Header("Blink Screen")]
    public Image getHitImage;
    public float speedBlinkGone;

    [Header("Car")]
    private GameManager _GameManager;
    public CarController _CarController;

    private SceneController _SceneController;


    private void Start()
    {

        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        _SceneController = FindObjectOfType(typeof(SceneController)) as SceneController;

        maxHP = HP;

        if (tipoPersonagem == characterType.PLAYER)
        {
            lifeCounter.text = maxHP.ToString();
        }
    }

    private void Update()
    {
        BlinkScreenDisappear();
    }

    void GetDamage(int amount)
    {
        HP -= amount;

        switch (tipoPersonagem)
        {
            case characterType.ENEMY:
                AIManager.SendMessage("GetShot", SendMessageOptions.DontRequireReceiver);

                if (HP <= 0)
                {
                    AIManager.SendMessage("IsDie", SendMessageOptions.DontRequireReceiver);
                }
                break;

            case characterType.PLAYER:

                UpdateHPBar();

                BlinkScreen();

                if (HP <= 0)
                {
                    _SceneController.LoadScene("GameOver"); // carrega cena pro gameover
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }

                break;

            case characterType.CAR:
                UpdateHPBarCar();

                if (HP <= 0)
                {

                    _CarController.isExplode = true;

                }


                break;
        }
        
    }

    private void UpdateHPBar()
    {
        float percHp = (float)HP / (float)maxHP;

        if (percHp <= 0.25f) // caso o player esteja igual ou menos de 25% da vida
        {
            hpBar.color = Color.red;
        }

        if (percHp < 0) { percHp = 0; HP = 0; }

        hpBar.fillAmount = percHp;
        lifeCounter.text = HP.ToString();
    }

    private void UpdateHPBarCar()
    {
        float percHp = (float)HP / (float)maxHP;


        if (percHp < 0) { percHp = 0; HP = 0; }

        _GameManager.hpBarCar.fillAmount = percHp;
    }


    private void BlinkScreen()
    {
        var color = getHitImage.color;
        color.a = 0.8f;

        getHitImage.color = color;

    }

    private void BlinkScreenDisappear()
    {
        if (getHitImage != null)
        {
            if (getHitImage.color.a > 0)
            {
                var color = getHitImage.color;

                color.a -= speedBlinkGone * Time.deltaTime;

                getHitImage.color = color;
            }
        }
    }

}
