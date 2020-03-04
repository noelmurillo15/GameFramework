/*
 * StringCharacterColorizer - Fills Text Mesh Pro character vertices with random colors
 * Created by : Allan N. Murillo
 * Last Edited : 3/3/2020
 */

using TMPro;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace ANM.Framework.Utils
{
    public class StringCharacterColorizer : MonoBehaviour
    {
        private TMP_Text _tmpText;
        private TMP_TextInfo _textInfo;
        private Coroutine _coroutine;


        private void Start()
        {
            _tmpText = GetComponent<TMP_Text>();
            _tmpText.enableWordWrapping = true;
            _tmpText.ForceMeshUpdate();
            _textInfo = _tmpText.textInfo;
            _coroutine = StartCoroutine(RandomTextColorFill());
        }

        private void OnDestroy()
        {
            if (_coroutine == null) return;
            StopCoroutine(_coroutine);
        }

        private IEnumerator RandomTextColorFill()
        {
            var currentCharacter = 0;
            var characterCount = _tmpText.text.Length;
            
            var c0 = new Color32(0, 255, 0, 255);
            var c1 = new Color32(0, 0, 0, 127);
            
            while (true)
            {
                if (currentCharacter == characterCount)
                {
                    yield return new WaitForSeconds(0.20f);
                    break;
                }

                if (currentCharacter == 0)
                {
                    c0 = new Color32((byte)Random.Range(0,255), 
                        (byte)Random.Range(0,255), (byte)Random.Range(0,255), 255);
                }

                var matIndex = _textInfo.characterInfo[currentCharacter].materialReferenceIndex;
                var vertexIndex = _textInfo.characterInfo[currentCharacter].vertexIndex;
                var newVertexColors = _textInfo.meshInfo[matIndex].colors32;
                
                if (_textInfo.characterInfo[currentCharacter].isVisible)
                {
                    c0 = newVertexColors[vertexIndex + 0] = c0;
                    newVertexColors[vertexIndex + 1] = c1;
                    newVertexColors[vertexIndex + 2] = c0;
                    newVertexColors[vertexIndex + 3] = c1;
                    _tmpText.UpdateVertexData();
                }
                currentCharacter = (currentCharacter + 1) % characterCount;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
