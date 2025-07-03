using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EltBuilding : MonoBehaviour
{
    public Image imgBuilding;
    public Button btnBuild;
    public Button btnLvUp;
    public GameWindow gameWindow;
    public UnityAction<BuildingType> funcBuild;
    public UnityAction<BuildingType> funcLvUp;

    BuildingType type;
    bool isNotBuilt = false;

    public void Set(BuildingType type, bool isNotBuilt)
    {
        this.type = type;
        this.isNotBuilt = isNotBuilt;

        imgBuilding.material = isNotBuilt ? Singleton.resourceManager.mat_GrayScale : null;
        btnBuild.SetActive(isNotBuilt);
        btnLvUp.SetActive(!isNotBuilt);
    }

    public void OnClick()
    {
        if (isNotBuilt)
            return;

        gameWindow.Show();
    }

    public void OnClickBuild()
    {
        funcBuild?.Invoke(type);
    }

    public void OnClickLvUp()
    {
        funcLvUp?.Invoke(type);
    }
}
