using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumBoxController : MonoBehaviour
{
    [Range(0, 9)]
    [SerializeField] private int numBoxNumber = 1;

    private Vector3 initPos;
    private Quaternion initRot;

    public int GetNumBoxNumber => numBoxNumber;

    // Start is called before the first frame update

    private void Start()
    {
        initPos = transform.position;
        initRot = transform.rotation;
    }

    private void Update()
    {
        Vector3 currentPos = this.transform.position;
        if (currentPos.y < 0.08f)
        {
            transform.position = initPos;
            transform.rotation = initRot;
        }
    }

    public void MoveInitalPlace()
    {
        gameObject.SetActive(false);
        transform.position = initPos;
        transform.rotation = initRot;
        gameObject.SetActive(true);
    }
}
