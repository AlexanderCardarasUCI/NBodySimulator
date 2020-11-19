using UnityEngine;

public class SystemPhysics : MonoBehaviour
{
  private GameObject[] planets;
  public double scale = 1E-35;
  public double timeStep = 100000000000.0;
  private double AU = 149600000000.0;

  private void Start() => this.updatePlanets();

  public void updatePlanets()
  {
    this.planets = new GameObject[this.transform.childCount];
    for (int index = 0; index < this.transform.childCount; ++index)
      this.planets[index] = this.transform.GetChild(index).gameObject;
  }

  public void clearForces()
  {
    for (int index = 0; index < this.planets.Length; ++index)
      this.planets[index].GetComponent<Body>().clearForce();
  }

  public void addForces()
  {
    for (int index1 = 0; index1 < this.planets.Length; ++index1)
    {
      Body component = this.planets[index1].GetComponent<Body>();
      for (int index2 = 0; index2 < this.planets.Length; ++index2)
      {
        if (index1 != index2)
          this.planets[index2].GetComponent<Body>().addForce(component);
      }
    }
  }

  public void applyForces()
  {
    for (int index = 0; index < this.planets.Length; ++index)
      this.planets[index].GetComponent<Body>().applyForce(this.timeStep);
  }

  private void updateVisuals()
  {
    for (int index = 0; index < this.planets.Length; ++index)
      this.planets[index].GetComponent<Body>().updateVisual(this.scale);
  }

  private void Update() => this.updateVisuals();

  private void FixedUpdate()
  {
    this.clearForces();
    this.addForces();
    this.applyForces();
  }
}
