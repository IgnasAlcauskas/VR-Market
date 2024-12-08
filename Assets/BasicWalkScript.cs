using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BasicWalkScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public bool inLine = false;
    NPC_Line line = null;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!inLine)
        {
            transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(speed, 0, 0) * Time.deltaTime, transform.rotation);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("AAA");
        if (other.gameObject.CompareTag("DeleteNpc"))
        {
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("LineNPC") && !inLine && speed>0)
        {
            var lineSpot = other.transform.GetChild(0);
            line = other.GetComponent<NPC_Line>();
            if (!line.taken)
            {
                line.taken = true;



                transform.DOMoveX(lineSpot.position.x, speed).SetEase(Ease.OutSine).SetSpeedBased();
                transform.DOMoveZ(lineSpot.position.z, speed).SetEase(Ease.InSine).SetSpeedBased();
                transform.DOLocalRotate(new Vector3(0, 180, 0), 0.8f).SetEase(Ease.InCirc);
                inLine = true;
                gameObject.GetComponent<Animator>().SetTrigger("ToIdle");
            }
        }
    }
    public void GetOutOfLine()
    {
        Destroy(gameObject);
        line.taken = false;
        //will implement later
    }
}
