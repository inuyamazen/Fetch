using System.Collections;
using UnityEngine;

public class KibaSpawner : MonoBehaviour {

    [SerializeField]
    private float _delay = 1f;
    [SerializeField]
    private GameObject _kibaPrefab;

    private void Start() {
        StartCoroutine(DelayedSpawn());
    }

    private IEnumerator DelayedSpawn() {
        yield return new WaitForSeconds(_delay);
        RaycastHit hitInfo;
        while (!Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 100.0f))
            yield return new WaitForSeconds(0.1f);
        Instantiate(_kibaPrefab, hitInfo.point + new Vector3(0, 0.1f, 0), Quaternion.identity);
    }
}
