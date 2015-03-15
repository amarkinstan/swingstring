using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DistanceMeter : MonoBehaviour
{



    public GameObject Killzone;
    public GameObject Label;
    public GameObject Backgorund;
    public Camera mainCamera;
    public float disappearDistance;

    private GameObject Player;
    private RectTransform rectBounds;
    private Text textLabel;
    private float maxWidth;
    private float height;
    private float maxHeight;
    private float distanceFromKillzone;
    private float rot;

    // Use this for initialization
    void Start()
    {

        Player = GameObject.Find("Player");
        rectBounds = GetComponent<RectTransform>();
        textLabel = Label.GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {

        distanceFromKillzone = (Vector3.Distance(Player.transform.position, Killzone.transform.position) - Killzone.transform.localScale.x);

        if (distanceFromKillzone > disappearDistance)
        {
            Backgorund.SetActive(true);
            Label.SetActive(true);

            rot = RopeControl.AngleSigned(Player.transform.position - Killzone.transform.position, Vector3.right, Vector3.back);

            maxWidth = mainCamera.pixelWidth / 2f;

            maxHeight = (mainCamera.pixelHeight / 2f) - (textLabel.rectTransform.rect.height / 2f);

            height = Mathf.Clamp(Mathf.Cos(Mathf.Abs(rot + 90f) * Mathf.Deg2Rad) * maxWidth, -maxHeight, maxHeight);

            rectBounds.rotation = Quaternion.Euler(0f, 0f, rot);



            //on the left
            if (Player.transform.position.x > 0f)
            {
                rectBounds.anchoredPosition = new Vector2(-maxWidth + (textLabel.rectTransform.rect.width / 2f), height);

                textLabel.text = "•" + distanceFromKillzone.ToString("00");
            }
            //on thr right
            else
            {
                textLabel.text = distanceFromKillzone.ToString("00") + "•";
                rectBounds.anchoredPosition = new Vector2(maxWidth - (textLabel.rectTransform.rect.width / 2f), height);

                // dont want text upside down
                rectBounds.rotation = Quaternion.Euler(0f, 0f, rot + 180f);
            }


        }
        else
        {
            Backgorund.SetActive(false);
            Label.SetActive(false);
        }
    }
}
