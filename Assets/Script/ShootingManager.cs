using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class ShootingManager : MonoBehaviour
{
    [Header("Configurações")]
    public Transform pontoDeDisparo;
    public int quantidadeTestePerformance = 10000;

    private BVHEnemy[] inimigosNaCena;

    void Start()
    {
        inimigosNaCena = FindObjectsOfType<BVHEnemy>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray raio = Camera.main.ScreenPointToRay(Input.mousePosition);
            AtirarComBVH(raio);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray raio = Camera.main.ScreenPointToRay(Input.mousePosition);
            AtirarSemBVH(raio);
        }

        if (Input.GetMouseButtonDown(2))
        {
            Ray raio = Camera.main.ScreenPointToRay(Input.mousePosition);
            CompararDisparoUnico(raio);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            RealizarTesteDePerformance();
        }
    }

    void AtirarComBVH(Ray raio)
    {
        RaycastHit hit;
        bool acertou = false;
        Stopwatch sw = Stopwatch.StartNew();

        foreach (var inimigo in inimigosNaCena)
        {
            if (inimigo.ChecarAcertoBVH(raio, out hit))
            {
                acertou = true;
                Debug.DrawLine(raio.origin, hit.point, Color.green, 2f);
                Debug.Log($"<color=green><b>[BVH]</b> ACERTOU: {hit.collider.name}</color> | Custo: <b>{sw.ElapsedTicks}</b> ticks");
                break;
            }
        }
        sw.Stop();

        if (!acertou)
            Debug.Log($"<color=green>[BVH]</color> ERROU (Otimizado) | Custo: <b>{sw.ElapsedTicks}</b> ticks");
    }

    void AtirarSemBVH(Ray raio)
    {
        RaycastHit hit;
        bool acertou = false;
        Stopwatch sw = Stopwatch.StartNew();

        foreach (var inimigo in inimigosNaCena)
        {
            if (inimigo.ChecarAcertoSemBVH(raio, out hit))
            {
                acertou = true;
                Debug.DrawLine(raio.origin, hit.point, Color.red, 2f);
                Debug.Log($"<color=red><b>[SEM BVH]</b> ACERTOU: {hit.collider.name}</color> | Custo: <b>{sw.ElapsedTicks}</b> ticks");
                break;
            }
        }
        sw.Stop();

        if (!acertou)
            Debug.Log($"<color=red>[SEM BVH]</color> ERROU (Pesado) | Custo: <b>{sw.ElapsedTicks}</b> ticks (Desperdiçado)");
    }

    // --- O PULO DO GATO PARA O RELATÓRIO ---
    void CompararDisparoUnico(Ray raio)
    {
        // 1. Mede Sem BVH
        Stopwatch swSem = Stopwatch.StartNew();
        RaycastHit hitSem;
        foreach (var e in inimigosNaCena) { if (e.ChecarAcertoSemBVH(raio, out hitSem)) break; }
        swSem.Stop();

        // 2. Mede Com BVH
        Stopwatch swCom = Stopwatch.StartNew();
        RaycastHit hitCom;
        bool acertou = false;
        foreach (var e in inimigosNaCena) { if (e.ChecarAcertoBVH(raio, out hitCom)) { acertou = true; break; } }
        swCom.Stop();

        long ticksSem = swSem.ElapsedTicks;
        long ticksCom = swCom.ElapsedTicks;

        // Evita divisão por zero
        if (ticksCom == 0) ticksCom = 1;

        float vezesMaisRapido = (float)ticksSem / (float)ticksCom;
        string cor = vezesMaisRapido > 1 ? "cyan" : "orange";

        string mensagem = $"<b><color={cor}>=== RELATÓRIO DO TIRO ===</color></b>\n";
        mensagem += $"Resultado: {(acertou ? "ACERTOU ALVO" : "ERROU (Céu)")}\n";
        mensagem += $"Sem BVH: {ticksSem} ticks\n";
        mensagem += $"Com BVH: {ticksCom} ticks\n";
        mensagem += $"<b><size=14><color={cor}>CONCLUSÃO: O BVH foi {vezesMaisRapido:F1}x mais rápido!</color></size></b>";

        Debug.Log(mensagem);
    }

    void RealizarTesteDePerformance()
    {
        Stopwatch sw = new Stopwatch();

        // --- COM BVH ---
        sw.Start();
        for (int i = 0; i < quantidadeTestePerformance; i++)
        {
            Vector3 dir = (pontoDeDisparo.forward + Random.insideUnitSphere * 0.2f).normalized;
            Ray r = new Ray(pontoDeDisparo.position, dir);
            foreach (var e in inimigosNaCena) { RaycastHit h; if (e.ChecarAcertoBVH(r, out h)) break; }
        }
        sw.Stop();
        long tempoBVH = sw.ElapsedMilliseconds;

        // --- SEM BVH ---
        sw.Reset();
        sw.Start();
        for (int i = 0; i < quantidadeTestePerformance; i++)
        {
            Vector3 dir = (pontoDeDisparo.forward + Random.insideUnitSphere * 0.2f).normalized;
            Ray r = new Ray(pontoDeDisparo.position, dir);
            foreach (var e in inimigosNaCena) { RaycastHit h; if (e.ChecarAcertoSemBVH(r, out h)) break; }
        }
        sw.Stop();
        long tempoSem = sw.ElapsedMilliseconds;

        UnityEngine.Debug.Log("Teste de estresse em massa (T) finalizado...");
        UnityEngine.Debug.Log($"RESULTADO ({quantidadeTestePerformance} tiros):\nBVH: {tempoBVH}ms\nSem BVH: {tempoSem}ms");
    }
}