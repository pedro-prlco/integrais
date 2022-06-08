using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Formula : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField limSup;
    [SerializeField]
    private TMP_InputField limInf;
    [SerializeField]
    private TMP_InputField numerator;
    [SerializeField]
    private TMP_InputField denominator;
    [SerializeField]
    private Button execute;
    [SerializeField]
    private TextMeshProUGUI response;    

    Vector2 responseStartPos;

    private void Start()
    {
        execute.onClick.AddListener(Calcular);    

        responseStartPos = response.transform.position;
        response.gameObject.SetActive(false);
    }

    private void Calcular()
    {
        string result = Calculadora.Executar(denominator.text, numerator.text, limSup.text, limInf.text);
        response.text = result;
        StartCoroutine(DisplayResponse(result));
    }

    IEnumerator DisplayResponse(string result)
    {
        response.DOFade(0, 0.01f);
        response.gameObject.SetActive(true);
        response.transform.position = responseStartPos + Vector2.right * 100;
        response.text = "R: " + result;
        yield return new WaitForEndOfFrame();
        response.DOFade(1, 1f);
        response.transform.DOMove(responseStartPos, 1f);
    }
}
