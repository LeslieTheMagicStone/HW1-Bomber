using UnityEngine;

public partial class Upgrade : MonoBehaviour
{
    public UpgradeEffect effect = UpgradeEffect.SPEED_UP;
    public float rotateSpeed = 40f;
    public float fallSpeed = 4f, elevateSpeed = 15f;
    public ParticleSystem upgradeParticle;
    public AudioClip upgradeSound;
    [SerializeField] private Detector foot, buffer;
    private void Update()
    {
        if (!foot.detected && !buffer.detected) transform.Translate(0, -fallSpeed * Time.deltaTime, 0);
        else if (foot.detected) transform.Translate(0, elevateSpeed * Time.deltaTime, 0);

        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
