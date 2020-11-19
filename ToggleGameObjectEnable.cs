using UnityEngine;

public class ToggleGameObjectEnable : MonoBehaviour
{
  [SerializeField]
  private GameObject _go;
  private bool _enabled = true;

  private void Update()
  {
    if (!Input.GetKeyDown(KeyCode.H))
      return;
    this._enabled = !this._enabled;
    this._go.SetActive(this._enabled);
  }
}
