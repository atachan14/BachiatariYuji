using UnityEngine;

public class SnakeBitePunish : Punish
{
    public PunishParams para;
    public SnakeBite snakeBite;

    private float lastBiteTime = -999f; // Å‰‚Í‚¾‚¢‚Ô‘O‚ÉŠš‚ñ‚¾‚±‚Æ‚É‚µ‚Ä‚¨‚­

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (Time.time - lastBiteTime >= para.cd)
        {
            snakeBite.Bite(collision.transform);
            lastBiteTime = Time.time;
        }
    }
}