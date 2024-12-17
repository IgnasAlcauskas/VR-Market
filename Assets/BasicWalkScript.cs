using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BasicWalkScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public bool inLine = false;
    public bool canEnterLine = true;
    NPC_Line line = null;
    private Vector3 exitPosition;
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
    private IEnumerator EnterLineCoroutine()
	{
        yield return new WaitForSeconds(0.8f);
        gameObject.GetComponent<Animator>().SetTrigger("ToIdle");
        yield return null;
    }
    private IEnumerator ExitLineCoroutine()
	{
        canEnterLine = false;
        transform.DOLocalRotate(new Vector3(0, -180, 0), 0.4f);
        yield return new WaitForSeconds(0.4f);
        gameObject.GetComponent<Animator>().SetTrigger("ToWalk");
        transform.DOMoveX(exitPosition.x, speed).SetEase(Ease.OutSine).SetSpeedBased();
        transform.DOMoveZ(exitPosition.z, speed).SetEase(Ease.InSine).SetSpeedBased();
        transform.DOLocalRotate(new Vector3(0, 90, 0), 0.8f).SetEase(Ease.InCirc);
        yield return new WaitForSeconds(0.6f);
        inLine = false;
        line.taken = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeleteNpc"))
        {
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("LineNPC") && !inLine && speed>0 && canEnterLine)
        {
            var lineSpot = other.transform.GetChild(0);
            line = other.GetComponent<NPC_Line>();
            if (!line.taken)
            {

                line.taken = true;

                exitPosition = transform.position;
                exitPosition.x += 2f;

                transform.DOMoveX(lineSpot.position.x, speed).SetEase(Ease.OutSine).SetSpeedBased();
                transform.DOMoveZ(lineSpot.position.z, speed).SetEase(Ease.InSine).SetSpeedBased();
                transform.DOLocalRotate(new Vector3(0, 180, 0), 0.8f).SetEase(Ease.InCirc);
                inLine = true;
                StartCoroutine(EnterLineCoroutine());
            }
        }
    }
    public void GetOutOfLine()
    {
        //Destroy(gameObject);
        //will implement later
        StartCoroutine(ExitLineCoroutine());
    }
}
