using UnityEngine;

public class Player : MonoBehaviour
{
    private HLP.Dir lastInput = HLP.Dir.None;

    public void Move()
    {
        Vector3 moveDir = GetDirectionVector(lastInput);

        if(CanMove(moveDir))
        {
            transform.Translate(moveDir);
        };

        lastInput = HLP.Dir.None;
        GameManager.instance.UpdateUiWithIput("Last Input: None");
    }

    void Update()
    {
        SetDirection();
        ShowDirection();
    }

    private void SetDirection()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            lastInput = HLP.Dir.Up;
            GameManager.instance.UpdateUiWithIput("Last Input: W");   
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            lastInput = HLP.Dir.Left;
            GameManager.instance.UpdateUiWithIput("Last Input: A");
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            lastInput = HLP.Dir.Down;
            GameManager.instance.UpdateUiWithIput("Last Input: S");
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            lastInput = HLP.Dir.Right;
            GameManager.instance.UpdateUiWithIput("Last Input: D");
        }
    }

    private void ShowDirection()
    {
        // Teken een debug ray om de richting van de input te zien.
        Vector3 rayStart = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Debug.DrawRay(rayStart, GetDirectionVector(lastInput), Color.red);
    }

    private bool CanMove(Vector3 dir)
    {
        RaycastHit hit;
        // Als er geen hit is, dan proberen we buiten het grid te gaan.
        if(Physics.Raycast(transform.position, dir, out hit, 1f))
        {
            // We kunnen hier alleen heen als er geen wall staat.
            if(hit.collider.tag != "Wall")
            return true;
        }

        return false;
    }

    private Vector3 GetDirectionVector(HLP.Dir keyPress)
    {
        Vector3 chosenDir = Vector3.zero;

        switch (keyPress)
        {
            case HLP.Dir.Left:
                chosenDir = Vector3.left;
                break;
            case HLP.Dir.Right:
                chosenDir = Vector3.right;
                break;
            case HLP.Dir.Up:
                chosenDir = Vector3.forward;
                break;
            case HLP.Dir.Down:
                chosenDir = Vector3.back;
                break;
            default:
            break;
        }

        return chosenDir;
    }
    
    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.tag == "Enemy")
        {

            GameManager.instance.PlayerDeath();
            // Add a sound
            // Add a particle effect

        } else if (other.gameObject.tag == "Treasure")
        {

            GameManager.instance.CollectTreasure(other.gameObject);
            // Add a sound
            // Add a particle effect

        }
    }

}
