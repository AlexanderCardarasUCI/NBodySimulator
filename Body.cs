using UnityEngine;

public class Body : MonoBehaviour
{
  private double G = -6.673E-11;
  private double AU = 149600000000.0;
  private double DAY_SEC = 86400.0;
  public double mass;
  public double radius;
  public double startingX;
  public double startingY;
  public double startingZ;
  public double startingVX;
  public double startingVY;
  public double startingVZ;
  public Vector3d pos;
  public Vector3d vel;
  public Vector3d force;
  private GameObject visual;
  private ParticleSystem ps;

  private void Start()
  {
    if (!((Object) this.visual == (Object) null))
      return;
    this.Init();
  }

  public void Init()
  {
    this.ps = this.GetComponentInChildren<ParticleSystem>();
    this.ps.Stop();
    this.mass *= Mathd.Pow(10.0, 24.0);
    this.radius *= 1000.0;
    this.pos = new Vector3d(this.startingX * this.AU, this.startingY * this.AU, this.startingZ * this.AU);
    this.vel = new Vector3d(this.startingVX * this.AU / this.DAY_SEC, this.startingVY * this.AU / this.DAY_SEC, this.startingVZ * this.AU / this.DAY_SEC);
    this.visual = this.transform.GetChild(0).gameObject;
  }

  public void clearForce() => this.force = Vector3d.zero;

  public void addForce(Body other)
  {
    Vector3d vector3d = this.pos - other.pos;
    double magnitude = vector3d.magnitude;
    double num = this.G * (this.mass * other.mass) / (magnitude * magnitude);
    this.force += vector3d.normalized * num;
  }

  public void applyForce(double dt)
  {
    this.vel += this.force / this.mass * dt;
    this.pos += this.vel * dt;
  }

  public void updateVisual(double scale)
  {
    float x = (float) (this.pos.x * scale / this.AU);
    float z = (float) (this.pos.y * scale / this.AU);
    float y = (float) (this.pos.z * scale / this.AU);
    this.visual.transform.position = new Vector3(x, y, z);
    if (!this.ps.isStopped)
      return;
    this.ps.Play();
  }
}
