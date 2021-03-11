using Dweiss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserInput : MonoBehaviour
{
    public UserManager userMngr;
    public Move mover;
    public Transform transDir;
    public Locomotion locomotion;

    public float rotPower = 0.5f;
    public float movePower = 1;
    public float strifeSideFactor = 1;
    public float teleportDistance = 1;

    public float inputFactorOnBoost = 5;
    public float userLocomotionFactorOnBoost = 5;
    private float prevHorizontalValue = 0;

    public bool stepRotate = true;

    public void KeyboardRotate(float factor) {
        //var h = Input.GetKey(KeyCode.Q) ? -1 : 0;
        //h += Input.GetKey(KeyCode.E) ? 1 : 0;
        //var rotAmount = Input.GetAxis("Horizontal");

        var rotAmount = Input.GetAxis("Horizontal");
        if (stepRotate) {
            if (prevHorizontalValue == 0 && rotAmount != 0)
                mover.SimpleRotate(Mathf.Sign(rotAmount) * factor / Time.deltaTime);
            prevHorizontalValue = rotAmount;
        } else {
            mover.SimpleRotate(rotAmount * factor / Time.deltaTime);
        }
    }
    public void KeyboardMove(float factor) {

        var v = Input.GetAxis("Vertical");
        var rotLeft = Input.GetKey(KeyCode.Q) ? strifeSideFactor : 0;
        var rotRight = Input.GetKey(KeyCode.E) ? -strifeSideFactor : 0;
        var h = rotLeft + rotRight;

        mover.SimpleMove(v, h, factor);
    }

    public float sensetivity = 0.6f;
    private bool IsPressed(string inputButtonName, string inputAxisName = null, int joystickIndex = -1) {
        var axis = string.IsNullOrEmpty(inputAxisName) ? false : Mathf.Abs(Input.GetAxis(inputAxisName)) > sensetivity;
        var button = string.IsNullOrEmpty(inputButtonName) ? false : Input.GetButton(inputButtonName);
        var joystickId = joystickIndex < 0 ? false: Mathf.Abs(Input.GetAxis("joystick button " + joystickIndex)) > sensetivity;

        return axis || button || joystickId;
    }
    private bool IsPressedDown(string inputButtonName, string inputAxisName, int joystickIndex, bool wasPressed) {
        var button = string.IsNullOrEmpty(inputButtonName) ? false : Input.GetButtonDown(inputButtonName);
        var axisNow = string.IsNullOrEmpty(inputAxisName) ? false : Mathf.Abs(Input.GetAxis(inputAxisName)) > sensetivity;
        var joystickIdNow = Mathf.Abs(Input.GetAxis("joystick button " + joystickIndex)) > sensetivity;

        return button || (wasPressed == false && (joystickIdNow || axisNow));
    }


    private void TestJoystick() {

        var allKeys = System.Enum.GetValues(typeof(KeyCode));
        foreach(var k in allKeys) {
            if (Input.GetKey((KeyCode)k)) Debug.Log((KeyCode)k);
        }
        
    }

    public bool affectLocomotion;


    public void Restart() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    //private bool wasJumping, wasTeleporting;
    private void Update() {
       // TestJoystick();

        if (Input.GetKeyDown(KeyCode.Escape)) 
            Application.Quit();

        if (Input.GetButtonDown("Cancel"))
            Restart();

        if (Input.GetKeyDown(KeyCode.R) && mover.IsJumping == false && userMngr.IsTeleporting == false) {
            Restart(); 
            //mover.ResetTransform();
            //locomotion.ResetLocomotionPosToZero();
        }

        if (userMngr.IsTeleporting) return;

        var isInputFactor = IsPressed("Fire2", null, 12);//Input.GetButton("Fire2") || 
        var isMoveFactor = Input.GetButton("Fire3");

        KeyboardMove(isInputFactor ? inputFactorOnBoost * movePower: movePower);
        KeyboardRotate(isInputFactor ? inputFactorOnBoost * rotPower : rotPower);
        if(affectLocomotion) locomotion.SetXZFactor(isMoveFactor ? userLocomotionFactorOnBoost : 1);

        //wasJumping = IsPressedDown("Jump", null, 12, wasJumping);

        //Jump
        if (Input.GetButtonDown("Jump") && userMngr.IsTeleporting == false) mover.TryJump();
        
        //mouse rotate
        //mover.CursorRotate(Input.GetMouseButtonDown(1), Input.GetMouseButton(1), Input.mousePosition);

        var groundForward = Vector3.ProjectOnPlane(transDir.forward, Vector3.up).normalized;

        if (Input.GetButtonDown("Fire1") && mover.IsJumping == false) userMngr.TryTeleport(groundForward * (isInputFactor? 5:1) *teleportDistance + transform.position, null, null);

      
    }
}
