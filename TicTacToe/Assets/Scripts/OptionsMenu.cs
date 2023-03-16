using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using TMPro;
using UnityEngine.InputSystem;

public class OptionsMenu: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI boardSizeText;
    public static int BoardSize = 10;

    public void SetBoardSize(float size)
    {
        BoardSize = (int)size;
        boardSizeText.text = BoardSize.ToString();
        Debug.Log(size);
    }
}
