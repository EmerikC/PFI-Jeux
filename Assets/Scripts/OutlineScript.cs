using UnityEngine;
using System.Linq;

public class OutlineScript : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private float maxOutlineDistance = 5f;

    //Variable pour activer/d�sactiver l'outline
    public bool OutliningEnabled = true;

    private GameObject lastOutlinedObject;

    private void Update()
    {
        //Si l'outline est d�sactiv�, on ne fait rien
        if (!OutliningEnabled)
        {
            //Si un objet �tait outline, on le retire
            if (lastOutlinedObject != null)
            {
                RemoveOutline(lastOutlinedObject);
                lastOutlinedObject = null;
            }
            return; //Sortie de la fonction
        }

        //R�cup�ration des objets au centre de l'�cran
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Utilisation de QueryTriggerInteraction.Collide pour inclure les colliders "isTrigger"
        var hits = Physics.RaycastAll(ray, maxOutlineDistance, ~0, QueryTriggerInteraction.Collide);

        //On trouve l'objet le plus proche avec le tag "CanPickUp"
        var hitObject = hits
            .OrderBy(h => h.distance)
            .Select(h => h.transform)
            .FirstOrDefault(t => t != null && t.CompareTag("CanPickUp"))?.gameObject;

        //On applique ou retire l'outline selon l'objet d�tect�
        if (hitObject != lastOutlinedObject)
        {
            if (lastOutlinedObject != null)
            {
                RemoveOutline(lastOutlinedObject);
                lastOutlinedObject = null;
            }

            if (hitObject != null)
            {
                ApplyOutline(hitObject);
                lastOutlinedObject = hitObject;
            }
        }
        else if (hitObject == null && lastOutlinedObject != null) //Si aucun objet n'est d�tect� et qu'il y avait un objet outline, on retire l'outline
        {
            RemoveOutline(lastOutlinedObject);
            lastOutlinedObject = null;
        }
    }

    //Fonction pour appliquer l'outline � tous les renderers de l'objet et de ses enfants
    private void ApplyOutline(GameObject obj)
    {
        //R�cup�ration de tous les renderers de l'objet et de ses enfants
        var renderers = obj.GetComponentsInChildren<Renderer>();

        //On ajoute l'outline � chaque renderer
        foreach (var renderer in renderers)
        {
            //Utilisation de sharedMaterials pour �viter la cr�ation d'instances
            var materials = renderer.sharedMaterials;

            //V�rification si l'outline est d�j� appliqu�
            bool alreadyOutlined = false;
            foreach (var mat in materials)
            {
                if (mat == outlineMaterial)
                {
                    alreadyOutlined = true; //L'outline est d�j� appliqu�
                    break;
                }
            }
            if (alreadyOutlined)
                continue;

            //Ajout de l'outline
            var newMaterials = new Material[materials.Length + 1];
            materials.CopyTo(newMaterials, 0);
            newMaterials[newMaterials.Length - 1] = outlineMaterial;
            renderer.sharedMaterials = newMaterials;
        }
    }

    //Fonction pour retirer l'outline de tous les renderers de l'objet et de ses enfants
    private void RemoveOutline(GameObject obj)
    {
        //R�cup�ration de tous les renderers de l'objet et de ses enfants
        var renderers = obj.GetComponentsInChildren<Renderer>();

        //On retire l'outline de chaque renderer
        foreach (var renderer in renderers)
        {
            // Utilisation de sharedMaterials pour �viter la cr�ation d'instances
            var materials = renderer.sharedMaterials;

            //R�cup�ration de l'index de l'outline
            int index = System.Array.IndexOf(materials, outlineMaterial);
            if (index < 0)
                continue; //L'outline n'est pas appliqu�

            //Retrait de l'outline
            var newMaterials = new Material[materials.Length - 1];
            for (int i = 0, j = 0; i < materials.Length; i++)
            {
                if (i != index)
                    newMaterials[j++] = materials[i];
            }
            renderer.sharedMaterials = newMaterials;
        }
    }
}
