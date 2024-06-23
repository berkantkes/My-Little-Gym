using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyPayAreaController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Image _fillImage;

    private SingleEnvironmentController _singleEnvironmentController;
    private int _defaultPrice;
    private int _currentPrice = 200;
    public int CurrentPrice => _currentPrice;
    
    public void Initialize(SingleEnvironmentController singleEnvironmentController)
    {
        _singleEnvironmentController = singleEnvironmentController;
        _defaultPrice = 200;
        _currentPrice = _defaultPrice;
        SetPriceText();
        transform.gameObject.SetActive(true); 
    }


    public void SetCurrentPrice(int currentPrice)
    {
        _currentPrice = currentPrice;
        SetPriceText();
        SetFillImage();
    }

    public void SetandUpdateCurrentPrice(int currentPrice)
    {
        _currentPrice = currentPrice;
        SetPriceText();
        SetFillImage();
        ChangeCurrentPrice();

        if (_currentPrice == 0)
        {
            PaidArea();
        }
    }

    private void ChangeCurrentPrice()
    {
        _singleEnvironmentController.ChangeCurrentPrice();
    }

    public void PaidArea()
    {
        _singleEnvironmentController.PaidArea();
    }

    private void SetFillImage()
    {
        _fillImage.fillAmount =  1 - ((float)_currentPrice / (float)_defaultPrice);
    }
    public void SetPriceText()
    {
        _priceText.SetText(_currentPrice.ToString());
    }
    public int GetPrice()
    {
        return _defaultPrice;
    }
    public int GetCurrentPrice()
    {
        return _currentPrice;
    }

}
