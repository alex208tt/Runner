using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private CharacterController controller;
    private Vector3 dir;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private int coins;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private Text coinsText;


    private int lineToMove = 1;
    public float lineDistance = 4;
    private float maxSpeed = 110;
    

    void Start() {

        
        controller = GetComponent<CharacterController>();
        StartCoroutine(SpeedIncrease());
        Time.timeScale = 1;

    }

    // Update is called once per frame
    private void Update() {

        if (SwipeController.swipeRight) {
            if (lineToMove < 2)
                lineToMove++;
        }

        if (SwipeController.swipeLeft) {
            if (lineToMove > 0)
                lineToMove--;
        }

        if (SwipeController.swipeUp) {
            if (controller.isGrounded)
            Jump();
        }

         Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
            if (lineToMove == 0)
                targetPosition += Vector3.left * lineDistance;
            else if (lineToMove == 2)
                targetPosition += Vector3.right * lineDistance;

        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 modeDir = diff.normalized * 25 * Time.deltaTime;
        if (modeDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(modeDir);
        else
            controller.Move(diff);

        
    }

    private void Jump() {
        dir.y = jumpForce;
    }

    private void FixedUpdate()
    {
        dir.z = speed;
        dir.y += gravity * Time.fixedDeltaTime;
        controller.Move(dir * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        
        if(hit.gameObject.tag == "obstacle") {

            losePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.tag == "Coin") {

            coins++;
            coinsText.text = coins.ToString();
            Destroy(other.gameObject);
        }
    }

    private IEnumerator SpeedIncrease() {
        yield return new WaitForSeconds(4);
        if (speed < maxSpeed) {
            speed += 3;
            StartCoroutine(SpeedIncrease());
        }
    }
}
