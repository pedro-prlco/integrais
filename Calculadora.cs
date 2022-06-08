// Lógica realizada por Lucas DePaula.
// Porte para Android realizado por Pedro Pereira 

using System.Text.RegularExpressions;
using UnityEngine;

public static class Calculadora
{
    // Confere se a string possui parenteses ou se precisa fatorar
    private static bool isParentesis(string s) {
        for (int i = 0; i < s.Length; i++) {
            if (s[i] == '(') {
                return true;
            }
        }
        return false;
    }

    /**
     * Método para fatorar a string utilizando Bhaskara
     * 
     * @param s : expressão a ser fatorada
     * @return string fatorada e em parenteses
     */
    private static string fatorar(string s) {
        s = s.Trim();
        int[] array = { 1, 0, 0 };
        double x1, x2;
        string tmp;
        int n = 0, q = 0;
        
        string[] constants = s.Split(new char[] { '+', '-' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach(string consta in constants)
        {
            Debug.Log("constants:" + consta);
        }

        for(int i = 0; i < constants.Length; i++)
        {
            if(constants[i].Contains("^"))
            {
                array[n] = 1;
            }
            else
            {
                array[n] = int.Parse(constants[i].Replace("x", ""));
            }
            n++;

        }

        // Calculo do delta
        n = (array[1] * array[1]) + (-4 * (array[0] * array[2]));

        if (n >= 0) {
            x1 = (double) ((-(array[1]) + Mathf.Sqrt(n)) / 2 * array[0]);
            x2 = (double) ((-(array[1]) - Mathf.Sqrt(n)) / 2 * array[0]);

            Debug.Log("x1: "  + x1 + " x2: " + x2);

            if (x1 < 0) {
                tmp = "(x" + (int) x1 + ")";
            } else {
                tmp = "(x+" + (int) (x1) + ")";
            }
            if (x2 < 0) {
                s = tmp + "(x" + (int) x2 + ")";
            } else {
                s = tmp + "(x+" + (int) (x2) + ")";
            }
        } else {
            Debug.Log("ERRO (DENOMINADOR): Delta não possui raiz!");
        }
        return s;
    }

    /**
     * Método para calcular números de uma string
     * 
     * @param s : string com os números
     * @return : resultado do calculo
     */
    private static int calcular(string s) {
        int n = 0;

        Regex reg = new Regex(@"-?\d+");

        var matches = reg.Matches(s);
        
        for(int i = 0; i < matches.Count; i++)
        {
            if(i + 1 < matches.Count)
            {
                Debug.Log(matches[i].ToString() + " + " + matches[i+1].ToString());
                n += int.Parse(matches[i].ToString()) + int.Parse(matches[i+1].ToString());
            }
            // else
            // {
            //     Debug.Log("n+=" + matches[i].ToString());
            //     n += int.Parse(matches[i].ToString());
            // }
        }

        return n;

        // s = s.Substring(0, s.Length - 1);
        // for (int i = s.Length - 1; i >= 0; i--) {
        //     if (s[i] == '+') {
        //         n = int.Parse(s.Substring(1, i)) + int.Parse(s.Substring(i));
        //         break;
        //     } else if (s[i] == '-') {
        //         n = int.Parse(s.Substring(1, i)) - int.Parse(s.Substring(i + 1));
        //         break;
        //     }
        // }
    }

    public static string Executar(string denominador, string numerador, string limSup, string limInferior) {
        int a = 0, b = 0, c = 0, d = 0, n = 0, m = 0, q = 1;
        int[] array = { 0, 0, 0 };
        string tmp = "", numA = "", numB = "", numC = "";
        string[] partes = { "", "", "" };

        numerador = numerador.Trim();
        numerador = numerador.Replace(" ", "");
        numerador = numerador.Replace("X", "x");

        denominador = denominador.Trim();
        denominador = denominador.Replace(" ", "");
        denominador = denominador.Replace("X", "x");
        
        if(int.Parse(limSup) < int.Parse(limInferior)){
            return "limite superior menor que inferior.";
        }

        if (!isParentesis(denominador)) {
            denominador = fatorar(denominador);
        }

        Debug.Log("Numerador: " + numerador + "\nDenominador: " + denominador);

        // Extrai as constantes do numerador
        for (int i = 0; i < numerador.Length; i++) {
            if (i != 0 && numerador[i] == 'x') {
                q = int.Parse(numerador.Substring(0, i));
            }
            if (numerador[i] == '+' || numerador[i] == '-') {
                try
                {
                    a = int.Parse(numerador.Substring(i));
                }
                catch
                {
                    return "Bad format";
                }

                break;
            } else {
                a = 0;
            }
        }

        // Extrai as constantes do denominador
        tmp = denominador.Replace("x", "");
        Regex reg = new Regex(@"-?\d+");
        var matches = reg.Matches(tmp);

        int index = 0;
        foreach(var match in matches)
        {
            array[index] = int.Parse(match.ToString());
            index++;
        }

        b = array[0];
        c = array[1];
        d = array[2];

        //Separar os blocos de parenteses
        Regex reg2 = new Regex(@"\(.*?\)");
        Debug.Log(denominador);
        MatchCollection matches2 = reg2.Matches(denominador);
        n=0;
        foreach(var x in matches2)
        {
            partes[n] = x.ToString();
            Debug.Log(partes[n]);
            n++;
        }


        tmp = "";

        // Calcula o número referente a A
        m = a - (b * q);
        if (d == 0) { // Caso possua apenas 2 pares de parenteses
            n = c - b;
        } else {
            n = (c - b) * (d - b);
        }
        if (m != 0 || !(string.IsNullOrEmpty(partes[0]))) {
            if (n < 0) { // se for negativo inverte os valores
                n *= -1;
                m *= -1;
            }
            if (n == 0) {
                numA = m.ToString();
            } else if (m % n == 0) {
                numA = (m/ n).ToString();
            } else {
                numA = m + "/" + n;
            }
            if(m > 0){
                numA = '+' + numA;
            }
            tmp += numA + "*ln" + partes[0] + " ";
        }

        // Calcula o número referente a B
        m = a - (c * q);
        if (d == 0) {
            n = b - c;
        } else {
            n = (b - c) * (d - c);
        }
        if (m != 0 || !(string.IsNullOrEmpty(partes[1]))) {
            if (n < 0) {
                n *= -1;
                m *= -1;
            }

            if (n == 0) {
                numB = m.ToString();
            } else if (m % n == 0) {
                numB = (m/n).ToString();
            } else {
                numB = m + "/" + n;
            }
            if (m >= 0) {
                numB = "+ " + numB;
            }
            tmp += numB + "*ln" + partes[1] + " ";
        }

        // Calcula o número referente a C
        if (!(string.IsNullOrEmpty(partes[2]))) {
            m = a - (d * q);
            n = (b - d) * (c - d);

            if (m != 0) {
                if (n < 0) {
                    n *= -1;
                    m *= -1;
                }

                if (n == 0) {
                    numC = m.ToString();
                } else if (m % n == 0) {
                    numC = m.ToString();
                } else {
                    numC = m + "/" + n;
                }
                if (m >= 0) {
                    numC = "+ " + numC;
                }
                tmp += numC + "*ln" + partes[2] + " ";
            }
        }


        // Calculo do limite
        if (int.Parse(limSup) != 0 || int.Parse(limInferior) != 0) {
            tmp = "";
            
            Debug.Log(int.Parse(limSup));
            Debug.Log(int.Parse(limInferior));

            array[0] = calcular(partes[0].Replace("x", ""+limSup));
            array[1] = calcular(partes[1].Replace("x", ""+limSup));
            if (!string.IsNullOrEmpty(partes[2])) {
                array[2] = calcular(partes[2].Replace("x", ""+limSup));
            }
            if (array[0] != 0) {
                tmp += numA + "*ln(" + array[0] + ") ";
            }
            if (array[1] != 0) {
                tmp += numB + "*ln(" + array[1] + ") ";
            }
            if (array[2] != 0) {
                tmp += numC + "*ln(" + array[0] + ") ";
            }

            array[0] = calcular(partes[0].Replace("x", ""+limInferior));
            array[1] = calcular(partes[1].Replace("x", ""+limInferior));
            if (!string.IsNullOrEmpty(partes[2])) {
                array[2] = calcular(partes[2].Replace("x", ""+limInferior));
            }

            tmp += "- (";
            if (array[0] != 0) {
                tmp += numA + "*ln(" + array[0] + ") ";
            }
            if (array[1] != 0) {
                tmp += numB + "*ln(" + array[1] + ") ";
            }
            if (array[2] != 0) {
                tmp += numC + "*ln(" + array[0] + ") ";
            }

            tmp += ") ";
        }

        return tmp + "+ C";
    }


}
