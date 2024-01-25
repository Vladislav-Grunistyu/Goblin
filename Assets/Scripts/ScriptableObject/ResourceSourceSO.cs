using UnityEngine;

[CreateAssetMenu(fileName = "New ResourceSourceSO", menuName = "Game designer tools/ResourceSourceSO", order = 2)]
public class ResourceSourceSO : ScriptableObject
{
    public ResourceStage[] stages;//—тадии после каждого удара
    public ResourceSO resourceType;//“ип ресурса который выбрасывает источник
    public float miningSpeed;//—корость с которой добываетс€ ресурс
    public float animationForce;//Ќа сколько сильно будет сжиматьс€ источник при добывании
    public int resourcePerHit;//—колько выпадает ресурсов за один удар
    public float fullRecoveryTime;//¬рем€ востановлени€ при полном добычии источника
}

[System.Serializable]
public class ResourceStage//¬ывел в отдельный так как на каждой стадии могут нужны индивидульные настройки
{
    public GameObject visualDisplayStage;//3D моделька стадии
}
