using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trident : MonoBehaviour
{
    [SerializeField] private GameObject _trident;
    [SerializeField] private GameObject _defaultTrident;
    [SerializeField] private LayerMask _stopsTrident;
    [SerializeField] private LayerMask _enemiesLayerMask;
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private bool hasTrident = true;
    [SerializeField] private bool canPickUpTrident = false;
    [SerializeField] private GameObject _throwSound;
    [SerializeField] private GameObject _retrieveSound;
    [SerializeField] private GameObject _particleObject;
    [SerializeField] private float _tridentParticleDensity = 4.0f;
    [SerializeField] private GameObject _reticle;

    private Animator animator;
    private GameObject currentTrident;
    private Vector2 tridentCoordinate; 
    private Vector2 tridentNormal;
    private ParticleSystem tridentParticles;

    // Start is called before the first frame update
    void Start()
    {
        tridentParticles = _particleObject.GetComponent<ParticleSystem>();

        animator = GetComponent<Animator>();
        if(hasTrident) {
            animator.SetBool("hasTrident", true);
        } else {
            animator.SetBool("hasTrident", false);
        }
        
        // tridentParticles.emission.rateOverDistance = 0.0f;
        var tridentParticlesEmission = tridentParticles.emission;
        tridentParticlesEmission.rateOverDistance = 0.0f;
    }


    // Throw trident mechanic
    private void throwTrident(Vector3 _rayCastDirection) {
        RaycastHit2D tileHit = Physics2D.Raycast(transform.position, _rayCastDirection, Mathf.Infinity, _stopsTrident, -Mathf.Infinity, Mathf.Infinity);
            if (tileHit.collider != null) {
                float travelDistance = tileHit.distance;
                Vector3 tridentCoordinate = tileHit.point;
                Vector3 tridentNormal = tileHit.normal;

                // spawning trident
                currentTrident = Instantiate(_trident, tridentCoordinate, Quaternion.identity);
                currentTrident.transform.Translate(tridentNormal * 0.4f);

                animator.SetBool("hasTrident", false);
                
                if(tridentNormal.x < 0) {
                    currentTrident.transform.Rotate(0, 0, -90);
                }
                if(tridentNormal.x > 0) {
                    currentTrident.transform.Rotate(0, 0, 90);
                }
                if(tridentNormal.y > 0) {
                    currentTrident.transform.Rotate(0, 0, -180);
                }
                
                _throwSound.GetComponent<AudioSource>().Play();

                hasTrident = !hasTrident;

                RaycastHit2D[] enemiesHit = Physics2D.RaycastAll(transform.position, _rayCastDirection, travelDistance, _enemiesLayerMask, -Mathf.Infinity, Mathf.Infinity);

                foreach(RaycastHit2D enemy in enemiesHit) {
                    if (enemy.collider.GetComponent<EnemyBehavior>() != null) {
                        enemy.collider.GetComponent<EnemyBehavior>().DecrementHealth();
                    }
                }
            }
    }

    private void retrieveTrident() {
        Vector3 reverseRayCastDirection;
        if(_defaultTrident != null) {
            reverseRayCastDirection = (transform.position - _defaultTrident.transform.position).normalized;
            RaycastHit2D gaugeDistance = Physics2D.Raycast(_defaultTrident.transform.position, reverseRayCastDirection, Mathf.Infinity, _playerLayerMask, -Mathf.Infinity, Mathf.Infinity);
            float tridentDistance = gaugeDistance.distance;
            RaycastHit2D[] enemiesHit = Physics2D.RaycastAll(_defaultTrident.transform.position, reverseRayCastDirection, tridentDistance, _enemiesLayerMask, -Mathf.Infinity, Mathf.Infinity);
                
            // deal damage here
            foreach(RaycastHit2D enemy in enemiesHit) {
                if (enemy.collider.GetComponent<EnemyBehavior>() != null) {
                    enemy.collider.GetComponent<EnemyBehavior>().DecrementHealth();
                }
            }
            
            _retrieveSound.GetComponent<AudioSource>().Play();
            animator.SetBool("hasTrident", true);

            Destroy(_defaultTrident);
            hasTrident = !hasTrident;

        } else {
            reverseRayCastDirection = (transform.position - currentTrident.transform.position).normalized;
            RaycastHit2D gaugeDistance = Physics2D.Raycast(currentTrident.transform.position, reverseRayCastDirection, Mathf.Infinity, _playerLayerMask, -Mathf.Infinity, Mathf.Infinity);
            float tridentDistance = gaugeDistance.distance;
            RaycastHit2D[] enemiesHit = Physics2D.RaycastAll(currentTrident.transform.position, reverseRayCastDirection, tridentDistance, _enemiesLayerMask, -Mathf.Infinity, Mathf.Infinity);

            // deal damage here
            foreach(RaycastHit2D enemy in enemiesHit) {
                if (enemy.collider.GetComponent<EnemyBehavior>() != null) {
                    enemy.collider.GetComponent<EnemyBehavior>().DecrementHealth();
                }
            }

            _retrieveSound.GetComponent<AudioSource>().Play();
            animator.SetBool("hasTrident", true);

            Destroy(currentTrident);
            hasTrident = !hasTrident;
        }
    }



    // Update is called once per frame
    void Update()
    {
        Vector3 relativeMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rayCastDirection = (relativeMousePosition - transform.position).normalized; 
        var tridentParticlesEmission = tridentParticles.emission;

        if(hasTrident) {
            RaycastHit2D reticleCast = Physics2D.Raycast(transform.position, rayCastDirection, Mathf.Infinity, _stopsTrident, -Mathf.Infinity, Mathf.Infinity);
            if (reticleCast.collider != null) {
                // Handling reticle movement
                Vector3 reticleCoordinate = reticleCast.point;
                _reticle.transform.position = reticleCoordinate;
                // If left click, throw trident
                if(Input.GetMouseButtonDown(0)) {
                    throwTrident(rayCastDirection);
                }
            } else {
                // Move reticle out of the way if trident is not aiming at anything
                _reticle.transform.position = new Vector3 (-99999, -99999, 0);
            }
            // Handle particles when trident is not thrown
            tridentParticlesEmission.rateOverDistance = _tridentParticleDensity;
            tridentParticles.transform.position = transform.position;
        } else {
            if (Input.GetMouseButtonDown(1) && canPickUpTrident) {
                retrieveTrident();
            }
            // Handle particles when trident is thrown
            tridentParticlesEmission.rateOverDistance = _tridentParticleDensity;
            if(currentTrident != null) {
                tridentParticles.transform.position = currentTrident.transform.position;
            }
            // Move reticle out of the way if player does not have trident
            _reticle.transform.position = new Vector3 (-99999, -99999, 0);
        }

    }
}
