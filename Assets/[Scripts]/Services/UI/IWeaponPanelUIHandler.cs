using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponPanelUIHandler : IGameService
{
    public void SetDetailPanelInfo(InvenItem invenItem);
    public void UpdateCharacterStatPanel();
}
