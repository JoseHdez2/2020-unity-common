using UnityEngine;
using System.Collections;
using System.Linq;
using static UnityEngine.ParticleSystem;
using System;

public class FakeTunnelParticles : MonoBehaviour
{
    [Range(0, 1)]
    private float initialParticlesSpeed = 0f;

    private ParticleSystem[] particles;
    private void Awake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
        particles.ToList().ForEach(p => { MainModule main = p.main; main.simulationSpeed = initialParticlesSpeed; });
    }

    public void Resume()
    {
        StartCoroutine(ResumeInner());
    }

    public void Pause()
    {
        StartCoroutine(ResumePause());
    }

    private IEnumerator ResumeInner()
    {
        particles.ToList().ForEach(p => { MainModule main = p.main; main.simulationSpeed = 0.33f; });
        yield return new WaitForSeconds(0.33f);
        particles.ToList().ForEach(p => { MainModule main = p.main; main.simulationSpeed = 0.66f; });
        yield return new WaitForSeconds(0.33f);
        particles.ToList().ForEach(p => { MainModule main = p.main; main.simulationSpeed = 1; });
    }

    private IEnumerator ResumePause()
    {
        particles.ToList().ForEach(p => { MainModule main = p.main; main.simulationSpeed = 0.66f; });
        yield return new WaitForSeconds(0.33f);
        particles.ToList().ForEach(p => { MainModule main = p.main; main.simulationSpeed = 0.33f; });
        yield return new WaitForSeconds(0.33f);
        particles.ToList().ForEach(p => { MainModule main = p.main; main.simulationSpeed = 0; });
    }
}
