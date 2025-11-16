using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DustCleaner : MonoBehaviour
{
    public Camera cam;
    public ParticleSystem dustSystem;
    public float cleanRadius = 0.1f;
    public GameObject smokePrefab;
    public float rotationSpeed = 100f;
    public Image whiteBackground;

    ParticleSystem.Particle[] particles;
    [SerializeField] List<DustZone> zones;

    void Start()
    {
        zones = new List<DustZone>(FindObjectsByType<DustZone>(FindObjectsSortMode.None));

        particles = new ParticleSystem.Particle[dustSystem.main.maxParticles];

        foreach (var p in particles)
        {
            foreach (var z in zones)
            {
                //Debug.Log(z.GetComponent<Collider>().bounds.ToString());
                if (z.GetComponent<Collider>().bounds.Contains(dustSystem.transform.parent.TransformPoint(p.position)))
                {
                    z.RegisterParticle();
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlaying || !MiniGameManager.instance.IsPlaying) return;

        if (Input.GetMouseButton(1))
        {
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            dustSystem.transform.parent.Rotate(Vector3.up, -rotX, Space.World);
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 adjustDir = (hit.normal + ray.direction * -0.5f).normalized;
                Vector3 adjustedHit = hit.point + adjustDir * 0.03f;
                if (ray.direction.y > 0)
                    adjustedHit.y += 20f * ray.direction.y; 
                else
                    adjustedHit.y += 2f * ray.direction.y;
                CleanAt(adjustedHit);
            }
        }
    }

    void CleanAt(Vector3 hitPos)
    {
        //dustSystem.Pause();
        //Debug.Log("CleanAt");
        int count = dustSystem.GetParticles(particles);
        int newCount = 0;

        float yRot = dustSystem.transform.parent.eulerAngles.y;
        Quaternion rot = Quaternion.Euler(0f, yRot, 0f);

        Vector3 centerWorld = transform.position;
        Vector3 centerLocal = dustSystem.transform.InverseTransformPoint(centerWorld);


        //Debug.Log(hitPos);
        for (int i = 0; i < count; i++)
        {
            var localPos = particles[i].position;
            // 2. 회전 중심 기준으로 이동
            Vector3 relative = localPos - centerLocal;

            // 3. Y축 회전 적용
            Vector3 rotatedRelative = rot * relative;

            // 4. 다시 중심 기준으로 복귀
            Vector3 rotatedLocal = centerLocal + rotatedRelative;

            // 5. 월드 좌표로 변환
            Vector3 pWorld = transform.TransformPoint(rotatedLocal);

            //Debug.Log("particle " + i.ToString() + " " + pWorld.ToString());
            if (Vector3.Distance(pWorld, hitPos) < cleanRadius)
            {
                if (smokePrefab != null)
                {
                    Instantiate(smokePrefab, hitPos, Quaternion.Euler(-90f, 0f, 0f), transform);
                }
                foreach (var z in zones)
                {
                    if (z.GetComponent<Collider>().bounds.Contains(pWorld))
                        z.CleanParticle();
                }

                /*
                particles[i].remainingLifetime = -1;
                var c = particles[i].startColor;
                c.a = 0;
                particles[i].startColor = c;
                */
                //Debug.Log("erase " + particles[i].position.ToString());
                continue;
            }
            particles[newCount++] = particles[i];
        }

        dustSystem.SetParticles(particles, newCount);
        dustSystem.Simulate(0f, true, false);
        //dustSystem.Play();
        CheckAllZonesCleaned();
    }

    void CheckAllZonesCleaned()
    {
        bool allClean = true;
        foreach (var z in zones)
            if (!z.IsClean) allClean = false;

        if (allClean)
        {
            Invoke(nameof(End), 10f);
        }
    }

    void End()
    {
        whiteBackground.DOFade(1f, 3f).OnComplete(() =>
        {
            MiniGameManager.instance.GameEnd();
        });
    }
}
