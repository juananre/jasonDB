using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class HttpTest : MonoBehaviour
{
    [SerializeField] int UserId = 1;
    [SerializeField] string URL = "https://my-json-server.typicode.com/juananre/jasonDB";
    [SerializeField] string ApiRickyMorty = "https://rickandmortyapi.com/api/character";

    [SerializeField] private user Myuser;
    [SerializeField] TextMeshProUGUI UserNameLable;
    [SerializeField] TextMeshProUGUI[] CharacterNameLabels;
    [SerializeField] TextMeshProUGUI[] CharacterSpicesLabels;
    [SerializeField] TextMeshProUGUI[] CharacterGenderLabels;
    [SerializeField] RawImage[] MyDeck;

    public void boton()
    {
        StartCoroutine(GetUser());
    }
    IEnumerator GetUser()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL + "/user/"+ UserId);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("cagaste" + www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            if (www != null && www.responseCode == 200)
            {
                Myuser = JsonUtility.FromJson<user>(www.downloadHandler.text);

                UserNameLable.text = Myuser.username;

                for (int i = 0; i < Myuser.deck.Length; i++)
                {
                    StartCoroutine(GetCharacter(i));
                }
              
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }
    IEnumerator GetCharacter(int index)
    {
        int characterid = Myuser.deck[index];
        UnityWebRequest www = UnityWebRequest.Get(ApiRickyMorty + "/"+ characterid);
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            if (www != null && www.responseCode == 200)
            {
                character Character = JsonUtility.FromJson<character>(www.downloadHandler.text);
                CharacterNameLabels[index].text = Character.name;
                CharacterSpicesLabels[index].text = Character.species;
                CharacterGenderLabels[index].text = Character.gender;
                StartCoroutine(DownloadImge(Character.image,MyDeck[index]));
            }
            else
            {
                Debug.Log(www.error);
            }
        }


    }
    IEnumerator DownloadImge(string Mediaurl,RawImage image)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(Mediaurl);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else if(!www.isHttpError)
        {
            image.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }


    public void Usurio1()
    {
        UserId = 1;
    }
    public void Usurio2()
    {
        UserId = 2;
    }
    public void Usurio3()
    {
        UserId = 3;
    }

    /*public void dropDownSwap(int index)
    {
        /*switch (index)
        {
            case 0: UserId = 1; break;
            case 1: UserId = 2; break;
            case 2: UserId = 3; break;
        }
        UserId = index;

    }*/
}

[System.Serializable]
public class Userlist
{
    public List<user> usuario;
}
[System.Serializable]
public class user
{
    public int id;
    public string username;
    public int[] deck;
}
public class character
{
    public int id;
    public string name;
    public string species;
    public string gender;
    public string image;
}



