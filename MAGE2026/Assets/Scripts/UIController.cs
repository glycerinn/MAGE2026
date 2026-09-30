using UnityEngine;
using DG.Tweening;
using TMPro;

public class UIController : MonoBehaviour
{
    public RectTransform[] buttons;
    public TMP_Text title;
    public float slideDuration = 0.6f;
    public float delayDuration = 0.2f;

    private void Start()
    {
        slideIn();
    }

    void Update()
    {
        textWave();
    }

    void slideIn()
    {
        for(int i = 0; i<buttons.Length; i++)
        {
            RectTransform btns = buttons[i];
            Vector2 target = btns.anchoredPosition;
            btns.anchoredPosition = new Vector2(-500f, target.y);

            btns.DOAnchorPos(target, slideDuration).SetEase(Ease.OutBack).SetDelay(i*delayDuration);
        }
    }

    void textWave()
    {
        title.ForceMeshUpdate();
        var textInfo = title.textInfo;

        for(int i = 0; i <textInfo.characterCount; ++i)
        {
            var charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
            {
                continue;
            }

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            for(int j = 0; j < 4; ++j)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time*2f + orig.x*0.01f) * 5f, 0);
            }
        }

        for(int i = 0 ; i<textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            title.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
