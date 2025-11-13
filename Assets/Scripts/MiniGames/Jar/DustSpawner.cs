using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DustSpawner : MonoBehaviour
{
    public float radius = 0.5f;  // 원통 반지름
    public float height = 1.0f;  // 원통 높이
    public int dustCount = 1000;

    private ParticleSystem ps;

    void Start()
    {
        Emit();
    }

    public void Emit()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Stop();
        ps.Clear();

        for (int i = 0; i < dustCount; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float y = Random.Range(-height / 2f, height / 2f);

            Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, y, Mathf.Sin(angle) * radius);
            //pos = transform.TransformPoint(transform.localPosition + pos); // 로컬 → 월드 변환

            var emitParams = new ParticleSystem.EmitParams();
            emitParams.position = transform.localPosition + pos;

            emitParams.velocity = Vector3.zero;

            Vector3 normal = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

            Quaternion rot = Quaternion.LookRotation(-normal, Vector3.up);
            emitParams.rotation3D = rot.eulerAngles;
            //emitParams.startSize = 0.02f;
            //emitParams.startColor = new Color(0.6f, 0.5f, 0.4f, 1f);
            ps.Emit(emitParams, 1);
        }

        ps.Play();
    }
}
