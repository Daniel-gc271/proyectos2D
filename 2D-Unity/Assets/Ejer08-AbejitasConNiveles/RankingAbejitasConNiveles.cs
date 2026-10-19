using UnityEngine;

public static class Ranking
{
    public const int TAMANO = 5;

    // Carga el ranking guardado en arrays de nombres, niveles y esquivadas
    public static void Cargar(string[] nombres, int[] niveles, int[] esquivadas)
    {
        for (int i = 0; i < TAMANO; i++)
        {
            nombres[i] = PlayerPrefs.GetString("TopNombre" + i, "---");
            niveles[i] = PlayerPrefs.GetInt("TopNivel" + i, 1);
            esquivadas[i] = PlayerPrefs.GetInt("TopEsquivadas" + i, 0);
        }
    }

    // Compara si una puntuación nueva es mejor que otra existente.
    // Devuelve true si (nuevoNivel, nuevasEsquivadas) supera a (viejoNivel, viejasEsquivadas).
    private static bool EsMejorPuntuacion(int nuevoNivel, int nuevasEsquivadas, int viejoNivel, int viejasEsquivadas)
    {
        if (nuevoNivel > viejoNivel) return true;
        if (nuevoNivel == viejoNivel && nuevasEsquivadas > viejasEsquivadas) return true;
        return false;
    }

    // Intenta meter una puntuación nueva en el top 5 basado en nivel y esquivadas.
    // Devuelve la posición en la que ha quedado (0 a 4) o -1 si no entra.
    public static int GuardarPuntuacion(string nombre, int nivelNuevo, int esquivadasNuevas)
    {
        string[] nombres = new string[TAMANO];
        int[] niveles = new int[TAMANO];
        int[] esquivadas = new int[TAMANO];
        Cargar(nombres, niveles, esquivadas);

        int posicion = -1;

        for (int i = 0; i < TAMANO; i++)
        {
            if (EsMejorPuntuacion(nivelNuevo, esquivadasNuevas, niveles[i], esquivadas[i]))
            {
                // Desplazamos hacia abajo los puestos peores
                for (int j = TAMANO - 1; j > i; j--)
                {
                    niveles[j] = niveles[j - 1];
                    esquivadas[j] = esquivadas[j - 1];
                    nombres[j] = nombres[j - 1];
                }
                // Insertamos la nueva puntuación en su hueco
                niveles[i] = nivelNuevo;
                esquivadas[i] = esquivadasNuevas;
                nombres[i] = nombre;
                posicion = i;
                break;
            }
        }

        // Guardamos el Top 5 actualizado en PlayerPrefs
        for (int i = 0; i < TAMANO; i++)
        {
            PlayerPrefs.SetString("TopNombre" + i, nombres[i]);
            PlayerPrefs.SetInt("TopNivel" + i, niveles[i]);
            PlayerPrefs.SetInt("TopEsquivadas" + i, esquivadas[i]);
        }

        // Guardamos los datos de la última partida jugada
        PlayerPrefs.SetString("UltimoNombre", nombre);
        PlayerPrefs.SetInt("UltimoNivel", nivelNuevo);
        PlayerPrefs.SetInt("UltimasEsquivadas", esquivadasNuevas);
        PlayerPrefs.SetInt("UltimaPosicion", posicion);

        PlayerPrefs.Save();
        return posicion;
    }

    // Borrar todos los récords almacenados
    public static void Borrar()
    {
        for (int i = 0; i < TAMANO; i++)
        {
            PlayerPrefs.DeleteKey("TopNombre" + i);
            PlayerPrefs.DeleteKey("TopNivel" + i);
            PlayerPrefs.DeleteKey("TopEsquivadas" + i);
        }
        PlayerPrefs.DeleteKey("UltimoNombre");
        PlayerPrefs.DeleteKey("UltimoNivel");
        PlayerPrefs.DeleteKey("UltimasEsquivadas");
        PlayerPrefs.DeleteKey("UltimaPosicion");
        PlayerPrefs.Save();
    }
}