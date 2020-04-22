using SplineMesh;
using UnityEngine;

/// <summary>
/// Follows a spline position and rotation at a given speed.
/// Spline must be in parent's transform object
/// </summary>
public class SplineFollower : MonoBehaviour {

    public float speedInUnitsPerSecond = 1.0f;

    private Spline _splineRef;
    private Transform _splineParent;
    private float _posOnSpline = 0.0f;
    private bool _isOk = true;


    private void Start() {
        _splineRef = transform.parent.GetComponent<Spline>();

        if(!_splineRef) {
            Debug.LogWarning("Not child of a Spline. Can't follow...'");
            _isOk = false;
        }

        _splineParent = _splineRef.transform;
        if(!_splineParent)
            Debug.LogWarning("Spline has no parent. Cannot rotate properly");
    }

    /// <summary>
    /// Following spline progressively.
    /// </summary>
    void Update() {
        if(!_isOk) return;

        _posOnSpline += Time.deltaTime * (speedInUnitsPerSecond / _splineRef.Length);

        // Don't query for position outside curve bounds
        _posOnSpline = Mathf.Clamp(_posOnSpline, 0.0f, 1.0f);

        CurveSample sample = _splineRef.GetSampleAtDistance(_posOnSpline * _splineRef.Length);

        // Add Spline object position offset
        if(_splineParent)
            transform.position = (_splineParent.rotation * sample.location) + _splineRef.transform.position;
        else
            transform.position = sample.location + _splineRef.transform.position;
    }


    public float GetPercentageOfSplineProgress() { return _posOnSpline; }

}