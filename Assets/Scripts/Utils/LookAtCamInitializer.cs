using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(LookAtConstraint))]
public class LookAtCamInitializer : MonoBehaviour {

    private void Start() {
        LookAtConstraint c = GetComponent<LookAtConstraint>();
        ConstraintSource constraintSource = new ConstraintSource {sourceTransform = Camera.main.transform, weight = 1};
        c.AddSource(constraintSource);
    }

}