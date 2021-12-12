using System;

public static class UIEvents
{
    public static event Action<string> OnAddressablesCheckE;


    #region StaticEvent

    public static void OnAddressablesCheck(string _fileName)
    {
        OnAddressablesCheckE?.Invoke(_fileName);

    }
    #endregion
}
