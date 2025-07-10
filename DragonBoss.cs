using UnityEngine;
using UnityEngine.AI;

public class DragonBoss : MonoBehaviour
{

    public Transform player;
    public GameObject breathEffectPrefab;
    public GameObject meteorPrefab;

    public float chaseRange = 15f;
    public float breathRange = 7f;
    public float breathWidth = 2f;
    public float breathDuration = 3f;
    public float meteorInterval = 10f;
    public float meteorSpawnHeight = 15f;

    private NavMeshAgent agent;
    private float meteorTimer;
    private bool isBreathing = false;
    private float breathTimer = 0f;
    private GameObject currentBreathEffect;
    private GameObject currentRangeIndicator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        meteorTimer = meteorInterval;
    }

    private void Update()
    {
        player = GameManager.singleton.player.transform;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 메테오 소환
        meteorTimer -= Time.deltaTime;
        if (meteorTimer <= 0f)
        {
            CastMeteor();
            meteorTimer = meteorInterval;
        }

        // 브레스 공격 중 처리
        if (isBreathing)
        {
            breathTimer -= Time.deltaTime;
            agent.isStopped = true;
            FacePlayer();

            //UpdateRangeIndicator(); // 브레스 중일 때 사거리 표시 방향 유지

            if (breathTimer <= 0f)
            {
                StopBreathAttack();
            }

            return;
        }

        // 브레스 사거리
        if (distance < breathRange)
        {
            StartBreathAttack();
        }
        // 추적
        else if (distance < chaseRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
        }
    }
    // 아 진짜 아아ㅏ아아아아아아ㅏ아앙마맘자ㅓㅁㅈ임이ㅣ이임이ㅜ미ㅜ이ㅜ미ㅜㅁ자ㅝㅇㅁㄴ지ㅏ워ㅣㅏㅁ워ㅏㅣ뮈ㅓㅏㅇ미ㅝㅇ미ㅜㄷ
    // 되는게 없네 씁
    // ㅁ랴ㅓㅐㄷㅁ댜ㅓㅐㅓㅑ나ㅓㄴ덛ㄹㄴㄷ뢔댜ㅕㅗㄹ고ㅕㅑㅓㅑㅐㄴㄷ럳ㄹ니닫ㄹㄷㄴㄷㄴ러ㅝㅏㄴㄹㄱ
    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void StartBreathAttack()
    {
        isBreathing = true;
        breathTimer = breathDuration;
        agent.isStopped = true;

        if (currentBreathEffect == null)
        {
            currentBreathEffect = Instantiate(breathEffectPrefab, transform.position + transform.forward * 1.5f + Vector3.up * 1f, transform.rotation, transform);
        }

        if (currentRangeIndicator == null)
        {
            currentRangeIndicator = CreateBreathLineIndicator();
        }
    }

    void StopBreathAttack()
    {
        isBreathing = false;
        agent.isStopped = false;

        if (currentBreathEffect != null)
        {
            Destroy(currentBreathEffect);
            currentBreathEffect = null;
        }

        if (currentRangeIndicator != null)
        {
            Destroy(currentRangeIndicator);
            currentRangeIndicator = null;
        }
    }

    void CastMeteor()
    {
        Vector3 spawnPosition = player.position + Vector3.up * meteorSpawnHeight;
        Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
    }

    GameObject CreateBreathLineIndicator()
    {
        GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Quad);
        indicator.name = "BreathLineIndicator";
        indicator.transform.SetParent(transform);
        indicator.transform.localPosition = Vector3.zero;
        indicator.transform.localRotation = Quaternion.identity;
        indicator.transform.localScale = new Vector3(breathWidth, breathRange, 1f);

        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = new Color(1f, 0f, 0f, 0.4f);
        indicator.GetComponent<MeshRenderer>().material = mat;

        Destroy(indicator.GetComponent<Collider>());
        
        indicator.transform.localPosition = new Vector3(0f, 0.01f, 0f);
        
        UpdateRangeIndicator(indicator);

        return indicator;
    }

    void UpdateRangeIndicator()
    {
        if (currentRangeIndicator != null)
        {
            UpdateRangeIndicator(currentRangeIndicator);
        }
    }

    void UpdateRangeIndicator(GameObject indicator)
    {
        indicator.transform.position = transform.position + transform.forward * (breathRange / 2f) + Vector3.up * 0.01f;
        indicator.transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }
}
