using System.Collections;
using System.Collections.Generic;
using _Kittens__Kitchen_.Editor.Production.Scripts.Upgrades.Voilt;
using UnityEngine;
using Zenject;

public class ClientSystem : MonoBehaviour, IClientObserver
{
    [Inject] private readonly EventManager _eventManager;

    [SerializeField] private VoiltsInfo voiltsInfo;
    [SerializeField] private GameObject purchaseSuccessful;
    [SerializeField] private GameObject purchaseFailed;
    [SerializeField] private float resultDisplayDuration = 2.0f;

    private VoiltData _tipsVoilt;

    public void RegisterClient(Client client)
    {
        client.RegisterObserver(this);
    }

    public void UnregisterClient(Client client)
    {
        client.UnregisterObserver(this);
    }

    public void OnObservableUpdate(Client client,ProductData product, int desiredQuantity)
    {
        OnProductBought(client, product, desiredQuantity);
        Debug.Log("OnObservableUpdate is invoked in ClientSystem");
    }

    private void OnProductBought(Client client, ProductData product, int desiredQuantity)
    {
        if (client.DesiredProduct == null) return;

        int productsToSell = desiredQuantity;
        int earnings = productsToSell * product.Cost;
        var tipsMultiplier = IfCanReturnTipsAndWildPig();

        Debug.Log("earnings: " + earnings);
        
        earnings *= (int)tipsMultiplier;

        Debug.Log($"earnings with wildPig and tips: {tipsMultiplier} = " + earnings);
        
        if (client.DesiredProduct.Count >= productsToSell)
        {
            client.DesiredProduct.Count -= productsToSell;
            client.SellProduct(productsToSell);

            _eventManager.ChangeValueGold(earnings);
            Debug.Log("Client money: " + client.MoneyToPay);
            
            ShowPurchaseResult(purchaseSuccessful, client.transform.position);
        }
    }

    private float IfCanReturnTipsAndWildPig()
    {
        VoiltData voiltTips = voiltsInfo.ReturnVoiltOfType(VoiltTypes.Tips);
        VoiltData voiltWildPig = voiltsInfo.ReturnVoiltOfType(VoiltTypes.WildPig);

        float tipsMultiplier = 1;
        if (voiltTips != null)
        {
            tipsMultiplier = voiltsInfo.LeaveTips(voiltTips, voiltWildPig);
        }

        Debug.Log("Tips is have");
        return tipsMultiplier;
    }

    private void ShowPurchaseResult(GameObject resultPrefab, Vector3 clientPosition)
    {
        GameObject resultInstance = Instantiate(resultPrefab, clientPosition + Vector3.up * 1, Quaternion.identity);
        Destroy(resultInstance, resultDisplayDuration);
    }
    
    public void ShowPurchaseFailed(Vector3 clientPosition)
    {
        ShowPurchaseResult(purchaseFailed, clientPosition);
    }

    public void ShowPurchaseSuccessful(Vector3 clientPosition)
    {
        ShowPurchaseResult(purchaseSuccessful, clientPosition);
    }
}
