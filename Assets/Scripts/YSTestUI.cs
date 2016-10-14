using UnityEngine;
using System.Collections;

public class YSTestUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Navire navire1 = new Navire("L'Hermione");
        Navire navire2 = new Navire("Le Grand Mât");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

enum typeEquipage
{
    CAPITAINE,
    MAINTENANCE,
    CANNON,
    SORT
}

public class MembreEquipage
{
    private int vie;
    private int bouteille;
    private int type;

    public MembreEquipage(int type)
    {
        this.type = type;
        this.bouteille = 0;
        this.vie = 3;
    }

    public int Vie
    {
        get
        {
            return vie;
        }

        set
        {
            vie = value;
        }
    }

    public int Bouteille
    {
        get
        {
            return bouteille;
        }

        set
        {
            bouteille = value;
        }
    }

    public int Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }
}

public class Navire
{
    private ArrayList equipage;
    private string nom;
    private int or;
    private int nourriture;
    private int qe;

    public Navire(string nom)
    {
        this.nom = nom;
        this.or = 500;
        this.nourriture = 250;
        this.qe = 20;
        this.equipage.Add(new MembreEquipage(0));
        this.equipage.Add(new MembreEquipage(1));
        this.equipage.Add(new MembreEquipage(1));
        this.equipage.Add(new MembreEquipage(2));
        this.equipage.Add(new MembreEquipage(3));
    }

    public ArrayList Equipage
    {
        get
        {
            return equipage;
        }
    }

    public string Nom
    {
        get
        {
            return nom;
        }

        set
        {
            nom = value;
        }
    }

    public int Or
    {
        get
        {
            return or;
        }

        set
        {
            or = value;
        }
    }

    public int Nourriture
    {
        get
        {
            return nourriture;
        }

        set
        {
            nourriture = value;
        }
    }

    public int Qe
    {
        get
        {
            return qe;
        }

        set
        {
            qe = value;
        }
    }
}
