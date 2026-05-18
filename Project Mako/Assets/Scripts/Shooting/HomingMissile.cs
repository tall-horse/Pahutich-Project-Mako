using System.Linq;
using Mako.VehicleDevices;
using UnityEngine;

namespace Mako.Shooting
{
  public class HomingMissile : Projectile
  {
    [Header("MOVEMENT")]
    [SerializeField] private float rotationSpeed;
    [Header("REFERENCES")]
    private bool isFlying = false;
    [SerializeField] private Scanner scanner;
    [Header("PREDICTION")]
    [SerializeField] private float maxDistancePredict = 100;
    [SerializeField] private float minDistancePredict = 5;
    [SerializeField] private float maxTimePrediction = 5;
    private Vector3 standardPrediction, deviatedPrediction;
    [Header("DEVIATION")]
    [SerializeField] private float deviationAmount = 50;
    [SerializeField] private float deviationSpeed = 2;
    private Health.BasicHealth target;
    protected override void Awake()
    {
      base.Awake();
      scanner = GameObject.FindObjectOfType<Scanner>();
    }

    protected override void OnEnable()
    {
      base.OnEnable();
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      target = null;
    }

    public override void OnShot(Vector3 direction)
    {
      target = scanner.ScannedEnemy;
      if (target == null)
      {
        target = FindTarget();
      }
      isFlying = true;
    }

    private Health.BasicHealth FindTarget()
    {
      var potentialTargets = GameObject.FindObjectsOfType<Health.BasicHealth>().ToList().Where(
        h => h != GameObject.FindGameObjectWithTag("Player").GetComponent<Health.BasicHealth>());
      float minDist = Mathf.Infinity;
      Health.BasicHealth candidateTarget = null;
      foreach (var pTarget in potentialTargets)
      {
        float dist = Vector3.Distance(transform.position, pTarget.transform.position);
        if (dist < minDist)
        {
          minDist = dist;
          candidateTarget = pTarget;
        }
      }
      return candidateTarget;
    }

    private void FixedUpdate()
    {
      if (isFlying)
      {
        var leadTimePercentage = Mathf.InverseLerp(minDistancePredict, maxDistancePredict,
        Vector3.Distance(transform.position, target.transform.position));
        PredictMovement(leadTimePercentage);
        AddDeviation(leadTimePercentage);
        RotateRocket();
        projectileRigidbody.velocity = transform.forward * speed;
      }
    }

    private void RotateRocket()
    {
      var heading = deviatedPrediction - transform.position;

      var rotation = Quaternion.LookRotation(heading);
      projectileRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime));
    }

    private void AddDeviation(float leadTimePercentage)
    {
      var deviation = new Vector3(Mathf.Cos(Time.time * deviationSpeed), 0, 0);

      var predictionOffset = transform.TransformDirection(deviation) * deviationAmount * leadTimePercentage;

      deviatedPrediction = standardPrediction + predictionOffset;
    }

    private void PredictMovement(float leadTimePercentage)
    {
      var predictionTime = Mathf.Lerp(0, maxTimePrediction, leadTimePercentage);

      standardPrediction = target.GetComponent<Rigidbody>().position + target.GetComponent<Rigidbody>().velocity * predictionTime;
    }
  }
}
