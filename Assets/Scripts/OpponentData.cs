using UnityEngine;

[CreateAssetMenu(fileName = "OpponentData", menuName = "Scriptable Objects/OpponentData")]
public class OpponentData : ScriptableObject
{
    public bool rareOpponent;
    public Sprite spr_normal;
    public Sprite[] spr_hit;
    public Sprite getSpr_hit { get { return spr_hit[Random.Range(0,spr_hit.Length)]; } }
    public Sprite spr_neutralized;

    public string Pokedex_name;
    public string Pokedex_text;
}
