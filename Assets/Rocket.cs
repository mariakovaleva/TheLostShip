using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    // Increases the speed of rocket's rotation (the higher, the faster)
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip onDeath;
    [SerializeField] AudioClip onWin;


    Rigidbody rigidbody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // allow controls when alive
        if (state.Equals(State.Alive)) 
        { 
        RespondToThrustInput();
        RespondToRotateInput();
        } 
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;

        //can thrust while rotating
        rigidbody.AddRelativeForce(Vector3.up * thrustThisFrame);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotateInput()
    {
        rigidbody.freezeRotation = true; // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }

        rigidbody.freezeRotation = false; //resume physics control of rotation

    }

    void OnCollisionEnter(Collision collision)
    {
        // do nothing if dead
        if (!state.Equals(State.Alive)) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                // start next level
                print("Finish hit");
                OnWin();
                
                break;
            default:
                // kill player
                print("Dead");
                OnDeath();
                break;
        }

    }

    private void OnWin()
    {
        state = State.Transcending;

        Invoke("LoadNextLevel", 2.5f);
        
        audioSource.Stop();
        audioSource.PlayOneShot(onWin);
    }

    private void OnDeath()
    {
        state = State.Dying;

        Invoke("ReloadCurrentLevel", 1f);
        
        audioSource.Stop();
        audioSource.PlayOneShot(onDeath);
    }

    private void ReloadCurrentLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentScene);
    }

    private void LoadNextLevel()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        // TODO add check if next level exists
        SceneManager.LoadScene(nextScene);
    }
}
