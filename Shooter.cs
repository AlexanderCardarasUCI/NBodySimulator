
using UnityEngine;

public class Shooter : MonoBehaviour
{
  public GameObject lightAsteroid;
  public GameObject heavyAsteroid;
  public double velocity;
  private double AU = 149600000000.0;
  private GameObject system;

  private void Start() => this.system = GameObject.Find("Solar System");

  private Vector3d worldToSystem(Vector3 pos) => new Vector3d(pos.x, pos.y, pos.z) / this.system.GetComponent<SystemPhysics>().scale;

  private void CreateAsteroid(GameObject prefab)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    Body component = gameObject.GetComponent<Body>();
    Vector3d system = this.worldToSystem(this.transform.position + this.transform.forward);
    Vector3d vector3d = new Vector3d(this.transform.forward.x, this.transform.forward.y, this.transform.forward.z) * this.velocity;
    component.startingX = system.x;
    component.startingY = system.z;
    component.startingZ = system.y;
    component.startingVX = vector3d.x;
    component.startingVY = vector3d.z;
    component.startingVZ = vector3d.y;
    component.Init();
    gameObject.transform.parent = this.system.transform;
    this.system.GetComponent<SystemPhysics>().updatePlanets();
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
      this.CreateAsteroid(this.lightAsteroid);
    if (!Input.GetMouseButtonDown(1))
      return;
    this.CreateAsteroid(this.heavyAsteroid);
  }
}
