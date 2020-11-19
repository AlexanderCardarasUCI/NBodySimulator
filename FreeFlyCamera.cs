using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof (Camera))]
public class FreeFlyCamera : MonoBehaviour
{
  [Space]
  [SerializeField]
  private bool _active = true;
  [Space]
  [SerializeField]
  private bool _enableRotation = true;
  [SerializeField]
  private float _mouseSense = 1.8f;
  [Space]
  [SerializeField]
  private bool _enableTranslation = true;
  [SerializeField]
  private float _translationSpeed = 55f;
  [Space]
  [SerializeField]
  private bool _enableMovement = true;
  [SerializeField]
  private float _movementSpeed = 10f;
  [SerializeField]
  private float _boostedSpeed = 50f;
  [Space]
  [SerializeField]
  private bool _enableSpeedAcceleration = true;
  [SerializeField]
  private float _speedAccelerationFactor = 1.5f;
  [Space]
  [SerializeField]
  private KeyCode _initPositonButton = KeyCode.R;
  private CursorLockMode _wantedMode;
  private float _currentIncrease = 1f;
  private float _currentIncreaseMem;
  private Vector3 _initPosition;
  private Vector3 _initRotation;

  private void Start()
  {
    this._initPosition = this.transform.position;
    this._initRotation = this.transform.eulerAngles;
  }

  private void OnEnable()
  {
    if (!this._active)
      return;
    this._wantedMode = CursorLockMode.Locked;
  }

  private void SetCursorState()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
      Application.Quit();
    Cursor.lockState = this._wantedMode;
    Cursor.visible = CursorLockMode.Locked != this._wantedMode;
  }

  private void CalculateCurrentIncrease(bool moving)
  {
    this._currentIncrease = Time.deltaTime;
    if (!this._enableSpeedAcceleration || this._enableSpeedAcceleration && !moving)
    {
      this._currentIncreaseMem = 0.0f;
    }
    else
    {
      this._currentIncreaseMem += Time.deltaTime * (this._speedAccelerationFactor - 1f);
      this._currentIncrease = Time.deltaTime + Mathf.Pow(this._currentIncreaseMem, 3f) * Time.deltaTime;
    }
  }

  private void Update()
  {
    if (!this._active)
      return;
    this.SetCursorState();
    if (Cursor.visible)
      return;
    if (this._enableTranslation)
      this.transform.Translate(Vector3.forward * Input.mouseScrollDelta.y * Time.deltaTime * this._translationSpeed);
    if (this._enableMovement)
    {
      Vector3 zero = Vector3.zero;
      float num = this._movementSpeed;
      if (Input.GetKey(KeyCode.LeftShift))
        num = this._boostedSpeed;
      if (Input.GetKey(KeyCode.W))
        zero += this.transform.forward;
      if (Input.GetKey(KeyCode.S))
        zero -= this.transform.forward;
      if (Input.GetKey(KeyCode.A))
        zero -= this.transform.right;
      if (Input.GetKey(KeyCode.D))
        zero += this.transform.right;
      this.CalculateCurrentIncrease(zero != Vector3.zero);
      this.transform.position += zero * num * this._currentIncrease;
    }
    if (this._enableRotation)
    {
      this.transform.rotation *= Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * this._mouseSense, Vector3.right);
      this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles.x, this.transform.eulerAngles.y + Input.GetAxis("Mouse X") * this._mouseSense, this.transform.eulerAngles.z);
    }
    if (!Input.GetKeyDown(this._initPositonButton))
      return;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
}
