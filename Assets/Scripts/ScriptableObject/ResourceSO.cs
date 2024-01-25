using UnityEngine;
[CreateAssetMenu(fileName = "New ResourceSO", menuName = "Game designer tools/ResourceSO", order = 1)]
public class ResourceSO : ScriptableObject
{
    public GameObject visualDisplayResource; //3D модель ресурса
    public Sprite icon; //Спрайт котороый показываетс в UI
    public string ResourceName;
    public float timeNotToSelect;//Время за которое игрок не может поднять ресурс после спавна 
    public float upwardForce;//Силы с которыйми выталскивается ресурс во время спавна
    public float sidewaysForce;
    public float flySpeed;//Скорость полета к игроку или споту
}
