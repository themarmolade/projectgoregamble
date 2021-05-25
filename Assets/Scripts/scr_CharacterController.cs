using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static scr_Models;

public class scr_CharacterController : MonoBehaviour
{
  private CharacterController characterController;
  private DefultInput defultInput;
  public Vector2 input_Movement;
  public Vector2 input_View;

  private Vector3 newCameraRotation;
  private Vector3 newCharacterRotation;

  [Header("References")]
  public Transform cameraHolder;

  [Header("Settings")]
  public PlayerSettingsModel playerSettings;
  public float viewClampYMin = -70;
  public float viewClampYMax = 80;

  private void Awake()
  {

      defultInput = new DefultInput();

      defultInput.Character.Movement.performed += e => input_Movement = e.ReadValue<Vector2>();
      defultInput.Character.View.performed += e => input_View = e.ReadValue<Vector2>();

      defultInput.Enable();

      newCameraRotation = cameraHolder.localRotation.eulerAngles;
      newCharacterRotation = transform.localRotation.eulerAngles;

      characterController = GetComponent<CharacterController>();
  }

  private void Update()
  {

      CalculateView();
      CalculateMovement();

  }

 
 private void CalculateView()
 {
     newCharacterRotation.y += playerSettings.ViewXSensitivity * (playerSettings.ViewXInverted ? -input_View.x : input_View.x) * Time.deltaTime;
     transform.rotation = Quaternion.Euler(newCharacterRotation);


     newCameraRotation.x += playerSettings.ViewYSensitivity * (playerSettings.ViewYInverted ? input_View.y : -input_View.y) * Time.deltaTime;
     newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

     cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
 }
 
  private void CalculateMovement()
  {
      var verticalSpeed = playerSettings.WalkingForwardSpeed * input_Movement.y * Time.deltaTime;
      var horizontalSpeed = playerSettings.WalkingStrafeSpeed * input_Movement.x * Time.deltaTime;

      var newMovementSpeed = new Vector3(horizontalSpeed, 0, verticalSpeed);
      newMovementSpeed = transform.TransformDirection(newMovementSpeed);
      
      characterController.Move(newMovementSpeed);
      
  }

}
