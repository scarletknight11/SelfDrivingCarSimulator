using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionCounter : MonoBehaviour {

    public Text CollisionCount;
    public Text PedestriansCount;
    public Text countText;
    public Text pedestriansText;
    public int countcollision;
    public int countpedestrianscollision;

    void Start()
    {
        countcollision = 0;
        countpedestrianscollision = 0;
        countText.text = "";
        pedestriansText.text = "";
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Side" || collision.gameObject.tag == "othercar")
        {
            CollisionCountText();
        }
        if (collision.gameObject.tag == "Pedestrians")
        {
            PedestrianCountText();
        }

    }

    void CollisionCountText()
    {
        countcollision = countcollision + 1;
        countText.text = countcollision.ToString();
    }

    void PedestrianCountText()
    {
        countpedestrianscollision = countpedestrianscollision + 1;
        pedestriansText.text = countpedestrianscollision.ToString();
    }


}
