 using UnityEngine;
 using System.Collections;
 using UnityEngine.UI;
 
 public class AutoScrollRect: MonoBehaviour {
     public ScrollRect myScrollRect;
     public float scrollTime;
     public static WaitForSeconds P3 = new WaitForSeconds(0.3f); 
 
     void Start() {
         StartCoroutine(AutoScroll(myScrollRect, 1, 0, scrollTime));
     }
     IEnumerator AutoScroll(ScrollRect srollRect, float startPosition, float endPosition, float duration) {
         yield return P3;
         float t0 = 0.0f;
         while (t0 < 1.0f) {
             t0 += Time.deltaTime / duration;
             srollRect.verticalNormalizedPosition = Mathf.Lerp(startPosition, endPosition,  t0);
             yield return null;
         }
     }}