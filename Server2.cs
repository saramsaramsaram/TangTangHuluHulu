using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Server2 : MonoBehaviour
{
    private InputField usernameInput;
    private InputField passwordInput;
    private Button loginButton;
    private Button registerButton;
    private Text statusText;

    private string baseUrl = "https://2c75-221-168-22-203.ngrok-free.app/api/auth/";

    [System.Serializable]
    public class AuthData
    {
        public string username;
        public string password;
    }

    void Start()
    {
        CreateUI();
        loginButton.onClick.AddListener(OnLoginClicked);
        registerButton.onClick.AddListener(OnRegisterClicked);
    }

    void CreateUI()
    {
        // Canvas
        GameObject canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // Panel
        GameObject panelGO = CreateUIElement("Panel", canvasGO.transform);
        RectTransform panelRT = panelGO.AddComponent<RectTransform>();
        panelGO.AddComponent<CanvasRenderer>();
        Image panelImage = panelGO.AddComponent<Image>();
        panelImage.color = new Color(1f, 1f, 1f, 0.1f);
        panelRT.sizeDelta = new Vector2(400, 300);
        panelRT.anchoredPosition = Vector2.zero;

        // Username InputField
        usernameInput = CreateInputField(panelGO.transform, "Username", new Vector2(0, 80));
        passwordInput = CreateInputField(panelGO.transform, "Password", new Vector2(0, 20));
        passwordInput.contentType = InputField.ContentType.Password;

        // Login Button
        loginButton = CreateButton(panelGO.transform, "Login", "로그인", new Vector2(0, -40));

        // Register Button
        registerButton = CreateButton(panelGO.transform, "Register", "회원가입", new Vector2(0, -100));

        // Status Text
        statusText = CreateText(panelGO.transform, "StatusText", "", new Vector2(0, -160));
        statusText.alignment = TextAnchor.MiddleCenter;
    }

    GameObject CreateUIElement(string name, Transform parent)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        return go;
    }

    InputField CreateInputField(Transform parent, string placeholder, Vector2 position)
    {
        GameObject go = CreateUIElement("InputField_" + placeholder, parent);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(300, 40);
        rt.anchoredPosition = position;

        go.AddComponent<CanvasRenderer>();
        Image image = go.AddComponent<Image>();
        image.color = Color.white;

        InputField input = go.AddComponent<InputField>();
        input.textComponent = CreateText(go.transform, "Text", "", Vector2.zero, 14);
        input.textComponent.alignment = TextAnchor.MiddleLeft;
        input.textComponent.rectTransform.sizeDelta = new Vector2(280, 40);

        Text placeholderText = CreateText(go.transform, "Placeholder", placeholder, Vector2.zero, 14);
        placeholderText.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        input.placeholder = placeholderText;

        return input;
    }

    Button CreateButton(Transform parent, string name, string label, Vector2 position)
    {
        GameObject go = CreateUIElement(name, parent);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(300, 40);
        rt.anchoredPosition = position;

        go.AddComponent<CanvasRenderer>();
        Image image = go.AddComponent<Image>();
        image.color = new Color(0.2f, 0.6f, 1f, 1f);

        Button button = go.AddComponent<Button>();
        button.targetGraphic = image;

        Text text = CreateText(go.transform, "Text", label, Vector2.zero, 16);
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;

        return button;
    }

    Text CreateText(Transform parent, string name, string content, Vector2 position, int fontSize = 12)
    {
        GameObject go = CreateUIElement(name, parent);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(300, 40);
        rt.anchoredPosition = position;

        go.AddComponent<CanvasRenderer>();
        Text text = go.AddComponent<Text>();
        text.text = content;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); // ✅ 최신 Unity 호환
        text.fontSize = fontSize;
        text.color = Color.black;

        return text;
    }

    void OnLoginClicked()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text;

        if (username == "" || password == "")
        {
            statusText.text = "아이디와 비밀번호를 입력하세요.";
            return;
        }

        StartCoroutine(LoginCoroutine(username, password));
    }

    void OnRegisterClicked()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text;

        if (username == "" || password == "")
        {
            statusText.text = "아이디와 비밀번호를 입력하세요.";
            return;
        }

        StartCoroutine(RegisterCoroutine(username, password));
    }

    IEnumerator RegisterCoroutine(string username, string password)
    {
        statusText.text = "회원가입 중...";
        AuthData data = new AuthData { username = username, password = password };
        string json = JsonUtility.ToJson(data);
        
        Debug.Log(json);
        Debug.Log(baseUrl+"register");

        UnityWebRequest request = new UnityWebRequest(baseUrl + "register", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            statusText.text = "회원가입 성공!";
            Debug.Log("회원가입 성공: " + request.downloadHandler.text);
        }
        else
        {
            statusText.text = "회원가입 실패: " + request.error;
            Debug.LogError("회원가입 실패: " + request.error);
        }
    }

    IEnumerator LoginCoroutine(string username, string password)
    {
        statusText.text = "로그인 중...";
        AuthData data = new AuthData { username = username, password = password };
        string json = JsonUtility.ToJson(data);

        Debug.Log(json);

        UnityWebRequest request = new UnityWebRequest(baseUrl + "login", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            statusText.text = "로그인 성공!";
            Debug.Log("로그인 성공: " + request.downloadHandler.text);
        }
        else
        {
            statusText.text = "로그인 실패: " + request.error;
            Debug.LogError("로그인 실패: " + request.error);
        }
    }
}
