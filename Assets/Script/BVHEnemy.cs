using UnityEngine;
using System.Collections.Generic;

public class BVHEnemy : MonoBehaviour
{
    [Header("Configurações de Debug")]
    [Tooltip("Desenha os Gizmos para visualizar a hierarquia")]
    public bool desenharGizmos = true;
    public Color corRaiz = Color.green;
    public Color corPartes = Color.yellow;

    [System.Serializable]
    public class BVHPart
    {
        public Collider collider;
        public Bounds bounds;
    }

    private List<BVHPart> partesDoCorpo = new List<BVHPart>();

    private Bounds boundsRaiz;

    void Start()
    {
        Collider[] collidersFilhos = GetComponentsInChildren<Collider>();

        foreach (var col in collidersFilhos)
        {
            if (col.gameObject == this.gameObject) continue;

            BVHPart novaParte = new BVHPart();
            novaParte.collider = col;
            novaParte.bounds = col.bounds;

            partesDoCorpo.Add(novaParte);
        }

        AtualizarBVH();
    }

    void Update()
    {
        AtualizarBVH();
    }

    void AtualizarBVH()
    {
        if (partesDoCorpo.Count == 0) return;

        for (int i = 0; i < partesDoCorpo.Count; i++)
        {
            partesDoCorpo[i].bounds = partesDoCorpo[i].collider.bounds;
        }

        boundsRaiz = partesDoCorpo[0].bounds;

        for (int i = 1; i < partesDoCorpo.Count; i++)
        {
            boundsRaiz.Encapsulate(partesDoCorpo[i].bounds);
        }
    }

    public bool ChecarAcertoBVH(Ray raio, out RaycastHit hitInfo)
    {
        hitInfo = new RaycastHit();

        if (!boundsRaiz.IntersectRay(raio))
        {
            return false;
        }

        float menorDistancia = float.MaxValue;
        bool acertouAlgumaParte = false;

        foreach (var parte in partesDoCorpo)
        {
            if (parte.bounds.IntersectRay(raio))
            {
                RaycastHit tempHit;

                if (parte.collider.Raycast(raio, out tempHit, 1000f))
                {
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

    public bool ChecarAcertoSemBVH(Ray raio, out RaycastHit hitInfo)
    {
        hitInfo = new RaycastHit();
        float menorDistancia = float.MaxValue;
        bool acertou = false;

        foreach (var parte in partesDoCorpo)
        {
            RaycastHit tempHit;
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

    void OnDrawGizmos()
    {
        if (!desenharGizmos) return;

        Gizmos.color = corRaiz;
        Gizmos.DrawWireCube(boundsRaiz.center, boundsRaiz.size);

        Gizmos.color = corPartes;
        if (partesDoCorpo != null)
        {
            foreach (var parte in partesDoCorpo)
            {
                if (parte.bounds.extents != Vector3.zero)
                    Gizmos.DrawWireCube(parte.bounds.center, parte.bounds.size);
            }
        }
    }
}