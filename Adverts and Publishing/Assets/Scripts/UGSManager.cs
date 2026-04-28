using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class UGSManager : MonoBehaviour
{
    public static UGSManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    async void Start()
    {
        await InitializeUGS();
    }

    private async Task InitializeUGS()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await SignInAnonymously();
            Debug.Log("UGS Initialized Successfully");
        }
        catch (System.Exception e)
        {
            Debug.Log($"UGS Initialization Failed: {e.Message}");
        }
    }

    private async Task SignInAnonymously()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Signed in as: {AuthenticationService.Instance.PlayerId}");
        }
    }
}
