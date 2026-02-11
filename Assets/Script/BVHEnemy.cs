using UnityEngine;
using System.Collections.Generic;

public class BVHEnemy : MonoBehaviour
{
    [Header("Configurações de Debug")]
    [Tooltip("Desenha os Gizmos para visualizar a hierarquia")]
    public bool desenharGizmos = true;
    public Color corRaiz = Color.green;
    public Color corPartes = Color.yellow;

    // Estrutura para armazenar as partes do corpo (Folhas do BVH)
    [System.Serializable]
    public class BVHPart
    {
        public Collider collider; // O MeshCollider da parte [cite: 18]
        public Bounds bounds;     // O Bounding Volume individual desta parte
    }

    // Lista de todas as partes do corpo (Nós folha)
    private List<BVHPart> partesDoCorpo = new List<BVHPart>();

    //[cite_start]// O Bounding Volume da Raiz (O "envelopador" total) [cite: 16]
    private Bounds boundsRaiz;

    void Start()
    {
        // 1. Inicializa a hierarquia buscando todos os coliders nos filhos
        Collider[] collidersFilhos = GetComponentsInChildren<Collider>();

        foreach (var col in collidersFilhos)
        {
            // Ignora o próprio colider da raiz se houver (focamos nas partes)
            if (col.gameObject == this.gameObject) continue;

            BVHPart novaParte = new BVHPart();
            novaParte.collider = col;
            // Inicializa o bounds base (será atualizado no Update)
            novaParte.bounds = col.bounds;

            partesDoCorpo.Add(novaParte);
        }

        AtualizarBVH();
    }

    void Update()
    {
        // Em um jogo real, você pode chamar isso apenas quando o inimigo anima/move
        // Para o trabalho, garante que o BVH acompanhe a animação frame a frame
        AtualizarBVH();
    }

    /// <summary>
    /// Recalcula os Bounds individuais e o Bounds da Raiz para encapsular tudo.
    //[cite_start]/// Requisito: Implementar BVH que agrupe partes de forma hierárquica [cite: 15]
                /// </summary>
    void AtualizarBVH()
    {
        if (partesDoCorpo.Count == 0) return;

        // Atualiza os bounds de cada parte (caso haja animação)
        for (int i = 0; i < partesDoCorpo.Count; i++)
        {
            partesDoCorpo[i].bounds = partesDoCorpo[i].collider.bounds;
        }

        // Reconstrói a Raiz começando pelo primeiro elemento
        boundsRaiz = partesDoCorpo[0].bounds;

        //[cite_start]// "Encapsulate" faz a caixa crescer para incluir as outras partes 
        for (int i = 1; i < partesDoCorpo.Count; i++)
        {
            boundsRaiz.Encapsulate(partesDoCorpo[i].bounds);
        }
    }

    /// <summary>
    /// Verifica se um raio atinge este inimigo usando a lógica de BVH.
    /// Retorna true se houver acerto na MALHA (Mesh Level).
    /// </summary>
    public bool ChecarAcertoBVH(Ray raio, out RaycastHit hitInfo)
    {
        hitInfo = new RaycastHit();

        //[cite_start]// [OTIMIZAÇÃO] Passo 1: Verifica interseção com o Bounds da Raiz [cite: 17]
        // Se o raio não tocar nem na caixa grande, nem perdemos tempo olhando as partes.
        if (!boundsRaiz.IntersectRay(raio))
        {
            return false;
        }

        // [PRECISÃO] Passo 2: Se passou da Raiz, verifica os volumes internos
        float menorDistancia = float.MaxValue;
        bool acertouAlgumaParte = false;

        foreach (var parte in partesDoCorpo)
        {
            // Verifica o Bounds da parte específica antes de testar a malha
            if (parte.bounds.IntersectRay(raio))
            {
                RaycastHit tempHit;
                //[cite_start]// Requisito: Verificação final a nível de mesh [cite: 18]
                // Usamos o Raycast do próprio collider da parte
                if (parte.collider.Raycast(raio, out tempHit, 1000f))
                {
                    // Queremos o acerto mais próximo (caso o tiro atravesse o braço e pegue o tronco)
                    if (tempHit.distance < menorDistancia)
                    {
                        menorDistancia = tempHit.distance;
                        hitInfo = tempHit;
                        acertouAlgumaParte = true;
                    }
                }
            }
        }

        return acertouAlgumaParte;
    }

    /// <summary>
    //[cite_start]/// Versão SEM otimização para comparação de performance [cite: 19]
                /// Testa direto contra todos os MeshColliders sem checar Bounds antes.
                /// </summary>
    public bool ChecarAcertoSemBVH(Ray raio, out RaycastHit hitInfo)
    {
        hitInfo = new RaycastHit();
        float menorDistancia = float.MaxValue;
        bool acertou = false;

        foreach (var parte in partesDoCorpo)
        {
            RaycastHit tempHit;
            // Custo alto: Testa geometria complexa diretamente
            if (parte.collider.Raycast(raio, out tempHit, 1000f))
            {
                if (tempHit.distance < menorDistancia)
                {
                    menorDistancia = tempHit.distance;
                    hitInfo = tempHit;
                    acertou = true;
                }
            }
        }
        return acertou;
    }

    //[cite_start]// Visualização exigida no trabalho 
    void OnDrawGizmos()
    {
        if (!desenharGizmos) return;

        // Desenha a Raiz (Verde)
        Gizmos.color = corRaiz;
        Gizmos.DrawWireCube(boundsRaiz.center, boundsRaiz.size);

        // Desenha as Partes Internas (Amarelo)
        Gizmos.color = corPartes;
        if (partesDoCorpo != null)
        {
            foreach (var parte in partesDoCorpo)
            {
                // Verifica se o bounds é válido antes de desenhar
                if (parte.bounds.extents != Vector3.zero)
                    Gizmos.DrawWireCube(parte.bounds.center, parte.bounds.size);
            }
        }
    }
}