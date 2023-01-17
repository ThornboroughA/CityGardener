using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Person
{
    public string name;
    public Ethnicity ethnicity;

    public Person()
    {
        this.ethnicity = Culture.AssignEthnicity();
        this.name = Culture.AssignName(ethnicity);
    }
}



public enum Ethnicity { Mahan, Jinhan, Byeonhan, Yamatai};
public static class Culture {

    public static Ethnicity AssignEthnicity()
    {
        

        float randomValue = (Random.Range(0, 100) * .01f);
        Ethnicity ethnicity;

        if (randomValue < 0.4f)
        {
            ethnicity = Ethnicity.Byeonhan;
        } else if (randomValue < 0.65f)
        {
            ethnicity = Ethnicity.Jinhan;
        } else if (randomValue < 0.9f)
        {
            ethnicity = Ethnicity.Mahan;
        } else
        {
            ethnicity = Ethnicity.Yamatai;
        }
        return ethnicity;
    }

    public static string AssignName(Ethnicity ethnicity)
    {
        string name = null;

        switch (ethnicity)
        {
            case Ethnicity.Mahan:
                name = "Goa";
                break;
            case Ethnicity.Jinhan:
                name = "Jisoo";
                break;
            case Ethnicity.Byeonhan:
                name = "Ena";
                break;
            case Ethnicity.Yamatai:
                name = "Hamamori";
                break;
            default:
                name = "Default";
                break;
        }

        return name;
    }

}